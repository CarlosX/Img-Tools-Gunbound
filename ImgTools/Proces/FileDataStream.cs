using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ImgTools
{
    public class FileDataStream : DataStream
    {

        private string m_Path;

        public FileDataStream(string path)
        {
            m_Path = path;
        }

        protected override Stream Aquire()
        {
            
            return new FileStream(m_Path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

    } // class FileDataStream
}
