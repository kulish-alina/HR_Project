using System.IO;
using System.Reflection;
using System.Web.Http;
using DAL;
using NUnit.Framework;
using WebUI.App_Start;

namespace Tests
{
    public class BaseTest
    {
        protected HttpConfiguration httpConf;
        protected BOTContext context;

        protected BOTContext GenerateNewContext()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var connectionString = @"Data Source=" + path + @"\BoTTestData.sdf;Persist Security Info=False;";
            return new BOTContext(connectionString);
        }

        public BaseTest()
        {

        }

        [OneTimeSetUp]
        public void GlobalInit()
        {
            System.Diagnostics.Debug.WriteLine("OneTimeSetUp");
            httpConf = WebApiConfig.Create();
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            System.Diagnostics.Debug.WriteLine("OneTimeTearDown");
        }
    }
}
