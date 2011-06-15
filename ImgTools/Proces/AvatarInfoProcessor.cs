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
