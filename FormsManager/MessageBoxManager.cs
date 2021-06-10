using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FormsManager
{
    public class MessageBoxManager
    {
        private const int WhCallwndprocret = 12;
        /*
                private const int WmDestroy = 0x0002;
        */
        private const int WmInitdialog = 0x0110;
        /*
                private const int WmTimer = 0x0113;
        */
        /*
                private const int WmUser = 0x400;
        */
        /*
                private const int DmGetdefid = WmUser + 0;
        */
        private const int Mbok = 1;
        private const int MbCancel = 2;
        private const int MbAbort = 3;
        private const int MbRetry = 4;
        private const int MbIgnore = 5;
        private const int MbYes = 6;
        private const int MbNo = 7;
        private static readonly HookProc HProc;
        private static readonly EnumChildProc EnumProc;
        [ThreadStatic] private static IntPtr _hHook;
        [ThreadStatic] private static int _nButton;

        /// <summary>
        ///     OK text
        /// </summary>
        public static string Ok = "&OK";

        /// <summary>
        ///     Cancel text
        /// </summary>
        public static string Cancel = "&Cancel";

        /// <summary>
        ///     Abort text
        /// </summary>
        public static string Abort = "&Abort";

        /// <summary>
        ///     Retry text
        /// </summary>
        public static string Retry = "&Retry";

        /// <summary>
        ///     Ignore text
        /// </summary>
        public static string Ignore = "&Ignore";

        /// <summary>
        ///     Yes text
        /// </summary>
        public static string Yes = "&Yes";

        /// <summary>
        ///     No text
        /// </summary>
        public static string No = "&No";

        static MessageBoxManager()
        {
            HProc = MessageBoxHookProc;
            EnumProc = MessageBoxEnumProc;
            _hHook = IntPtr.Zero;
        }

        /*
                [DllImport("user32.dll")]
                private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        */

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLengthW", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        /*
                [DllImport("user32.dll", EntryPoint = "GetWindowTextW", CharSet = CharSet.Unicode)]
                private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);
        */
        /*
                [DllImport("user32.dll")]
                private static extern int EndDialog(IntPtr hDlg, IntPtr nResult);
        */

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetDlgCtrlID(IntPtr hwndCtl);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDlgItem(IntPtr hDlg, int nIdDlgItem);

        [DllImport("user32.dll", EntryPoint = "SetWindowTextW", CharSet = CharSet.Unicode)]
        private static extern bool SetWindowText(IntPtr hWnd, string lpString);

        /// <summary>
        ///     Enables MessageBoxManager functionality
        /// </summary>
        /// <remarks>
        ///     MessageBoxManager functionality is enabled on current thread only.
        ///     Each thread that needs MessageBoxManager functionality has to call this method.
        /// </remarks>
        public static void Register()
        {
            if (_hHook != IntPtr.Zero)
                throw new NotSupportedException("One hook per thread allowed.");
#pragma warning disable 618
            _hHook = SetWindowsHookEx(WhCallwndprocret, HProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
#pragma warning restore 618
        }

        /// <summary>
        ///     Disables MessageBoxManager functionality
        /// </summary>
        /// <remarks>
        ///     Disables MessageBoxManager functionality on current thread only.
        /// </remarks>
        public static void Unregister()
        {
            if (_hHook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hHook);
                _hHook = IntPtr.Zero;
            }
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(_hHook, nCode, wParam, lParam);

            var msg = (Cwpretstruct) Marshal.PtrToStructure(lParam, typeof (Cwpretstruct));
            var hook = _hHook;

            if (msg.message == WmInitdialog)
            {
                // ReSharper disable UnusedVariable
                var nLength = GetWindowTextLength(msg.hwnd);
                // ReSharper restore UnusedVariable
                var className = new StringBuilder(10);
                GetClassName(msg.hwnd, className, className.Capacity);
                if (className.ToString() == "#32770")
                {
                    _nButton = 0;
                    EnumChildWindows(msg.hwnd, EnumProc, IntPtr.Zero);
                    if (_nButton == 1)
                    {
                        var hButton = GetDlgItem(msg.hwnd, MbCancel);
                        if (hButton != IntPtr.Zero)
                            SetWindowText(hButton, Ok);
                    }
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static bool MessageBoxEnumProc(IntPtr hWnd, IntPtr lParam)
        {
            var className = new StringBuilder(10);
            GetClassName(hWnd, className, className.Capacity);
            if (className.ToString() == "Button")
            {
                var ctlId = GetDlgCtrlID(hWnd);
                switch (ctlId)
                {
                    case Mbok:
                        SetWindowText(hWnd, Ok);
                        break;
                    case MbCancel:
                        SetWindowText(hWnd, Cancel);
                        break;
                    case MbAbort:
                        SetWindowText(hWnd, Abort);
                        break;
                    case MbRetry:
                        SetWindowText(hWnd, Retry);
                        break;
                    case MbIgnore:
                        SetWindowText(hWnd, Ignore);
                        break;
                    case MbYes:
                        SetWindowText(hWnd, Yes);
                        break;
                    case MbNo:
                        SetWindowText(hWnd, No);
                        break;
                }
                _nButton++;
            }

            return true;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Cwpretstruct
        {
            private readonly IntPtr lResult;
            private readonly IntPtr lParam;
            private readonly IntPtr wParam;
            public readonly uint message;
            public readonly IntPtr hwnd;
        };

        private delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}