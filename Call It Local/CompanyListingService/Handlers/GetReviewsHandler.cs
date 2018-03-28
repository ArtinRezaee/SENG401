using Messages.DataTypes.Database.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CompanyListingService.Handlers
{

    /// <summary>
    /// This is the handler class for the reverse echo. 
    /// This class is created and its methods called by the NServiceBus framework
    /// </summary>
    public class GetReviewsHandler : IHandleMessages<GetCompanyReviewsRequest>
    {
        private static readonly string URL = "http://35.226.176.140/api/reviews2";

        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<GetCompanyReviewsRequest>();

        /// <summary>
        /// Saves the echo to the database, reverses the data, and returns it back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public Task Handle(GetCompanyReviewsRequest message, IMessageHandlerContext context)
        {
            string path = URL + "/" + message.companyName;
            List<ReviewModel> reviews = null;
            
            HttpWebRequest getReq = (HttpWebRequest) WebRequest.Create(path);
            getReq.Method = "Get";
            getReq.KeepAlive = true;
            getReq.ContentType = "appication/json";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse) getReq.GetResponse())
                {
                    if(response.StatusCode != HttpStatusCode.OK)
                        return context.Reply(new GetCompanyReviewsResponse(false, response.StatusDescription, new List<ReviewModel>())); 
                    else
                    {

                        Stream dataStream = response.GetResponseStream();  
                        StreamReader reader = new StreamReader(dataStream);  
                        var responseFromServer = reader.ReadToEnd();
                        reviews = JsonConvert.DeserializeObject<List<ReviewModel>>(responseFromServer);

                        reader.Close();  
                        response.Close();

                        return context.Reply(new GetCompanyReviewsResponse(true, "Success", reviews));
                    }
                }
            }
            catch (WebException err)
            {                    
                return context.Reply(new GetCompanyReviewsResponse(false, err.Message, new List<ReviewModel>()));
            }
        }
    }
}
