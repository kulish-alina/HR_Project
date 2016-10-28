using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Extensions;
using DAL.Infrastructure;
using Domain.Entities;

namespace DAL.Services
{
    public class UserService
    {
        IUnitOfWork uow;

        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public IEnumerable<User> Get()
        {
            return uow.UserRepo.Get();
        }

        public UserDTO Get(int id)
        {
            var entity = uow.UserRepo.GetByID(id);
            return DTOService.ToDTO<User, UserDTO>(entity);
        }

        public UserDTO Get(string login)
        {
            User user = uow.UserRepo.Get(login);
            return DTOService.ToDTO<User, UserDTO>(user);
        }

        public async Task<UserDTO> GetAsync(string login)
        {
            User user = await uow.UserRepo.GetAsync(login);
            return DTOService.ToDTO<User, UserDTO>(user);
        }

        public UserDTO Get(Func<User, bool> predicate)
        {
            return DTOService.ToDTO<User, UserDTO>(uow.UserRepo.Get(predicate));
        }

        public UserDTO Add(UserDTO userToAdd)
        {
            if (Get((user) => user.Email == userToAdd.Email) != null ||
                Get((user) => user.Login == userToAdd.Login) != null)
            {
                throw new ArgumentException("User with such login or email alredy exist!");
            }
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
            else
            {
                uow.UserRepo.Delete(id);
                uow.Commit();
                deleteResult = true;
            }
            return deleteResult;
        }
    }
}
