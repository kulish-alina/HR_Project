using Data.EFData.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.EFData;

namespace Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        BOTContext dbContext;

        public BOTContext Init()
        {
            return dbContext ?? (dbContext = new BOTContext());
        }
        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
