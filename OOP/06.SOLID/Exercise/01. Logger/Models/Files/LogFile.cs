using _01._Logger.Common;
using _01._Logger.Models.Contracts;
using _01._Logger.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _01._Logger.Models.Files
{
    public class LogFile : IFile
    {
        private readonly IPathManager pathManager;


        public LogFile(IPathManager pathManager)
        {
            this.pathManager = pathManager;
            this.pathManager.EnsureDirectoryAndFileExists();
        }

        public string Path => this.pathManager.CurrentFilePath;

        public long Size => this.CalculateFileSize();

        public string Write(ILayout layout, IError error)
        {
            string format = layout.Format;

            DateTime dateTime = error.DateTime;
            string message = error.Message;
            Level level = error.Level;

            string formattedMessage = String.Format(format, dateTime.ToString(GlobalConstants.DateTimeFormat), level.ToString(), message);

            return formattedMessage;
        }
        private long CalculateFileSize()
        {
            string fileText = File.ReadAllText(this.Path);

            return fileText.ToCharArray().Where(c => Char.IsLetter(c)).Sum(c => c); // testing all chars
           // return fileText.ToCharArray().Sum(c => c);
        }
    }
}
