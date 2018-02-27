
namespace Assignment4.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string User_Review { get; set; }
        public string User { get; set; }
        public int Rating { get; set; }
        public long Timestamp { get; set; }
        public int Company_Id { get; set; }

        public Review(int i, string rev, string usr, int rat, long t, int c)
        {
            Id = i;
            User_Review = rev;
            User = usr;
            Rating = rat;
            Timestamp = t;
            Company_Id = c;
        }

        override
        public string ToString()
        {
            string line = "Id = " + Id + ", User = " + User + ", Review = " + User_Review + ", Timestamp = " + Timestamp;
            return line;
        }
    }
}