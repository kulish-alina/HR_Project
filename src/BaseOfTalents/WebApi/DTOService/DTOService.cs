using AutoMapper;
using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.DTO.DTOService
{
    public static class DTOService
    {
        public static ViewModel ToDTO<DomainEntity, ViewModel>(DomainEntity entity)
            where DomainEntity : BaseEntity, new()
            where ViewModel : new()
        {
            return Mapper.Map<DomainEntity, ViewModel>(entity);
        }

        public static DomainEntity ToEntity<ViewModel, DomainEntity>(ViewModel entity)
            where DomainEntity : BaseEntity, new()
            where ViewModel : new()
        {
            return Mapper.Map<ViewModel, DomainEntity>(entity);
        }
    }
}
