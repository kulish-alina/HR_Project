using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DTO.DTOModels;

namespace WebApi.DTO.DTOService.Abstract
{
    public interface ICandidateDTOService : IDTOService<Candidate, CandidateDTO>
    {
    }
}