using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CompanyListingService.Handlers
{
    public class SaveReviewHandler : IHandleMessages<SaveReviewRequest>
    {
        private static readonly string URL = "http://35.226.176.140/api/reviews2";

        static ILog log = LogManager.GetLogger<SaveReviewRequest>();

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
