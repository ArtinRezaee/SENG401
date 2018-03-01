using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment4.Models
{
    public class ReviewModel
    {
        public string Review { get; set; }
        public string User { get; set; }
        public int Rating { get; set; }
        public int CompanyId { get; set; }
        public long Timestamp { get; set; }

    }
}