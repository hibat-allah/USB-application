using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using Ookii.Dialogs.Wpf;
namespace NewApp
{

        public partial class Form1 : Form
        {
        private FileSystemWatcher originalFolderWatcher;
        private FileSystemWatcher tempFolderWatcher;
        private string originalFolderPath;// = @"C:\Users\ASUS\Documents\Test";  // Update with your original folder path
        private string tempFolderPath = @"C:\Users\user\Documents\Temp";  // Update with your temp folder path
        static string logFilePath2 = @"C:\Users\user\Documents\Log3.txt"; // Update with an appropriate path

        public static string publicKey;// = "<RSAKeyValue><Modulus>pEomPb6ig2STFFAu5/wPTWt3K+MI0wXRcu5YK+XaiPCs+6Ne2k5PFuDb02XqP80IRr7K96ybaza2f04Wa2xg1sKYwUF7v3IqQCpfqGIqf1yzOAwB7q5kQlJM7qBREv2cQsPup0r+mhlSTlgZrONk1VbUzdDwS7W1SJRFKK74ZAn9cuclbyRzN6yNgjJgzTQu8K94gIMviZ0kQNmdrwKnacUK3drZquwN8Q8n+0auytthmLGMldnBSpgotx8yNn0iEETFvZsoUO+Ks6tHZ0T+PKMYxV2hweVcHPR2HIpwQ+0FOXoK2G9VnJQmQafc9hwK9xQuKMLd8ftR3YpHbvoMmQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public static string privateKey;// = "<RSAKeyValue><Modulus>pEomPb6ig2STFFAu5/wPTWt3K+MI0wXRcu5YK+XaiPCs+6Ne2k5PFuDb02XqP80IRr7K96ybaza2f04Wa2xg1sKYwUF7v3IqQCpfqGIqf1yzOAwB7q5kQlJM7qBREv2cQsPup0r+mhlSTlgZrONk1VbUzdDwS7W1SJRFKK74ZAn9cuclbyRzN6yNgjJgzTQu8K94gIMviZ0kQNmdrwKnacUK3drZquwN8Q8n+0auytthmLGMldnBSpgotx8yNn0iEETFvZsoUO+Ks6tHZ0T+PKMYxV2hweVcHPR2HIpwQ+0FOXoK2G9VnJQmQafc9hwK9xQuKMLd8ftR3YpHbvoMmQ==</Modulus><Exponent>AQAB</Exponent><P>yDNuFd4Ooiy3I+3VfOzEAZFjeAwDTn0txgZkfUhz8tLNv57sE/YYOCdbeVB798T5SzQGSRgERGWhf38FUr4RZnk0PYlJIOhXYCW20+VqUXVi76AChZKeTD+LW6oA6SextQ5XDxguuBR3HXCSEzMLbztu7GKFKF4RLzPLfl6XHy8=</P><Q>0hRowDUJCPUrmHOKU20inrWKOJpFSYXBhkoxFLb3JL+wfoOEOageirRxcZGDFj9+wBY1VsiGirspHNigCaUfX+T22CcLni8daeEMD66OtBSDHVUStqVFXSvzvJ/HXM4oaczeF897kZoTMJaq0/0sEAOyL4riGLZDg1O1w8CY3rc=</Q><DP>rfj7ZidUwhtnfuJzzh6V6eLk9HJEAVYIi/gMJU7r64zC3Q0GjAYkX8/bzt1hDRvvoylveN9U/fbvU7MW9iNTTaBwBDWkBePB4jyD1zmanXUL57490sNpqkriGGLwbaxf4j2269gQ6Uhnhn7HgcWu3xdfui2XF1A/UKyK3qLmYmM=</DP><DQ>dCHuKlrnLLmDqRouHKJXIg9PYqz9ooA7lEYNTds7UmhZZAbGIK1sfNb1Y4fCRJlDM2LTh68AmU6Pkq+6ALr0VimXv7QN+xsmknJjHYQsxSYVuTBfUPKaHsB+VAarygT5WEt1dwou0DWxaAnSU7BAHHY+mYOYK3sOZVnwQE+G8o8=</DQ><InverseQ>gn/AB8Ipw4YZi8rITmsUV7XERHOsqrP4E7iS/eQcKMzVJK09kE/CCKqRvD5BT5sz8+xxJLzvBgEmOjUVp6OKEasJkdVilKF/dCB4RqFdLOi7a/zzHYoxI+cxC0A/Si/WswqN5z4AudJCNDG6m9xK7jkkz+Z2kMQm+xPNK7xrvWo=</InverseQ><D>OyQWMBP4HKSP7bgPqqMqPwx4kevguVZW17DFPMg5qyI4oz1kX+HgxrObbrY5ZU5BtHXqSs5wBGBlwd4yY+lX0veqVadlDL3kR8T5OnxcIA0W1w47g9hzbwyBdUVw6g0xuzcdKkvs6zxCTMGaPIp/Oe1QHechGaAH6mC1pLeX4yrByDbO2qwfOvizt8UVCuzYdnOzpfOBqJHcMjnMmyFMPorWGIF6ePiqVrTJmdgXIFOSNhxtYbdRpYrBBX2uY6oROEPXBHBcYLy3r4bYqK+oj29OWWQ2aT/DO+6sH7qV0BUtqLHT/FBywvX4SM9IOjzTrxxpa8TG62OX6DjHGLn8IQ==</D></RSAKeyValue>\r\n";
        public static string username = Environment.UserName;

        //Icons
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        private bool isExiting;

        private string currentPath;
        // Static constructor to initialize static fields
       

        public Form1(string originalPath)
        {
            (publicKey, privateKey) = GetCertificateKeysXml(username);
            Log("keys " + publicKey+" \n"+ privateKey);
            originalFolderPath = originalPath;
            InitializeComponent();
            InitializeFileSystemWatchers();
            UpdateCurrentPathLabel(currentPath);
            SetupNotifyIcon();
            this.FormClosing += Form1_FormClosing;

        }
        // -------- background functions ----------------------
        private void SetupNotifyIcon()
        {
            notifyIcon = new NotifyIcon
            {
                //Icon = SystemIcons.Application,
                Icon = LoadEmbeddedIcon("NewApp.Icons.icon.ico"),
                //Icon = System.Drawing.Image.FromFile("C:\\Users\\ASUS\\source\\repos\\NewApp\\NewApp\\Icons\\icon.ico"),
                Text = "File Encryption App",
                Visible = true
            };

            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Open", null, ShowForm);
            contextMenu.Items.Add("Quit", null, ExitApplication);
            notifyIcon.ContextMenuStrip = contextMenu;

            notifyIcon.DoubleClick += ShowForm;
        }
        private Icon LoadEmbeddedIcon(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    return new Icon(stream);
                }
                else
                {
                    throw new Exception("Icon resource not found.");
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!isExiting)
            {
                e.Cancel = true; // Cancel the close event
                this.Hide(); // Hide the form
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void ShowForm(object sender, EventArgs e)
        {
            this.Show(); // Show the form
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            isExiting = true;
            notifyIcon.Visible = false; // Hide the icon
            notifyIcon.Dispose();
            Application.Exit(); // Close the application
        }
        //------------------------------------------------------
        // --- Initialisation
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
            originalFolderWatcher.IncludeSubdirectories = true;
            originalFolderWatcher.Changed += OnChanged;
            originalFolderWatcher.Created += OnChanged;
            originalFolderWatcher.Renamed += OnRenamed;
            originalFolderWatcher.EnableRaisingEvents = true;

            // Watcher for the temp folder
            tempFolderWatcher = new FileSystemWatcher
            {
                Path = tempFolderPath,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*",
                IncludeSubdirectories = true
            };
            tempFolderWatcher.Changed += OnChanged;
            //tempFolderWatcher.Created += OnChanged;
            tempFolderWatcher.EnableRaisingEvents = true;
        }
        // -----------------------------------------------------


        // --------- loading functions -----------------
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeImageList();
            currentPath = originalFolderPath; // or your initial directory
            LoadFilesAndDirectories();
            UpdateCurrentPathLabel(currentPath);

        }
        private void UpdateCurrentPathLabel(string path)
        {
            // Update the label text with the current directory path
            label_CurrentPath.Text =  path;
        }

        private void LoadFilesAndDirectories()
            {
            if (listView_Files.InvokeRequired)
            {
                listView_Files.Invoke((MethodInvoker)delegate
                {
                    LoadFilesAndDirectories();
                });
            }
            else{
                listView_Files.Items.Clear();
                listView_Files.Columns.Clear();
                listView_Files.Columns.Add("Name", 250);
                listView_Files.Columns.Add("Type", 100);
                listView_Files.Columns.Add("Size", 100);

                try
                {
                    var dirs = Directory.GetDirectories(currentPath);
                    var files = Directory.GetFiles(currentPath);

                    foreach (var dir in dirs)
                    {
                        var dirInfo = new DirectoryInfo(dir);
                        var item = new ListViewItem(dirInfo.Name);
                        item.SubItems.Add("Directory");
                        item.ImageIndex = 0; // Assuming directory icon is at index 0
                        item.Tag = dirInfo.FullName;
                        listView_Files.Items.Add(item);
                    }

                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        var item = new ListViewItem(fileInfo.Name);
                        item.SubItems.Add(fileInfo.Extension);
                        item.SubItems.Add(fileInfo.Length.ToString());
                        item.ImageIndex = GetFileIconIndex(fileInfo.FullName); // Implement this method to get the correct icon index
                        item.Tag = fileInfo.FullName;
                        listView_Files.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading directory: {ex.Message}");
                }
            }
            }

         [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SYSICONINDEX = 0x4000;

        private void InitializeImageList()
        {
            var shfi = new SHFILEINFO();
            IntPtr hImgSmall = SHGetFileInfo("", 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_SMALLICON | SHGFI_SYSICONINDEX);
            IntPtr hImgLarge = SHGetFileInfo("", 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_LARGEICON | SHGFI_SYSICONINDEX);


            listView_Files.SmallImageList = new ImageList();
            listView_Files.SmallImageList.ImageSize = new Size(16, 16);
            listView_Files.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;

            listView_Files.LargeImageList = new ImageList();
            listView_Files.LargeImageList.ImageSize = new Size(32, 32);
            listView_Files.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;

            if (hImgSmall != IntPtr.Zero)
            {
                listView_Files.SmallImageList.Images.Add(Icon.FromHandle(shfi.hIcon));
            }

            if (hImgLarge != IntPtr.Zero)
            {
                Icon largeIcon = Icon.FromHandle(shfi.hIcon);
                listView_Files.LargeImageList.Images.Add(largeIcon);
                
            }
        }

        private int GetFileIconIndex(string extension)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            IntPtr hImg = SHGetFileInfo(extension, 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_SMALLICON);

            if (hImg != IntPtr.Zero)
            {
                Icon icon = Icon.FromHandle(shfi.hIcon);
                listView_Files.SmallImageList.Images.Add(icon);
                listView_Files.LargeImageList.Images.Add(icon);
                return listView_Files.SmallImageList.Images.Count - 1;
            }
            return 0; // Default to folder icon
        }

        // -- closing 
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

        // ------------------------------------------------------- 

        // ------------- buttons functions ----------------------
        private void button_Rename_Click(object sender, EventArgs e)
        {
            if (listView_Files.SelectedItems.Count > 0)
            {
                var selectedItem = listView_Files.SelectedItems[0];
                var oldPath = selectedItem.Tag.ToString();
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new name:", "Rename", selectedItem.Text);

                if (!string.IsNullOrEmpty(newName))
                {
                    try
                    {
                        string newPath = Path.Combine(currentPath, newName);

                        if (Directory.Exists(oldPath))
                        {
                            Directory.Move(oldPath, newPath);
                        }
                        else if (File.Exists(oldPath))
                        {
                            File.Move(oldPath, newPath);
                        }
                        // Ensure the UI update is done on the main UI thread
                        this.Invoke((MethodInvoker)delegate
                        {
                            LoadFilesAndDirectories(); // Refresh the list after renaming
                        });
                        //(); // Refresh the list after renaming
                        LoadFilesAndDirectories();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming file or directory: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a file or directory to rename.");
            }
        }

        private void comboBox_ViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedView = comboBox_ViewType.SelectedItem.ToString();

            switch (selectedView)
            {
                case "List":
                    listView_Files.View = View.Details;
                    break;
                case "Grid":
                    listView_Files.View = View.LargeIcon;
                    break;
                default:
                    listView_Files.View = View.Details;
                    comboBox_ViewType.Text = "View: List";
                    break;
            }
            comboBox_ViewType.Text = "View: " + selectedView;
        }

        private void listView_Files_DoubleClick(object sender, EventArgs e)
            {
                if (listView_Files.SelectedItems.Count > 0)
                {
                    var selectedItem = listView_Files.SelectedItems[0];
                    var path = selectedItem.Tag.ToString();
                    if (Directory.Exists(path))
                    {
                        currentPath = path;
                        LoadFilesAndDirectories();
                        UpdateCurrentPathLabel(path);
                }
                    else if (File.Exists(path))
                    {
                        if (!IsEncrypted(path))
                        {
                            Log("File is not encrypted.");
                            EncryptFile(path, path, privateKey);
                            path = AddCryptExtension(path);
                        }

                    // string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(path));
                    // Construct the full temporary file path by replicating the folder structure
                        string tempFilePath = Path.Combine(tempFolderPath, path.Substring(originalFolderPath.Length + 0));
                        string tempDirectory = Path.GetDirectoryName(tempFilePath);
                        if (!Directory.Exists(tempDirectory))
                        {
                            Directory.CreateDirectory(tempDirectory);
                        }
                        // Decrypt the selected file to the temporary folder
                        DecryptFile(path, tempFilePath, privateKey);
                        string outputFilePath2 = RemoveCryptExtension(tempFilePath);
                    // Open the file with the default application
                    MessageBox.Show("OPENING TEMP " + tempFilePath + " OUT2 " + outputFilePath2);
                        System.Diagnostics.Process.Start(outputFilePath2);
                        LoadFilesAndDirectories();
                    }
                }
            }

        private void button_CreateDirectory_Click(object sender, EventArgs e)
            {
                string newDirName = "New Folder";
                string newPath = Path.Combine(currentPath, newDirName);
                int count = 1;
                while (Directory.Exists(newPath))
                {
                    newDirName = $"New Folder ({count++})";
                    newPath = Path.Combine(currentPath, newDirName);
                }
                Directory.CreateDirectory(newPath);
                LoadFilesAndDirectories();
            }

        private void button_Delete_Click(object sender, EventArgs e)
            {
            if (listView_Files.SelectedItems.Count > 0)
            {
                var selectedItem = listView_Files.SelectedItems[0];
                var path = selectedItem.Tag.ToString();

                try
                {
                    if (Directory.Exists(path))
                    {
                        // Confirm directory deletion
                        var result = MessageBox.Show("Are you sure you want to delete this directory and all its contents?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            Directory.Delete(path, true);
                        }
                    }
                    else if (File.Exists(path))
                    {
                        // Confirm file deletion
                        var result = MessageBox.Show("Are you sure you want to delete this file?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            File.Delete(path);
                        }
                    }
                    LoadFilesAndDirectories(); // Refresh the list after deletion
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting file or directory: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a file or directory to delete.");
            }
        }
        
        private void button_Back_Click(object sender, EventArgs e)
        {
            if (currentPath != null && Directory.GetParent(currentPath) != null)
            {
                currentPath = Directory.GetParent(currentPath).FullName;
                LoadFilesAndDirectories();
                UpdateCurrentPathLabel(currentPath);
            }
        }
        /*
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
                        LoadFilesAndDirectories();
                        // Insert log
                        LogHelper.InsertLog(Environment.UserName, "copie", Path.GetFileName(sourceFilePath), sourceFilePath, "deviceId", "machineId", DateTime.Now);

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
            if (listView_Files.SelectedItems.Count > 0)
              {  
                var selectedItem = listView_Files.SelectedItems[0];
                var path = selectedItem.Tag.ToString();
                
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                        InitialDirectory = originalFolderWatcher.Path,
                        Filter = "All Files (*.*)|*.*"
                };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string sourceFilePath = path;
                        string destinationFilePath = saveFileDialog.FileName;

                        try
                        {
                            DecryptFile(sourceFilePath, destinationFilePath, privateKey);
                            LoadFilesAndDirectories();
                            // Insert log
                            LogHelper.InsertLog(Environment.UserName, "transfert", Path.GetFileName(sourceFilePath), sourceFilePath, "deviceId", "machineId", DateTime.Now);

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
        */
        //-----------------------------------------------------------------------------------------------
       
        private void button_TransferFile_Click(object sender, EventArgs e)
        {
            string sourceFilePath = CustomOpenFileDialog();
            MessageBox.Show("source file " + sourceFilePath);
            if (sourceFilePath != null)
            {
                string destinationDirectory = label_CurrentPath.Text; // Directly get the path from the TextBox

                string destinationFilePath = Path.Combine(destinationDirectory, Path.GetFileName(sourceFilePath));

                if (!File.Exists(destinationFilePath))
                {
                    try
                    {
                        File.Copy(sourceFilePath, destinationFilePath);
                        LoadFilesAndDirectories();
                        // Insert log
                        LogHelper.InsertLog(Environment.UserName, "copie", Path.GetFileName(sourceFilePath), sourceFilePath, "deviceId", "machineId", DateTime.Now);
                    }
                    catch (Exception ex)
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
            if (listView_Files.SelectedItems.Count > 0)
            {
                var selectedItem = listView_Files.SelectedItems[0];
                var path = selectedItem.Tag.ToString();

                string destinationFilePath = CustomSaveFileDialog(Path.GetFileName(path));
                if (destinationFilePath != null)
                {
                    try
                    {
                        DecryptFile(path, destinationFilePath, privateKey);
                        LoadFilesAndDirectories();
                        // Insert log
                        LogHelper.InsertLog(Environment.UserName, "transfert", Path.GetFileName(path), path, "deviceId", "machineId", DateTime.Now);
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

        public string CustomOpenFileDialog()
        {
            Form dialog = new Form();
            dialog.Text = "Open File";
            dialog.Size = new Size(700, 350);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.Icon = LoadEmbeddedIcon("NewApp.Icons.icon.ico"); // Set application icon

            TextBox pathTextBox = new TextBox
            {
                Text = "C:\\",
                Dock = DockStyle.Top
            };

            ListView fileListView = new ListView
            {
                View = View.Details,
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                MultiSelect = false,
                SmallImageList = listView_Files.SmallImageList,
                LargeImageList = listView_Files.LargeImageList
            };

            fileListView.Columns.Add("Name", 300);
            fileListView.Columns.Add("Type", 100);
            fileListView.Columns.Add("Size", 100);

            LoadFilesAndDirectories(fileListView, new DirectoryInfo(pathTextBox.Text));

            pathTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        LoadFilesAndDirectories(fileListView, new DirectoryInfo(pathTextBox.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Invalid path: {ex.Message}");
                    }
                }
            };

            fileListView.DoubleClick += (s, e) =>
            {
                if (fileListView.SelectedItems.Count > 0)
                {
                    var selectedItem = fileListView.SelectedItems[0];
                    var selectedPath = Path.Combine(pathTextBox.Text, selectedItem.Text);
                    if (Directory.Exists(selectedPath))
                    {
                        pathTextBox.Text = selectedPath;
                        LoadFilesAndDirectories(fileListView, new DirectoryInfo(selectedPath));
                    }
                }
            };

            Button backButton = new Button
            {
                Text = "Back",
                Size = new Size(75, 23), // Set button size
                Anchor = AnchorStyles.Top | AnchorStyles.Left, // Align to top-left
                Margin = new Padding(5) // Add margin
            };

            backButton.Click += (s, e) =>
            {
                try
                {
                    var currentPath = new DirectoryInfo(pathTextBox.Text);
                    if (currentPath.Parent != null)
                    {
                        pathTextBox.Text = currentPath.Parent.FullName;
                        LoadFilesAndDirectories(fileListView, currentPath.Parent);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error navigating back: {ex.Message}");
                }
            };

            TableLayoutPanel topPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40, // Set panel height
                ColumnCount = 2,
                ColumnStyles =
        {
            new ColumnStyle(SizeType.AutoSize),
            new ColumnStyle(SizeType.Percent, 100)
        }
            };

            topPanel.Controls.Add(backButton, 0, 0);
            topPanel.Controls.Add(pathTextBox, 1, 0);

            Button okButton = new Button
            {
                Text = "OK",
                Size = new Size(75, 23), // Set button size
                Anchor = AnchorStyles.Right, // Align to bottom-right
                Margin = new Padding(0, 0, 10, 0) // Add margin
            };

            Button cancelButton = new Button
            {
                Text = "Cancel",
                Size = new Size(75, 23), // Set button size
                Anchor = AnchorStyles.Right, // Align to bottom-right
                Margin = new Padding(5, 0, 0, 0) // Add margin
            };

            okButton.Click += (s, e) =>
            {
                if (fileListView.SelectedItems.Count > 0)
                {
                    var selectedItem = fileListView.SelectedItems[0];
                    var selectedPath = Path.Combine(pathTextBox.Text, selectedItem.Text);

                    if (Directory.Exists(selectedPath))
                    {
                        pathTextBox.Text = selectedPath;
                        LoadFilesAndDirectories(fileListView, new DirectoryInfo(selectedPath));
                    }
                    else
                    {
                        dialog.DialogResult = DialogResult.OK;
                        dialog.Close();
                    }
                }
            };

            cancelButton.Click += (s, e) => dialog.DialogResult = DialogResult.Cancel;

            // Create a TableLayoutPanel for organizing buttons
            TableLayoutPanel buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40, // Set panel height
                ColumnCount = 3,
                ColumnStyles =
        {
            new ColumnStyle(SizeType.Percent, 33.33F),
            new ColumnStyle(SizeType.Percent, 33.33F),
            new ColumnStyle(SizeType.Percent, 33.33F)
        }
            };

            buttonPanel.Controls.Add(cancelButton, 1, 0);
            buttonPanel.Controls.Add(okButton, 2, 0);

            // Add controls to dialog
            dialog.Controls.Add(fileListView);
            dialog.Controls.Add(topPanel); // Add top panel containing pathTextBox and backButton
            dialog.Controls.Add(buttonPanel); // Add TableLayoutPanel containing OK and Cancel buttons

            // Ensure the dialog's size is adjusted to its content
            if (dialog.ShowDialog() == DialogResult.OK && fileListView.SelectedItems.Count > 0)
            {
                var selectedItem = fileListView.SelectedItems[0];
                var selectedPath = Path.Combine(pathTextBox.Text, selectedItem.Text);

                if (!Directory.Exists(selectedPath))
                {
                    return selectedPath;
                }
            }

            return null;
        }




        /*
                 public string CustomOpenFileDialog()
                 {
                     Form dialog = new Form();
                     dialog.Text = "Open File";
                     dialog.Size = new Size(700, 350);
                     dialog.StartPosition = FormStartPosition.CenterParent;
                     dialog.Icon = LoadEmbeddedIcon("NewApp.Icons.icon.ico"); // Set application icon

                     TextBox pathTextBox = new TextBox
                     {
                         Text = "C:\\",
                         Dock = DockStyle.Top
                     };

                     ListView fileListView = new ListView
                     {
                         View = View.Details,
                         Dock = DockStyle.Fill,
                         FullRowSelect = true,
                         MultiSelect = false,
                         SmallImageList = listView_Files.SmallImageList,
                         LargeImageList = listView_Files.LargeImageList
                     };

                     fileListView.Columns.Add("Name", 300);
                     fileListView.Columns.Add("Type", 100);
                     fileListView.Columns.Add("Size", 100);

                     LoadFilesAndDirectories(fileListView, new DirectoryInfo(pathTextBox.Text));

                     pathTextBox.KeyDown += (s, e) =>
                     {
                         if (e.KeyCode == Keys.Enter)
                         {
                             try
                             {
                                 LoadFilesAndDirectories(fileListView, new DirectoryInfo(pathTextBox.Text));
                             }
                             catch (Exception ex)
                             {
                                 MessageBox.Show($"Invalid path: {ex.Message}");
                             }
                         }
                     };

                     fileListView.DoubleClick += (s, e) =>
                     {
                         if (fileListView.SelectedItems.Count > 0)
                         {
                             var selectedItem = fileListView.SelectedItems[0];
                             var selectedPath = Path.Combine(pathTextBox.Text, selectedItem.Text);
                             if (Directory.Exists(selectedPath))
                             {
                                 pathTextBox.Text = selectedPath;
                                 LoadFilesAndDirectories(fileListView, new DirectoryInfo(selectedPath));
                             }
                         }
                     };

                     Button backButton = new Button
                     {
                         Text = "Back",
                         Size = new Size(75, 23), // Set button size
                         Anchor = AnchorStyles.Top | AnchorStyles.Right, // Align to top-right
                         //Margin = new Padding(10) // Add margin
                     };

                     backButton.Click += (s, e) =>
                     {
                         try
                         {
                             var currentPath = new DirectoryInfo(pathTextBox.Text);
                             if (currentPath.Parent != null)
                             {
                                 pathTextBox.Text = currentPath.Parent.FullName;
                                 LoadFilesAndDirectories(fileListView, currentPath.Parent);
                             }
                         }
                         catch (Exception ex)
                         {
                             MessageBox.Show($"Error navigating back: {ex.Message}");
                         }
                     };

                     TableLayoutPanel topPanel = new TableLayoutPanel
                     {
                         Dock = DockStyle.Top,
                         Height = 40, // Set panel height
                         ColumnCount = 2,
                         ColumnStyles =
                 {
                     new ColumnStyle(SizeType.AutoSize),
                     new ColumnStyle(SizeType.Percent, 100)
                 }
                     };

                     topPanel.Controls.Add(backButton, 0, 0);
                     topPanel.Controls.Add(pathTextBox, 1, 0);

                     TextBox fileNameTextBox = new TextBox();

                     Button okButton = new Button
                     {
                         Text = "OK",
                         Size = new Size(75, 23), // Set button size
                         Anchor = AnchorStyles.Right , // Align to bottom-right
                         Margin = new Padding(0, 0, 10, 0) // Add margin
                     };

                     Button cancelButton = new Button
                     {
                         Text = "Cancel",
                         Size = new Size(75, 23), // Set button size
                         Anchor = AnchorStyles.Right, // Align to bottom-right
                         Margin = new Padding(5, 0, 0, 0) // Add margin
                     };

                     okButton.Click += (s, e) => dialog.DialogResult = DialogResult.OK;
                     cancelButton.Click += (s, e) => dialog.DialogResult = DialogResult.Cancel;

                     // Create a TableLayoutPanel for organizing buttons
                     TableLayoutPanel buttonPanel = new TableLayoutPanel
                     {
                         Dock = DockStyle.Bottom,
                         Height = 40, // Set panel height
                         ColumnCount = 3,
                         ColumnStyles =
                 {
                     new ColumnStyle(SizeType.Percent, 33.33F),
                     new ColumnStyle(SizeType.Percent, 33.33F),
                     new ColumnStyle(SizeType.Percent, 33.33F)
                 }
                     };

                     buttonPanel.Controls.Add(cancelButton, 1, 0);
                     buttonPanel.Controls.Add(okButton, 2, 0);

                     // Add controls to dialog
                     dialog.Controls.Add(fileListView);
                     dialog.Controls.Add(topPanel); // Add top panel containing pathTextBox and backButton
                     dialog.Controls.Add(fileNameTextBox);
                     dialog.Controls.Add(buttonPanel); // Add TableLayoutPanel containing OK and Cancel buttons

                     // Ensure the dialog's size is adjusted to its content

                     if (dialog.ShowDialog() == DialogResult.OK && fileListView.SelectedItems.Count > 0)
                     {
                         return Path.Combine(pathTextBox.Text, fileNameTextBox.Text);
                     }

                     return null;
                 }
                 */
        public string CustomSaveFileDialog(string defaultFileName = "")
        {
            Form dialog = new Form();
            dialog.Text = "Save File";
            dialog.Size = new Size(700, 350);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.Icon = LoadEmbeddedIcon("NewApp.Icons.icon.ico"); // Set application icon

            TextBox pathTextBox = new TextBox
            {
                Text = "C:\\",
                Dock = DockStyle.Top
            };

            ListView fileListView = new ListView
            {
                View = View.Details,
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                MultiSelect = false,
                SmallImageList = listView_Files.SmallImageList,
                LargeImageList = listView_Files.LargeImageList
            };

            fileListView.Columns.Add("Name", 300);
            fileListView.Columns.Add("Type", 100);
            fileListView.Columns.Add("Size", 100);

            LoadFilesAndDirectories(fileListView, new DirectoryInfo(pathTextBox.Text));

            pathTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        LoadFilesAndDirectories(fileListView, new DirectoryInfo(pathTextBox.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Invalid path: {ex.Message}");
                    }
                }
            };

            fileListView.DoubleClick += (s, e) =>
            {
                if (fileListView.SelectedItems.Count > 0)
                {
                    var selectedItem = fileListView.SelectedItems[0];
                    var selectedPath = Path.Combine(pathTextBox.Text, selectedItem.Text);
                    if (Directory.Exists(selectedPath))
                    {
                        pathTextBox.Text = selectedPath;
                        LoadFilesAndDirectories(fileListView, new DirectoryInfo(selectedPath));
                    }
                }
            };

            Button backButton = new Button
            {
                Text = "Back",
                Size = new Size(75, 23), // Set button size
                Anchor = AnchorStyles.Top | AnchorStyles.Right, // Align to top-right
                Margin = new Padding(10) // Add margin
            };

            backButton.Click += (s, e) =>
            {
                try
                {
                    var currentPath = new DirectoryInfo(pathTextBox.Text);
                    if (currentPath.Parent != null)
                    {
                        pathTextBox.Text = currentPath.Parent.FullName;
                        LoadFilesAndDirectories(fileListView, currentPath.Parent);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error navigating back: {ex.Message}");
                }
            };

            TableLayoutPanel topPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40, // Set panel height
                ColumnCount = 2,
                ColumnStyles =
        {
            new ColumnStyle(SizeType.AutoSize),
            new ColumnStyle(SizeType.Percent, 100)
        }
            };

            topPanel.Controls.Add(backButton, 0, 0);
            topPanel.Controls.Add(pathTextBox, 1, 0);

            TextBox fileNameTextBox = new TextBox
            {
                Text = defaultFileName,
                Dock = DockStyle.Bottom
            };

            Button okButton = new Button
            {
                Text = "OK",
                Size = new Size(75, 23), // Set button size
                Anchor = AnchorStyles.Right, // Align to bottom-right
                Margin = new Padding(0, 0, 10, 0) // Add margin
            };

            Button cancelButton = new Button
            {
                Text = "Cancel",
                Size = new Size(75, 23), // Set button size
                Anchor = AnchorStyles.Right, // Align to bottom-right
                Margin = new Padding(5, 0, 0, 0) // Add margin
            };

            okButton.Click += (s, e) => dialog.DialogResult = DialogResult.OK;
            cancelButton.Click += (s, e) => dialog.DialogResult = DialogResult.Cancel;

            // Create a TableLayoutPanel for organizing buttons
            TableLayoutPanel buttonPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40, // Set panel height
                ColumnCount = 3,
                ColumnStyles =
        {
            new ColumnStyle(SizeType.Percent, 33.33F),
            new ColumnStyle(SizeType.Percent, 33.33F),
            new ColumnStyle(SizeType.Percent, 33.33F)
        }
            };

            buttonPanel.Controls.Add(cancelButton, 1, 0);
            buttonPanel.Controls.Add(okButton, 2, 0);

            // Add controls to dialog
            dialog.Controls.Add(fileListView);
            dialog.Controls.Add(topPanel); // Add top panel containing pathTextBox and backButton
            dialog.Controls.Add(fileNameTextBox);
            dialog.Controls.Add(buttonPanel); // Add TableLayoutPanel containing OK and Cancel buttons

            // Ensure the dialog's size is adjusted to its content
           
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return Path.Combine(pathTextBox.Text, fileNameTextBox.Text);
            }

            return null;
        }



        private void LoadFilesAndDirectories(ListView fileListView, DirectoryInfo dirInfo)
        {
            fileListView.Items.Clear();
           try
            {
                var dirs = dirInfo.GetDirectories();
                var files = dirInfo.GetFiles();

                foreach (var dir in dirs)
                {
                    var item = new ListViewItem(dir.Name);
                    item.SubItems.Add("Directory");
                    item.ImageIndex = 0; // Assuming directory icon is at index 0
                    item.Tag = dirInfo.FullName;
                    fileListView.Items.Add(item);
                }

                foreach (var file in files)
                {
                    var item = new ListViewItem(file.Name);
                    item.SubItems.Add(file.Extension);
                    item.SubItems.Add(file.Length.ToString());
                    item.ImageIndex = GetFileIconIndex(file.FullName); // Pass the file path here
                    item.Tag = file.FullName;
                    fileListView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading directory: {ex.Message}");
            }


        }




        //----------------------------------------------------------------------------------------------------------
        private void button_CreateFile_Click(object sender, EventArgs e)
        {
            string newFileName = Microsoft.VisualBasic.Interaction.InputBox("Enter file name (with extension):", "Create File", "New File.txt");
            if (string.IsNullOrWhiteSpace(newFileName))
            {
                MessageBox.Show("Invalid file name.");
                return;
            }

            string newPath = Path.Combine(currentPath, newFileName);
            int count = 1;

            while (File.Exists(newPath))
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(newFileName);
                string extension = Path.GetExtension(newFileName);
                newFileName = $"{fileNameWithoutExtension} ({count++}){extension}";
                newPath = Path.Combine(currentPath, newFileName);
            }
            int count2 = 1;
            string newFileName2 = AddCryptExtension(newFileName);
            string newPath2 = Path.Combine(currentPath, newFileName2);
            while (File.Exists(newPath2))
            {
                string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(newFileName2);
                string extension = Path.GetExtension(newFileName2);
                newFileName2 = $"{fileNameWithoutExtension2} ({count++}){extension}";
                newPath2 = Path.Combine(currentPath, newFileName2);
                count2++;
            }
            try
            {
               if (count2 > 1)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(newFileName);
                    string extension = Path.GetExtension(newFileName);
                    newFileName = $"{fileNameWithoutExtension} ({count}){extension}";
                    newPath = Path.Combine(currentPath, newFileName);
                }
                File.Create(newPath).Dispose(); // Create the file and immediately close it
                LoadFilesAndDirectories(); // Refresh the list to include the new file
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating file: {ex.Message}");
            }
        }

        //-----------------------------------------------------------

        //------------------ Event Hendlers -----------------------------
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            LoadFilesAndDirectories();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (File.Exists(e.FullPath))
                    {
                    //if (Path.GetDirectoryName(e.FullPath) == originalFolderPath && e.ChangeType == WatcherChangeTypes.Created)
                    if (e.FullPath.StartsWith(originalFolderPath) && e.ChangeType == WatcherChangeTypes.Created)
                    {
                        try 
                        {
                            //Thread.Sleep(10); // Adjust the delay as needed
                       
                            // string tempEncryptedFilePath = Path.Combine(tempFolderPath, Path.GetFileName(e.FullPath));
                        string tempEncryptedPath = Path.GetTempPath();
                        string tempEncryptedFilePath = Path.Combine(tempEncryptedPath, Path.GetFileName(e.FullPath));

                            //Console.Write($"Temporary folder created at: {tempEncryptedFilePath}");
                            MessageBox.Show("path " + e.FullPath);
                        EncryptFile(e.FullPath, tempEncryptedFilePath, publicKey);
                        //MessageBox.Show($"after encryption,{path} to {tempEncryptedFilePath}");
                        string tempEncryptedFilePath2 = AddCryptExtension(tempEncryptedFilePath);
                        string orgpath = AddCryptExtension(e.FullPath);
                        
                        //MessageBox.Show("The tempE2 " + tempEncryptedFilePath2 + " and temp " + tempEncryptedFilePath+"org path "+ orgpath);
                        File.Copy(tempEncryptedFilePath2, e.FullPath, overwrite: true);
                        File.Move(e.FullPath, orgpath);
                        LoadFilesAndDirectories();
                            // Insert log
                            LogHelper.InsertLog(Environment.UserName, "creation", Path.GetFileName(e.FullPath), e.FullPath, "deviceId", "machineId", DateTime.Now);

                        if (File.Exists(tempEncryptedFilePath2))
                        {
                            File.Delete(tempEncryptedFilePath2);
                        }
                        if (File.Exists(e.FullPath))
                        {
                            File.Delete(e.FullPath);
                        }
                            LoadFilesAndDirectories();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error processing file change created event: {ex.Message}");
                        }

                    }
                    else if (e.FullPath.StartsWith(tempFolderPath) && e.ChangeType == WatcherChangeTypes.Changed)
                    {
                        try 
                        {  
                            string originalFilePath = Path.Combine(originalFolderPath, e.FullPath.Substring(tempFolderPath.Length + 1));
                            EncryptFile(e.FullPath, originalFilePath, publicKey);

                            // Insert log
                            LogHelper.InsertLog(Environment.UserName, "modified", Path.GetFileName(originalFilePath), originalFilePath, "deviceId", "machineId", DateTime.Now);
                               LoadFilesAndDirectories(); 
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error processing file change changed event: {ex.Message}");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing file change event: {ex.Message}");
            }
        }


        //----------------------------------------------------------

        //---------------- Crypting Functions ------------------------------
        public static void EncryptFile(string inputFilePath, string encryptedFilePath, string publicKey)
        {
            if (IsEncrypted(inputFilePath))
            {
                Log("File is encrypted.");
                return;
            }
            string encryptedFilePath2 = AddCryptExtension(encryptedFilePath);

            Log("ecryption : encryptFile " + encryptedFilePath + " path2 " + encryptedFilePath2);
            string tempFilePath = Path.GetTempFileName();
            try
            {
                // Copy the input file to a temporary location
                File.Copy(inputFilePath, tempFilePath, overwrite: true);

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

                    using (FileStream outputFileStream = new FileStream(encryptedFilePath2, FileMode.Create, FileAccess.Write, FileShare.None))
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
                if (File.Exists(encryptedFilePath))
                {
                    File.Delete(encryptedFilePath);
                }
                //Console.Write("File encrypted to " + encryptedFilePath);
            }
            catch (Exception ex)
            {
                Console.Write($"Error encrypting file: {ex.Message}");
            }
            /*finally
            {
                // Delete the temporary file
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }*/
        }

        public static void DecryptFile(string encryptedFilePath, string outputFilePath, string privateKey)
        {
            try
            {
                if (!IsEncrypted(encryptedFilePath))
                {
                    Log("File is not encrypted.");
                    return;
                }
                //Console.Write("Starting decryption...");
                string outputFilePath2 = RemoveCryptExtension(outputFilePath);
                Log("decryption : outputFile "+outputFilePath+" path2 "+outputFilePath2);
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

                    //Console.Write("AES key decrypted.");

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = aesKey;
                        aes.IV = aesIV;

                        using (FileStream outputFileStream = new FileStream(outputFilePath2, FileMode.Create, FileAccess.Write, FileShare.None))
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

                //Console.Write("File decrypted successfully.");
            }
            catch (Exception ex)
            {
                Console.Write($"Error decrypting file: {ex.Message}");
            }
        }

        public static bool IsEncrypted(string filePath)
        {
            return Path.GetFileName(filePath).Contains(".crypt");
        }

        private static string AddCryptExtension(string filePath)
        {
            if (IsEncrypted(filePath))
            {
                Log("File is not encrypted.");
                return filePath;
            }
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            string newFileName = $"{fileName}.crypt{extension}";
            return Path.Combine(directory, newFileName);
        }

        public static string RemoveCryptExtension(string filePath)
        {
            if (!IsEncrypted(filePath))
            {
                return filePath;
            }
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string newFileName = fileName.Replace(".crypt", "");
            return Path.Combine(directory, newFileName);
        }
        // getting the public and private keys from the certificate: need to call the function with the current username obtained by this instruction

        //string username = Environment.UserName;
        //Console.Write("Currently logged-in user: " + username);

        public static (string publicKeyXml, string privateKeyXml) GetCertificateKeysXml(string username)
        {
            // Find the certificate based on the username
            X509Certificate2 certificate = FindCertificate(username);

            if (certificate != null)
            {
                // Extract the public key
                RSA publicKey = certificate.GetRSAPublicKey();
                string publicKeyXml = publicKey.ToXmlString(false);

                // Extract the private key if available
                string privateKeyXml = null;
                if (certificate.HasPrivateKey)
                {
                    RSA privateKey = certificate.GetRSAPrivateKey();
                    privateKeyXml = privateKey.ToXmlString(true);
                }

                return (publicKeyXml, privateKeyXml);
            }
            else
            {
                throw new Exception("Certificate not found.");
            }
        }

        /*
        private static X509Certificate2 FindCertificate(string username)
        {
            // Get all certificates for the current user
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificates = store.Certificates;

            // Find the certificate based on username
            X509Certificate2 certificate = certificates.Cast<X509Certificate2>()
                .FirstOrDefault(cert =>
                    cert.SubjectName.Name.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0
                );
            
            store.Close();

            //return certificate;
            return null; //  delete this 
        }
        */

        private static X509Certificate2 FindCertificate(string username)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            foreach (X509Certificate2 cert in store.Certificates)
            {
                if (cert.Subject.IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    store.Close();
                    return cert;
                }
            }

            store.Close();
            return null;
        }

        //--------------------------------------------------------------


        static void Log(string message)
        {
            //lock (lockObj)
            //{
            using (StreamWriter writer = new StreamWriter(logFilePath2, true))
            {
                writer.WriteLine(message);
            }
            //}
        }

        private void listView_Files_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }


}
