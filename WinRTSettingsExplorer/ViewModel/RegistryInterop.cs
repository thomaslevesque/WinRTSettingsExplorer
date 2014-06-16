using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace WinRTSettingsExplorer.ViewModel
{
    static class RegistryInterop
    {
        // ReSharper disable UnusedMember.Local
        // ReSharper disable InconsistentNaming

        [DllImport("advapi32.dll")]
        public static extern int RegLoadAppKey(string lpFile, out SafeRegistryHandle phkResult, RegSam samDesired, int dwOptions, int reserved);

        [Flags]
        public enum RegSam
        {
            QueryValue = 0x0001,
            SetValue = 0x0002,
            CreateSubKey = 0x0004,
            EnumerateSubKeys = 0x0008,
            Notify = 0x0010,
            CreateLink = 0x0020,
            WOW64_32Key = 0x0200,
            WOW64_64Key = 0x0100,
            WOW64_Res = 0x0300,
            Read = 0x00020019,
            Write = 0x00020006,
            Execute = 0x00020019,
            AllAccess = 0x000f003f
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, BestFitMapping = false)]
        public static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, out int lpType, [Out] byte[] lpData, out int lpcbData);

        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Local
    }
}