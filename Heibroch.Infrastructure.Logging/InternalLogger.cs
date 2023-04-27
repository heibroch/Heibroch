using Heibroch.Infrastructure.Interfaces.Logging;
using System;

namespace Heibroch.Infrastructure.Logging
{
    public class InternalLogger : IInternalLogger
    {
        public bool IsFailingLogging { get; set; }

        public Action<string> LogInfoAction { get; set; } = x => Console.WriteLine(x);        
        public Action<string> LogWarningAction { get; set; } = x => Console.WriteLine(x);
        public Action<string> LogErrorAction { get; set; } = x => Console.WriteLine(x);
        
        public void LogError(string message)
        {
            try
            {
                if (IsFailingLogging) return;
                LogErrorAction(message); 
            } 
            catch
            {
                IsFailingLogging = true;
            }            
        }

        public void LogInfo(string message)
        {
            try
            {
                if (IsFailingLogging) return;
                LogInfoAction(message);
            }
            catch
            {
                IsFailingLogging = true;
            }
        }

        public void LogWarning(string message)
        {
            try
            {
                if (IsFailingLogging) return;
                LogWarningAction(message);
            }
            catch
            {
                IsFailingLogging = true;
            }            
        }
    }
}
