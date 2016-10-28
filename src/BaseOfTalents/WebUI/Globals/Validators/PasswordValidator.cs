using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Results;

namespace WebUI.Globals.Validators
{
    public class PasswordValidator
    {
        public ValidationPasswordResult Validate(string enteredPassword, string savedPassword)
        {
            return new ValidationPasswordResult(enteredPassword == savedPassword ? true : false, "Wrong old password.");
        }
    }
}