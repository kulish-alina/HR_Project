using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class TagService : BaseService<Tag, TagDTO>
    {
        public TagService(IUnitOfWork uow) : base(uow, uow.TagRepo)
        {

        }
    }
}
