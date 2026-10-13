using System;
using System.Collections.Generic;
using OS_Shell.Utilities;

namespace OS_Shell.Miscellaneous
{
    public class ChangeFontColorCommand : Command
    {
        private readonly Dictionary<string, string> argumentAliases = new Dictionary<string, string>
        {
            ["reset"] = "reset",
            ["r"] = "reset",
            ["current"] = "current",
            ["now"] = "current",
        };

        public void Execute(string[] args)
        {
            try
            {
                // Validates that a color argument was provided and retrieves the sub-command from the argument aliases dictionary.
                // If fewer than 2 arguments are provided, an exception with COLOR_IS_EMPTY error message is thrown.
                // The second argument (args[1]) is looked up in the argumentAliases dictionary to resolve command aliases
                // (e.g., "r" is resolved to "reset", "now" is resolved to "current").
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.COLOR_IS_EMPTY);
                }

                string subCommand = GetArgumentFromAliases(argumentAliases, args[1]);

                if (string.Equals(subCommand, "reset"))
                {
                    // Reset font color to the default White
                    Program.ConsoleColor = ConsoleColor.White;
                    PrintExecuteCompleted("Font color reset to White.");
                    return;
                }

                if (string.Equals(subCommand, "current"))
                {
                    // Display the currently active font color
                    PrintNotify($"Current font color is {Program.ConsoleColor}.");
                    return;
                }

                // Apply custom color from the provided color name string
                Program.ConsoleColor = ColorStringToConsoleColor(args[1]);
                PrintExecuteCompleted($"Font color change to {Program.ConsoleColor}.");
            }
            catch(Exception e)
            {
                PrintExecuteFailed(e.Message);
            }
        }

        private ConsoleColor ColorStringToConsoleColor(string color)
        {
            switch (color.ToLower())
            {
                case "black":
                    return ConsoleColor.Black;
                case "darkblue":
                    return ConsoleColor.DarkBlue;
                case "darkgreen":
                    return ConsoleColor.DarkGreen;
                case "darkcyan":
                    return ConsoleColor.DarkCyan;
                case "darkred":
                    return ConsoleColor.DarkRed;
                case "darkmagenta":
                    return ConsoleColor.DarkMagenta;
                case "darkyellow":
                    return ConsoleColor.DarkYellow;
                case "gray":
                    return ConsoleColor.Gray;
                case "darkgray":
                    return ConsoleColor.DarkGray;
                case "blue":
                    return ConsoleColor.Blue;
                case "green":
                    return ConsoleColor.Green;
                case "cyan":
                    return ConsoleColor.Cyan;
                case "red":
                    return ConsoleColor.Red;
                case "magenta":
                    return ConsoleColor.Magenta;
                case "yellow":
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}
