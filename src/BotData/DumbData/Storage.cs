using BotLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotData.DumbData
{
    public static class Storage
    {
        public static IList<User> Users = new List<User>()
        {
            new User()
            {
                Id =1,
                Login = "admin",
                Password = "admin",
                IsActive = true,
                EditTime = DateTime.Now,
                ContactInfo = new ContactInfo() { Email = "admin@admin.admin", Id = 1,
                                                  EditTime = DateTime.Now, IsActive = true, Skype = "admin", PhoneNumbers = new List<string>()},
                Files = new List<File>(),
                Location = new Location() { City = new City() {Id = 1, EditTime = DateTime.Now, Country = new Country() {Id = 1,
                                            EditTime = DateTime.Now, IsActive = true, Name = "Ukraine" }, IsActive = true, Name = "Sicheslav"  },
                                            EditTime = DateTime.Now, Id = 1, IsActive = true},
                PersonalInfo = new PersonalInfo() { FirstName = "admin", MiddleName = "admin", LastName = "admin", BirthDate = DateTime.Now, Id = 1,
                                                    Gender = true, EditTime = DateTime.Now, IsActive = true, Photo = new Photo()},
                Role = new Role() { Name = "administrator", IsActive = true, EditTime = DateTime.Now, Id = 1, PermissionId = 1}                
            }               
        };                
    }
}
