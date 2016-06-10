using BaseOfTalents.DAL;
using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Exceptions;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Services
{
    public class LanguageService : BaseService<Language, LanguageDTO>
    {
        public LanguageService(IUnitOfWork uow) : base(uow, uow.LanguageRepo)
        {

        }
    }
}
