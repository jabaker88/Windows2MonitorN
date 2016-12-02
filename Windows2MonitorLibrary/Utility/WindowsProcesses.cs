using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;

namespace Windows2MonitorLibrary.Utility
{
    /// <summary>
    /// Utility methods for Windows Processes
    /// </summary>
    public sealed class WindowsProcesses
    {

        /// <summary>
        /// Gets all Windows with titles
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllWindowTitles()
        {
            List<string> windowTitles = new List<string>();
            Process[] windowsProcs = Process.GetProcesses();

            foreach(var handle in windowsProcs)
            {
                if (!String.IsNullOrWhiteSpace(handle.MainWindowTitle))
                    windowTitles.Add(handle.MainWindowTitle);
            }

            return windowTitles;
        }

        /// <summary>
        /// Gets all Window Handles
        /// </summary>
        /// <returns></returns>
        public static List<IntPtr> GetAllWindowHandles()
        {
            List<IntPtr> windowHandles = new List<IntPtr>();
            Process[] windowsProcs = Process.GetProcesses();

            foreach(var handle in windowsProcs)
            {
                if(!String.IsNullOrWhiteSpace(handle.MainWindowTitle))
                    windowHandles.Add(handle.MainWindowHandle);
            }

            return windowHandles;
        }

        /// <summary>
        /// Gets the first occurrence of a Window Handle by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IntPtr GetWindowHandelByName(string name)
        {
            IntPtr handle = IntPtr.Zero;

            foreach (var procs in Process.GetProcessesByName(name))
                return procs.MainWindowHandle;

            return handle;
        }

        /// <summary>
        /// Gets a Process by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Process GetProcessByName(string name)
        {
            return Process.GetProcessesByName(name).First();
        }
    }
}
