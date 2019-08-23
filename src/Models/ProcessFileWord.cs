using System;
using System.Linq;
using Microsoft.Office.Interop.Word;
using PdfMaker.Helpers;

namespace PdfMaker.Models
{
    internal class ProcessFileWord : ProcessFile
    {
        public ProcessFileWord(string filePath) : base(filePath, FileTypes.Word) { }

        public override bool IsValid()
        {
            var validExtensions = new[] { ".doc", ".docx" };
            return validExtensions.Contains(FileExtension);
        }

        public override void ExportToPdf()
        {
            ApplicationClass wordApplication = null;
            Document wordDocument = null;
            object paramSourceDocPath = FullPath;
            var paramMissing = Type.Missing;

            var paramExportFilePath = TempFileHelper.GetPdfFileName();

            try
            {

                wordApplication = new ApplicationClass();

                wordDocument = wordApplication.Documents.Open(
                    ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                    ref paramMissing, ref paramMissing, ref paramMissing,
                    ref paramMissing, ref paramMissing, ref paramMissing,
                    ref paramMissing, ref paramMissing, ref paramMissing,
                    ref paramMissing, ref paramMissing, ref paramMissing,
                    ref paramMissing);

                wordDocument?.ExportAsFixedFormat(paramExportFilePath,
                    PARAM_EXPORT_FORMAT, PARAM_OPEN_AFTER_EXPORT,
                    PARAM_EXPORT_OPTIMIZE_FOR, PARAM_EXPORT_RANGE, PARAM_START_PAGE,
                    PARAM_END_PAGE, PARAM_EXPORT_ITEM, PARAM_INCLUDE_DOC_PROPS,
                    PARAM_KEEP_IRM, PARAM_CREATE_BOOKMARKS, PARAM_DOC_STRUCTURE_TAGS,
                    PARAM_BITMAP_MISSING_FONTS, PARAM_USE_ISO19005_1,
                    ref paramMissing);

                PdfFile = paramExportFilePath;
                CleanUp = true;
            }
            catch (Exception ex)
            {
                // Respond to the error
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(false);
                    wordDocument = null;
                }

                if (wordApplication != null)
                {
                    wordApplication.Quit(false);
                    wordApplication = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private const WdExportFormat PARAM_EXPORT_FORMAT = WdExportFormat.wdExportFormatPDF;
        private const bool PARAM_OPEN_AFTER_EXPORT = false;
        private const WdExportOptimizeFor PARAM_EXPORT_OPTIMIZE_FOR = WdExportOptimizeFor.wdExportOptimizeForPrint;
        private const WdExportRange PARAM_EXPORT_RANGE = WdExportRange.wdExportAllDocument;
        private const int PARAM_START_PAGE = 0;
        private const int PARAM_END_PAGE = 0;
        private const WdExportItem PARAM_EXPORT_ITEM = WdExportItem.wdExportDocumentContent;
        private const bool PARAM_INCLUDE_DOC_PROPS = true;
        private const bool PARAM_KEEP_IRM = true;
        private const WdExportCreateBookmarks PARAM_CREATE_BOOKMARKS = WdExportCreateBookmarks.wdExportCreateWordBookmarks;
        private const bool PARAM_DOC_STRUCTURE_TAGS = true;
        private const bool PARAM_BITMAP_MISSING_FONTS = true;
        private const bool PARAM_USE_ISO19005_1 = false;
    }
}
