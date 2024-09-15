using UnityEngine;

namespace MamboSdk.Core
{
    public interface ILogger
    {
        void Log(string message, LogType logType);
    }
}