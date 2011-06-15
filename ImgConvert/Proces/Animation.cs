using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImgConvert
{
    public class Animation
    {

        private int[] m_Durations;
        private Frame[] m_Frames;
        private int m_TotalDuration;



        public Animation(Frame[] frames, int[] durations)
        {
            this.m_Frames = frames;
            this.m_Durations = durations;
            for (int i = 0; i < frames.Length; i++)
            {
                this.m_TotalDuration += this.m_Durations[i];
            }
        }

        public Frame GetFrame(int time)
        {
            if ((this.m_Frames.Length != 0) && (this.m_TotalDuration != 0))
            {
                time = time % this.m_TotalDuration;
                for (int i = 0; i < this.m_Frames.Length; i++)
                {
                    Frame frame = this.m_Frames[i];
                    if (time < this.m_Durations[i])
                    {
                        return frame;
                    }
                    time -= this.m_Durations[i];
                }
            }
            return null;
        }
        public int[] Durations
        {
            get
            {
                return m_Durations;
            }
        }

        public Frame[] Frames
        {
            get
            {
                return m_Frames;
            }
        }

        public int TotalDuration
        {
            get
            {
                return m_TotalDuration;
            }
        }

    } // class Animation

}
