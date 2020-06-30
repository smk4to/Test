using NLog;
using NLog.Config;
using NLog.Targets;

namespace Asd_Logging
{
    public static class Logging
    {
        private static void Configure()
        {
            LogManager.Configuration = GetConfig();
        }

        private static LoggingConfiguration GetConfig()
        {
            const string layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss\\:ffff} | ${uppercase:${level}} | ${message}";
            const string subFileName = @"${basedir}/Logs/${level}/${date:format=yyyy-MM-dd}.log";
            const string fileName = @"${basedir}/Logs/${date:format=yyyy-MM-dd}.log";

            var config = new LoggingConfiguration();

            var targetTrace = new FileTarget { FileName = subFileName, Layout = layout };
            config.AddTarget("Trace", targetTrace);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, LogLevel.Trace, targetTrace));

            var targetDebug = new FileTarget { FileName = subFileName, Layout = layout + "\r\n\r\n" };
            config.AddTarget("Debug", targetDebug);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, LogLevel.Debug, targetDebug));

            var targetInfo = new FileTarget { FileName = fileName, Layout = layout + "\r\n\r\n\r\n" };
            config.AddTarget("Info", targetInfo);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, LogLevel.Info, targetInfo));

            var targetWarn = new FileTarget { FileName = fileName, Layout = layout + "\r\n\r\n\r\n" };
            config.AddTarget("Warn", targetWarn);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, LogLevel.Warn, targetWarn));

            var targetError = new FileTarget { FileName = fileName, Layout = layout + "\r\n\r\n\r\n" };
            config.AddTarget("Error", targetError);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, LogLevel.Error, targetError));

            var targetFatal = new FileTarget { FileName = fileName, Layout = layout + "\r\n\r\n\r\n" };
            config.AddTarget("Fatal", targetFatal);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Fatal, LogLevel.Fatal, targetFatal));

            return config;
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Trace(string message)
        {
            Configure();
            Logger.Trace(message);
        }
        public static void Debug(string message)
        {
            Configure();
            Logger.Debug(message);
        }
        public static void Request(string message)
        {
            Configure();
            Logger.Info(message);
        }
        public static void Info(string message)
        {
            Configure();
            Logger.Info(message);
        }
        public static void Response(string message)
        {
            Configure();
            Logger.Info(message);
        }
        public static void Warn(string message)
        {
            Configure();
            Logger.Warn(message);
        }
        public static void Error(string message)
        {
            Configure();
            Logger.Error(message);
        }
        public static void Exception(string message)
        {
            Configure();
            Logger.Fatal(message);
        }
    }
}
