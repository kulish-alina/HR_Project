using Domain.Entities;
using WebUI.Results;

namespace WebUI.Globals.Validators
{
    public class PasswordValidator
    {
        public ValidationPasswordResult Validate(string enteredPassword, Password savedPassword)
        {
            return new ValidationPasswordResult(savedPassword.Equals(enteredPassword) ? true : false, "Wrong old password.");
        }
    }
}