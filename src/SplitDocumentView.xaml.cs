using System.Windows;

namespace PdfMaker
{
    /// <summary>
    /// Interaction logic for SplitDocumentView.xaml
    /// </summary>
    public partial class SplitDocumentView : Window
    {
        public SplitDocumentView()
        {
            InitializeComponent();
            DataContext = new SplitDocumentViewModel(this);
        }
    }
}
