using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment4.Models;

namespace Assignment4.Controllers.Api
{
    public class UpdateController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Update(ReviewModel rev)
        {
            try
            {
                bool valid = Database.getInstance().updateReview(rev);

                if (valid)
                    return Request.CreateResponse(HttpStatusCode.OK, "Review has been updated");
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Updated failed");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}
