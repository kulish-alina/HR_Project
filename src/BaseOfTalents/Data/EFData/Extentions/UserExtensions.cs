using Domain.DTO.DTOModels;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Extentions
{
    public static class UserExtensions
    {
        public static void Update(this User destination, UserDTO source, IRepository<Photo> photoRepository, IRepository<PhoneNumber> phoneNumberRepository)
        {
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
            destination.LocationId = source.LocationId;

            PerformPhotoSaving(destination, source, photoRepository);
            PerformPhoneNumbersSaving(destination, source, phoneNumberRepository);
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
                    throw new ArgumentNullException("Request contains unknown entity");
                }
                if (updatedPhoneNumber.ShouldBeRemoved())
                {
                    phoneNumberRepository.Remove(updatedPhoneNumber.Id);
                }
                else
                {
                    domainPhoneNumber.Update(updatedPhoneNumber);
                }
            });
        }

        private static void PerformPhotoSaving(User destination, UserDTO source, IRepository<Photo> photoRepository)
        {
            if (source.Photo != null)
            {
                if (source.Photo.IsNew())
                {
                    var photoBd = photoRepository.Get(source.Photo.Id);
                    photoBd.Update(source.Photo);
                    destination.Photo = photoBd;
                }
                else if (source.Photo.ShouldBeRemoved())
                {
                    photoRepository.Remove(destination.Photo.Id);
                }
                else
                {
                    destination.Photo.Update(source.Photo);
                }
            }
        }
    }
}
