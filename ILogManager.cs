using System;
using System.Collections.Generic;
using System.Text;

namespace Ensemble.Logging
{
    public interface ILogManager
    {
        ILogger GetLogger();
        ILogger GetLogger(Type type);
    }
}
