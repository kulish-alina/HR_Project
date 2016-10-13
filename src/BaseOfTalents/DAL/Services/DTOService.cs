using AutoMapper;
using DAL.DTO;
using Domain.Entities;

namespace DAL.Services
{
    public static class DTOService
    {
        public static DTO ToDTO<DomainEntity, DTO>(DomainEntity entity)
            where DomainEntity : new()
            where DTO : BaseEntityDTO, new()
        {
            return Mapper.Map<DomainEntity, DTO>(entity);
        }

        public static DomainEntity ToEntity<DTO, DomainEntity>(DTO entity)
            where DTO : BaseEntityDTO, new()
            where DomainEntity : BaseEntity, new()
        {
            return Mapper.Map<DTO, DomainEntity>(entity);
        }
    }
}
