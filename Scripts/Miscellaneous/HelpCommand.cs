using System;
using System.IO;
using System.Diagnostics;
using OS_Shell.Properties;
using OS_Shell.Utilities;

namespace OS_Shell.Miscellaneous
{
    public class HelpCommand : Command
    {
        public void Execute()
        {
            try
            {
                // Get the application's base directory and construct the PDF resource path
                string buildPath = AppDomain.CurrentDomain.BaseDirectory;
                string pdfPath = Path.Combine(buildPath, @"Resources\document.pdf");

                // Extract the embedded PDF to disk if it doesn't already exist
                if (!File.Exists(pdfPath))
                {
                    Directory.CreateDirectory(Path.Combine(buildPath, "Resources"));
                    byte[] pdfBytes = Resources.document;
                    File.WriteAllBytes(pdfPath, pdfBytes);
                }

                // Configure and launch the PDF with the default system viewer
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                };

                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
