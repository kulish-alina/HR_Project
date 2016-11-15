using DAL.DTO;
using DAL.Extensions;
using DAL.Infrastructure;
using Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var users = uow.UserRepo.Get();
            return users.Select(x => DTOService.ToDTO<User, UserDTO>(x));
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
        /// <summary>
        /// Perfoms accessing to user of specified login and password
        /// </summary>
        /// <param name="login">Application user login</param>
        /// <param name="password">User password (hashed)</param>
        /// <returns>Corresponting user dto object</returns>
        /// <exception cref="System.Exception">Is thrown, when there is no user with such a login and password</exception>
        public UserDTO Authentificate(string login, string password)
        {
            var user = uow.UserRepo.Get(login, password);
            if (user == null)
            {
                throw new Exception("Wrong login or password");
                //TODO: Extract message to external source
                //TODO: new exception type
            }
            return DTOService.ToDTO<User, UserDTO>(user);
        }
        /// <summary>
        /// Perfoms accessing to user of specified login and password async
        /// </summary>
        /// <param name="login">Application user login</param>
        /// <param name="password">User password (hashed)</param>
        /// <returns>Corresponting user dto object</returns>
        /// <exception cref="System.Exception">Is thrown, when there is no user with such a login and password</exception>
        public async Task<UserDTO> AuthentificateAsync(string login, string password)
        {
            var user = await uow.UserRepo.GetAsync(login, password);
            if (user == null)
            {
                throw new Exception("Wrong login or password");
                //TODO: Extract message to external source
                //TODO: new exception type
            }
            return DTOService.ToDTO<User, UserDTO>(user);
        }
    }
}
