using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using DAL.Services;
using Domain.DTO.DTOModels.SetupDTO;
using System;

namespace DAL.Services
{
    public class EventTypeService : BaseService<EventType, EventTypeDTO>
    {
        public EventTypeService(IUnitOfWork uow) : base(uow, uow.EventTypeRepo)
        {

        }
    }
}
