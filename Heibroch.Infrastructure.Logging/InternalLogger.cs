using Heibroch.Infrastructure.Interfaces.Logging;
using System;

namespace Heibroch.Infrastructure.Logging
{
    public class InternalLogger : IInternalLogger
    {
        public Action<string> LogInfoAction { get; set; } = x => Console.WriteLine(x);
        
        public Action<string> LogWarningAction { get; set; } = x => Console.WriteLine(x);

        public Action<string> LogErrorAction { get; set; } = x => Console.WriteLine(x);

        public void LogError(string message) => LogErrorAction(message);

        public void LogInfo(string message) => LogInfoAction(message);

        public void LogWarning(string message) => LogWarningAction(message);
    }
}
