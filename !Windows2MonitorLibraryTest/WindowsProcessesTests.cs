using System;
using System.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows2MonitorLibrary.Utility;
using Windows2MonitorLibrary.ServicePollers;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Windows2MonitorLibrary.Entities;

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

        [TestMethod]
        public void MoveToMonitor2Test()
        {
            var cmdProc = Process.Start(new ProcessStartInfo("cmd.exe"));
            bool hasMoved = false;
            int movedTrysCount = 0;

            while (!hasMoved)
            {
                hasMoved = WindowPositioner.SetWindowPositionByMonitor(cmdProc.MainWindowHandle, 0, 0, 2);
                movedTrysCount++;
            }

            Thread.Sleep(2000);

            WindowPositioner.GetWindowPosition(cmdProc.MainWindowHandle);
            var pos = WindowPositioner.GetWindowPosition(cmdProc.MainWindowHandle);

            cmdProc.CloseMainWindow(); cmdProc.Close();
        }

        [TestMethod]
        public void CountMonitorsTest()
        {
            var monitorCount = WindowPositioner.GetMonitorCount();
            Assert.IsTrue(monitorCount > 1);
        }

        [TestMethod]
        public void ProcessMonitorTest()
        {
            ProcessMonitor procMon = new ProcessMonitor();

            while (procMon.NewProcessesCount < 10)
            {
            }
        }
    }
}
