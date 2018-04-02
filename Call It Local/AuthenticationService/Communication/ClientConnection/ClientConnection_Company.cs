using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
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
        private ServiceBusResponse companyDirectoryRequest(CompanyDirectoryServiceRequest request)
        {
            switch (request.requestType)
            {
                case (CompanyDirectoryRequest.CompanySearch):
                    return searchCompany((CompanySearchRequest) request);
                case (CompanyDirectoryRequest.GetCompanyInfo):
                    return getCompanyInfo((GetCompanyInfoRequest) request);
                case (CompanyDirectoryRequest.GetCompanyReviews):
                    return getReviews((GetCompanyReviewsRequest) request);
                case (CompanyDirectoryRequest.SaveReview):
                    return sendReview((SaveReviewRequest) request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid Request. Request received was: " + request.requestType.ToString());
            }
        }

        private ServiceBusResponse searchCompany(CompanySearchRequest request)
        {
            if (authenticated == false)
                return new CompanySearchResponse(false, "Error: You must be logged in to use the echo reverse functionality.", null);

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Company");
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private ServiceBusResponse getCompanyInfo(GetCompanyInfoRequest request)
        {
            if(authenticated == false)
                return new GetCompanyReviewsResponse(false, "Error: You must be logged in to use the company look up functionality.", null);

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Company");
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private ServiceBusResponse getReviews(GetCompanyReviewsRequest request)
        {
            if(authenticated == false)
                return new GetCompanyReviewsResponse(false, "Error: You must be logged in to use the company look up functionality.", null);

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Company");
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        private ServiceBusResponse sendReview(SaveReviewRequest request)
        {
            if(authenticated == false)
                return new ServiceBusResponse(false, "Error: You must be logged in to use the company look up functionality.");

            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Company");
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
