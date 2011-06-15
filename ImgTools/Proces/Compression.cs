using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ImgTools
{
    public class Compression
    {

        public const int x1884 = 0x621;
        public const int x4E8 = 0x13A;
        public const int x9D0 = 0x274;
        public const int x9D4 = 0x275;
        public const int xD460 = 0x3518;
        public const int xDE28 = 0x378A;
        public const int xDE2C = 0x378B;
        public const int xDE30 = 0x378C;
        public const int xDE34 = 0x378D;
        public const int xE7F8 = 0x39FE;
        public const int xE7FC = 0x39FF;
        public const int xECE4 = 0x3B39;
        public const int xF1CC = 0x3C73;

        private static byte[] m_BackReference;
        private static int[] m_Memory;
        private static byte[] m_OutputBuffer;
        private static byte[] m_Table_531C20;
        private static byte[] m_Table_531D20;

        static Compression()
        {
            Compression.m_Memory = new int[0x10000];
            Compression.m_Table_531C20 = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 0xA, 0xA, 0xA, 0xA, 0xA, 0xA, 0xA, 0xA, 0xB, 0xB, 0xB, 0xB, 0xB, 0xB, 0xB, 0xB, 0xC, 0xC, 0xC, 0xC, 0xD, 0xD, 0xD, 0xD, 0xE, 0xE, 0xE, 0xE, 0xF, 0xF, 0xF, 0xF, 0x10, 0x10, 0x10, 0x10, 0x11, 0x11, 0x11, 0x11, 0x12, 0x12, 0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 0x14, 0x14, 0x14, 0x14, 0x15, 0x15, 0x15, 0x15, 0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17, 0x18, 0x18, 0x19, 0x19, 0x1A, 0x1A, 0x1B, 0x1B, 0x1C, 0x1C, 0x1D, 0x1D, 0x1E, 0x1E, 0x1F, 0x1F, 0x20, 0x20, 0x21, 0x21, 0x22, 0x22, 0x23, 0x23, 0x24, 0x24, 0x25, 0x25, 0x26, 0x26, 0x27, 0x27, 0x28, 0x28, 0x29, 0x29, 0x2A, 0x2A, 0x2B, 0x2B, 0x2C, 0x2C, 0x2D, 0x2D, 0x2E, 0x2E, 0x2F, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F };
            Compression.m_Table_531D20 = new byte[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 };
            Compression.m_BackReference = new byte[0x1000];
            Compression.m_OutputBuffer = new byte[0x100000];
        }

        public Compression()
        {
        }

        public static void Decompress(byte[] input, int inputLength, byte[] output, int knownLength, ref int outputLength)
        {
            Log.WriteLine("Decompress");
            int inputValue = 0;
            int inputBits = 0;
            int inputIndex = 0;
            ResetMemory();
            for (int i = 0; i < m_BackReference.Length; i++)
            {
                m_BackReference[i] = 0x20;
            }
            int index = 0xfc4;
            while (outputLength < knownLength)
            {
                int num6 = 0x270;
                while (num6 < 0x273)
                {
                    if (inputBits == 0)
                    {
                        if (ReadBit(input, inputLength, ref inputIndex, ref inputValue, ref inputBits))
                        {
                            num6++;
                        }
                    }
                    else
                    {
                        inputBits--;
                        if ((inputValue & 0x8000) != 0)
                        {
                            num6++;
                        }
                        inputValue = inputValue << 1;
                        inputValue &= 0xffff;
                    }
                    num6 = m_Memory[0x3b39 + num6];
                }
                int num7 = SomeFunc(num6 - 0x273);
                if (num7 < 0x100)
                {
                    output[outputLength++] = (byte)num7;
                    m_BackReference[index] = (byte)num7;
                    index++;
                    index &= 0xfff;
                }
                else
                {
                    int num8 = ReadByte(input, inputLength, ref inputIndex, ref inputValue, ref inputBits);
                    int num9 = m_Table_531C20[num8];
                    int num10 = m_Table_531D20[num8];
                    num9 = num9 << 6;
                    num10 -= 2;
                    int num13 = num10;
                    do
                    {
                        num8 = num8 << 1;
                        if (inputBits == 0)
                        {
                            if (ReadBit(input, inputLength, ref inputIndex, ref inputValue, ref inputBits))
                            {
                                num8++;
                            }
                        }
                        else
                        {
                            inputBits--;
                            if ((inputValue & 0x8000) != 0)
                            {
                                num8++;
                            }
                            inputValue = inputValue << 1;
                            inputValue &= 0xffff;
                        }
                        num13--;
                    }
                    while (num13 != 0);
                    num8 &= 0x3f;
                    num8 |= num9;
                    int num11 = index - num8;
                    num11--;
                    num10 = num7 - 0xfd;
                    int num12 = num10 + index;
                    num11 &= 0xfff;
                    bool flag = true;
                    int num14 = num12;
                    if (num12 < 0x1000)
                    {
                        num12 = num10 + num11;
                        if ((num12 < 0x1000) && ((num12 <= index) || (num11 >= index)))
                        {
                            flag = false;
                            num9 = 0;
                            num9 = (num10 < 0) ? 1 : 0;
                            if (num10 < 0)
                            {
                                int num16 = 0;
                                num16++;
                            }
                            index += num13;
                            int num15 = num11 + num13;
                            int num17 = index;
                            num9--;
                            num9 &= num10;
                            num12 = num9;
                            num11 = num12;
                            for (int j = 0; j < num12; j++)
                            {
                                m_BackReference[index + j] = m_BackReference[num15 + j];
                            }
                            num11 = outputLength;
                            num12 = num9;
                            num11 = num12;
                            for (int k = 0; k < num12; k++)
                            {
                                output[outputLength++] = m_BackReference[index + k];
                            }
                            index = num14;
                        }
                    }
                    if (flag)
                    {
                        num10 += num11;
                        num12 = num11;
                        if (num11 < num10)
                        {
                            int num20 = num10;
                            num20 -= num11;
                            int num21 = num20;
                            do
                            {
                                num9 = outputLength;
                                num11 = num12;
                                num11 &= 0xfff;
                                num11 = m_BackReference[num11];
                                output[num9] = (byte)num11;
                                outputLength++;
                                m_BackReference[index] = (byte)num11;
                                index++;
                                index &= 0xfff;
                                num12++;
                            }
                            while (num12 < num10);
                        }
                    }
                }
            }
            if (outputLength != knownLength)
            {
                int num22 = 0;
                num22++;
            }
        }

        public static byte[] Decompress(byte[] input, int inputLength, int knownLength, ref int outputLength)
        {
            outputLength = 0;
            Compression.Decompress(input, inputLength, Compression.m_OutputBuffer, knownLength, ref outputLength);
            return Compression.m_OutputBuffer;
        }

        public static void FormatBuffer(TextWriter output, byte[] input, int length)
        {
            output.WriteLine("           0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("          -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");
            int num = 0;
            int num2 = length >> 4;
            int capacity = length & 15;
            int num4 = 0;
            int num5 = 0;
            while (num5 < num2)
            {
                StringBuilder builder = new StringBuilder(0x31);
                StringBuilder builder2 = new StringBuilder(0x10);
                for (int i = 0; i < 0x10; i++)
                {
                    int num7 = input[num4++];
                    builder.Append(num7.ToString("X2"));
                    if (i != 7)
                    {
                        builder.Append(' ');
                    }
                    else
                    {
                        builder.Append("  ");
                    }
                    if ((num7 >= 0x20) && (num7 < 0x80))
                    {
                        builder2.Append((char)num7);
                    }
                    else
                    {
                        builder2.Append('.');
                    }
                }
                output.Write(' ');
                output.Write(num.ToString("X6"));
                output.Write("   ");
                output.Write(builder.ToString());
                output.Write("  ");
                output.WriteLine(builder2.ToString());
                num5++;
                num += 0x10;
            }
            if (capacity != 0)
            {
                StringBuilder builder3 = new StringBuilder(0x31);
                StringBuilder builder4 = new StringBuilder(capacity);
                for (int j = 0; j < 0x10; j++)
                {
                    if (j < capacity)
                    {
                        int num9 = input[num4++];
                        builder3.Append(num9.ToString("X2"));
                        if (j != 7)
                        {
                            builder3.Append(' ');
                        }
                        else
                        {
                            builder3.Append("  ");
                        }
                        if ((num9 >= 0x20) && (num9 < 0x80))
                        {
                            builder4.Append((char)num9);
                        }
                        else
                        {
                            builder4.Append('.');
                        }
                    }
                    else
                    {
                        if (j == 7)
                        {
                            builder3.Append(' ');
                        }
                        builder3.Append("   ");
                    }
                }
                output.Write(' ');
                output.Write(num.ToString("X6"));
                output.Write("   ");
                output.Write(builder3.ToString());
                output.Write("  ");
                output.WriteLine(builder4.ToString());
            }
        }

        public static bool ReadBit(byte[] input, int inputLength, ref int inputIndex, ref int inputValue, ref int inputBits)
        {
            while (inputBits <= 8)
            {
                int i1 = 0;
                if (inputIndex < inputLength)
                {
                    int i2 = inputIndex;
                    inputIndex++;
                    i1 = input[i2];
                }
                i1 <<= (8 - inputBits) & 0x1F;
                inputValue |= i1;
                inputBits += 8;
            }
            bool flag1 = (inputValue & 0x8000) != 0;
            inputValue <<= 1;
            inputValue &= 0xFFFF;
            inputBits--;
            return flag1;
        }

        public static byte ReadByte(byte[] input, int inputLength, ref int inputIndex, ref int inputValue, ref int inputBits)
        {
            while (inputBits <= 8)
            {
                int i1 = 0;
                if (inputIndex < inputLength)
                {
                    int i2 = inputIndex;
                    inputIndex++;
                    i1 = input[i2];
                }
                i1 <<= (8 - inputBits) & 0x1F;
                inputValue |= i1;
                inputBits += 8;
            }
            byte b1 = (byte)(inputValue >> 8);
            inputValue <<= 8;
            inputValue &= 0xFFFF;
            inputBits -= 8;
            return b1;
        }

        public static void ResetMemory()
        {
            int i1 = 0x3B39, i2 = 0;
            while (i2 < 0x13A)
            {
                Compression.m_Memory[i1 - 0x13A] = i2;
                Compression.m_Memory[i1 - 0x621] = 1;
                Compression.m_Memory[i1] = 0x273 + i2;
                i2++;
                i1++;
            }
            int i3 = 0x3C73, i4 = 0x3518, i5 = 0x13A;
            for (int i6 = 0; i5 <= 0x272; i6 = i6 + 2)
            {
                int i7 = Compression.m_Memory[i4];
                i7 += Compression.m_Memory[i4 + 1];
                Compression.m_Memory[i3] = i6;
                Compression.m_Memory[i3 - 0x621] = i7;
                Compression.m_Memory[i4 + 0x274] = i5;
                Compression.m_Memory[i4 + 0x275] = i5;
                i4 += 2;
                i3++;
                i5++;
            }
            Compression.m_Memory[0x39FE] = 0;
            Compression.m_Memory[0x378B] = 0xFFFF;
        }

        public static int SomeFunc(int ip)
        {
            int i2 = ip;
            int i3 = ip;
            int i4 = Compression.m_Memory[0x39FF + ip];
            while (true)
            {
                int i5 = Compression.m_Memory[0x3518 + i4];
                i3 = Compression.m_Memory[0x3518 + i4 + 1];
                i5++;
                int i1 = i5;
                Compression.m_Memory[0x3518 + i4] = i5;
                if (i1 > i3)
                {
                    i3 = i4 + 1;
                    i5 = i3;
                    do
                    {
                        i5++;
                        i2 = Compression.m_Memory[0x3518 + i5];
                        i3++;
                        if (i1 > i2)
                        {
                            i5++;
                            i2 = Compression.m_Memory[0x3518 + i5];
                            i3++;
                            if (i1 > i2)
                            {
                                i5++;
                                i2 = Compression.m_Memory[0x3518 + i5];
                                i3++;
                            }
                        }
                    } while (i1 > i2);
                    i3--;
                    i5 = Compression.m_Memory[0x3518 + i3];
                    Compression.m_Memory[0x3518 + i4] = i5;
                    Compression.m_Memory[0x3518 + i3] = i1;
                    i5 = Compression.m_Memory[0x3B39 + i4];
                    Compression.m_Memory[0x378C + i5] = i3;
                    if (i5 < 0x273)
                        Compression.m_Memory[0x378D + i5] = i3;
                    i1 = Compression.m_Memory[0x3B39 + i3];
                    Compression.m_Memory[0x3B39 + i3] = i5;
                    Compression.m_Memory[0x378C + i1] = i4;
                    if (i1 < 0x273)
                        Compression.m_Memory[0x378D + i1] = i4;
                    Compression.m_Memory[0x3B39 + i4] = i1;
                    i4 = Compression.m_Memory[0x378C + i3];
                }
                else
                {
                    i4 = Compression.m_Memory[0x378C + i4];
                }
                if (i4 == 0)
                    break;
            }
            return ip;
        }

    } // class Compression

}
