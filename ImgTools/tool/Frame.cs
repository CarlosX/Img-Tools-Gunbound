/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       Frame.cs
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
using System.Drawing;

namespace ImgTools
{
    public class Frame
    {

        private int m_CenterX;
        private int m_CenterY;
        private Bitmap m_Image;

        public int CenterX
        {
            get
            {
                return m_CenterX;
            }
        }

        public int CenterY
        {
            get
            {
                return m_CenterY;
            }
        }

        public Bitmap Image
        {
            get
            {
                return m_Image;
            }
        }

        public Frame(Bitmap image, int xCenter, int yCenter)
        {
            m_Image = image;
            m_CenterX = xCenter;
            m_CenterY = yCenter;
        }

    } // class Frame
}