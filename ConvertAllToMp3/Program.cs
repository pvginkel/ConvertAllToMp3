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

#if !DEBUG
            if (PerformUpdate(args))
                return;
#endif

            Form mainForm;
            if (args.Length == 0)
                mainForm = new CreateShortcutForm();
            else
                mainForm = new MainForm(args);

            Application.Run(mainForm);
        }

        private static bool PerformUpdate(string[] args)
        {
            try
            {
                const string packageCode = "ConvertAllToMP3";

                bool updateAvailable = NuGetUpdate.Update.IsUpdateAvailable(packageCode);
                if (updateAvailable)
                {
                    NuGetUpdate.Update.StartUpdate(packageCode, args);
                    return true;
                }
            }
            catch
            {
                // Ignore exceptions.
            }

            return false;
        }
    }
}
