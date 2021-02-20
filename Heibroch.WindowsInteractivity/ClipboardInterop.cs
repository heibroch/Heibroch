using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

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
        //public static event EventHandler<ClipboardPasted> ClipboardPaste;
        private static ClipboardInteropInternal clipboardInteropInternal = new ClipboardInteropInternal();
        private static void OnClipboardUpdate(ClipboardUpdated e) => ClipboardUpdate?.Invoke(null, e);
        //private static void OnClipboardPaste(ClipboardPasted e) => ClipboardPaste?.Invoke(null, e);

        public static void TriggerPaste() => clipboardInteropInternal.Paste();

        internal class ClipboardInteropInternal : Form
        {
            public ClipboardInteropInternal()
            {
                SetParent(Handle, HWND_MESSAGE);
                AddClipboardFormatListener(Handle);
            }
                                 
            public void Paste()
            {
                Console.WriteLine("PASTED!");
                //SendKeys.SendWait(Clipboard.GetText());
                //SendMessage(hwnd, WM_COMMAND, (IntPtr)0xfff1, IntPtr.Zero);


                //// Get the Notepad Handle
                //IntPtr hWnd = process.Handle;

                //// Activate the Notepad Window
                //BringWindowToTop(hWnd);

                // Use SendKeys to Paste
                //SendKeys.SendWait("^V");


                //keybd_event(0x11, 0, 1 | 0, 0);//ctrl down
                //keybd_event(0x56, 0, 1 | 0, 0);//v down
                //keybd_event(0x56, 0, 1 | 2, 0);//v up
                //keybd_event(0x11, 0, 1 | 2, 0);//ctrl up

                //var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.LeftCtrl | Key.V);

                //var eventArgs = new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, "^V"));
                //eventArgs.RoutedEvent = TextInputEvent;

                //InputManager.Current.ProcessInput(eventArgs);


                var pasteKey = new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.LeftCtrl | Key.V);
                pasteKey.RoutedEvent = Keyboard.KeyDownEvent;
                InputManager.Current.ProcessInput(pasteKey);

                // This doesn't  
                //var aKey = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.A);
                //aKey.RoutedEvent = Keyboard.KeyDownEvent;
                //InputManager.Current.ProcessInput(aKey);

            }

            //[DllImport("user32.dll", SetLastError = true)]
            //public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

            //[DllImport("user32")]
            //private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
            //private const uint WM_COMMAND = 0x0111;
            //SendMessage(hwnd, WM_COMMAND, (IntPtr)0xfff1, IntPtr.Zero);

            protected override void WndProc(ref Message m)
            {
                Console.WriteLine($"Clipboard updated! Msg: {m.Msg}");
                switch (m.Msg)
                {
                    case WM_CLIPBOARDUPDATE:
                    {
                        Console.WriteLine("Clipboard updated!");
                        var clipboardUpdated = new ClipboardUpdated();
                        OnClipboardUpdate(clipboardUpdated);
                        if (!clipboardUpdated.ProcessUpdate) return;
                        break;
                    }
                    //case WM_CLIPBOARDPASTE:
                    //{
                    //    Console.WriteLine("Clipboard pasted!");
                    //    var clipboardPasted = new ClipboardPasted();
                    //    OnClipboardPaste(clipboardPasted);
                    //    if (!clipboardPasted.ProcessPaste) return;
                    //    break;
                    //}
                    default: break;
                }         

                base.WndProc(ref m);
            }


            //            int hwnd = 0;
            //            int hwndChild = 0;

            //            hwnd = FindWindow(null, "Untitled - Notepad");

            //              if (hwnd != 0)
            //              {
            //	                Clipboard.SetText("sample text from clipboard");
            //	                hwnd = FindWindow(null, "Untitled - Notepad");
            //                  hwndChild = GetWindow(hwnd, GetWindow_Cmd.GW_CHILD);
            //                  SendMessage(hwndChild , WM_PASTE, 0, IntPtr.Zero);
            //              }


            //[DllImport("user32.dll")]
            //private static extern IntPtr GetForegroundWindow();



            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(int hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

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