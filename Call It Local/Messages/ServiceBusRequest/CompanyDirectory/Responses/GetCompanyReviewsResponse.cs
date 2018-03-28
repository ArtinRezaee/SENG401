using Messages.DataTypes.Database.CompanyDirectory;

using System;
using System.Collections.Generic;

namespace Messages.ServiceBusRequest.CompanyDirectory.Responses
{
    [Serializable]
    public class GetCompanyReviewsResponse : ServiceBusResponse
    {
        public GetCompanyReviewsResponse(bool result, string response, List<ReviewModel> revs)
            : base(result, response)
        {
            this.reviews = revs;
        }

        public List<ReviewModel> reviews;
    }
}
