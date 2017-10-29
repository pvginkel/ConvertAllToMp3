using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertAllToMp3
{
    public partial class ProgressForm : SystemEx.Windows.Forms.Form
    {
        public static void Show(IWin32Window owner, string title, Action<IProgress> action)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (title == null)
                throw new ArgumentNullException(nameof(title));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (var form = new ProgressForm())
            {
                form.Text = title;

                Exception exception = null;

                form.Shown += (s, e) =>
                {
                    var thread = new Thread(() =>
                    {
                        try
                        {
                            action(new Progress(form));
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }

                        form.BeginInvoke(new Action(form.Dispose));
                    });

                    thread.Start();
                };

                form.ShowDialog(owner);

                if (exception != null)
                    throw exception;
            }
        }

        private ProgressForm()
        {
            InitializeComponent();
        }

        private class Progress : IProgress
        {
            private readonly ProgressForm _form;

            public Progress(ProgressForm form)
            {
                _form = form;
            }

            public void SetLabel(string label)
            {
                if (_form.InvokeRequired)
                    _form.BeginInvoke(new Action(() => SetLabel(label)));
                else
                    _form._label.Text = label;
            }

            public void SetProgress(float progress)
            {
                if (_form.InvokeRequired)
                {
                    _form.BeginInvoke(new Action(() => SetProgress(progress)));
                    return;
                }

                if (progress < 0)
                {
                    if (_form._progress.Style != ProgressBarStyle.Marquee)
                        _form._progress.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    if (_form._progress.Style != ProgressBarStyle.Continuous)
                        _form._progress.Style = ProgressBarStyle.Continuous;

                    _form._progress.Value = Math.Min(Math.Max((int)(_form._progress.Maximum * progress), 0), _form._progress.Maximum);
                }
            }
        }
    }

    public interface IProgress
    {
        void SetLabel(string label);

        void SetProgress(float progress);
    }
}
