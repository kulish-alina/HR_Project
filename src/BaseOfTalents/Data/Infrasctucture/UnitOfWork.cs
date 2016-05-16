using Data.EFData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext dbContext;

        public UnitOfWork(DbContext context)
        {
            this.dbContext = context;
        }

        public DbContext Context
        {
            get
            {
                return dbContext;
            }
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
