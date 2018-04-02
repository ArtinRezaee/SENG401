using NServiceBus;
using Messages;
using System;
using System.Threading.Tasks;
using ChatService.Database;

namespace ChatService
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Chat";

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Chat");

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeAssemblies("MySql.Data.dll");

            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Debug.consoleMsg("Press Enter to exit.");
            string entry;
            
            do
            {
                entry = Console.ReadLine();

                switch (entry)
                {
                    case ("DELETEDB"):
                        ChatDatabase.getInstance().deleteDatabase();
                        Debug.consoleMsg("Delete database attempt complete");
                        break;
                    case ("CREATEDB"):
                        ChatDatabase.getInstance().createDB();
                        Debug.consoleMsg("Completed Database Creation Attempt.");
                        break;
                    default:
                        Debug.consoleMsg("Command not understood");
                        break;
                }
            } while (!entry.Equals(""));

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
