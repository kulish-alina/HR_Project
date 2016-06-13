using BaseOfTalents.DAL.Mapping;
using BaseOfTalents.Domain.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public class NoteConfiguration : BaseEntityConfiguration<Note>
    {
        public NoteConfiguration()
        {
            HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
