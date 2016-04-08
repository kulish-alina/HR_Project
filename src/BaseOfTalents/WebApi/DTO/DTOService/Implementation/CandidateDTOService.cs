using Domain.Entities;
using WebApi.DTO.DTOModels;
using WebApi.DTO.DTOService.Abstract;
using AutoMapper;

namespace WebApi.DTO.DTOService.Implementation
{
    public class CandidateDTOService : ICandidateDTOService
    {
        public CandidateDTO ToDTO(Candidate entity)
        {
            return Mapper.Map<Candidate, CandidateDTO>(entity);
        }

        public Candidate ToEntity(CandidateDTO dto)
        {
            return Mapper.Map<CandidateDTO, Candidate>(dto);
        }
    }
}