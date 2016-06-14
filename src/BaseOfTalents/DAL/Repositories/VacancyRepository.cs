using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace BaseOfTalents.DAL.Repositories
{
    public class VacancyRepository : BaseRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(DbContext context) : base(context)
        {
        }
    }
}