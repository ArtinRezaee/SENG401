using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Assignment4.Models;

namespace Assignment4.Controllers.Api
{
    public class ReviewsController : ApiController
    {

        public IEnumerable<Review> GetReview(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            else
                return DatabaseController.getInstance().getReviews(id);
        }

        [HttpPost]
        public Response UpdateReview(Review rev)
        {
            try
            {
                bool valid = DatabaseController.getInstance().updateReview(rev);

                if (valid)
                    return new Response((int) HttpStatusCode.OK, "Success - review has been updated");
                else
                    return new Response((int) HttpStatusCode.BadRequest, "Failure - review has NOT been updated");
            }
            catch (Exception err)
            {
                return new Response((int) HttpStatusCode.BadRequest, "Failure - " + err.Message);
            }

        }
    }
}
