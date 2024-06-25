using NewApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AppService
{
    public static class Program
    {
        static string logFilePath = @"C:\Users\user\Documents\startingAppService.txt"; // Update with an appropriate path

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //start app of the password 
            // need to check if it's first added or not
            Log("Application starting...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Log("ip db" + ServerConfiguration.IPDB());
            Log("before db");
            bool connected = BD.IsFirstTimeLogin();
            Log("conn " + connected);
            if (!connected)
            {
                using (var passwordForm = new PasswordValidationForm())
                {
                    Log("here i am in the password form");
                   Application.Run(passwordForm);

                }
            //Application.Run(new PasswordValidationForm());
            }

            //start the service

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            // Create and hide the main form

            Log("starting the app...");
            var mainForm = new Form1();
            mainForm.Visible = false;
            Application.Run();
        }

        public static void Log(string message)
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
