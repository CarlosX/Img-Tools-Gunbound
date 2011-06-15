using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ImgConvert
{
    public class StreamDisplay : Control
    {

        private ContextMenu cmContext;
        private Container components;
        private BorderStyle m_BorderStyle;
        private SolidBrush m_Brush;
        private byte[] m_Buffer;
        private RectangleF[] m_ByteBoundaries;
        private StringBuilder m_ByteStringBuffer;
        private int m_Caret;
        private RectangleF[] m_CharBoundaries;
        private StringBuilder m_CharStringBuffer;
        private int m_DragOffset;
        private string m_FileName;
        private bool m_IsCharDrag;
        private bool m_IsDragging;
        private int m_KnownLength;
        private Font m_LastFont;
        private int m_LineHeight;
        private StringBuilder m_LineStringBuffer;
        private GraphicsPath m_Path;
        private int m_Position;
        private CharacterRange[] m_Ranges;
        private Region m_Region;
        private int m_SelLength;
        private int m_SelStart;
        private Stream m_Stream;
        private StringFormat m_StringFormat;
        private PointF m_TempPointF;
        private Rectangle m_TempRect;
        private RectangleF m_TempRectF;
        private bool m_ValidSelection;
        private MenuItem miCopy;
        private MenuItem miSave;
        private MenuItem miSelectAll;
        private VScrollBar vScrollBar1;

        public BorderStyle BorderStyle
        {
            get
            {
                return m_BorderStyle;
            }
            set
            {
                if (m_BorderStyle == value)
                {
                }
                else
                {
                    m_BorderStyle = value;
                    m_ValidSelection = false;
                    RecreateHandle();
                }
            }
        }

        public int Caret
        {
            get
            {
                return m_Caret;
            }
            set
            {
                if (m_Caret == value)
                {
                }
                else
                {
                    m_Caret = value;
                    Invalidate();
                }
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

        public int Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                if (m_Position == value)
                {
                }
                else
                {
                    m_Position = value;
                    m_ValidSelection = false;
                    Invalidate();
                }
            }
        }

        public int SelLength
        {
            get
            {
                return m_SelLength;
            }
            set
            {
                if (m_SelLength == value)
                {
                }
                else
                {
                    m_SelLength = value;
                    m_ValidSelection = false;
                    Invalidate();
                }
            }
        }

        public int SelStart
        {
            get
            {
                return m_SelStart;
            }
            set
            {
                if (m_SelStart == value)
                {
                }
                else
                {
                    m_SelStart = value;
                    m_ValidSelection = false;
                    Invalidate();
                }
            }
        }

        public Stream Stream
        {
            get
            {
                return m_Stream;
            }
            set
            {
                if (m_Stream == value)
                {
                }
                else
                {
                    m_Stream = value;
                    m_ValidSelection = false;
                    Invalidate();
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams1 = base.CreateParams;
                switch (BorderStyle)
                {
                    case BorderStyle.Fixed3D:
                        createParams1.ExStyle |= 0x200;
                        break;

                    case BorderStyle.FixedSingle:
                        createParams1.Style |= 0x800000;
                        break;
                }
                return createParams1;
            }
        }

        public StreamDisplay()
        {
            this.components = null;
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.miCopy = cmContext.MenuItems.Add("&Copy", new EventHandler(Copy_OnClick));
            this.miSelectAll = cmContext.MenuItems.Add("S&elect All", new EventHandler(SelectAll_OnClick));
            this.cmContext.MenuItems.Add("-");
            this.miSave = cmContext.MenuItems.Add("&Save Stream", new EventHandler(SaveStream_OnClick));
            this.cmContext.Popup += new EventHandler(cmContext_Popup);
        }

        private void cmContext_Popup(object sender, EventArgs e)
        {
            this.UpdateMenus();
        }

        public void Copy()
        {
            if (((this.m_Stream != null) && (this.m_SelStart >= 0)) && (this.m_SelLength > 0))
            {
                byte[] buffer = new byte[this.m_SelLength];
                long position = this.m_Stream.Position;
                this.m_Stream.Position = this.m_SelStart;
                int capacity = this.m_Stream.Read(buffer, 0, this.m_SelLength);
                this.m_Stream.Position = position;
                if (this.m_IsCharDrag)
                {
                    StringBuilder builder = new StringBuilder(capacity);
                    for (int i = 0; i < capacity; i++)
                    {
                        int num4 = buffer[i];
                        if ((num4 >= 0x20) && (num4 < 0x80))
                        {
                            builder.Append((char)num4);
                        }
                        else
                        {
                            builder.Append('.');
                        }
                    }
                    Clipboard.SetDataObject(builder.ToString(), true);
                }
                else
                {
                    StringWriter output = new StringWriter();
                    FormatBuffer(output, buffer, 0, capacity, this.m_SelStart);
                    Clipboard.SetDataObject(output.ToString(), true);
                }
            }
        }

        private void Copy_OnClick(object sender, EventArgs e)
        {
            this.Copy();
        }

        public void EnsureCaretVisible()
        {
            int position = this.m_Position;
            if (this.m_Caret < this.m_Position)
            {
                position -= ((this.m_Position - this.m_Caret) + 15) & -16;
            }
            else
            {
                int lineHeight = this.m_LineHeight;
                int num3 = 0;
                int num4 = 0;
                num3 += 5;
                num4 += 5;
                int num5 = ((base.ClientSize.Height - (num4 * 2)) / lineHeight) - 2;
                int num6 = num5 * 0x10;
                if (this.m_Caret >= (this.m_Position + num6))
                {
                    position += (((this.m_Caret - this.m_Position) + 0x10) & -16) - num6;
                }
            }
            if (position < 0)
            {
                position = 0;
            }
            this.Position = position;
        }

        public bool GetByteOffset(int mx, int my, int type, ref int offset, ref bool charDrag)
        {
            if ((this.m_ByteBoundaries == null) || (this.m_CharBoundaries == null))
            {
                offset = -1;
                charDrag = false;
                return false;
            }
            int lineHeight = this.m_LineHeight;
            int num2 = 0;
            int num3 = 0;
            num2 += 5;
            num3 += 5;
            int num4 = (base.ClientSize.Height - (num3 * 2)) / lineHeight;
            mx -= num2;
            my -= num3;
            int num5 = (my / lineHeight) - 2;
            if ((num5 < 0) || (num5 >= num4))
            {
                offset = -1;
                charDrag = false;
                return false;
            }
            my = 5;
            this.m_TempPointF.X = mx;
            this.m_TempPointF.Y = my;
            if (type == 0)
            {
                for (int i = 0; i < 0x10; i++)
                {
                    if (this.m_ByteBoundaries[i].Contains(this.m_TempPointF))
                    {
                        offset = (num5 * 0x10) + i;
                        charDrag = false;
                        return true;
                    }
                    if (this.m_CharBoundaries[i].Contains(this.m_TempPointF))
                    {
                        offset = (num5 * 0x10) + i;
                        charDrag = true;
                        return true;
                    }
                }
            }
            else
            {
                if (type == 1)
                {
                    for (int j = 0; j < 0x10; j++)
                    {
                        if (this.m_ByteBoundaries[j].Contains(this.m_TempPointF))
                        {
                            offset = (num5 * 0x10) + j;
                            charDrag = false;
                            return true;
                        }
                    }
                    if (this.m_TempPointF.X <= this.m_ByteBoundaries[0].X)
                    {
                        offset = num5 * 0x10;
                        return false;
                    }
                    offset = (num5 * 0x10) + 0x10;
                    return false;
                }
                if (type == 2)
                {
                    for (int k = 0; k < 0x10; k++)
                    {
                        if (this.m_CharBoundaries[k].Contains(this.m_TempPointF))
                        {
                            offset = (num5 * 0x10) + k;
                            charDrag = true;
                            return true;
                        }
                    }
                    if (this.m_TempPointF.X <= this.m_CharBoundaries[0].X)
                    {
                        offset = num5 * 0x10;
                        return false;
                    }
                    offset = (num5 * 0x10) + 0x10;
                    return false;
                }
            }
            offset = -1;
            charDrag = false;
            return false;
        }

        private void InitializeComponent()
        {
            this.vScrollBar1 = new VScrollBar();
            this.cmContext = new ContextMenu();
            this.SuspendLayout();
            this.vScrollBar1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            this.vScrollBar1.LargeChange = 0x100;
            this.vScrollBar1.Location = new Point(0x158, 0);
            this.vScrollBar1.Maximum = 0x186A0;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new Size(0x10, 0x178);
            this.vScrollBar1.SmallChange = 0x10;
            this.vScrollBar1.TabIndex = 0;
            this.vScrollBar1.Value = 0xC350;
            this.vScrollBar1.Scroll += new ScrollEventHandler(vScrollBar1_Scroll);
            this.BackColor = SystemColors.Window;
            this.ContextMenu = cmContext;
            this.Controls.Add(vScrollBar1);
            this.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Size = new Size(0x190, 0x190);
            this.ResumeLayout(false);
        }

        public Region[] Measure(Graphics g, Font f, string str, int x, int y, int xOffset, int yOffset, int cw, int index, int length)
        {
            if (this.m_Ranges == null)
            {
                this.m_Ranges = new CharacterRange[1];
            }
            this.m_Ranges[0].First = index;
            this.m_Ranges[0].Length = length;
            this.m_StringFormat.SetMeasurableCharacterRanges(this.m_Ranges);
            this.m_TempRectF.X = x;
            this.m_TempRectF.Y = y;
            this.m_TempRectF.Width = cw;
            this.m_TempRectF.Height = this.m_LineHeight;
            return g.MeasureCharacterRanges(str, f, this.m_TempRectF, this.m_StringFormat);
        }

        public void Outline(ref Region region, ref GraphicsPath path, Graphics g, Font f, string str, int x, int y, int xOffset, int yOffset, int cw, int lineHeight, int border, int index, int length)
        {
            if (region == null)
            {
                if (this.m_Region == null)
                {
                    this.m_Region = new Region();
                }
                this.m_Region.MakeInfinite();
                region = this.m_Region;
            }
            if (path == null)
            {
                if (this.m_Path == null)
                {
                    this.m_Path = new GraphicsPath();
                }
                else
                {
                    this.m_Path.Reset();
                }
                path = this.m_Path;
            }
            Region[] regionArray = this.Measure(g, f, str, 0, y, xOffset, yOffset, cw, index, length);
            for (int i = 0; i < regionArray.Length; i++)
            {
                Rectangle rect = Rectangle.Ceiling(regionArray[i].GetBounds(g));
                rect.X -= border;
                rect.Width += border * 2;
                rect.Y = y + 1;
                rect.Height = lineHeight;
                region.Exclude(rect);
                path.AddRectangle(rect);
                this.m_Brush.Color = ControlPaint.Dark(this.BackColor, -0.4f);
                g.FillRectangle(this.m_Brush, rect);
                this.m_Brush.Color = this.ForeColor;
            }
        }

        private void SaveStream_OnClick(object sender, EventArgs e)
        {
            if ((this.m_Stream != null) && (this.m_KnownLength >= 0))
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    OverwritePrompt = true,
                    Filter = "All files|*.*",
                    FileName = (this.m_FileName == null) ? "Data.dat" : this.m_FileName,
                    Title = "Save Stream"
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        int num2;
                        byte[] buffer = new byte[0x10000];
                        long position = this.m_Stream.Position;
                        this.m_Stream.Position = 0L;
                        do
                        {
                            num2 = this.m_Stream.Read(buffer, 0, buffer.Length);
                            if (num2 > 0)
                            {
                                stream.Write(buffer, 0, num2);
                            }
                        }
                        while (num2 > 0);
                        this.m_Stream.Position = position;
                    }
                }
            }
        }

        private void SelectAll_OnClick(object sender, EventArgs e)
        {
            if ((this.m_Stream != null) && (this.m_KnownLength >= 0))
            {
                this.SetSelection(0, this.m_KnownLength);
            }
        }

        public void SetCaret(int caret)
        {
            if (caret < 0)
            {
                caret = 0;
            }
            if ((this.m_KnownLength >= 0) && (caret > this.m_KnownLength))
            {
                caret = this.m_KnownLength;
            }
            this.Caret = caret;
        }

        public void SetSelection(int start, int length)
        {
            if (this.m_KnownLength >= 0)
            {
                if (start >= this.m_KnownLength)
                {
                    start = this.m_KnownLength - 1;
                }
                if ((start + length) > this.m_KnownLength)
                {
                    length = this.m_KnownLength - start;
                }
            }
            if ((this.m_SelStart != start) || (this.m_SelLength != length))
            {
                this.m_SelStart = start;
                this.m_SelLength = length;
                this.m_ValidSelection = false;
                base.Invalidate();
            }
        }

        public void UpdateMenus()
        {
            this.miCopy.Enabled = ((this.m_Stream != null) && (this.m_SelStart >= 0)) && (this.m_SelLength > 0);
            this.miSelectAll.Enabled = ((this.m_Stream != null) && (this.m_KnownLength > 0)) && ((this.m_SelStart != 0) || (this.m_SelLength != this.m_KnownLength));
            this.miSave.Enabled = (this.m_Stream != null) && (this.m_KnownLength >= 0);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int newValue;
            if (this.m_KnownLength >= 0)
            {
                newValue = e.NewValue;
            }
            else
            {
                newValue = (this.m_Position + e.NewValue) - 0xc350;
                e.NewValue = 0xc350;
            }
            newValue &= -16;
            newValue |= this.m_Position & 15;
            if (newValue < 0)
            {
                newValue = 0;
            }
            this.Position = newValue;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (m_Region != null)
                    m_Region.Dispose();
                if (m_Path != null)
                    m_Path.Dispose();
                if (m_Brush != null)
                    m_Brush.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Focus();
                int i = 0;
                bool flag = false;
                if (GetByteOffset(e.X + 2, e.Y, 0, ref i, ref flag))
                {
                    this.m_IsDragging = true;
                    this.m_IsCharDrag = flag;
                    this.m_DragOffset = m_Position + i;
                    this.SetSelection(m_DragOffset, 0);
                    this.SetCaret(m_DragOffset);
                }
                else
                {
                    this.m_IsDragging = false;
                    this.m_DragOffset = -1;
                    this.SetSelection(-1, 0);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && m_IsDragging)
            {
                int i = 0;
                bool flag = false;
                GetByteOffset(e.X + (m_IsCharDrag ? 2 : 6), e.Y, m_IsCharDrag ? 2 : 1, ref i, ref flag);
                i += m_Position;
                if (i < m_DragOffset)
                    SetSelection(i, m_DragOffset - i);
                else
                    SetSelection(m_DragOffset, i - m_DragOffset);
                SetCaret(i);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_IsDragging = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int i = Position;
            if (e.Delta > 0)
                i -= 0x30;
            else if (e.Delta < 0)
                i += 0x30;
            if (i < 0)
                i = 0;
            Position = i;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Log.WriteLine("OnPaint1");
            if (this.m_Brush == null)
            {
                this.m_Brush = new SolidBrush(this.ForeColor);
            }
            else if (this.m_Brush.Color != this.ForeColor)
            {
                this.m_Brush.Color = this.ForeColor;
            }
            Graphics g = e.Graphics;
            Font font = this.Font;
            Brush brush = this.m_Brush;
            int xOffset = 0;
            int y = 0;
            Size clientSize = base.ClientSize;
            int width = clientSize.Width;
            int height = clientSize.Height;
            this.vScrollBar1.SetBounds((width - xOffset) - this.vScrollBar1.Width, y, this.vScrollBar1.Width, (height - y) - y);
            xOffset += 5;
            y += 5;
            g.TranslateTransform((float)xOffset, (float)y);
            if (this.m_StringFormat == null)
            {
                this.m_StringFormat = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
            }
            if (this.m_Stream == null)
            {
                g.DrawString("No input specified", font, brush, 0f, 0f, this.m_StringFormat);
                return;
            }
            if (this.m_LastFont != font)
            {
                this.m_LineHeight = (int)g.MeasureString(" -0123456789ABCDEF", font).Height;
                this.m_ValidSelection = false;
            }
            if (((this.m_LastFont != font) || (this.m_ByteBoundaries == null)) || (this.m_CharBoundaries == null))
            {
                this.m_ByteBoundaries = new RectangleF[0x10];
                this.m_CharBoundaries = new RectangleF[0x10];
                string str = "________   __ __ __ __ __ __ __ __  __ __ __ __ __ __ __ __   ________________";
                for (int j = 0; j < 0x10; j++)
                {
                    Region[] regionArray = this.Measure(g, font, str, 0, 0, -xOffset, -y, width, (11 + (j * 3)) + ((j > 7) ? 1 : 0), (j == 7) ? 4 : 3);
                    if (regionArray.Length > 0)
                    {
                        this.m_ByteBoundaries[j] = regionArray[0].GetBounds(g);
                    }
                    regionArray = this.Measure(g, font, str, 0, 0, -xOffset, -y, width, 0x3e + j, 1);
                    if (regionArray.Length > 0)
                    {
                        this.m_CharBoundaries[j] = regionArray[0].GetBounds(g);
                    }
                }
                int num6 = 0;
                num6++;
            }
            int lineHeight = this.m_LineHeight;
            int num8 = (height - (y * 2)) / lineHeight;
            if (this.m_Buffer == null)
            {
                this.m_Buffer = new byte[0x10];
            }
            long position = this.m_Stream.Position;
            if (this.m_LineStringBuffer == null)
            {
                this.m_LineStringBuffer = new StringBuilder(0x4e);
            }
            if (this.m_ByteStringBuffer == null)
            {
                this.m_ByteStringBuffer = new StringBuilder(0x30);
            }
            if (this.m_CharStringBuffer == null)
            {
                this.m_CharStringBuffer = new StringBuilder(0x10);
            }
            StringBuilder lineStringBuffer = this.m_LineStringBuffer;
            StringBuilder byteStringBuffer = this.m_ByteStringBuffer;
            StringBuilder charStringBuffer = this.m_CharStringBuffer;
            int num10 = this.m_Position;
            this.m_Stream.Position = num10;
            Region region = null;
            GraphicsPath path = null;
            Rectangle clipRectangle = e.ClipRectangle;
            try
            {
                this.m_KnownLength = (int)this.m_Stream.Length;
            }
            catch
            {
                this.m_KnownLength = -1;
            }
            for (int i = 0; i < num8; i++)
            {
                int num12 = i * lineHeight;
                switch (i)
                {
                    case 0:
                        this.m_TempRect.X = xOffset;
                        this.m_TempRect.Y = y + num12;
                        this.m_TempRect.Width = width;
                        this.m_TempRect.Height = lineHeight;
                        if (clipRectangle.IntersectsWith(this.m_TempRect))
                        {
                            g.DrawString("            0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F", font, brush, 0f, (float)num12, this.m_StringFormat);
                        }
                        break;

                    case 1:
                        this.m_TempRect.X = xOffset;
                        this.m_TempRect.Y = y + num12;
                        this.m_TempRect.Width = width;
                        this.m_TempRect.Height = lineHeight;
                        if (clipRectangle.IntersectsWith(this.m_TempRect))
                        {
                            g.DrawString("           -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --", font, brush, 0f, (float)num12, this.m_StringFormat);
                        }
                        break;

                    default:
                        {
                            int num13 = this.m_Stream.Read(this.m_Buffer, 0, 0x10);
                            bool flag = false;
                            if (num13 < 0x10)
                            {
                                if (this.m_KnownLength == -1)
                                {
                                    for (int n = num13; n < 0x10; n++)
                                    {
                                        switch ((n & 3))
                                        {
                                            case 0:
                                                this.m_Buffer[n] = 0xde;
                                                break;

                                            case 1:
                                                this.m_Buffer[n] = 0xad;
                                                break;

                                            case 2:
                                                this.m_Buffer[n] = 190;
                                                break;

                                            case 3:
                                                this.m_Buffer[n] = 0xef;
                                                break;
                                        }
                                    }
                                    num13 = 0x10;
                                }
                                else
                                {
                                    if (num13 == 0)
                                    {
                                        goto Label_07D2;
                                    }
                                    flag = true;
                                }
                            }
                            lineStringBuffer.Length = 0;
                            byteStringBuffer.Length = 0;
                            charStringBuffer.Length = 0;
                            for (int k = 0; k < num13; k++)
                            {
                                int num16 = this.m_Buffer[k];
                                byteStringBuffer.Append(num16.ToString("X2"));
                                if (k == 7)
                                {
                                    byteStringBuffer.Append("  ");
                                }
                                else if (k < 15)
                                {
                                    byteStringBuffer.Append(' ');
                                }
                                if ((num16 >= 0x20) && (num16 < 0x80))
                                {
                                    charStringBuffer.Append((char)num16);
                                }
                                else
                                {
                                    charStringBuffer.Append('.');
                                }
                            }
                            for (int m = num13; m < 0x10; m++)
                            {
                                if (m == 7)
                                {
                                    byteStringBuffer.Append("    ");
                                }
                                else if (m < 15)
                                {
                                    byteStringBuffer.Append("   ");
                                }
                                else
                                {
                                    byteStringBuffer.Append("  ");
                                }
                                charStringBuffer.Append(' ');
                            }
                            lineStringBuffer.Append(num10.ToString("X8"));
                            lineStringBuffer.Append("   ");
                            lineStringBuffer.Append(byteStringBuffer);
                            lineStringBuffer.Append("   ");
                            lineStringBuffer.Append(charStringBuffer);
                            lineStringBuffer.Append(' ');
                            string str2 = lineStringBuffer.ToString();
                            if (((this.m_SelStart < (num10 + 0x10)) && ((this.m_SelStart + this.m_SelLength) > num10)) && (this.m_SelLength > 0))
                            {
                                if (this.m_ValidSelection)
                                {
                                    if ((region == null) && (this.m_Region != null))
                                    {
                                        Region clip = g.Clip;
                                        g.SetClip(this.m_Region, CombineMode.Xor);
                                        g.Clear(ControlPaint.Dark(this.BackColor, -0.4f));
                                        g.SetClip(clip, CombineMode.Replace);
                                    }
                                    region = this.m_Region;
                                    path = this.m_Path;
                                }
                                else
                                {
                                    int num18 = this.m_SelStart - num10;
                                    int num19 = ((this.m_SelStart + this.m_SelLength) - 1) - num10;
                                    if (num18 < 0)
                                    {
                                        num18 = 0;
                                    }
                                    if (num19 > 15)
                                    {
                                        num19 = 15;
                                    }
                                    int length = (num19 - num18) + 1;
                                    this.Outline(ref region, ref path, g, font, str2, 0, num12, xOffset, y, width, lineHeight, 2, (11 + (num18 * 3)) + ((num18 > 7) ? 1 : 0), ((length * 3) - 1) + (((num19 > 7) && (num18 <= 7)) ? 1 : 0));
                                    this.Outline(ref region, ref path, g, font, str2, 0, num12, xOffset, y, width, lineHeight, 0, 0x3e + num18, length);
                                }
                            }
                            if (((this.m_SelLength == 0) && (this.m_Caret < (num10 + 0x10))) && (this.m_Caret >= num10))
                            {
                                g.FillRectangle(brush, (this.m_IsCharDrag ? this.m_CharBoundaries[(this.m_Caret - this.m_Position) % 0x10].X : this.m_ByteBoundaries[(this.m_Caret - this.m_Position) % 0x10].X) - 2f, (float)(num12 + 1), 1f, (float)lineHeight);
                            }
                            this.m_TempRect.X = xOffset;
                            this.m_TempRect.Y = y + num12;
                            this.m_TempRect.Width = width;
                            this.m_TempRect.Height = lineHeight;
                            if (clipRectangle.IntersectsWith(this.m_TempRect))
                            {
                                g.DrawString(str2, font, brush, 0f, (float)num12, this.m_StringFormat);
                            }
                            num10 += 0x10;
                            if (flag)
                            {
                                goto Label_07D2;
                            }
                            break;
                        }
                }
            }
        Label_07D2:
            if (region != null)
            {
                g.SetClip(region, CombineMode.Intersect);
                g.TranslateTransform(-1f, -1f, MatrixOrder.Append);
                g.FillPath(brush, path);
                g.TranslateTransform(0f, 2f, MatrixOrder.Append);
                g.FillPath(brush, path);
                g.TranslateTransform(2f, -2f, MatrixOrder.Append);
                g.FillPath(brush, path);
                g.TranslateTransform(0f, 2f, MatrixOrder.Append);
                g.FillPath(brush, path);
                g.ResetClip();
                this.m_ValidSelection = true;
            }
            this.m_LastFont = font;
            if (this.m_KnownLength >= 0)
            {
                if (this.vScrollBar1.Maximum != this.m_KnownLength)
                {
                    this.vScrollBar1.Maximum = this.m_KnownLength;
                }
                if (this.vScrollBar1.Value != this.m_Position)
                {
                    this.vScrollBar1.Value = this.m_Position;
                }
            }
            else
            {
                if (this.vScrollBar1.Maximum != 0x186a0)
                {
                    this.vScrollBar1.Maximum = 0x186a0;
                }
                if (this.vScrollBar1.Value != 0xc350)
                {
                    this.vScrollBar1.Value = 0xc350;
                }
            }
            this.m_Stream.Position = position;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            m_ByteBoundaries = null;
            m_CharBoundaries = null;
            m_ValidSelection = false;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            bool flag;

            int i1 = 0;
            switch ((Keys)(keyData & Keys.KeyCode))
            {
                case Keys.Up:
                    i1 = 10;
                    break;

                case Keys.Down:
                    i1 = 0x10;
                    break;

                case Keys.Left:
                    i1 = 1;
                    break;

                case Keys.Right:
                    i1 = 1;
                    break;

                case Keys.Prior:
                    i1 = -256;
                    break;

                case Keys.PageDown:
                    i1 = 0x100;
                    break;

                case Keys.End:
                    i1 = 0x10 - ((m_Caret - m_Position) & 0xF);
                    break;

                case Keys.Home:
                    i1 = -((m_Caret - m_Position) & 0xF);
                    break;

                case Keys.C:
                    if ((keyData & Keys.Control) != Keys.None)
                        Copy();
                    break;
            }
            if (i1 == 0)
            {
                flag = base.ProcessDialogKey(keyData);
            }
            else
            {
                int i2 = m_Caret;
                SetCaret(m_Caret + i1);
                if ((keyData & Keys.Shift) != Keys.None)
                {
                    if (m_DragOffset == -1)
                        m_DragOffset = i2;
                    if (m_Caret < m_DragOffset)
                        SetSelection(m_Caret, m_DragOffset - m_Caret);
                    else
                        SetSelection(m_DragOffset, m_Caret - m_DragOffset);
                }
                else
                {
                    if (m_SelStart >= 0)
                    {
                        if (i1 > 0)
                            SetCaret(m_SelStart + m_SelLength + i1);
                        else
                            SetCaret(m_SelStart + i1);
                    }
                    m_DragOffset = -1;
                    SetSelection(-1, 0);
                }
                EnsureCaretVisible();
                flag = true;
            }
            return flag;
        }

        public static void FormatBuffer(TextWriter output, byte[] buffer, int offset, int length, int bonus)
        {
            output.WriteLine("            0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("           -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");
            int i1 = 0;
            int i2 = length >> 4;
            int i3 = length & 0xF;
            int i4 = 0;
            while (i4 < i2)
            {
                StringBuilder stringBuilder1 = new StringBuilder(0x31);
                StringBuilder stringBuilder2 = new StringBuilder(0x10);
                for (int i5 = 0; i5 < 0x10; i5++)
                {
                    int i6 = buffer[i1 + offset + i5];
                    stringBuilder1.Append(i6.ToString("X2"));
                    if (i5 != 7)
                        stringBuilder1.Append(' ');
                    else
                        stringBuilder1.Append("  ");
                    if ((i6 >= 0x20) && (i6 < 0x80))
                        stringBuilder2.Append((ushort)i6);
                    else
                        stringBuilder2.Append('.');
                }
                int i9 = i1 + bonus;
                output.Write(i9.ToString("X8"));
                output.Write("   ");
                output.Write(stringBuilder1.ToString());
                output.Write("  ");
                output.WriteLine(stringBuilder2.ToString());
                i4++;
                i1 += 0x10;
            }
            if (i3 != 0)
            {
                StringBuilder stringBuilder3 = new StringBuilder(0x31);
                StringBuilder stringBuilder4 = new StringBuilder(i3);
                for (int i7 = 0; i7 < 0x10; i7++)
                {
                    if (i7 < i3)
                    {
                        int i8 = buffer[i1 + offset + i7];
                        stringBuilder3.Append(i8.ToString("X2"));
                        if (i7 != 7)
                            stringBuilder3.Append(' ');
                        else
                            stringBuilder3.Append("  ");
                        if ((i8 >= 0x20) && (i8 < 0x80))
                            stringBuilder4.Append((ushort)i8);
                        else
                            stringBuilder4.Append('.');
                    }
                    else
                    {
                        if (i7 == 7)
                        {
                            stringBuilder3.Append("    ");
                            continue;
                        }
                        stringBuilder3.Append("   ");
                    }
                }
                int i10 = i1 + bonus;
                output.Write(i10.ToString("X8"));
                output.Write("   ");
                output.Write(stringBuilder3.ToString());
                output.Write("  ");
                output.WriteLine(stringBuilder4.ToString());
            }
        }

    } // class StreamDisplay
}
