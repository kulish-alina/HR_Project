using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repositories
{
    public class CandidateRepository : BaseRepository<Candidate>, ICandidateRepository
    {
        public CandidateRepository(DbContext context) : base(context)
        {
        }

        public override void Delete(Candidate entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            entityToDelete.Comments.ToList().ForEach(c => context.DeleteEntity(c));
            entityToDelete.Events.ToList().ForEach(e => context.DeleteEntity(e));
            entityToDelete.SocialNetworks.ToList().ForEach(sn => context.DeleteEntity(sn));
            entityToDelete.Files.ToList().ForEach(f => context.DeleteEntity(f));
            entityToDelete.VacanciesProgress.ToList().ForEach(vsi => context.DeleteEntity(vsi));
            entityToDelete.PhoneNumbers.ToList().ForEach(pn => context.DeleteEntity(pn));
            entityToDelete.Sources.ToList().ForEach(s => context.DeleteEntity(s));
            if (entityToDelete.Photo != null)
            {
                context.DeleteEntity(entityToDelete.Photo);
            }
            dbSet.Remove(entityToDelete);
        }
    }
}