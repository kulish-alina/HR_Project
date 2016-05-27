using Domain.DTO.DTOModels;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.IO;
using Service.Extentions;
using System.Web.Http;
using System.Net;

namespace Service.Services
{
    public class FileService
    {
        IRepository<Domain.Entities.File> repository;

        public FileService(IRepository<Domain.Entities.File> repository)
        {
            this.repository = repository;
        }

        public FileDTO Add(HttpRequestMessage request, HttpContext currentContext)
        {
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string savingPath = getSavingPath();
            createDirectoryIfDoesntExists(savingPath);
            var savedFile = readFileTo(savingPath, request);
            var domainFile = new Domain.Entities.File()
            {
                FilePath = getFilePath(savedFile),
                Description = getClearFileName(savedFile)
            };
            repository.Add(domainFile);
            repository.Commit();
            return DTOService.ToDTO<Domain.Entities.File, FileDTO>(domainFile);

        }

        private MultipartFileData readFileTo(string savingPath, HttpRequestMessage request)
        {
            var provider = new BoTMultipartDataStreamProvider(savingPath);
            request.Content.ReadAsMultipartAsync(provider).ContinueWith((data) =>
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

        private string getFilePath(MultipartFileData savedFile)
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
