using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TriangleOfPower.Code;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace TriangleOfPower
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RaspberryConnection raspberryConnection;
        public MainWindow()
        {
            InitializeComponent();
            InitializeIcon();
            InitializeConnection();

        }

        private void InitializeConnection()
        {
            var btClient = DeviceFinder.GetClientForDevice("HC-05");
            raspberryConnection = new RaspberryConnection(btClient.GetStream(), btClient.Client);
            raspberryConnection.Start();
            App.MainIcon.ShowBalloonTip(10000, Properties.Resources.AppName, "Connected!", ToolTipIcon.Info);
        }

        private void InitializeIcon()
        {
            App.MainIcon = new NotifyIcon();
            App.MainIcon.Text = Properties.Resources.AppName;
            App.MainIcon.Icon = TriangleOfPower.Properties.Resources.icon;
            App.MainIcon.Visible = true;
            App.MainIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem()
                {
                    Text = Properties.Resources.AppName,
                    Visible = true,
                    Enabled = false
                }
            });
        }
    }
}
