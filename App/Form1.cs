using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using System.Security.Cryptography;
/*
namespace App
{
    /*
        public partial class Form1 : Form
        {
            public Form1()
            {
                InitializeComponent();
            }

            private void openToolStripMenuItem_Click(object sender, EventArgs e)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;
                        string fileContent = File.ReadAllText(filePath);
                        textBox_FileContent.Text = fileContent;
                        this.Text = $"File Editor - {Path.GetFileName(filePath)}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening file: {ex.Message}");
                    }
                }
            }

            private void saveToolStripMenuItem_Click(object sender, EventArgs e)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = saveFileDialog.FileName;
                        File.WriteAllText(filePath, textBox_FileContent.Text);
                        MessageBox.Show("File saved successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}");
                    }
                }
            }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    */

/* 2nd editor + openWith
    public partial class Form1 : Form
    {
        private string currentFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All Files|*.*",
                Title = "Open File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentFilePath = openFileDialog.FileName;
                    string fileContent = File.ReadAllText(currentFilePath);
                    textBox_FileContent.Text = fileContent;
                    this.Text = $"File Editor - {Path.GetFileName(currentFilePath)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}");
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "All Files|*.*",
                    Title = "Save File"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            try
            {
                File.WriteAllText(currentFilePath, textBox_FileContent.Text);
                MessageBox.Show("File saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}");
            }
        }

        private void trackBar_Zoom_Scroll(object sender, EventArgs e)
        {
            textBox_FileContent.Font = new Font(textBox_FileContent.Font.FontFamily, trackBar_Zoom.Value);
        }

        private void button_OpenWithDefault_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(currentFilePath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file with default application: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No file is selected to open.");
            }
        }
    }
*/

/* 3rd openwith alone
     public partial class Form1 : Form
     {
         public Form1()
         {
             InitializeComponent();
         }

         private void openFilesToolStripMenuItem_Click(object sender, EventArgs e)
         {
             OpenFileDialog openFileDialog = new OpenFileDialog
             {
                 Filter = "All Files|*.*",
                 Title = "Open Files",
                 Multiselect = true
             };

             if (openFileDialog.ShowDialog() == DialogResult.OK)
             {
                 listBox_Files.Items.AddRange(openFileDialog.FileNames);
             }
         }

         private void button_OpenWithDefault_Click(object sender, EventArgs e)
         {
             if (listBox_Files.SelectedItem != null)
             {
                 string selectedFile = listBox_Files.SelectedItem.ToString();
                 try
                 {
                     Process.Start(new ProcessStartInfo(selectedFile) { UseShellExecute = true });
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show($"Error opening file with default application: {ex.Message}");
                 }
             }
             else
             {
                 MessageBox.Show("No file is selected to open.");
             }
         }
     }
 */


/*
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void button_LoadFiles_Click(object sender, EventArgs e)
    {
        string folderPath = textBox_FolderPath.Text;

        if (Directory.Exists(folderPath))
        {
            listBox_Files.Items.Clear();
            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                listBox_Files.Items.Add(file);
            }
        }
        else
        {
            MessageBox.Show("The specified folder path does not exist.");
        }
    }

    private void button_OpenWithDefault_Click(object sender, EventArgs e)
    {
        if (listBox_Files.SelectedItem != null)
        {
            string selectedFile = listBox_Files.SelectedItem.ToString();
            try
            {
                Process.Start(new ProcessStartInfo(selectedFile) { UseShellExecute = true });
                // Remember the path for further processing (e.g., decryption)
                // Call your decryption function here, passing the selectedFile path
                // Example: DecryptFile(selectedFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file with default application: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("No file is selected to open.");
        }
    }
}
*/

/*
    public partial class Form1 : Form
    {

    private FileSystemWatcher fileSystemWatcher;

    public Form1()
    {
        InitializeComponent();

        // Subscribe to the Load event of the form
        this.Load += Form1_Load;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Initialize FileSystemWatcher
        fileSystemWatcher = new FileSystemWatcher
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            IncludeSubdirectories = true
        };
        fileSystemWatcher.Changed += OnChanged;
        fileSystemWatcher.Created += OnChanged;
        fileSystemWatcher.Deleted += OnChanged;
        fileSystemWatcher.Renamed += OnRenamed;
    }

    private void button_LoadFiles_Click(object sender, EventArgs e)
    {
        string folderPath = textBox_FolderPath.Text;

        if (Directory.Exists(folderPath))
        {
            listBox_Files.Items.Clear();
            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                listBox_Files.Items.Add(file);
            }

            // Check if fileSystemWatcher is initialized before setting its Path property
            if (fileSystemWatcher != null)
            {
                // Start monitoring the folder
                fileSystemWatcher.Path = folderPath;
                fileSystemWatcher.EnableRaisingEvents = true;
            }
            else
            {
                MessageBox.Show("FileSystemWatcher is not initialized.");
            }
        }
        else
        {
            MessageBox.Show("The specified folder path does not exist.");
        }
    }
    private void button_OpenWithDefault_Click(object sender, EventArgs e)
        {
            if (listBox_Files.SelectedItem != null)
            {
                string selectedFile = listBox_Files.SelectedItem.ToString();
                try
                {
                    // Decrypt the file before opening
                    DecryptFile(selectedFile);

                    Process.Start(new ProcessStartInfo(selectedFile) { UseShellExecute = true });

                    // Remember the path for further processing (e.g., re-encrypting after closing)
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file with default application: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No file is selected to open.");
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            // Encrypt the file when it is changed, created, or deleted
            EncryptFile(e.FullPath);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            // Encrypt the file when it is renamed
            EncryptFile(e.FullPath);
        }

        private void EncryptFile(string filePath)
        {
            // Add your encryption logic here
            MessageBox.Show($"Encrypting file: {filePath}");
        }

        private void DecryptFile(string filePath)
        {
            // Add your decryption logic here
            MessageBox.Show($"Decrypting file: {filePath}");
        }
    }
*/
/*
public partial class Form1 : Form
{
    private FileSystemWatcher fileSystemWatcher;
    private string selectedFilePath;
    private string tempFolderPath;

    public Form1()
    {
        InitializeComponent();

        // Subscribe to the Load event of the form
        this.Load += Form1_Load;
        this.FormClosing += Form1_FormClosing;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        // Initialize FileSystemWatcher
        fileSystemWatcher = new FileSystemWatcher
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            IncludeSubdirectories = true
        };
        fileSystemWatcher.Changed += OnChanged;
        fileSystemWatcher.Created += OnChanged;
        fileSystemWatcher.Deleted += OnChanged;
        fileSystemWatcher.Renamed += OnRenamed;
    }

    private void button_LoadFiles_Click(object sender, EventArgs e)
    {
        string folderPath = textBox_FolderPath.Text;

        if (Directory.Exists(folderPath))
        {
            listBox_Files.Items.Clear();
            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                listBox_Files.Items.Add(file);
            }

            // Start monitoring the original folder
            fileSystemWatcher.Path = folderPath;
            fileSystemWatcher.EnableRaisingEvents = true;
        }
        else
        {
            MessageBox.Show("The specified folder path does not exist.");
        }
    }

    private void button_OpenWithDefault_Click(object sender, EventArgs e)
    {
        if (listBox_Files.SelectedItem != null)
        {
            selectedFilePath = listBox_Files.SelectedItem.ToString();
            tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempFolderPath);

            string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(selectedFilePath));

            // Decrypt the selected file to the temporary folder
            DecryptFile(selectedFilePath, tempFilePath);

            Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });

            // Start monitoring the temporary folder
            fileSystemWatcher.Path = tempFolderPath;
            fileSystemWatcher.EnableRaisingEvents = true;
        }
        else
        {
            MessageBox.Show("Please select a file to open.");
        }
    }

    private void DecryptFile(string encryptedFilePath, string outputFilePath)
    {
        // Implement your decryption logic here
        File.Copy(encryptedFilePath, outputFilePath, true);
        MessageBox.Show($"Decrypting file: {encryptedFilePath} in the path :{outputFilePath}");
    }

    private void EncryptFile(string inputFilePath, string encryptedFilePath)
    {
        // Implement your encryption logic here
        File.Copy(inputFilePath, encryptedFilePath, true);
        MessageBox.Show($"Encrypting file: {inputFilePath} in the path :{encryptedFilePath}");
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        // Encrypt the modified file and update the original file
        if (e.ChangeType == WatcherChangeTypes.Changed)
        {
            EncryptFile(e.FullPath, selectedFilePath);
        }
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        // Handle file renaming if necessary
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Cleanup temporary files
        if (Directory.Exists(tempFolderPath))
        {
            Directory.Delete(tempFolderPath, true);
        }
    }
}
*/

/* this
        public partial class Form1 : Form
        {
            private FileSystemWatcher fileSystemWatcher;
            private string selectedFilePath;
            private string tempFolderPath;

            // Public and private keys (replace these with your generated keys)
            string publicKey = "<RSAKeyValue><Modulus>pEomPb6ig2STFFAu5/wPTWt3K+MI0wXRcu5YK+XaiPCs+6Ne2k5PFuDb02XqP80IRr7K96ybaza2f04Wa2xg1sKYwUF7v3IqQCpfqGIqf1yzOAwB7q5kQlJM7qBREv2cQsPup0r+mhlSTlgZrONk1VbUzdDwS7W1SJRFKK74ZAn9cuclbyRzN6yNgjJgzTQu8K94gIMviZ0kQNmdrwKnacUK3drZquwN8Q8n+0auytthmLGMldnBSpgotx8yNn0iEETFvZsoUO+Ks6tHZ0T+PKMYxV2hweVcHPR2HIpwQ+0FOXoK2G9VnJQmQafc9hwK9xQuKMLd8ftR3YpHbvoMmQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string privateKey = "<RSAKeyValue><Modulus>pEomPb6ig2STFFAu5/wPTWt3K+MI0wXRcu5YK+XaiPCs+6Ne2k5PFuDb02XqP80IRr7K96ybaza2f04Wa2xg1sKYwUF7v3IqQCpfqGIqf1yzOAwB7q5kQlJM7qBREv2cQsPup0r+mhlSTlgZrONk1VbUzdDwS7W1SJRFKK74ZAn9cuclbyRzN6yNgjJgzTQu8K94gIMviZ0kQNmdrwKnacUK3drZquwN8Q8n+0auytthmLGMldnBSpgotx8yNn0iEETFvZsoUO+Ks6tHZ0T+PKMYxV2hweVcHPR2HIpwQ+0FOXoK2G9VnJQmQafc9hwK9xQuKMLd8ftR3YpHbvoMmQ==</Modulus><Exponent>AQAB</Exponent><P>yDNuFd4Ooiy3I+3VfOzEAZFjeAwDTn0txgZkfUhz8tLNv57sE/YYOCdbeVB798T5SzQGSRgERGWhf38FUr4RZnk0PYlJIOhXYCW20+VqUXVi76AChZKeTD+LW6oA6SextQ5XDxguuBR3HXCSEzMLbztu7GKFKF4RLzPLfl6XHy8=</P><Q>0hRowDUJCPUrmHOKU20inrWKOJpFSYXBhkoxFLb3JL+wfoOEOageirRxcZGDFj9+wBY1VsiGirspHNigCaUfX+T22CcLni8daeEMD66OtBSDHVUStqVFXSvzvJ/HXM4oaczeF897kZoTMJaq0/0sEAOyL4riGLZDg1O1w8CY3rc=</Q><DP>rfj7ZidUwhtnfuJzzh6V6eLk9HJEAVYIi/gMJU7r64zC3Q0GjAYkX8/bzt1hDRvvoylveN9U/fbvU7MW9iNTTaBwBDWkBePB4jyD1zmanXUL57490sNpqkriGGLwbaxf4j2269gQ6Uhnhn7HgcWu3xdfui2XF1A/UKyK3qLmYmM=</DP><DQ>dCHuKlrnLLmDqRouHKJXIg9PYqz9ooA7lEYNTds7UmhZZAbGIK1sfNb1Y4fCRJlDM2LTh68AmU6Pkq+6ALr0VimXv7QN+xsmknJjHYQsxSYVuTBfUPKaHsB+VAarygT5WEt1dwou0DWxaAnSU7BAHHY+mYOYK3sOZVnwQE+G8o8=</DQ><InverseQ>gn/AB8Ipw4YZi8rITmsUV7XERHOsqrP4E7iS/eQcKMzVJK09kE/CCKqRvD5BT5sz8+xxJLzvBgEmOjUVp6OKEasJkdVilKF/dCB4RqFdLOi7a/zzHYoxI+cxC0A/Si/WswqN5z4AudJCNDG6m9xK7jkkz+Z2kMQm+xPNK7xrvWo=</InverseQ><D>OyQWMBP4HKSP7bgPqqMqPwx4kevguVZW17DFPMg5qyI4oz1kX+HgxrObbrY5ZU5BtHXqSs5wBGBlwd4yY+lX0veqVadlDL3kR8T5OnxcIA0W1w47g9hzbwyBdUVw6g0xuzcdKkvs6zxCTMGaPIp/Oe1QHechGaAH6mC1pLeX4yrByDbO2qwfOvizt8UVCuzYdnOzpfOBqJHcMjnMmyFMPorWGIF6ePiqVrTJmdgXIFOSNhxtYbdRpYrBBX2uY6oROEPXBHBcYLy3r4bYqK+oj29OWWQ2aT/DO+6sH7qV0BUtqLHT/FBywvX4SM9IOjzTrxxpa8TG62OX6DjHGLn8IQ==</D></RSAKeyValue>\r\n";

            public Form1()
            {
                InitializeComponent();

                // Subscribe to the Load event of the form
                this.Load += Form1_Load;
                this.FormClosing += Form1_FormClosing;
            }

            private void Form1_Load(object sender, EventArgs e)
            {
                // Initialize FileSystemWatcher
                fileSystemWatcher = new FileSystemWatcher
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    IncludeSubdirectories = true
                };
                fileSystemWatcher.Changed += OnChanged;
                fileSystemWatcher.Created += OnChanged;
                fileSystemWatcher.Deleted += OnChanged;
                fileSystemWatcher.Renamed += OnRenamed;
            }

            private void button_LoadFiles_Click(object sender, EventArgs e)
            {
            /*
                string folderPath = textBox_FolderPath.Text;

                if (Directory.Exists(folderPath))
                {
                    listBox_Files.Items.Clear();
                    string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

                    foreach (string file in files)
                    {
                        listBox_Files.Items.Add(file);
                    }

                    // Start monitoring the original folder
                    fileSystemWatcher.Path = folderPath;
                    fileSystemWatcher.EnableRaisingEvents = true;
                }
                else
                {
                    MessageBox.Show("The specified folder path does not exist.");
                }
                */
/* this
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = folderBrowserDialog.SelectedPath;
                    fileSystemWatcher.Path = folderPath;
                    fileSystemWatcher.EnableRaisingEvents = true;

                    listBox_Files.Items.Clear();
                    var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        listBox_Files.Items.Add(file);
                    }
                }
        }

            private void button_OpenWithDefault_Click(object sender, EventArgs e)
            {
                if (listBox_Files.SelectedItem != null)
                {
                    selectedFilePath = listBox_Files.SelectedItem.ToString();
                    tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                    Directory.CreateDirectory(tempFolderPath);

                    string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(selectedFilePath));

                    // Decrypt the selected file to the temporary folder
                    DecryptFile(selectedFilePath, tempFilePath);

                    Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });

                    // Start monitoring the temporary folder
                    fileSystemWatcher.Path = tempFolderPath;
                    fileSystemWatcher.EnableRaisingEvents = true;
                }
                else
                {
                    MessageBox.Show("Please select a file to open.");
                }
            }

            private void DecryptFile(string encryptedFilePath, string outputFilePath)
            {
                byte[] encryptedData = File.ReadAllBytes(encryptedFilePath);
                byte[] decryptedData;

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);
                    decryptedData = rsa.Decrypt(encryptedData, false);  // PKCS#1 v1.5 padding
                }

                File.WriteAllBytes(outputFilePath, decryptedData);
            }

            private void EncryptFile(string inputFilePath, string encryptedFilePath)
            {
                try
                {
                    if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(encryptedFilePath))
                    {
                        MessageBox.Show($"Input or encrypted file path is empty.{inputFilePath}, {encryptedFilePath}");
                        return;
                    }
                    byte[] data = File.ReadAllBytes(inputFilePath);
                    byte[] encryptedData;

                    using (var rsa = new RSACryptoServiceProvider())
                    {
                        rsa.FromXmlString(publicKey);
                        encryptedData = rsa.Encrypt(data, false);  // PKCS#1 v1.5 padding
                    }
              //  MessageBox.Show($"Filepath input {inputFilePath}. and encrypt: {encryptedFilePath}");
                File.WriteAllBytes(encryptedFilePath, encryptedData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error encrypting file: {ex.Message}");
                }
            }

            private void OnChanged(object sender, FileSystemEventArgs e)
            {
                // Encrypt the modified file and update the original file
                if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    EncryptFile(e.FullPath, selectedFilePath);
                }
            }

            private void OnRenamed(object sender, RenamedEventArgs e)
            {
                // Handle file renaming if necessary
            }

            private void Form1_FormClosing(object sender, FormClosingEventArgs e)
            {
                // Cleanup temporary files
                if (Directory.Exists(tempFolderPath))
                {
                    Directory.Delete(tempFolderPath, true);
                }
            }
        /* old creating
            private void button_CreateFile_Click(object sender, EventArgs e)
            {
                if (fileSystemWatcher.Path != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        InitialDirectory = fileSystemWatcher.Path,
                        Filter = "All Files (*.*)|*.*"
                    };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.Create(saveFileDialog.FileName).Close();
                        EncryptFile(saveFileDialog.FileName, saveFileDialog.FileName);
                        listBox_Files.Items.Add(saveFileDialog.FileName);
                    }
                }
            }
            */
/* this 
private void button_CreateFile_Click(object sender, EventArgs e)
        {
            if (fileSystemWatcher.Path != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = fileSystemWatcher.Path,
                    Filter = "All Files (*.*)|*.*"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.Create(saveFileDialog.FileName).Close();
                    EncryptFile(saveFileDialog.FileName, saveFileDialog.FileName);
                    listBox_Files.Items.Add(saveFileDialog.FileName);
                }
            }
        }
        private void button_DeleteFile_Click(object sender, EventArgs e)
            {
                if (listBox_Files.SelectedItem != null)
                {
                    string filePath = listBox_Files.SelectedItem.ToString();
                    File.Delete(filePath);
                    listBox_Files.Items.Remove(filePath);
                }
            }

        private void button_TransferFile_Click(object sender, EventArgs e)
        {/*
            if (fileSystemWatcher.Path != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string destinationFilePath = Path.Combine(fileSystemWatcher.Path, Path.GetFileName(openFileDialog.FileName));
                    File.Copy(openFileDialog.FileName, destinationFilePath);
                    EncryptFile(destinationFilePath, destinationFilePath);
                    listBox_Files.Items.Add(destinationFilePath);
                }
            }
            */
/* this
   if (fileSystemWatcher.Path != null)
           {
               OpenFileDialog openFileDialog = new OpenFileDialog();
               if (openFileDialog.ShowDialog() == DialogResult.OK)
               {
                   string destinationFilePath = Path.Combine(fileSystemWatcher.Path, Path.GetFileName(openFileDialog.FileName));
                   try
                   {
                       if (!File.Exists(destinationFilePath))
                       {
                           File.Copy(openFileDialog.FileName, destinationFilePath);
                           MessageBox.Show($"Filepath input {destinationFilePath}. and encrypt: {destinationFilePath}");
                           EncryptFile(destinationFilePath, destinationFilePath);
                           listBox_Files.Items.Add(destinationFilePath);
                       }
                       else
                       {
                           MessageBox.Show("File already exists in the target folder.");
                       }
                   }
                   catch (Exception ex)
                   {
                       MessageBox.Show($"Error transferring file: {ex.Message}");
                   }
               }
           }
               else
               {
                   MessageBox.Show("No folder selected. Please load files from a folder first.");
               }
       }

       /* old one 
           private void button_TransferFile_Click(object sender, EventArgs e)
           {
               if (fileSystemWatcher.Path != null)
               {
                   OpenFileDialog openFileDialog = new OpenFileDialog();
                   if (openFileDialog.ShowDialog() == DialogResult.OK)
                   {
                       string destinationFilePath = Path.Combine(fileSystemWatcher.Path, Path.GetFileName(openFileDialog.FileName));
                       File.Copy(openFileDialog.FileName, destinationFilePath);
                       EncryptFile(destinationFilePath, destinationFilePath);
                       listBox_Files.Items.Add(destinationFilePath);
                   }
               }
           }
          */

/* this one function but the file brk li mkhlt
private void button_TransferOut_Click(object sender, EventArgs e)
    {
        if (listBox_Files.SelectedItem != null)
        {
            string sourceFilePath = listBox_Files.SelectedItem.ToString();
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = Path.GetFileName(sourceFilePath),
                Filter = "All Files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string destinationFilePath = saveFileDialog.FileName;
                File.Copy(sourceFilePath, destinationFilePath, true);
                DecryptFile(destinationFilePath, destinationFilePath);
                listBox_Files.Items.Remove(sourceFilePath);
                listBox_Files.Items.Add(destinationFilePath);
            }
        }
    }
*/
/* this
   private void button_TransferOut_Click(object sender, EventArgs e)
       {
           if (listBox_Files.SelectedItem != null)
           {
               string sourceFilePath = listBox_Files.SelectedItem.ToString();
               SaveFileDialog saveFileDialog = new SaveFileDialog
               {
                   FileName = Path.GetFileName(sourceFilePath),
                   Filter = "All Files (*.*)|*.*"
               };
               if (saveFileDialog.ShowDialog() == DialogResult.OK)
               {
                   string destinationFilePath = saveFileDialog.FileName;
                   DecryptFile(sourceFilePath, destinationFilePath);
                   //listBox_Files.Items.Remove(sourceFilePath);
               }
           }
       }



   }
}

*/



namespace App
{
    public partial class Form1 : Form
    {
        private FileSystemWatcher originalFolderWatcher;
        private FileSystemWatcher tempFolderWatcher;
        private string originalFolderPath = @"C:\Users\ASUS\Documents\Test";  // Update with your original folder path
        private string tempFolderPath = @"C:\Users\ASUS\Documents\Temp";  // Update with your temp folder path

        string publicKey = "<RSAKeyValue><Modulus>pEomPb6ig2STFFAu5/wPTWt3K+MI0wXRcu5YK+XaiPCs+6Ne2k5PFuDb02XqP80IRr7K96ybaza2f04Wa2xg1sKYwUF7v3IqQCpfqGIqf1yzOAwB7q5kQlJM7qBREv2cQsPup0r+mhlSTlgZrONk1VbUzdDwS7W1SJRFKK74ZAn9cuclbyRzN6yNgjJgzTQu8K94gIMviZ0kQNmdrwKnacUK3drZquwN8Q8n+0auytthmLGMldnBSpgotx8yNn0iEETFvZsoUO+Ks6tHZ0T+PKMYxV2hweVcHPR2HIpwQ+0FOXoK2G9VnJQmQafc9hwK9xQuKMLd8ftR3YpHbvoMmQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        string privateKey = "<RSAKeyValue><Modulus>pEomPb6ig2STFFAu5/wPTWt3K+MI0wXRcu5YK+XaiPCs+6Ne2k5PFuDb02XqP80IRr7K96ybaza2f04Wa2xg1sKYwUF7v3IqQCpfqGIqf1yzOAwB7q5kQlJM7qBREv2cQsPup0r+mhlSTlgZrONk1VbUzdDwS7W1SJRFKK74ZAn9cuclbyRzN6yNgjJgzTQu8K94gIMviZ0kQNmdrwKnacUK3drZquwN8Q8n+0auytthmLGMldnBSpgotx8yNn0iEETFvZsoUO+Ks6tHZ0T+PKMYxV2hweVcHPR2HIpwQ+0FOXoK2G9VnJQmQafc9hwK9xQuKMLd8ftR3YpHbvoMmQ==</Modulus><Exponent>AQAB</Exponent><P>yDNuFd4Ooiy3I+3VfOzEAZFjeAwDTn0txgZkfUhz8tLNv57sE/YYOCdbeVB798T5SzQGSRgERGWhf38FUr4RZnk0PYlJIOhXYCW20+VqUXVi76AChZKeTD+LW6oA6SextQ5XDxguuBR3HXCSEzMLbztu7GKFKF4RLzPLfl6XHy8=</P><Q>0hRowDUJCPUrmHOKU20inrWKOJpFSYXBhkoxFLb3JL+wfoOEOageirRxcZGDFj9+wBY1VsiGirspHNigCaUfX+T22CcLni8daeEMD66OtBSDHVUStqVFXSvzvJ/HXM4oaczeF897kZoTMJaq0/0sEAOyL4riGLZDg1O1w8CY3rc=</Q><DP>rfj7ZidUwhtnfuJzzh6V6eLk9HJEAVYIi/gMJU7r64zC3Q0GjAYkX8/bzt1hDRvvoylveN9U/fbvU7MW9iNTTaBwBDWkBePB4jyD1zmanXUL57490sNpqkriGGLwbaxf4j2269gQ6Uhnhn7HgcWu3xdfui2XF1A/UKyK3qLmYmM=</DP><DQ>dCHuKlrnLLmDqRouHKJXIg9PYqz9ooA7lEYNTds7UmhZZAbGIK1sfNb1Y4fCRJlDM2LTh68AmU6Pkq+6ALr0VimXv7QN+xsmknJjHYQsxSYVuTBfUPKaHsB+VAarygT5WEt1dwou0DWxaAnSU7BAHHY+mYOYK3sOZVnwQE+G8o8=</DQ><InverseQ>gn/AB8Ipw4YZi8rITmsUV7XERHOsqrP4E7iS/eQcKMzVJK09kE/CCKqRvD5BT5sz8+xxJLzvBgEmOjUVp6OKEasJkdVilKF/dCB4RqFdLOi7a/zzHYoxI+cxC0A/Si/WswqN5z4AudJCNDG6m9xK7jkkz+Z2kMQm+xPNK7xrvWo=</InverseQ><D>OyQWMBP4HKSP7bgPqqMqPwx4kevguVZW17DFPMg5qyI4oz1kX+HgxrObbrY5ZU5BtHXqSs5wBGBlwd4yY+lX0veqVadlDL3kR8T5OnxcIA0W1w47g9hzbwyBdUVw6g0xuzcdKkvs6zxCTMGaPIp/Oe1QHechGaAH6mC1pLeX4yrByDbO2qwfOvizt8UVCuzYdnOzpfOBqJHcMjnMmyFMPorWGIF6ePiqVrTJmdgXIFOSNhxtYbdRpYrBBX2uY6oROEPXBHBcYLy3r4bYqK+oj29OWWQ2aT/DO+6sH7qV0BUtqLHT/FBywvX4SM9IOjzTrxxpa8TG62OX6DjHGLn8IQ==</D></RSAKeyValue>\r\n";

        public Form1()
        {
            InitializeComponent();

            InitializeFileSystemWatchers();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }
        /*
        private void InitializeFileSystemWatchers()
        {
            // Initialize watcher for original folder
            originalFolderWatcher = new FileSystemWatcher
            {
                Path = originalFolderPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime
            };
            originalFolderWatcher.Created += OnCreated;
            originalFolderWatcher.EnableRaisingEvents = true;

            // Initialize watcher for temp folder
            tempFolderWatcher = new FileSystemWatcher
            {
                Path = tempFolderPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime
            };
            tempFolderWatcher.Changed += OnChanged;
            tempFolderWatcher.EnableRaisingEvents = true;
        }
        */

        private void InitializeFileSystemWatchers()
        {
            // Watcher for the original folder
            originalFolderWatcher = new FileSystemWatcher
            {
                Path = originalFolderPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*",
                IncludeSubdirectories = true
            };
            originalFolderWatcher.Changed += OnChanged;
            originalFolderWatcher.Created += OnChanged;
            originalFolderWatcher.EnableRaisingEvents = true;

            // Watcher for the temp folder
            tempFolderWatcher = new FileSystemWatcher
            {
                Path = tempFolderPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*",
                IncludeSubdirectories = false
            };
            tempFolderWatcher.Changed += OnChanged;
            //tempFolderWatcher.Created += OnChanged;
            tempFolderWatcher.EnableRaisingEvents = true;
        }
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                EncryptFile(e.FullPath, e.FullPath, publicKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error encrypting file: {ex.Message}");
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            /*
            try
            {
                string originalFilePath = Path.Combine(originalFolderPath, Path.GetFileName(e.FullPath));
                EncryptFile(e.FullPath, originalFilePath, publicKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error encrypting modified file: {ex.Message}");
            }
            */
            try
            {
                string path=e.FullPath;
                if (Path.GetDirectoryName(e.FullPath) == originalFolderPath && e.ChangeType == WatcherChangeTypes.Created)
                {
                   // string tempEncryptedFilePath = Path.Combine(tempFolderPath, Path.GetFileName(e.FullPath));
                    string tempEncryptedFilePath = Path.GetTempFileName();
                    EncryptFile(e.FullPath, tempEncryptedFilePath, publicKey);
                    MessageBox.Show($"after encryption,{path} to {tempEncryptedFilePath}");
                    //File.Copy(tempEncryptedFilePath, e.FullPath, overwrite: true);
                    if (File.Exists(tempEncryptedFilePath))
                    {
                        File.Copy(tempEncryptedFilePath, e.FullPath, overwrite: true);
                        //File.Delete(tempEncryptedFilePath);
                    }
                    else
                    {
                        MessageBox.Show($"Temporary encrypted file not found: {tempEncryptedFilePath}");
                    }
                    if (File.Exists(tempEncryptedFilePath))
                    {
                        File.Delete(tempEncryptedFilePath);
                    }
                }
                else if (Path.GetDirectoryName(e.FullPath) == tempFolderPath && e.ChangeType == WatcherChangeTypes.Changed)
                {
                    string originalFilePath = Path.Combine(originalFolderPath, Path.GetFileName(e.FullPath));
                    EncryptFile(e.FullPath, originalFilePath, publicKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing file change event: {ex.Message}");
            }
        }

        private void button_CreateFile_Click(object sender, EventArgs e)
        {
            if (originalFolderWatcher.Path != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = originalFolderWatcher.Path,
                    Filter = "All Files (*.*)|*.*"
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.Create(saveFileDialog.FileName).Close();
                    listBox_Files.Items.Add(saveFileDialog.FileName);
                }
            }
        }

        private void OpenWithDefault_Click(string selectedFilePath)
        {
                string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(selectedFilePath));

                // Decrypt the selected file to the temporary folder
                DecryptFile(selectedFilePath, tempFilePath, privateKey);

                Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });
        }

        private void button_TransferFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string sourceFilePath = openFileDialog.FileName;
                string destinationFilePath = Path.Combine(originalFolderPath, Path.GetFileName(sourceFilePath));

                if (!File.Exists(destinationFilePath))
                {
                    try
                    {
                        File.Copy(sourceFilePath, destinationFilePath);
                        EncryptFile(destinationFilePath, destinationFilePath, publicKey);
                        LoadFilesInFolder(originalFolderPath);

                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Error transferring file: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("File already exists in the folder.");
                }
            }
        }

        private void button_TransferOut_Click(object sender, EventArgs e)
        {
            if (listBox_Files.SelectedItem != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFilePath = listBox_Files.SelectedItem.ToString();
                    string destinationFilePath = saveFileDialog.FileName;

                    try
                    {
                        DecryptFile(sourceFilePath, destinationFilePath, privateKey);
                        LoadFilesInFolder(originalFolderPath);
                    }
                    catch (CryptographicException ex)
                    {
                        MessageBox.Show($"Error decrypting file: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a file to transfer out.");
            }
        }
        /* 2nd attempt suite
        private void button_TransferFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string destinationFilePath = Path.Combine(originalFolderPath, Path.GetFileName(openFileDialog.FileName));

                try
                {
                    if (!File.Exists(destinationFilePath))
                    {
                        File.Copy(openFileDialog.FileName, destinationFilePath);
                        MessageBox.Show($"Filepath input: {destinationFilePath}. and encrypt: {destinationFilePath}");
                        EncryptFile(destinationFilePath, destinationFilePath, publicKey);
                        listBox_Files.Items.Add(destinationFilePath);
                    }
                    else
                    {
                        MessageBox.Show("File already exists in the target folder.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error transferring file: {ex.Message}");
                }
            }
        }

        private void button_TransferOut_Click(object sender, EventArgs e)
        {
            if (listBox_Files.SelectedItem != null)
            {
                string sourceFilePath = listBox_Files.SelectedItem.ToString();
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = Path.GetFileName(sourceFilePath)
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string destinationFilePath = saveFileDialog.FileName;

                    try
                    {
                        DecryptFile(sourceFilePath, destinationFilePath, privateKey);
                        MessageBox.Show($"File decrypted to {destinationFilePath}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error decrypting file: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("No file selected to transfer out.");
            }
        }
        */

        /*
        private void EncryptFile(string inputFilePath, string encryptedFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(encryptedFilePath))
                {
                    MessageBox.Show("Input or encrypted file path is empty.");
                    return;
                }

                byte[] data = File.ReadAllBytes(inputFilePath);
                byte[] encryptedData;

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    encryptedData = rsa.Encrypt(data, false);  // PKCS#1 v1.5 padding
                }

                MessageBox.Show($"Filepath input: {inputFilePath}. and encrypt: {encryptedFilePath}");

                File.WriteAllBytes(encryptedFilePath, encryptedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error encrypting file: {ex.Message}");
            }
        }

        private void DecryptFile(string encryptedFilePath, string outputFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedFilePath) || string.IsNullOrEmpty(outputFilePath))
                {
                    MessageBox.Show("Encrypted file path or output file path is empty.");
                    return;
                }

                byte[] encryptedData = File.ReadAllBytes(encryptedFilePath);
                byte[] decryptedData;

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);
                    decryptedData = rsa.Decrypt(encryptedData, false);  // PKCS#1 v1.5 padding
                }

                File.WriteAllBytes(outputFilePath, decryptedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error decrypting file: {ex.Message}");
            }
        }
        */
        /* 2nd attempt
        public static void EncryptFile(string inputFilePath, string encryptedFilePath, string publicKey)
        {
            // Generate a new AES key and IV
            using (Aes aes = Aes.Create())
            {
                byte[] aesKey = aes.Key;
                byte[] aesIV = aes.IV;

                // Encrypt the AES key with RSA
                byte[] encryptedAesKey;
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    encryptedAesKey = rsa.Encrypt(aesKey, false);
                }

                using (FileStream outputFileStream = new FileStream(encryptedFilePath, FileMode.Create))
                {
                    // Write the encrypted AES key and IV to the output file
                    outputFileStream.Write(encryptedAesKey, 0, encryptedAesKey.Length);
                    outputFileStream.Write(aesIV, 0, aesIV.Length);

                    // Create a CryptoStream for AES encryption
                    using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream inputFileStream = new FileStream(inputFilePath, FileMode.Open))
                        {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cryptoStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
            }
        }

        public static void DecryptFile(string encryptedFilePath, string outputFilePath, string privateKey)
        {
            using (FileStream inputFileStream = new FileStream(encryptedFilePath, FileMode.Open))
            {
                // Read the encrypted AES key and IV from the input file
                byte[] encryptedAesKey = new byte[256];
                inputFileStream.Read(encryptedAesKey, 0, encryptedAesKey.Length);

                byte[] aesIV = new byte[16];
                inputFileStream.Read(aesIV, 0, aesIV.Length);

                // Decrypt the AES key with RSA
                byte[] aesKey;
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);
                    aesKey = rsa.Decrypt(encryptedAesKey, false);
                }

                using (Aes aes = Aes.Create())
                {
                    aes.Key = aesKey;
                    aes.IV = aesIV;

                    using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
                    {
                        // Create a CryptoStream for AES decryption
                        using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                outputFileStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
            }
        }
        */

        public static void EncryptFile(string inputFilePath, string encryptedFilePath, string publicKey)
        {
            string tempFilePath = Path.GetTempFileName();
            try
            {
                // Copy the input file to a temporary location
                File.Copy(inputFilePath, tempFilePath, true);

                using (Aes aes = Aes.Create())
                {
                    byte[] aesKey = aes.Key;
                    byte[] aesIV = aes.IV;

                    byte[] encryptedAesKey;
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.FromXmlString(publicKey);
                        encryptedAesKey = rsa.Encrypt(aesKey, false);
                    }

                    using (FileStream outputFileStream = new FileStream(encryptedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // Write the encrypted AES key and IV to the output file
                        outputFileStream.Write(encryptedAesKey, 0, encryptedAesKey.Length);
                        outputFileStream.Write(aesIV, 0, aesIV.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (FileStream inputFileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    cryptoStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("File encrypted to " + encryptedFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encrypting file: {ex.Message}");
            }
            finally
            {
                // Delete the temporary file
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        public static void DecryptFile(string encryptedFilePath, string outputFilePath, string privateKey)
        {
            try
            {
                Console.WriteLine("Starting decryption...");

                using (FileStream inputFileStream = new FileStream(encryptedFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    byte[] encryptedAesKey = new byte[256];
                    inputFileStream.Read(encryptedAesKey, 0, encryptedAesKey.Length);

                    byte[] aesIV = new byte[16];
                    inputFileStream.Read(aesIV, 0, aesIV.Length);

                    byte[] aesKey;
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.FromXmlString(privateKey);
                        aesKey = rsa.Decrypt(encryptedAesKey, false);
                    }

                    Console.WriteLine("AES key decrypted.");

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = aesKey;
                        aes.IV = aesIV;

                        using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    outputFileStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("File decrypted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decrypting file: {ex.Message}");
            }
        }


        private void listBox_Files_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_Files.SelectedItem != null)
            {
                string filePath = listBox_Files.SelectedItem.ToString();
                try
                {
                    // Process.Start(filePath);
                    OpenWithDefault_Click(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}");
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFilesInFolder(originalFolderPath);
        }

        private void LoadFilesInFolder(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    listBox_Files.Items.Clear();
                    string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

                    foreach (string file in files)
                    {
                        listBox_Files.Items.Add(file);
                    }

                }
                else
                {
                    MessageBox.Show("The specified folder path does not exist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}");
            }
        }
       
        private void button_DeleteFile_Click(object sender, EventArgs e)
            {
                if (listBox_Files.SelectedItem != null)
                {
                    string filePath = listBox_Files.SelectedItem.ToString();
                    File.Delete(filePath);
                    listBox_Files.Items.Remove(filePath);
                }
            }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanupTempFolder();
        }

        private void CleanupTempFolder()
        {
            try
            {
                if (Directory.Exists(tempFolderPath))
                {
                    string[] files = Directory.GetFiles(tempFolderPath);
                    foreach (string file in files)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show($"Error deleting file {file}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cleaning up temp folder: {ex.Message}");
            }
        }

    }
}









