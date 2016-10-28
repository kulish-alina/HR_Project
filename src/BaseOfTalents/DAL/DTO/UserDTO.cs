using System;
using System.Collections.Generic;
using Domain.Entities;

namespace DAL.DTO
{
    public class UserDTO : BaseEntityDTO
    {
        public UserDTO()
        {
            PhoneNumbers = new List<PhoneNumberDTO>();
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool isMale { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string Login { get; set; }
        public Password Password { get; set; }

        public int RoleId { get; set; }

        public FileDTO Photo { get; set; }

        public int? CityId { get; set; }

        public IEnumerable<PhoneNumberDTO> PhoneNumbers { get; set; }
    }
}
