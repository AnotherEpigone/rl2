using System;

namespace Roguelike2.Logging
{
    public enum LogType
    {
        Debug,
        Error,
        Gameplay,
    }

    public interface ILogger
    {
        void RegisterEventListener(LogType type, Action<string> listener);
        void UnregisterEventListener(LogType type, Action<string> listener);
        void Log(string message, LogType logType);
        void Debug(string message);
        void Gameplay(string message);
    }
}
