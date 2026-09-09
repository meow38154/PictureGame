using System;
using System.Runtime.InteropServices;

namespace Test
{
    internal static class WindowsNative
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        
        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "MessageBoxW")]
        internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);        
#endif
    }
}