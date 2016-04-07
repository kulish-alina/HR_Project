using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class SocialNetwork: BaseEntity
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }
}
