using System.Data.Entity;
using System.IO;
using DAL.Migrations;
using WebUI.Extensions;

namespace WebUI.App_Start
{
    public class AppConfiguration
    {
        public static void ConfigureAutomapper()
        {
            AutomapperConfig.Configure();
        }

        public static void ConfigureJsonConverter()
        {
            JsonConverterConfig.Configure();
        }

        public static void ConfigureDirrectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void ConfigureDatabaseInitializer()
        {
            Database.SetInitializer(new BOTContextInitializer());
        }

        public static void ConfigureMailAgent(string email, string password)
        {
            Mailer.MailAgent
                .UseDefaultConfiguration()
                .UseSender(email)
                .UsePassword(password);
        }
    }
}