using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities;

namespace DAL.Services
{
    public class MailService : BaseService<MailContent, MailDTO>
    {
        public MailService(IUnitOfWork uow) : base(uow, uow.MailRepo)
        {
        }
    }
}
