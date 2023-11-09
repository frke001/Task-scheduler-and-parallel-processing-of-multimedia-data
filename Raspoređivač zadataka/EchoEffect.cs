using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspoređivač_zadataka
{
    public class EchoEffect : IAction
    {
        public int Delay { get; set; } = 500;
        public float Decay { get; set; } = 0.8f;
        public string InputPathName { get; set; }
        public string OutputPath { get; set; }
        
        //public List<string> AudioPaths { get; set; }

        public EchoEffect()
        {

        }
        public EchoEffect(int delay, float decay, string outputPath,/* List<string> audioPaths,*/ string inputPathName)
        {
            this.Delay = delay;
            this.Decay = decay;
            this.OutputPath = outputPath;
            //this.AudioPaths = audioPaths;
            this.InputPathName = inputPathName;
        }

        public void Run(ICoOperative taskApi)
        {
            
            List<string> AudioPaths = Directory.GetFiles(InputPathName).ToList();
            ParallelOptions parallelOptions = new ParallelOptions();
            // setting the maximum number of cores that will be used to execute parallelized code
            parallelOptions.MaxDegreeOfParallelism = taskApi.GetMaxDegreeOfParallelism();
            // f MaxDegreeOfParallelism = 4, at most 4 iterations will be executed in parallel
            WaveformAudioFile waveFile;
            if (AudioPaths.Count != 1)
            {
                int progress = 0;
                Parallel.For(0, AudioPaths.Count, parallelOptions, (it) =>
                {
                    // we parallelize for a list of files
                    taskApi.SetProgress(progress++ / (float)(AudioPaths.Count));
                    try
                    {
                        waveFile = WaveformAudioFile.ReadFile(AudioPaths[it]);
                        byte[] audioData = waveFile.data; // data from audio recordings
                        short[] audioDataInShorts = new short[audioData.Length / 2]; // samples
                        short[] echoedData = new short[audioDataInShorts.Length]; // field for echo data
                        byte[] echoedDataInBytes;

                        // conversion of a byte field into a short size field
                        for (int i = 0; i < audioData.Length; i += 2)
                        {
                            audioDataInShorts[i / 2] = BitConverter.ToInt16(audioData, i); 
                            taskApi.CheckForPause();
                            taskApi.CheckForTermination();
                            taskApi.CheckForContextSwitch();
                        }

                        // echo generation
                        for (int i = 0; i < audioDataInShorts.Length; i++)
                        {                                                  
                            int sourceIndex = i - (Delay * waveFile.sampleRate / 1000);
                            if (sourceIndex < 0) continue;
                            audioDataInShorts[i] = (short)(audioDataInShorts[i] * 0.5);
                            echoedData[i] = (short)(audioDataInShorts[i] + audioDataInShorts[sourceIndex] * Decay);
                            taskApi.CheckForPause();
                            taskApi.CheckForTermination();
                            taskApi.CheckForContextSwitch();
                        }

                        // conversion of samples to bytes
                        echoedDataInBytes = new byte[echoedData.Length * 2];

                        Buffer.BlockCopy(echoedData, 0, echoedDataInBytes, 0, echoedDataInBytes.Length);
                        waveFile.data = echoedDataInBytes;
                        taskApi.CheckForPause();
                        taskApi.CheckForTermination();
                        taskApi.CheckForContextSwitch();
                        // audio.wav => audioEcho.wav
                        string storePath = Path.Combine(OutputPath, Path.GetFileNameWithoutExtension(AudioPaths[it]) + "Echo.wav");
                        Console.WriteLine(storePath);
                        WaveformAudioFile.WriteFile(storePath, waveFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    taskApi.SetProgress(progress / (float)(AudioPaths.Count));
                });

            }
            else
            {   // file/algorithm parallelization
                try
                {
                    waveFile = WaveformAudioFile.ReadFile(AudioPaths[0]);
                    byte[] audioData = waveFile.data; // data from an audio track in a byte field
                    short[] audioDataInShorts = new short[audioData.Length / 2]; // field of short samples
                    short[] echoedData = new short[audioDataInShorts.Length]; // deferred data field
                    byte[] echoedDataInBytes = new byte[echoedData.Length * 2]; // byte array for delayed data
                    Parallel.For(0, 1, parallelOptions, (it) =>
                    {
                        // conversion of a byte field into a short size field
                        for (int i = 0; i < audioData.Length; i += 2)
                        {
                            audioDataInShorts[i / 2] = BitConverter.ToInt16(audioData, i);
                        }
                        int pr = 0;
                        // generating a delayed signal
                        for (int i = 0; i < audioDataInShorts.Length; i++)
                        {
                            taskApi.CheckForPause();
                            taskApi.CheckForTermination();
                            taskApi.CheckForContextSwitch();
                            taskApi.SetProgress(pr++ / (float)(audioDataInShorts.Length));
                            int sourceIndex = i - (Delay * waveFile.sampleRate / 1000);
                            if (sourceIndex < 0) continue;
                            audioDataInShorts[i] = (short)(audioDataInShorts[i] * 0.2);
                            echoedData[i] = (short)(audioDataInShorts[i] + audioDataInShorts[sourceIndex] * Decay);
                        }
                        Buffer.BlockCopy(echoedData, 0, echoedDataInBytes, 0, echoedDataInBytes.Length);
                        taskApi.CheckForPause();
                        taskApi.CheckForTermination();
                        taskApi.CheckForContextSwitch();
                        taskApi.SetProgress(pr / (float)(audioDataInShorts.Length));
                    });
                    waveFile.data = echoedDataInBytes;
                    string storePath = Path.Combine(OutputPath, Path.GetFileNameWithoutExtension(AudioPaths[0]) + "Echo.wav");
                    WaveformAudioFile.WriteFile(storePath, waveFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
