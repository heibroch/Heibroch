using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Heibroch.WindowsInteractivity
{
    public class KeyPressed
    {
        public int Key { get; set; }

        public bool ProcessKey { get; set; } = true;
    }

    public sealed class KeyboardInterop
    {
        public static event EventHandler KeyPressed;
        private static KeyboardInteropInternal keyboardInteropInternal = new KeyboardInteropInternal();
        private static void OnClipboardUpdate(KeyPressed keyPressed, EventArgs e) => KeyPressed?.Invoke(keyPressed, e);

        private class KeyboardInteropInternal
        {
            private const int WH_KEYBOARD_LL = 13;
            private const int WM_KEYDOWN = 0x0100;
            private static LowLevelKeyboardProc proc = HookCallback;
            private static IntPtr hookId = IntPtr.Zero;
            
            public KeyboardInteropInternal()
            {
                hookId = SetHook(proc);
            }

            ~KeyboardInteropInternal()
            {
                UnhookWindowsHookEx(hookId);
            }

            private static IntPtr SetHook(LowLevelKeyboardProc proc)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                {
                    using (ProcessModule curModule = curProcess.MainModule)
                    {
                        return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                    }
                }
            }

            private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

            private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    var globalKeyPressed = new KeyPressed() { Key = Marshal.ReadInt32(lParam) };

                    OnClipboardUpdate(globalKeyPressed, new EventArgs());

                    if (!globalKeyPressed.ProcessKey) return new IntPtr(1);
                }

                return CallNextHookEx(hookId, nCode, wParam, lParam);
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);
        }
    } 
}