using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment4.Models
{
    public class ReviewModel
    {
        public int ReviewId { get; set; }
        public string Review { get; set; }
        public string User { get; set; }
        public int Rating { get; set; }
        public int CompanyId { get; set; }
        public long Timestamp { get; set; }
        public ReviewModel(int i, string rev, string usr, int rat, long t, int c)
        {
            ReviewId = i;
            Review = rev;
            User = usr;
            Rating = rat;
            Timestamp = t;
            CompanyId = c;
        }

        override
        public string ToString()
        {
            string line = "Id = " + ReviewId + ", User = " + User + ", Review = " + Review + ", Timestamp = " + Timestamp;
            return line;
        }
    }
}