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
