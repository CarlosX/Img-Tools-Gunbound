/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       FileDecoder.cs
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
using System.Windows.Forms;

namespace ImgTools
{
    public abstract class FileDecoder
    {

        private string m_Extension;
        private string m_Title;

        private static FileDecoder[] m_Decoders;

        public string Extension
        {
            get
            {
                return m_Extension;
            }
        }

        public string Title
        {
            get
            {
                return m_Title;
            }
        }

        public static FileDecoder[] Decoders
        {
            get
            {
                return FileDecoder.m_Decoders;
            }
        }

        public FileDecoder(string title, string extension)
        {
            m_Title = title;
            m_Extension = extension;
        }

        static FileDecoder()
        {
            FileDecoder[] fileDecoderArr = new FileDecoder[] {
                                                               new ImageDecoder(".img")
                                                               };
            FileDecoder.m_Decoders = fileDecoderArr;
        }

        public abstract void FillPanel(ArchivedFile file, Panel pn);

        public virtual string GetType(ArchivedFile file)
        {
            return m_Title;
        }

        public static FileDecoder FindDecoder(string extension)
        {
            for (int i = 0; i < FileDecoder.m_Decoders.Length; i++)
            {
                if (FileDecoder.m_Decoders[i].Extension == extension)
                {
                    Console.WriteLine(extension);
                    return FileDecoder.m_Decoders[i];
                }
            }
            return FileDecoder.m_Decoders[FileDecoder.m_Decoders.Length - 1];
        }

    } // class FileDecoder
}