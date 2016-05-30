using Service.Extentions;
using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Repositories;
using System;

namespace Service.Services
{
    public class UserService : ControllerService<User, UserDTO>
    {
        IRepository<Photo> photoRepository;
        IRepository<PhoneNumber> phoneNumberRepository;

        public UserService(
            IRepository<User> userRepository, 
            IRepository<Photo> photoRepository, 
            IRepository<PhoneNumber> phoneNumberRepository) : base(userRepository)
        {
            this.photoRepository = photoRepository;
            this.phoneNumberRepository = phoneNumberRepository;
        }
        public override UserDTO Add(UserDTO userToAdd)
        {
            User _user = new User();

            _user.Update(userToAdd,
                photoRepository, 
                phoneNumberRepository);
            entityRepository.Add(_user);
            entityRepository.Commit();

            return DTOService.ToDTO<User, UserDTO>(_user);
        }
        public override UserDTO Put(UserDTO entity)
        {
            User _user = entityRepository.Get(entity.Id);

            _user.Update(entity,
                photoRepository,
                phoneNumberRepository);
            entityRepository.Update(_user);
            entityRepository.Commit();

            return DTOService.ToDTO<User, UserDTO>(_user);
        }

        public override object Search(object searchParams)
        {
            throw new NotImplementedException();
        }
    }
}
