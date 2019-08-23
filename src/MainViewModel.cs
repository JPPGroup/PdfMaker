using System;
using System.Collections;
using Jpp.Common;
using PdfMaker.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace PdfMaker
{
    internal class MainViewModel : BaseNotify
    {
        private readonly Window _view;
        private string _selectedFolder = string.Empty;
        private ObservableCollection<ProcessFile> _filesCollection = new ObservableCollection<ProcessFile>();
        private ObservableCollection<ProcessFile> _toCreateCollection = new ObservableCollection<ProcessFile>();
        private ICommand _selectFolderCommand;
        private ICommand _resetCommand;
        private ICommand _okCommand;
        private ICommand _splitCommand;

        public string SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                SetField(ref _selectedFolder, value, nameof(SelectedFolder));
                ReadFilesFromPath();
            } 
        }
        public ObservableCollection<ProcessFile> FilesCollection
        {
            get => _filesCollection;
            set => SetField(ref _filesCollection, value, nameof(FilesCollection));
        }
        public ObservableCollection<ProcessFile> ToCreateCollection
        {
            get => _toCreateCollection;
            set => SetField(ref _toCreateCollection, value, nameof(ToCreateCollection));
        }
        public ICommand SelectFolderCommand => _selectFolderCommand ?? (_selectFolderCommand = new DelegateCommand(DoFolderSelect));
        public ICommand ResetCommand => _resetCommand ?? (_resetCommand = new DelegateCommand(DoReset));
        public ICommand OkCommand => _okCommand ?? (_okCommand = new DelegateCommand(DoOk));
        public ICommand SplitCommand =>_splitCommand ??(_splitCommand = new DelegateCommand<ProcessFile>(DoSplit));
        
        public MainViewModel(Window view)
        {
            _view = view;
        }

        private void DoSplit(ProcessFile item)
        {
            var split = new SplitDocumentView { Owner = _view };
            split.ShowDialog();

            var context = (SplitDocumentViewModel)split.DataContext;
            if (!context.ShouldSplit) return;

            var newItem = ProcessFile.Create(item.FullPath);
            if (newItem == null) throw new ArgumentException(nameof(item));

            item.IsSplit = true;
            item.EndPage = context.PageNumber;

            newItem.IsSplit = true;
            newItem.StartPage = context.PageNumber + 1;

            ToCreateCollection.Add(newItem);
        }

        private void DoFolderSelect()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectedFolder = dialog.SelectedPath;
            }
        }

        private void DoReset()
        {
            ReadFilesFromPath();
        }

        private void DoOk()
        {
            if (ToCreateCollection.Count < 1) return;

            DoOkWithModal(progress =>
            {
                foreach (var processFile in ToCreateCollection)
                {
                    progress.Report($"Processing {processFile.FileName}");
                    processFile.ExportToPdf();
                }

                progress.Report("Merging documents to PDF");

                var merger = new PdfMerge(ToCreateCollection.ToList());
                merger.Merge(SelectedFolder);
            });
        }

        private void ReadFilesFromPath()
        {
            FilesCollection.Clear();
            ToCreateCollection.Clear();

            if (string.IsNullOrWhiteSpace(SelectedFolder)) return;

            var filePaths = Directory.GetFiles(SelectedFolder);

            foreach (var file in filePaths)
            {
                var procFile = ProcessFile.Create(file);
                if (procFile != null) FilesCollection.Add(procFile);
            }
        }

        private void DoOkWithModal(Action<IProgress<string>> work)
        {
            var splash = new SplashView { Owner = _view };

            splash.Loaded += (_, args) =>
            {
                var worker = new BackgroundWorker();
                var progress = new Progress<string>( data => ((SplashViewModel) splash.DataContext).ProgressText = data);

                worker.DoWork += (s, workerArgs) => work(progress);
                worker.RunWorkerCompleted += (s, workerArgs) => splash.Close();
                worker.RunWorkerAsync();
            };

            splash.ShowDialog();
        }
    }
}
