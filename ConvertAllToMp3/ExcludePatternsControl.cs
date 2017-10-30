using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertAllToMp3
{
    public partial class ExcludePatternsControl : UserControl
    {
        public IList<string> Patterns => _patterns.Items.Cast<string>().ToList();

        public ExcludePatternsControl()
        {
            InitializeComponent();

            UpdateMargins();
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _addButton.Enabled = _pattern.Text.Length > 0;
            _removeButton.Enabled = _patterns.SelectedIndex >= 0;
        }

        private void UpdateMargins()
        {
            _pattern.Margin = new Padding(
                _pattern.Margin.Left,
                (_addButton.Height - _pattern.Height) / 2 + _addButton.Margin.Top - 1,
                _pattern.Margin.Right,
                0
            );
        }

        private void _addButton_SizeChanged(object sender, EventArgs e)
        {
            UpdateMargins();
        }

        private void _pattern_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _removeButton_Click(object sender, EventArgs e)
        {
            _patterns.Items.RemoveAt(_patterns.SelectedIndex);
            UpdateEnabled();
        }

        private void _addButton_Click(object sender, EventArgs e)
        {
            _patterns.Items.Add(_pattern.Text);
            _pattern.Text = null;
        }

        private void _patterns_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }
    }
}
