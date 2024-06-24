using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
namespace NewApp
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

        static ServerConfiguration()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
           
            //string configFilePath = Path.Combine(currentDirectory, "servers.json");
            // Get the directory of the executing assembly (the .exe file)
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string configFilePath = Path.Combine(exeDirectory, "servers.json");
            
            if (!File.Exists(configFilePath))
            {
               
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
    }

}
