using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Lame;
using NAudio.Wave;

namespace ConvertAllToMp3
{
    public class Converter
    {
        private List<string> _fileNames;
        private int _threadCount;
        private readonly object _syncRoot = new object();

        public event EventHandler Done;
        public event ConvertProgressEventHandler Progress;

        public Converter(IEnumerable<string> fileNames)
        {
            _fileNames = new List<string>(fileNames);
#if DEBUG
            _threadCount = 1;
#else
            _threadCount = Environment.ProcessorCount;
#endif

            var threads = new List<Thread>();

            for (int i = 0; i < _threadCount; i++)
            {
                threads.Add(new Thread(ThreadProc) { IsBackground = true });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        private void ThreadProc()
        {
            while (true)
            {
                string fileName;

                lock (_syncRoot)
                {
                    if (_fileNames.Count == 0)
                    {
                        _threadCount--;
                        if (_threadCount == 0)
                            OnDone();

                        return;
                    }

                    fileName = _fileNames[0];
                    _fileNames.RemoveAt(0);
                }

                try
                {
                    DoConvert(fileName);
                }
                catch (Exception ex)
                {
                    OnProgress(new ConvertProgressEventArgs(fileName, 1, true, ex));
                }
            }
        }

        private void DoConvert(string fileName)
        {
            string target = Path.ChangeExtension(fileName, ".mp3");

            using (var reader = new AudioFileReader(fileName))
            using (var writer = new LameMP3FileWriter(target, reader.WaveFormat, LAMEPreset.V2))
            {
                long length = reader.Length;
                long totalRead = 0;
                var buffer = new byte[8192];
                int read;

                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writer.Write(buffer, 0, read);

                    totalRead += read;

                    float progress = (float)totalRead / length;

                    OnProgress(new ConvertProgressEventArgs(fileName, progress, false, null));
                }
            }

            File.Delete(fileName);

            OnProgress(new ConvertProgressEventArgs(fileName, 1, true, null));
        }

        protected virtual void OnDone()
        {
            Done?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnProgress(ConvertProgressEventArgs e)
        {
            Progress?.Invoke(this, e);
        }
    }

    public class ConvertProgressEventArgs : EventArgs
    {
        public string FileName { get; }
        public float Progress { get; }
        public bool Done { get; }
        public Exception Excetion { get; }

        public ConvertProgressEventArgs(string fileName, float progress, bool done, Exception excetion)
        {
            FileName = fileName;
            Progress = progress;
            Done = done;
            Excetion = excetion;
        }
    }

    public delegate void ConvertProgressEventHandler(object sender, ConvertProgressEventArgs e);
}
