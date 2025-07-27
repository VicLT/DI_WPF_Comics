using Lamas_Victor_ComicsWPF.ViewModels;
using System.Windows.Controls;

namespace Lamas_Victor_ComicsWPF.Views
{
    /// <summary>
    /// Lógica de interacción para StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        public StockView()
        {
            InitializeComponent();
            DataContext = new StockViewModel();
        }
    }
}
