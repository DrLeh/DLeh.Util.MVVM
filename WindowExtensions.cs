using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace DLeh.Util.MVVM
{
    public static class WindowExtensions
    {
        #region Window Flashing API Stuff

        private const UInt32 FLASHW_STOP = 0; //Stop flashing. The system restores the window to its original state.
        private const UInt32 FLASHW_CAPTION = 1; //Flash the window caption.
        private const UInt32 FLASHW_TRAY = 2; //Flash the taskbar button.
        private const UInt32 FLASHW_ALL = 3; //Flash both the window caption and taskbar button.
        private const UInt32 FLASHW_TIMER = 4; //Flash continuously, until the FLASHW_STOP flag is set.
        private const UInt32 FLASHW_TIMERNOFG = 12; //Flash continuously until the window comes to the foreground.

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public UInt32 cbSize; //The size of the structure in bytes.
            public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.
            public UInt32 dwFlags; //The Flash Status.
            public UInt32 uCount; // number of times to flash the window
            public UInt32 dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        #endregion

        /// <summary>
        /// Flashes window count times. If count not specified, will flash continuously until selected. 
        /// If count is specified, make sure you have a call to StopFlashingWindow()
        /// </summary>
        public static void FlashWindow(this Window win, UInt32 count = UInt32.MaxValue)
        {
            //If windows is active, do nothing
            if (win.IsActive) return;

            bool flashTimer = count == UInt32.MaxValue;
            uint flags = FLASHW_TRAY | (flashTimer ? FLASHW_TIMER : FLASHW_TIMERNOFG);

            FLASHWINFO info = new FLASHWINFO
            {
                hwnd = new WindowInteropHelper(win).Handle,
                dwFlags = flags,
                uCount = count,
                dwTimeout = 0
            };

            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            FlashWindowEx(ref info);
        }

        public static void StopFlashingWindow(this Window win)
        {
            FLASHWINFO info = new FLASHWINFO();
            info.hwnd = new WindowInteropHelper(win).Handle;
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            info.dwFlags = FLASHW_STOP;
            info.uCount = UInt32.MaxValue;
            info.dwTimeout = 0;

            FlashWindowEx(ref info);
        }

        public static void ShowWindow(this Window win, object datacontext)
        {
            win.DataContext = datacontext;
            win.ShowDialog();
        }
        public static void ShowWindow(this Window win, bool dialog = true)
        {
            win.Owner = Application.Current.MainWindow;
            if (dialog)
                win.ShowDialog();
            else
                win.Show();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static void ShowWindowCentered(this Window win)
        {
            double currentWinHeight = Application.Current.MainWindow.ActualHeight;
            double currentWinWidth = Application.Current.MainWindow.ActualWidth;
            IntPtr myHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            RECT rct;

            if (GetWindowRect(myHandle, out rct))
            {
                // THIS IS YOUR DIALOG WINDOW  
                win.Top = (rct.Top + (currentWinHeight / 2.0)) - (win.Height / 2.0);
                win.Left = (rct.Left + (currentWinWidth / 2.0)) - (win.Width / 2.0);
                win.ShowDialog();
            }
        }
    }
}
