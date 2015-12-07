using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Wdh.Digisharp.UI.Common.Support
{
    internal enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }

    public static class MouseHook
    {
        public static event EventHandler LeftMouseButtonDown = null;
        public static event EventHandler LeftMouseButtonUp = null;
        public static event EventHandler RightMouseButtonDown = null;
        public static event EventHandler RightMouseButtonUp = null;

        public static void Start()
        {
            if (hookId != IntPtr.Zero) return;
            hookId = SetHook(loweLevelMouseProcess);
        }
        public static void Stop()
        {
            if (hookId == IntPtr.Zero) return;
            UnhookWindowsHookEx(hookId);
            hookId = IntPtr.Zero;
        }

        private static LowLevelMouseProc loweLevelMouseProcess = HookCallback;
        private static IntPtr hookId = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            //14 is WH_MOUSE_LL
            var hook = SetWindowsHookEx(14, proc, GetModuleHandle("user32"), 0);
            if (hook == IntPtr.Zero) throw new System.ComponentModel.Win32Exception();
            return hook;
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0) return CallNextHookEx(hookId, nCode, wParam, lParam);

            if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages) wParam && LeftMouseButtonDown != null)
                LeftMouseButtonDown(null, null);

            if (MouseMessages.WM_LBUTTONUP == (MouseMessages) wParam && LeftMouseButtonUp != null)
                LeftMouseButtonUp(null, null);

            if (MouseMessages.WM_RBUTTONDOWN == (MouseMessages) wParam && RightMouseButtonDown != null)
                RightMouseButtonDown(null, null);

            if (MouseMessages.WM_RBUTTONUP == (MouseMessages) wParam && RightMouseButtonUp != null)
                RightMouseButtonUp(null, null);

            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
