using System.Net.Http;
using System.Web.Http;
using Domain.DTO.DTOModels;
using Service.Services;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Net;
using System;
using Service.Extentions;
using System.Net.Http.Headers;
using System.IO;
using System.Linq;

namespace WebApi.Controllers
{
    public class FilesController : ApiController
    {
        FileService fileService;
        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public FilesController(FileService service)
        {
            this.fileService = service;
        }

        [HttpPost]
        [Route("api/files")]
        public async Task<IHttpActionResult> Upload()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var paths = GetUploadPath();

                if (!Directory.Exists(paths.Item1))
                {
                    Directory.CreateDirectory(paths.Item1);
                }

                var uploadProvider = new UploadMultipartFormProvider(paths.Item1);
                var result = await Request.Content.ReadAsMultipartAsync(uploadProvider);
                var originalFileName = GetDeserializedFileName(result.FileData.First());

                var file = new Domain.Entities.File { Description = originalFileName, FilePath = paths.Item2 + result.FileData.First().LocalFileName.Replace(paths.Item1,"") };

                var fileResult = fileService.Add(file);
                fileResult.FileSize = new FileInfo(result.FileData.First().LocalFileName).Length;

                return Json(fileResult, BOT_SERIALIZER_SETTINGS);
            }
            catch (System.Exception e)
            {

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }
        public string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }

        private Tuple<string,string> GetUploadPath()
        {
            var root = @"~/Uploads";
            var year = DateTime.Now.Year;
            var week = DateTime.Now.GetIso8601WeekOfYear();

            return new Tuple<string, string>(
                string.Format(@"{0}\{1}\{2}\", HttpContext.Current.Server.MapPath(root),year,week),
                string.Format(@"{0}/{1}/{2}/", root, year, week)
                );
        }

        private class UploadMultipartFormProvider : MultipartFormDataStreamProvider
        {
            string path;

            public UploadMultipartFormProvider(string path) : base(path)
            {
                this.path = path;
            }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                if (headers != null && headers.ContentDisposition != null)
                {
                    return Directory.GetFiles(RootPath).Length +
                        headers.ContentDisposition.FileName.TrimEnd('"').TrimStart('"');
                }

                return base.GetLocalFileName(headers);
            }
        }
    }
}