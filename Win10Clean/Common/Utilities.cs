using System;
using Microsoft.Win32;

/*
 * Win10Clean - Cleanup your Windows 10 environment
 * Copyright (C) 2017 Hawaii_Beach & deadmoon
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the license, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/

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
