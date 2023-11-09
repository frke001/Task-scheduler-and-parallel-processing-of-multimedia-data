using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    internal class WaveformAudioFile
    {
        private int riffTag;
        private int fileSize;
        private int waveTag;
        private int fmtTag;
        private int fmtChunkSize;
        private short audioFormat;
        private short numChannels;
        public int sampleRate;
        private int byteRate;
        private short blockAlign;
        private short bitsPerSample;
        private int dataHeader;
        private int dataSize;
        public byte[] data;

        public WaveformAudioFile()
        {

        }

        public static WaveformAudioFile ReadFile(string path)
        {
            WaveformAudioFile waveFile = new WaveformAudioFile();
            if (!path.EndsWith(".wav"))
                throw new ArgumentException("Invalid file type!");
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist!");

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {

                    waveFile.riffTag = reader.ReadInt32(); 
                    
                    if (waveFile.riffTag != 0x46464952)
                        throw new ArgumentException("Not a WAV file");

                    // Reading file size
                    waveFile.fileSize = reader.ReadInt32();

                    // Reading the "WAVE" tag
                    waveFile.waveTag = reader.ReadInt32();

                    // Checking if it is a WAV file
                    if (waveFile.waveTag != 0x45564157)
                        throw new ArgumentException("Not a WAV file");

                    // Reading "fmt " tag
                    waveFile.fmtTag = reader.ReadInt32();

                    // Reading the size of the "fmt " chunk
                    waveFile.fmtChunkSize = reader.ReadInt32();

                    // Reading audio formats
                    waveFile.audioFormat = reader.ReadInt16();

                    // Checking if it is a PCM format
                    if (waveFile.audioFormat != 1)
                        throw new ArgumentException("Not a PCM WAV file");

                    // Reading the channel number
                    waveFile.numChannels = reader.ReadInt16();

                    // Checking if the audio track is mono
                    if (waveFile.numChannels != 1)
                        throw new ArgumentException("Not a mono WAV file");

                    // Reading the sampling frequency
                    waveFile.sampleRate = reader.ReadInt32();

                    // Read data rate
                    waveFile.byteRate = reader.ReadInt32();

                    // Read block size (number of bytes per channel)
                    waveFile.blockAlign = reader.ReadInt16();

                    // Reading the sample size
                    waveFile.bitsPerSample = reader.ReadInt16();

                    // Checking if samples are 16-bit
                    if (waveFile.bitsPerSample != 16)
                        throw new ArgumentException("Not a 16-bit WAV file");

                    waveFile.dataHeader = reader.ReadInt32();
                    waveFile.dataSize = reader.ReadInt32();

                    waveFile.data = reader.ReadBytes(waveFile.dataSize);

                }
            }
            return waveFile;
        }
        public static bool WriteFile(string path, WaveformAudioFile waveFile)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(fileStream))
                {
                    // Writing the header
                    writer.Write(waveFile.riffTag);
                    writer.Write(waveFile.fileSize);
                    writer.Write(waveFile.waveTag);
                    writer.Write(waveFile.fmtTag);
                    writer.Write(waveFile.fmtChunkSize);
                    writer.Write(waveFile.audioFormat);
                    writer.Write(waveFile.numChannels);
                    writer.Write(waveFile.sampleRate);
                    writer.Write(waveFile.byteRate);
                    writer.Write(waveFile.blockAlign);
                    writer.Write(waveFile.bitsPerSample);
                    writer.Write(waveFile.dataHeader);
                    writer.Write(waveFile.dataSize);

                    // Writing audio data
                    writer.Write(waveFile.data);
                    return true;
                }
            }
            return false; 
        }
    }
}
