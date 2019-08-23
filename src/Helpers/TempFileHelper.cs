using System;
using System.IO;

namespace PdfMaker.Helpers
{
    internal static class TempFileHelper
    {
        private const string DATA_FOLDER = @"JPP Group\PdfMaker\";

        public static string GetPdfFileName()
        {
            var appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DATA_FOLDER);
            Directory.CreateDirectory(appDataFolder);

            return Path.Combine(appDataFolder, $"{Guid.NewGuid().ToString()}.pdf");
        }
    }
}
