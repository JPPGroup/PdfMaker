using System.Windows;
using Jpp.Common;

namespace PdfMaker
{
    internal class SplashViewModel : BaseNotify
    {
        private readonly Window _view;
        private string _progressText = string.Empty;

        public string ProgressText
        {
            get => _progressText;
            set => SetField(ref _progressText, value, nameof(ProgressText));
        }

        public SplashViewModel(Window view)
        {
            _view = view;
        }
    }
}
