using System.IO;

namespace OS_Shell.DirectoryManagement
{
    public class DirectoryManagementCommands
    {
        #region Variables
        // Command implementation for changing the current directory.
        private readonly ChangeDirectoryCommand _changeDirectoryCommand;

        // Command implementation for listing files and folders in a directory.
        private readonly ListItemsCommand _listItemsCommand;

        // Command implementation for creating new files or folders.
        private readonly CreateCommand _createCommand;

        // Command implementation for deleting files or folders.
        private readonly DeleteCommand _deleteCommand;

        // Command implementation for copying files to a target directory.
        private readonly CopyCommand _copyCommand;

        // Command implementation for editing file contents.
        private readonly EditCommand _editCommand;

        // Command implementation for cutting (moving) files or folders.
        private readonly CutCommand _cutCommand;

        // Command implementation for opening files.
        private readonly OpenCommand _openCommand;

        // Tracks the current working directory. Initialized to the application's starting directory and updated when directory change commands are executed.
        public static string CurrentDirectory = Directory.GetCurrentDirectory();

        #endregion

        #region Main Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryManagementCommands"/> class.
        /// Creates and instantiates all directory command implementations that are used throughout the class.
        /// </summary>
        public DirectoryManagementCommands()
        {
            _changeDirectoryCommand = new ChangeDirectoryCommand();
            _listItemsCommand = new ListItemsCommand();
            _createCommand = new CreateCommand();
            _deleteCommand = new DeleteCommand();
            _copyCommand = new CopyCommand();
            _editCommand = new EditCommand();
            _cutCommand = new CutCommand();
            _openCommand = new OpenCommand();
        }

        /// <summary>
        /// Executes a command to change the current working directory to a specified target.
        /// </summary>
        /// <param name="args">Command arguments where args[0] is the target directory path.</param>
        public void ExecuteChangeDirectoryCommand(string[] args)
        {
            _changeDirectoryCommand.Execute(args);
        }

        /// <summary>
        /// Executes a command to list all files and folders in the current directory.
        /// </summary>
        /// <param name="args">Command arguments where args[0] is the target directory to list.</param>
        public void ExecuteListItemsCommand(string[] args)
        {
            _listItemsCommand.Execute(args);
        }

        /// <summary>
        /// Executes a command to create a new file or folder in the current directory.
        /// </summary>
        /// <param name="args">Command arguments where args[0] specifies the file type and args[1] specifies the file's display name.</param>
        public void ExecuteCreateCommand(string[] args)
        {
            _createCommand.Execute(args);
        }

        /// <summary>
        /// Executes a command to delete a file or folder from the current directory.
        /// </summary>
        /// <param name="args">Command arguments where args[0] is the file or folder name to delete.</param>
        public void ExecuteDeleteCommand(string[] args)
        {
            _deleteCommand.Execute(args);
        }

        /// <summary>
        /// Executes a command to copy a file from the source to a target directory.
        /// </summary>
        /// <param name="args">Command arguments where args[0] is the source file path and args[1] is the target directory path.</param>
        public void ExecuteCopyCommand(string[] args)
        {
            _copyCommand.Execute(args);
        }

        /// <summary>
        /// Executes a command to edit the contents of a file in a specified directory.
        /// </summary>
        /// <param name="args">Command arguments where args[0] is the target directory containing the file to edit.</param>
        public void ExecuteEditCommand(string[] args)
        {
            _editCommand.Execute(args);
        }

        /// <summary>
        /// Executes a command to cut (move) a file or folder to a target directory.
        /// </summary>
        /// <param name="args">Command arguments where args[0] is the source file or folder path and args[1] is the target directory path.</param>
        public void ExecuteCutCommand(string[] args)
        {
            _cutCommand.Execute(args);
        }

        /// <summary>
        /// Executes a command to open a file from the current directory.
        /// </summary>
        /// <param name="args">Command arguments for the file to open. The specific argument requirements depend on the file type.</param>
        public void ExecuteOpenCommand(string[] args)
        {
            _openCommand.Execute(args);
        }
        #endregion

        #region Reusable Methods
        /// <summary>
        /// Normalizes a directory path to ensure it conforms to the standard Windows directory format.
        /// Specifically, if the path ends with a colon (e.g., "C:"), it appends a backslash to create a valid drive reference (e.g., "C:\").
        /// </summary>
        /// <param name="directory">The directory path to normalize.</param>
        /// <returns>The normalized directory path in the correct form.</returns>
        public static string FixDirectoryPath(string directory)
        {
            if (directory.EndsWith(":"))
            {
                directory = directory.Replace(":", @":\");
            }

            return directory;
        }

        /// <summary>
        /// Intelligently combines two path parameters based on whether the second parameter is an absolute path or a relative file/folder name.
        /// If the second parameter is already a complete directory path (contains a drive letter), it is returned as-is.
        /// If the second parameter is a file or folder name, it is combined with the first parameter to create a full path.
        /// Both parameters are normalized before comparison and combination.
        /// </summary>
        /// <param name="dir">The base directory path to combine with (if needed).</param>
        /// <param name="notSureDir">The second path parameter, which may be either an absolute path or a relative file/folder name.</param>
        /// <returns>The second parameter if it is already a complete directory path; otherwise, a combined path created by joining the first and second parameters.</returns>
        public static string CombinePathIfParam2IsFileOrFolder(string dir, string notSureDir)
        {
            dir = FixDirectoryPath(dir);
            notSureDir = FixDirectoryPath(notSureDir);

            if (notSureDir.Contains(@":\"))
            {
                return notSureDir;
            }

            return Path.Combine(dir, notSureDir);
        }
        #endregion
    }
}
