using NAudio.Wave;
using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Project3
{
    public enum DisplayMode { Signal, DTW }

    public partial class Form1 : Form
    {
        private string? wavPath1 = null;
        private string? wavPath2 = null;

        private WavFile? wavFile1 = null;
        private WavFile? wavFile2 = null;
        private DtwAnalyzer? dtwAnalyzer = null;

        private DisplayMode currentMode = DisplayMode.Signal;

        private TableLayoutPanel? tlpDtw;
        private FormsPlot? plotHeatmap;
        private FormsPlot? plotBottom;
        private FormsPlot? plotLeft;

        private WaveInEvent? waveIn = null;
        private WaveFileWriter? writer = null;
        private bool isRecording = false;
        private string tempRecordFile = "mic_record.wav";

        private Data? data = null;
        private readonly string nagraniaPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Nagrania"));

        public Form1()
        {
            InitializeComponent();
            SetupDtwLayout();
        }

        private void SetupDtwLayout()
        {
            tlpDtw = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Visible = false,
                BackColor = System.Drawing.Color.White
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

            panelPlot.Controls.Add(tlpDtw);
        }

        private void ApplyPreprocessing(WavFile wav)
        {
            if (chkNormalize.Checked) wav.NormalizeAudio();
            if (chkTrimSilence.Checked) wav.TrimSilence(thresholdOffsetDb: 25.0, marginFrames: 15);
            if (chkPreemphasis.Checked) wav.ApplyPreEmphasis();
        }

        private List<double[]> ExtractFeatures(WavFile wav)
        {
            double frameSizeSec = 0.025;
            double shiftSec = 0.010;

            if (rbMFCC.Checked)
                return wav.CalculateMFCC(frameSizeSec, shiftSec);
            else
                return wav.CalculateAllFramesSpectrum(frameSizeSec, shiftSec);
        }

        private DtwMetric GetSelectedMetric()
        {
            return rbCosine.Checked ? DtwMetric.Cosine : DtwMetric.Euclidean;
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (!isRecording) StartRecording();
            else StopRecording();
        }

        private void StartRecording()
        {
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            writer = new WaveFileWriter(tempRecordFile, waveIn.WaveFormat);
            waveIn.DataAvailable += (s, a) => writer.Write(a.Buffer, 0, a.BytesRecorded);

            waveIn.RecordingStopped += (s, a) =>
            {
                writer?.Dispose(); writer = null;
                waveIn?.Dispose(); waveIn = null;

                wavPath1 = tempRecordFile;
                btnShowSignal_Click(s, a);
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
            wavPath1 = SelectWavFilePath();
            if (wavPath1 != null) btnShowSignal_Click(sender, e);
        }

        private void btnLoad2_Click(object sender, EventArgs e)
        {
            wavPath2 = SelectWavFilePath();
            if (wavPath2 != null) btnShowSignal_Click(sender, e);
        }

        private string? SelectWavFilePath()
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "WAV files (*.wav)|*.wav" };
            return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null;
        }

        private void btnShowSignal_Click(object sender, EventArgs e)
        {
            if (wavPath1 != null) { wavFile1 = new WavFile(wavPath1); ApplyPreprocessing(wavFile1); }
            if (wavPath2 != null) { wavFile2 = new WavFile(wavPath2); ApplyPreprocessing(wavFile2); }

            currentMode = DisplayMode.Signal;
            UpdatePlot();
        }

        private void btnShowDTW_Click(object sender, EventArgs e)
        {
            if (wavPath1 == null || wavPath2 == null)
            {
                MessageBox.Show("Najpierw wczytaj oba pliki (Wav 1 i Wav 2)!");
                return;
            }

            wavFile1 = new WavFile(wavPath1);
            wavFile2 = new WavFile(wavPath2);

            ApplyPreprocessing(wavFile1);
            ApplyPreprocessing(wavFile2);

            var frames1 = ExtractFeatures(wavFile1);
            var frames2 = ExtractFeatures(wavFile2);

            dtwAnalyzer = new DtwAnalyzer(frames1, frames2, GetSelectedMetric());
            dtwAnalyzer.CalculateDtw();

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

        private void btnGenerateDB_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Generowanie bazy potrwa chwilę i nadpisze poprzednią.\nUpewnij się, że opcje ekstrakcji (MFCC/FFT) są wybrane poprawnie!\nCzy chcesz kontynuować?", "Aktualizacja Bazy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.No) return;

            var baza = new Data();
            string bazaNagranDir = Path.Combine(nagraniaPath, "Baza_nagran");
            if (!Directory.Exists(bazaNagranDir)) { MessageBox.Show("Folder z bazą nie istnieje!"); return; }

            string[] plikiWav = Directory.GetFiles(bazaNagranDir, "*.wav", SearchOption.AllDirectories);

            foreach (string plik in plikiWav)
            {
                string fileName = Path.GetFileNameWithoutExtension(plik);
                string etykietaSlowa = fileName.Contains("_") ? fileName.Substring(0, fileName.IndexOf('_')) : fileName;

                var wav = new WavFile(plik);
                ApplyPreprocessing(wav); 
                var cechy = ExtractFeatures(wav);

                baza.patterns.Add(new WzorzecNagrania
                {
                    Number = etykietaSlowa,
                    FilePath = plik,
                    FFT = cechy 
                });
            }

            string jsonString = JsonSerializer.Serialize(baza, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(nagraniaPath, "baza"), jsonString);

            data = baza;
            MessageBox.Show("Baza została wygenerowana pomyślnie!", "Gotowe");
        }

        private void LoadData()
        {
            string bazaJsonPath = Path.Combine(nagraniaPath, "baza");
            if (File.Exists(bazaJsonPath))
            {
                string jsonString = File.ReadAllText(bazaJsonPath);
                data = JsonSerializer.Deserialize<Data>(jsonString);
            }
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {
            if (wavPath1 == null)
            {
                MessageBox.Show("Najpierw nagraj lub wczytaj plik 1 do rozpoznania!");
                return;
            }

            if (data == null || data.patterns.Count == 0)
            {
                LoadData();
                if (data == null || data.patterns.Count == 0)
                {
                    MessageBox.Show("Baza jest pusta. Kliknij 'Generate Database' przed klasyfikacją.");
                    return;
                }
            }

            wavFile1 = new WavFile(wavPath1);
            ApplyPreprocessing(wavFile1);
            var cechyTestowe = ExtractFeatures(wavFile1);

            var wyniki = new List<(string Klasa, double Koszt, WzorzecNagrania Wzorzec)>();
            DtwMetric metric = GetSelectedMetric();

            foreach (var wzorzec in data.patterns)
            {
                var dtw = new DtwAnalyzer(cechyTestowe, wzorzec.FFT, metric);
                dtw.CalculateDtw();
                wyniki.Add((wzorzec.Number, dtw.NormalizedDistance, wzorzec));
            }

            int k = 3;
            var topK = wyniki.OrderBy(w => w.Koszt).Take(k).ToList();

            var najlepszaGrupa = topK.GroupBy(w => w.Klasa)
                                     .OrderByDescending(g => g.Count())
                                     .ThenBy(g => g.Min(w => w.Koszt)) 
                                     .First();

            string rozpoznaneSlowo = najlepszaGrupa.Key;
            double minimalnyKoszt = najlepszaGrupa.Min(w => w.Koszt);
            WzorzecNagrania bestMatch = najlepszaGrupa.OrderBy(w => w.Koszt).First().Wzorzec;

            MessageBox.Show($"Rozpoznano słowo: {rozpoznaneSlowo}\nNajmniejszy znormalizowany koszt DTW: {minimalnyKoszt:F4}", "Wynik DTW", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (bestMatch != null && File.Exists(bestMatch.FilePath))
            {
                wavPath2 = bestMatch.FilePath;
                wavFile2 = new WavFile(wavPath2);
                ApplyPreprocessing(wavFile2);

                var cechyWzorca = ExtractFeatures(wavFile2);
                dtwAnalyzer = new DtwAnalyzer(cechyTestowe, cechyWzorca, metric);
                dtwAnalyzer.CalculateDtw();

                currentMode = DisplayMode.DTW;
                UpdatePlot();
            }
        }
    }
}