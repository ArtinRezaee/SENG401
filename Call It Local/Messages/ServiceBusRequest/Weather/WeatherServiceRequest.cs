using System;

namespace Messages.ServiceBusRequest.Weather
{
    [Serializable]
    public class WeatherServiceRequest : ServiceBusRequest
    {
        public WeatherServiceRequest(WeatherRequestType requestType)
            : base(Service.Weather)
        {
            this.requestType = requestType;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the weather service
        /// </summary>
        public WeatherRequestType requestType;
    }

    public enum WeatherRequestType { citySearch, getWeather };
}
