using System;
using System.IO;
using OS_Shell.Utilities;

namespace OS_Shell.DirectoryManagement
{
    public class CopyCommand : Command
    {
        public void Execute(string[] args)
        {
            try
            {
                // Validate minimum arguments (command + source file required)
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.SOURCE_IS_EMPTY_ERROR);
                }

                string sourceDirectory = args[1];
                string targetDirectory;

                // Handle target directory: either auto-generate or use provided path
                if (args.Length < 3)
                {
                    // No target specified - create copy in current directory with "-copy" suffix
                    string sourceFileDirectory = Path.Combine(DirectoryManagementCommands.CurrentDirectory, args[1]);
                    string sourceFileName = Path.GetFileNameWithoutExtension(sourceFileDirectory);
                    string sourceFileType = Path.GetExtension(sourceFileDirectory);

                    targetDirectory = Path.Combine(DirectoryManagementCommands.CurrentDirectory, $"{sourceFileName}-copy{sourceFileType}");
                }
                else
                {
                    // Target directory specified - validate format and existence
                    if (!args[2].Contains(@":\"))
                    {
                        throw new Exception(ErrorMessages.INVALID_DIRECTORY_ERROR);
                    }

                    if (!Directory.Exists(args[2]))
                    {
                        throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_NOT_EXIST_ERROR);
                    }

                    targetDirectory = Path.Combine(args[2], args[1]);
                }

                // Convert source path to absolute path if it's relative
                if (!args[1].Contains(@":\"))
                {
                    sourceDirectory = Path.Combine(DirectoryManagementCommands.CurrentDirectory, args[1]);
                }

                // Validate source file exists before attempting copy
                if (!File.Exists(sourceDirectory))
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_NOT_EXIST_ERROR);
                }

                // Perform file copy with overwrite enabled
                File.Copy(sourceDirectory, targetDirectory, true);

                PrintExecuteCompleted($"File copied successfully in \"{targetDirectory}\".");
            }
            catch (Exception e)
            {
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
