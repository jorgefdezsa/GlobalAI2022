using System.Collections.Generic;

namespace ViejadelVisilloBot.Services.Audio
{
    public interface IAudioService
    {
        List<string> ExtractAudio(string source);

    }
}
