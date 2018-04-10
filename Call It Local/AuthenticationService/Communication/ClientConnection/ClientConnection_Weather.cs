using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Weather;
using Messages.ServiceBusRequest.Weather.Responses;
using NServiceBus;

namespace AuthenticationService.Communication
{
    /// <summary>
    /// This portion of the class contains methods specifically for accessing the company listing service.
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Listens for the client to secifify which task is being requested from the company listing service
        /// </summary>
        /// <param name="request">Includes which task is being requested and any additional information required for the task to be executed</param>
        /// <returns>A response message</returns>
        private ServiceBusResponse weatherServiceRequest(WeatherServiceRequest request)
        {
            return getWeatherCondition(request);
        }

        private ServiceBusResponse getWeatherCondition(WeatherServiceRequest request)
        {
            if (authenticated == false)
                return new WeatherResponse(false, "Error: You must be logged in to use the echo reverse functionality.");

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Weather");
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}