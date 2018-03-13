using System;

namespace Ensemble.Logging
{
    public interface ILogger
    {
        void Info(object message);
        void Info(object message, Exception exception);
        void Error(Exception message);
        void Error(object message, Exception exception);
    }
}
