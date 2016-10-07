using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public interface ILogable
    {
        void Log(LogUnit unitToLog);
    }
}
