using _01._Logger.Common;
using _01._Logger.Core;
using _01._Logger.Core.Contracts;
using _01._Logger.Factories;
using _01._Logger.IOManagement;
using _01._Logger.IOManagement.Contracts;
using _01._Logger.Models;
using _01._Logger.Models.Contracts;
using _01._Logger.Models.Enumerations;
using _01._Logger.Models.Files;
using _01._Logger.Models.IOManagement;
using _01._Logger.Models.IOManagement.Contracts;
using _01._Logger.Models.PathManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01._Logger
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            LayoutFactory layoutFactory = new LayoutFactory();
            AppenderFactory appenderFactory = new AppenderFactory();
            int n = int.Parse(Console.ReadLine());
            IReader reader = new ConsoleReader();
            IWriter writer = new ConsoleWriter();
            IPathManager pathManager = new PathManager("logs", "logs.txt");
            IFile file = new LogFile(pathManager);

            ILogger logger = SetUpLogger(n, reader, writer, file,layoutFactory,appenderFactory);

            IEngine engine = new Engine(logger, reader, writer);

            engine.Run();

        }
        private static ILogger SetUpLogger(int appendersCount, IReader reader, IWriter writer, IFile file, LayoutFactory layoutFactory, AppenderFactory appenderFactory)
        {
            ICollection<IAppender> appenders = new List<IAppender>();
            for (int i = 0; i < appendersCount; i++)
            {
                List<string> appendersInfo = reader.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                string appenderType = appendersInfo[0];
                string layoutType = appendersInfo[1];

                bool hasError = false;
                Level level = ParseLevel(appendersInfo, writer, ref hasError);
                if (hasError)
                {
                    continue;
                }
                try
                {
                    ILayout layout = layoutFactory.CreateLayout(layoutType);
                    IAppender appender = appenderFactory.CreateAppender(appenderType, layout, level, file);

                    appenders.Add(appender);
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                }
            }

            ILogger logger = new Logger(appenders);
            return logger;
        }
        private static Level ParseLevel(List<string> levelString, IWriter writer, ref bool hasError)
        {
            Level appenderLevel = Level.INFO;

            if (levelString.Count == 3)
            {
                bool isEnumValid = Enum.TryParse(typeof(Level), levelString[2], true, out object enumParsed);
                if (!isEnumValid)
                {
                    writer.WriteLine(GlobalConstants.INVALID_LEVEL_TYPE);
                    hasError = true;
                }
                appenderLevel = (Level)enumParsed;
            }
            return appenderLevel;
        }
    }
}
