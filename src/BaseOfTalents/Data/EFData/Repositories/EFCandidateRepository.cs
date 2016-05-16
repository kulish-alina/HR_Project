using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using System.Linq.Expressions;
using System;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;
using System.Linq;
using Domain.DTO.DTOModels;
using Data.Infrastructure;

namespace Data.EFData.Repositories
{
    public class EFCandidateRepository : EFBaseEntityRepository<Candidate>, ICandidateRepository
    {
        public EFCandidateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
