using System;
using DAL;
using Microsoft.Owin.Hosting;
using WebUI;
using WebUI.Globals;

namespace ApiHost
{
    public class Program
    {
        static void Main()
        {
            ISettingsLoader loader = new JsonSettingsLoader();
            try
            {
                loader.Load("deploy.json");
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message);
            }

            int port = SettingsContext.Instance.Port;
            string url = SettingsContext.Instance.HostUrl;

            StartOptions options = new StartOptions($"{url}:{port}")
            {
                ServerFactory = "Microsoft.Owin.Host.HttpListener"
            };

            using (WebApp.Start<ApiStartup>(options))
            {
                Console.WriteLine("Server started");
                Console.WriteLine($"\tMachine name: {Environment.MachineName}");
                Console.WriteLine($"\tUrl: {url}:{port}");
                Console.WriteLine($"\tDatabase name : {DbSettingsContext.Instance.DbInitialCatalog}");
#if DEBUG
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                var res = client.GetAsync($"http://localhost:{port}/api/tag").Result;
                Console.WriteLine($"{res.Content.ReadAsStringAsync().Result}");
#endif
                Console.WriteLine("Press ENTER to stop server...");
                Console.ReadLine();
            }
        }
    }
}