// <copyright file="Startup.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Communications.Common.Telemetry;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using ViejadelVisilloBot.Service.Settings;
using ViejadelVisilloBot.Services.Audio;
using ViejadelVisilloBot.Services.Bot;
using ViejadelVisilloBot.Services.Logging;

namespace ViejadelVisilloBot.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options =>

            {

                options.AddPolicy("AllowAllOrigins",

                    currentbuilder =>

                    {

                        currentbuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

                    });

            });
            services.AddSingleton<IGraphLogger, GraphLogger>(_ => new GraphLogger("PsiBot", redirectToTrace: true));
            services.AddSingleton<InMemoryObserver, InMemoryObserver>();
            services.AddSingleton<IAudioService, AudioService>();
            services.Configure<BotConfiguration>(Configuration.GetSection(nameof(BotConfiguration)));
            services.PostConfigure<BotConfiguration>(config => config.Initialize());
            services.AddSingleton<IBotService, BotService>(provider =>
            {
                var bot = new BotService(
                    provider.GetRequiredService<IGraphLogger>(),
                    provider.GetRequiredService<IOptions<BotConfiguration>>(),
                    provider.GetRequiredService<ILogger<LogBase>>());
                bot.Initialize();
                return bot;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Junta de Vecinos",
                    Version = "v1",
                    Description = "Descripción de la API de Junta de Vecinos",
                    Contact = new OpenApiContact
                    {
                        Email = "info@viejadelvisillo.com",
                        Name = "JUNTADEVECINOS",
                        Url = new Uri("https://www.encamina.com")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                                    Enter 'Bearer' [space] and then your token in the text input below.
                                    \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
            });
            var expectedInstrumentationKey = Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
            services.AddApplicationInsightsTelemetry(expectedInstrumentationKey);
        }

        public void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            var expectedInstrumentationKey = Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
            var seriLogger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.ApplicationInsights(expectedInstrumentationKey, TelemetryConverter.Events)
                .WriteTo.ApplicationInsights(expectedInstrumentationKey, TelemetryConverter.Traces)
                .CreateLogger();
            loggerFactory.AddSerilog(seriLogger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ConfigureLogger(loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Junta de Vecinos");
            });

            //app.UseHttpsRedirection();
            //app.UseExceptionHandler();
            app.UseMvc();
        }
    }
}
