using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly LAMEPreset _quality;
        private readonly List<string> _fileNames;
        private int _threadCount;
        private readonly object _syncRoot = new object();

        public event EventHandler Done;
        public event ConvertProgressEventHandler Progress;

        public Converter(IEnumerable<string> fileNames, LAMEPreset quality)
        {
            if (fileNames == null)
                throw new ArgumentNullException(nameof(fileNames));
            _quality = quality;

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

                    OnProgress(new ConvertProgressEventArgs(fileName, 1, true, null));
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
            using (var writer = new LameMP3FileWriter(target, reader.WaveFormat, _quality))
            {
                long length = reader.Length;
                writer.OnProgress += (sender, inputBytes, outputBytes, finished) =>
                    OnProgress(new ConvertProgressEventArgs(fileName, (float)inputBytes / length, false, null));

                reader.CopyTo(writer);
            }
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
        public Exception Exception { get; }

        public ConvertProgressEventArgs(string fileName, float progress, bool done, Exception exception)
        {
            FileName = fileName;
            Progress = progress;
            Done = done;
            Exception = exception;
        }
    }

    public delegate void ConvertProgressEventHandler(object sender, ConvertProgressEventArgs e);
}
