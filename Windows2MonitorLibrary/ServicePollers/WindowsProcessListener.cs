using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows2MonitorLibrary.ObservableCollections;

namespace Windows2MonitorLibrary.ServicePollers
{
    /// <summary>
    /// Listens for new Windows events via WMI
    /// Notice: this has really bad performance
    /// </summary>
    [Obsolete]
    public class WindowsProcessWmiListener : IDisposable
    {
        private readonly string Win32ProcessStartTraceQuery = 
            @"
                SELECT * FROM Win32_ProcessStartTrace 
                WHERE ProcessName <> 'Win32_ProcessTrace'
            ";

        private ManagementEventWatcher watcher;

        public event EventArrivedEventHandler ProcessStartedEvent;

        public WindowsProcessWmiListener()
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

    /// <summary>
    /// Event Handler for New Processes 
    /// </summary>
    /// <param name="object"></param>
    /// <param name="NewProcessEventArgs"></param>
    public delegate void NewProcessEventHandler<T>(object sender, NewProcessEventArgs<T> e);

    /// <summary>
    /// Event Arg for NewProcessEventHandler
    /// </summary>
    public class NewProcessEventArgs<T>
    {
        public List<T> NewProcesses { get; internal set; }
    }
    
    /// <summary>
    /// Polls Windows processes and notifies on a new process entering the collection
    /// </summary>
    public class WindowsProcessListener
    {
        /// <summary>
        /// Notification of a new process that has started
        /// </summary>
        public event NewProcessEventHandler<Process> NewProcessEvent;

        /// <summary>
        /// How often we should pool for new processes 
        /// </summary>
        public double PollingInterval { get; set; } = 10000;

        private event NotifySetChangedEventHandler<Process> ProcessCollectionAdded;
        private ObservableHashSet<Process> processColletion;
        private Timer processTimer;
        
        public WindowsProcessListener()
        {
            if (processColletion == null)
                processColletion = new ObservableHashSet<Process>();

            if (processTimer == null)
                processTimer = new Timer(PollingInterval);

            processTimer.AutoReset = true;
            processTimer.Enabled = true;

            //init collection
            foreach (Process proc in Process.GetProcesses())
                processColletion.Add(proc);

            //add timers and events
            processColletion.NotifySetAddedElement += ProcessCollectionAddedEvent;
            processTimer.Elapsed += ProcessTimerElasped;
        }

        /// <summary>
        /// Ctor sets up the Pooling Timer interval
        /// </summary>
        /// <param name="poolingInterval"></param>
        public WindowsProcessListener(double poolingInterval) : this()
        {
            this.PollingInterval = poolingInterval;
        }

        private void ProcessTimerElasped(object sender, ElapsedEventArgs e)
        {
            foreach(Process proc in Process.GetProcesses())
            {
                //only add if item is not in collection
                if (!processColletion.Contains(proc))
                    processColletion.Add(proc);
            }

            //Remove inactive processes
            var inactiveProcs = processColletion.Except(Process.GetProcesses());

            foreach(Process inactiveProc in inactiveProcs)
            {
                processColletion.Remove(inactiveProc);
            }
        }

        private void ProcessCollectionAddedEvent(object sender, NotifySetChangedEventArgs<Process> e)
        {
            if (e.NewItem != null)
            {
                NewProcessEventArgs<Process> eventArgs = new NewProcessEventArgs<Process>();

                if (eventArgs.NewProcesses == null)
                    eventArgs.NewProcesses = new List<Process>(); 

                eventArgs.NewProcesses.Add(e.NewItem);

                NewProcessEvent(sender, eventArgs);
            }

            ProcessCollectionAdded(sender, e);
        }
    }
}
