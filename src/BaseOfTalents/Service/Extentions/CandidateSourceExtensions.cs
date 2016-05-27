using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Extentions
{
    public static class CandidateSourceExtensions
    {
        public static void Update(this CandidateSource domain, CandidateSourceDTO dto)
        {
            domain.Path = dto.Path;
            domain.Source = dto.Source;
            domain.State = dto.State;
        }
    }
}
