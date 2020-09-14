using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using Microsoft.Extensions.Logging;

namespace Stars
{
    public class StarsConsoleReporter : ConsoleReporter, ILogger
    {
        public StarsConsoleReporter(IConsole console, bool verbose) : base(console, verbose, false)
        {
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);

            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    base.Error(message);
                    break;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    base.Verbose(message);
                    break;
                case LogLevel.Information:
                    base.Output(message);
                    break;
                case LogLevel.Warning:
                    base.Warn(message);
                    break;
            }


        }
    }
}