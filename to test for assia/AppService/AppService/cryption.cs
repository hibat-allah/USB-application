using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AppService
{
    public class cryption
    {
        static string logFilePath2 = @"C:\Users\user\Documents\Log3.txt"; // Update with an appropriate path



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
                Log("decryption : outputFile " + outputFilePath + " path2 " + outputFilePath2);
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

    }
}
