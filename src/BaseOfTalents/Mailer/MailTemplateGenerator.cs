using System;
using Mailer.Validation;

namespace Mailer
{
    public static class MailTemplateGenerator
    {
        private static string textInvitation = "#TextInvitation";
        private static string textBody = "#TextBody";
        private static string textFarewell = "#TextFarewell";
        private static string urlOfRedirect = "#urlOfRedirect";
        private static string urlOfImage = "#urlOfImage";


        /// <summary>
        /// The method that generates from dynamic html template a new html email with replacing of dynamic fields with user specified data
        /// </summary>
        /// <param name="dynamicTemplate">Is an html letter which content should be fullfilled with real user data</param>
        /// <param name="invitation">Invitation part of the letter</param>
        /// <param name="body">Body of letter - text, which consists of paragraps, including information of password and login</param>
        /// <param name="urlOfImageHosting">Url of where the image of letter is located</param>
        /// <param name="urlOfOuterSource">Url of where to redirect user on link click</param>
        /// <param name="farewell">Farewell - part of the letter where are written down wishes to incomer</param>
        /// <returns>A string with static tamplate</returns>
        public static string Generate(string dynamicTemplate, string invitation, string body, string farewell,
            string urlOfImageHosting, string urlOfOuterSource)
        {
            var validator = new DynamicContentValidator()
            {
                textInvitation,
                textBody,
                textFarewell,
                urlOfImage,
                urlOfRedirect
            };

            DynamicContentValidationResult validationResult = validator.ValidateContent(dynamicTemplate);
            if (!validationResult.IsAcceptable)
            {
                throw new ArgumentException($"An error occured {validationResult.ErrorMessage}");
            }

            string mail = dynamicTemplate
                .Replace(urlOfImage, urlOfImageHosting)
                .Replace(urlOfRedirect, urlOfOuterSource)
                .Replace(textInvitation, invitation)
                //Use AngleSharp to find the paragraphs
                .Replace(textBody, body)
                .Replace(textFarewell, farewell);

            return PreMailer.Net.PreMailer.MoveCssInline(mail).Html;
        }

        /// <summary>
        /// The method that creates a specified Email with subject, html body from the dynamic html template and data replace
        /// </summary>
        /// <param name="dynamicTemplate">Is an html letter which content should be fullfilled with real user data</param>
        /// <param name="mailInvitation">Invitation part of the letter</param>
        /// <param name="mailBody">Body of letter - text, which consists of paragraps, including information of password and login</param>
        /// <param name="mailFarewell">Farewell - part of the letter where are written down wishes to incomer</param>
        /// <param name="mailSubject">Subject of the letter</param>
        /// <param name="urlOfImageHosting">Url of where the image of letter is located</param>
        /// <param name="urlOfOuterSource">Url of where to redirect user on link click</param>
        /// <returns>Model of generated mail</returns>
        public static MailModel Generate(string dynamicTemplate, string mailInvitation, string mailBody, string mailFarewell, string mailSubject,
            string urlOfImageHosting, string urlOfOuterSource)
        {
            string template = Generate(dynamicTemplate, mailInvitation, mailBody, mailFarewell, urlOfImageHosting, urlOfOuterSource);
            return new MailModel(template, mailSubject);
        }
    }
}