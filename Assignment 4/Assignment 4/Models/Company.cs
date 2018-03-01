using System;
namespace Assignment4.Models
{
    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
        public string desciption { get; set; }

        public Company(int i, string cName, string desc)
        {
            id = i;
            name = cName;
            desciption = desc;
        }

        override
        public string ToString(){
            string line = "Company: " + name + ", Id: " + id + ", Description: " + desciption;
            return line;
        }
    }
}
