namespace ProcessSpeechText.Extensions
{
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using System.Reflection;

    public static class ApplicationLogging
    {
        private static ILoggerFactory factory = null;

        public static void ConfigureLogger()
        {
#pragma warning disable CS0618 // Type or member is obsolete, serilog documentation still recommends this
            var telemetryConfiguration = TelemetryConfiguration.Active;
#pragma warning restore CS0618 // Type or member is obsolete
            if (telemetryConfiguration == null || string.IsNullOrWhiteSpace(telemetryConfiguration.InstrumentationKey))
            {
                telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                telemetryConfiguration.InstrumentationKey = Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
            }



            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .Enrich.FromLogContext()



                            .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Events)
                            .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)



                            .WriteTo.Console()
                            .CreateLogger();


            LoggerFactory.AddSerilog(Log.Logger);
        }


        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (factory == null)
                {
                    factory = new LoggerFactory();
                }
                return factory;
            }
            set { factory = value; }
        }

        public static IConfiguration Configuration { get; set; } = null;


        public static Microsoft.Extensions.Logging.ILogger CreateLogger() => LoggerFactory.CreateLogger($"{Assembly.GetExecutingAssembly().GetName().Name}");
    }
}
