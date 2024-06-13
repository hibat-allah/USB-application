using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AppService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //start app of the password 
            // need to check if it's first added or not
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool connected = BD.IsFirstTimeLogin();
            Console.WriteLine("conn " + connected);
            if (!connected)
            {
                using (var passwordForm = new PasswordValidationForm())
                {
                   Application.Run(passwordForm);
                }
            //Application.Run(new PasswordValidationForm());
            }

            //start the service

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            // Create and hide the main form
            
            
            var mainForm = new Form1();
            mainForm.Visible = false;
            Application.Run();
        }
    }
}
