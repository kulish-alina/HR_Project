﻿using BaseOfTalents.Domain.Entities;
using Domain.DTO.DTOModels;

namespace DAL.Extensions
{
    public static class CandidateSourceExtensions
    {
        public static void Update(this CandidateSource domain, CandidateSourceDTO dto)
        {
            domain.Path = dto.Path;
            domain.SourceId = dto.SourceId;
            domain.State = dto.State;
        }
    }
}
