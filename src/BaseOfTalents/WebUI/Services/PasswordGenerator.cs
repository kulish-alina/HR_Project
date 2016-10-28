using DAL.DTO;
using Domain.Entities;

namespace WebUI.Services
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword()
        {
            return System.Web.Security.Membership.GeneratePassword(12, 5);
        }

        public static void GeneratePassword(this UserDTO user)
        {
            user.Password = (Password)GeneratePassword();
        }
    }
}