using Data.Infrastructure;
using Domain.DTO.DTOModels.SetupDTO;
using Domain.Entities.Enum.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class TagsController : BoTController<Tag, TagDTO>
    {
        public TagsController(IDataRepositoryFactory repoFatory, IUnitOfWork unitOfWork, IErrorRepository errorRepo)
            : base (repoFatory, unitOfWork, errorRepo)
        {

        }
    }
}
