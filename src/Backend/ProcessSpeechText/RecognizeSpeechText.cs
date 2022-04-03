namespace ProcessSpeechText
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.CognitiveServices.Speech;
    using Microsoft.CognitiveServices.Speech.Audio;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RecognizeSpeechText
    {
        private readonly SettingsFx settings;
        private const string functionName = "RecognizeSpeechText";


        public RecognizeSpeechText(IOptions<SettingsFx> settings)
        {
            this.settings = settings.Value;
        }

        [FunctionName(functionName)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string fileName = req.Query["fileName"];

            var words = await RecognitionWithAutoDetectSourceLanguageAsync(fileName);

            if (words != null && words.Any())
            {
                return new OkObjectResult(
                    words.LastOrDefault()
                    .Replace(".","")
                    .Replace(",","").Split(" ").ToList());
            }

            return new OkObjectResult(string.Empty);
        }

        private async Task<List<string>> RecognitionWithAutoDetectSourceLanguageAsync(string fileName)
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription(settings.SubscritionKey, settings.Region);

            var words = new List<string>();

            // Creates an instance of AutoDetectSourceLanguageConfig with the 2 source language candidates
            // Currently this feature only supports 2 different language candidates
            // Replace the languages to be the language candidates for your speech. Please see https://docs.microsoft.com/azure/cognitive-services/speech-service/language-support for all supported languages
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new string[] { "es-ES" });

            var stopRecognition = new TaskCompletionSource<int>();

            // Creates a speech recognizer using the auto detect source language config, and the file as audio input.
            // Replace with your own audio file name.
            using (var audioInput = AudioConfig.FromWavFileInput(@"C:\\" + fileName))
            {
                using (var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig, audioInput))
                {
                    // Subscribes to events.
                    recognizer.Recognizing += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizingSpeech)
                        {
                            words.Add(e.Result.Text);
                            //Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
                            // Retrieve the detected language
                            var autoDetectSourceLanguageResult = AutoDetectSourceLanguageResult.FromResult(e.Result);
                            //Console.WriteLine($"DETECTED: Language={autoDetectSourceLanguageResult.Language}");
                        }
                    };

                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            words.Add(e.Result.Text);
                            // Retrieve the detected language
                            var autoDetectSourceLanguageResult = AutoDetectSourceLanguageResult.FromResult(e.Result);
                            //Console.WriteLine($"DETECTED: Language={autoDetectSourceLanguageResult.Language}");
                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            //Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        }
                    };

                    recognizer.Canceled += (s, e) =>
                    {
                        //Console.WriteLine($"CANCELED: Reason={e.Reason}");

                        if (e.Reason == CancellationReason.Error)
                        {
                            //Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                            //Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                            //Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }

                        stopRecognition.TrySetResult(0);
                    };

                    recognizer.SessionStarted += (s, e) =>
                    {
                        //Console.WriteLine("\n    Session started event.");
                    };

                    recognizer.SessionStopped += (s, e) =>
                    {
                        //Console.WriteLine("\n    Session stopped event.");
                        //Console.WriteLine("\nStop recognition.");
                        stopRecognition.TrySetResult(0);
                    };

                    // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                    await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                    // Waits for completion.
                    // Use Task.WaitAny to keep the task rooted.
                    Task.WaitAny(new[] { stopRecognition.Task });

                    // Stops recognition.
                    await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

                    return words;
                }
            }
        }


    }
}
