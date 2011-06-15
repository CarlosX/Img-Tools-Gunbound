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
