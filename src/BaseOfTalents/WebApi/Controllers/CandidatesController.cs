using Domain.Entities;
using Domain.Repositories;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.DTO.DTOService;
using System;
using Data.EFData.Design;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using System.Collections.Generic;
using Domain.DTO.DTOModels;

namespace WebApi.Controllers
{
    public class CandidatesController : BoTController<Candidate, CandidateDTO>
    {
        public CandidatesController(IRepositoryFacade facade) : base(facade)
        {
            _currentRepo = _repoFacade.CandidateRepository;
        }

    }
}
