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
    class HistoryHandler : IHandleMessages<GetChatHistoryRequest>
    {
        static ILog log = LogManager.GetLogger<GetChatHistoryRequest>();

        public Task Handle(GetChatHistoryRequest message, IMessageHandlerContext context)
        {
            try
            {
                GetChatHistory history = ChatDatabase.getInstance().getHistory(message.getCommand);

                if(history != null)
                    return context.Reply(new GetChatHistoryResponse(true, "Successful", history));
                else
                    return context.Reply(new GetChatHistoryResponse(false, "Unable to connect to database", message.getCommand));
            }
            catch(Exception err)
            {
                return context.Reply(new GetChatHistoryResponse(false, err.Message, message.getCommand));
            }
        }
    }
}
