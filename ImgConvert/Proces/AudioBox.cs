using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImgConvert
{

    public class AudioBox : UserControl
    {

        private CheckBox chkLoop;
        private ContextMenu cmContext;
        private Button cmdDummy;
        private Button cmdPause;
        private Button cmdPlay;
        private Button cmdStop;
        private IContainer components;
        private GroupBox gbGroup;
        private Label lblDuration;
        private Label lblStatus;
        private IAudio m_Sound;
        private MenuItem miLoop;
        private MenuItem miPause;
        private MenuItem miPlay;
        private MenuItem miSave;
        private MenuItem miSep;
        private MenuItem miStop;
        private TrackBar tbProgress;
        private Timer timUpdate;

        public IAudio Sound
        {
            get
            {
                return m_Sound;
            }
            set
            {
                m_Sound = value;
                UpdateControls();
            }
        }

        public AudioBox()
        {
            InitializeComponent();
            cmdDummy.Focus();
            cmdPlay.GotFocus += new EventHandler(ChangeFocus_Callback);
            cmdPause.GotFocus += new EventHandler(ChangeFocus_Callback);
            cmdStop.GotFocus += new EventHandler(ChangeFocus_Callback);
        }

        private void ChangeFocus_Callback(object sender, EventArgs e)
        {
            cmdDummy.Focus();
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cmContext_Popup(object sender, EventArgs e)
        {
            miPlay.Enabled = cmdPlay.Enabled;
            miPause.Enabled = cmdPause.Enabled;
            miStop.Enabled = cmdStop.Enabled;
            miLoop.Checked = chkLoop.Checked;
        }

        private void cmdPause_Click(object sender, EventArgs e)
        {
            if (m_Sound != null)
                m_Sound.Pause();
            UpdateControls();
        }

        private void cmdPlay_Click(object sender, EventArgs e)
        {
            if (m_Sound != null)
                m_Sound.Play(chkLoop.Checked);
            UpdateControls();
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            if (m_Sound != null)
                m_Sound.Stop();
            UpdateControls();
        }

        public string ConvertTimeSpan(TimeSpan ts)
        {
            int i1 = (int)ts.TotalMilliseconds;
            int i2 = i1 / 0x3E8;
            int i3 = i2 / 0x3C;
            i1 %= 0x3E8;
            i2 %= 0x3C;
            return String.Format("{0}:{1:D2}:{2:D2}", i3, i2, i1 / 0xA);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbGroup = new System.Windows.Forms.GroupBox();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.cmdDummy = new System.Windows.Forms.Button();
            this.cmdStop = new System.Windows.Forms.Button();
            this.cmdPause = new System.Windows.Forms.Button();
            this.cmdPlay = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.tbProgress = new System.Windows.Forms.TrackBar();
            this.timUpdate = new System.Windows.Forms.Timer(this.components);
            this.cmContext = new System.Windows.Forms.ContextMenu();
            this.miPlay = new System.Windows.Forms.MenuItem();
            this.miPause = new System.Windows.Forms.MenuItem();
            this.miStop = new System.Windows.Forms.MenuItem();
            this.miLoop = new System.Windows.Forms.MenuItem();
            this.miSep = new System.Windows.Forms.MenuItem();
            this.miSave = new System.Windows.Forms.MenuItem();
            this.gbGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbProgress)).BeginInit();
            this.SuspendLayout();
            // 
            // gbGroup
            // 
            this.gbGroup.Controls.Add(this.chkLoop);
            this.gbGroup.Controls.Add(this.cmdDummy);
            this.gbGroup.Controls.Add(this.cmdStop);
            this.gbGroup.Controls.Add(this.cmdPause);
            this.gbGroup.Controls.Add(this.cmdPlay);
            this.gbGroup.Controls.Add(this.lblStatus);
            this.gbGroup.Controls.Add(this.lblDuration);
            this.gbGroup.Controls.Add(this.tbProgress);
            this.gbGroup.Location = new System.Drawing.Point(8, 8);
            this.gbGroup.Name = "gbGroup";
            this.gbGroup.Size = new System.Drawing.Size(269, 101);
            this.gbGroup.TabIndex = 0;
            this.gbGroup.TabStop = false;
            this.gbGroup.Text = "Audio";
            // 
            // chkLoop
            // 
            this.chkLoop.Location = new System.Drawing.Point(176, 64);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(56, 24);
            this.chkLoop.TabIndex = 5;
            this.chkLoop.Text = "&Loop";
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // cmdDummy
            // 
            this.cmdDummy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdDummy.Location = new System.Drawing.Point(156, 156);
            this.cmdDummy.Name = "cmdDummy";
            this.cmdDummy.Size = new System.Drawing.Size(20, 20);
            this.cmdDummy.TabIndex = 0;
            // 
            // cmdStop
            // 
            this.cmdStop.Enabled = false;
            this.cmdStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop.Location = new System.Drawing.Point(120, 64);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(48, 24);
            this.cmdStop.TabIndex = 3;
            this.cmdStop.Text = "&Stop";
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // cmdPause
            // 
            this.cmdPause.Enabled = false;
            this.cmdPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdPause.Location = new System.Drawing.Point(64, 64);
            this.cmdPause.Name = "cmdPause";
            this.cmdPause.Size = new System.Drawing.Size(48, 24);
            this.cmdPause.TabIndex = 2;
            this.cmdPause.Text = "P&ause";
            this.cmdPause.Click += new System.EventHandler(this.cmdPause_Click);
            // 
            // cmdPlay
            // 
            this.cmdPlay.Enabled = false;
            this.cmdPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdPlay.Location = new System.Drawing.Point(8, 64);
            this.cmdPlay.Name = "cmdPlay";
            this.cmdPlay.Size = new System.Drawing.Size(48, 24);
            this.cmdPlay.TabIndex = 1;
            this.cmdPlay.Text = "&Play";
            this.cmdPlay.Click += new System.EventHandler(this.cmdPlay_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(8, 40);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(104, 16);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "No sound loaded";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDuration
            // 
            this.lblDuration.Location = new System.Drawing.Point(128, 40);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(128, 16);
            this.lblDuration.TabIndex = 2;
            this.lblDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbProgress
            // 
            this.tbProgress.AutoSize = false;
            this.tbProgress.Enabled = false;
            this.tbProgress.LargeChange = 2500;
            this.tbProgress.Location = new System.Drawing.Point(8, 16);
            this.tbProgress.Name = "tbProgress";
            this.tbProgress.Size = new System.Drawing.Size(248, 20);
            this.tbProgress.SmallChange = 500;
            this.tbProgress.TabIndex = 4;
            this.tbProgress.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbProgress.Scroll += new System.EventHandler(this.tbProgress_Scroll);
            // 
            // timUpdate
            // 
            this.timUpdate.Enabled = true;
            this.timUpdate.Interval = 30;
            this.timUpdate.Tick += new System.EventHandler(this.timUpdate_Tick);
            // 
            // cmContext
            // 
            this.cmContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miPlay,
            this.miPause,
            this.miStop,
            this.miLoop,
            this.miSep,
            this.miSave});
            this.cmContext.Popup += new System.EventHandler(this.cmContext_Popup);
            // 
            // miPlay
            // 
            this.miPlay.Index = 0;
            this.miPlay.Text = "&Play";
            this.miPlay.Click += new System.EventHandler(this.miPlay_Click);
            // 
            // miPause
            // 
            this.miPause.Index = 1;
            this.miPause.Text = "P&ause";
            this.miPause.Click += new System.EventHandler(this.miPause_Click);
            // 
            // miStop
            // 
            this.miStop.Index = 2;
            this.miStop.Text = "&Stop";
            this.miStop.Click += new System.EventHandler(this.miStop_Click);
            // 
            // miLoop
            // 
            this.miLoop.Index = 3;
            this.miLoop.Text = "&Loop";
            this.miLoop.Click += new System.EventHandler(this.miLoop_Click);
            // 
            // miSep
            // 
            this.miSep.Index = 4;
            this.miSep.Text = "-";
            // 
            // miSave
            // 
            this.miSave.Index = 5;
            this.miSave.Text = "Sa&ve As";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // AudioBox
            // 
            this.ContextMenu = this.cmContext;
            this.Controls.Add(this.gbGroup);
            this.Name = "AudioBox";
            this.Size = new System.Drawing.Size(280, 112);
            this.gbGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbProgress)).EndInit();
            this.ResumeLayout(false);

        }

        private void miLoop_Click(object sender, EventArgs e)
        {
            chkLoop.Checked = !chkLoop.Checked;
        }

        private void miPause_Click(object sender, EventArgs e)
        {
            cmdPause_Click(cmdPlay, EventArgs.Empty);
        }

        private void miPlay_Click(object sender, EventArgs e)
        {
            cmdPlay_Click(cmdPlay, EventArgs.Empty);
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            if (m_Sound == null)
            {
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.Filter = m_Sound.FilterString;
                saveFileDialog.FileName = m_Sound.FileName;
                saveFileDialog.Title = "Save Stream";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        m_Sound.SaveFile(fileStream);
                    }
                }
            }
        }

        private void miStop_Click(object sender, EventArgs e)
        {
            cmdStop_Click(cmdPlay, EventArgs.Empty);
        }

        private void tbProgress_Scroll(object sender, EventArgs e)
        {
            if (m_Sound != null)
                m_Sound.Position = TimeSpan.FromTicks((long)(tbProgress.Value * 0x3E8));
        }

        private void timUpdate_Tick(object sender, EventArgs e)
        {
            UpdateControls();
        }

        public void UpdateControls()
        {
            UpdateDuration();
            if (m_Sound == null)
            {
                lblStatus.Text = "No sound loaded";
                cmdPlay.Enabled = false;
                cmdPause.Enabled = false;
                cmdStop.Enabled = false;
            }
            else if (m_Sound.Playing)
            {
                lblStatus.Text = "Playing";
                cmdPlay.Enabled = false;
                cmdPause.Enabled = true;
                cmdStop.Enabled = true;
            }
            else if (m_Sound.Position > TimeSpan.Zero)
            {
                lblStatus.Text = "Paused";
                cmdPlay.Enabled = true;
                cmdPause.Enabled = false;
                cmdStop.Enabled = true;
            }
            else
            {
                lblStatus.Text = "Stopped";
                cmdPlay.Enabled = true;
                cmdPause.Enabled = false;
                cmdStop.Enabled = false;
            }
        }

        public void UpdateDuration()
        {
            if (m_Sound == null)
            {
                lblDuration.Text = "";
                tbProgress.Value = 0;
                tbProgress.SetRange(0, 0xA);
                tbProgress.Enabled = false;
            }
            else
            {
                tbProgress.Enabled = true;
                TimeSpan timeSpan1 = m_Sound.Position;
                TimeSpan timeSpan2 = m_Sound.Length;
                if (timeSpan1 > TimeSpan.Zero || m_Sound.Playing)
                    lblDuration.Text = String.Format("{0} / {1}", ConvertTimeSpan(timeSpan1), ConvertTimeSpan(timeSpan2));
                else
                    lblDuration.Text = ConvertTimeSpan(timeSpan2);
                tbProgress.SetRange(0, (int)(timeSpan2.Ticks / (long)0x3E8));
                tbProgress.Value = (int)(timeSpan1.Ticks / (long)0x3E8);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (m_Sound != null)
                    m_Sound.Dispose();
            }
            base.Dispose(disposing);
        }

    } // class AudioBox

}

