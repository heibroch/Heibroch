using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Heibroch.WindowsInteractivity
{
    public class ClipboardPasted
    {
        public bool ProcessPaste { get; set; } = true;
    }

    public class ClipboardUpdated
    {
        public bool ProcessUpdate { get; set; } = true;
    }

    public sealed class ClipboardInterop
    {
        public static event EventHandler<ClipboardUpdated> ClipboardUpdate;
        public static event EventHandler<ClipboardPasted> ClipboardPaste;
        private static ClipboardInteropInternal clipboardInteropInternal = new ClipboardInteropInternal();
        private static void OnClipboardUpdate(ClipboardUpdated e) => ClipboardUpdate?.Invoke(null, e);
        private static void OnClipboardPaste(ClipboardPasted e) => ClipboardPaste?.Invoke(null, e);

        private class ClipboardInteropInternal : Form
        {
            public ClipboardInteropInternal()
            {
                SetParent(Handle, HWND_MESSAGE);
                AddClipboardFormatListener(Handle);
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_CLIPBOARDUPDATE:
                    {
                        var clipboardUpdated = new ClipboardUpdated();
                        OnClipboardUpdate(clipboardUpdated);
                        if (!clipboardUpdated.ProcessUpdate) return;
                        break;
                    }
                    case WM_CLIPBOARDPASTE:
                    {
                        var clipboardPasted = new ClipboardPasted();
                        OnClipboardPaste(clipboardPasted);
                        if (!clipboardPasted.ProcessPaste) return;
                        break;
                    }
                    default: break;
                }         

                base.WndProc(ref m);
            }

            // See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx
            public const int WM_CLIPBOARDUPDATE = 0x031D;
            public const int WM_CLIPBOARDPASTE = 0x0302;
            public static IntPtr HWND_MESSAGE = new IntPtr(-3);

            // See http://msdn.microsoft.com/en-us/library/ms632599%28VS.85%29.aspx#message_only
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AddClipboardFormatListener(IntPtr hwnd);

            // See http://msdn.microsoft.com/en-us/library/ms633541%28v=vs.85%29.aspx
            // See http://msdn.microsoft.com/en-us/library/ms649033%28VS.85%29.aspx
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        }
    }
}