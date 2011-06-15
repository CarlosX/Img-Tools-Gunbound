using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ImgConvert
{

    public class AnimationBox : UserControl
    {

        private ContextMenu cmOptions;
        private IContainer components;
        private Animation[] m_Animations;
        private DateTime m_Begin;
        private string m_FileName;
        private Label m_Label;
        private string m_Title;
        private int m_xCenter;
        private int m_yCenter;
        private MenuItem miOptionsCenter;
        private MenuItem miOptionsLoop;
        private MenuItem miOptionsPlay;
        private MenuItem miOptionsSave;
        private MenuItem miOptionsSep;
        private MenuItem miOptionsStop;
        private Timer timRedraw;

        private static TimeSpan Interval;

        public Animation[] Animations
        {
            get
            {
                return m_Animations;
            }
            set
            {
                m_Animations = value;
            }
        }

        public string FileName
        {
            get
            {
                return m_FileName;
            }
            set
            {
                m_FileName = value;
            }
        }

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                if (m_Title == value)
                {
                }
                else
                {
                    m_Title = value;
                    if (m_Title != null)
                    {
                        if (m_Label == null)
                        {
                            m_Label = new Label();
                            Size size = ClientSize;
                            m_Label.SetBounds(4, 4, size.Width - 8, 0x10);
                            m_Label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                            m_Label.TextAlign = ContentAlignment.MiddleCenter;
                            Controls.Add(m_Label);
                        }
                        m_Label.Text = m_Title;
                    }
                    else if (m_Label != null)
                    {
                        Controls.Remove(m_Label);
                        m_Label.Dispose();
                        m_Label = null;
                    }
                }
            }
        }

        public AnimationBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            m_Begin = DateTime.Now;
        }

        static AnimationBox()
        {
            AnimationBox.Interval = TimeSpan.FromMilliseconds(50.0);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timRedraw = new System.Windows.Forms.Timer(this.components);
            this.cmOptions = new System.Windows.Forms.ContextMenu();
            this.miOptionsPlay = new System.Windows.Forms.MenuItem();
            this.miOptionsStop = new System.Windows.Forms.MenuItem();
            this.miOptionsLoop = new System.Windows.Forms.MenuItem();
            this.miOptionsCenter = new System.Windows.Forms.MenuItem();
            this.miOptionsSep = new System.Windows.Forms.MenuItem();
            this.miOptionsSave = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // timRedraw
            // 
            this.timRedraw.Interval = 30;
            this.timRedraw.Tick += new System.EventHandler(this.timRedraw_Tick);
            // 
            // cmOptions
            // 
            this.cmOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miOptionsPlay,
            this.miOptionsStop,
            this.miOptionsLoop,
            this.miOptionsCenter,
            this.miOptionsSep,
            this.miOptionsSave});
            // 
            // miOptionsPlay
            // 
            this.miOptionsPlay.Enabled = false;
            this.miOptionsPlay.Index = 0;
            this.miOptionsPlay.Text = "&Play";
            this.miOptionsPlay.Click += new System.EventHandler(this.miOptionsPlay_Click);
            // 
            // miOptionsStop
            // 
            this.miOptionsStop.Index = 1;
            this.miOptionsStop.Text = "&Stop";
            this.miOptionsStop.Click += new System.EventHandler(this.miOptionsStop_Click);
            // 
            // miOptionsLoop
            // 
            this.miOptionsLoop.Checked = true;
            this.miOptionsLoop.Index = 2;
            this.miOptionsLoop.Text = "&Loop";
            this.miOptionsLoop.Click += new System.EventHandler(this.miOptionsLoop_Click);
            // 
            // miOptionsCenter
            // 
            this.miOptionsCenter.Index = 3;
            this.miOptionsCenter.Text = "Show &Center";
            this.miOptionsCenter.Click += new System.EventHandler(this.miOptionsCenter_Click);
            // 
            // miOptionsSep
            // 
            this.miOptionsSep.Index = 4;
            this.miOptionsSep.Text = "-";
            // 
            // miOptionsSave
            // 
            this.miOptionsSave.Index = 5;
            this.miOptionsSave.Text = "&Save As";
            this.miOptionsSave.Click += new System.EventHandler(this.miOptionsSave_Click);
            // 
            // AnimationBox
            // 
            this.ContextMenu = this.cmOptions;
            this.Name = "AnimationBox";
            this.Size = new System.Drawing.Size(484, 452);
            this.ResumeLayout(false);

        }

        private void miOptionsCenter_Click(object sender, EventArgs e)
        {
            miOptionsCenter.Checked = !miOptionsCenter.Checked;
            Invalidate();
        }

        private void miOptionsLoop_Click(object sender, EventArgs e)
        {
            miOptionsLoop.Checked = !miOptionsLoop.Checked;
        }

        private void miOptionsPlay_Click(object sender, EventArgs e)
        {
            miOptionsPlay.Enabled = false;
            miOptionsStop.Enabled = true;
            m_Begin = DateTime.Now;
        }

        private void miOptionsSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog;
            ImageFormat png;
            int num;
            if (this.m_Animations != null)
            {
                dialog = new SaveFileDialog
                {
                    OverwritePrompt = true,
                    Filter = "All Supported Files|*.png;*.bmp;*.gif;*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|JPEG Files (*.jpg)|*.jpg",
                    FileName = (this.m_FileName == null) ? "Image" : this.m_FileName,
                    Title = "Save Image"
                };
                try
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        switch (Path.GetExtension(dialog.FileName).ToLower())
                        {
                            case ".png":
                                png = ImageFormat.Png;
                                goto Label_00E1;

                            case ".bmp":
                                png = ImageFormat.Bmp;
                                goto Label_00E1;

                            case ".gif":
                                png = ImageFormat.Gif;
                                goto Label_00E1;

                            case ".jpg":
                                png = ImageFormat.Jpeg;
                                goto Label_00E1;
                        }
                        MessageBox.Show("Unsupported image extension.");
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLine(ex.Message);
                }
            }
            return;
        Label_00E1:
            num = 0;
            int length = 0;
            int num3 = 0x7fffffff;
            int num4 = 0x7fffffff;
            int num5 = -2147483648;
            int num6 = -2147483648;
            for (int i = 0; i < this.m_Animations.Length; i++)
            {
                Animation animation = this.m_Animations[i];
                
                Frame[] frames = animation.Frames;
                num += frames.Length;
                if (frames.Length > length)
                {
                    length = frames.Length;
                }
                for (int j = 0; j < frames.Length; j++)
                {
                    Frame frame = frames[j];
                    int centerX = frame.CenterX;
                    int centerY = frame.CenterY;
                    int num11 = centerX + frame.Image.Width;
                    int num12 = centerY + frame.Image.Height;
                    if (centerX < num3)
                    {
                        num3 = centerX;
                    }
                    if (centerY < num4)
                    {
                        num4 = centerY;
                    }
                    if (num11 > num5)
                    {
                        num5 = num11;
                    }
                    if (num12 > num6)
                    {
                        num6 = num12;
                    }
                }
            }
            if (num3 != -2147483648)
            {
                string str;
                int width = num5 - num3;
                int height = num6 - num4;
                if (num > 1)
                {
                    str = "{0}_{1:D3}{2}";
                }
                else
                {
                    str = "{0}{2}";
                }
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(dialog.FileName);
                string extension = Path.GetExtension(dialog.FileName);
                Bitmap image = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(image);
                int num15 = -num3;
                int num16 = -num4;
                for (int k = 0; k < length; k++)
                {
                    graphics.Clear((png == ImageFormat.Png) ? Color.Transparent : Color.Black);
                    for (int m = 0; m < this.m_Animations.Length; m++)
                    {
                        Animation animation2 = this.m_Animations[m];
                        Frame frame2 = animation2.Frames[k % animation2.Frames.Length];
                        //Log.WriteLine("Data",Log.HexStr(BmpToBytes_Unsafe(frame2.Image)).Replace("00",""));
                        graphics.DrawImageUnscaled(frame2.Image, num15 + frame2.CenterX, num16 + frame2.CenterY, frame2.Image.Width, frame2.Image.Height);
                    }
                    image.Save(string.Format(str, fileNameWithoutExtension, k, extension), png);
                }
                graphics.Dispose();
                image.Dispose();
            }
        }
        private unsafe byte[] BmpToBytes_Unsafe(Bitmap bmp)
        {
            BitmapData bData = bmp.LockBits(new Rectangle(new Point(), bmp.Size),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            // number of bytes in the bitmap
            int byteCount = bData.Stride * bmp.Height;
            byte[] bmpBytes = new byte[byteCount];

            // Copy the locked bytes from memory
            Marshal.Copy(bData.Scan0, bmpBytes, 0, byteCount);

            // don't forget to unlock the bitmap!!
            bmp.UnlockBits(bData);

            return bmpBytes;
        }
        private void miOptionsStop_Click(object sender, EventArgs e)
        {
            miOptionsPlay.Enabled = true;
            miOptionsStop.Enabled = false;
            m_Begin = DateTime.MinValue;
        }

        private void timRedraw_Tick(object sender, EventArgs e)
        {
            Size size1 = ClientSize;
            Size size2 = ClientSize;
            Invalidate(new Rectangle(0xA, 0x22, size1.Width - 0x14, size2.Height - 0x2C), false);
        }

        public void UpdateSize()
        {
            int i1 = 0x50, i2 = 0x50, i3 = 0;
            if (m_Animations != null)
            {
                int i4 = int.MaxValue, i5 = int.MaxValue, i6 = int.MinValue, i7 = int.MinValue;
                for (int i8 = 0; i8 < (int)m_Animations.Length; i8++)
                {
                    Animation animation = m_Animations[i8];
                    Frame[] frameArr = animation.Frames;
                    i3 += frameArr.Length;
                    for (int i9 = 0; i9 < frameArr.Length; i9++)
                    {
                        Frame frame = frameArr[i9];
                        int i10 = frame.CenterX;
                        int i11 = frame.CenterY;
                        int i12 = i10 + frame.Image.Width;
                        int i13 = i11 + frame.Image.Height;
                        if (i10 < i4)
                            i4 = i10;
                        if (i11 < i5)
                            i5 = i11;
                        if (i12 > i6)
                            i6 = i12;
                        if (i13 > i7)
                            i7 = i13;
                    }
                }
                if (i4 == int.MinValue)
                    return;
                i1 = i6 - i4;
                i2 = i7 - i5;
                m_xCenter = 0xA - i4 + (Math.Max(60 - i1, 0) / 2);
                m_yCenter = 0xA - i5 + (Math.Max(60 - i2, 0) / 2);
                if (i1 < 60)
                    i1 = 60;
                if (i2 < 60)
                    i2 = 60;
                i1 += 20;
                i2 += 20;
                if (m_Label != null)
                    m_yCenter += 24;
            }
            SetBounds(0, 0, i1, i2 + (m_Label == null ? 0 : 24), BoundsSpecified.Size);
            bool flag = i3 > 1;
            if (flag && !timRedraw.Enabled)
                timRedraw.Start();
            else if (!flag && timRedraw.Enabled)
                timRedraw.Stop();
            miOptionsLoop.Visible = flag;
            miOptionsPlay.Visible = flag;
            miOptionsStop.Visible = flag;
            miOptionsSep.Visible = flag;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (m_Animations != null)
                {
                    for (int i1 = 0; i1 < m_Animations.Length; i1++)
                    {
                        Animation animation = m_Animations[i1];
                        for (int i2 = 0; i2 < animation.Frames.Length; i2++)
                        {
                            animation.Frames[i2].Image.Dispose();
                        }
                    }
                }
            }
            base.Dispose(disposing);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //Log.WriteLine("OnPaint2");
            Graphics graphics = e.Graphics;
            graphics.Clear(Color.Black);
            ControlPaint.DrawBorder3D(graphics, 0, 0, base.Width, base.Height, Border3DStyle.Sunken);
            if (m_Label != null)
            {
                ControlPaint.DrawBorder3D(graphics, 2, 2, base.Width - 4, 20, Border3DStyle.Raised);
            }
            if (m_Animations != null)
            {
                int i1 = 0;
                if (m_Begin > DateTime.MinValue)
                {
                    i1 = (int)((DateTime.Now - m_Begin).Ticks / Interval.Ticks);
                }
                int j = m_xCenter;
                int k = m_yCenter;
                bool flag = false;
                if (miOptionsCenter.Checked)
                {
                    graphics.DrawLine(Pens.LightBlue, new Point(j - 5, k - 5), new Point(j + 5, k + 5));
                    graphics.DrawLine(Pens.LightBlue, new Point(j + 5, k - 5), new Point(j - 5, k + 5));
                    graphics.DrawEllipse(Pens.LightBlue, j - 5, k - 5, 10, 10);
                }
                for (int i2 = 0; i2 < (int)m_Animations.Length; i2++)
                {
                    Animation animation = m_Animations[i2];
                    Frame frame = animation.GetFrame(i1);
                    if (frame != null)
                    {
                        graphics.DrawImageUnscaled(frame.Image, j + frame.CenterX, k + frame.CenterY, frame.Image.Width, frame.Image.Height);
                    }
                    if (i1 >= animation.TotalDuration)
                    {
                        flag = true;
                    }
                }
                if (flag && !miOptionsLoop.Checked)
                {
                    miOptionsPlay.Enabled = true;
                    miOptionsStop.Enabled = false;
                    m_Begin = DateTime.MinValue;
                }
            }
        }

    } // class AnimationBox

}

