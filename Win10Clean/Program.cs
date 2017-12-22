using System;
using System.Windows.Forms;

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
                MessageBox.Show("This app is compatible only with Windows 10 Fall Creators Update (1709)!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
