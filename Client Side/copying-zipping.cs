using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Ionic.Zip;  // DotNetZip

namespace ClientSideDocker
{
    
    class Program
    {
        static void Main(string[] args)
        {
            UsbDeviceListener usbListener = new UsbDeviceListener();
            usbListener.UsbDeviceConnected += async (driveLetter) =>
            {
                Console.WriteLine($"New USB Device Connected - Drive: {driveLetter}");
                await HandleUsbDevice(driveLetter);
            };
            usbListener.StartListening();

            Console.WriteLine("Listening for USB devices. Press any key to exit...");
            Console.ReadKey();
            usbListener.StopListening();
        }
        
        static async Task HandleUsbDevice(string usbDriveLetter)
        {
            //string usbDriveLetter = "F:"; // Replace with the actual USB drive letter
            string tempFolderPath = @"TempFolde";
            string zipFilePath = @"output.zip";


            try
            {
                Console.WriteLine($"letter: {usbDriveLetter}");
                // listing the content of the device : it's just optionnal to verify all the folders in there, and deleting the recycle bin
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(usbDriveLetter);

                    // Print directories
                    Console.WriteLine("Directories:");
                    foreach (var dir in dirInfo.GetDirectories())
                    {
                        Console.WriteLine(dir.FullName);


                    }

                    // Print files
                    Console.WriteLine("\nFiles:");
                    foreach (var file in dirInfo.GetFiles())
                    {
                        Console.WriteLine(file.FullName);
                    }

                    // deleting the recycle bin of the device if it exists
                    const string Path1 = @"E:\$RECYCLE.BIN";
                    DeleteDirectory(Path1);

                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Access denied: " + ex.Message);
                }



                // Copy the contents of the USB drive (excluding System Volume Information) to the temporary folder

                // Create a temporary folder to store the copied files
                Directory.CreateDirectory(tempFolderPath);
                await CopyContentsAsync(usbDriveLetter, tempFolderPath);



                // Zip the temporary folder

                Console.WriteLine("ziping....");
                ZipDirectory(tempFolderPath, zipFilePath);

                Console.WriteLine("done ziping....");



                // Send the zip file to the server
                await UploadZipFile(zipFilePath);



                Console.WriteLine("done uploading....");
                Console.WriteLine("File uploaded successfully.");
                Console.WriteLine("press any key to exit...");
                Console.ReadLine();


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Clean up: Delete the temporary folder and zip file
                Directory.Delete(tempFolderPath, true);
                File.Delete(zipFilePath);
            }

        }


        static void ZipDirectory(string sourceDir, string zipFilePath)
        {
            
            using (ZipFile zip = new ZipFile())
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed; // Adjust level as needed
                zip.AddDirectory(sourceDir, "");
                zip.Save(zipFilePath);
            }
        }
        
        static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
                Console.WriteLine("Folder deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Directory {path} does not exist.");
            }
        }

        static async Task CopyContentsAsync(string sourceDir, string destDir)
        {
            foreach (var dir in Directory.EnumerateDirectories(sourceDir))
            {
                Console.WriteLine($"copying this dir : {dir}");
                // Skip "System Volume Information" folder
                if (Path.GetFileName(dir).Equals("System Volume Information", StringComparison.OrdinalIgnoreCase))
                    continue;

                string newDir = Path.Combine(destDir, Path.GetFileName(dir));
                Directory.CreateDirectory(newDir);
                await CopyContentsAsync(dir, newDir);
            }

            foreach (var file in Directory.EnumerateFiles(sourceDir))
            {
                Console.WriteLine("copying files of the dir : "+sourceDir);
                string destFilePath = Path.Combine(destDir, Path.GetFileName(file));
                using (var sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await sourceStream.CopyToAsync(destStream);
                }
            }
            Console.WriteLine("done copying this dir: "+sourceDir);
        }

        static async Task UploadZipFile(string zipFilePath)
        {
            Console.WriteLine("uploading....");
            string serverUrl = "http://localhost:3000/upload"; // Your server's upload endpoint
            
            // need to fix the username 
            string username = "hiba";//Environment.UserName; // Get the domain username

            using (HttpClient client = new HttpClient())
            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                // Add sender ID to the request headers
                //client.DefaultRequestHeaders.Add("sender-id", senderId);

                // Add the username to the request headers
                client.DefaultRequestHeaders.Add("username", username);

                // Add the ZIP file to the request content
                byte[] fileBytes = File.ReadAllBytes(zipFilePath);
                ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/zip");
                content.Add(fileContent, "file", Path.GetFileName(zipFilePath));

                // Send the request
                HttpResponseMessage response = await client.PostAsync(serverUrl, content);
                Console.WriteLine("ddooone uploading....");
                string responseContent = await response.Content.ReadAsStringAsync();

                // need to fix the response handling 
                /*
                 if (result == true ){ block the usb device}
                else if the result == undefined also block the device or somthing like that
                if it's false means it doesn't containt a bad file so give him the access ( write and execute)
                 */
    
                // Output the server's response
                Console.WriteLine(responseContent);
            }
        }
    }
    
}
