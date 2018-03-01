using System;
namespace Assignment4.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Company(int i, string cName, string desc)
        {
            CompanyId = i;
            Name = cName;
            Description = desc;
        }

        override
        public string ToString()
        {
            string line = "Id: " + CompanyId + ", Name: " + Name + ", Description: " + Description;
            return line;
        }
    }
}
