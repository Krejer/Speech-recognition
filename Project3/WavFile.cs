using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.IntegralTransforms;

namespace Project3
{
    public class WavFile
    {
        public string fileName;
        public string riffChunkId;
        public int riffChunkSize;
        public string riffFormat;
        public string fmtChunkId;
        public int fmtChunkSize;
        public short audioFormat;
        public short numChannels;
        public int samplePerSecond;
        public double timeStep;
        public int avgBytesPerSec;
        public short blockAlign;
        public short bitsPerSample;
        public string dataChunkId;
        public int dataChunkSize;
        public byte[] data;
        public List<short> leftChannel { get; set; } = new List<short>();
        public List<short> rightChannel { get; set; } = new List<short>();
        double frameSize = 0.01;
        public double shift = 0.005;
        int frameSamples;
        int shiftSamples;

        private List<double>? _cachedVolume = null;
        private List<double>? _cachedZCR = null;
        private List<double>? _cachedSTE = null;
        private List<(int start, int end)>? _cachedSilenceFrames = null;
        private List<(int start, int end)>? _cachedVoicedFrames = null;
        private List<(int start, int end)>? _cachedUnvoicedFrames = null;

        public WavFile(string filePath)
        {
            if (filePath == null || !File.Exists(filePath))
            {
                return;
            }
            using (var file = File.Open(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(file))
            {
                fileName = Path.GetFileName(filePath);
                riffChunkId = Encoding.ASCII.GetString(reader.ReadBytes(4));
                riffChunkSize = reader.ReadInt32();
                riffFormat = Encoding.ASCII.GetString(reader.ReadBytes(4));

                if (riffChunkId != "RIFF" || riffFormat != "WAVE")
                {
                    MessageBox.Show("Not a correct WAV file.");
                    return;
                }

                bool fmtFound = false;
                bool dataFound = false;

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    string chunkId = Encoding.ASCII.GetString(reader.ReadBytes(4));
                    int chunkSize = reader.ReadInt32();

                    if (chunkId == "fmt ")
                    {
                        fmtChunkId = chunkId;
                        fmtChunkSize = chunkSize;
                        audioFormat = reader.ReadInt16();
                        numChannels = reader.ReadInt16();
                        samplePerSecond = reader.ReadInt32();
                        avgBytesPerSec = reader.ReadInt32();
                        blockAlign = reader.ReadInt16();
                        bitsPerSample = reader.ReadInt16();
                        timeStep = 1.0 / samplePerSecond;

                        int extraBytes = chunkSize - 16;
                        if (extraBytes > 0)
                        {
                            reader.BaseStream.Seek(extraBytes, SeekOrigin.Current);
                        }
                        fmtFound = true;
                    }
                    else if (chunkId == "data")
                    {
                        dataChunkId = chunkId;
                        dataChunkSize = chunkSize;
                        data = reader.ReadBytes(chunkSize);
                        dataFound = true;
                        break;
                    }
                    else
                    {
                        reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
                    }
                }

                if (!fmtFound || !dataFound)
                {
                    MessageBox.Show("Error");
                    return;
                }
                if (bitsPerSample != 16)
                {
                    MessageBox.Show("Error");
                    return;
                }

                int bytesPerSample = bitsPerSample / 8;
                for (int i = 0; i < data.Length; i += blockAlign)
                {
                    short leftSample = BitConverter.ToInt16(data, i);
                    leftChannel.Add(leftSample);

                    if (numChannels == 2)
                    {
                        short rightSample = BitConverter.ToInt16(data, i + bytesPerSample);
                        rightChannel.Add(rightSample);
                    }
                    else
                    {
                        rightChannel.Add(leftSample);
                    }
                }
                frameSamples = (int)(frameSize * samplePerSecond);
                shiftSamples = (int)(shift * samplePerSecond);
            }
        }

        public List<double> CalculateVolume()
        {
            List<double> res = new List<double>();
            for (int i = 0; i < leftChannel.Count; i += shiftSamples)
            {
                double startTime = (double)i / samplePerSecond;
                double endTime = (double)(i + shiftSamples) / samplePerSecond;

                var spectrum = CalculateSpectrum(startTime, endTime);
                double temporaryRes = 0;
                for (int j = 0; j < spectrum.Count; j++)
                {
                    temporaryRes += spectrum[j].linearMag * spectrum[j].linearMag;
                }
                var energy = temporaryRes / spectrum.Count;
                double volumeDb = 10 * Math.Log10(energy > 0 ? energy : 1e-10);
                res.Add(volumeDb);
            }
            return res;
        }

        public void TrimSilence(double thresholdOffsetDb = 25.0, int marginFrames = 15)
        {
            var volumes = CalculateVolume();
            if (volumes.Count == 0) return;

            double maxVol = volumes.Max();
            double threshold = maxVol - thresholdOffsetDb;

            int startFrame = 0;
            while (startFrame < volumes.Count && volumes[startFrame] < threshold)
                startFrame++;

            int endFrame = volumes.Count - 1;
            while (endFrame >= 0 && volumes[endFrame] < threshold)
                endFrame--;

            if (startFrame >= endFrame) return;

            startFrame = Math.Max(0, startFrame - marginFrames);
            endFrame = Math.Min(volumes.Count - 1, endFrame + marginFrames);

            int startSample = startFrame * shiftSamples;
            int endSample = Math.Min(leftChannel.Count, (endFrame * shiftSamples) + frameSamples);

            if (startSample < endSample)
            {
                leftChannel = leftChannel.GetRange(startSample, endSample - startSample);
                if (rightChannel.Count > 0)
                {
                    rightChannel = rightChannel.GetRange(startSample, endSample - startSample);
                }
            }
        }

        public void ApplyPreEmphasis(double alpha = 0.97)
        {
            if (leftChannel.Count < 2) return;

            for (int i = leftChannel.Count - 1; i > 0; i--)
            {
                leftChannel[i] = (short)(leftChannel[i] - alpha * leftChannel[i - 1]);
            }

            if (rightChannel.Count > 1)
            {
                for (int i = rightChannel.Count - 1; i > 0; i--)
                {
                    rightChannel[i] = (short)(rightChannel[i] - alpha * rightChannel[i - 1]);
                }
            }
        }

        public List<(double freq, double mag, double linearMag)> CalculateSpectrum(double startTime, double endTime)
        {
            int startSample = Math.Max(0, (int)(startTime * samplePerSecond));
            int endSample = Math.Min(leftChannel.Count, (int)(endTime * samplePerSecond));

            int frameLength = endSample - startSample;
            if (frameLength <= 0) return new List<(double, double, double)>();

            int fftSize = (int)Math.Pow(2, Math.Ceiling(Math.Log(frameLength, 2)));
            if (fftSize < 2) fftSize = 2;

            Complex[] complexSignal = new Complex[fftSize];

            double[] window = MathNet.Numerics.Window.Hann(fftSize);

            for (int i = 0; i < fftSize; i++)
            {
                if (i < frameLength)
                {
                    double rawSample = leftChannel[startSample + i] / 32768.0;
                    double windowedSample = rawSample * window[i];
                    complexSignal[i] = new Complex(windowedSample, 0);
                }
                else
                {
                    complexSignal[i] = new Complex(0, 0);
                }
            }

            Fourier.Forward(complexSignal, FourierOptions.Matlab);

            List<(double Frequency, double Magnitude, double linearMag)> spectrum = new List<(double, double, double)>();

            for (int i = 0; i < fftSize / 2; i++)
            {
                double freq = (double)i * samplePerSecond / fftSize;
                double mag = complexSignal[i].Magnitude / (fftSize / 2.0);
                double magDb = 20 * Math.Log10(mag > 0 ? mag : 1e-10);

                spectrum.Add((freq, magDb, mag));
            }

            return spectrum;
        }

        public List<double[]> CalculateAllFramesSpectrum(double? customFrameSize = null, double? customShift = null)
        {
            double activeFrameSize = customFrameSize ?? this.frameSize;
            double activeShift = customShift ?? this.shift;

            int activeFrameSamples = (int)(activeFrameSize * samplePerSecond);
            int activeShiftSamples = (int)(activeShift * samplePerSecond);

            List<double[]> allSpectra = new List<double[]>();

            for (int i = 0; i <= leftChannel.Count - activeFrameSamples; i += activeShiftSamples)
            {
                double startTime = (double)i / samplePerSecond;
                double endTime = startTime + activeFrameSize;

                var spectrum = CalculateSpectrum(startTime, endTime);

                if (spectrum.Count == 0) continue;

                double[] magnitudes = spectrum.Select(s => s.linearMag).ToArray();
                allSpectra.Add(magnitudes);
            }

            return allSpectra;
        }

        public List<double[]> CalculateMFCC(double? customFrameSize = null, double? customShift = null)
        {
            var spectra = CalculateAllFramesSpectrum(customFrameSize, customShift);
            
            int numFilters = 26;
            int numCoeffs = 13;
            double minFreq = 0;
            double maxFreq = 8000.0;
            
            double minMel = 2595.0 * Math.Log10(1.0 + minFreq / 700.0);
            double maxMel = 2595.0 * Math.Log10(1.0 + maxFreq / 700.0);
            
            double[] melPoints = new double[numFilters + 2];
            for (int i = 0; i < melPoints.Length; i++)
            {
                melPoints[i] = minMel + i * (maxMel - minMel) / (numFilters + 1);
            }
            
            double[] hzPoints = new double[numFilters + 2];
            for (int i = 0; i < melPoints.Length; i++)
            {
                hzPoints[i] = 700.0 * (Math.Pow(10, melPoints[i] / 2595.0) - 1.0);
            }
            
            double activeFrameSize = customFrameSize ?? this.frameSize;
            int activeFrameSamples = (int)(activeFrameSize * samplePerSecond);
            int fftSize = (int)Math.Pow(2, Math.Ceiling(Math.Log(activeFrameSamples, 2)));
            if (fftSize < 2) fftSize = 2;
            int numBins = fftSize / 2;
            
            int[] binPoints = new int[numFilters + 2];
            for (int i = 0; i < hzPoints.Length; i++)
            {
                binPoints[i] = (int)Math.Floor((fftSize + 1) * hzPoints[i] / samplePerSecond);
                if (binPoints[i] >= numBins) binPoints[i] = numBins - 1;
            }
            
            List<double[]> mfccFrames = new List<double[]>();
            
            foreach (var spectrumMag in spectra)
            {
                double[] filterbankEnergies = new double[numFilters];
                
                for (int i = 1; i <= numFilters; i++)
                {
                    double energy = 0;
                    for (int j = binPoints[i - 1]; j < binPoints[i]; j++)
                    {
                        double weight = (j - binPoints[i - 1]) / (double)(Math.Max(1, binPoints[i] - binPoints[i - 1]));
                        energy += weight * Math.Pow(spectrumMag[j], 2);
                    }
                    for (int j = binPoints[i]; j <= binPoints[i + 1]; j++)
                    {
                        double weight = (binPoints[i + 1] - j) / (double)(Math.Max(1, binPoints[i + 1] - binPoints[i]));
                        energy += weight * Math.Pow(spectrumMag[j], 2);
                    }
                    filterbankEnergies[i - 1] = energy;
                }
                
                double[] logEnergies = new double[numFilters];
                for (int i = 0; i < numFilters; i++)
                {
                    logEnergies[i] = Math.Log(filterbankEnergies[i] < 1e-10 ? 1e-10 : filterbankEnergies[i]);
                }
                
                double[] mfcc = new double[numCoeffs];
                for (int i = 0; i < numCoeffs; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < numFilters; j++)
                    {
                        sum += logEnergies[j] * Math.Cos(Math.PI * i / numFilters * (j + 0.5));
                    }
                    mfcc[i] = sum;
                }
                mfccFrames.Add(mfcc);
            }
            
            if (mfccFrames.Count > 0)
            {
                double[] means = new double[numCoeffs];
                foreach (var frame in mfccFrames)
                {
                    for (int i = 0; i < numCoeffs; i++) means[i] += frame[i];
                }
                for (int i = 0; i < numCoeffs; i++) means[i] /= mfccFrames.Count;
                
                foreach (var frame in mfccFrames)
                {
                    for (int i = 0; i < numCoeffs; i++) frame[i] -= means[i];
                }
            }
            
            return mfccFrames;
        }

        public void NormalizeAudio()
        {
            if (leftChannel.Count == 0) return;

            int maxAmp = 0;
            foreach (short sample in leftChannel)
            {
                int absSample = Math.Abs((int)sample); 
                if (absSample > maxAmp)
                {
                    maxAmp = absSample;
                }
            }

            if (maxAmp == 0 || maxAmp >= 32767) return;

            double ratio = 32767.0 / maxAmp;

            for (int i = 0; i < leftChannel.Count; i++)
            {
                leftChannel[i] = (short)(leftChannel[i] * ratio);
            }

            if (rightChannel.Count > 0)
            {
                maxAmp = 0;
                foreach (short sample in rightChannel)
                {
                    int absSample = Math.Abs((int)sample);
                    if (absSample > maxAmp) maxAmp = absSample;
                }

                if (maxAmp > 0 && maxAmp < 32767)
                {
                    ratio = 32767.0 / maxAmp;
                    for (int i = 0; i < rightChannel.Count; i++)
                    {
                        rightChannel[i] = (short)(rightChannel[i] * ratio);
                    }
                }
            }
        }
    }
}
