using System;
using System.IO;
using System.Diagnostics;
using OS_Shell.Utilities;

namespace OS_Shell.DirectoryManagement
{
    public class EditCommand : Command
    {
        public void Execute(string[] args)
        {
            try
            {
                //Validates that a file path argument has been provided.
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_EMPTY_ERROR);
                }

                //Combines the current directory with the provided file/folder parameter to construct the full file path.
                string path = DirectoryManagementCommands.CombinePathIfParam2IsFileOrFolder(DirectoryManagementCommands.CurrentDirectory, args[1]);

                // Verifies that the file exists at the specified path before attempting to open it.
                if (!File.Exists(path))
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_NOT_EXIST_ERROR);
                }

                //Notifies the user that Notepad is being launched.
                PrintNotify("Openning notepad...");
                
                //Launches Notepad application with the specified file path.
                Process.Start("notepad.exe", path);
            }
            catch (Exception e)
            {
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
