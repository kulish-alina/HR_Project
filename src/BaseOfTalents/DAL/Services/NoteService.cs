using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Services
{
    public class NoteService
    {
        IUnitOfWork uow;
        public NoteService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public NoteDTO Get(int id)
        {
            var entity = uow.NoteRepo.GetByID(id);
            return DTOService.ToDTO<Note, NoteDTO>(entity);
        }

        public NoteDTO Add(NoteDTO noteToAdd)
        {
            var newNote = DTOService.ToEntity<NoteDTO, Note>(noteToAdd);
            uow.NoteRepo.Insert(newNote);
            uow.Commit();
            return DTOService.ToDTO<Note, NoteDTO>(newNote);
        }

        public IEnumerable<NoteDTO> GetByUserId(int userId)
        {
            var filter = new List<Expression<Func<Note, bool>>>();
            filter.Add(x => x.UserId == userId);
            var notes = uow.NoteRepo.Get(filter);
            return notes.Select(x => DTOService.ToDTO<Note, NoteDTO>(x));
        }

        public NoteDTO Update(NoteDTO noteToChange)
        {
            var changedNote = DTOService.ToEntity<NoteDTO, Note>(noteToChange);
            uow.NoteRepo.Update(changedNote);
            uow.Commit();
            return DTOService.ToDTO<Note, NoteDTO>(changedNote);
        }
        public bool Delete(int id)
        {
            bool deleteResult;
            var entityToDelete = uow.NoteRepo.GetByID(id);
            if (entityToDelete == null)
            {
                deleteResult = false;
            }
            else {
                uow.NoteRepo.Delete(id);
                uow.Commit();
                deleteResult = true;
            }
            return deleteResult;
        }
    }
}
