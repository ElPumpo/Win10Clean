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
                if (MessageBox.Show("This app is compatible only with Windows 10 Fall Creators Update (1709)!" + Environment.NewLine + "Are you sure you want to continue?", "Continue at your own risk", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Run(new MainForm());
                }
            }
        }
    }
}
