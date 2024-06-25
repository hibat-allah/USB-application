using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppService

{
    public class Docker
    {
        private static readonly HttpClient client = new HttpClient();
        private static bool isInfected = false;
        private static readonly object lockObj = new object();
        private static readonly string serverUrl = ServerConfiguration.URLDocker();//"http://192.168.1.130:3000/analyze";
        private static readonly string username = Environment.UserName; // Set the client username here
        public static string path="";
        
        static string logFilePath = @"C:\Users\user\Documents\Log2.txt"; // Update with an appropriate path
        public static string publicKey;// = "<RSAKeyValue><Modulus>pEomPb6ig2STFFAu5/wPTWt3K+MI0wXRcu5YK+XaiPCs+6Ne2k5PFuDb02XqP80IRr7K96ybaza2f04Wa2xg1sKYwUF7v3IqQCpfqGIqf1yzOAwB7q5kQlJM7qBREv2cQsPup0r+mhlSTlgZrONk1VbUzdDwS7W1SJRFKK74ZAn9cuclbyRzN6yNgjJgzTQu8K94gIMviZ0kQNmdrwKnacUK3drZquwN8Q8n+0auytthmLGMldnBSpgotx8yNn0iEETFvZsoUO+Ks6tHZ0T+PKMYxV2hweVcHPR2HIpwQ+0FOXoK2G9VnJQmQafc9hwK9xQuKMLd8ftR3YpHbvoMmQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public static string privateKey;// = "<RSAKeyValue><Modulus>pEomPb6ig2STFFAu5/wPTWt3K+MI0wXRcu5YK+XaiPCs+6Ne2k5PFuDb02XqP80IRr7K96ybaza2f04Wa2xg1sKYwUF7v3IqQCpfqGIqf1yzOAwB7q5kQlJM7qBREv2cQsPup0r+mhlSTlgZrONk1VbUzdDwS7W1SJRFKK74ZAn9cuclbyRzN6yNgjJgzTQu8K94gIMviZ0kQNmdrwKnacUK3drZquwN8Q8n+0auytthmLGMldnBSpgotx8yNn0iEETFvZsoUO+Ks6tHZ0T+PKMYxV2hweVcHPR2HIpwQ+0FOXoK2G9VnJQmQafc9hwK9xQuKMLd8ftR3YpHbvoMmQ==</Modulus><Exponent>AQAB</Exponent><P>yDNuFd4Ooiy3I+3VfOzEAZFjeAwDTn0txgZkfUhz8tLNv57sE/YYOCdbeVB798T5SzQGSRgERGWhf38FUr4RZnk0PYlJIOhXYCW20+VqUXVi76AChZKeTD+LW6oA6SextQ5XDxguuBR3HXCSEzMLbztu7GKFKF4RLzPLfl6XHy8=</P><Q>0hRowDUJCPUrmHOKU20inrWKOJpFSYXBhkoxFLb3JL+wfoOEOageirRxcZGDFj9+wBY1VsiGirspHNigCaUfX+T22CcLni8daeEMD66OtBSDHVUStqVFXSvzvJ/HXM4oaczeF897kZoTMJaq0/0sEAOyL4riGLZDg1O1w8CY3rc=</Q><DP>rfj7ZidUwhtnfuJzzh6V6eLk9HJEAVYIi/gMJU7r64zC3Q0GjAYkX8/bzt1hDRvvoylveN9U/fbvU7MW9iNTTaBwBDWkBePB4jyD1zmanXUL57490sNpqkriGGLwbaxf4j2269gQ6Uhnhn7HgcWu3xdfui2XF1A/UKyK3qLmYmM=</DP><DQ>dCHuKlrnLLmDqRouHKJXIg9PYqz9ooA7lEYNTds7UmhZZAbGIK1sfNb1Y4fCRJlDM2LTh68AmU6Pkq+6ALr0VimXv7QN+xsmknJjHYQsxSYVuTBfUPKaHsB+VAarygT5WEt1dwou0DWxaAnSU7BAHHY+mYOYK3sOZVnwQE+G8o8=</DQ><InverseQ>gn/AB8Ipw4YZi8rITmsUV7XERHOsqrP4E7iS/eQcKMzVJK09kE/CCKqRvD5BT5sz8+xxJLzvBgEmOjUVp6OKEasJkdVilKF/dCB4RqFdLOi7a/zzHYoxI+cxC0A/Si/WswqN5z4AudJCNDG6m9xK7jkkz+Z2kMQm+xPNK7xrvWo=</InverseQ><D>OyQWMBP4HKSP7bgPqqMqPwx4kevguVZW17DFPMg5qyI4oz1kX+HgxrObbrY5ZU5BtHXqSs5wBGBlwd4yY+lX0veqVadlDL3kR8T5OnxcIA0W1w47g9hzbwyBdUVw6g0xuzcdKkvs6zxCTMGaPIp/Oe1QHechGaAH6mC1pLeX4yrByDbO2qwfOvizt8UVCuzYdnOzpfOBqJHcMjnMmyFMPorWGIF6ePiqVrTJmdgXIFOSNhxtYbdRpYrBBX2uY6oROEPXBHBcYLy3r4bYqK+oj29OWWQ2aT/DO+6sH7qV0BUtqLHT/FBywvX4SM9IOjzTrxxpa8TG62OX6DjHGLn8IQ==</D></RSAKeyValue>\r\n";



        public static async Task<bool> HandleUsbDevice(string usbDriveLetter)
        {
            int retryCount = 0;
            const int maxRetries = 1;
            bool yes = true;

            (publicKey, privateKey) = cryption.GetCertificateKeysXml(username);


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
                    if (cryption.IsEncrypted(file))
                    {
                        //decrypt the file in a temp file
                        string key = privateKey;

                        //string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(path) + "-" + Path.GetFileName(file));
                        //Console.WriteLine($" temp path {tempFilePath} for the file {file}");
                        //Form1.DecryptFile(file, tempFilePath, key);
                        //string tempFilePath2 = Form1.RemoveCryptExtension(tempFilePath);


                        string tempFilePath = Path.Combine(tempFolderPath, file.Substring(Path.GetDirectoryName(file).Length + 0));
                        string tempDirectory = Path.GetDirectoryName(tempFilePath);
                        if (!Directory.Exists(tempDirectory))
                        {
                            Directory.CreateDirectory(tempDirectory);
                        }
                        // Decrypt the selected file to the temporary folder
                        cryption.DecryptFile(path, tempFilePath, privateKey);
                        string tempFilePath2 = cryption.RemoveCryptExtension(tempFilePath);


                        if (isInfected) return;
                        await AnalyzeFile(tempFilePath2);

                        //File.Delete(tempFilePath2);
                    }

                    else
                    { //file not crypted

                        // analyse the file
                        if (isInfected) return;
                        await AnalyzeFile(file);
                        if (isInfected) return;

                        // crypt the file
                        string key = publicKey;
                        cryption.EncryptFile(file, file, key);

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

    }

}
