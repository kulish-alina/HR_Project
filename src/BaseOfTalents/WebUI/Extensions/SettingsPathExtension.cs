using System;
using System.IO;
using WebUI.Globals;

namespace WebUI.Extensions
{
    public static class SettingsExtension
    {
        public static string CreateAbsolutePathFromApplication(string relativePath)
        {
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, relativePath);
        }

        public static string GetRootPath(this SettingsContext context)
        {
            return CreateAbsolutePathFromApplication(context.WWWRoot);
        }

        public static string GetUploadsPath(this SettingsContext context)
        {
            return CreateAbsolutePathFromApplication(context.Uploads);
        }

        public static string GetImageUrl(this SettingsContext context)
        {
            return $"{context.IssuerUrl}:{context.Port}{context.ImageUrl}";
        }

        public static string GetOuterUrl(this SettingsContext context)
        {
            return $"{context.IssuerUrl}:{context.Port}{context.OuterUrl}";
        }
    }
}