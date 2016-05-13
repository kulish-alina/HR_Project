using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enum
{
    public enum EntityState
    {
        Inactive = 1,
        Active,
        Verfied,
        Unverified,
        Open,
        Processing,
        Closed,
        Cancelled
    }
}
