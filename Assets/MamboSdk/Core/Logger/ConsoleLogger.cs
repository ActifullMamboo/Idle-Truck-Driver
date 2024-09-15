using UnityEngine;

namespace MamboSdk.Core
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}