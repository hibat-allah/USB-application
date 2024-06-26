using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppService
{
    public partial class password : Form
    {
      
            public string Password { get; private set; }

            public password()
            {
                InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += PasswordValidationForm_FormClosing;
        }

            private void okButton_Click(object sender, EventArgs e)
            {
                Password = passwordTextBox.Text;
                DialogResult = DialogResult.OK;
                this.FormClosing -= PasswordValidationForm_FormClosing; // Allow the form to close
                this.Close();
            
            }

          

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void password_Load(object sender, EventArgs e)
        {

        }

        private void PasswordValidationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Prevent the form from closing
                MessageBox.Show("You must enter a valid password to proceed.");
            }
        }
    }
    
}
