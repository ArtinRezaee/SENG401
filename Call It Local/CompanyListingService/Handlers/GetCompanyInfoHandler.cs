using CompanyListingService.Database;
using Messages.DataTypes.Database.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace CompanyListingService.Handlers
{
    public class GetCompanyInfoHandler : IHandleMessages<GetCompanyInfoRequest>
    {
        static ILog log = LogManager.GetLogger<GetCompanyInfoRequest>();

        public object CompanyistingDatabase { get; private set; }

        public Task Handle(GetCompanyInfoRequest message, IMessageHandlerContext context)
        {
            try
            {
                CompanyInstance company = CompanyListingDatabase.getInstance().getCompany(message.companyInfo);
                if(company == null)
                    return context.Reply(new GetCompanyInfoResponse(false, "No company found", company));
                else
                    return context.Reply(new GetCompanyInfoResponse(true, "Company info found", company));
            }
            catch(Exception err)
            {
                return context.Reply(new GetCompanyInfoResponse(false, err.Message, null));
            }
        }
    }
}
