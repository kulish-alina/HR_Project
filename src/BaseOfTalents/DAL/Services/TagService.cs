using BaseOfTalents.DAL.Infrastructure;
using BaseOfTalents.Domain.Entities.Enum.Setup;
using Domain.DTO.DTOModels.SetupDTO;

namespace DAL.Services
{ 
    public class TagService : BaseService<Tag, TagDTO>
    {
        public TagService(IUnitOfWork uow) : base(uow, uow.TagRepo)
        {

        }
    }
}
