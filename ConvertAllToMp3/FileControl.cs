using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertAllToMp3
{
    public partial class FileControl : UserControl
    {
        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                _label.Text = Path.GetFileName(value);
            }
        }

        public float Progress
        {
            get => (float)_progress.Value / _progress.Maximum;
            set => _progress.Value = (int)(value * _progress.Maximum);
        }

        public FileControl()
        {
            InitializeComponent();
        }
    }
}
