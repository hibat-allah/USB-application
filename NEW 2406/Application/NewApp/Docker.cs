using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewApp

{
    public class Docker
    {
        private static readonly HttpClient client = new HttpClient();
        private static bool isInfected = false;
        private static readonly object lockObj = new object();
        private static readonly string serverUrl = ServerConfiguration.URLDocker();//"http://192.168.1.130:3000/analyze";
        private static readonly string username = Environment.UserName; // Set the client username here
        public static string path="";
        
        static string logFilePath = @"C:\Users\ASUS\Documents\Log2.txt"; // Update with an appropriate path

        
        public static async Task<bool> HandleUsbDevice(string usbDriveLetter)
        {
            int retryCount = 0;
            const int maxRetries = 1;
            bool yes = true;
            while (retryCount <= maxRetries && yes)
            {
                try
                {

                    Log("Application started at: " + DateTime.Now);

                    // Create Temp folder
                    string tempFolderPath = Path.GetTempPath();
                    //Console.Write($"tempfolder {tempFolderPath}");
                    Directory.CreateDirectory(tempFolderPath);
                    Log($"tempfolder : {tempFolderPath}");

                    // Analyze the driverLetter
                    await TraverseAndAnalyze(usbDriveLetter, tempFolderPath);
                    yes = false;
                    // Delete the temporary folder
                    Directory.Delete(tempFolderPath, true);
                }
                catch (HttpRequestException ex)
                {
                    Log($"Server connection error: {ex.Message}");
                    if (retryCount < maxRetries)
                    {
                        Log("Retrying connection to the server...");
                        retryCount++;
                    }
                    else
                    {
                        MessageBox.Show("Connection to the server is down. Please contact your administrator.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //BlockPeripheral();
                        return true; // Assuming you want to block the device if server is not reachable
                    }
                }
                catch (Exception ex)
                {
                    Log($"Error: {ex.Message}");
                }
            }

            if (isInfected)
            {
                // Block the device logic here
                Log("Device is infected and will be blocked."+path);
                //Console.ReadKey();
            }
            else
            {
                Log("Device is clean.");
                //Console.ReadKey();
            }
                
            return isInfected;
        }

        static async Task TraverseAndAnalyze(string path,string tempFolderPath)
        {
            try
            {
                Log($"Entering TraverseAndAnalyze with path: {path}");
                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    Log($"i'm in the start of analyzing function");
                    Log($"Analyzing directory: {dir}");
                    if (Path.GetFileName(dir).Equals("System Volume Information", StringComparison.OrdinalIgnoreCase))
                        continue;

                    await TraverseAndAnalyze(dir, tempFolderPath);

                    if (isInfected) return;
                }

                Log($"i'm in analyzing function");

                foreach (var file in Directory.EnumerateFiles(path))
                {
                    Log($"Analyzing file: {file}");
                    if (Form1.IsEncrypted(file))
                    {
                        //decrypt the file in a temp file
                        string key = Form1.privateKey;

                        string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(path) + "-" + Path.GetFileName(file));
                        Console.WriteLine($" temp path {tempFilePath} for the file {file}");
                        // Form1.DecryptFile(file, tempFilePath, key);
                        await DecryptFileAsync(file, tempFilePath, key);
                        string tempFilePath2 = Form1.RemoveCryptExtension(tempFilePath);
                        if (isInfected) return;
                        await AnalyzeFile(tempFilePath2);

                        File.Delete(tempFilePath2);
                    }

                    else
                    { //file not crypted

                        // analyse the file
                        if (isInfected) return;
                        await AnalyzeFile(file);
                        if (isInfected) return;

                       

                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Error during traversal and analysis: {ex.Message}");
                
            }
        }


        /*
        static async Task AnalyzeFile(string filePath)
        {

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {

                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(fileContent, "file", Path.GetFileName(filePath));
                        // Add headers only once
                        client.DefaultRequestHeaders.Clear(); // Clear existing headers
                        client.DefaultRequestHeaders.Add("username", username);

                        HttpResponseMessage response = await client.PostAsync(serverUrl, content);
                        response.EnsureSuccessStatusCode();

                        bool result = bool.Parse(await response.Content.ReadAsStringAsync());

                        if (result)
                        {
                            lock (lockObj)
                            {
                                isInfected = true;
                                path = filePath;
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Log($"Server connection error during file analysis: {ex.Message}");
                throw;
            }
            catch (IOException ex)
            {
                Log($"I/O error during file analysis: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Log($"Unexpected error during file analysis: {ex.Message}");
                throw;
            }
        }
        */
        static async Task AnalyzeFile(string filePath)
        {
            int retryCount = 0;
            const int maxRetries = 1;

            while (retryCount <= maxRetries)
            {
                try
                {
                    await AttemptToSendFile(filePath);
                    return;
                }
                catch (HttpRequestException ex)
                {
                    Log($"Server connection error during file analysis: {ex.Message}");
                    if (retryCount < maxRetries)
                    {
                        Log("Retrying file send...");
                        retryCount++;
                    }
                    else
                    {
                        MessageBox.Show("Connection to the server is down. Please contact your administrator.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //BlockPeripheral();
                        lock (lockObj)
                        {
                            isInfected = true;
                        }
                  //      throw;
                    }
                }
                catch (IOException ex)
                {
                    Log($"I/O error during file analysis: {ex.Message}");
                //    throw;
                }
                catch (Exception ex)
                {
                    Log($"Unexpected error during file analysis: {ex.Message}");
                    //throw;
                }
            }
        }

        static async Task AttemptToSendFile(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                using (var content = new MultipartFormDataContent())
                {
                    content.Add(fileContent, "file", Path.GetFileName(filePath));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("username", username);

                    HttpResponseMessage response = await client.PostAsync(serverUrl, content);
                    response.EnsureSuccessStatusCode();

                    bool result = bool.Parse(await response.Content.ReadAsStringAsync());

                    if (result)
                    {
                        lock (lockObj)
                        {
                            isInfected = true;
                            path = filePath;
                        }
                    }
                }
            }
        }
        static void Log(string message)
        {
            //lock (lockObj)
            //{
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(message);
                }
            //}
        }

        public static async Task DecryptFileAsync(string encryptedFilePath, string outputFilePath, string privateKey)
        {
            try
            {
                if (!Form1.IsEncrypted(encryptedFilePath))
                {
                    Log("File is not encrypted.");
                    return;
                }

                string outputFilePath2 = Form1.RemoveCryptExtension(outputFilePath);
                //Log("decryption : outputFile " + outputFilePath + " path2 " + outputFilePath2);

                using (FileStream inputFileStream = new FileStream(encryptedFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, true))
                {
                    byte[] encryptedAesKey = new byte[256];
                    await inputFileStream.ReadAsync(encryptedAesKey, 0, encryptedAesKey.Length);

                    byte[] aesIV = new byte[16];
                    await inputFileStream.ReadAsync(aesIV, 0, aesIV.Length);

                    byte[] aesKey;
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.FromXmlString(privateKey);
                        aesKey = rsa.Decrypt(encryptedAesKey, false);
                    }

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = aesKey;
                        aes.IV = aesIV;

                        using (FileStream outputFileStream = new FileStream(outputFilePath2, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = await cryptoStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await outputFileStream.WriteAsync(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }

                //Log("File decrypted successfully.");
            }
            catch (Exception ex)
            {
                Log($"Error decrypting file: {ex.Message}");
            }
        }

    }

}
