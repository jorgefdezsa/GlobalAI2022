namespace ViejadelVisilloBot.Services.Audio
{
    using Microsoft.Psi;
    using Microsoft.Psi.Audio;
    using Microsoft.Psi.Data;
    using Microsoft.Psi.TeamsBot;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;

    public class AudioService : IAudioService
    {
        private readonly HttpClient httpClient;

        public AudioService()
        {
            this.httpClient = new HttpClient();
        }

        public List<string> ExtractAudio(string source)
        {
            var folders = new List<string>();

            var response = new List<string>();

            DirectoryInfo di = new(source);

            DirectoryInfo[] diArr = di.GetDirectories();

            var i = 0;

            // Display the names of the directories.
            foreach (DirectoryInfo dri in diArr)
            {
                i++;
                //Console.WriteLine($"Extracting audio {i} of {diArr.Length} | {dri.Name}");
                if (dri.Name.Contains(".0000"))
                {
                    folders.Add((dri.Name.Split('.')[0]));
                }
                else
                {
                    //Console.WriteLine($"Invalid folder {dri.Name}");
                }
            }

            foreach (var folder in folders)
            {
                using (var pipeline = Pipeline.Create())
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    var store = OpenStore(pipeline, folder, source);
#pragma warning restore CA2000 // Dispose objects before losing scope

                    var participantAudioStreams = new List<IProducer<(string, AudioBuffer)>>();

                    IProducer<Dictionary<string, (AudioBuffer, DateTime)>> audio = null;

                    foreach (var streamMetadata in store.AvailableStreams)
                    {
                        var subNames = streamMetadata.Name.Split('.');

                        if (subNames.Length == 3 && subNames[0] == "Participants" && subNames[2] == "Audio")
                        {
                            participantAudioStreams.Add(store
                                .OpenStream<AudioBuffer>(streamMetadata.Name)
                                .Select(ab => (subNames[1], ab)));
                        }
                    }

                    if (participantAudioStreams != null && participantAudioStreams.Any())
                    {

                        audio = Microsoft.Psi.Operators.Merge(participantAudioStreams)
                            .Select(message => new Dictionary<string, (AudioBuffer, DateTime)>() { { message.Data.Item1, (message.Data.Item2, message.OriginatingTime) } });

                        var audioWaveFileWriter = Microsoft.Psi.Operators.Merge(participantAudioStreams).Select(message => message.Data.Item2);

                        //var infoUri = "http://localhost:7071/api/ProcessBuffer";

                        //var buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(audioWaveFileWriter.Out));
                        //var byteContent = new ByteArrayContent(ReadFully(pipeline));
                        //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        //var request = this.httpClient.PostAsync(infoUri, byteContent).Result;
                        //var response = request.Content.ReadAsStringAsync().Result;


                        var fileName = $"{folder}.wav";
                        var fullPath = Path.Combine("c:\\Wavs\\", fileName);
#pragma warning disable CA2000 // Dispose objects before losing scope
                        var waveFileWriter = new WaveFileWriter(pipeline, fullPath);
#pragma warning restore CA2000 // Dispose objects before losing scope
                        audioWaveFileWriter.Out.PipeTo(waveFileWriter.In);

#pragma warning disable CA2000 // Dispose objects before losing scope
                        var teamsBot = CreateTeamsBot(pipeline);
#pragma warning restore CA2000 // Dispose objects before losing scope
                        audio.PipeTo(teamsBot.AudioIn);

                        pipeline.RunAsync();

                        response.Add($"{folder}.wav");

                        Console.WriteLine($"{folder} wav Processed...");
                        Thread.Sleep(2000);
                        //Console.ReadKey();
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                        //Console.WriteLine("Done");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

                    }
                }
            }

            return response;
        }

        private static PsiImporter OpenStore(Pipeline pipeline, string folder, string path)
        {
            return PsiStore.Open(pipeline, folder, path);
        }

        private static ITeamsBot CreateTeamsBot(Pipeline pipeline)
        {
            // create your Teams bot \psi component
            return new ParticipantEngagementScaleBot(pipeline, TimeSpan.FromSeconds(1.0 / 15.0), 1920, 1080, true);
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
