using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewApp;
using Npgsql;

namespace AppService
{
    public class BD
    {
        //static string connectionString = "Host=192.168.1.133;Port=5432;Username=zflub;Password=M#N@d31N;Database=entreprisebd";
        private static string connectionString = "Host=" + ServerConfiguration.IPDB() + ";Port=5432;Username=zflub;Password=M#N@d31N;Database=entreprisebd";

        public static bool IsFirstTimeLogin()
        {
            string username=Environment.UserName;
            string query = "SELECT firstTime FROM Users WHERE username = @userName";
            try
            {
               // MessageBox.Show("users " + username );

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@userName", username);
                 //   MessageBox.Show("cnx open.... ");
                    connection.Open();
                   // MessageBox.Show("after ");
                    object result = command.ExecuteScalar();
                    //MessageBox.Show("users " + username+" firstTime "+ result);
                    if (result != null && result is bool)
                    {
                        //return (bool)result == false;
                        return (bool)result;
                    }

                    return false;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                // Log the exception (consider using a logging framework)
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                // Log the exception (consider using a logging framework)
                return false;
            }

        }
        
        public static void UpdateUserPassword(string username, string newPassword)
        {
            string query = "UPDATE Users SET password = @Password, firstTime = @FirstTime WHERE userName = @userName";
            try
            {
                string password = PasswordHasher.HashPassword(newPassword);
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@FirstTime", true);
                    command.Parameters.AddWithValue("@userName", username);
                    Console.WriteLine("updating the password " + password + " " + username);
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                // Log the exception (consider using a logging framework)
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                // Log the exception (consider using a logging framework)

            }
        }

        
        // chack the blocage status of the machine and call wchich function to use depending on it
        public static string CheckMachineTypeAndPerformAction()
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

                        return typeM;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking machine type: {ex.Message}");
                return null;
                // Handle the exception (e.g., show a message box)
            }
        }



        // check if the device is for the current user and return a boolean
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

        // check if the class of the device is allowed and is in the table Class in BD
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

        // check if the password given is the same as the password stored for the current user
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

        // return the inf file of the given device
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

        // Get the Guid and Chemain of the usb classes authorized to the current user and Add them in the regestry
        // this is the method that allow the classes by registries
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

                                Blocking.ActivateStartValue(partialPath,3);

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


        // this methid disable all classes ( blocage complet par registres)
        public static void BlockingClasses()
        {
            try
            {
              
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT c.chemain FROM Class";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                       
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fullPath = reader["chemain"].ToString();
                                string partialPath = fullPath.Replace("HKEY_LOCAL_MACHINE\\", "");
                                
                                Blocking.ActivateStartValue(partialPath,4);
                                LogHelper.Log("the blocking for all classes");
                               
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


        // Get the name of drivers that must be added to registries (gpo) with the connected device
        public static List<string> GetRelatedDeviceIds(string deviceId)
        {
            List<string> relatedDeviceIds = new List<string>();

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to fetch GUID based on deviceId
                    string query = "SELECT classi_id FROM Device WHERE idDevice = @deviceId";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@deviceId", deviceId);
                        var classi_id = cmd.ExecuteScalar();

                        // Query to fetch namedrives based on GUID
                        query = "SELECT nameDrives FROM DriverClass WHERE guid = @guid";

                        using (var cmd2 = new NpgsqlCommand(query, conn))
                        {
                            cmd2.Parameters.AddWithValue("@guid", classi_id);
                            using (var reader = cmd2.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    relatedDeviceIds.Add(reader.GetString(0));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching related device IDs: {ex.Message}");
            }

            return relatedDeviceIds;
        }

        // add classes to the gpo



    }
}
