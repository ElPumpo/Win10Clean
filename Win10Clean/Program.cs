using System;
using System.Windows.Forms;

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
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if (Utilities.IsCompatible())
            {
                Application.Run(new MainForm());
            }
            else
            {
                if (MessageBox.Show("This app is compatible only with Windows 10 Fall Creators Update (1709)!" + Environment.NewLine + "Are you sure you want to continue?", "Continue at your own risk", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Run(new MainForm());
                }
            }
        }
    }
}
