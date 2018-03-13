using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Context;

namespace Ensemble.Logging
{
    public class SerilogLogger : ILogger
    {
        static Serilog.ILogger _serilogLogger = null;
        public SerilogLogger()
        {
        }
        public SerilogLogger(IConfiguration configuration)
        {
            _serilogLogger = new LoggerConfiguration()
                                   .ReadFrom.Configuration(configuration)
                                   .CreateLogger();
        }
        public ILogger ForContext(Type type, IConfiguration configuration)
        {
            _serilogLogger = _serilogLogger.ForContext(type);
            string correlationId = configuration.GetSection("Serilog:Properties:CorrelationId").Value;
            LogContext.PushProperty("CorrelationId", correlationId);

            //Uncommented
            string destinationAppName = configuration.GetSection("Serilog:Properties:DestinationAppName").Value;
            LogContext.PushProperty("DestinationAppName", destinationAppName);

            return this;
        }
        public void Error(object message, Exception ex)
        {
            _serilogLogger.Error(ex, ex.Message);
        }
        public void Error(Exception ex)
        {
            //Print Message and Stack Trace
            _serilogLogger.Error(string.Format("Message: {0} Stack Trace: {1}", ex.Message, ex.StackTrace));
            //_serilogLogger.Error(string.Format("Stack Trace: {0}", ex.StackTrace));
        }
        public void Info(object message)
        {
            _serilogLogger.Information(message.ToString());
        }
        public void Info(object message, Exception ex)
        {
            _serilogLogger.Information(ex, message.ToString());
        }
    }
}
