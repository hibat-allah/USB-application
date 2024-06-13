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
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("servers.json")
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
