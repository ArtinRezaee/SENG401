﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

namespace Assignment4.Models
{
    public class Database
    {
        private static Database instance = null;

        private MySqlConnection conn;
        private Database() {
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = "localhost";
            conn_string.UserID = "root";
            conn_string.Password = "1234";
            conn_string.Database = "assignment4";

            conn = new MySqlConnection(conn_string.ToString());
        }

        public static Database getInstance()
        {
            if (instance == null)
                return new Database();
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

        public bool updateReview(ReviewModel oldRev)
        {
            if (this.openConnection())
            {
                DateTime foo = DateTime.UtcNow;
                long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();


                string query = "UPDATE reviews SET ";
                query += "user = '" + oldRev.User + "', review = '" + oldRev.Review + "', rating = " + oldRev.Rating;
                query += ", timestamp = " + unixTime + " WHERE review_id = " + oldRev.ReviewId + ";";


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

        public List<Company> getAllCompanies()
        {
            List<Company> companies = new List<Company>();

            string query = "SELECT * FROM companies;";
            try
            {

                if (this.openConnection())
                {
                    MySqlCommand command = new MySqlCommand(query, this.conn);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Company temp = new Company(reader.GetInt32("company_id"), reader.GetString("name"), reader.GetString("description"));
                        companies.Add(temp);
                    }

                    this.closeConnection();
                    return companies;
                }

                else
                    return companies;

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return companies;
            }
        }

        public Company getCompany(int id)
        {
            string query = "SELECT * FROM companies WHERE company_id=" + id + ";";

            if (this.openConnection())
            {
                MySqlCommand command = new MySqlCommand(query, this.conn);
                MySqlDataReader reader = command.ExecuteReader();

                Company comp = null;
                while (reader.Read())
                {
                    comp = new Company(reader.GetInt32("company_id"), reader.GetString("name"), reader.GetString("description"));
                }

                this.closeConnection();
                return comp;
            }
            else
                return null;
        }

        public List<ReviewModel> getReviewForCompany(int company_id)
        {
            List<ReviewModel> RList = new List<ReviewModel>();
            String query = "SELECT * from reviews where company_id=" + company_id + ";";

            try
            {
                if (this.openConnection())
                {
                    MySqlCommand command = new MySqlCommand(query, this.conn);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ReviewModel R1 = new ReviewModel(reader.GetInt32("review_id"), reader.GetString("review"), reader.GetString("user"), reader.GetInt32("rating"), reader.GetInt64("timestamp"), reader.GetInt32("company_id"));
                        RList.Add(R1);
                    }
                    this.closeConnection();
                    return RList;
                }

                else
                    return RList;

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return RList;
            }
        }

        public List<ReviewModel2> getReview2ForCompany(string name)
        {
            List<ReviewModel2> RList = new List<ReviewModel2>();
            String query = "SELECT * from reviews2 where company_name='" + name + "';";

            try
            {
                if (this.openConnection())
                {
                    MySqlCommand command = new MySqlCommand(query, this.conn);
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ReviewModel2 R1 = new ReviewModel2(reader.GetInt32("review_id"), reader.GetString("review"), reader.GetString("user"), reader.GetInt32("rating"), reader.GetInt64("timestamp"), reader.GetString("company_name"));
                        RList.Add(R1);
                    }
                    this.closeConnection();
                    return RList;
                }

                else
                    return RList;

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return RList;
            }
        }

        public void addReview2(ReviewModel2 review)
        {
            int timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string query = string.Format("INSERT INTO reviews2 (review, user, rating, timestamp, company_name) values ('{0}', '{1}', {2}, '{3}', '{4}')",
                           review.Review, review.User, review.Rating, timestamp, review.CompanyName);

            if (this.openConnection())
            {
                MySqlCommand command = new MySqlCommand(query, this.conn);
                command.ExecuteNonQuery();
                this.closeConnection();
            }
        }



        public void addCompany(Company company)
        {
            string query = "INSERT INTO companies (name,description) VALUES ('" + company.Name + "', '" + company.Description + "');";

            if (this.openConnection())
            {
                MySqlCommand command = new MySqlCommand(query, this.conn);
                command.ExecuteNonQuery();
                this.closeConnection();
            }
        }
    }
}