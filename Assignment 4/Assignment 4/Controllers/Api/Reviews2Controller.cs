using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment4.Models;
using MySql.Data.MySqlClient;

namespace Assignment4.Controllers.Api
{
    public class Reviews2Controller : ApiController
    {
        public List<ReviewModel2> Get(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            try
            {
                List<ReviewModel2> R1 = Database.getInstance().getReview2ForCompany(id);
                return R1;
            }
            catch(Exception err)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }


        [HttpPost]
        public HttpResponseMessage Post(ReviewModel2 review)
        {
            System.Diagnostics.Debug.WriteLine(review.ToString());
            if (review == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            else if(String.IsNullOrWhiteSpace(review.Review) || String.IsNullOrWhiteSpace(review.User) || String.IsNullOrWhiteSpace(review.CompanyName))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                try
                {
                    Database.getInstance().addReview2(review);
                    return Request.CreateResponse(HttpStatusCode.OK, "Review has been added");
                }
                catch(Exception err)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err.Message);
                }
            }      
           
        }
    }
}
