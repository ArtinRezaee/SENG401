using Messages.DataTypes.Database.CompanyDirectory;

using System;

namespace Messages.ServiceBusRequest.CompanyDirectory.Requests
{
    [Serializable]
    public class GetCompanyReviewsRequest : CompanyDirectoryServiceRequest
    {
        public GetCompanyReviewsRequest(String name)
            : base(CompanyDirectoryRequest.GetCompanyReviews)
        {
            this.companyName = name;
        }

        public String companyName;
    }
}

