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
        public static void Update(this User domain, UserDTO dto, IRepository<Photo> photoRepo, IRepository<PhoneNumber> phoneRepo)
        {
            domain.FirstName = dto.FirstName;
            domain.MiddleName = dto.MiddleName;
            domain.LastName = dto.LastName;
            domain.isMale = dto.isMale;
            domain.BirthDate = dto.BirthDate;
            domain.Email = dto.Email;
            domain.Skype = dto.Skype;
            domain.Login = dto.Login;
            domain.Password = dto.Password;
            domain.RoleId = dto.RoleId;
            if(dto.Photo.Id!=0)
            {
                var photoBd = photoRepo.Get(dto.Photo.Id);
                photoBd.Description = dto.Photo.Description;
                photoBd.ImagePath = dto.Photo.ImagePath;
                photoBd.State = dto.Photo.State;
                domain.Photo = photoBd;
            }
            else
            {
                domain.Photo = new Photo
                {
                    Id = dto.Photo.Id,
                    Description = dto.Photo.Description,
                    ImagePath = dto.Photo.ImagePath,
                    State = dto.Photo.State
                };
            }
            domain.LocationId = dto.LocationId;

            foreach(var dtoPhone in dto.PhoneNumbers)
            {
                var domainPhone = domain.PhoneNumbers.FirstOrDefault(x=> x.Id == dtoPhone.Id);
                if (domainPhone == null)
                {
                    var number = new PhoneNumber()
                    {
                        Number = dtoPhone.Number
                    };
                    domain.PhoneNumbers.Add(number);
                }
                else
                {
                    domainPhone.Number = dtoPhone.Number;
                    domainPhone.State = dtoPhone.State;
                }
            }
        }
    }
}
