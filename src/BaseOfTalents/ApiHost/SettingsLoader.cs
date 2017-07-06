using System.IO;
using DAL;
using Newtonsoft.Json;
using WebUI.Globals;

namespace ApiHost
{
    class JsonSettingsLoader : ISettingsLoader
    {
        private class Settings
        {
            public string Url { get; set; }
            public int Port { get; set; }

            public string Email { get; set; }
            public string Password { get; set; }

            public string DbInitialCatalog { get; set; }
            public string DbDataSource { get; set; }
            public string UserId { get; set; }
            public string UserPassword { get; set; }

            public string FrAccessUrl { get; set; }
        }

        public void Load(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"{fileName} with application settings doesn't exist");
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                string json = reader.ReadToEnd();
                var settings = JsonConvert.DeserializeObject<Settings>(json);
                SettingsContext.SetInstance(settings.Url, settings.FrAccessUrl, settings.Port, settings.Email, settings.Password);
                DbSettingsContext.SetInstance(settings.DbInitialCatalog, settings.DbDataSource, settings.UserId, settings.UserPassword);
            }
        }
    }
}