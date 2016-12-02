using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Windows2MonitorLibrary.ServicePollers;
using Windows2MonitorLibrary.Utility;

namespace Windows2MonitorLibrary.Entities
{
    /// <summary>
    /// Service wrapper for new processes and handling their positions
    /// </summary>
    public sealed class ProcessMonitor : WindowsProcessListener
    {
        public int MonitorCount { get; private set; } = 0;
        public int DefaultMonitor { get; set; } = 1;
        public Vector2 DefaultWindowLocation { get; set; } = new Vector2();
        public long NewProcessesCount { get; private set; } = 0;
        public long AttemptedWindowMovesCount { get; private set; } = 0;
        public int MaximumWindowMoveAttempts { get; set; } = 10;

        private object _lock = new object();

        public ProcessMonitor() : base()
        {
            MonitorCount = WindowPositioner.GetMonitorCount();
            this.NewProcessEvent += ProcessMonitorProcessStartedEvent;
        }

        public ProcessMonitor(Vector2 defaultWindowLocation) : this()
        {
            DefaultWindowLocation = defaultWindowLocation;
        }

        private void ProcessMonitorProcessStartedEvent(object sender, NewProcessEventArgs<Process> e)
        {
            lock (_lock)
            {
                try
                {
                    NewProcessesCount += e.NewProcesses.Count;

                    List<Process> procs = new List<Process>();

                    foreach (Process proc in e.NewProcesses)
                    {
                        procs.Add(proc);

                        //On a new process spawn, we'll spin on attempts to move it
                        bool hasMoved = false;

                        for (int i = 0; !hasMoved || i < MaximumWindowMoveAttempts; i++)
                        {
                            hasMoved = WindowPositioner.SetWindowPositionByMonitor(proc.MainWindowHandle, DefaultWindowLocation.X, DefaultWindowLocation.Y, DefaultMonitor);
                            AttemptedWindowMovesCount++;
                        }
                    }
                }
                catch
                {
                    //TODO: Log exceptions
                    throw;
                }
            }
        }
    }
}
