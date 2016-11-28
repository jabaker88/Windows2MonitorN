using System;
using System.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows2MonitorLibrary.Utility;
using Windows2MonitorLibrary.ServicePollers;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace _Windows2MonitorLibraryTest
{
    [TestClass]
    public class WindowsProcessesTests
    {
        [TestMethod]
        public void GetAllWindowTitlesTest()
        {
            var titles = WindowsProcesses.GetAllWindowTitles();
            Assert.IsTrue(titles.Count > 0);
        }
        
        [TestMethod]
        public void GetAllWindowHandlesTest()
        {
            var handles = WindowsProcesses.GetAllWindowHandles();
            Assert.IsTrue(handles.Count > 0);
        }

        [TestMethod]
        public void ProcessHasStartedEventTest()
        {
            WindowsProcessListener winProcess = new WindowsProcessListener();
            List<string> recievedEvents = new List<string>();

            winProcess.ProcessStartedEvent += delegate (object sender, EventArrivedEventArgs e)
            {
                recievedEvents.Add(e.NewEvent.Properties["ProcessName"].Value.ToString());
            };

            var cmdProc = Process.Start(new ProcessStartInfo("cmd.exe"));
            Thread.Sleep(1000);
            cmdProc.CloseMainWindow();
            cmdProc.Close();

            Assert.IsTrue(recievedEvents.Contains("cmd.exe"));
        }

        [TestMethod]
        public void MoveWindowTest()
        {
            var cmdProc = Process.Start(new ProcessStartInfo("cmd.exe"));
            int MAX_WIDTH = 400;

            for (int i = 0; i < MAX_WIDTH; i++)
            {
                WindowPositioner.SetWindowPosition(cmdProc.MainWindowHandle, i, 0);
                Thread.Sleep(1);
            }
            Thread.Sleep(2000);

            WindowPositioner.GetWindowPosition(cmdProc.MainWindowHandle);

            var pos = WindowPositioner.GetWindowPosition(cmdProc.MainWindowHandle);

            cmdProc.CloseMainWindow(); cmdProc.Close();

            Assert.IsTrue(pos.X == MAX_WIDTH - 1 && pos.Y == 0);
        }
    }
}
