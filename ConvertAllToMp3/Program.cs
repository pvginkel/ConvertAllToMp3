using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertAllToMp3
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form mainForm;
            if (args.Length == 0)
                mainForm = new CreateShortcutForm();
            else
                mainForm = new MainForm(args);

            Application.Run(mainForm);
        }
    }
}
