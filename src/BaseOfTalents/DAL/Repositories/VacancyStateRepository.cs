using DAL.Infrastructure;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class VacancyStateRepository: BaseRepository<VacancyState>, IVacancyStateRepository
    {
        public VacancyStateRepository(DbContext context) : base(context)
        {

        }
    }
}
