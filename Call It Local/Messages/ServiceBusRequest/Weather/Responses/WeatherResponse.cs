using System;

namespace Messages.ServiceBusRequest.Weather.Responses
{
    [Serializable]
    public class WeatherResponse: ServiceBusResponse
    {
        public WeatherResponse(bool result, string response)
            : base(result, response)
        {
        }
    }
}
