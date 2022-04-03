// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Psi.TeamsBot
{
    using Microsoft.Psi.Audio;
    using Microsoft.Psi.Data;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using PsiImage = Microsoft.Psi.Imaging.Image;

    /// <summary>
    /// Teams Bot test runner.
    /// </summary>
    public class Program
    {
        private static ITeamsBot CreateTeamsBot(Pipeline pipeline)
        {
            // create your Teams bot \psi component
            return new ParticipantEngagementScaleBot(pipeline, TimeSpan.FromSeconds(1.0 / 15.0), 1920, 1080, true);
        }

        private static PsiImporter OpenStore(Pipeline pipeline, string folder, string path)
        {
            return PsiStore.Open(pipeline, folder, path);
        }

        [STAThread]
        private static void Main(string[] args)
        {
            var source = string.Empty;
            var target = string.Empty;

            source = args[0];
            target = args[1];

            Console.WriteLine($"source: {source}");
            Console.WriteLine($"target: {target}");

            var folders = new List<string>();

            DirectoryInfo di = new DirectoryInfo(source);

            DirectoryInfo[] diArr = di.GetDirectories();

            Console.WriteLine($"{diArr.Length} files found");

            var i = 0;

            // Display the names of the directories.
            foreach (DirectoryInfo dri in diArr)
            {
                i++;
                Console.WriteLine($"Extracting audio {i} of {diArr.Length} | {dri.Name}");
                if (dri.Name.Contains(".0000"))
                {
                    folders.Add((dri.Name.Split('.')[0]));
                }
                else
                {
                    Console.WriteLine($"Invalid folder {dri.Name}");
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

                        var fileName = $"{folder}.wav";
                        var fullPath = Path.Combine(target, fileName);
#pragma warning disable CA2000 // Dispose objects before losing scope
                        var waveFileWriter = new WaveFileWriter(pipeline, fullPath);
#pragma warning restore CA2000 // Dispose objects before losing scope
                        audioWaveFileWriter.Out.PipeTo(waveFileWriter.In);

#pragma warning disable CA2000 // Dispose objects before losing scope
                        var teamsBot = CreateTeamsBot(pipeline);
#pragma warning restore CA2000 // Dispose objects before losing scope
                        audio.PipeTo(teamsBot.AudioIn);

                        pipeline.RunAsync();

                        Console.WriteLine($"{folder} wav Processed...");
                        Thread.Sleep(2000);
                        //Console.ReadKey();
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                        //Console.WriteLine("Done");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                    }
                }
            }
        }
    }
}
