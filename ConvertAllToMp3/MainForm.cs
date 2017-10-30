using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using SearchOption = System.IO.SearchOption;

namespace ConvertAllToMp3
{
    public partial class MainForm : SystemEx.Windows.Forms.Form
    {
        private readonly string[] _args;

        public MainForm(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            _args = args;

            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (_args.Length != 1)
            {
                TaskDialogEx.Error(this, "No path provided on the command line.");
                Dispose();
                return;
            }

            string path = _args[0];

            string configFileName = Path.Combine(path, ".converttomp3.conf");
            Config config = null;

            if (File.Exists(configFileName))
            {
                try
                {
                    config = Config.Load(configFileName);
                }
                catch (Exception ex)
                {
                    TaskDialogEx.Error(this, "Could not load configuration file.", ex.Message);
                    Dispose();
                    return;
                }
            }

            StartConversion(path, config);
        }

        private void StartConversion(string path, Config config)
        {
            path = Path.GetFullPath(path);
            if (path[path.Length - 1] == Path.DirectorySeparatorChar)
                path = path.Substring(0, path.Length - 1);

            var files = new List<string>();

            ProgressForm.Show(this, Text, progress =>
            {
                progress.SetLabel("Finding files to convert");
                progress.SetProgress(-1);

                foreach (string fileName in Directory.GetFiles(path, "*.wav", SearchOption.AllDirectories))
                {
                    if (!fileName.StartsWith(path, StringComparison.OrdinalIgnoreCase))
                        throw new Exception("Expected found path to start with prefix");
                    string subFileName = fileName.Substring(path.Length);

                    if (config == null || config.Include(subFileName))
                        files.Add(fileName);
                }
            });

            if (files.Count == 0)
            {
                TaskDialogEx.Error(this, "Found no files to convert.", icon: TaskDialogIcon.Information);
                Dispose();
                return;
            }

            var controls = QueueFiles(files);

            var result = TaskDialogEx.Confirm(this, $"Found {files.Count} to convert. Do you want to start the conversion?", icon: TaskDialogIcon.Information);
            if (result != DialogResult.Yes)
            {
                Dispose();
                return;
            }

            StartConversion(files, controls);
        }

        private Dictionary<string, FileControl> QueueFiles(IEnumerable<string> files)
        {
            var controls = new Dictionary<string, FileControl>();

            _filesPanel.SuspendLayout();

            foreach (string file in files.Reverse())
            {
                var control = new FileControl
                {
                    FileName = file,
                    Dock = DockStyle.Top
                };

                _filesPanel.Controls.Add(control);

                controls.Add(file, control);
            }

            _filesPanel.ResumeLayout();

            return controls;
        }

        private void StartConversion(IEnumerable<string> fileNames, Dictionary<string, FileControl> controls)
        {
            var converter = new Converter(fileNames);

            var converted = new List<string>();

            converter.Done += (s, e) => ConversionCompleted(converted);
            converter.Progress += (s, e) => SetProgress(controls, converted, e);
        }

        private void ConversionCompleted(List<string> converted)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ConversionCompleted(converted)));
                return;
            }

            if (converted.Count > 0)
            {
                var taskDialog = TaskDialogEx.CreateDialog("Do you want to deleted the converted audio files?", $"{converted.Count} files were converted successfully.", TaskDialogIcon.Information);

                taskDialog.CommonButtons = TaskDialogCommonButtons.Cancel;
                taskDialog.AllowDialogCancellation = true;
                taskDialog.Buttons = new[]
                {
                    new TaskDialogButton
                    {
                        ButtonText = "Delete to Recycle Bin",
                        ButtonId = 1
                    },
                    new TaskDialogButton
                    {
                        ButtonText = "Delete Permanently",
                        ButtonId = 2
                    }
                };
                taskDialog.UseCommandLinks = true;

                var result = taskDialog.Show(this);

                switch (result)
                {
                    case 1:
                        DeleteConverted(converted, true);
                        break;
                    case 2:
                        DeleteConverted(converted, false);
                        break;
                }
            }

            Dispose();
        }

        private void DeleteConverted(List<string> converted, bool allowUndo)
        {
            ShellFileOperation.Delete(this, converted, allowUndo, false, true);
        }

        private void SetProgress(Dictionary<string, FileControl> controls, List<string> converted, ConvertProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetProgress(controls, converted, e)));
                return;
            }

            if (e.Done && e.Exception == null)
                converted.Add(e.FileName);

            var control = controls[e.FileName];

            if (e.Done)
                control.Dispose();
            else
                control.Progress = e.Progress;
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = TaskDialogEx.Confirm(this, "Are you sure you want to cancel conversion?");
                if (result != DialogResult.Yes)
                    e.Cancel = true;
            }
        }
    }
}
