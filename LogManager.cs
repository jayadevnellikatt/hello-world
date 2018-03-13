using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ensemble.Logging
{
    public class LogManager : ILogManager
    {
        public static IConfiguration Configuration { get; set; }
        public LogManager(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static ILogger GetLogger()
        {
            //Read the value from the config file for the  Mode of Loggging
            //This value is read from the client application's Jason file / configuration file
            string logMode = Configuration.GetSection("LoggingMode").Value;
            ILogger logger = null;
            switch (logMode)
            {
                case "SeriLog":
                    logger = new SerilogLogger(Configuration);
                    break;
                default:
                    break;
            }
            return logger;
        }
        public static ILogger GetLogger(Type type)
        {
            string logMode;
            try
            {
                logMode = Configuration.GetSection("LoggingMode").Value;
            }
            catch(Exception ex)
            {
                logMode = "SeriLog";
            }
            //string logMode = "SeriLog";
            ILogger logger = null;
            //string correlationId = Configuration.GetSection("Serilog:Properties:CorrelationId").Value;
            switch (logMode)
            {
                case "SeriLog":
                    logger = new SerilogLogger().ForContext(type, Configuration);
                    break;
                default:
                    break;
            }
            return logger;
        }
        public static void SetLogProperties(Dictionary<string, string> properties)
        {
            foreach (var key in properties.Keys)
            {
                switch (key)
                {
                    case "CorrelationId":
                        Configuration.GetSection("Serilog:Properties:CorrelationId").Value = properties[key].ToString();
                        break;
                    case "SourceAppName":
                        Configuration.GetSection("Serilog:Properties:SourceAppName").Value = properties[key].ToString();
                        break;
                    case "DestinationAppName":
                        Configuration.GetSection("Serilog:Properties:DestinationAppName").Value = properties[key].ToString();
                        break;
                    default:
                        break;
                }
            }
        }
        ILogger ILogManager.GetLogger()
        {
            return GetLogger();
        }
        ILogger ILogManager.GetLogger(Type type)
        {
            return GetLogger(type);
        }
    }
}
