using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

namespace Assignment4.Models
{
    public class DatabaseController
    {
        private static DatabaseController instance = null;

        private MySqlConnection conn;
        private DatabaseController() {
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = "35.184.174.117";
            conn_string.UserID = "root";
            conn_string.Password = "1234";
            conn_string.Database = "seng401";

            conn = new MySqlConnection(conn_string.ToString());
        }

        public static DatabaseController getInstance()
        {
            if (instance == null)
                return new DatabaseController();
            return instance;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool openConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool closeConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return false;
            }
        }

        public string getAllCompanies()
        {
            string query = "SELECT * FROM companies;";

            if (this.openConnection())
            {
                MySqlCommand command = new MySqlCommand(query, this.conn);
                MySqlDataReader reader = command.ExecuteReader();

                string result = "";
                while (reader.Read())
                    result += reader.GetString("name") + "\n";

                this.closeConnection();
                return result;
            }

            else
                return "Failed";
        }

        public List<Review> getReviews(string company)
        {
            List<Review> list = new List<Review>();

            try
            {
                int id = Int32.Parse(company);
                string query = "SELECT * FROM reviews WHERE company_id = " + id + ";";

                if (this.openConnection())
                {
                    MySqlCommand command = new MySqlCommand(query, this.conn);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Review tmp = new Review(reader.GetInt32("review_id"), reader.GetString("review"), reader.GetString("user"), reader.GetInt32("rating"), reader.GetInt64("timestamp"), reader.GetInt32("company_id"));
                        list.Add(tmp);

                    }

                    this.closeConnection();
                    return list;
                }

                else
                    return list;

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return list;
            }
        }

        public bool updateReview(Review oldRev)
        {
            if (this.openConnection())
            {
                DateTime foo = DateTime.UtcNow;
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();


                string query = "UPDATE reviews SET ";
                query += "user = '" + oldRev.User + "', review = '" + oldRev.User_Review + "', rating = " + oldRev.Rating;
                query += ", timestamp = " + unixTime + " WHERE review_id = " + oldRev.Id + ";";


                MySqlCommand command = new MySqlCommand(query, this.conn);
                int rows = command.ExecuteNonQuery();
                this.closeConnection();

                if (rows > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}