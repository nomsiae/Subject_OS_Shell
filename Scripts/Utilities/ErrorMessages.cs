namespace OS_Shell.Utilities
{
    public static class ErrorMessages
    {
        #region Directory Management
        // Create file/folder command error messages
        public static string FILE_TYPE_EMPTY_ERROR = "File type must be specified.";
        public static string FILE_DISPLAY_NAME_EMPTY_ERROR = "File display name is required.";
        //------------------------------------------------------------------------------------------------------------------------------

        // Common command error messages
        public static string SOURCE_IS_EMPTY_ERROR = "Source path must be specified.";
        public static string SOURCE_IS_NOT_EXIST_ERROR = "Source path does not exist.";
        public static string TARGET_DIRECTORY_IS_EMPTY_ERROR = "Target directory path must be specified.";
        public static string TARGET_DIRECTORY_IS_NOT_EXIST_ERROR = "Target directory does not exist.";
        public static string SAME_DIRECTORY_ERROR = "Source and target directories cannot be the same.";
        public static string INVALID_DIRECTORY_ERROR = "The specified directory is invalid or inaccessible.";
        public static string NAMING_WITH_FORBIDDEN_WORDS_ERROR = "The name contains reserved keywords and cannot be used.";
        //------------------------------------------------------------------------------------------------------------------------------
        #endregion

        #region Miscellaneous
        // Remind command error messages
        public static string REMIND_SECONDS_IS_EMPTY_ERROR = "Wait time must be specified for the remind command.";
        public static string REMIND_SECONDS_IS_NOT_INTEGER_ERROR = "Wait time must be a whole number representing seconds.";
        public static string COLOR_IS_EMPTY = "Color must be specified.";
        #endregion

        #region Networks
        public static string IPADDRESS_IS_EMPTY_ERROR = "Target IP address or domain name is required for ping command.";
        public static string INVALID_IPADDRESS_ERROR = "The specified IP address or domain name is invalid.";
        public static string PIPE_SUBCOMMAND_IS_EMPTY = "Usage: pipe <create|join>";
        #endregion
    }
}
