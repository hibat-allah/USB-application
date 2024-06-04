using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NewApp

{
    public class Docker
    {
        private static readonly HttpClient client = new HttpClient();
        private static bool isInfected = false;
        private static readonly object lockObj = new object();
        private static readonly string serverUrl = "http://172.22.64.1:3000/analyze";
        private static readonly string username = "hebi_"; // Set the client username here
        public static string path="";
        public static async Task<bool> HandleUsbDevice(string usbDriveLetter)
        {
            //string usbDriveLetter = "F:\\"; // Replace with the actual USB drive letter
            
            try
            {
                // Create Temp folder
                string tempFolderPath = Path.GetTempPath();
                Console.WriteLine($"tempfolder {tempFolderPath}");
                Directory.CreateDirectory(tempFolderPath);

                // Analyze the driverLetter
                await TraverseAndAnalyze(usbDriveLetter, tempFolderPath);

                // Delete the temporary folder
                Directory.Delete(tempFolderPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (isInfected)
            {
                // Block the device logic here
                Console.WriteLine("Device is infected and will be blocked."+path);
                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Device is clean.");
                //Console.ReadKey();
            }
            return isInfected;
        }

        static async Task TraverseAndAnalyze(string path,string tempFolderPath)
        {
            foreach (var dir in Directory.EnumerateDirectories(path))
            {
                if (Path.GetFileName(dir).Equals("System Volume Information", StringComparison.OrdinalIgnoreCase))
                    continue;

                await TraverseAndAnalyze(dir, tempFolderPath);

                if (isInfected) return;
            }
           

            foreach (var file in Directory.EnumerateFiles(path))
            {
                //decrypt the file in a temp file
                string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(path)+ "-"+Path.GetFileName(file));
                string key = Form1.privateKey;
                Console.WriteLine($" temp path {tempFilePath} for the file {file}");


                Form1.DecryptFile(file, tempFilePath, key);

                if (isInfected) return;
                await AnalyzeFile(tempFilePath);

                File.Delete(tempFilePath);
            }
        }


        
        static async Task AnalyzeFile(string filePath)
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
                            path= filePath;
                        }
                    }
                }
            }
        }
        


    }

}
