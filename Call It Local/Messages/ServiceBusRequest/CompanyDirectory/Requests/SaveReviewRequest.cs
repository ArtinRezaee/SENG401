using Messages.DataTypes.Database.CompanyDirectory;

using System;

namespace Messages.ServiceBusRequest.CompanyDirectory.Requests
{
    [Serializable]
    public class SaveReviewRequest : CompanyDirectoryServiceRequest
    {
        public SaveReviewRequest(ReviewModel rev)
            : base(CompanyDirectoryRequest.SaveReview)
        {
            this.review = rev;
        }

        public ReviewModel review;
    }
}

