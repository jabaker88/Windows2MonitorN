using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClrPlus.Windows.Api;
using System.Windows.Forms;
using System.Drawing;
using ClrPlus.Windows.Api.Structures;
using Windows2MonitorLibrary.WindowsApi.User32;

namespace Windows2MonitorLibrary.Utility
{
    /// <summary>
    /// 2D Vector Type
    /// </summary>
    public struct Vector2
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    /// <summary>
    /// Utility Class to move Windows by Process 
    /// </summary>
    public static class WindowPositioner
    {
        /// <summary>
        /// Window types flags
        /// </summary>
        public enum WindowFlags : int
        {
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOSIZE     = 0x0001
        }

        /// <summary>
        /// Wrapping method for User32 SetWindowPos
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="uFlag"></param>
        /// <returns></returns>
        public static bool SetWindowPosition(IntPtr hWnd, int x, int y, int uFlag = (int)WindowFlags.SWP_NOSIZE)
        {
            return User32.SetWindowPos(hWnd, IntPtr.Zero, x, y, 0, 0, uFlag);
        }

        /// <summary>
        /// Allows setting position, by monitor
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="monitorNumber"></param>
        /// <param name="uFlag"></param>
        /// <returns></returns>
        public static bool SetWindowPositionByMonitor
        (IntPtr hWnd, int x, int y, int monitorNumber, int uFlag = (int) WindowFlags.SWP_NOSIZE)
        {
            bool windowMoved = false;

            //Check to make sure the screen refereced is avaible
            if(Screen.AllScreens.Count() >= monitorNumber)
            {
                //Adjust the the resolution location
                Rectangle workingArea = Screen.AllScreens[monitorNumber].WorkingArea;
                x = workingArea.X + x;
                y = workingArea.Y + y;

                SetWindowPosition(hWnd, x, y, uFlag);
            }
            else
            {
                throw new 
                    MonitorReferencedNotFoundException("The monitor count is too high in: " + nameof(SetWindowPositionByMonitor));
            }

            return windowMoved;
        }

        /// <summary>
        /// Gets the Window position, by process handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Vector2 GetWindowPosition(IntPtr hWnd)
        {
            Rectangle pos;

            pos = User32Ext.GetWindowsRectangle(hWnd);
            Vector2 vec2 = new Vector2();

            vec2.X = pos.X;
            vec2.Y = pos.Y;

            return vec2;
        }
    }
}
