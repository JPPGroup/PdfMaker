using Microsoft.Office.Interop.Excel;
using PdfMaker.Helpers;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace PdfMaker.Models
{
    internal class ProcessFileExcel : ProcessFileBase
    {
        public ProcessFileExcel(string filePath) : base(filePath, FileTypes.Excel) { }

        public override bool IsValid()
        {
            var validExtensions = new[] { ".xls", ".xlsx" };
            return validExtensions.Contains(FileExtension.ToLower());
        }

        public override void ExportToPdf()
        {
            ApplicationClass excelApplication = null;
            Workbook excelDocument = null;
            var paramExportFilePath = TempFileHelper.GetPdfFileName();

            try
            {
                excelApplication = new ApplicationClass();

                excelDocument = excelApplication.Workbooks.Open(FullPath);
                
                excelDocument?.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, paramExportFilePath);

                PdfFile = paramExportFilePath;
                CleanUp = true;
            }
            catch (Exception)
            {
                //TODO : Log error
            }
            finally
            {

                if (excelDocument != null)
                {
                    excelDocument.Close(false);
                    Marshal.ReleaseComObject(excelDocument);
                }

                if (excelApplication != null)
                {
                    excelApplication.Quit();
                    Marshal.ReleaseComObject(excelApplication);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
