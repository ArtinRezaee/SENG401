using System;

namespace Messages.ServiceBusRequest.Weather
{
    [Serializable]
    public class WeatherRequest : WeatherServiceRequest
    {
        public WeatherRequest(string location)
            : base(WeatherRequestType.getWeather)
        {
            this.Location = location;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the weather service
        /// </summary>
        public string Location;
    }
}
