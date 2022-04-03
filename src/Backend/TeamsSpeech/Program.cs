namespace TeamsSpeech
{
    using Microsoft.CognitiveServices.Speech;
    using Microsoft.CognitiveServices.Speech.Audio;
    using System;
    using System.Threading.Tasks;
    using TeamsSpeech.Helpers;

    class Program
    {
        static void Main(string[] args)
        {
            var prompt = "Your choice (0: Stop): ";

            Console.WriteLine("1. Speech recognition with microphone input.");
            Console.WriteLine("2. Speech recognition in the specified language and using detailed output format.");
            Console.WriteLine("3. Speech continuous recognition with file input.");
            Console.WriteLine("4. Speech recognition using customized model.");

            Console.Write(prompt);

            RecognitionWithAutoDetectSourceLanguageAsync().Wait();
        }

        public static async Task RecognitionAudioStreamAsync()
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("YouSubscriptionKey", "YourRegion");

            var stopRecognition = new TaskCompletionSource<int>();

            // Create an audio stream from a wav file.
            // Replace with your own audio file name.
            using (var audioInput = Helper.OpenWavFile(@"c:\\yourfile.wav"))
            {
                // Creates a speech recognizer using audio stream input.
                using (var recognizer = new SpeechRecognizer(config, audioInput))
                {
                    // Subscribes to events.
                    recognizer.Recognizing += (s, e) =>
                    {
                        Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
                    };

                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        }
                    };

                    recognizer.Canceled += (s, e) =>
                    {
                        Console.WriteLine($"CANCELED: Reason={e.Reason}");

                        if (e.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }

                        stopRecognition.TrySetResult(0);
                    };

                    recognizer.SessionStarted += (s, e) =>
                    {
                        Console.WriteLine("\nSession started event.");
                    };

                    recognizer.SessionStopped += (s, e) =>
                    {
                        Console.WriteLine("\nSession stopped event.");
                        Console.WriteLine("\nStop recognition.");
                        stopRecognition.TrySetResult(0);
                    };

                    // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                    await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                    // Waits for completion.
                    // Use Task.WaitAny to keep the task rooted.
                    Task.WaitAny(new[] { stopRecognition.Task });

                    // Stops recognition.
                    await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                }
            }
        }

        public static async Task RecognitionWithAutoDetectSourceLanguageAsync()
        {
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("YouSubscriptionKey", "YourRegion");

            // Creates an instance of AutoDetectSourceLanguageConfig with the 2 source language candidates
            // Currently this feature only supports 2 different language candidates
            // Replace the languages to be the language candidates for your speech. Please see https://docs.microsoft.com/azure/cognitive-services/speech-service/language-support for all supported languages
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new string[] { "es-ES" });

            var stopRecognition = new TaskCompletionSource<int>();

            // Creates a speech recognizer using the auto detect source language config, and the file as audio input.
            // Replace with your own audio file name.
            using (var audioInput = AudioConfig.FromWavFileInput(@"c:\\YourFile.wav"))
            {
                using (var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig, audioInput))
                {
                    // Subscribes to events.
                    recognizer.Recognizing += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizingSpeech)
                        {
                            Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
                            // Retrieve the detected language
                            var autoDetectSourceLanguageResult = AutoDetectSourceLanguageResult.FromResult(e.Result);
                            Console.WriteLine($"DETECTED: Language={autoDetectSourceLanguageResult.Language}");
                        }
                    };

                    recognizer.Recognized += (s, e) =>
                    {
                        if (e.Result.Reason == ResultReason.RecognizedSpeech)
                        {
                            Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
                            // Retrieve the detected language
                            var autoDetectSourceLanguageResult = AutoDetectSourceLanguageResult.FromResult(e.Result);
                            Console.WriteLine($"DETECTED: Language={autoDetectSourceLanguageResult.Language}");
                        }
                        else if (e.Result.Reason == ResultReason.NoMatch)
                        {
                            Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                        }
                    };

                    recognizer.Canceled += (s, e) =>
                    {
                        Console.WriteLine($"CANCELED: Reason={e.Reason}");

                        if (e.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }

                        stopRecognition.TrySetResult(0);
                    };

                    recognizer.SessionStarted += (s, e) =>
                    {
                        Console.WriteLine("\n    Session started event.");
                    };

                    recognizer.SessionStopped += (s, e) =>
                    {
                        Console.WriteLine("\n    Session stopped event.");
                        Console.WriteLine("\nStop recognition.");
                        stopRecognition.TrySetResult(0);
                    };

                    // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                    await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                    // Waits for completion.
                    // Use Task.WaitAny to keep the task rooted.
                    Task.WaitAny(new[] { stopRecognition.Task });

                    // Stops recognition.
                    await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                }
            }
        }

    }
}
