using Jpp.Common;
using System.Windows;

namespace PdfMaker
{
    internal class ProgressViewModel : BaseNotify
    {
        private readonly Window _view;
        private string _progressText = string.Empty;

        public string ProgressText
        {
            get => _progressText;
            set => SetField(ref _progressText, value, nameof(ProgressText));
        }

        public ProgressViewModel(Window view)
        {
            _view = view;
        }
    }
}
