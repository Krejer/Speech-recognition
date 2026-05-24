using System.ComponentModel.Design;
using System.Formats.Tar;
using System.Windows.Forms;

namespace Project3
{
    public enum DisplayMode
    {
        Signal
    }
    public partial class Form1 : Form
    {
        WavFile? wavFile = null;

        private DisplayMode currentMode = DisplayMode.Signal;
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "WAV files (*.wav)|*.wav";
            string wavFilePath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                wavFilePath = ofd.FileName;
            }
            else
                return;
            wavFile = new WavFile(wavFilePath);
            signalButton_Click(sender, e);
        }
        private void signalButton_Click(object sender, EventArgs e)
        {
            currentMode = DisplayMode.Signal;
            UpdatePlot();
        }
        private void UpdatePlot()
        {
            if (wavFile == null) return;

            formsPlot1.Plot.Clear();

            switch (currentMode)
            {
                case DisplayMode.Signal:
                    DrawSignal();
                    break;
            }

            formsPlot1.Refresh();
        }
        private void DrawSignal()
        {
            int numSamples = wavFile.data.Length / wavFile.blockAlign;
            double[] values = new double[numSamples];

            for (int i = 0; i < numSamples; i++)
            {
                int byteIndex = i * wavFile.blockAlign;
                short sample = BitConverter.ToInt16(wavFile.data, byteIndex);
                values[i] = sample;
            }

            var sig = formsPlot1.Plot.Add.Signal(values);
            sig.Data.Period = 1.0 / wavFile.samplePerSecond;

            double totalTime = (double)numSamples / wavFile.samplePerSecond;
            formsPlot1.Plot.Axes.SetLimits(0, totalTime, -32768, 32767);
        }
    }
}
