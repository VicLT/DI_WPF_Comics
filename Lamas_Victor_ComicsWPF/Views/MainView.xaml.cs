using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Lamas_Victor_ComicsWPF.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void panelControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                WindowInteropHelper helper = new WindowInteropHelper(this);
                SendMessage(helper.Handle, 161,2,0);
            }
        }
        private void panelControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "¿Seguro que desea salir?",
                "SALIR",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }
    }
}