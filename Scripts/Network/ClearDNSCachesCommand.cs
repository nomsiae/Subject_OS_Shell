using System;
using System.Diagnostics;
using OS_Shell.Utilities;

namespace OS_Shell.Network
{
    public class ClearDNSCachesCommand : Command
    {
        public void Execute()
        {
            try
            {
                // Configure cmd.exe to execute ipconfig /flushdns command and capture the output
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = true,
                    Arguments = "/c \"ipconfig /flushdns\""
                };

                // Execute the DNS cache flush command and display the result to the user
                using (Process process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();

                    PrintNotify(output);
                }
            }
            catch (Exception e)
            {
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
