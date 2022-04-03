namespace ViejadelVisilloBot.Services.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph.Communications.Common.Telemetry;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ViejadelVisilloBot.Model.Constants;
    using ViejadelVisilloBot.Model.Models;
    using ViejadelVisilloBot.Services.Audio;
    using ViejadelVisilloBot.Services.Bot;
    using ViejadelVisilloBot.Services.Logging;

    public class ExtractAudioController : ControllerBase
    {
        private readonly IGraphLogger logger;

        private readonly HttpClient client;
        private ILogger<LogBase> ailogger { get; set; }

        private readonly IAudioService audioService;

        private readonly IBotService botService;

        public ExtractAudioController(IBotService botService, IAudioService audioService, IGraphLogger logger, ILogger<LogBase> ailogger)
        {
            this.logger = logger;
            this.botService = botService;
            this.audioService = audioService; 
            this.ailogger = ailogger;
            this.client = new HttpClient();
        }

        [SwaggerOperation(
            Summary = "Extract Audio",
            Description = "",
            Tags = new[] { "Audio" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Extract Audio Successfully", typeof(List<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data", typeof(ValidationProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null)]
        [HttpGet]
        [Route(HttpRouteConstants.ExtractAudioPrefix + "/")]
        public IActionResult ExtractAudio()
        {
            logger.Info("Extract audios");

            var wavs =  audioService.ExtractAudio("C:\\Temp\\Recordings");
           
            var calls = new List<Dictionary<string, string>>();
            //foreach (var callHandler in _botService.CallHandlers.Values)
            //{
            //    var call = callHandler.Call;
            //    var callPath = "/" + HttpRouteConstants.CallRoute.Replace("{callLegId}", call.Id);
            //    var callUri = new Uri(botConfiguration.CallControlBaseUrl, callPath).AbsoluteUri;
            //    var values = new Dictionary<string, string>
            //    {
            //        { "legId", call.Id },
            //        { "scenarioId", call.ScenarioId.ToString() },
            //        { "call", callUri },
            //        { "logs", callUri.Replace("/calls/", "/logs/") },
            //    };
            //    calls.Add(values);
            //}
            return Ok(wavs);
        }




        [SwaggerOperation(
        Summary = "Process Audio",
            Description = "",
            Tags = new[] { "Audio" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Process Audio Successfully", typeof(List<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data", typeof(ValidationProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null)]
        [HttpPost]
        [Route(HttpRouteConstants.ExtractAudioPrefix + "/process")]
        public async Task<IActionResult> ProcessAudioAsync([FromBody] ProcessAudio processAudio)
        {
            ailogger.LogInformation($"SMARTBOT | ExtractAudioController | ProcessAudioAsync");

            var values = new List<string>();

            try
            {
                if (processAudio != null && !string.IsNullOrEmpty(processAudio.FileName))
                {
                    var processAudioFxUri = $"http://localhost:7071/api/RecognizeSpeechText?fileName={processAudio.FileName}";

                    HttpResponseMessage responseFx = await this.client.GetAsync(processAudioFxUri);

                    responseFx.EnsureSuccessStatusCode();
                    string responseBody = await responseFx.Content.ReadAsStringAsync();

                     values = JsonConvert.DeserializeObject<List<string>>(responseBody);

                }

                var json = JsonConvert.SerializeObject(values);

                return Ok(values);
            }

            catch (Exception e)
            {
                logger.Error(e, $"Received HTTP {this.Request.Method}, {this.Request.Path.Value}");
                return StatusCode(500, e.Message);
            }
        }

    }
}
