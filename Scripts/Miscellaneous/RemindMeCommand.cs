using System;
using System.Threading.Tasks;
using System.Diagnostics;
using OS_Shell.Utilities;

namespace OS_Shell.Miscellaneous
{
    public class RemindMeCommand : Command
    {
        public async void Execute(string[] args)
        {
            try
            {
                // Validate that reminder duration argument is provided
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.REMIND_SECONDS_IS_EMPTY_ERROR);
                }

                // Parse the wait time in seconds; throw exception if not a valid integer
                if (!int.TryParse(args[1], out int waitTime))
                {
                    throw new Exception(ErrorMessages.REMIND_SECONDS_IS_NOT_INTEGER_ERROR);
                }

                // Wait asynchronously for the specified duration (converted to milliseconds)
                await Task.Delay(waitTime * 1000);

                // Launch a command prompt window to display the reminder completion message
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    UseShellExecute = true,
                    CreateNoWindow = false,
                    Arguments = "/K \"echo -----Remind Finished-----\" & pause & exit"
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
