using Lamas_Victor_ComicsWPF.ViewModels;
using System.Windows.Controls;

namespace Lamas_Victor_ComicsWPF.Views
{
    /// <summary>
    /// Lógica de interacción para InicioView.xaml
    /// </summary>
    public partial class InicioView : UserControl
    {
        public InicioView()
        {
            InitializeComponent();
            DataContext = new InicioViewModel();
        }
    }
}
