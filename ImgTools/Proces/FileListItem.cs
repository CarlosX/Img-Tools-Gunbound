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
