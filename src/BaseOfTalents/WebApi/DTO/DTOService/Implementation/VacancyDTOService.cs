using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using WebApi.DTO.DTOModels;
using WebApi.DTO.DTOService.Abstract;

namespace WebApi.DTO.DTOService.Implementation
{
    public class VacancyDTOService : IVacancyDTOService
    {
        public VacancyDTO ToDTO(Vacancy entity)
        {
            return AutoMapper.Mapper.Map<Vacancy, VacancyDTO>(entity); 
        }

        public Vacancy ToEntity(VacancyDTO dto)
        {
            return AutoMapper.Mapper.Map<VacancyDTO, Vacancy>(dto);
        }
    }
}