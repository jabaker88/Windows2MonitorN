using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Windows2MonitorLibrary.WindowsApi.User32
{
    public static class User32Ext
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RecthWnd lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct RecthWnd
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static Rectangle GetWindowsRectangle(IntPtr hWnd)
        {
            Rectangle rec; ;
            RecthWnd hWndRec = new RecthWnd();

            GetWindowRect(hWnd, ref hWndRec);

            rec = new Rectangle(hWndRec.Left, hWndRec.Top, hWndRec.Right, hWndRec.Bottom);

            return rec;
        }
    }
}
