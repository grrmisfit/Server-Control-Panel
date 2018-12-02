using System.IO;
using Newtonsoft.Json;

namespace DediServerCron
{
    public class Config
    {

        private const string configFolder = "Config";
        private const string configFile = "config.json";

        public static ServerManConfig Smc;

        static Config()
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            if (!File.Exists(configFolder + "/" + configFile))
                //if
            {

                Smc = new ServerManConfig();
                string json = JsonConvert.SerializeObject(Smc, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                Smc = JsonConvert.DeserializeObject<ServerManConfig>(json);

            }
        }
        public static void SaveConfig()
        {
            var conf = Smc;
            var json = JsonConvert.SerializeObject(Smc, Formatting.Indented);
            File.WriteAllText(configFolder + "/" + configFile, json);
        }
    }
   
    public struct ServerManConfig
    {

        public string BatchFileLocation;
        public string ConfigFileLocation;
        public bool AutoRestart;
    }
}
