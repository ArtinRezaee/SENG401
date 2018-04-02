using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using CompanyListingService.Database;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using System;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;

namespace CompanyListingService.Handlers
{
    public class CompanySearchHandler : IHandleMessages<CompanySearchRequest>
    {
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
