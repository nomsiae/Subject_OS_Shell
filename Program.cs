using System;
using System.Collections.Generic;
using OS_Shell.Miscellaneous;
using OS_Shell.DirectoryManagement;
using OS_Shell.Network;

namespace OS_Shell
{
    public class Program
    {
        #region Variables
        // Handles miscellaneous shell commands like help, quit, and clear.
        private static MiscellaneousCommands miscellaneousCommands;

        // Handles directory and file management commands like cd, list, create, delete, copy, etc.
        private static DirectoryManagementCommands directoryManagementCommands;

        // Handles network-related commands like ping, cleardns, and pipe.
        private static NetworkCommands networkCommands;

        // Flag to control the main application loop. When true, the application terminates.
        private static bool shouldExit;

        /// <summary>The foreground color used for console output throughout the shell.
        public static ConsoleColor ConsoleColor = ConsoleColor.White;

        // Delegate that triggers the main update loop. Additional methods can be subscribed to execute periodic tasks or receive input.
        public static Action Update;

        /// <summary>
        /// Dictionary mapping command aliases and full command names to their canonical command names.
        /// Supports multiple aliases per command (e.g., "quit", "exit", "close", "terminate" all map to "quit").
        /// This enables flexible user input while maintaining consistent internal command handling.
        /// </summary>
        private readonly static Dictionary<string, string> commandNames = new Dictionary<string, string>
        {
            // 1st
            ["help"] = "help",
            ["h"] = "help",
            // 2nd
            ["quit"] = "quit",
            ["exit"] = "quit",
            ["close"] = "quit",
            ["terminate"] = "quit",
            // 3rd
            ["clear"] = "clear",
            ["clr"] = "clear",
            // 4th
            ["remind"] = "remind",
            // 5th
            ["fontcolor"] = "fontcolor",
            ["fc"] = "fontcolor",
            // 6th
            ["cd"] = "cd",
            // 7th
            ["list"] = "list",
            ["l"] = "list",
            // 8th
            ["create"] = "create",
            ["cr"] = "create",
            // 9th
            ["delete"] = "delete",
            ["del"] = "delete",
            ["remove"] = "delete",
            // 10th
            ["copy"] = "copy",
            ["duplicate"] = "copy",
            ["dup"] = "copy",
            ["clone"] = "copy",
            // 11th
            ["edit"] = "edit",
            ["modify"] = "edit",
            // 12th
            ["cut"] = "cut",
            ["move"] = "cut",
            // 13th
            ["open"] = "open",
            ["o"] = "open",
            // 14th
            ["cleardns"] = "cleardns",
            ["cdns"] = "cleardns",
            // 15th
            ["ping"] = "ping",
            // 16th
            ["pipe"] = "pipe",
            ["p"] = "pipe",
        };
        #endregion

        #region Main Methods
        /// <summary>
        /// The main entry point of the OS Shell application.
        /// Initializes all command modules, displays the welcome message,
        /// and maintains the main loop that processes user input until the application exits.
        /// </summary>
        /// <param name="args">Command-line arguments (currently unused).</param>
        private static void Main(string[] args)
        {
            Console.Title = "OS Shell";

            Initialize();

            Console.ForegroundColor = ConsoleColor;
            Console.WriteLine("Type \"help\" to show all commands");

            while (!shouldExit)
            {
                Update?.Invoke();
            }
        }

        /// <summary>
        /// Initializes the application by instantiating all command handler modules
        /// and subscribing the ReceiveInput method to the Update delegate.
        /// Must be called before the main application loop begins.
        /// </summary>
        private static void Initialize()
        {
            miscellaneousCommands = new MiscellaneousCommands();
            directoryManagementCommands = new DirectoryManagementCommands();
            networkCommands = new NetworkCommands();

            Update += ReceiveInput;
        }
        #endregion

        #region Reusable Methods
        /// <summary>
        /// Continuously prompts the user for input, parses the command and arguments,
        /// and delegates execution to the appropriate command handler.
        /// Called repeatedly by the Update delegate in the main loop.
        /// </summary>
        private static void ReceiveInput()
        {
            Console.ForegroundColor = ConsoleColor;
            Console.Write("<OS Shell>: ");
            string inputString = Console.ReadLine();

            if (string.IsNullOrEmpty(inputString)) return;

            string[] args = SplitCommandLine(inputString).ToArray();
            string commandName = GetCommandName(args[0].ToLower());

            ExecuteCommand(args, commandName);
        }

        /// <summary>
        /// Enables or disables the ability to receive user input by subscribing or unsubscribing
        /// the ReceiveInput method from the Update delegate.
        /// </summary>
        /// <param name="enabled">True to enable input reception; false to disable it.</param>
        public static void SetCanReceiveInput(bool enabled)
        {
            if (enabled)
            {
                Update += ReceiveInput;
            }
            else
            {
                Update -= ReceiveInput;
            }
        }

        /// <summary>
        /// Routes the parsed command to the appropriate handler module based on the command name,
        /// passing along any provided arguments.
        /// Supports 16 command categories: miscellaneous, file/directory, and network operations.
        /// If the command is not recognized, logs an error message.
        /// </summary>
        /// <param name="args">The tokenized input arguments; args[0] is the command name.</param>
        /// <param name="commandName">The canonical command name (from commandNames dictionary).</param>
        private static void ExecuteCommand(string[] args, string commandName)
        {
            switch (commandName)
            {
                // Miscellaneous shell commands
                case "help": // 1st
                    miscellaneousCommands.ExecuteHelpCommand(); break;
                case "quit": // 2nd
                    shouldExit = true; break;
                case "clear": // 3rd
                    Console.Clear(); break;
                case "remind": // 4th
                    miscellaneousCommands.ExecuteRemindMeCommand(args); break;
                case "fontcolor": // 5th
                    miscellaneousCommands.ExecuteChangeFontColorCommand(args); break;

                // File/Directory commands
                case "cd": // 6th
                    directoryManagementCommands.ExecuteChangeDirectoryCommand(args); break;
                case "list": // 7th
                    directoryManagementCommands.ExecuteListItemsCommand(args); break;
                case "create": // 8th
                    directoryManagementCommands.ExecuteCreateCommand(args); break;
                case "delete": // 9th
                    directoryManagementCommands.ExecuteDeleteCommand(args); break;
                case "copy": // 10th
                    directoryManagementCommands.ExecuteCopyCommand(args); break;
                case "edit": // 11th
                    directoryManagementCommands.ExecuteEditCommand(args); break;
                case "cut": // 12th
                    directoryManagementCommands.ExecuteCutCommand(args); break;
                case "open": // 13th
                    directoryManagementCommands.ExecuteOpenCommand(args); break;

                // Network commands
                case "cleardns": // 14th
                    networkCommands.ExecuteClearDNSCacheCommand(); break;
                case "ping": // 15th
                    networkCommands.ExecutePingCommand(args); break;
                case "pipe": // 16th
                    networkCommands.ExecutePipesCommand(args); break;

                default:
                    Logger.Log($"Unknown command: \"{args[0]}\". Please type \"help\" to open document.", ConsoleColor); break;
            }
        }

        /// <summary>
        /// Parses a command-line input string into individual tokens (arguments),
        /// respecting quoted strings as single tokens.
        /// Handles escaped quotes (preceded by backslash) and removes surrounding quotes from tokens.
        /// </summary>
        /// <param name="input">The raw command-line input string from the user.</param>
        /// <returns>A list of tokenized arguments. Empty strings are filtered out.</returns>
        /// <remarks>
        /// Example: 'create "My Folder" file.txt' returns ["create", "My Folder", "file.txt"]
        /// </remarks>
        private static List<string> SplitCommandLine(string input)
        {
            var result = new List<string>();
            bool inQuotes = false;
            int startIndex = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '"' && (i == 0 || input[i - 1] != '\\'))
                {
                    inQuotes = !inQuotes;
                }
                else if (input[i] == ' ' && !inQuotes)
                {
                    if (i > startIndex)
                    {
                        string token = input.Substring(startIndex, i - startIndex).Trim(' ', '"');
                        if (!string.IsNullOrEmpty(token)) result.Add(token);
                    }
                    startIndex = i + 1;
                }
            }

            if (startIndex < input.Length)
            {
                string token = input.Substring(startIndex).Trim(' ', '"');
                if (!string.IsNullOrEmpty(token)) result.Add(token);
            }

            return result;
        }

        /// <summary>
        /// Resolves a user-input command (or alias) to its canonical command name
        /// using the commandNames dictionary.
        /// </summary>
        /// <param name="inputString">The normalized (lowercase) user input.</param>
        /// <returns>The canonical command name if found; otherwise null.</returns>
        private static string GetCommandName(string inputString)
        {
            if (commandNames.TryGetValue(inputString, out string command))
            {
                return command;
            }

            return null;
        }
        #endregion
    }
}