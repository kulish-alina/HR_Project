using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class EventType : BaseEntity
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}
