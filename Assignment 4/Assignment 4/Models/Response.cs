
namespace Assignment4.Models
{
    public class Response
    {
        public int Code { get; set; }
        public string Comment { get; set; }
        public Response(int cod, string comm)
        {
            Code = cod;
            Comment = comm;
        }
    }
}