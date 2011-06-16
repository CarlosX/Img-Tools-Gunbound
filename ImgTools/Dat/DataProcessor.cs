/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       DataProcessor.cs
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