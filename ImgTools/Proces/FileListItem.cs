/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       FileListItem.cs
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
using System.IO;
using System.Windows.Forms;

namespace ImgTools
{
    public class FileListItem : ListViewItem
    {

        private ArchivedFile m_ArchivedFile;

        public ArchivedFile ArchivedFile
        {
            get
            {
                return m_ArchivedFile;
            }
        }

        public FileListItem(ArchivedFile archivedFile, int icon)
            : base(new string[] { archivedFile.FileName, GetFileSize(archivedFile.Length), FileDecoder.FindDecoder(Path.GetExtension(archivedFile.FileName)).GetType(archivedFile) })
        {
            string[] sArr = new string[] {
                                           archivedFile.FileName, 
                                           FileListItem.GetFileSize(archivedFile.Length), 
                                           FileDecoder.FindDecoder(Path.GetExtension(archivedFile.FileName)).GetType(archivedFile) };
            ImageIndex = 2 + (icon * 2);
            m_ArchivedFile = archivedFile;
        }

        public static string GetFileSize(int bytes)
        {
            string s;

            if (bytes < 0xF4240)
                s = String.Format("{0:N2} KB", (double)bytes / 1024.0, bytes);
            else if (bytes < 0x3B9ACA00)
                s = String.Format("{0:N2} MB", (double)bytes / 1024.0 / 1024.0, bytes);
            else
                s = String.Format("{0:N2} GB", ((double)bytes / 1024.0 / 1024.0) / 1024.0, bytes);
            return s;
        }

    } // class FileListItem
}