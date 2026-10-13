using System;
using System.IO;
using System.Collections.Generic;
using OS_Shell.Utilities;

namespace OS_Shell.DirectoryManagement
{
    public class CutCommand : Command
    {
        private readonly Dictionary<string, string> argumentAliases = new Dictionary<string, string>
        {
            ["current"] = "current",
            ["now"] = "current",
        };

        public void Execute(string[] args)
        {
            try
            {
                // Validate source path argument is provided
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.SOURCE_IS_EMPTY_ERROR);
                }

                // Validate target path argument is provided
                if (args.Length < 3)
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_EMPTY_ERROR);
                }

                // Resolve source and target paths, combining with current directory if needed
                string sourcePath = DirectoryManagementCommands.CombinePathIfParam2IsFileOrFolder(DirectoryManagementCommands.CurrentDirectory, args[1]);
                string targetPath = DirectoryManagementCommands.CombinePathIfParam2IsFileOrFolder(args[2], Path.GetFileName(args[1]));

                // Resolve target argument aliases (e.g., "current" or "now" → "current")
                string arg2 = GetArgumentFromAliases(argumentAliases, args[2]);

                // If target is the "current" alias, set target path to current directory
                if (string.Equals(arg2, "current"))
                {
                    targetPath = Path.Combine(DirectoryManagementCommands.CurrentDirectory, Path.GetFileName(args[1]));
                }

                // Validate that source and target paths are not the same
                if (string.Equals(sourcePath, targetPath))
                {
                    throw new Exception(ErrorMessages.SAME_DIRECTORY_ERROR);
                }

                // Validate that target directory exists (skip if using "current" alias)
                if (!Directory.Exists(args[2]) && !string.Equals(arg2, "current"))
                {
                    throw new Exception(ErrorMessages.TARGET_DIRECTORY_IS_NOT_EXIST_ERROR);
                }

                // Determine if source is a file or directory
                FileAttributes sourceAtt = File.GetAttributes(args[1]);

                if (sourceAtt.HasFlag(FileAttributes.Directory))
                {
                    // Source is a directory - validate it exists and move it
                    if (!Directory.Exists(sourcePath))
                    {
                        throw new Exception(ErrorMessages.SOURCE_IS_NOT_EXIST_ERROR);
                    }

                    Directory.Move(sourcePath, targetPath);
                }
                else
                {
                    // Source is a file - validate it exists and move it
                    if (!File.Exists(sourcePath))
                    {
                        throw new Exception(ErrorMessages.SOURCE_IS_NOT_EXIST_ERROR);
                    }

                    File.Move(sourcePath, targetPath);
                }

                // Report successful completion
                PrintExecuteCompleted($"{Path.GetFileName(args[1])} was moved to \"{Directory.GetParent(targetPath)}\".");
            }
            catch (Exception e)
            {
                // Report execution failure
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
