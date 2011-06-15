using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImgTools
{
    public class FileNode : TreeNode
    {

        private ArchivedFile m_ArchivedFile;
        private int m_Icon;

        public ArchivedFile ArchivedFile
        {
            get
            {
                return m_ArchivedFile;
            }
        }

        public int Icon
        {
            get
            {
                return m_Icon;
            }
        }

        public FileNode(ArchivedFile archivedFile, int icon)
            : base(archivedFile.FileName)
        {
            ImageIndex = 2 + (icon * 2);
            SelectedImageIndex = 3 + (icon * 2);
            m_ArchivedFile = archivedFile;
            m_Icon = icon;
        }

    } // class FileNode

}
