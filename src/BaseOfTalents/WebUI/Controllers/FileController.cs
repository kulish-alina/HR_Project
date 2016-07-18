using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using DAL.DTO;
using DAL.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Extensions;

namespace WebUI.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        FileService fileService;

        protected static JsonSerializerSettings BOT_SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public FileController(FileService service)
        {
            this.fileService = service;
        }

        public FileController()
        {

        }

        [HttpPost]
        [Route("RemoveGroup")]
        public IHttpActionResult RemoveGroup([FromBody]List<int> fileIds)
        {
            var deletedIds = new List<int>();
            fileIds.ForEach(fileId =>
            {
                if (fileService.Delete(fileId))
                {
                    deletedIds.Add(fileId);
                }
            });
            return Json(deletedIds, BOT_SERIALIZER_SETTINGS);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Remove(int id)
        {
            if (fileService.Delete(id))
            {
                return Json(new { Id = id }, BOT_SERIALIZER_SETTINGS);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("")]
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

                var file = new FileDTO
                {
                    Description = originalFileName,
                    FilePath = paths.Item2 + result.FileData.First().LocalFileName.Replace(paths.Item1, ""),
                    Size = new FileInfo(result.FileData.First().LocalFileName).Length
                };

                var uploadedFile = fileService.Add(file);

                return Json(uploadedFile, BOT_SERIALIZER_SETTINGS);
            }
            catch (Exception e)
            {

                throw new ApplicationException(e.Message);
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

        private Tuple<string, string> GetUploadPath()
        {
            var upload = @"uploads";
            var uploadsPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Globals.Constants.RootFolder, upload);
            var year = DateTime.Now.Year;
            var week = DateTime.Now.GetIso8601WeekOfYear();

            var first = $"{uploadsPath}\\{year}\\{week}";
            var second = $"{upload}/{year}/{week}";

            return Tuple.Create(first, second);
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
