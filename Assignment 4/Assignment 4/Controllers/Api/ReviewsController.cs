using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment4.Models;
using MySql.Data.MySqlClient;

namespace Assignment4.Controllers.Api
{
    public class ReviewsController : ApiController
    {
        public List<ReviewModel> Get(int id)
        {
            try
            {
                List<ReviewModel> R1 = Database.getInstance().getReviewForCompany(id);
                return R1;
            }
            catch(Exception err)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }


        [HttpPost]
        public HttpResponseMessage Post(ReviewModel reviewModel)
        {
            try
            {
                if (reviewModel == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                var conn = new MySqlConnection("server=35.184.174.117;user id=root;password=1234;database=seng401");
                int timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                string insertReviewCmd = string.Format("INSERT INTO reviews (review, user, rating, timestamp, company_id) values ('{0}', '{1}', {2}, '{3}', {4})",
                    reviewModel.Review, reviewModel.User, reviewModel.Rating, timestamp, reviewModel.CompanyId);

                MySqlCommand cmd = new MySqlCommand(insertReviewCmd, conn);

                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd.ExecuteNonQuery();                
                return Request.CreateResponse(HttpStatusCode.OK, "Review has been added");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
