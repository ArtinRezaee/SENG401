using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CompanyListingService.Handlers
{

    /// <summary>
    /// This is the handler class for the reverse echo. 
    /// This class is created and its methods called by the NServiceBus framework
    /// </summary>
    public class SaveReviewHandler : IHandleMessages<SaveReviewRequest>
    {
        private static readonly string URL = "http://35.226.176.140/api/reviews2";

        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<SaveReviewRequest>();

        /// <summary>
        /// Saves the echo to the database, reverses the data, and returns it back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public Task Handle(SaveReviewRequest message, IMessageHandlerContext context)
        {
            HttpWebRequest postReq = (HttpWebRequest) WebRequest.Create(URL);
            postReq.Method = "POST";
            postReq.ContentType = "application/json";
            postReq.MediaType = "application/json";
            postReq.Accept = "application/json";

            try
            {
                using (var streamWriter = new StreamWriter(postReq.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(message.review);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse) postReq.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();  
                    StreamReader reader = new StreamReader(dataStream);  
                    string responseFromServer = reader.ReadToEnd();

                    reader.Close();  
                    response.Close();

                    if(response.StatusCode == HttpStatusCode.OK)
                        return context.Reply(new ServiceBusResponse(true, responseFromServer));
                    else
                        return context.Reply(new ServiceBusResponse(false, responseFromServer));                    
                }
            }
            catch (WebException err)
            {                    
                return context.Reply(new ServiceBusResponse(false, err.Message));
            }
        }
    }
}
