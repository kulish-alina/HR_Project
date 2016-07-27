using System;
using System.IO;

namespace WebUI.Globals
{
    public sealed class Constants
    {
        public static string RootFolder { get; } = "wwwroot";
        public static string Uploads { get; } = "uploads";

        public static string AppPath { get; } = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, RootFolder);
    }
}