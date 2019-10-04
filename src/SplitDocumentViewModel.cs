using Jpp.Common;
using System.Windows;
using System.Windows.Input;

namespace PdfMaker
{
    internal class SplitDocumentViewModel : BaseNotify
    {
        private readonly Window _view;

        private int _pageNumber;
        private ICommand _okCommand;
        private ICommand _cancelCommand;

        public ICommand OkCommand => _okCommand ??= new DelegateCommand(DoOk);
        public ICommand CancelCommand => _cancelCommand ??= new DelegateCommand(DoCancel);
        public int PageNumber
        {
            get => _pageNumber;
            set => SetField(ref _pageNumber, value, nameof(PageNumber));
        }
        public bool ShouldSplit { get; private set; }

        public SplitDocumentViewModel(Window view)
        {
            _view = view;
        }

        private void DoCancel()
        {
            ShouldSplit = false;
            _view.Close();
        }

        private void DoOk()
        {
            ShouldSplit = true;
            _view.Close();
        }
    }
}
