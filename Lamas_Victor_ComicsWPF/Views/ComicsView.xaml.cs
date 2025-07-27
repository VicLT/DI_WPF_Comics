using Lamas_Victor_ComicsWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lamas_Victor_ComicsWPF.Views
{
    /// <summary>
    /// Lógica de interacción para ComicsView.xaml
    /// </summary>
    public partial class ComicsView : UserControl
    {
        public ComicsView()
        {
            InitializeComponent();
        }

        private void txtBuscar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscar.Text == "Buscar por cómic, autor y/o editorial.")
            {
                txtBuscar.Text = "";
                txtBuscar.Foreground = Brushes.Black;
            }
        }

        private void txtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = "Buscar por cómic, autor y/o editorial.";
                txtBuscar.Foreground = Brushes.Gray;
            }
            else
            {
                txtBuscar.Foreground = Brushes.Black;
            }
        }
    }
}
