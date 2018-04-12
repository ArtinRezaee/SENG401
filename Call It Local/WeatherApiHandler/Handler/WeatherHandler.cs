using Messages.ServiceBusRequest;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;
using Messages.ServiceBusRequest.Weather;
using Messages.ServiceBusRequest.Weather.Responses;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ChatService.Handlers
{
    class WeatherHandler : IHandleMessages<WeatherRequest>
    {
        static ILog log = LogManager.GetLogger<WeatherRequest>();

        public string GetApiResponse(string url,string type,string cityName="") {

            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);


            string responseFromServer = reader.ReadToEnd();
            string result = string.Empty;
            if(type == "C")
            {
                 result = responseFromServer.Substring(responseFromServer.IndexOf("Key") + 6, 5);

            }
            else
            {
                var weatherText = responseFromServer.Split(':')[6];
                weatherText = weatherText.Substring(1, weatherText.IndexOf(',') - 2);

                var tempValue = responseFromServer.Split(':')[11];
                tempValue = tempValue.Substring(0, tempValue.IndexOf(','));

                var tempMetric = responseFromServer.Split(':')[9];
                tempMetric = tempMetric.Substring(2, tempMetric.Length - 3);

                var tempUnit = responseFromServer.Split(':')[12];
                tempUnit = tempUnit.Substring(1, 1);

                var tempRealFeel = responseFromServer.Split(':')[20];
                tempRealFeel = tempRealFeel.Substring(0, tempRealFeel.IndexOf(','));
               result = $"<div class='city-name'>{cityName}</div><div class='temp-value'>{tempValue} {tempUnit}</div><div class='temp-text'>{weatherText}</div><div class='real-feel'>Real Feel: {tempRealFeel} {tempUnit}</div>";

            }
         

            reader.Close();
            dataStream.Close();
            response.Close();
            return result;
        }

        public Task Handle(WeatherRequest message, IMessageHandlerContext context)
        {
            try
            {
                // call the Accuweather API and get the response
               var cityCode = GetApiResponse($"http://dataservice.accuweather.com/locations/v1/cities/search?q={message.Location}&apikey=XdbGqPfM7qNFQXAMHjjboxc0wAZ8nSpE","C");
               var weatherCondition = GetApiResponse($"http://dataservice.accuweather.com/currentconditions/v1/{cityCode}?&details=true&apikey=XdbGqPfM7qNFQXAMHjjboxc0wAZ8nSpE","W",message.Location);

                return context.Reply(new WeatherResponse(true, weatherCondition));
            }
            catch (Exception err)
            {
                return context.Reply(new WeatherResponse(false, err.Message));
            }
        }
    }
}
