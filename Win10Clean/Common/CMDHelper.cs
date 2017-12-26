using System;
using System.Diagnostics;

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

namespace Win10Clean.Common
{
    class CMDHelper
    {
        public static void RunCommand(string command)
        {
            using (var process = new Process()) {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;

                try {
                    process.Start(); // start a command prompt

                    process.StandardInput.WriteLine(command); // run the command
                    process.StandardInput.Close();
                    process.WaitForExit();
                } catch (Exception ex) { }
            }
        }

        public static string RunCommandReturn(string command)
        {
            using (var process = new Process()) {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;

                try {
                    process.Start(); // start a command prompt

                    process.StandardInput.WriteLine(command); // run the command
                    process.StandardInput.Close();
                    process.WaitForExit();

                    return process.StandardOutput.ReadToEnd();
                } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }

            return null;
        }
    }
}
