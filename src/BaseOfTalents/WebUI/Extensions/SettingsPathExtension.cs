using System;
using System.IO;
using WebUI.Globals;

namespace WebUI.Extensions
{
    public static class SettingsPathExtension
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
    }
}