/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       Archive.cs
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
using System.Collections;
using System.IO;

namespace ImgTools
{
    public class Archive
    {

        private ArchivedFile[] m_Files;
        private string m_Name;

        public ArchivedFile[] Files
        {
            get
            {
                return m_Files;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public Archive(string name, ArchivedFile[] files)
        {
            m_Name = name;
            m_Files = files;
            for (int i = 0; i < files.Length; i++)
            {
                files[i].Archive = this;
            }
        }

        public ArchivedFile Search(string fileName)
        {
            CaseInsensitiveComparer caseInsensitiveComparer = CaseInsensitiveComparer.Default;
            for (int i = 0; i < m_Files.Length; i++)
            {
                if (caseInsensitiveComparer.Compare(m_Files[i].FileName, fileName) == 0)
                {
                    return m_Files[i];
                }
            }
            return null;
        }

        public static Archive LoadIMG(string path)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            ArrayList list = new ArrayList();
            if (File.Exists(path))
            {
                DataStream stream = new FileDataStream(path);
                int lk = stream.Length;
                string fileName = fileNameWithoutExtension;
                bool compressed = false;
                int lookup = 0;
                int num13 = stream.Length;
                int diskLength = stream.Length;
                list.Add(new ArchivedFile(fileName, stream, lookup, num13, diskLength, compressed, true));
            }
            return new Archive(fileNameWithoutExtension, (ArchivedFile[])list.ToArray(typeof(ArchivedFile)));
        }
        
    } // class Archive
}