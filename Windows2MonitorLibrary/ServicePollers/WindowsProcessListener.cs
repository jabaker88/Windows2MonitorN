using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Windows2MonitorLibrary.ServicePollers
{
    /// <summary>
    /// Wrapper class for 
    /// </summary>
    public sealed class WindowsProcessListener : IDisposable
    {
        private readonly string Win32ProcessStartTraceQuery = @"SELECT * FROM Win32_ProcessStartTrace";
        private ManagementEventWatcher watcher;

        public event EventArrivedEventHandler ProcessStartedEvent;

        public WindowsProcessListener()
        {
            watcher = new ManagementEventWatcher(new WqlEventQuery(Win32ProcessStartTraceQuery));
            watcher.EventArrived += ProcessStartedEvent_Method;
            watcher.Start();
        }

        //Event chain to call ProcessStartedEvent
        private void ProcessStartedEvent_Method(object sender, EventArrivedEventArgs e)
        {
            ProcessStartedEvent(sender, e);
        }

        public void Dispose()
        {
            watcher.Stop();
        }
    }
}
