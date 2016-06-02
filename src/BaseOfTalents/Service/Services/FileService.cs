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

        public FileDTO Add(Domain.Entities.File file)
        {
            repository.Add(file);
            repository.Commit();
            return DTOService.ToDTO<Domain.Entities.File, FileDTO>(file);
        }

        public bool Remove(int id)
        {
            bool result;
            var file = repository.Get(id);
            if (file != null)
            {
                repository.Remove(file);
                repository.Commit();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
