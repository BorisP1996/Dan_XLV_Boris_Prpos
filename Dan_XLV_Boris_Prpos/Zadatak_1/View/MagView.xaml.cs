using System.Windows;
using Zadatak_1.ViewModel;

namespace Zadatak_1.View
{
    /// <summary>
    /// Interaction logic for MagView.xaml
    /// </summary>
    public partial class MagView : Window
    {
        public MagView()
        {
            InitializeComponent();
            this.DataContext = new MagViewModel(this);
        }
    }
}
