
namespace Assignment4.Models
{
    public class HttpBody
    {
        public int Review_Id { get; set; }
        public string Review { get; set; }
        public string User { get; set; }
        public int Rating { get; set; }
        public long Timestamp { get; set; }
        public int Company_Id { get; set; }

        public HttpBody(int i, string rev, string usr, int rat, long t, int c)
        {
            Review_Id = i;
            Review = rev;
            User = usr;
            Rating = rat;
            Timestamp = t;
            Company_Id = c;
        }

        override
        public string ToString()
        {
            string line = "Id = " + Review_Id + ", User = " + User + ", Review = " + Review + ", Timestamp = " + Timestamp;
            return line;
        }
    }
}