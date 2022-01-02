using _01._Logger.IOManagement;
using _01._Logger.Models.Contracts;
using _01._Logger.Models.Enumerations;
using _01._Logger.Models.IOManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01._Logger.Models.Appenders
{
    public class FileAppender : Appender
    {
        private readonly IWriter writer;
        public FileAppender(ILayout layout, Level level, IFile file) : base(layout, level)
        {
            this.File = file;

            this.writer = new FileWriter(this.File.Path);
        }
        public IFile File { get; }

        public override void Append(IError error)
        {
            string formattedMessages = this.File.Write(this.Layout, error);

            this.writer.WriteLine(formattedMessages);
            this.messagesAppended++;
        }

        public override string ToString()
        {
            return base.ToString() + $", File size {this.File.Size}";
        }
    }
}
