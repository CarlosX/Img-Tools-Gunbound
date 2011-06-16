/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       Animation.cs
 *  Author:     CARLOSX
 *  ----------------------------------------------------------------------------
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImgTools
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