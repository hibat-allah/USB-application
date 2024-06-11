using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewApp;
using Npgsql;

namespace AppService
{

    public static class LogHelper
    {
        //static string connectionString = "Host=192.168.1.133;Port=5432;Username=zflub;Password=M#N@d31N;Database=entreprisebd";
        private static string connectionString = "Host=" + ServerConfiguration.IPDB() + ";Port=5432;Username=zflub;Password=M#N@d31N;Database=entreprisebd";
        static string logFilePath = @"C:\Users\user\Documents\LogLogsService.txt"; // Update with an appropriate path

        public static void InsertLog(string userId, string actionType, string fileName, string filePath, string deviceId, string machineId, DateTime timestamp)
        {
            try
            {
                string machineName = Environment.MachineName;
                Log("machineName: " + machineName);
                deviceId = @"USB\\VID_058F&PID_6387";
                //using (var connection = new NpgsqlConnection(connectionString))
                //{
                //  connection.Open();
                    Log("connection opened");
                    Log("content: " + userId + " " + actionType + " " + fileName + " " + filePath + " " + deviceId);
               
                    string query = @"
                INSERT INTO logs (user_id, action_type, file_name, file_path, device_id, machine_id,timestamp)
                VALUES (@userId, @actionType, @fileName, @filePath, @deviceId, @machineId,@timestamp)";
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);

                    //using (var command = new NpgsqlCommand(query, connection))
                    //{
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@actionType", actionType);
                        command.Parameters.AddWithValue("@fileName", fileName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@filePath", filePath ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@deviceId", deviceId);
                        command.Parameters.AddWithValue("@machineId", machineName);
                    command.Parameters.AddWithValue("@timestamp", timestamp);

                    connection.Open();
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Log($"Error inserting log: {ex.Message}");
                        }
                    }
                //}
            }
            catch (NpgsqlException ex)
            {
                Log($"Database error: {ex.Message}");
                // Log the exception (consider using a logging framework)

            }
            catch (Exception ex)
            {
                Log($"Unexpected error: {ex.Message}");
                // Log the exception (consider using a logging framework)

            }
        }

        static void Log(string message)
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
