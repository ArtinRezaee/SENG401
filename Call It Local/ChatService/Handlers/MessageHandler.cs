using ChatService.Database;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.ServiceBusRequest;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace ChatService.Handlers
{
    class MessageHandler : IHandleMessages<SendMessageRequest>
    {
        static ILog log = LogManager.GetLogger<SendMessageRequest>();

        public Task Handle(SendMessageRequest message, IMessageHandlerContext context)
        {
            try
            {
                string response = ChatDatabase.getInstance().sendMessage(message.message);

                if (String.Equals(response, "Successful"))
                    return context.Reply(new ServiceBusResponse(true, response));
                else
                    return context.Reply(new ServiceBusResponse(false, response));
            }
            catch(Exception err)
            {
                return context.Reply(new ServiceBusResponse(false, err.Message));
            }
        }
    }
}
