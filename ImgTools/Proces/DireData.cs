using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ImgTools
{
    public class DirectDataStream : DataStream
    {

        private Stream m_DirectStream;

        public DirectDataStream(Stream directStream)
        {
            m_DirectStream = directStream;
        }

        protected override Stream Aquire()
        {
            return m_DirectStream;
        }

    } // class DirectDataStream
}
