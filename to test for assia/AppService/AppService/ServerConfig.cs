using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
namespace AppService
{
    public class ServerConfig
    {
        public string Database { get; set; }
        public string ADCS { get; set; }
        public string Docker { get; set; }
    }

    public class ServerConfiguration
    {
        private static IConfigurationRoot configuration;
        private static string logFilePath = @"C:\Users\user\Documents\startingAppService.txt";

        static ServerConfiguration()
        {
            Log("Initializing ServerConfiguration...");

            string currentDirectory = Directory.GetCurrentDirectory();
            Log($"Current Directory: {currentDirectory}");

            //string configFilePath = Path.Combine(currentDirectory, "servers.json");
            // Get the directory of the executing assembly (the .exe file)
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configFilePath = Path.Combine(exeDirectory, "servers.json");
            Log($"Config File Path: {configFilePath}");

            if (!File.Exists(configFilePath))
            {
                Log("Error: Config file does not exist.");
                return;
            }
            configuration = new ConfigurationBuilder()
                .SetBasePath(exeDirectory)
                .AddJsonFile(configFilePath)
                .Build();
        }

        public static ServerConfig GetServerConfig()
        {
            return configuration.GetSection("Servers").Get<ServerConfig>();
        }
        public static string IPDB()
        {
            var serverConfig = ServerConfiguration.GetServerConfig();
            
            return serverConfig.Database;
        }
        public static string IPAD()
        {
            var serverConfig = ServerConfiguration.GetServerConfig();
            return serverConfig.ADCS;
        }
        public static string URLDocker()
        {
            var serverConfig = ServerConfiguration.GetServerConfig();
            return serverConfig.Docker;
        }
        private static void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }

}
