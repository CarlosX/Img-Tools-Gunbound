/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       ArchivedFile.cs
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
using System.IO;
using System.Windows.Forms;

namespace ImgTools
{
    public class ArchivedFile
    {

        private Archive m_Archive;
        private bool m_Compressed;
        private int m_DiskLength;
        private string m_FileName;
        private TreeNode m_Folder;
        private bool m_Fragmented;
        private int m_Icon;
        private int m_Length;
        private int m_Lookup;
        private TreeNode m_Node;
        private DataStream m_Stream;

        public Archive Archive
        {
            get
            {
                return m_Archive;
            }
            set
            {
                m_Archive = value;
            }
        }

        public bool Compressed
        {
            get
            {
                return m_Compressed;
            }
        }

        public int DiskLength
        {
            get
            {
                return m_DiskLength;
            }
        }

        public string FileName
        {
            get
            {
                return m_FileName;
            }
        }

        public TreeNode Folder
        {
            get
            {
                return m_Folder;
            }
            set
            {
                m_Folder = value;
            }
        }

        public bool Fragmented
        {
            get
            {
                return m_Fragmented;
            }
        }

        public int Icon
        {
            get
            {
                return m_Icon;
            }
            set
            {
                m_Icon = value;
            }
        }

        public int Length
        {
            get
            {
                return m_Length;
            }
        }

        public int Lookup
        {
            get
            {
                return m_Lookup;
            }
        }

        public TreeNode Node
        {
            get
            {
                return m_Node;
            }
            set
            {
                m_Node = value;
            }
        }

        public DataStream Stream
        {
            get
            {
                return m_Stream;
            }
        }

        public ArchivedFile(string fileName, DataStream stream, int lookup, int length, int diskLength, bool compressed, bool fragmented)
        {
            this.m_FileName = fileName;
            this.m_Stream = stream;
            this.m_Lookup = lookup;
            this.m_Length = length;
            this.m_DiskLength = diskLength;
            this.m_Compressed = compressed;
            this.m_Fragmented = fragmented;
        }

        public void Download(Stream output)
        {
            int i3;
            //Log.WriteLine("Download");
            //m_Stream.Seek(m_Lookup, SeekOrigin.Begin);
            if (!m_Compressed)
            {
                Log.WriteLine("Tamaño", m_Length);
                byte[] bArr1 = m_Stream.ReadBytes(m_Length);
                output.Write(bArr1, 0, m_Length);
            }
            else if (m_Fragmented)
            {
                for (int i1 = m_Length; i1 > 0; i1 = i1 - i3)
                {
                    int i2 = m_Stream.ReadInt32();
                    i3 = 0;
                    m_Stream.ReadInt32();
                    byte[] bArr2 = m_Stream.ReadBytes(i2);
                    byte[] bArr3 = Compression.Decompress(bArr2, i2, 4096, ref i3);
                    //output.Write(bArr3, 0, i3 > i1 ? i1 : i3);
                    output.Write(bArr3, 0, (i3 <= i1) ? i3 : i1);
                }
            }
            else
            {
                int i4 = m_Length;
                int i5 = 0;
                byte[] bArr4 = m_Stream.ReadBytes(i4);
                byte[] bArr5 = Compression.Decompress(bArr4, i4, 131072, ref i5);
                output.Write(bArr5, 0, i5);
            }
        }

    } // class ArchivedFile
}