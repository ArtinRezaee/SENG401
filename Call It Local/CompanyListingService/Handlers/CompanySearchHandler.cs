
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using CompanyListingService.Database;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest;
using System;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;

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
            try
            {
                return context.Reply(CompanyListingDatabase.getInstance().searchCompany(message.searchDeliminator));
            }
            catch(Exception err)
            {
                return context.Reply(new CompanySearchResponse(false, err.Message, null));
            }
        }
    }
}
