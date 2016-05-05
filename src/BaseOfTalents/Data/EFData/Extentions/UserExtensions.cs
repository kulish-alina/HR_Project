using Domain.DTO.DTOModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EFData.Extentions
{
    public static class UserExtensions
    {
        public static void Update(this User domain, UserDTO dto)
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
            domain.Photo = new Photo {
                Id = dto.Photo.Id,
                Description = dto.Photo.Description,
                ImagePath = dto.Photo.ImagePath,
                State = dto.Photo.State
            };
            domain.LocationId = dto.LocationId;
            domain.PhoneNumbers = dto.PhoneNumbers.Select(x => new PhoneNumber()
            {
                Id = x.Id,
                Number = x.Number,
                State = x.State
            }).ToList();
        }
    }
}
