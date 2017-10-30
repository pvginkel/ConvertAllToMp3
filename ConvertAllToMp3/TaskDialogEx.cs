using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;

namespace ConvertAllToMp3
{
    public static class TaskDialogEx
    {
        public static TaskDialog CreateDialog(string title, string subtitle, TaskDialogIcon mainIcon)
        {
            return new TaskDialog
            {
                CanBeMinimized = false,
                MainIcon = mainIcon,
                MainInstruction = title,
                Content = subtitle,
                PositionRelativeToWindow = true,
                WindowTitle = "Convert All to MP3"
            };
        }

        public static DialogResult Confirm(IWin32Window owner, string title, string subtitle = null, TaskDialogCommonButtons buttons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon icon = TaskDialogIcon.Warning)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            var taskDialog = CreateDialog(title, subtitle, icon);

            taskDialog.AllowDialogCancellation = (buttons & TaskDialogCommonButtons.Cancel) != 0;
            taskDialog.CommonButtons = buttons;

            return (DialogResult)taskDialog.Show(owner);
        }

        public static void Error(IWin32Window owner, string title, string subtitle = null, TaskDialogIcon icon = TaskDialogIcon.Error)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            var taskDialog = CreateDialog(title, subtitle, TaskDialogIcon.Error);

            taskDialog.AllowDialogCancellation = true;
            taskDialog.CommonButtons = TaskDialogCommonButtons.OK;

            taskDialog.Show(owner);
        }
    }
}
