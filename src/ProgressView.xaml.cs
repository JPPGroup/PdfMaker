using System.Windows;

namespace PdfMaker
{
    /// <summary>
    /// Interaction logic for ProgressView.xaml
    /// </summary>
    public partial class ProgressView : Window
    {
        public ProgressView()
        {
            InitializeComponent();
            DataContext = new ProgressViewModel(this);
        }
    }
}
