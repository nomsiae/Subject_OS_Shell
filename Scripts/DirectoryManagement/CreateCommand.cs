using System;
using System.IO;
using OS_Shell.Utilities;

namespace OS_Shell.DirectoryManagement
{
    public class CreateCommand : Command
    {
        public void Execute(string[] args)
        {
            try
            {
                // Validate Arguments
                // Check if file type argument is provided
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.FILE_TYPE_EMPTY_ERROR);
                }

                // Check if display name argument is provided
                if (args.Length < 3)
                {
                    throw new Exception(ErrorMessages.FILE_DISPLAY_NAME_EMPTY_ERROR);
                }

                // Validate display name against reserved keywords and forbidden words
                if (IsStringForbidden(args[2]))
                {
                    throw new Exception(ErrorMessages.NAMING_WITH_FORBIDDEN_WORDS_ERROR);
                }

                // Extract and normalize command parameters
                string fileType = args[1].ToLower();
                string fileDisplayName = args[2];
                string directory = DirectoryManagementCommands.CurrentDirectory;

                // Build the full path for the new file or folder
                string path = Path.Combine(directory, fileDisplayName);
                bool isFolderFileType = string.Equals(fileType, "folder");

                // Determine output message based on item type
                // Folders display as "Folder: <name>"
                // Files display as "<name>.<extension>"
                string fileName = isFolderFileType ? $"Folder: {fileDisplayName}" : $"{fileDisplayName}.{fileType}";
                PrintExecuteCompleted($"{fileName} was created in \"{DirectoryManagementCommands.CurrentDirectory}\".");

                // Create folder and exit early if folder type
                if (isFolderFileType)
                {
                    Directory.CreateDirectory(path);
                    return;
                }

                // Append file extension to path
                path = string.Join(".", path, fileType);

                // Special handling for generic "file" type (no extension)
                // Removes the ".file" extension if file type is literally "file"
                if (string.Equals(fileType, "file"))
                {
                    path = path.Remove(path.Length - fileType.Length, fileType.Length);
                }

                // Create the file at the computed path with full path resolution
                using (File.Create(Path.GetFullPath(path))) { }
            }
            catch (Exception e)
            {
                // Handle any exceptions and display error message to user
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
