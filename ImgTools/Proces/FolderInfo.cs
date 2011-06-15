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
