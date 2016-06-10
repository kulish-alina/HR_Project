using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities;
using DAL.Extensions;
using Domain.DTO.DTOModels;
using System;

namespace DAL.Services
{
    public class CandidateService
    {
        IUnitOfWork uow;
        public CandidateService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public CandidateDTO Get(int id)
        {
            var entity = uow.CandidateRepo.GetByID(id);
            return DTOService.ToDTO<Candidate, CandidateDTO>(entity);
        }

        public CandidateDTO Add(CandidateDTO candidateToAdd)
        {
            Candidate _candidate = new Candidate();
            _candidate.Update(candidateToAdd, uow);
            uow.CandidateRepo.Insert(_candidate);
            uow.Commit();
            return DTOService.ToDTO<Candidate, CandidateDTO>(_candidate);
        }

        public object Get(object searchParameters)
        {
            throw new NotImplementedException();
            //return base.Get(searchParameters);
        }

        public CandidateDTO Update(CandidateDTO entity)
        {
            Candidate _candidate = uow.CandidateRepo.GetByID(entity.Id);
            _candidate.Update(entity, uow);
            uow.CandidateRepo.Update(_candidate);
            uow.Commit();
            return DTOService.ToDTO<Candidate, CandidateDTO>(_candidate);
        }
        public bool Delete(int id)
        {
            bool deleteResult;
            var entityToDelete = uow.CandidateRepo.GetByID(id);
            if (entityToDelete == null)
            {
                deleteResult = false;
            }
            else {
                uow.CandidateRepo.Delete(id);
                uow.Commit();
                deleteResult = true;
            }
            return deleteResult;
        }
    }
}
