// <copyright file="JoinCallController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Communications.Common.Telemetry;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ViejadelVisilloBot.Model.Constants;
using ViejadelVisilloBot.Model.Models;
using ViejadelVisilloBot.Service.Settings;
using ViejadelVisilloBot.Services.Bot;
using ViejadelVisilloBot.Services.Logging;

namespace ViejadelVisilloBot.Services.Controllers
{
    /// <summary>
    /// JoinCallController is a third-party controller (non-Bot Framework) that can be called in CVI scenario to trigger the bot to join a call.
    /// </summary>
    public class JoinCallController : ControllerBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly IGraphLogger _logger;

        /// <summary>
        /// The bot service
        /// </summary>
        private readonly IBotService _botService;

        /// <summary>
        /// The bot configuration
        /// </summary>
        private readonly BotConfiguration botConfiguration;

        private ILogger<LogBase> ailogger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinCallController" /> class.

        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        /// <param name="botService">The bot service.</param>
        /// <param name="settings">The bot configuration.</param>
        public JoinCallController(IBotService botService, IOptions<BotConfiguration> botConfiguration,
            IGraphLogger logger, ILogger<LogBase> ailogger)
        {
            _logger = logger;
            _botService = botService;
            this.botConfiguration = botConfiguration.Value;
            this.ailogger = ailogger;
        }

        /// <summary>
        /// The join call async.
        /// </summary>
        /// <param name="joinCallBody">The join call body.</param>
        /// <returns>The <see cref="HttpResponseMessage" />.</returns>
        //[SwaggerOperation(
        //Summary = "Join Call",
        //    Description = "",
        //    Tags = new[] { "Calls" }
        //)]
        //[SwaggerResponse(StatusCodes.Status200OK, "Join Call Successfully", typeof(JoinURLResponse))]
        //[SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data", typeof(ValidationProblemDetails))]
        //[SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null)]
        //[HttpPost]
        //[Route(HttpRouteConstants.JoinCall)]
        //public async Task<IActionResult> JoinCallAsync([FromBody] JoinCallBody joinCallBody)
        //{
        //    ailogger.LogInformation($"SMARTBOT | JoinCallController | JoinCallAsync");

        //    try
        //    {
        //        var call = await _botService.JoinCallAsync(joinCallBody).ConfigureAwait(false);
        //        var callPath = $"/{HttpRouteConstants.CallRoute.Replace("{callLegId}", call.Id)}";
        //        var callUri = $"{botConfiguration.ServiceCname}{callPath}";

        //        var values = new JoinURLResponse()
        //        {
        //            Call = callUri,
        //            CallId = call.Id,
        //            ScenarioId = call.ScenarioId,
        //            Logs = callUri.Replace("/calls/", "/logs/")
        //        };

        //        var json = JsonConvert.SerializeObject(values);

        //        return Ok(values);
        //    }
        //    catch (ServiceException e)
        //    {
        //        HttpResponseMessage response = (int)e.StatusCode >= 300
        //            ? new HttpResponseMessage(e.StatusCode)
        //            : new HttpResponseMessage(HttpStatusCode.InternalServerError);

        //        if (e.ResponseHeaders != null)
        //        {
        //            foreach (var responseHeader in e.ResponseHeaders)
        //            {
        //                response.Headers.TryAddWithoutValidation(responseHeader.Key, responseHeader.Value);
        //            }
        //        }

        //        response.Content = new StringContent(e.ToString());
        //        return StatusCode(500, e.ToString());
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Error(e, $"Received HTTP {this.Request.Method}, {this.Request.Path.Value}");
        //        return StatusCode(500, e.Message);
        //    }
        //}


        [SwaggerOperation(
        Summary = "Join Call",
            Description = "",
            Tags = new[] { "Calls" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Join Call Successfully", typeof(JoinURLResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data", typeof(ValidationProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null)]
        [HttpPost]
        [Route(HttpRouteConstants.JoinCall)]
        public async Task<IActionResult> JoinCallAsync([FromBody] JoinCallBody joinCallBody)
        {
            ailogger.LogInformation($"SMARTBOT | JoinCallController | JoinCallAsync");

            try
            {
                var call = await _botService.JoinCallAsync(joinCallBody).ConfigureAwait(false);
                var callPath = $"/{HttpRouteConstants.CallRoute.Replace("{callLegId}", call.Id)}";
                var callUri = $"{botConfiguration.ServiceCname}{callPath}";

                var values = new JoinURLResponse()
                {
                    Call = callUri,
                    CallId = call.Id,
                    ScenarioId = call.ScenarioId,
                    Logs = callUri.Replace("/calls/", "/logs/")
                };

                var json = JsonConvert.SerializeObject(values);

                return Ok(values);
            }
            catch (ServiceException e)
            {
                HttpResponseMessage response = (int)e.StatusCode >= 300
                    ? new HttpResponseMessage(e.StatusCode)
                    : new HttpResponseMessage(HttpStatusCode.InternalServerError);

                if (e.ResponseHeaders != null)
                {
                    foreach (var responseHeader in e.ResponseHeaders)
                    {
                        response.Headers.TryAddWithoutValidation(responseHeader.Key, responseHeader.Value);
                    }
                }

                response.Content = new StringContent(e.ToString());
                return StatusCode(500, e.ToString());
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Received HTTP {this.Request.Method}, {this.Request.Path.Value}");
                return StatusCode(500, e.Message);
            }
        }

    }
}
