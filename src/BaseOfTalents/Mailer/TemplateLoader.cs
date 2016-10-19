using System;
using System.IO;

namespace Mailer
{
    public class TemplateLoader : ITemplateLoader
    {
        private static string _file;

        /// <summary>
        /// The method to setup file for loading HTML mail template
        /// </summary>
        /// <param name="file">Relative path to mail</param>
        public static void SetupFile(string file)
        {
            _file = file;
        }

        /// <summary>
        /// Loads the content of the file specified with SetupFile method
        /// </summary>
        /// <see cref="SetupFile(string)"/>
        /// <returns>The string of file content</returns>
        public string Load()
        {
            if (string.IsNullOrEmpty(_file))
            {
                throw new ArgumentException("Not right name of file");
            }
            return File.ReadAllText(_file);
        }

        /// <summary>
        /// Loads the content of specified file
        /// </summary>
        /// <param name="file">Relative path to mail</param>
        /// <returns>The string of file content</returns>
        public string Load(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentException("Not right name of file");
            }
            return File.ReadAllText(_file);
        }
    }
}