/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       AvatarInfoProcessor.cs
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

namespace ImgTools
{
    public class AvatarInfoProcessor : DataProcessor
    {

        public AvatarInfoProcessor()
            : base("Avatar Information", "[mf][bfgh].dat")
        {
        }

        public override void Process(DataStream ip, TextWriter op)
        {
            int i1 = ip.ReadInt32();
            op.WriteLine("// {0} entries", i1);
            op.WriteLine();
            for (int i2 = 0; i2 < i1; i2++)
            {
                string s = "{0}\t{1}";
                op.WriteLine();
                op.WriteLine(s, "Index\t", ip.ReadInt32());
                op.WriteLine(s, "Name\t", ip.ReadString(0x17));
                op.WriteLine(s, "Buyable\t", ip.ReadBoolean());
                op.WriteLine(s, "Gold\t", ip.ReadInt32());
                op.WriteLine(s, "Cash\t", ip.ReadInt32());
                op.WriteLine(s, "Shot Delay", ip.ReadInt32());
                op.WriteLine(s, "Bunge\t", ip.ReadInt32());
                op.WriteLine(s, "Attack\t", ip.ReadInt32());
                op.WriteLine(s, "Defense\t", ip.ReadInt32());
                op.WriteLine(s, "Health\t", ip.ReadInt32());
                op.WriteLine(s, "Item Delay", ip.ReadInt32());
                op.WriteLine(s, "Shield\t", ip.ReadInt32());
                op.WriteLine(s, "Popularity\t", ip.ReadInt32());
                op.WriteLine(s, "Description", ip.ReadString(0x40));
            }
        }

    } // class AvatarInfoProcessor
}