using Data.EFData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BOTContext Init();
    }
}
