using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ImgTools
{
    public abstract class DataProcessor
    {

        private Regex m_Regex;
        private string m_Type;

        public Regex Regex
        {
            get
            {
                return m_Regex;
            }
            set
            {
                m_Regex = value;
            }
        }

        public string Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        public DataProcessor(string type, string match)
        {
            m_Type = type;
            m_Regex = new Regex(match, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public bool IsMatch(string fileName)
        {
            return m_Regex.IsMatch(fileName);
        }

        public abstract void Process(DataStream ip, TextWriter op);

        public virtual MemoryStream Mutate(MemoryStream stream)
        {
            return stream;
        }

    } // class DataProcessor
}
