using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AppendPath
{
    internal static class Program
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessageTimeout(
            IntPtr hWnd,
            [MarshalAs(UnmanagedType.U4)] int Msg,
            IntPtr wParam,
            IntPtr lParam,
            [MarshalAs(UnmanagedType.U4)] int fuFlags,
            [MarshalAs(UnmanagedType.U4)] int uTimeout,
            [MarshalAs(UnmanagedType.U4)] ref int lpdwResult);

        [STAThread]
        private static void Main(string[] args)
        {
            string path;
            if (args.Length == 1)
            {
                path = args[0];
            }
            else
            {
                var fd = new CommonOpenFileDialog
                {
                    IsFolderPicker = true
                };
                if (fd.ShowDialog() != CommonFileDialogResult.Ok) return;
                path = fd.FileName;
            }
            Environment.SetEnvironmentVariable(
                "Path",
                Environment.GetEnvironmentVariable(
                    "Path",
                    EnvironmentVariableTarget.Machine) + ";" +
                path,
                EnvironmentVariableTarget.Machine);
            var r = 0;
            SendMessageTimeout(
                (IntPtr)0xffff,
                0x001A,
                (IntPtr)0,
                Marshal.StringToHGlobalAnsi("Environment"),
                0x0002,
                5000,
                ref r);
        }
    }
}