using System.Linq;

namespace PdfMaker.Models
{
    internal class ProcessFilePdf : ProcessFileBase
    {
        public ProcessFilePdf(string filePath) : base(filePath, FileTypes.Pdf) { }

        public override bool IsValid()
        {
            var validExtensions = new[] { ".pdf" };
            return validExtensions.Contains(FileExtension.ToLower());
        }

        public override void ExportToPdf()
        {
            PdfFile = FullPath;
            CleanUp = false;
        }
    }
}
