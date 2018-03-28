using System;

namespace Messages.DataTypes.Database.CompanyDirectory
{
    [Serializable]
    public class ReviewModel
    {
        public int ReviewId { get; set; }
        public string Review { get; set; }
        public string User { get; set; }
        public int Rating { get; set; }
        public string CompanyName { get; set; }
        public long Timestamp { get; set; }


        override
        public string ToString()
        {
            string line = "Id = " + ReviewId + ", Review = " + Review + ", User = " + User + ", Rating = " + Rating + ", Company = " + CompanyName + ", Timestamp = " + Timestamp;
            return line;
        }
    }
}