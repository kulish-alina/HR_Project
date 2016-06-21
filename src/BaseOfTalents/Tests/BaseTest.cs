using BaseOfTalents.DAL;
using BaseOfTalents.WebUI;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Web.Http;

namespace Tests
{
    public class BaseTest
    {
        protected HttpConfiguration httpConf;
        protected BOTContext context;

        [OneTimeSetUp]
        public void GlobalInit()
        {
            System.Diagnostics.Debug.WriteLine("OneTimeSetUp");

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string connectionString = @"Data Source=" + path + @"\BoTTestData.sdf;Persist Security Info=False;";
            context = new BOTContext(connectionString);
            httpConf = new HttpConfiguration();
            WebApiConfig.Register(httpConf);
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            System.Diagnostics.Debug.WriteLine("OneTimeTearDown");
            context.Delete();
        }

    }
}
