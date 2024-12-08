using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace RestartDevice
{
    class Restart
    {
        public void restartDev(string Instance)
        {
                
            try
            {
                // Modifier la valeur de la clé de registre HIDUSB à 3
               // Redémarrer le périphérique avec pnputil
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false
                };

                Process process = Process.Start(psi);
                if (process != null)
                {
                   process.StandardInput.WriteLine("pnputil /restart-device \""+Instance+"\"");

                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                    process.WaitForExit();
                }

                Console.WriteLine("Le périphérique a été redémarré avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}