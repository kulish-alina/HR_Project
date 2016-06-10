using Domain.DTO.DTOModels;
using System;
using BaseOfTalents.Domain.Entities;
using BaseOfTalents.DAL.Infrastructure;
using DAL.Extensions;

namespace DAL.Services
{
    public class UserService
    {
        IUnitOfWork uow;
        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public UserDTO Get(int id)
        {
            var entity = uow.UserRepo.GetByID(id);
            return DTOService.ToDTO<User, UserDTO>(entity);
        }

        public object Get(object searchParameters)
        {
            throw new NotImplementedException();
            //return base.Get(searchParameters);
        }

        public UserDTO Add(UserDTO userToAdd)
        {
            User _user = new User();
            _user.Update(userToAdd, uow);
            uow.UserRepo.Insert(_user);
            uow.Commit();
            return DTOService.ToDTO<User, UserDTO>(_user);
        }

        public UserDTO Update(UserDTO entity)
        {
            User _user = uow.UserRepo.GetByID(entity.Id);
            _user.Update(entity, uow);
            uow.UserRepo.Update(_user);
            uow.Commit();
            return DTOService.ToDTO<User, UserDTO>(_user);
        }

        public bool Delete(int id)
        {
            bool deleteResult;
            var entityToDelete = uow.UserRepo.GetByID(id);
            if (entityToDelete == null)
            {
                deleteResult = false;
            }
            else {
                uow.UserRepo.Delete(id);
                uow.Commit();
                deleteResult = true;
            }
            return deleteResult;
        }
    }
}
