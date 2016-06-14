using Domain.DTO.DTOModels;
using BaseOfTalents.Domain.Entities;
using BaseOfTalents.DAL.Infrastructure;

namespace DAL.Services
{
    public class FileService : BaseService<File, FileDTO>
    {
        public FileService(IUnitOfWork uow) : base(uow, uow.FileRepo)
        {

        }

        public override FileDTO Add(FileDTO entity)
        {
            var file = DTOService.ToEntity<FileDTO, File>(entity);
            currentRepo.Insert(file);
            uow.Commit();
            return DTOService.ToDTO<File, FileDTO>(file);
        }

        public override bool Delete(int id)
        {
            bool result;
            var file = currentRepo.GetByID(id);
            if (file != null)
            {
                currentRepo.Delete(file);
                uow.Commit();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
