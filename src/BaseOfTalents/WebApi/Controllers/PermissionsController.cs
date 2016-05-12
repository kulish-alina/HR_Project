using Data.Infrastructure;
using Domain.DTO.DTOModels;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class PermissionsController : BoTController<Permission, PermissionDTO>
    {
        public PermissionsController(IDataRepositoryFactory repoFatory, IUnitOfWork unitOfWork, IErrorRepository errorRepo)
            : base(repoFatory, unitOfWork, errorRepo)
        {

        }

        public PermissionsController()
        {

        }

    }
}
