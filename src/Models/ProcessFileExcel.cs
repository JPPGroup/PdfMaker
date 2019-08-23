using System;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using PdfMaker.Helpers;

namespace PdfMaker.Models
{
    internal class ProcessFileExcel : ProcessFile
    {
        public ProcessFileExcel(string filePath) : base(filePath, FileTypes.Excel) { }

        public override bool IsValid()
        {
            var validExtensions = new[] { ".xls", ".xlsx" };
            return validExtensions.Contains(FileExtension);
        }

        public override void ExportToPdf()
        {
            ApplicationClass excelApplication = null;
            Workbook excelDocument = null;
            string paramExportFilePath = TempFileHelper.GetPdfFileName();

            try
            {
                excelApplication = new ApplicationClass();

                excelDocument = excelApplication.Workbooks.Open(FullPath);
                excelDocument?.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, paramExportFilePath);

                PdfFile = paramExportFilePath;
                CleanUp = true;
            }
            catch (Exception ex)
            {
                // Respond to the error
            }
            finally
            {

                if (excelDocument != null)
                {
                    excelDocument.Close(false);
                    excelDocument = null;
                }

                if (excelApplication != null)
                {
                    excelApplication.Quit();
                    excelApplication = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
