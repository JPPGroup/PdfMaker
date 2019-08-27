using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Collections.Generic;
using System.IO;

namespace PdfMaker.Models
{
    internal class PdfMerge
    {
        private const string FILE_NAME = "MergedDocument.pdf";
        public List<ProcessFileBase> Files { get; }

        public PdfMerge(List<ProcessFileBase> files)
        {
            Files = files;
        }

        public void Merge(string folder)
        {
            var outputDocument = new PdfDocument();
            foreach (var file in Files)
            {
                if (!File.Exists(file.PdfFile)) continue;

                var inputDocument = PdfReader.Open(file.PdfFile, PdfDocumentOpenMode.Import);
                var start = GetStartIndex(inputDocument, file);
                var end = GetEndIndex(inputDocument, file);

                for (var i = start; i <= end; i++)
                {
                    outputDocument.AddPage(inputDocument.Pages[i]);
                }

                if (file.CleanUp) File.Delete(file.PdfFile);
            }

            outputDocument.Save(Path.Combine(folder, FILE_NAME));
        }

        private static int GetEndIndex(PdfDocument inputDocument, ProcessFileBase file)
        {
            if(!file.IsSplit || file.EndPage > inputDocument.PageCount || file.EndPage <= 0) return inputDocument.PageCount - 1;
            if (file.EndPage < file.StartPage) return file.StartPage - 1;

            return file.EndPage -1;
        }

        private static int GetStartIndex(PdfDocument inputDocument, ProcessFileBase file)
        {
            if (!file.IsSplit || file.StartPage <= 0) return 0;
            if (file.StartPage >= inputDocument.PageCount) return inputDocument.PageCount - 1;

            return file.StartPage - 1;
        }
    }
}
