using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;

namespace blocage

{
    public class BD
    {
        //static string connectionString = "Host=192.168.1.133;Port=5432;Username=zflub;Password=M#N@d31N;Database=entreprisebd";
        private static string connectionString = "Host=" + ServerConfiguration.IPDB() + ";Port=5432;Username=zflub;Password=M#N@d31N;Database=entreprisebd";

        public static void CheckMachineTypeAndPerformAction()
        {
            string machineName = Environment.MachineName;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT typeM FROM Machine WHERE nameM = @machineName";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@machineName", machineName);
                        string typeM = command.ExecuteScalar()?.ToString();

                        if (typeM == "complet")
                        {
                            // Call the functions for 'complet' type
                            //FunctionForCompletType();
                        }
                        else if (typeM == "selectif")
                        {
                            // Call the functions for 'selectif' type
                            //FunctionForSelectifType();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking machine type: {ex.Message}");
                // Handle the exception (e.g., show a message box)
            }
        }

        public static bool CheckDeviceForUser(string deviceId)
        {
            string userName = Environment.UserName;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Device_Users WHERE device_id = @deviceId AND user_id = @userName";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@deviceId", deviceId);
                        command.Parameters.AddWithValue("@userName", userName);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count == 0)
                        {
                            return false;
                        }
                        else
                        {
                            // Continue the code execution
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking device for user: {ex.Message}");
                // Handle the exception (e.g., show a message box)
                return false;
            }
        }

        public static bool CheckDeviceType(string deviceId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT EXISTS (
                    SELECT 1 
                    FROM Class c
                    JOIN Device d ON c.guid = d.classI_id
                    WHERE d.idDevice = @deviceId
                )";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@deviceId", deviceId);

                        bool exists = (bool)command.ExecuteScalar();

                        return exists;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking device type: {ex.Message}");
                // Handle the exception (e.g., show a message box)
                return false;
            }
        }

        public static bool CheckPassword(string password)
        {
            string userName = Environment.UserName;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT password FROM Users WHERE userName = @userName";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userName", userName);

                        string storedHash = command.ExecuteScalar()?.ToString();

                        return PasswordHasher.VerifyPassword(password, storedHash);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying user password: {ex.Message}");
                // Handle the exception (e.g., show a message box)
                return false;
            }
        }

        public static string GetDeviceInfFile(string deviceId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT inf FROM Device WHERE idDevice = @deviceId";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@deviceId", deviceId);

                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            Console.WriteLine($"No INF file found for device ID: {deviceId}");
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving INF file for device ID {deviceId}: {ex.Message}");
                return null;
            }
        }

        public static void ProcessAuthorizedUsbClasses()
        {
            try
            {
                /*
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT chemain, guid, isAuthorized FROM Class WHERE isAuthorized = true";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string chemin = reader["chemain"].ToString();
                                string guid = reader["guid"].ToString();
                                bool isAuthorized = (bool)reader["isAuthorized"];

                                if (isAuthorized)
                                {
                                    Blocking.ActivateStartValue(chemin);
                                    Blocking.AddToGpo(guid);
                                }
                            }
                        }
                    }
                }
            */
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT c.chemain, c.guid 
                FROM Class c
                JOIN Class_Users cu ON c.guid = cu.class_id
                JOIN Users u ON cu.user_id = u.userName
                WHERE u.userName = @currentUserName";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@currentUserName", Environment.UserName);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //string chemin = reader["chemain"].ToString();
                                //string guid = reader["guid"].ToString();
                                string fullPath = reader["chemain"].ToString();
                                string partialPath = fullPath.Replace("HKEY_LOCAL_MACHINE\\", "");
                                string guid = reader["guid"].ToString();

                                //Blocking.ActivateStartValue(partialPath);

                                Console.WriteLine("guid " + guid + "chemin " + partialPath);

                                //Blocking.ActivateStartValue(chemin);
                                //Blocking.AddToGpo(guid);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing USB classes: {ex.Message}");
            }
        }
    }
}
