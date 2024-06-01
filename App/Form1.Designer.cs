//namespace App
//{
/*   
       partial class Form1
       {
           private System.ComponentModel.IContainer components = null;
           private System.Windows.Forms.MenuStrip menuStrip1;
           private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
           private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
           private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
           private System.Windows.Forms.TextBox textBox_FileContent;

           protected override void Dispose(bool disposing)
           {
               if (disposing && (components != null))
               {
                   components.Dispose();
               }
               base.Dispose(disposing);
           }

           private void InitializeComponent()
           {
           this.menuStrip1 = new System.Windows.Forms.MenuStrip();
           this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
           this.textBox_FileContent = new System.Windows.Forms.TextBox();
           this.menuStrip1.SuspendLayout();
           this.SuspendLayout();
           // 
           // menuStrip1
           // 
           this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
           this.fileToolStripMenuItem});
           this.menuStrip1.Location = new System.Drawing.Point(0, 0);
           this.menuStrip1.Name = "menuStrip1";
           this.menuStrip1.Size = new System.Drawing.Size(800, 29);
           this.menuStrip1.TabIndex = 0;
           this.menuStrip1.Text = "menuStrip1";
           // 
           // fileToolStripMenuItem
           // 
           this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
           this.openToolStripMenuItem,
           this.saveToolStripMenuItem});
           this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
           this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 25);
           this.fileToolStripMenuItem.Text = "File";
           // 
           // openToolStripMenuItem
           // 
           this.openToolStripMenuItem.Name = "openToolStripMenuItem";
           this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
           this.openToolStripMenuItem.Text = "Open";
           this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
           // 
           // saveToolStripMenuItem
           // 
           this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
           this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 26);
           this.saveToolStripMenuItem.Text = "Save";
           this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
           // 
           // textBox_FileContent
           // 
           this.textBox_FileContent.Location = new System.Drawing.Point(12, 27);
           this.textBox_FileContent.Multiline = true;
           this.textBox_FileContent.Name = "textBox_FileContent";
           this.textBox_FileContent.Size = new System.Drawing.Size(776, 411);
           this.textBox_FileContent.TabIndex = 1;
           // 
           // Form1
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size(800, 450);
           this.Controls.Add(this.textBox_FileContent);
           this.Controls.Add(this.menuStrip1);
           this.MainMenuStrip = this.menuStrip1;
           this.Name = "Form1";
           this.Text = "File Editor";
           this.Load += new System.EventHandler(this.Form1_Load);
           this.menuStrip1.ResumeLayout(false);
           this.menuStrip1.PerformLayout();
           this.ResumeLayout(false);
           this.PerformLayout();

           }
       }
   */

/* 2nd
      partial class Form1
      {
          private System.ComponentModel.IContainer components = null;
          private System.Windows.Forms.MenuStrip menuStrip1;
          private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
          private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
          private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
          private System.Windows.Forms.TextBox textBox_FileContent;
          private System.Windows.Forms.TrackBar trackBar_Zoom;
          private System.Windows.Forms.Button button_OpenWithDefault;

          protected override void Dispose(bool disposing)
          {
              if (disposing && (components != null))
              {
                  components.Dispose();
              }
              base.Dispose(disposing);
          }

          private void InitializeComponent()
          {
              this.menuStrip1 = new System.Windows.Forms.MenuStrip();
              this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
              this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
              this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
              this.textBox_FileContent = new System.Windows.Forms.TextBox();
              this.trackBar_Zoom = new System.Windows.Forms.TrackBar();
              this.button_OpenWithDefault = new System.Windows.Forms.Button();
              this.menuStrip1.SuspendLayout();
              ((System.ComponentModel.ISupportInitialize)(this.trackBar_Zoom)).BeginInit();
              this.SuspendLayout();

              // menuStrip1
              this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
          this.fileToolStripMenuItem});
              this.menuStrip1.Location = new System.Drawing.Point(0, 0);
              this.menuStrip1.Name = "menuStrip1";
              this.menuStrip1.Size = new System.Drawing.Size(800, 24);
              this.menuStrip1.TabIndex = 0;
              this.menuStrip1.Text = "menuStrip1";

              // fileToolStripMenuItem
              this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
          this.openToolStripMenuItem,
          this.saveToolStripMenuItem});
              this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
              this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
              this.fileToolStripMenuItem.Text = "File";

              // openToolStripMenuItem
              this.openToolStripMenuItem.Name = "openToolStripMenuItem";
              this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
              this.openToolStripMenuItem.Text = "Open";
              this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);

              // saveToolStripMenuItem
              this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
              this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
              this.saveToolStripMenuItem.Text = "Save";
              this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);

              // textBox_FileContent
              this.textBox_FileContent.Location = new System.Drawing.Point(12, 27);
              this.textBox_FileContent.Multiline = true;
              this.textBox_FileContent.Name = "textBox_FileContent";
              this.textBox_FileContent.Size = new System.Drawing.Size(776, 360);
              this.textBox_FileContent.TabIndex = 1;

              // trackBar_Zoom
              this.trackBar_Zoom.Location = new System.Drawing.Point(12, 393);
              this.trackBar_Zoom.Minimum = 8;
              this.trackBar_Zoom.Maximum = 24;
              this.trackBar_Zoom.Value = 12;
              this.trackBar_Zoom.Scroll += new System.EventHandler(this.trackBar_Zoom_Scroll);

              // button_OpenWithDefault
              this.button_OpenWithDefault.Location = new System.Drawing.Point(713, 393);
              this.button_OpenWithDefault.Name = "button_OpenWithDefault";
              this.button_OpenWithDefault.Size = new System.Drawing.Size(75, 23);
              this.button_OpenWithDefault.TabIndex = 3;
              this.button_OpenWithDefault.Text = "Open With";
              this.button_OpenWithDefault.UseVisualStyleBackColor = true;
              this.button_OpenWithDefault.Click += new System.EventHandler(this.button_OpenWithDefault_Click);

              // Form1
              this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
              this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
              this.ClientSize = new System.Drawing.Size(800, 450);
              this.Controls.Add(this.button_OpenWithDefault);
              this.Controls.Add(this.trackBar_Zoom);
              this.Controls.Add(this.textBox_FileContent);
              this.Controls.Add(this.menuStrip1);
              this.MainMenuStrip = this.menuStrip1;
              this.Name = "Form1";
              this.Text = "File Editor";
              this.menuStrip1.ResumeLayout(false);
              this.menuStrip1.PerformLayout();
              ((System.ComponentModel.ISupportInitialize)(this.trackBar_Zoom)).EndInit();
              this.ResumeLayout(false);
              this.PerformLayout();
          }
      }
  */

/* 3rd
     partial class Form1
     {
         private System.ComponentModel.IContainer components = null;
         private System.Windows.Forms.MenuStrip menuStrip1;
         private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
         private System.Windows.Forms.ToolStripMenuItem openFilesToolStripMenuItem;
         private System.Windows.Forms.ListBox listBox_Files;
         private System.Windows.Forms.Button button_OpenWithDefault;

         protected override void Dispose(bool disposing)
         {
             if (disposing && (components != null))
             {
                 components.Dispose();
             }
             base.Dispose(disposing);
         }

         private void InitializeComponent()
         {
             this.menuStrip1 = new System.Windows.Forms.MenuStrip();
             this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
             this.openFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
             this.listBox_Files = new System.Windows.Forms.ListBox();
             this.button_OpenWithDefault = new System.Windows.Forms.Button();
             this.menuStrip1.SuspendLayout();
             this.SuspendLayout();

             // menuStrip1
             this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.fileToolStripMenuItem});
             this.menuStrip1.Location = new System.Drawing.Point(0, 0);
             this.menuStrip1.Name = "menuStrip1";
             this.menuStrip1.Size = new System.Drawing.Size(800, 24);
             this.menuStrip1.TabIndex = 0;
             this.menuStrip1.Text = "menuStrip1";

             // fileToolStripMenuItem
             this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
         this.openFilesToolStripMenuItem});
             this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
             this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
             this.fileToolStripMenuItem.Text = "File";

             // openFilesToolStripMenuItem
             this.openFilesToolStripMenuItem.Name = "openFilesToolStripMenuItem";
             this.openFilesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
             this.openFilesToolStripMenuItem.Text = "Open Files";
             this.openFilesToolStripMenuItem.Click += new System.EventHandler(this.openFilesToolStripMenuItem_Click);

             // listBox_Files
             this.listBox_Files.FormattingEnabled = true;
             this.listBox_Files.Location = new System.Drawing.Point(12, 27);
             this.listBox_Files.Name = "listBox_Files";
             this.listBox_Files.Size = new System.Drawing.Size(776, 368);
             this.listBox_Files.TabIndex = 1;

             // button_OpenWithDefault
             this.button_OpenWithDefault.Location = new System.Drawing.Point(713, 401);
             this.button_OpenWithDefault.Name = "button_OpenWithDefault";
             this.button_OpenWithDefault.Size = new System.Drawing.Size(75, 23);
             this.button_OpenWithDefault.TabIndex = 2;
             this.button_OpenWithDefault.Text = "Open With";
             this.button_OpenWithDefault.UseVisualStyleBackColor = true;
             this.button_OpenWithDefault.Click += new System.EventHandler(this.button_OpenWithDefault_Click);

             // Form1
             this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
             this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
             this.ClientSize = new System.Drawing.Size(800, 450);
             this.Controls.Add(this.button_OpenWithDefault);
             this.Controls.Add(this.listBox_Files);
             this.Controls.Add(this.menuStrip1);
             this.MainMenuStrip = this.menuStrip1;
             this.Name = "Form1";
             this.Text = "File Explorer";
             this.menuStrip1.ResumeLayout(false);
             this.menuStrip1.PerformLayout();
             this.ResumeLayout(false);
             this.PerformLayout();
         }
     }
 */

/* this one is working good but it doesn't containt the transfert event
partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ListBox listBox_Files;
    private System.Windows.Forms.Button button_OpenWithDefault;
    private System.Windows.Forms.TextBox textBox_FolderPath;
    private System.Windows.Forms.Button button_LoadFiles;
    private System.Windows.Forms.Label label1;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.listBox_Files = new System.Windows.Forms.ListBox();
        this.button_OpenWithDefault = new System.Windows.Forms.Button();
        this.textBox_FolderPath = new System.Windows.Forms.TextBox();
        this.button_LoadFiles = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.menuStrip1.SuspendLayout();
        this.SuspendLayout();

        // menuStrip1
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.fileToolStripMenuItem});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(800, 24);
        this.menuStrip1.TabIndex = 0;
        this.menuStrip1.Text = "menuStrip1";

        // fileToolStripMenuItem
        this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
        this.fileToolStripMenuItem.Text = "File";

        // listBox_Files
        this.listBox_Files.FormattingEnabled = true;
        this.listBox_Files.Location = new System.Drawing.Point(12, 80);
        this.listBox_Files.Name = "listBox_Files";
        this.listBox_Files.Size = new System.Drawing.Size(776, 329);
        this.listBox_Files.TabIndex = 1;

        // button_OpenWithDefault
        this.button_OpenWithDefault.Location = new System.Drawing.Point(713, 415);
        this.button_OpenWithDefault.Name = "button_OpenWithDefault";
        this.button_OpenWithDefault.Size = new System.Drawing.Size(75, 23);
        this.button_OpenWithDefault.TabIndex = 2;
        this.button_OpenWithDefault.Text = "Open With";
        this.button_OpenWithDefault.UseVisualStyleBackColor = true;
        this.button_OpenWithDefault.Click += new System.EventHandler(this.button_OpenWithDefault_Click);

        // textBox_FolderPath
        this.textBox_FolderPath.Location = new System.Drawing.Point(102, 27);
        this.textBox_FolderPath.Name = "textBox_FolderPath";
        this.textBox_FolderPath.Size = new System.Drawing.Size(600, 20);
        this.textBox_FolderPath.TabIndex = 3;

        // button_LoadFiles
        this.button_LoadFiles.Location = new System.Drawing.Point(713, 25);
        this.button_LoadFiles.Name = "button_LoadFiles";
        this.button_LoadFiles.Size = new System.Drawing.Size(75, 23);
        this.button_LoadFiles.TabIndex = 4;
        this.button_LoadFiles.Text = "Load Files";
        this.button_LoadFiles.UseVisualStyleBackColor = true;
        this.button_LoadFiles.Click += new System.EventHandler(this.button_LoadFiles_Click);

        // label1
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(12, 30);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(84, 13);
        this.label1.TabIndex = 5;
        this.label1.Text = "Select Folder:";

        // Form1
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.button_LoadFiles);
        this.Controls.Add(this.textBox_FolderPath);
        this.Controls.Add(this.button_OpenWithDefault);
        this.Controls.Add(this.listBox_Files);
        this.Controls.Add(this.menuStrip1);
        this.MainMenuStrip = this.menuStrip1;
        this.Name = "Form1";
        this.Text = "File Explorer";
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
*/

/*
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBox_Files;
        private System.Windows.Forms.Button button_LoadFiles;
        private System.Windows.Forms.Button button_OpenWith;
        private System.Windows.Forms.Button button_CreateFile;
        private System.Windows.Forms.Button button_DeleteFile;
        private System.Windows.Forms.Button button_TransferFile;
        private System.Windows.Forms.Button button_TransferOut;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBox_Files = new System.Windows.Forms.ListBox();
            this.button_LoadFiles = new System.Windows.Forms.Button();
            this.button_OpenWith = new System.Windows.Forms.Button();
            this.button_CreateFile = new System.Windows.Forms.Button();
            this.button_DeleteFile = new System.Windows.Forms.Button();
            this.button_TransferFile = new System.Windows.Forms.Button();
            this.button_TransferOut = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // listBox_Files
            // 
            this.listBox_Files.FormattingEnabled = true;
            this.listBox_Files.ItemHeight = 16;
            this.listBox_Files.Location = new System.Drawing.Point(12, 12);
            this.listBox_Files.Name = "listBox_Files";
            this.listBox_Files.Size = new System.Drawing.Size(776, 324);
            this.listBox_Files.TabIndex = 0;

            // 
            // button_LoadFiles
            // 
            this.button_LoadFiles.Location = new System.Drawing.Point(12, 350);
            this.button_LoadFiles.Name = "button_LoadFiles";
            this.button_LoadFiles.Size = new System.Drawing.Size(75, 23);
            this.button_LoadFiles.TabIndex = 1;
            this.button_LoadFiles.Text = "Load Files";
            this.button_LoadFiles.UseVisualStyleBackColor = true;
            this.button_LoadFiles.Click += new System.EventHandler(this.button_LoadFiles_Click);

            // 
            // button_OpenWith
            // 
            this.button_OpenWith.Location = new System.Drawing.Point(93, 350);
            this.button_OpenWith.Name = "button_OpenWith";
            this.button_OpenWith.Size = new System.Drawing.Size(75, 23);
            this.button_OpenWith.TabIndex = 2;
            this.button_OpenWith.Text = "Open With";
            this.button_OpenWith.UseVisualStyleBackColor = true;
            this.button_OpenWith.Click += new System.EventHandler(this.button_OpenWithDefault_Click);

            // 
            // button_CreateFile
            // 
            this.button_CreateFile.Location = new System.Drawing.Point(174, 350);
            this.button_CreateFile.Name = "button_CreateFile";
            this.button_CreateFile.Size = new System.Drawing.Size(75, 23);
            this.button_CreateFile.TabIndex = 3;
            this.button_CreateFile.Text = "Create File";
            this.button_CreateFile.UseVisualStyleBackColor = true;
            this.button_CreateFile.Click += new System.EventHandler(this.button_CreateFile_Click);

            // 
            // button_DeleteFile
            // 
            this.button_DeleteFile.Location = new System.Drawing.Point(255, 350);
            this.button_DeleteFile.Name = "button_DeleteFile";
            this.button_DeleteFile.Size = new System.Drawing.Size(75, 23);
            this.button_DeleteFile.TabIndex = 4;
            this.button_DeleteFile.Text = "Delete File";
            this.button_DeleteFile.UseVisualStyleBackColor = true;
            this.button_DeleteFile.Click += new System.EventHandler(this.button_DeleteFile_Click);

            // 
            // button_TransferFile
            // 
            this.button_TransferFile.Location = new System.Drawing.Point(336, 350);
            this.button_TransferFile.Name = "button_TransferFile";
            this.button_TransferFile.Size = new System.Drawing.Size(107, 23);
            this.button_TransferFile.TabIndex = 5;
            this.button_TransferFile.Text = "Transfer to Folder";
            this.button_TransferFile.UseVisualStyleBackColor = true;
            this.button_TransferFile.Click += new System.EventHandler(this.button_TransferFile_Click);

            // 
            // button_TransferOut
            // 
            this.button_TransferOut.Location = new System.Drawing.Point(449, 350);
            this.button_TransferOut.Name = "button_TransferOut";
            this.button_TransferOut.Size = new System.Drawing.Size(131, 23);
            this.button_TransferOut.TabIndex = 6;
            this.button_TransferOut.Text = "Transfer from Folder copy outside";
            this.button_TransferOut.UseVisualStyleBackColor = true;
            this.button_TransferOut.Click += new System.EventHandler(this.button_TransferOut_Click);

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_TransferOut);
            this.Controls.Add(this.button_TransferFile);
            this.Controls.Add(this.button_DeleteFile);
            this.Controls.Add(this.button_CreateFile);
            this.Controls.Add(this.button_OpenWith);
            this.Controls.Add(this.button_LoadFiles);
            this.Controls.Add(this.listBox_Files);
            this.Name = "Form1";
            this.Text = "File Explorer App";
            this.ResumeLayout(false);

        }
    }
*/

//}



namespace App
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.listBox_Files = new System.Windows.Forms.ListBox();
            this.button_CreateFile = new System.Windows.Forms.Button();
            this.button_TransferFile = new System.Windows.Forms.Button();
            this.button_TransferOut = new System.Windows.Forms.Button();
            this.button_DeleteFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_Files
            // 
            this.listBox_Files.FormattingEnabled = true;
            this.listBox_Files.Location = new System.Drawing.Point(64, 47);
            this.listBox_Files.Name = "listBox_Files";
            this.listBox_Files.Size = new System.Drawing.Size(380, 251);
            this.listBox_Files.TabIndex = 0;
            this.listBox_Files.SelectedIndexChanged += new System.EventHandler(this.listBox_Files_SelectedIndexChanged);
            // 
            // button_CreateFile
            // 
            this.button_CreateFile.Location = new System.Drawing.Point(459, 65);
            this.button_CreateFile.Name = "button_CreateFile";
            this.button_CreateFile.Size = new System.Drawing.Size(85, 35);
            this.button_CreateFile.TabIndex = 1;
            this.button_CreateFile.Text = "Create File";
            this.button_CreateFile.UseVisualStyleBackColor = true;
            this.button_CreateFile.Click += new System.EventHandler(this.button_CreateFile_Click);
            // 
            // button_TransferFile
            // 
            this.button_TransferFile.Location = new System.Drawing.Point(459, 159);
            this.button_TransferFile.Name = "button_TransferFile";
            this.button_TransferFile.Size = new System.Drawing.Size(85, 35);
            this.button_TransferFile.TabIndex = 2;
            this.button_TransferFile.Text = "Transfer In";
            this.button_TransferFile.UseVisualStyleBackColor = true;
            this.button_TransferFile.Click += new System.EventHandler(this.button_TransferFile_Click);
            // 
            // button_TransferOut
            // 
            this.button_TransferOut.Location = new System.Drawing.Point(459, 215);
            this.button_TransferOut.Name = "button_TransferOut";
            this.button_TransferOut.Size = new System.Drawing.Size(85, 35);
            this.button_TransferOut.TabIndex = 3;
            this.button_TransferOut.Text = "Transfer Out";
            this.button_TransferOut.UseVisualStyleBackColor = true;
            this.button_TransferOut.Click += new System.EventHandler(this.button_TransferOut_Click);
            // 
            // button_DeleteFile
            // 
            this.button_DeleteFile.Location = new System.Drawing.Point(459, 107);
            this.button_DeleteFile.Name = "button_DeleteFile";
            this.button_DeleteFile.Size = new System.Drawing.Size(85, 35);
            this.button_DeleteFile.TabIndex = 4;
            this.button_DeleteFile.Text = "Delete File";
            this.button_DeleteFile.UseVisualStyleBackColor = true;
            this.button_DeleteFile.Click += new System.EventHandler(this.button_DeleteFile_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(589, 355);
            this.Controls.Add(this.button_TransferOut);
            this.Controls.Add(this.button_TransferFile);
            this.Controls.Add(this.button_CreateFile);
            this.Controls.Add(this.button_DeleteFile);
            this.Controls.Add(this.listBox_Files);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Files;
        private System.Windows.Forms.Button button_CreateFile;
        private System.Windows.Forms.Button button_TransferFile;
        private System.Windows.Forms.Button button_TransferOut;
        private System.Windows.Forms.Button button_DeleteFile;
    }
}









