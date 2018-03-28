
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using CompanyListingService.Database;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest;

namespace CompanyListingService.Handlers
{
    /// <summary>
    /// This is the handler class for the reverse echo. 
    /// This class is created and its methods called by the NServiceBus framework
    /// </summary>
    public class CompanySearchHandler : IHandleMessages<CompanySearchRequest>
    {
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<CompanySearchRequest>();

        public Task Handle(CompanySearchRequest message, IMessageHandlerContext context)
        {
            var result = CompanyListingDatabase.getInstance().searchCompany(message.searchDeliminator);

            //The context is used to give a reply back to the endpoint that sent the request
            return context.Reply(new ServiceBusResponse(true, string.Join(",", result.list.companyNames)));
        }
    }
}
