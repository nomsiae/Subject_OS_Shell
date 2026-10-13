namespace OS_Shell.Miscellaneous
{
    public class MiscellaneousCommands
    {
        // Command instances for miscellaneous operations: help documentation, reminders, and font color changes
        private readonly HelpCommand _helpCommand;
        private readonly RemindMeCommand _remindMeCommand;
        private readonly ChangeFontColorCommand _changeFontColorCommand;

        // Initialize all miscellaneous command handlers in the constructor
        public MiscellaneousCommands()
        {
            _helpCommand = new HelpCommand();
            _remindMeCommand = new RemindMeCommand();
            _changeFontColorCommand = new ChangeFontColorCommand();
        }

        // Open the help documentation PDF file
        public void ExecuteHelpCommand()
        {
            _helpCommand.Execute();
        }

        // Execute a background reminder that triggers after a specified number of seconds
        public void ExecuteRemindMeCommand(string[] args)
        {
            _remindMeCommand.Execute(args);
        }

        // Execute the font color change command with the provided arguments
        public void ExecuteChangeFontColorCommand(string[] args)
        {
            _changeFontColorCommand.Execute(args);
        }
    }
}
