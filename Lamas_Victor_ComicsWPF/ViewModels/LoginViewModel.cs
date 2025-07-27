using Lamas_Victor_ComicsWPF.Models;
using Lamas_Victor_ComicsWPF.Services;
using Lamas_Victor_ComicsWPF.ViewModels.Base;
using Lamas_Victor_ComicsWPF.Views;
using System.Windows;
using System.Windows.Input;

/// <author>VÍCTOR LAMAS TURRILLO - 2ºDAM SEMI</author>

namespace Lamas_Victor_ComicsWPF.ViewModels
{
    /// <summary>
    /// ViewModel para la vista de Login.
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        // Fields
        private string nif = "16774086W";
        private string password = string.Empty;
        private string errorMessage = string.Empty;
        private int failedTries = 0;
        private string nifCheckIsVisible = "Hidden";
        private string passCheckIsVisible = "Hidden";
        private string nifErrorIsVisible = "Hidden";
        private string passErrorIsVisible = "Hidden";

        // Properties
        public string Nif
        {
            get { return nif; }
            set { nif = value.ToUpper(); OnPropertyChanged(); }
        }
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; OnPropertyChanged(); }
        }
        public int FailedTries
        {
            get { return failedTries; }
            set { failedTries = value; OnPropertyChanged(); }
        }
        public string NifCheckIsVisible
        {
            get { return nifCheckIsVisible; }
            set { nifCheckIsVisible = value; OnPropertyChanged(); }
        }
        public string PassCheckIsVisible
        {
            get { return passCheckIsVisible; }
            set { passCheckIsVisible = value; OnPropertyChanged(); }
        }
        public string NifErrorIsVisible
        {
            get { return nifErrorIsVisible; }
            set { nifErrorIsVisible = value; OnPropertyChanged(); }
        }
        public string PassErrorIsVisible
        {
            get { return passErrorIsVisible; }
            set { passErrorIsVisible = value; OnPropertyChanged(); }
        }

        // Commands
        public ICommand LoginCommand { get; }
        public ICommand SalirCommand { get; }
        public ICommand LimpiarCommand { get; }

        // Constructor
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(PerformLoginCommand, CanExecuteLoginCommand);
            SalirCommand = new RelayCommand(PerformSalirCommand);
            LimpiarCommand = new RelayCommand(PerformLimpiarCommand);
        }

        /// <summary>
        /// Comprueba si se pueden ejecutar las credenciales.
        /// </summary>
        /// <param name="parameter">(object) Parámetro del comando.</param>
        /// <returns>True si no se cumple ninguna condición.</returns>
        private bool CanExecuteLoginCommand(object? parameter)
        {
            return !(string.IsNullOrWhiteSpace(Nif)
                        || Nif.Length != 9
                        || string.IsNullOrWhiteSpace(Password));
        }

        /// <summary>
        /// Realiza la comprobación de las credenciales.
        /// </summary>
        /// <param name="parameter">(object) Parámetro del comando.</param>
        private void PerformLoginCommand(object? parameter = null)
        {
            Window? loginView = parameter as Window;
            int resultadoComprobacion = -1;

            using (var empleadosService = new EmpleadosService())
            {
                resultadoComprobacion =
                    empleadosService.comprobarCredenciales(nif, password);
            }
            
            // Cambia de ventana o muestra error
            if (resultadoComprobacion == 0)
            {
                ErrorMessage = String.Empty;

                NifCheckIsVisible = "Visible";
                NifErrorIsVisible = "Hidden";
                PassCheckIsVisible = "Visible";
                PassErrorIsVisible = "Hidden";

                if (loginView != null)
                {
                    MessageBox.Show(
                        "¡Bienvenido, " + EmpleadoLogged.Instance?.empleado?.Nombre + "!");
                    MainView mainView = new MainView();
                    mainView.Show();
                    loginView.Close();
                }
            }
            else
            {
                // Mensaje de error
                ErrorMessage = "El NIF o la contraseña introducidos no son correctos.";

                // Cambia la visibilidad de los iconos de comprobación credenciales
                if (resultadoComprobacion == 1)
                {
                    NifCheckIsVisible = "Visible";
                    NifErrorIsVisible = "Hidden";
                    PassCheckIsVisible = "Hidden";
                    PassErrorIsVisible = "Visible";
                }
                else if (resultadoComprobacion == 2)
                {
                    NifCheckIsVisible = "Hidden";
                    NifErrorIsVisible = "Visible";
                    PassCheckIsVisible = "Visible";
                    PassErrorIsVisible = "Hidden";
                }
                else if (resultadoComprobacion == 3)
                {
                    NifCheckIsVisible = "Hidden";
                    NifErrorIsVisible = "Visible";
                    PassCheckIsVisible = "Hidden";
                    PassErrorIsVisible = "Visible";
                }

                // MessageBox de intentos fallidos
                failedTries++;
                if (failedTries < 3 && failedTries > 0)
                {
                    MessageBox.Show(
                        "Intento fallido. Queda/n " + (3 - failedTries) + " intento/s.",
                        "LOGIN ERROR");
                }
                else if (failedTries >= 3)
                {
                    MessageBox.Show(
                        "Se han agotado todos los intentos.",
                        "SALIR",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    Application.Current.Shutdown();
                }
            }
        }

        /// <summary>
        /// Cierra la aplicación.
        /// </summary>
        /// <param name="parameter">(object) Parámetro del comando.</param>
        private void PerformSalirCommand(object? parameter = null)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Limpia los campos de texto y los iconos de comprobación.
        /// </summary>
        /// <param name="parameter">(object) Parámetro del comando.</param>
        private void PerformLimpiarCommand(object? parameter = null)
        {
            Nif = String.Empty;
            ErrorMessage = String.Empty;

            NifCheckIsVisible = "Hidden";
            PassCheckIsVisible = "Hidden";
            NifErrorIsVisible = "Hidden";
            PassErrorIsVisible = "Hidden";

            // Accede al PasswordBox y limpia su contenido
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Application.Current.Windows.OfType<Views.LoginView>()
                    .FirstOrDefault() is Views.LoginView loginView)
                {
                    loginView.pbPass.Clear();
                }
            });
        }
    }
}
