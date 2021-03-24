using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace SIFP.Core
{
    public class Win32Api
    {
        public const int WS_CHILDWINDOW = WS_CHILD;

        public const int WS_CLIPSIBLINGS = 67108864;

        public const int WS_CLIPCHILDREN = 33554432;

        public const int WS_VISIBLE = 268435456;

        public const int WS_VSCROLL = 2097152;

        public const int WS_MAXIMIZEBOX = 65536;

        public const int WS_MINIMIZEBOX = 131072;

        public const int WS_THICKFRAME = 262144;

        public const int WS_SIZEBOX = 262144;

        public const int WS_SYSMENU = 524288;

        public const int WS_DLGFRAME = 4194304;

        public const int WS_BORDER = 8388608;

        public const int WS_CAPTION = 12582912;

        public const int WS_CHILD = 1073741824;

        public const uint WS_POPUP = 2147483648u;

        public const int WS_MAXIMIZE = 16777216;

        public const int WS_MINIMIZE = 536870912;

        public const int WS_EX_DLGMODALFRAME = 1;

        public const int WS_EX_WINDOWEDGE = 256;

        public const int WS_EX_CLIENTEDGE = 512;

        public const int WS_EX_STATICEDGE = 131072;

        public const int WS_EX_LAYERED = 524288;

        public const int WM_DESTROY = 2;

        public const int WM_MOVE = 3;

        public const int WM_SIZE = 5;

        public const int WM_CLOSE = 16;

        public const int WM_QUIT = 18;

        public const int WM_COMMAND = 273;

        public const int WM_SYSCOMMAND = 274;

        public const int WM_KEYDOWN = 256;

        public const int WM_LBUTTONDOWN = 513;

        public const int WM_LBUTTONUP = 514;

        public const int WM_EXITSIZEMOVE = 562;

        public const int WM_SIZING = 532;

        public const int SIZE_RESTORED = 0;

        public const int SIZE_MINIMIZED = 1;

        public const int SIZE_MAXIMIZED = 2;

        public const int SIZE_MAXSHOW = 3;

        public const int SIZE_MAXHIDE = 4;

        public const int SWP_NOSIZE = 1;

        public const int SWP_NOMOVE = 2;

        public const int SWP_NOZORDER = 4;

        public const int SWP_NOREDRAW = 8;

        public const int SWP_NOACTIVATE = 16;

        public const int SWP_FRAMECHANGED = 32;

        public const int SWP_SHOWWINDOW = 64;

        public const int SWP_NOOWNERZORDER = 512;

        public const int SWP_ASYNCWINDOWPOS = 16384;

        public const int HWND_TOP = 0;

        public const int HWND_BOTTOM = 1;

        public const int HWND_TOPMOST = -1;

        public const int HWND_NOTOPMOST = -2;

        public const int GWL_STYLE = -16;

        public const int GWL_EXSTYLE = -20;

        public const int SC_CLOSE = 61536;

        public const int SC_MAXIMIZE = 61488;

        public const int SC_MINIMIZE = 61472;

        public const int SC_MOVE = 61456;

        public const int SC_RESTORE = 61728;

        public const int SC_SIZE = 61440;

        public const int MF_BYPOSITION = 1024;

        public const int MF_REMOVE = 4096;

        public const int LWA_COLORKEY = 1;

        public const int LWA_ALPHA = 2;

        public const int SW_HIDE = 0;
        public const int SW_NORMAL = 1;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;

        internal static uint GetHiword(int doubleWord)
        {
            return ((uint)doubleWord >> 16) & 0xFFFF;
        }

        internal static uint GetLoword(int doubleWord)
        {
            return (uint)(doubleWord & 0xFFFF);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool ShowWindowAsync(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateWindowEx(int exStyle, string className, string windowName, int style, int x, int y, int width, int height, IntPtr hwndParent, IntPtr hMenu, IntPtr hInstance, [MarshalAs(UnmanagedType.IUnknown)] object pvParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int AdjustWindowRect(ref Rect Lprect, int dwStyle, bool bMENU);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref Rect rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetFocus();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hwnd, int message, int wParam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hwnd, int message, IntPtr wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PostMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetFocus(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        internal static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindowEx(int hwndParent, int hwndChildAfter, string strClassName, string strWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetWindowText(IntPtr hwnd, string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll")]
        internal static extern int DrawMenuBar(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool revert);

        [DllImport("user32.dll")]
        internal static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("gdi32")]
        internal static extern uint SetBkColor(IntPtr hdc, int crColor);

        [DllImport("user32")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        internal static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32")]
        internal static extern bool OpenIcon(IntPtr hWnd);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        internal const int WM_ACTIVATE = 0x0006;
        internal readonly static IntPtr WA_ACTIVE = new IntPtr(1);
        internal readonly static IntPtr WA_INACTIVE = new IntPtr(0);

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        internal static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        internal static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);

        //operType参数如下：
        public const int Hide = 0;

        public const int Nearest_Size_Location = 1;
        public const int Min = 2;
        public const int Max = 3;
    }
}
