using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    MessageBox.Show("Wybrany plik nie jest prawidłowym plikiem WAV.");
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
                    MessageBox.Show("Nie znaleziono wymaganych sekcji (fmt/data). Plik może być uszkodzony.");
                    return;
                }
                if (bitsPerSample != 16)
                {
                    MessageBox.Show("Ten program obsługuje tylko pliki 16-bitowe.");
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
    }
}
