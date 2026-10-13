using System;
using System.Collections.Generic;

namespace OS_Shell.Utilities
{
    public abstract class Command
    {
        #region Variables
        // Reserved keywords that cannot be used as input values
        private readonly string[] forbiddenWords = new[]
        {
            "reset",
            "r",
            "current",
            "now",
            "parent"
        };

        // Display error message with red formatting and OS Shell header
        private readonly Dictionary<string, string> yesNoOptionAliases = new Dictionary<string, string>
        {
            ["yes"] = "yes",
            ["y"] = "yes",
            ["1"] = "yes",
            ["no"] = "no",
            ["n"] = "no",
            ["0"] = "no",
        };
        #endregion

        #region Reusable Methods
        /// <summary>
        /// Check if the provided text matches any forbidden reserved words
        /// </summary>
        /// <param name="text">A string that needed to be checked</param>
        /// <returns>true if the text is keywords; otherwise, false.</returns>
        protected bool IsStringForbidden(string text)
        {
            // Check if text is in keywords
            foreach (string word in forbiddenWords)
            {
                if (string.Equals(word, text)) return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieve the standard value for an input string from the aliases dictionary
        /// </summary>
        /// <param name="aliases">Command argument's aliases</param>
        /// <param name="inputString">User input</param>
        /// <returns></returns>
        protected string GetArgumentFromAliases(Dictionary<string, string> aliases, string inputString)
        {
            if (aliases.TryGetValue(inputString, out string alias))
            {
                return alias;
            }

            return null;
        }

        #region Display Methods
        /// <summary>
        /// Prompt user with a yes/no question and validate input against recognized aliases
        /// </summary>
        /// <param name="text">message to display</param>
        /// <returns></returns>
        protected bool YesNoOption(string text)
        {
            PrintConfirmAction($"{text}? (Y/N)");
            Console.Write("            > ");

            string inputString = Console.ReadLine().ToLower();

            if (yesNoOptionAliases.TryGetValue(inputString, out string output))
            {
                return string.Equals(output, "yes");
            }

            return false;
        }

        /// <summary>
        /// Display success message with green formatting and OS Shell header
        /// </summary>
        /// <param name="messages">message to display</param>
        protected void PrintExecuteCompleted(params string[] messages)
        {
            PrintWithHeader(ConsoleColor.Green, messages);
        }

        /// <summary>
        /// Display error message with red formatting and OS Shell header
        /// </summary>
        /// <param name="messages">message to display</param>
        protected void PrintExecuteFailed(params string[] messages)
        {
            PrintWithHeader(ConsoleColor.Red, messages);
        }

        /// <summary>
        /// Display informational message with cyan formatting and OS Shell header
        /// </summary>
        /// <param name="messages">message to display</param>
        protected void PrintNotify(params string[] messages)
        {
            PrintWithHeader(ConsoleColor.Cyan, messages);
        }

        /// <summary>
        /// Display confirmation prompt with yellow formatting and OS Shell header
        /// </summary>
        /// <param name="messages">message to display</param>
        private void PrintConfirmAction(params string[] messages)
        {
            PrintWithHeader(ConsoleColor.Yellow, messages);
        }

        /// <summary>
        /// Output message with "<OS Shell>: " prefix in specified color and log it
        /// </summary>
        /// <param name="color"></param>
        /// <param name="messages"></param>
        private void PrintWithHeader(ConsoleColor color, params string[] messages)
        {
            Console.ForegroundColor = Program.ConsoleColor;
            Console.Write("<OS Shell>: ");

            string messageOutput = AdjustLines(messages);
            Logger.Log(messageOutput, color);
        }

        /// <summary>
        /// Join multiple messages with proper indentation for vertically aligned console output
        /// </summary>
        /// <param name="texts"></param>
        /// <returns></returns>
        private string AdjustLines(string[] texts)
        {
            return string.Join("\n            ", texts);
        }
        #endregion
        #endregion
    }
}
