using AutoMapper;
using Domain.Entities;
using Domain.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.DTO.DTOModels;

namespace WebApi
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<Candidate, CandidateDTO>();
                x.CreateMap<CandidateDTO, Candidate>();
                x.CreateMap<Vacancy, VacancyDTO>();
                x.CreateMap<VacancyDTO, Vacancy>();
                x.CreateMap<SocialNetwork, SocialNetworkDTO>();
                x.CreateMap<SocialNetworkDTO, SocialNetwork>();
                x.CreateMap<City, CityDTO>();
                x.CreateMap<CityDTO, City>();
                x.CreateMap<Country, CountryDTO>();
                x.CreateMap<CountryDTO, Country>();
            });
        }
    }
}