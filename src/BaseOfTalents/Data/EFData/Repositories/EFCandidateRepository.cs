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

namespace Data.EFData.Repositories
{
    public class EFCandidateRepository : EFBaseEntityRepository<Candidate>, ICandidateRepository
    {
        public override IQueryable<Candidate> GetAll()
        {
            return _context.Candidates
                .Include(x => x.PhoneNumbers)
                .Include(x => x.SocialNetworks.Select(y => y.SocialNetwork))
                .Include(x => x.VacanciesProgress)
                .Include(x => x.Skills)
                .Include(x => x.Location.Country)
                .Include(x => x.LanguageSkills.Select(y => y.Language))
                .Include(x => x.Files)
                .Include(x => x.Comments)
                .Include(x => x.VacanciesProgress.Select(y => y.VacancyStage))
                .Include(x => x.Sources);
        }
    }
}
