using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Cirno.ChinaGS.Injection.Experimental
{
    /// <summary>
    /// ControlPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        public Program program;

        public ControlPanel()
        {
            InitializeComponent();
        }

        public ControlPanel(Program prog)
        {
            InitializeComponent();
            this.program = prog;
        }

        private void Btn_1_Click(object sender, RoutedEventArgs e)
        {
            Process[] processesByName = Process.GetProcessesByName("LockMouse");
            if (processesByName != null)
            {
                Process[] array = processesByName;
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].Kill();
                }
            }

            ShowWindow(FindWindow("progman", null), 0);
            ShowWindow(FindWindow("Shell_traywnd", null), 0);
            ShowWindow(FindWindow("button", null), 0);

            ShowWindow(FindWindow("progman", null), 1);
            ShowWindow(FindWindow("Shell_traywnd", null), 1);
            ShowWindow(FindWindow("button", null), 1);
        }

        private void Btn_2_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("osk.exe");
        }

        private void Btn_3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.program.RemoveAdminPanel();
                this.program.isPanelOpened = false;
            }
            catch
            {
                StatusLabel.Content = "错误：无法关闭窗口";
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int nStyle);
    }
}
