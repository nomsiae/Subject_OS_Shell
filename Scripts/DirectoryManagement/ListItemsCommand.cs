using System;
using System.IO;
using System.Collections.Generic;
using OS_Shell.Utilities;

namespace OS_Shell.DirectoryManagement
{
    public class ListItemsCommand : Command
    {
        public void Execute(string[] args)
        {
            try
            {
                // Handle command arguments - if a directory path is provided, use it instead of the current directory
                if (args.Length > 1)
                {
                    // Normalize drive letter arguments (e.g., "C:" becomes "C:\")
                    if (args[1].EndsWith(":"))
                    {
                        args[1] = args[1].Replace(":", @":\");
                    }

                    DirectoryManagementCommands.CurrentDirectory = args[1];
                }

                // Retrieve all files and subdirectories from the current directory
                string[] files = Directory.GetFiles(DirectoryManagementCommands.CurrentDirectory);
                string[] folders = Directory.GetDirectories(DirectoryManagementCommands.CurrentDirectory);

                // Create a list to organize and display the directory contents
                List<string> items = new List<string>();

                // Display a summary notification with the count of files and folders found
                PrintNotify($"\"{DirectoryManagementCommands.CurrentDirectory}\" has {files.Length} files and {folders.Length} folders.");

                // Add files section if any files exist in the directory
                if (files.Length > 0)
                {
                    items.Add("Files:");

                    // Extract and add each file's name to the display list with a leading dash
                    foreach (string file in files)
                    {
                        items.Add($"- {Path.GetFileName(file)}");
                    }
                }

                // Add folders section if any subdirectories exist in the directory
                if (folders.Length > 0)
                {
                    items.Add("Folders:");

                    // Extract and add each folder's name to the display list with a leading dash
                    foreach (string folder in folders)
                    {
                        items.Add($"- {Path.GetFileName(folder)}");
                    }
                }

                // Display the organized list of files and folders to the user
                PrintNotify(items.ToArray());
            }
            catch (Exception e)
            {
                // Handle errors gracefully by displaying a failure message with exception details
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
