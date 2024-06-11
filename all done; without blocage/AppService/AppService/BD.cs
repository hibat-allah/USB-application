using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@userName", username);
                    connection.Open();

                    object result = command.ExecuteScalar();
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

}
}
