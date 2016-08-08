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

            public string WWWRoot { get; set; }
            public string Uploads { get; set; }

            public string DbInitialCatalog { get; set; }
            public string DbDataSource { get; set; }
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
                SettingsContext.SetInstance(settings.Url, settings.Port, settings.WWWRoot,
                    settings.Uploads, settings.Email, settings.Password);
                DbSettingsContext.SetInstance(settings.DbInitialCatalog, settings.DbDataSource);
            }
        }
    }
}