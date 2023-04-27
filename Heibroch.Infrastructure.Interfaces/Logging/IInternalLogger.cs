using System;

namespace Heibroch.Infrastructure.Interfaces.Logging
{
    public interface IInternalLogger
    {
        bool IsFailingLogging { get; set; }

        void LogInfo(string message);

        void LogWarning(string message);

        void LogError(string message);

        Action<string> LogInfoAction { get; set; }

        Action<string> LogWarningAction { get; set; }

        Action<string> LogErrorAction { get; set; }
    }
}
