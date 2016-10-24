using DAL.Infrastructure;
using Domain.Entities;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repositories
{
    public class VacancyRepository : BaseRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(DbContext context) : base(context)
        {

        }

        public override void Delete(Vacancy entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            entityToDelete.Files.ToList().ForEach(f => context.DeleteEntity(f));
            entityToDelete.Comments.ToList().ForEach(c => context.DeleteEntity(c));
            entityToDelete.CandidatesProgress.ToList().ForEach(vsi => context.DeleteEntity(vsi));

            dbSet.Remove(entityToDelete);
        }
    }

    public static class RepositoryExtensions
    {
        public static void DeleteEntity<TEntity>(this DbContext context, TEntity entity)
            where TEntity : BaseEntity, new()
        {
            context.Set<TEntity>().Attach(entity);
            context.Set<TEntity>().Remove(entity);
        }
    }
}