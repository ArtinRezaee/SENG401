using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment4.Models
{
    public class ReviewModel2
    {
        public int ReviewId { get; set; }
        public string Review { get; set; }
        public string User { get; set; }
        public int Rating { get; set; }
        public string CompanyName { get; set; }
        public long Timestamp { get; set; }
        public ReviewModel2(int i, string rev, string usr, int rat, long t, string c)
        {
            ReviewId = i;
            Review = rev;
            User = usr;
            Rating = rat;
            Timestamp = t;
            CompanyName = c;
        }

        override
        public string ToString()
        {
            string line = "Id = " + ReviewId + ", Review = " + Review + ", User = " + User + ", Rating = " + Rating + ", Company = " + CompanyName + ", Timestamp = " + Timestamp;
            return line;
        }
    }
}