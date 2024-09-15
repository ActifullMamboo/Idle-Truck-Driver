using System.Reflection;
using UnityEngine;

namespace MamboSdk.Core
{
    public static class MambooLog
    {
        private static ILogger _logger = new ConsoleLogger();
        
        public static void LogDebug(string tag, string message)
            => Log(tag, message, LogType.Log);

        public static void LogError(string tag, string message)
            => Log(tag, message, LogType.Error);

        public static void LogWarning(string tag, string message)
            => Log(tag, message, LogType.Warning);

        static void Log(string tag, string message, LogType logType)
            => _logger.Log(Format(tag, message), logType);
        
        static string Format(string tag, string message)
        {
            return $"MambooSDK [{tag}]: {message}";
        }
    }
}