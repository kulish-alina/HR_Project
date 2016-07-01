using DAL.DTO.SetupDTO;
using DAL.Infrastructure;
using Domain.Entities.Enum.Setup;

namespace DAL.Services
{
    public class LanguageService : BaseService<Language, LanguageDTO>
    {
        public LanguageService(IUnitOfWork uow) : base(uow, uow.LanguageRepo)
        {

        }
    }
}
