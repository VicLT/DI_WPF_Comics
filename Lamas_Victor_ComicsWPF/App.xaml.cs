using System.Windows;

namespace Lamas_Victor_ComicsWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded)
                {
                    MainView mainView = new MainView();
                    mainView.Show();
                    loginView.Close();
                }
            };
        }*/
    }
}
