using _01._Logger.Common;
using _01._Logger.Core.Contracts;
using _01._Logger.Factories;
using _01._Logger.IOManagement.Contracts;
using _01._Logger.Models.Contracts;
using _01._Logger.Models.Enumerations;
using _01._Logger.Models.Errors;
using _01._Logger.Models.IOManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace _01._Logger.Core
{
    public class Engine : IEngine
    {
        private readonly ILogger logger;
        private readonly IReader reader;
        private readonly IWriter writer;

        public Engine(ILogger logger, IReader reader, IWriter writer)
        {
            this.logger = logger;
            this.reader = reader;
            this.writer = writer;
        }
        public void Run()
        {
            string command;
            while ((command = this.reader.ReadLine()) != "END")
            {
                List<string> errorInfo = command.Split("|").ToList();
                string levelStr = errorInfo[0];
                string dateTimeStr = errorInfo[1];
                string errorMessage = errorInfo[2];

                Level level;
                bool isLevelValid = Enum.TryParse(typeof(Level), levelStr, true, out object levelObj);

                if (!isLevelValid)
                {
                    this.writer.WriteLine(GlobalConstants.INVALID_LEVEL_TYPE);
                    continue;
                }
                level = (Level)levelObj;
                bool isDateTimeValid = DateTime.TryParseExact(dateTimeStr, GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);
                if (!isDateTimeValid)
                {
                    this.writer.WriteLine(GlobalConstants.INVALID_DATETIME_FORMAT);
                }
                IError error = new Error(dateTime,errorMessage,level);

                this.logger.Log(error);

                try
                {
                }
                catch (Exception)
                {

                    throw;
                }
            }

            this.writer.WriteLine(this.logger.ToString());
        }
    }
}
