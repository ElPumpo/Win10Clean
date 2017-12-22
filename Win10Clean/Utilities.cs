using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Win10Clean
{
    internal static class Utilities
    {
        /// <summary>
        /// Checks if Windows build is compatible with this app.
        /// </summary>
        internal static bool IsCompatible()
        {
            bool isWindows10;
            bool isBuild1709;

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false))
            {
                isWindows10 = ((string)key.GetValue("ProductName", string.Empty)).Contains("Windows 10");
                isBuild1709 = Environment.OSVersion.Version.Build == 16299;
            }

            return isWindows10 && isBuild1709;
        }
    }
}
