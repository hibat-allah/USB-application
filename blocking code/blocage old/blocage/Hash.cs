using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace blocage
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            try
            {
                // Generate a salt
                byte[] salt = new byte[16];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                }

                // Combine the salt and password
                byte[] saltedPassword = Combine(salt, Encoding.UTF8.GetBytes(password));

                // Hash the salted password
                byte[] hash;
                using (var sha256 = SHA256.Create())
                {
                    hash = sha256.ComputeHash(saltedPassword);
                }

                // Combine the salt and hash for storage
                byte[] saltedHash = Combine(salt, hash);

                // Convert the salted hash to a base64 string
                return Convert.ToBase64String(saltedHash);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while hashing the password.");
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while generating the salt or hash.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw new Exception("An unexpected error occurred.");
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                // Convert the stored hash from a base64 string to a byte array
                byte[] saltedHash = Convert.FromBase64String(storedHash);

                // Ensure the stored hash is of the correct length
                if (saltedHash.Length != 16 + 32) // 16 bytes for the salt, 32 bytes for the SHA-256 hash
                {
                    throw new ArgumentException("Invalid length of the stored hash.");
                }

                // Extract the salt from the stored hash
                byte[] salt = new byte[16];
                Array.Copy(saltedHash, 0, salt, 0, salt.Length);

                // Hash the provided password with the extracted salt
                byte[] saltedPassword = Combine(salt, Encoding.UTF8.GetBytes(password));
                byte[] hash;
                using (var sha256 = SHA256.Create())
                {
                    hash = sha256.ComputeHash(saltedPassword);
                }

                // Combine the extracted salt and the new hash
                byte[] newSaltedHash = Combine(salt, hash);

                // Compare the new salted hash with the stored hash
                return CompareByteArrays(saltedHash, newSaltedHash);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new ArgumentException("The stored hash is not in a valid Base64 format.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new ArgumentException("Invalid stored hash format.");
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while hashing the password.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw new Exception("An unexpected error occurred.");
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            try
            {
                byte[] combined = new byte[first.Length + second.Length];
                Array.Copy(first, 0, combined, 0, first.Length);
                Array.Copy(second, 0, combined, first.Length, second.Length);
                return combined;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new ArgumentException("Input arrays cannot be null.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new ArgumentException("Input arrays have invalid lengths.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw new Exception("An unexpected error occurred.");
            }
        }

        private static bool CompareByteArrays(byte[] a, byte[] b)
        {
            try
            {
                if (a.Length != b.Length)
                    return false;

                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] != b[i])
                        return false;
                }

                return true;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new ArgumentException("Input arrays cannot be null.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw new Exception("An unexpected error occurred while comparing arrays.");
            }
        }
    }

}
