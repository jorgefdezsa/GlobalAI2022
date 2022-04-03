using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ProcessSpeechText.Extensions;
using System;
using System.Collections.Generic;

[assembly: FunctionsStartup(typeof(ProcessSpeechText.Startup))]

namespace ProcessSpeechText
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = config.Build();
            builder.Services.AddOptions();
            builder.Services.AddSingleton<IConfiguration>(configuration);
            builder.Services.Configure<SettingsFx>(configuration.GetSection("SettingsFx"));

            builder.Services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));

            ApplicationLogging.Configuration = configuration;
            ApplicationLogging.ConfigureLogger();
        }
    }
}
