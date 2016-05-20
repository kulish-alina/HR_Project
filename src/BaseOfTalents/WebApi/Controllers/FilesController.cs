using System.Net.Http;
using System.Web.Http;
using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Repositories;
using System.Diagnostics;
using System.Web;
using System.Net;
using System.Net.Http.Headers;
using System;
using System.IO;
using WebApi.DTO.DTOService;
using System.Globalization;
using Data.EFData.Extentions;

namespace WebApi.Controllers
{
    public class FilesController : BoTController<Domain.Entities.File, FileDTO>
    {
        public FilesController(IDataRepositoryFactory repoFatory, IErrorRepository errorRepo)
            : base(repoFatory, errorRepo)
        {
        }

        [HttpPost]
        [Route("api/files")]
        public IHttpActionResult Add(HttpRequestMessage request)
        {
            var filesRepository = _repoFactory.GetDataRepository<Domain.Entities.File>(request);

            return CreateResponse(request, () =>
            {
               if (!Request.Content.IsMimeMultipartContent())
               {
                   throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
               }
               string savingPath = getSavingPath();
               createDirectoryIfDoesntExists(savingPath);
               var savedFile = readFileTo(savingPath);
               var domainFile = new Domain.Entities.File()
               {
                   FilePath = getPhotoPath(savedFile),
                   Description = getClearFileName(savedFile)
               };
               filesRepository.Add(domainFile);
               filesRepository.Commit();
               return Json(DTOService.ToDTO<Domain.Entities.File, FileDTO>(domainFile), BOT_SERIALIZER_SETTINGS);
            });
        }

        private MultipartFileData readFileTo(string savingPath)
        {
            var provider = new BoTMultipartDataStreamProvider(savingPath);
            Request.Content.ReadAsMultipartAsync(provider).ContinueWith((data) =>
            {
                if (data.IsFaulted)
                {
                    throw data.Exception;
                }
            });
            return provider.FileData[0];
        }

        private void createDirectoryIfDoesntExists(string savingPath)
        {
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }
        }

        private string getClearFileName(MultipartFileData savedFile)
        {
            return savedFile.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }

        private string getPhotoPath(MultipartFileData savedFile)
        {
            string serverPath = HttpContext.Current.Server.MapPath("~");
            var removedServerPath = savedFile.LocalFileName.Replace(serverPath, string.Empty);
            var replacedWrongSlashes = removedServerPath.Replace(@"\", @"/");
            return replacedWrongSlashes;
        }

        private string getSavingPath()
        {
            string rootPath = HttpContext.Current.Server.MapPath("~/Upload");
            string thisYearPath = rootPath + @"/" + DateTime.Now.Year;
            string finalPath = thisYearPath + @"/" + DateTime.Now.GetIso8601WeekOfYear();
            return finalPath;
        }
    }

    class BoTMultipartDataStreamProvider : MultipartFormDataStreamProvider
    {
        public BoTMultipartDataStreamProvider(string path) : base(path)
        {

        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            int uniqueFileIdentifier = Directory.GetFiles(RootPath).Length;
            string fileName = (uniqueFileIdentifier + headers.ContentDisposition.FileName).Replace("\"", string.Empty);
            return fileName;
        }

        
    }

}