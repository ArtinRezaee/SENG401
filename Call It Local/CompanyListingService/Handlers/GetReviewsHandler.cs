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
    public class GetReviewsHandler : IHandleMessages<GetCompanyReviewsRequest>
    {
        private static readonly string URL = "http://35.226.176.140/api/reviews2";

        static ILog log = LogManager.GetLogger<GetCompanyReviewsRequest>();

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
