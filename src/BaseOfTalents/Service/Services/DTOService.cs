using AutoMapper;
using Domain.Entities;

namespace Service.Services
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
