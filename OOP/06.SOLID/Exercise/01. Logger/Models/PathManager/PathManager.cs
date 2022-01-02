using _01._Logger.Models.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _01._Logger.Models.PathManager
{
    public class PathManager : IPathManager
    {
        // private const string PATH_DELIMITER = "\\";
        private readonly string currentPath;
        private readonly string folderName;
        private readonly string fileName;

        private PathManager()
        {
            this.currentPath = this.GetCurrentPath();
        }

        public PathManager(string folderName, string fileName) : this()
        {
            this.folderName = folderName;
            this.fileName = fileName;
        }
        public string CurrentDirectoryPath => Path.Combine(this.currentPath, this.folderName);

        public string CurrentFilePath => Path.Combine(this.CurrentDirectoryPath, this.fileName);

        public void EnsureDirectoryAndFileExists()
        {
            if (!Directory.Exists(this.CurrentDirectoryPath))
            {
                Directory.CreateDirectory(this.CurrentDirectoryPath);
            }

            File.AppendAllText(this.CurrentFilePath, string.Empty);
        }

        public string GetCurrentPath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
