using NAudio.Wave;
using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;

namespace Project3
{
    public enum DisplayMode
    {
        Signal,
        DTW
    }

    public partial class Form1 : Form
    {
        WavFile? wavFile1 = null;
        WavFile? wavFile2 = null;
        DtwAnalyzer? dtwAnalyzer = null;

        private DisplayMode currentMode = DisplayMode.Signal;

        private TableLayoutPanel? tlpDtw;
        private FormsPlot? plotHeatmap;
        private FormsPlot? plotBottom;
        private FormsPlot? plotLeft;

        private WaveInEvent? waveIn = null;
        private WaveFileWriter? writer = null;
        private bool isRecording = false;
        private string tempRecordFile = "mic_record.wav";

        public Form1()
        {
            InitializeComponent();
            SetupDtwLayout();
        }

        private void SetupDtwLayout()
        {
            tlpDtw = new TableLayoutPanel
            {
                Location = new Point(233, 24),
                Size = new Size(1118, 852),
                ColumnCount = 2,
                RowCount = 2,
                Visible = false
            };

            tlpDtw.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpDtw.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tlpDtw.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tlpDtw.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

            plotHeatmap = new FormsPlot { Dock = DockStyle.Fill };
            plotBottom = new FormsPlot { Dock = DockStyle.Fill };
            plotLeft = new FormsPlot { Dock = DockStyle.Fill };

            tlpDtw.Controls.Add(plotLeft, 0, 0);
            tlpDtw.Controls.Add(plotHeatmap, 1, 0);
            tlpDtw.Controls.Add(plotBottom, 1, 1);

            Controls.Add(tlpDtw);
        }
        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }

        private void StartRecording()
        {
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);

            writer = new WaveFileWriter(tempRecordFile, waveIn.WaveFormat);

            waveIn.DataAvailable += (s, a) =>
            {
                writer.Write(a.Buffer, 0, a.BytesRecorded);
            };

            waveIn.RecordingStopped += (s, a) =>
            {
                writer?.Dispose();
                writer = null;
                waveIn?.Dispose();
                waveIn = null;

                wavFile1 = new WavFile(tempRecordFile);
                ResetAnalyzer();
                UpdatePlot();
            };

            waveIn.StartRecording();
            isRecording = true;
            btnRecord.Text = "🔴 Stop Rec";
            btnRecord.ForeColor = System.Drawing.Color.Red;
        }

        private void StopRecording()
        {
            waveIn?.StopRecording();
            isRecording = false;
            btnRecord.Text = "🎙️ Record Mic";
            btnRecord.ForeColor = System.Drawing.Color.Black;
        }

        private void btnLoad1_Click(object sender, EventArgs e)
        {
            wavFile1 = LoadWavFile();
            ResetAnalyzer();
            currentMode = DisplayMode.Signal;
            UpdatePlot();
        }

        private void btnLoad2_Click(object sender, EventArgs e)
        {
            wavFile2 = LoadWavFile();
            ResetAnalyzer();
            currentMode = DisplayMode.Signal;
            UpdatePlot();
        }

        private WavFile? LoadWavFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "WAV files (*.wav)|*.wav";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return new WavFile(ofd.FileName);
            }
            return null;
        }

        private void ResetAnalyzer()
        {
            dtwAnalyzer = null;
        }

        private void btnShowSignal_Click(object sender, EventArgs e)
        {
            currentMode = DisplayMode.Signal;
            UpdatePlot();
        }

        private void btnShowDTW_Click(object sender, EventArgs e)
        {
            if (wavFile1 == null || wavFile2 == null)
            {
                MessageBox.Show("Missing WAV files");
                return;
            }

            if (dtwAnalyzer == null)
            {

                double frameSizeSec = 256.0 / wavFile1.samplePerSecond;
                double shiftSec = 128.0 / wavFile1.samplePerSecond;

                var frames1 = wavFile1.CalculateAllFramesSpectrum(frameSizeSec, shiftSec);
                var frames2 = wavFile2.CalculateAllFramesSpectrum(frameSizeSec, shiftSec);

                dtwAnalyzer = new DtwAnalyzer(frames1, frames2);
                dtwAnalyzer.CalculateDtw();

                System.Diagnostics.Debug.WriteLine($"DTW cost: {dtwAnalyzer.TotalDistance}");
            }

            currentMode = DisplayMode.DTW;
            UpdatePlot();
        }

        private void UpdatePlot()
        {
            formsPlot1.Visible = false;
            if (tlpDtw != null) tlpDtw.Visible = false;

            switch (currentMode)
            {
                case DisplayMode.Signal:
                    formsPlot1.Visible = true;
                    DrawSignals();
                    break;
                case DisplayMode.DTW:
                    if (tlpDtw != null) tlpDtw.Visible = true;
                    DrawDtwLayout();
                    break;
            }
        }

        private void DrawSignals()
        {
            formsPlot1.Plot.Clear();

            if (wavFile1 != null)
            {
                var sig1 = formsPlot1.Plot.Add.Signal(wavFile1.leftChannel.Select(s => (double)s).ToArray());
                sig1.Data.Period = 1.0 / wavFile1.samplePerSecond;
            }

            if (wavFile2 != null)
            {
                var sig2 = formsPlot1.Plot.Add.Signal(wavFile2.leftChannel.Select(s => (double)s).ToArray());
                sig2.Data.Period = 1.0 / wavFile2.samplePerSecond;
                sig2.Data.YOffset = 60000;
            }

            formsPlot1.Plot.Axes.AutoScale();
            formsPlot1.Refresh();
        }

        private void DrawDtwLayout()
        {
            if (dtwAnalyzer?.LocalDistanceMatrix == null || plotHeatmap == null || plotBottom == null || plotLeft == null) return;

            plotHeatmap.Plot.Clear();
            plotBottom.Plot.Clear();
            plotLeft.Plot.Clear();

            int samplesX = wavFile1!.leftChannel.Count;
            int samplesY = wavFile2!.leftChannel.Count;

            var hm = plotHeatmap.Plot.Add.Heatmap(dtwAnalyzer.LocalDistanceMatrix);
            hm.Colormap = new ScottPlot.Colormaps.Grayscale().Reversed();

            hm.Extent = new ScottPlot.CoordinateRect(0, samplesX, 0, samplesY);

            if (dtwAnalyzer.OptimalPathX != null && dtwAnalyzer.OptimalPathY != null)
            {
                double scaleX = (double)samplesX / dtwAnalyzer.FramesX.Count;
                double scaleY = (double)samplesY / dtwAnalyzer.FramesY.Count;

                double[] scaledPathX = dtwAnalyzer.OptimalPathX.Select(x => x * scaleX).ToArray();
                double[] scaledPathY = dtwAnalyzer.OptimalPathY.Select(y => y * scaleY).ToArray();

                var pathLine = plotHeatmap.Plot.Add.ScatterLine(scaledPathX, scaledPathY);
                pathLine.Color = ScottPlot.Colors.White;
                pathLine.LineWidth = 3;
            }

            plotHeatmap.Plot.Axes.Margins(0, 0);

            double[] sigX = wavFile1.leftChannel.Select(s => (double)s).ToArray();
            var sigBottom = plotBottom.Plot.Add.Signal(sigX);
            plotBottom.Plot.Axes.Margins(0, 0);

            double[] sigYData = wavFile2.leftChannel.Select(s => (double)s).ToArray();
            double[] ys = Enumerable.Range(0, sigYData.Length).Select(i => (double)i).ToArray();
            var sigLeft = plotLeft.Plot.Add.ScatterLine(sigYData, ys);
            plotLeft.Plot.Axes.Margins(0, 0);

            plotBottom.Plot.Axes.Bottom.TickLabelStyle.IsVisible = false;
            plotLeft.Plot.Axes.Left.TickLabelStyle.IsVisible = false;

            plotHeatmap.Refresh();
            plotBottom.Refresh();
            plotLeft.Refresh();
        }

        private void buttonNorm_Click(object sender, EventArgs e)
        {
            if (wavFile1 == null || wavFile2 == null)
            {
                MessageBox.Show("Missing WAV files");
                return;
            }
            wavFile1.NormalizeAudio();
            wavFile2.NormalizeAudio();
            UpdatePlot();
        }

        private void buttonTrim_Click(object sender, EventArgs e)
        {
            if (wavFile1 == null || wavFile2 == null)
            {
                MessageBox.Show("Missing WAV files");
                return;
            }
            wavFile1.TrimSilence();
            wavFile2.TrimSilence();
            UpdatePlot();
        }
    }
}