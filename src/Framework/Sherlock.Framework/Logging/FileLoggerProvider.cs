using Microsoft.Extensions.Logging;
using Sherlock.Framework.FileSystem.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.Framework.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _folder;
        private int _backlogSize;
        public FileLoggerProvider(string folderName = "logs", Func<string, LogLevel, bool> filter= null, int backlogSize = 10 * 1024)
        {
            this._filter = filter;
            this._folder = folderName.IfNullOrWhiteSpace("logs");
            this._backlogSize = backlogSize;
        }

        public ILogger CreateLogger(string name)
        {
            return new FileLogger(name, _filter, _folder, _backlogSize);
        }

        public void Dispose()
        {
             
        }
    }
}
