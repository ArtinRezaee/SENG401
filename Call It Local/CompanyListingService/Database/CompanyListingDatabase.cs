using Messages;
using Messages.Database;
using Messages.DataTypes.Database.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;

namespace CompanyListingService.Database
{
    /// <summary>
    /// This portion of the class contains methods and functions
    /// </summary>
    public partial class CompanyListingDatabase : AbstractDatabase
    {
        /// <summary>
        /// Private default constructor to enforce the use of the singleton design pattern
        /// </summary>
        private CompanyListingDatabase() { }

        /// <summary>
        /// Gets the singleton instance of the database
        /// </summary>
        /// <returns>The singleton instance of the database</returns>
        public static CompanyListingDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new CompanyListingDatabase();
            }
            return instance;
        }

        /// <summary>
        /// Saves the foreward echo to the database
        /// </summary>
        /// <param name="echo">Information about the echo</param>
        public void saveCompany(CompanyInstance company)
        {
            if(openConnection() == true)
            {
                string query = $@"INSERT INTO companylisting(timestamp, companyname, phonenumber, email, locations)
            VALUES('{DateTimeOffset.Now.ToUnixTimeSeconds().ToString()}', '{company.companyName}', '{company.phoneNumber}', '{company.email}', '{company.locations[0]}');";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                closeConnection();
            }
            else
            {
                throw new Exception("Unable to connect to database.");
            }
        }

        /// <summary>
        /// Gets the company info
        /// </summary>
        /// <param name="echo">Information about the echo</param>
        public CompanyInstance getCompany(CompanyInstance company)
        {

            if(openConnection() == true)
            {
                string query = "SELECT * FROM companylisting WHERE companyname = '" + company.companyName + "';";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                CompanyInstance c = null;
                while (reader.Read())
                {
                    String[] list = new String[1];
                    list[0] = reader.GetString("locations");

                   c = new CompanyInstance(reader.GetString("companyname"), reader.GetString("phonenumber"), reader.GetString("email"), list);
                }

                closeConnection();  
                return c;
            }
            else
            {
                throw new Exception("Unable to connect to database.");
            }
        }

        public CompanySearchResponse searchCompany(string companyName)
        {
            Console.WriteLine("Searching for " + companyName);

            CompanyList companyList = new CompanyList();
            List<string> companyNamesResult = new List<string>();

            if (openConnection() == true)
            {
                string query = $@"SELECT * FROM companylistingdb.companylisting where companyname like '%{companyName}%';";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    companyNamesResult.Add(reader.GetString("companyname"));
                }

                closeConnection();
                companyList.companyNames = companyNamesResult.ToArray();

                for (int i = 0; i < companyNamesResult.Count; i++)
                    Console.WriteLine(companyNamesResult[i]);

                if(companyNamesResult.Count == 0)
                    return new CompanySearchResponse(false, "No companies found matching '" + companyName + "'", null);
                else
                    return new CompanySearchResponse(true, "Companies found", companyList);
            }
            else
            {
                throw new Exception("Unable to connect to database");
            }            
        }
    }

    /// <summary>
    /// This portion of the class contains the properties and variables 
    /// </summary>
    public partial class CompanyListingDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>
        private const String dbname = "companylistingdb";
        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton isntance of the database
        /// </summary>
        protected static CompanyListingDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
            (
                dbname,
                "companylisting",
                new Column[]
                {
                    new Column
                    (
                        "id", "INT(32)",
                        new string[]
                        {
                            "NOT NULL",
                            "UNIQUE",
                            "AUTO_INCREMENT"
                        }, true
                    ),
                    new Column
                    (
                        "timestamp", "INT(32)",
                        new string[]
                        {
                            "NOT NULL",
                        }, false
                    ),
                    new Column
                    (
                        "companyname", "VARCHAR(100)",
                        new string[] 
                        {
                            "NOT NULL",
                            "UNIQUE"
                        },
                        false
                    ),
                    new Column
                    (
                        "phonenumber", "VARCHAR(10)",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                    new Column
                    (
                        "email", "VARCHAR(100)",
                        new string[]
                        {
                            "NOT NULL",
                            "UNIQUE"
                        }, false
                    ),
                    new Column
                    (
                        "locations", "VARCHAR(1000)",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                }
            )
        };
    }
}
