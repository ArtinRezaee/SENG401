using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using Assignment4.Models;

namespace Assignment4
{
    public class ReviewsPersistence
    {
        public MySql.Data.MySqlClient.MySqlConnection conn;

        public ReviewsPersistence()
        {
            String myConnectionString = "server=35.184.174.117;user id=root;password=1234;database=seng401";

            //try the connection
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<ReviewModel> getReviewForCompany(int company_id)
        {
            ReviewModel R1 = new ReviewModel();
            List<ReviewModel> RList = new List<ReviewModel>();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            String sqlString = "SELECT * from reviews where company_id= " + company_id.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                R1.Review = reader.GetString(1);
                R1.User = reader.GetString(2);
                R1.Rating = reader.GetInt32(3);
                R1.Timestamp = reader.GetInt32(4);
                R1.CompanyId = reader.GetInt32(5);

                RList.Add(R1);
            }

            return RList;
        }

        public List<ReviewModel> getAllReviews()
        {
            ReviewModel R1 = new ReviewModel();
            List<ReviewModel> RList = new List<ReviewModel>();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            String sqlString = "SELECT * from reviews";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                R1.Review = reader.GetString(1);
                R1.User = reader.GetString(2);
                R1.Rating = reader.GetInt32(3);
                R1.Timestamp = reader.GetInt32(4);
                R1.CompanyId = reader.GetInt32(5);

                RList.Add(R1);
            }

            return RList;
        }

        public int getTotalNumberOfReviews(int company_id)
        {
            ReviewModel R1 = new ReviewModel();
            int counter = 0;
            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            String sqlString = "SELECT count(*) from reviews where company_id= " + company_id.ToString() + " GROUP BY company_id";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                counter++;
            }

            return counter;

        }



    }
}