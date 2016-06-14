using BaseOfTalents.DAL.Repositories;
using DAL.Infrastructure;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class NoteRepository : BaseRepository<Note>, INoteRepository
    {
        public NoteRepository(DbContext context) : base(context)
        {

        }
    }
}
