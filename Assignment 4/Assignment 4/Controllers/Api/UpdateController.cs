using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment4.Models;

namespace Assignment4.Controllers.Api
{
    public class UpdateController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Update(HttpBody rev)
        {
            try
            {
                bool valid = Database.getInstance().updateReview(rev);

                if (valid)
                    return new HttpResponseMessage(HttpStatusCode.OK);
                else
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }
    }
}
