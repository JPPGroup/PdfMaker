using System;
using System.IO;
using System.Linq;
using System.Windows;
using Jpp.Common;
using PdfMaker.Helpers;

namespace PdfMaker.Models
{
    internal abstract class ProcessFile : BaseNotify
    {
        private bool _isSplit;
        private int _startPage;
        private int _endPage;

        public string DisplayFile => IsSplit ? $"*{FileName}" : FileName;
        public string DisplayPage => IsSplit ? $"{(StartPage == 0 ? "*" : StartPage.ToString())} - {(EndPage == 0 ? "*" : EndPage.ToString())}" : "";
        public Visibility IsPageInfoVisible => IsSplit ? Visibility.Visible : Visibility.Collapsed;

        public string FileName { get; }
        public string FileExtension { get; }
        public string FullPath { get; }
        public FileTypes FileType { get; }
        public string PdfFile { get; protected set; }
        public bool CleanUp { get; protected set; }
        public bool IsSplit
        {
            get => _isSplit;
            set
            {
                SetField(ref _isSplit, value, nameof(IsSplit));

                OnPropertyChanged(nameof(DisplayFile));
                OnPropertyChanged(nameof(IsPageInfoVisible));
            } 
        }
        public int StartPage
        {
            get => _startPage;
            set
            {
                SetField(ref _startPage, value, nameof(StartPage));

                OnPropertyChanged(nameof(DisplayPage));
            }
        }
        public int EndPage
        {
            get => _endPage;
            set
            {
                SetField(ref _endPage, value, nameof(EndPage));

                OnPropertyChanged(nameof(DisplayPage));
            }
        }

        protected ProcessFile(string filePath, FileTypes type)
        {
            if (!File.Exists(filePath)) throw new ArgumentException(nameof(filePath));

            FullPath = filePath;
            FileName = Path.GetFileName(filePath);
            FileExtension = Path.GetExtension(filePath);
            FileType = type;
            PdfFile = string.Empty;
            CleanUp = false;
        }

        public static ProcessFile Create(string filePath)
        {
            var objects = ReflectiveEnumerator.GetEnumerableOfType<ProcessFile>(filePath);

            return objects.FirstOrDefault(obj => obj.IsValid());
        }

        public abstract void ExportToPdf();
        public abstract bool IsValid();
    }

    public enum FileTypes { Word, Excel, Pdf }
}
