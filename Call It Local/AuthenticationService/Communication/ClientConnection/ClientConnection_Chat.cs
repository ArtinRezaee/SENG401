using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Chat;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.ServiceBusRequest.Chat.Responses;
using NServiceBus;


namespace AuthenticationService.Communication
{
    /// <summary>
    /// This portion of the class contains methods specifically for accessing the chat service.
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Listens for the client to secifify which task is being requested from the chat service
        /// </summary>
        /// <param name="request">Includes which task is being requested and any additional information required for the task to be executed</param>
        /// <returns>A response message</returns>
        private ServiceBusResponse chatRequest(ChatServiceRequest request)
        {
            switch (request.requestType)
            {
                case (ChatRequest.getChatContacts):
                    return getContacts((GetChatContactsRequest) request);
                case (ChatRequest.getChatHistory):
                    return getHistory((GetChatHistoryRequest) request);
                case (ChatRequest.sendMessage):
                    return sendMessage((SendMessageRequest) request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid Request. Request received was: " + request.requestType.ToString());
            }
        }

        private ServiceBusResponse getContacts(GetChatContactsRequest request)
        {
            if (authenticated == false)
                return new GetChatContactsResponse(false, "Error: You must be logged in to use the get contacts functionality.", request.getCommand);

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private ServiceBusResponse getHistory(GetChatHistoryRequest request)
        {
            if(authenticated == false)
                return new GetChatHistoryResponse(false, "Error: You must be logged in to use the get history functionality.", request.getCommand);

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private ServiceBusResponse sendMessage(SendMessageRequest request)
        {
            if(authenticated == false)
                return new ServiceBusResponse(false, "Error: You must be logged in to use the send message functionality.");

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
