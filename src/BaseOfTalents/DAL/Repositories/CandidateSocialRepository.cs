using BaseOfTalents.DAL.Repositories;
using BaseOfTalents.Domain.Entities;
using DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CandidateSocialRepository : BaseRepository<CandidateSocial>, ICandidateSocialRepository
    {
        public CandidateSocialRepository(DbContext context) : base(context)
        {

        }
    }
}
