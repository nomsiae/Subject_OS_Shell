using System;
using System.IO;
using System.Diagnostics;
using OS_Shell.Utilities;

namespace OS_Shell.DirectoryManagement
{
    public class OpenCommand : Command
    {
        public void Execute(string[] args)
        {
            try
            {
                // Validate that required arguments are provided
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_EMPTY_ERROR);
                }

                // Resolve the full path combining current directory with target parameter
                string path = DirectoryManagementCommands.CombinePathIfParam2IsFileOrFolder(DirectoryManagementCommands.CurrentDirectory, args[1]);

                // Verify the file exists before attempting to open
                if (!File.Exists(path))
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_NOT_EXIST_ERROR);
                }

                // Configure process information to open the file with default application
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true  // Allows OS to determine the default application
                };

                PrintNotify($"Openning {Path.GetFileName(args[1])}...");
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                // Display error message to user without propagating exception
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
