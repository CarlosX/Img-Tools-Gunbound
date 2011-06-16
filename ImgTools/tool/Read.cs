/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       Read.cs
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

namespace ImgTools
{
    public abstract class DataStream
    {
        private static byte[] m_Buffer = new byte[0x800000];
        private Stream m_Stream;

        protected abstract Stream Aquire();
        public byte[] Data_x()
        {
            if (!this.Validate())
            {
                byte[] xc = new byte[] { 0x01};
                return xc;
            }
            this.m_Stream.Read(m_Buffer, 0, m_Buffer.Length);
            return m_Buffer;
        }
        public bool ReadBoolean()
        {
            if (!this.Validate())
            {
                return false;
            }
            this.m_Stream.Read(m_Buffer, 0, 1);
            return (m_Buffer[0] != 0);
        }

        public byte ReadByte()
        {
            if (!this.Validate())
            {
                return 0;
            }
            this.m_Stream.Read(m_Buffer, 0, 1);
            return m_Buffer[0];
        }

        public byte[] ReadBytes(int length)
        {
            if (!this.Validate())
            {
                for (int i = 0; i < length; i++)
                {
                    m_Buffer[i] = 0;
                }
            }
            else
            {
                this.m_Stream.Read(m_Buffer, 0, length);
            }
            return m_Buffer;
        }

        public short ReadInt16()
        {
            if (!this.Validate())
            {
                return 0;
            }
            this.m_Stream.Read(m_Buffer, 0, 2);
            return (short)(m_Buffer[0] | (m_Buffer[1] << 8));
        }

        public int ReadInt32()
        {
            if (!this.Validate())
            {
                return 0;
            }
            this.m_Stream.Read(m_Buffer, 0, 4);
            return (((m_Buffer[0] | (m_Buffer[1] << 8)) | (m_Buffer[2] << 0x10)) | (m_Buffer[3] << 0x18));
        }

        public string ReadString(int length)
        {
            if (!this.Validate())
            {
                return "";
            }
            if (m_Buffer.Length < length)
            {
                m_Buffer = new byte[length];
            }
            this.m_Stream.Read(m_Buffer, 0, length);
            int index = 0;
            index = 0;
            while ((index < length) && (m_Buffer[index] != 0))
            {
                index++;
            }
            return Encoding.ASCII.GetString(m_Buffer, 0, index);
        }

        public void Seek(int offset, SeekOrigin origin)
        {
            if (this.Validate())
            {
                this.m_Stream.Seek((long)offset, origin);
            }
        }

        public bool Validate()
        {
            if (this.m_Stream == null)
            {
                this.m_Stream = this.Aquire();
            }
            return (this.m_Stream != null);
        }

        public int Length
        {
            get
            {
                if (!this.Validate())
                {
                    return 0;
                }
                return (int)this.m_Stream.Length;
            }
        }
    }
}