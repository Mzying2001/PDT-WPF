﻿using System.Windows;
using System.Windows.Interop;

namespace PDT_WPF.Utils
{
    public static class WindowHelper
    {
        public static void SetForeground(Window window)
        {
            var wndPid = WinApi.GetWindowThreadProcessId(new WindowInteropHelper(window).Handle, default);
            var curPid = WinApi.GetWindowThreadProcessId(WinApi.GetForegroundWindow(), default);

            WinApi.AttachThreadInput(curPid, wndPid, true);
            window.Show();
            window.Activate();
            WinApi.AttachThreadInput(curPid, wndPid, false);
        }
    }
}
