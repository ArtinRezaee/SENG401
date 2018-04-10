using Messages.ServiceBusRequest;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;
using Messages.ServiceBusRequest.Weather;
using Messages.ServiceBusRequest.Weather.Responses;

namespace ChatService.Handlers
{
    class WeatherHandler : IHandleMessages<WeatherRequest>
    {
        static ILog log = LogManager.GetLogger<WeatherRequest>();

        public Task Handle(WeatherRequest message, IMessageHandlerContext context)
        {
            try
            {
                string response = "Sunny";

                // call the Accuweather API and get the response

                return context.Reply(new WeatherResponse(true, response));
            }
            catch (Exception err)
            {
                return context.Reply(new WeatherResponse(false, err.Message));
            }
        }
    }
}
