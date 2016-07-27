using DAL.DTO;
using DAL.Infrastructure;
using Domain.Entities;
using System;
using System.Linq;

namespace DAL.Extensions
{
    public static class UserExtensions
    {
        public static void Update(this User destination, UserDTO source, IUnitOfWork uow)
        {
            destination.State = source.State;

            destination.FirstName = source.FirstName;
            destination.MiddleName = source.MiddleName;
            destination.LastName = source.LastName;
            destination.isMale = source.isMale;
            destination.BirthDate = source.BirthDate;
            destination.Email = source.Email;
            destination.Skype = source.Skype;
            destination.Login = source.Login;
            destination.Password = source.Password;
            destination.RoleId = source.RoleId;
            destination.CityId = source.CityId;

            PerformPhotoSaving(destination, source, uow.FileRepo);
            PerformPhoneNumbersSaving(destination, source, uow.PhoneNumberRepo);
        }

        private static void PerformPhoneNumbersSaving(User destination, UserDTO source, IRepository<PhoneNumber> phoneNumberRepository)
        {
            RefreshExistingPhoneNumbers(destination, source, phoneNumberRepository);
            CreateNewPhoneNumbers(destination, source);
        }
        private static void CreateNewPhoneNumbers(User destination, UserDTO source)
        {
            source.PhoneNumbers.Where(x => x.IsNew()).ToList().ForEach(newPhoneNumber =>
            {
                var toDomain = new PhoneNumber();
                toDomain.Update(newPhoneNumber);
                destination.PhoneNumbers.Add(toDomain);
            });
        }
        private static void RefreshExistingPhoneNumbers(User destination, UserDTO source, IRepository<PhoneNumber> phoneNumberRepository)
        {
            source.PhoneNumbers.Where(x => !x.IsNew()).ToList().ForEach(updatedPhoneNumber =>
            {
                var domainPhoneNumber = destination.PhoneNumbers.FirstOrDefault(x => x.Id == updatedPhoneNumber.Id);
                if (domainPhoneNumber == null)
                {
                    throw new ArgumentNullException("You trying to update phone number which is actually doesn't exists in database");
                }
                if (updatedPhoneNumber.ShouldBeRemoved())
                {
                    phoneNumberRepository.Delete(updatedPhoneNumber.Id);
                }
                else
                {
                    domainPhoneNumber.Update(updatedPhoneNumber);
                }
            });
        }

        private static void PerformPhotoSaving(User destination, UserDTO source, IRepository<File> fileRepository)
        {
            var photoInDTO = source.Photo;
            if (photoInDTO != null)
            {
                var photoInDb = fileRepository.GetByID(source.Photo.Id);
                if (photoInDb == null)
                {
                    throw new Exception("Database doesn't contains such entity");
                }
                if (photoInDTO.ShouldBeRemoved())
                {
                    fileRepository.Delete(photoInDb.Id);
                }
                else
                {
                    photoInDb.Update(photoInDTO);
                    destination.Photo = photoInDb;
                }
            }
        }
    }
}
