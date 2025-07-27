using Lamas_Victor_ComicsWPF.ViewModels;
using System.Windows.Controls;

namespace Lamas_Victor_ComicsWPF.Views
{
    /// <summary>
    /// Lógica de interacción para EstadisticasView.xaml
    /// </summary>
    public partial class EstadisticasView : UserControl
    {
        public EstadisticasView()
        {
            InitializeComponent();
            DataContext = new EstadisticasViewModel();
        }
    }
}
