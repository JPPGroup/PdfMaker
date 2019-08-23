using System.Linq;

namespace PdfMaker.Models
{
    internal class ProcessFilePdf : ProcessFile
    {
        public ProcessFilePdf(string filePath) : base(filePath, FileTypes.Pdf) { }

        public override bool IsValid()
        {
            var validExtensions = new[] { ".pdf" };
            return validExtensions.Contains(FileExtension);
        }

        public override void ExportToPdf()
        {
            PdfFile = FullPath;
            CleanUp = false;
        }
    }
}
