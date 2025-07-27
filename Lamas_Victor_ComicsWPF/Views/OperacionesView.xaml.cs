using Lamas_Victor_ComicsWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lamas_Victor_ComicsWPF.Views
{
    /// <summary>
    /// Lógica de interacción para OperacionesView.xaml
    /// </summary>
    public partial class OperacionesView : UserControl
    {
        public OperacionesView()
        {
            InitializeComponent();
            DataContext = new OperacionesViewModel();
        }

        private void txtBuscarCliente_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscarCliente.Text == "Buscar por nombre, apellidos, NIF y/o dirección.")
            {
                txtBuscarCliente.Text = "";
                txtBuscarCliente.Foreground = Brushes.Black;
            }
        }

        private void txtBuscarCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscarCliente.Text))
            {
                txtBuscarCliente.Text = "Buscar por nombre, apellidos, NIF y/o dirección.";
                txtBuscarCliente.Foreground = Brushes.Gray;
            }
            else
            {
                txtBuscarCliente.Foreground = Brushes.Black;
            }
        }
    }
}
