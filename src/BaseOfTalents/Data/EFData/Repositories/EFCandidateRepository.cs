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
    public class EFCandidateRepository : EFBaseEntityRepository, ICandidateRepository
    {
        public void Add(Candidate entity)
        {
            _context.Candidates.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<Candidate> FindBy(Expression<Func<Candidate, bool>> predicate)
        {
            return _context.Candidates.Where(predicate);
        }

        public Candidate Get(int id)
        {
            return _context.Candidates.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Candidate> GetAll()
        {
            return _context.Candidates.AsQueryable();
        }

        public void Remove(Candidate entity)
        {
            entity.State = EntityState.Inactive;
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(Candidate entity)
        {
            var attachedEntity = _context.Candidates.Attach(entity);
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
