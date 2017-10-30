using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvertAllToMp3
{
    public static class ShellFileOperation
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public FileFuncFlags wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            public FileOpFlags fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszProgressTitle;
        }

        private enum FileFuncFlags : uint
        {
            FO_MOVE = 0x1,
            FO_COPY = 0x2,
            FO_DELETE = 0x3,
            FO_RENAME = 0x4
        }

        [Flags]
        private enum FileOpFlags : ushort
        {
            FOF_MULTIDESTFILES = 0x1,
            FOF_CONFIRMMOUSE = 0x2,
            FOF_SILENT = 0x4,
            FOF_RENAMEONCOLLISION = 0x8,
            FOF_NOCONFIRMATION = 0x10,
            FOF_WANTMAPPINGHANDLE = 0x20,
            FOF_ALLOWUNDO = 0x40,
            FOF_FILESONLY = 0x80,
            FOF_SIMPLEPROGRESS = 0x100,
            FOF_NOCONFIRMMKDIR = 0x200,
            FOF_NOERRORUI = 0x400,
            FOF_NOCOPYSECURITYATTRIBS = 0x800,
            FOF_NORECURSION = 0x1000,
            FOF_NO_CONNECTED_ELEMENTS = 0x2000,
            FOF_WANTNUKEWARNING = 0x4000,
            FOF_NORECURSEREPARSE = 0x8000
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

        private static string BuildPath(IEnumerable<string> paths)
        {
            var sb = new StringBuilder();

            foreach (string path in paths)
            {
                sb.Append(path).Append('\0');
            }

            return sb.Append('\0').ToString();
        }

        public static bool Delete(IWin32Window owner, IEnumerable<string> paths, bool allowUndo, bool askForConformation, bool showUi)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));

            var flags = (FileOpFlags)0;

            if (allowUndo)
                flags |= FileOpFlags.FOF_ALLOWUNDO;
            if (!askForConformation)
                flags |= FileOpFlags.FOF_NOCONFIRMATION;

            if (showUi)
                flags |= FileOpFlags.FOF_WANTNUKEWARNING;
            else
                flags |= FileOpFlags.FOF_NOERRORUI | FileOpFlags.FOF_SILENT;

            try
            {
                var fs = new SHFILEOPSTRUCT
                {
                    hwnd = owner.Handle,
                    wFunc = FileFuncFlags.FO_DELETE,
                    pFrom = BuildPath(paths),
                    fFlags = flags
                };

                SHFileOperation(ref fs);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
