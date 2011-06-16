/*  ----------------------------------------------------------------------------
 *  Copyright (C) 2011 XfsGames <http://www.xfsgames.com.ar/>
 *  ----------------------------------------------------------------------------
 *  Img Tools
 *  ----------------------------------------------------------------------------
 *  File:       FileTabSheet.cs
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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace ImgTools
{

    public class FileTabSheet : UserControl
    {

        private Container components;
        private Panel pnDecodedPanel;
        private TabControl tabControl1;
        private TabPage tbpDecoded;
        private TabPage tbpRawData;

        public Panel DecodedPanel
        {
            get
            {
                return pnDecodedPanel;
            }
        }

        public FileTabSheet()
        {
            components = null;
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpDecoded = new System.Windows.Forms.TabPage();
            this.pnDecodedPanel = new System.Windows.Forms.Panel();
            this.tbpRawData = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tbpDecoded.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbpDecoded);
            this.tabControl1.Controls.Add(this.tbpRawData);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(454, 431);
            this.tabControl1.TabIndex = 0;
            // 
            // tbpDecoded
            // 
            this.tbpDecoded.Controls.Add(this.pnDecodedPanel);
            this.tbpDecoded.Location = new System.Drawing.Point(4, 22);
            this.tbpDecoded.Name = "tbpDecoded";
            this.tbpDecoded.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.tbpDecoded.Size = new System.Drawing.Size(446, 405);
            this.tbpDecoded.TabIndex = 0;
            this.tbpDecoded.Text = "Decoded";
            // 
            // pnDecodedPanel
            // 
            this.pnDecodedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnDecodedPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnDecodedPanel.Location = new System.Drawing.Point(-4, 0);
            this.pnDecodedPanel.Name = "pnDecodedPanel";
            this.pnDecodedPanel.Size = new System.Drawing.Size(454, 409);
            this.pnDecodedPanel.TabIndex = 0;
            // 
            // tbpRawData
            // 
            this.tbpRawData.Location = new System.Drawing.Point(4, 22);
            this.tbpRawData.Name = "tbpRawData";
            this.tbpRawData.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.tbpRawData.Size = new System.Drawing.Size(446, 405);
            this.tbpRawData.TabIndex = 1;
            this.tbpRawData.Text = "Raw Data";
            // 
            // FileTabSheet
            // 
            this.Controls.Add(this.tabControl1);
            this.Name = "FileTabSheet";
            this.Size = new System.Drawing.Size(454, 431);
            this.tabControl1.ResumeLayout(false);
            this.tbpDecoded.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public void SetFile(ArchivedFile file)
        {
            //Log.WriteLine("SetFile");
            MemoryStream memoryStream = new MemoryStream();
            file.Download(memoryStream);
            SetStream(file.FileName, memoryStream);
        }

        public void SetStream(string fileName, Stream stream)
        {
            //Log.WriteLine("SetStream");
            tbpRawData.Controls.Clear();
            StreamDisplay streamDisplay = new StreamDisplay();
            streamDisplay.FileName = fileName;
            streamDisplay.BorderStyle = BorderStyle.Fixed3D;
            streamDisplay.Stream = stream;
            streamDisplay.Position = 0;
            streamDisplay.Dock = DockStyle.Fill;
            tbpRawData.Controls.Add(streamDisplay);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

    } // class FileTabSheet

}