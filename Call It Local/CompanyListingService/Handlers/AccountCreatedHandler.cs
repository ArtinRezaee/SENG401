using Messages.DataTypes.Database.CompanyDirectory;
using Messages.NServiceBus.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CompanyListingService.Database;

namespace CompanyListingService.Handlers
{
    public class AccountCreatedHandler : IHandleMessages<AccountCreated>
    {       
        static ILog log = LogManager.GetLogger<AccountCreated>();

        public Task Handle(AccountCreated message, IMessageHandlerContext context)
        {
            if (message.type == Messages.DataTypes.AccountType.business)
            {
                List<String> addresses = new List<string>();
                addresses.Add(message.address);
                CompanyInstance company = new CompanyInstance(message.username, message.phonenumber, message.email, addresses.ToArray());
                CompanyListingDatabase.getInstance().saveCompany(company);
            }
            return Task.CompletedTask;
        }
    }
}
