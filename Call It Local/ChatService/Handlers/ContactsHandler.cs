using ChatService.Database;
using Messages.NServiceBus.Commands;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.ServiceBusRequest.Chat.Responses;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace ChatService.Handlers
{
    class ContactsHandler : IHandleMessages<GetChatContactsRequest>
    {
        static ILog log = LogManager.GetLogger<GetChatContactsRequest>();

        public Task Handle(GetChatContactsRequest message, IMessageHandlerContext context)
        {
            try
            {
                GetChatContacts contacts = ChatDatabase.getInstance().getContacts(message.getCommand);

                if(contacts != null)
                    return context.Reply(new GetChatContactsResponse(true, "Successful", contacts));
                else
                    return context.Reply(new GetChatContactsResponse(false, "Unable to connect to database", message.getCommand));
            }
            catch(Exception err)
            {
                return context.Reply(new GetChatContactsResponse(false, err.Message, message.getCommand));
            }
        }
    }
}
