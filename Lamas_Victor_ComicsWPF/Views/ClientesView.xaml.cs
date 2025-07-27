using Lamas_Victor_ComicsWPF.ViewModels;
using System.Windows.Controls;

namespace Lamas_Victor_ComicsWPF.Views
{
    /// <summary>
    /// Lógica de interacción para ClientesView.xaml
    /// </summary>
    public partial class ClientesView : UserControl
    {
        public ClientesView()
        {
            InitializeComponent();
            DataContext = new ClientesViewModel();
        }
    }
}
