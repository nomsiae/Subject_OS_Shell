using System;
using System.IO;
using System.Collections.Generic;
using OS_Shell.Utilities;

namespace OS_Shell.DirectoryManagement
{
    public class ChangeDirectoryCommand : Command
    {
        private readonly Dictionary<string, string> argumentAliases = new Dictionary<string, string>
        {
            ["reset"] = "reset",
            ["r"] = "reset",
            ["current"] = "current",
            ["now"] = "current",
            ["parent"] = "parent"
        };
        // Stores the default directory path (typically the application startup directory). Used when the user invokes the "reset" command.
        private readonly string DEFAULT_DIRECTORY = Directory.GetCurrentDirectory();

        public void Execute(string[] args)
        {
            try
            {
                // Error Handling
                // Check if target directory is empty
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_EMPTY_ERROR);
                }

                string arg1 = GetArgumentFromAliases(argumentAliases, args[1].ToLower());

                // Check if user type specific argument: reset
                // Reset directory to default
                if (string.Equals(arg1, "reset"))
                {
                    DirectoryManagementCommands.CurrentDirectory = DEFAULT_DIRECTORY;
                    PrintNotify($"Directory is now reset to default: \"{DEFAULT_DIRECTORY}\".");
                    return;
                }

                // Check if user type specific argument: current
                // Show current directory
                if (string.Equals(arg1, "current"))
                {
                    PrintNotify($"Current directory is \"{DirectoryManagementCommands.CurrentDirectory}\".");
                    return;
                }

                // Check if user type specific argument: parent
                // Change directory to parent
                if (string.Equals(arg1, "parent"))
                {
                    string parentPath = Path.GetDirectoryName(DirectoryManagementCommands.CurrentDirectory);
                    args[1] = Path.GetFullPath(parentPath);
                    Execute(args);
                    return;
                }

                if (args[1].EndsWith(":"))
                {
                    args[1] = args[1].Replace(":", @":\");
                }

                string path = Path.Combine(DirectoryManagementCommands.CurrentDirectory, args[1]);

                // Check if target directory is not exist
                if (!Directory.Exists(Path.GetFullPath(path)))
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_NOT_EXIST_ERROR);
                }

                // Set current directory to arg1
                DirectoryManagementCommands.CurrentDirectory = path;
                // Display 
                PrintExecuteCompleted($"Directory is now set to \"{DirectoryManagementCommands.CurrentDirectory}\".");
            }
            catch (Exception e)
            {
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
