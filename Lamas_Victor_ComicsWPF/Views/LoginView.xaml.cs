using Lamas_Victor_ComicsWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lamas_Victor_ComicsWPF.Views
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void pbPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((LoginViewModel)this.DataContext).Password =
                    ((PasswordBox)sender).Password;
            }
        }

        private void pbPass_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((LoginViewModel)this.DataContext).Password =
                    ((PasswordBox)sender).Password;
            }
        }
    }
}
