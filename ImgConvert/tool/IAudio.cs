using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ImgConvert
{
    public interface IAudio
    {
        void Dispose();
        void Pause();
        void Play(bool loop);
        void Reset();
        void SaveFile(Stream fs);
        void Stop();

        string FileName { get; }

        string FilterString { get; }

        TimeSpan Length { get; }

        bool Playing { get; }

        TimeSpan Position { get; set; }
    }
}
