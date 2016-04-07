using Domain.Entities;
using Domain.Entities.Enum;
using Domain.Entities.Setup;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Data.EFData.Repositories
{
    public class EFVacancyRepository : EFBaseEntityRepository, IVacancyRepository
    {
        public void Add(Vacancy entity)
        {
            _context.Vacancies.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<Vacancy> FindBy(Expression<Func<Vacancy, bool>> predicate)
        {
            return _context.Vacancies.Where(predicate);
        }

        public Vacancy Get(int id)
        {
            return _context.Vacancies.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Vacancy> GetAll()
        {
            return _context.Vacancies.AsQueryable();
        }

        public void Remove(Vacancy entity)
        {
            var attachedEntity = _context.Vacancies.Attach(entity);
            attachedEntity.State = EntityState.Inactive;
            _context.Entry(attachedEntity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(Vacancy entity)
        {
            var attachedEntity = _context.Vacancies.Attach(entity);
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
