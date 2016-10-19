using System;
using Mailer.Validation;

namespace Mailer
{
    public static class MailBodyContentReplacer
    {
        private static string userLoginMarkup = "#UserLogin";
        private static string userPasswordMarkup = "#UserPassword";

        /// <summary>
        /// The method that accepts text with fields that should be replace with user data
        /// </summary>
        /// <param name="text">Body text, where should be some dynamic data</param>
        /// <param name="login">User Login to replace loginField</param>
        /// <param name="password">User Password to replace passwordField</param>
        /// <returns>Static email body</returns>
        public static string Replace(string text, string login, string password)
        {
            var validator = new DynamicContentValidator()
            {
                userLoginMarkup,
                userPasswordMarkup
            };

            DynamicContentValidationResult validationResult = validator.ValidateContent(text);
            if (!validationResult.IsAcceptable)
            {
                throw new ArgumentException($"An error occured {validationResult.ErrorMessage}");
            }

            return text
                    .Replace(userLoginMarkup, login)
                    .Replace(userPasswordMarkup, password);
        }

    }
}
