/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       ImageDecoder.cs
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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;
using System.Windows.Forms;

namespace ImgTools
{
    public class ImageDecoder : FileDecoder
    {

        public ImageDecoder(string extension)
            : base("Image", extension)
        {
        }

        public override void FillPanel(ArchivedFile file, Panel pn)
        {
            Frame[] frameArr1 = ImageDecoder.LoadFrames(file);
            Panel panel = pn;
            FileTabSheet fileTabSheet = new FileTabSheet();
            fileTabSheet.SetFile(file);
            pn = fileTabSheet.DecodedPanel;
            pn.AutoScroll = true;
            pn.AutoScrollMargin = new Size(5, 5);
            int i1 = 5;
            for (int i2 = 0; i2 < (int)frameArr1.Length; i2++)
            {
                Frame[] frameArr2 = new Frame[] { frameArr1[i2] };
                int[] iArr = new int[] { 1 };
                Animation animation = new Animation(frameArr2, iArr);
                AnimationBox animationBox = new AnimationBox();
                animationBox.Title = "Imagen";
                Animation[] animationArr = new Animation[] { animation };
                animationBox.Animations = animationArr;
                animationBox.Name = Path.GetFileNameWithoutExtension(file.FileName);
                animationBox.UpdateSize();
                animationBox.SetBounds(5, i1, animationBox.Width, animationBox.Height);
                pn.Controls.Add(animationBox);
                i1 += animationBox.Height + 5;
            }
            panel.Controls.Add(fileTabSheet);
        }

        public unsafe static Frame[] LoadFrames(ArchivedFile file)
        {
            MemoryStream memoryStream = new MemoryStream();
            file.Download(memoryStream);
            memoryStream.Seek((long)0, SeekOrigin.Begin);
            DataStream dataStream = new DirectDataStream(memoryStream);
            dataStream.ReadInt32();
            int i1 = dataStream.ReadInt32();
            Frame[] frames1 = new Frame[i1];
            int j1 = 0;
            while (j1 < i1)
            {
                int k1 = dataStream.ReadInt16();
                int i2 = dataStream.ReadInt16();
                int j2 = dataStream.ReadInt32();
                int k2 = dataStream.ReadInt32();
                int i3 = dataStream.ReadInt32();
                int j3 = dataStream.ReadInt32();
                int k3 = dataStream.ReadInt32();
                int i4 = dataStream.ReadInt32();
                int j4 = dataStream.ReadInt32();
                int k4 = dataStream.ReadInt32();
                int i5 = dataStream.ReadInt32();
                Debug.Assert((i5 < 16777216 && j2 > 0 && k2 > 0 && j2 <= 2048) ? (k2 > 2048 == false) : false);
                byte[] bs = dataStream.ReadBytes(i5);
                int j5 = k1 & 255;
                PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
                Bitmap bitmap = new Bitmap(j2, k2, pixelFormat);
                frames1[j1] = new Frame(bitmap, i3, j3);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, j2, k2), ImageLockMode.WriteOnly, pixelFormat);
                switch (j5)
                {
                    case 0:
                        fixed (byte* b1 = &bs[0])
                        {
                            short* s1 = (short*)b1;
                            int* k5 = (int*)bitmapData.Scan0;
                            short* s2 = s1 + (j2 * k2);
                            int i6 = (bitmapData.Stride >> 2) - bitmapData.Width;
                            while (s1 < s2)
                            {
                                short* s3 = s1 + j2;
                                while (s1 < s3)
                                {
                                    int j6 = *s1++;
                                    int k6 = j6 & 31;
                                    int i7 = j6 >> 5 & 63;
                                    int j7 = j6 >> 11 & 31;
                                    int k7 = 255;
                                    k6 *= 255;
                                    k6 /= 31;
                                    i7 *= 255;
                                    i7 /= 63;
                                    j7 *= 255;
                                    j7 /= 31;
                                    if (j7 == 0 && i7 == 0 && k6 == 0)
                                    {
                                        k7 = 0;
                                    }
                                    i7 <<= 8;
                                    j7 <<= 16;
                                    k7 <<= 24;
                                    j6 = k7 | j7 | i7 | k6;
                                    *k5++ = j6;
                                }
                                k5 += i6;
                            }
                        }
                        goto default;

                    case 2:
                        fixed (byte* b2 = &bs[0])
                        {
                            short* s4 = (short*)b2;
                            int* i8 = (int*)bitmapData.Scan0;
                            short* s5 = s4 + (j2 * k2);
                            int j8 = (bitmapData.Stride >> 2) - bitmapData.Width;
                            while (s4 < s5)
                            {
                                short* s6 = s4 + j2;
                                while (s4 < s6)
                                {
                                    int k8 = *s4++;
                                    int i9 = k8 & 15;
                                    int j9 = k8 >> 4 & 15;
                                    int k9 = k8 >> 8 & 15;
                                    int i10 = k8 >> 12 & 15;
                                    i9 *= 255;
                                    i9 /= 15;
                                    j9 *= 255;
                                    j9 /= 15;
                                    k9 *= 255;
                                    k9 /= 15;
                                    i10 *= 255;
                                    i10 /= 15;
                                    if (k9 == 0 && j9 == 0 && i9 == 0)
                                    {
                                        i10 = 0;
                                    }
                                    j9 <<= 8;
                                    k9 <<= 16;
                                    i10 <<= 24;
                                    k8 = i10 | k9 | j9 | i9;
                                    *i8++ = k8;
                                }
                                i8 += j8;
                            }
                        }
                        goto default;

                    case 1:
                        fixed (byte* b3 = &bs[0])
                        {
                            short* s7 = (short*)b3;
                            int* j10 = (int*)bitmapData.Scan0;
                            int k10 = bitmapData.Stride >> 2;
                            int* i12;
                            for (int i11 = 0; i11 < k2; i11++)
                            {
                                int* j11 = j10 + (i11 * k10);
                                int* k11 = j11;
                                i12 = k11;
                                int j12 = *s7++;
                                int k12 = *s7++;
                                for (int i13 = 0; i13 < k12; i13++)
                                {
                                    int j13 = *s7++;
                                    int* k13 = j11 + j13;
                                    if (k11 < k13)
                                    {
                                        try
                                        {
                                            for (*k11++ = 0; k11 < k13; *k11++ = 0)
                                            {
                                            }
                                        }
                                        catch { }
                                    }
                                    else if (k11 > k13)
                                    {
                                        try
                                        {
                                            for (*k11-- = 0; k11 > k13; *k11-- = 0)
                                            {
                                            }
                                        }
                                        catch { }
                                    }
                                    k11 = k13;
                                    int i14 = *s7++;
                                    k13 = k11 + i14;
                                    while (k11 < k13)
                                    {
                                        int j14 = *s7++;
                                        int k14 = j14 & 31;
                                        int i15 = j14 >> 5 & 63;
                                        int j15 = j14 >> 11 & 31;
                                        int k15 = 255;
                                        k14 *= 255;
                                        k14 /= 31;
                                        i15 *= 255;
                                        i15 /= 63;
                                        j15 *= 255;
                                        j15 /= 31;
                                        if (j15 == 0 && i15 == 0 && k14 == 0)
                                        {
                                            k15 = 0;
                                        }
                                        i15 <<= 8;
                                        j15 <<= 16;
                                        k15 <<= 24;
                                        j14 = k15 | j15 | i15 | k14;
                                        *k11++ = j14;
                                    }
                                }
                                while (k11 < i12)
                                {
                                    *k11++ = 0;
                                }
                                i12 += k10;
                            }
                        }
                        goto default;

                    default:
                        bitmap.UnlockBits(bitmapData);
                        j1++;
                        break;
                }
            }
            return frames1;
        }
    }
}