using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.ViewModel;

namespace Zadatak_1.View
{
    /// <summary>
    /// Interaction logic for ManView.xaml
    /// </summary>
    public partial class ManView : Window
    {
        public ManView()
        {
            InitializeComponent();
            this.DataContext = new ManViewModel(this);
        }
        private void NumbersOnlyTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void LettersValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
