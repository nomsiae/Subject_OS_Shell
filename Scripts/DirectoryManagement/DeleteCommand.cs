using System;
using System.IO;
using OS_Shell.Utilities;

namespace OS_Shell.DirectoryManagement
{
    public class DeleteCommand : Command
    {
        private const string CONFIRM_DELETION_TEXT = "Are sure to remove %target% in %directory%";

        public void Execute(string[] args)
        {
            try
            {
                // Validate that the required argument (target path) is provided
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_EMPTY_ERROR);
                }

                // Construct the full path to the target file or directory
                string path = DirectoryManagementCommands.CombinePathIfParam2IsFileOrFolder(DirectoryManagementCommands.CurrentDirectory, args[1]);

                // Determine if the target is a file or directory by checking its attributes
                FileAttributes pathAtt = File.GetAttributes(path);
                bool isFolder = pathAtt.HasFlag(FileAttributes.Directory);

                // Build and personalize the deletion confirmation message with placeholders
                string confirmDeletion = CONFIRM_DELETION_TEXT;
                confirmDeletion = confirmDeletion.Replace("%target%", Path.GetFileName(args[1]));
                confirmDeletion = confirmDeletion.Replace("%directory%", $"\"{path}\"");

                // Prompt user for confirmation; abort if user declines
                if (!YesNoOption(confirmDeletion))
                {
                    return;
                }

                // Delete directory if target is a folder
                if (isFolder)
                {
                    if (!Directory.Exists(path))
                    {
                        throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_NOT_EXIST_ERROR);
                    }

                    Directory.Delete(path);
                }
                // Delete file if target is a file
                else
                {
                    if (!File.Exists(path))
                    {
                        throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_NOT_EXIST_ERROR);
                    }

                    File.Delete(path);
                }

                // Report successful completion to user
                PrintExecuteCompleted($"{args[1]} is removed.");
            }
            catch (Exception e)
            {
                // Report any errors encountered during execution
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
