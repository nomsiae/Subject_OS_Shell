using System;

namespace OS_Shell.Network
{
    public class NetworkCommands
    {
        // Network command handlers for pipes, DNS cache clearing, and ping operations
        private readonly PipesCommand _pipesCommand;
        private readonly ClearDNSCachesCommand _clearDNSCachesCommand;
        private readonly PingCommand _pingCommand;

        // Initialize all network command instances
        public NetworkCommands()
        {
            _pipesCommand = new PipesCommand();
            _clearDNSCachesCommand = new ClearDNSCachesCommand();
            _pingCommand = new PingCommand();
        }

        // Execute the pipes command with provided arguments for network pipe operations
        public void ExecutePipesCommand(string[] args)
        {
            _pipesCommand.Execute(args);
        }

        // Execute the DNS cache flush command to clear system DNS entries
        public void ExecuteClearDNSCacheCommand()
        {
            _clearDNSCachesCommand.Execute();
        }

        // Execute the ping command to test connectivity to a specified host
        public void ExecutePingCommand(string[] args)
        {
            _pingCommand.Execute(args);
        }

        // Validate whether the input is a valid IPv4 address, IPv6 address, or domain name
        public static bool IsValidIpOrDomain(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            UriHostNameType hostType = Uri.CheckHostName(input);

            // Returns true if it matches IPv4, IPv6, or a valid DNS domain name
            return hostType == UriHostNameType.IPv4 ||
                   hostType == UriHostNameType.IPv6 ||
                   hostType == UriHostNameType.Dns;
        }
    }
}
