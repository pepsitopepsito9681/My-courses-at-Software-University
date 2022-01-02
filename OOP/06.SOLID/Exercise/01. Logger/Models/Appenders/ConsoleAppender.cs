using _01._Logger.Common;
using _01._Logger.Models.Contracts;
using _01._Logger.Models.Enumerations;
using _01._Logger.Models.IOManagement;
using _01._Logger.Models.IOManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01._Logger.Models.Appenders
{
    public class ConsoleAppender : Appender
    {
        private readonly IWriter writer;

        public ConsoleAppender(ILayout layout, Level level) : base(layout, level)
        {
            this.writer = new ConsoleWriter();
        }

        public override void Append(IError error)
        {
            string format = this.Layout.Format;

            DateTime dateTime = error.DateTime;
            string message = error.Message;
            Level level = error.Level;

            string formattedString = string.Format(format, dateTime.ToString(GlobalConstants.DateTimeFormat), level.ToString(), message);

            this.writer.WriteLine(formattedString);
            this.messagesAppended++;
        }
    }
}
