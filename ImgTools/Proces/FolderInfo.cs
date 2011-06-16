/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       FolderInfo.cs
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
using System.Text.RegularExpressions;

namespace ImgTools
{
    public class FolderInfo
    {

        public static readonly FolderInfo[] Folders;

        private string[] m_Path;
        private Regex m_Regex;

        public string[] Path
        {
            get
            {
                return m_Path;
            }
        }

        public FolderInfo(string path, string match)
        {
            char[] chArr = new char[] { '\\' };
            m_Path = path.Split(chArr);
            m_Regex = new Regex(match, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        static FolderInfo()
        {
            FolderInfo[] folderInfoArr = new FolderInfo[] {
                                                            new FolderInfo("", ".img$")
            };
            FolderInfo.Folders = folderInfoArr;
        }

        public static FolderInfo GetFolder(string fileName)
        {


            FolderInfo[] folderInfoArr = FolderInfo.Folders;
            for (int i = 0; i < folderInfoArr.Length; i++)
            {
                if (folderInfoArr[i].m_Regex.IsMatch(fileName))
                {
                    return folderInfoArr[i];
                }
            }
            return null;
        }

    } // class FolderInfo
}