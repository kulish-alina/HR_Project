using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.DTOModels
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
        public string Password { get; set; }

        public int RoleId { get; set; }

        public PhotoDTO Photo { get; set; }

        public int LocationId { get; set; }

        public IEnumerable<PhoneNumberDTO> PhoneNumbers { get; set; }
    }
}
