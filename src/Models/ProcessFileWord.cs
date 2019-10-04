using Microsoft.Office.Interop.Word;
using PdfMaker.Helpers;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace PdfMaker.Models
{
    internal class ProcessFileWord : ProcessFileBase
    {
        public ProcessFileWord(string filePath) : base(filePath, FileTypes.Word) { }

        public override bool IsValid()
        {
            var validExtensions = new[] { ".doc", ".docx" };
            return validExtensions.Contains(FileExtension.ToLower());
        }

        public override void ExportToPdf()
        {
            ApplicationClass wordApplication = null;
            Document wordDocument = null;
            
            object paramSourceDocPath = FullPath;
            var paramMissing = Type.Missing;

            try
            {
                var paramExportFilePath = TempFileHelper.GetPdfFileName();
                wordApplication = new ApplicationClass();

                wordDocument = wordApplication.Documents.Open(
                    ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                    ref paramMissing, ref paramMissing, ref paramMissing,
                    ref paramMissing, ref paramMissing, ref paramMissing,
                    ref paramMissing, ref paramMissing, ref paramMissing,
                    ref paramMissing, ref paramMissing, ref paramMissing,
                    ref paramMissing);

                wordDocument?.ExportAsFixedFormat(paramExportFilePath,
                    EXPORT_FORMAT, OPEN_AFTER_EXPORT,
                    EXPORT_OPTIMIZE_FOR, EXPORT_RANGE, START_PAGE,
                    END_PAGE, EXPORT_ITEM, INCLUDE_DOC_PROPS,
                    KEEP_IRM, CREATE_BOOKMARKS, DOC_STRUCTURE_TAGS,
                    BITMAP_MISSING_FONTS, USE_ISO19005_1,
                    ref paramMissing);

                PdfFile = paramExportFilePath;
                CleanUp = true;
            }
            catch (Exception)
            {
                //TODO : Log error
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(false);
                    Marshal.ReleaseComObject(wordDocument);
                }

                if (wordApplication != null)
                {
                    wordApplication.Quit(false);
                    Marshal.ReleaseComObject(wordApplication);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private const WdExportFormat EXPORT_FORMAT = WdExportFormat.wdExportFormatPDF;
        private const bool OPEN_AFTER_EXPORT = false;
        private const WdExportOptimizeFor EXPORT_OPTIMIZE_FOR = WdExportOptimizeFor.wdExportOptimizeForPrint;
        private const WdExportRange EXPORT_RANGE = WdExportRange.wdExportAllDocument;
        private const int START_PAGE = 0;
        private const int END_PAGE = 0;
        private const WdExportItem EXPORT_ITEM = WdExportItem.wdExportDocumentContent;
        private const bool INCLUDE_DOC_PROPS = true;
        private const bool KEEP_IRM = true;
        private const WdExportCreateBookmarks CREATE_BOOKMARKS = WdExportCreateBookmarks.wdExportCreateWordBookmarks;
        private const bool DOC_STRUCTURE_TAGS = true;
        private const bool BITMAP_MISSING_FONTS = true;
        private const bool USE_ISO19005_1 = false;
    }
}
