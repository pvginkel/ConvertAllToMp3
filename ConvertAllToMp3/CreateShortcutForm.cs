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
using SystemEx;
using SystemEx.Windows.Forms;
using ConvertAllToMp3.Properties;

namespace ConvertAllToMp3
{
    public partial class CreateShortcutForm : SystemEx.Windows.Forms.Form
    {
        public CreateShortcutForm()
        {
            InitializeComponent();

            var browseDirectoryButton = new Button
            {
                Image = Resources.folder
            };

            browseDirectoryButton.Click += BrowseDirectoryButton_Click;

            _directory.RightButtons.Add(browseDirectoryButton);

            _afterConversion.Items.Add(new ComboBoxItem<AfterConvert>("Ask always", AfterConvert.Ask));
            _afterConversion.Items.Add(new ComboBoxItem<AfterConvert>("Move to Recycle Bin", AfterConvert.Recycle));
            _afterConversion.Items.Add(new ComboBoxItem<AfterConvert>("Delete Permanently", AfterConvert.Delete));
            _afterConversion.SelectedIndex = 0;

            _quality.Items.Add(new ComboBoxItem<Quality>("Very high (~245 kbps)", Quality.V0));
            _quality.Items.Add(new ComboBoxItem<Quality>("High (~225 kbps)", Quality.V1));
            _quality.Items.Add(new ComboBoxItem<Quality>("Standard (~190 kbps)", Quality.V2));
            _quality.Items.Add(new ComboBoxItem<Quality>("Medium (~175 kbps)", Quality.V3));
            _quality.Items.Add(new ComboBoxItem<Quality>("Portable high (~165 kbps)", Quality.V4));
            _quality.Items.Add(new ComboBoxItem<Quality>("Portable medium (~130 kbps)", Quality.V5));
            _quality.Items.Add(new ComboBoxItem<Quality>("Portable low (~115 kbps)", Quality.V6));
            _quality.SelectedIndex = 2;

            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _acceptButton.Enabled = _directory.Text.Length > 0;
        }

        private void BrowseDirectoryButton_Click(object sender, EventArgs e)
        {
            string directory = BrowseForFolderDialog.Show(
                this,
                "Directory to convert",
                BrowseForFolderOptions.EditBox |
                BrowseForFolderOptions.NewDialogStyle |
                BrowseForFolderOptions.NoNewFolderButton |
                BrowseForFolderOptions.ReturnOnlyFileSystemDirectories |
                BrowseForFolderOptions.UseNewUI
            );

            if (directory != null)
                _directory.Text = directory;
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _directory_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            string fileName;

            using (var form = new SaveFileDialog())
            {
                form.AutoUpgradeEnabled = true;
                form.FileName = $"Convert {Path.GetFileName(_directory.Text)} to MP3.lnk";
                form.Filter = "Shortcut (*.lnk)|*.lnk|All Files (*.*)|*.*";
                form.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                form.OverwritePrompt = true;
                form.Title = "Save Shortcut";

                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                fileName = form.FileName;
            }

            SaveShortcut(fileName);

            Close();
        }

        private void SaveShortcut(string fileName)
        {
            var arguments = new List<string>();

            arguments.Add(_directory.Text);

            foreach (string pattern in _excludePatterns.Patterns)
            {
                arguments.Add("--exclude");
                arguments.Add(pattern);
            }

            switch (((ComboBoxItem<AfterConvert>)_afterConversion.SelectedItem).Value)
            {
                case AfterConvert.Recycle:
                    arguments.Add("--recycle");
                    break;
                case AfterConvert.Delete:
                    arguments.Add("--delete");
                    break;
            }

            arguments.Add("--quality");
            arguments.Add(((ComboBoxItem<Quality>)_quality.SelectedItem).Value.ToString());

            var shellLink = new ShellLink();
            shellLink.Description = $"Convert all WAV files in {Path.GetFileName(_directory.Text)} to MP3 files";
            shellLink.Arguments = Escaping.ShellEncode(arguments);
            shellLink.IconIndex = 0;
            shellLink.IconPath = GetType().Assembly.Location;
            shellLink.Target = GetType().Assembly.Location;
            shellLink.WorkingDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
            shellLink.Save(fileName);
        }

        private class ComboBoxItem<T>
        {
            private readonly string _label;

            public T Value { get; }

            public ComboBoxItem(string label, T value)
            {
                _label = label;
                Value = value;
            }

            public override string ToString()
            {
                return _label;
            }
        }

        private enum Quality
        {
            V0,
            V1,
            V2,
            V3,
            V4,
            V5,
            V6
        }
    }
}
