using NewApp.Properties;
using System.Drawing;
using System.Resources;

namespace NewApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listView_Files = new System.Windows.Forms.ListView();
            this.button_CreateDirectory = new System.Windows.Forms.Button();
            this.button_Delete = new System.Windows.Forms.Button();
            this.button_TransferFile = new System.Windows.Forms.Button();
            this.button_TransferOut = new System.Windows.Forms.Button();
            this.button_CreateFile = new System.Windows.Forms.Button();
            this.button_Back = new System.Windows.Forms.Button();
            this.comboBox_ViewType = new System.Windows.Forms.ComboBox();
            this.button_Rename = new System.Windows.Forms.Button();
            this.label_CurrentPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView_Files
            // 
            this.listView_Files.HideSelection = false;
            this.listView_Files.Location = new System.Drawing.Point(12, 38);
            this.listView_Files.Name = "listView_Files";
            this.listView_Files.Size = new System.Drawing.Size(550, 300);
            this.listView_Files.TabIndex = 0;
            this.listView_Files.UseCompatibleStateImageBehavior = false;
            this.listView_Files.View = System.Windows.Forms.View.Details;
            this.listView_Files.SelectedIndexChanged += new System.EventHandler(this.listView_Files_SelectedIndexChanged);
            this.listView_Files.DoubleClick += new System.EventHandler(this.listView_Files_DoubleClick);
            // 
            // button_CreateDirectory
            // 
            this.button_CreateDirectory.Image = ((System.Drawing.Image)(resources.GetObject("button_CreateDirectory.Image")));
            this.button_CreateDirectory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_CreateDirectory.Location = new System.Drawing.Point(571, 38);
            this.button_CreateDirectory.Name = "button_CreateDirectory";
            this.button_CreateDirectory.Size = new System.Drawing.Size(118, 35);
            this.button_CreateDirectory.TabIndex = 1;
            this.button_CreateDirectory.Text = "Create Directory";
            this.button_CreateDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_CreateDirectory.UseVisualStyleBackColor = true;
            this.button_CreateDirectory.Click += new System.EventHandler(this.button_CreateDirectory_Click);
            // 
            // button_Delete
            // 
            this.button_Delete.Image = ((System.Drawing.Image)(resources.GetObject("button_Delete.Image")));
            this.button_Delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Delete.Location = new System.Drawing.Point(571, 79);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(118, 35);
            this.button_Delete.TabIndex = 2;
            this.button_Delete.Text = "Delete ";
            this.button_Delete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // button_TransferFile
            // 
            this.button_TransferFile.Image = ((System.Drawing.Image)(resources.GetObject("button_TransferFile.Image")));
            this.button_TransferFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_TransferFile.Location = new System.Drawing.Point(571, 120);
            this.button_TransferFile.Name = "button_TransferFile";
            this.button_TransferFile.Size = new System.Drawing.Size(118, 35);
            this.button_TransferFile.TabIndex = 3;
            this.button_TransferFile.Text = "Transfer In";
            this.button_TransferFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_TransferFile.UseVisualStyleBackColor = true;
            this.button_TransferFile.Click += new System.EventHandler(this.button_TransferFile_Click);
            // 
            // button_TransferOut
            // 
            this.button_TransferOut.Image = ((System.Drawing.Image)(resources.GetObject("button_TransferOut.Image")));
            this.button_TransferOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_TransferOut.Location = new System.Drawing.Point(571, 161);
            this.button_TransferOut.Name = "button_TransferOut";
            this.button_TransferOut.Size = new System.Drawing.Size(118, 35);
            this.button_TransferOut.TabIndex = 4;
            this.button_TransferOut.Text = "Transfer Out";
            this.button_TransferOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_TransferOut.UseVisualStyleBackColor = true;
            this.button_TransferOut.Click += new System.EventHandler(this.button_TransferOut_Click);
            // 
            // button_CreateFile
            // 
            this.button_CreateFile.Image = ((System.Drawing.Image)(resources.GetObject("button_CreateFile.Image")));
            this.button_CreateFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_CreateFile.Location = new System.Drawing.Point(571, 202);
            this.button_CreateFile.Name = "button_CreateFile";
            this.button_CreateFile.Size = new System.Drawing.Size(118, 35);
            this.button_CreateFile.TabIndex = 1;
            this.button_CreateFile.Text = "Create File";
            this.button_CreateFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_CreateFile.UseVisualStyleBackColor = true;
            this.button_CreateFile.Click += new System.EventHandler(this.button_CreateFile_Click);
            // 
            // button_Back
            // 
            this.button_Back.Image = ((System.Drawing.Image)(resources.GetObject("button_Back.Image")));
            this.button_Back.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Back.Location = new System.Drawing.Point(12, 12);
            this.button_Back.Name = "button_Back";
            this.button_Back.Size = new System.Drawing.Size(67, 23);
            this.button_Back.TabIndex = 5;
            this.button_Back.Text = "back";
            this.button_Back.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_Back.UseVisualStyleBackColor = true;
            this.button_Back.UseWaitCursor = true;
            this.button_Back.Click += new System.EventHandler(this.button_Back_Click);
            // 
            // comboBox_ViewType
            // 
            this.comboBox_ViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ViewType.FormattingEnabled = true;
            this.comboBox_ViewType.Items.AddRange(new object[] {
            "List",
            "Grid"});
            this.comboBox_ViewType.Location = new System.Drawing.Point(82, 12);
            this.comboBox_ViewType.Name = "comboBox_ViewType";
            this.comboBox_ViewType.Size = new System.Drawing.Size(55, 24);
            this.comboBox_ViewType.TabIndex = 6;
            this.comboBox_ViewType.SelectedIndexChanged += new System.EventHandler(this.comboBox_ViewType_SelectedIndexChanged);
            // 
            // button_Rename
            // 
            this.button_Rename.Image = ((System.Drawing.Image)(resources.GetObject("button_Rename.Image")));
            this.button_Rename.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Rename.Location = new System.Drawing.Point(571, 243);
            this.button_Rename.Name = "button_Rename";
            this.button_Rename.Size = new System.Drawing.Size(118, 35);
            this.button_Rename.TabIndex = 7;
            this.button_Rename.Text = "Rename";
            this.button_Rename.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_Rename.UseVisualStyleBackColor = true;
            this.button_Rename.Click += new System.EventHandler(this.button_Rename_Click);
            // 
            // label_CurrentPath
            // 
            this.label_CurrentPath.AutoSize = true;
            this.label_CurrentPath.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label_CurrentPath.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label_CurrentPath.Location = new System.Drawing.Point(149, 16);
            this.label_CurrentPath.Name = "label_CurrentPath";
            this.label_CurrentPath.Size = new System.Drawing.Size(10, 16);
            this.label_CurrentPath.TabIndex = 0;
            this.label_CurrentPath.Text = " ";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(708, 370);
            this.Controls.Add(this.button_Rename);
            this.Controls.Add(this.comboBox_ViewType);
            this.Controls.Add(this.button_Back);
            this.Controls.Add(this.button_TransferOut);
            this.Controls.Add(this.button_TransferFile);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.button_CreateDirectory);
            this.Controls.Add(this.button_CreateFile);
            this.Controls.Add(this.listView_Files);
            this.Controls.Add(this.label_CurrentPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.ListView listView_Files;
            private System.Windows.Forms.Button button_CreateDirectory;
            private System.Windows.Forms.Button button_Delete;
            private System.Windows.Forms.Button button_TransferFile;
            private System.Windows.Forms.Button button_TransferOut;
            private System.Windows.Forms.Button button_CreateFile;
            private System.Windows.Forms.Button button_Back;
            private System.Windows.Forms.ComboBox comboBox_ViewType;
            private System.Windows.Forms.Button button_Rename;
            private System.Windows.Forms.Label label_CurrentPath;
    }
    

}

