using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using OS_Shell.Utilities;

namespace OS_Shell.Network
{
    public class PingCommand : Command
    {
        public void Execute(string[] args)
        {
            try
            {
                // Validate that the IP address or domain argument is provided
                if (args.Length < 2)
                {
                    throw new Exception(ErrorMessages.IPADDRESS_IS_EMPTY_ERROR);
                }

                // Verify the argument is a valid IPv4, IPv6 address, or domain name
                if (!NetworkCommands.IsValidIpOrDomain(args[1]))
                {
                    throw new Exception(ErrorMessages.INVALID_IPADDRESS_ERROR);
                }

                PrintNotify("Pinging...");

                // Configure cmd.exe to execute the ping command and capture the output
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = true,
                    Arguments = $"/c \"ping {args[1]}\""
                };

                // Execute ping and process the result to display statistics or error messages
                using (Process process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();

                    // Extract and display ping statistics if the command was successful
                    if (output.Contains("Minimum"))
                    {
                        string pingInfo = Regex.Match(output, @"Minimum.*").Value;

                        PrintNotify(pingInfo);
                    }

                    // Report host unreachable error
                    if (output.Contains("host unreachable"))
                    {
                        PrintExecuteFailed("Host unreachable");
                    }

                    // Report timeout error when the request does not receive a response
                    if (output.Contains("timed out"))
                    {
                        PrintExecuteFailed("Request timed out");
                    }
                }
            }
            catch (Exception e)
            {
                PrintExecuteFailed(e.Message);
            }
        }
    }
}
