using DAL.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using WebUI.Extensions;
using WebUI.Globals;
using WebUI.Models;

namespace WebUI.Controllers
{
    [RoutePrefix("api/CVParser")]
    public class CVParserController : ApiController
    {
        CVParserService cvParserService;

        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public CVParserController(CVParserService service)
        {
            this.cvParserService = service;
        }

        public CVParserController()
        {

        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Parse([FromBody]CVPathModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Json(cvParserService.Parse($"{SettingsContext.Instance.GetRootPath()}//{model.Path}"), BOT_SERIALIZER_SETTINGS);
        }
    }
}
