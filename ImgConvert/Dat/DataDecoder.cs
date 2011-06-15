using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImgConvert
{
    public class DataDecoder : FileDecoder
    {

        private static DataProcessor[] m_Processors;

        public DataDecoder()
            : base("Data", ".dat")
        {
        }

        static DataDecoder()
        {
            DataProcessor[] dataProcessorArr = new DataProcessor[] {
                                                                     new AvatarInfoProcessor()};
            DataDecoder.m_Processors = dataProcessorArr;
        }

        public override void FillPanel(ArchivedFile file, Panel pn)
        {
            MemoryStream memoryStream = new MemoryStream();
            file.Download(memoryStream);
            Panel panel = null;
            FileTabSheet fileTabSheet = null;
            for (int i = 0; i < DataDecoder.m_Processors.Length; i++)
            {
                DataProcessor dataProcessor = DataDecoder.m_Processors[i];
                if (dataProcessor.IsMatch(file.FileName))
                {
                    panel = pn;
                    fileTabSheet = new FileTabSheet();
                    pn = fileTabSheet.DecodedPanel;
                    memoryStream = dataProcessor.Mutate(memoryStream);
                    fileTabSheet.SetStream(file.FileName, memoryStream);
                    memoryStream.Seek((long)0, SeekOrigin.Begin);
                    DataStream dataStream = new DirectDataStream(memoryStream);
                    TextWriter textWriter = new StringWriter();
                    dataProcessor.Process(dataStream, textWriter);
                    TextBox textBox = new TextBox();
                    pn.BorderStyle = BorderStyle.None;
                    textBox.Dock = DockStyle.Fill;
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.BackColor = SystemColors.Window;
                    textBox.WordWrap = false;
                    textBox.ScrollBars = ScrollBars.Both;
                    textBox.Text = textWriter.ToString();
                    pn.Controls.Add(textBox);
                    break;
                }
            }
            if (panel != null)
            {
                panel.Controls.Add(fileTabSheet);
            }
            else
            {
                StreamDisplay streamDisplay = new StreamDisplay();
                streamDisplay.Dock = DockStyle.Fill;
                streamDisplay.Stream = memoryStream;
                streamDisplay.Position = 0;
                streamDisplay.BorderStyle = BorderStyle.Fixed3D;
                pn.Controls.Add(streamDisplay);
            }
        }

    } // class DataDecoder
}
