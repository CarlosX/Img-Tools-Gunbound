using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ImgConvert
{
    public class Log
    {
        public static void WriteLine(string st, object log)
        {
            Console.WriteLine("[Log]: {0}: {1}", st, log);
        }
        public static void WriteLine(object log)
        {
            Console.WriteLine("[Log]: {0}", log);
        }



        public static string HexStr(byte[] p)
        {
            char[] c = new char[p.Length * 2 + 2];
            byte b;
            //c[0] = '0'; c[1] = 'x';
            for (int y = 0, x = 2; y < p.Length; ++y, ++x)
            {
                b = ((byte)(p[y] >> 4));
                c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(p[y] & 0xF));
                c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }
        private string Bytes_To_String(byte[] bytes_Input)
        {
            // convert the byte array back to a true string
            string strTemp = "";
            for (int x = 0; x <= bytes_Input.GetUpperBound(0); x++)
            {
                int number = int.Parse(bytes_Input[x].ToString());
                strTemp += number.ToString("X").PadLeft(2, '0');
            }
            // return the finished string of hex values
            return strTemp;
        }
    }
}
