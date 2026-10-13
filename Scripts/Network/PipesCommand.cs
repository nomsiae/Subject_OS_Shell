using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using OS_Shell.Utilities;

namespace OS_Shell.Network
{
    public class PipesCommand : Command
    {
        // Named pipe identifier for server-client communication
        private const string PIPE_NAME = "OS_Shell";

        // Instance field - used ONLY in client mode
        private string clientId;

        // Stream objects for reading and writing data through the pipe
        private StreamReader _reader;
        private StreamWriter _writer;

        // Entry point that routes execution to server (create) or client (join) mode
        public void Execute(string[] args)
        {
            if (args.Length < 2)
            {
                throw new Exception(ErrorMessages.PIPE_SUBCOMMAND_IS_EMPTY);
            }

            string subCommand = args[1].ToLower();

            if (string.Equals(subCommand, "create"))
            {
                StartServer();
                return;
            }

            if (string.Equals(subCommand, "join"))
            {
                StartClient();
            }
        }

        // Initialize server mode and attach the server loop to the main update cycle
        private void StartServer()
        {
            Program.SetCanReceiveInput(false);

            Console.Title = $"Server - {PIPE_NAME}";
            Console.WriteLine("[SERVER] Running... Waiting for incoming client connections.");

            Program.Update += RunServer;
        }

        // Create a named pipe server and wait for client connections in a background task
        private void RunServer()
        {
            var server = new NamedPipeServerStream(
                PIPE_NAME,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances);

            server.WaitForConnection();
            Task.Run(() => HandleClient(server));
        }

        // Process messages from a connected client, handle disconnections, and maintain the connection loop
        private void HandleClient(NamedPipeServerStream server)
        {
            var reader = new StreamReader(server);
            var writer = new StreamWriter(server);
            string clientId = null;

            try
            {
                // Extract client ID from the first message; generate a unique ID if not provided
                string firstLine = reader.ReadLine();
                Logger.DeLog(firstLine);
                if (firstLine != null && firstLine.StartsWith("[CLIENT_ID]"))
                {
                    clientId = firstLine.Substring("[CLIENT_ID]".Length);
                }
                else
                {
                    clientId = Guid.NewGuid().ToString().Substring(0, 8);
                }

                Logger.Log($"[Server] Client {clientId} connected!");

                // Read messages from client until disconnection
                while (server.IsConnected)
                {
                    string line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line)) continue;

                    // Break loop if client sends disconnect command
                    if (string.Equals(line.ToLower(), "disconnect")) break;

                    // Log received message from the client
                    Logger.Log($"[Server] From {clientId}; received: {line}");
                    Logger.DeLog(clientId);

                    // Flush acknowledgment back to client
                    writer.Flush();
                }
            }
            finally
            {
                Logger.Log($"[Server] Client {clientId} disconnected!");

                DisconnectClient();
            }
        }

        // Initialize client mode and establish connection to the server named pipe
        private void StartClient()
        {
            Program.SetCanReceiveInput(false);

            this.clientId = Guid.NewGuid().ToString().Substring(0, 8);
            Console.Title = clientId;
            var client = new NamedPipeClientStream(PIPE_NAME);

            PrintNotify("Connecting to server...");
            client.Connect();
            PrintExecuteCompleted("Connected!");
            PrintNotify("Type a message and press Enter (type disconnect to quit)");

            _reader = new StreamReader(client);
            _writer = new StreamWriter(client);

            // Send the client's unique ID to the server for identification
            _writer.WriteLine($"[CLIENT_ID]{clientId}");
            _writer.Flush();

            Program.Update += RunClient;
        }

        // Read user input and send it to the server; listen for disconnect command
        private void RunClient()
        {
            Console.ForegroundColor = Program.ConsoleColor;
            Console.Write($"[Shell: {clientId}] ");
            string stringInput = Console.ReadLine();

            // Disconnect if user types disconnect command
            if (string.Equals(stringInput.ToLower(), "disconnect"))
            {
                DisconnectClient();
                PrintNotify("Disconnect from server");
                return;
            }

            // Send message to server and flush the stream
            _writer.WriteLine(stringInput);
            _writer.Flush();
        }

        // Clean up resources and restore input handling; suppress ObjectDisposedException if pipe is already closed
        private void DisconnectClient()
        {
            Program.Update -= RunClient;

            try
            {
                _reader?.Dispose();
                _writer?.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // Pipe already closed, ignore
            }

            Console.Title = "OS Shell";
            Program.SetCanReceiveInput(true);
        }
    }
}
