using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.EFData.Repositories
{
    public class EFCandidateRepository : EFBaseEntityRepository<Candidate>, ICandidateRepository
    {
        
    }
}
