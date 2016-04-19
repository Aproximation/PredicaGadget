using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
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
using Application = System.Windows.Forms.Application;
using ContextMenu = System.Windows.Forms.ContextMenu;
using Control = System.Windows.Forms.Control;
using MenuItem = System.Windows.Forms.MenuItem;
using Point = System.Drawing.Point;

namespace TriangleOfPower
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private RaspberryConnection raspberryConnection;
        private readonly ContextMenu contextMenu = new ContextMenu();
        public MainWindow()
        {
            InitializeComponent();
            InitializeIcon();
            Task.Run(() => InitializeConnection());
            Application.Run();
        }

        private void InitializeConnection()
        {
            string DeviceName = "HC-05";
            App.Status = "Starting up...";
            var btClient = DeviceFinder.GetClientForDevice(DeviceName);
            raspberryConnection = new RaspberryConnection(btClient.GetStream(), btClient.Client);
            raspberryConnection.Start();
            App.Status = $"Connected to {DeviceName}";
            App.MainIcon.ShowBalloonTip(10000, Properties.Resources.AppName, "Connected!", ToolTipIcon.Info);
        }

        private void InitializeIcon()
        {
            contextMenu.MenuItems.Add(new MenuItem()
            {
                Text = $"{Properties.Resources.AppName} {Assembly.GetExecutingAssembly().GetName().Version}",
                Visible = true,
                Enabled = false,
            });
            contextMenu.MenuItems.Add(App.StatusMenuItem);
            contextMenu.MenuItems.Add(new MenuItem("-"));
            contextMenu.MenuItems.Add(new MenuItem(Properties.Resources.MainWindow_InitializeIcon_Exit,
                (sender, args) => Environment.Exit(0)));
            App.MainIcon = new NotifyIcon
            {
                Text = Properties.Resources.AppName,
                Icon = Properties.Resources.icon,
                Visible = true,
                ContextMenu = contextMenu
            };
        }

        public void Dispose()
        {
            App.MainIcon?.Dispose();
        }
    }
}
