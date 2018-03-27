using Messages;
using Messages.Database;
using Messages.DataTypes;
using Messages.DataTypes.Database.CompanyDirectory;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest.Echo.Requests;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Debug.consoleMsg("Unable to connect to database");
            }
        }

        /// <summary>
        /// Saves the reverse echo to the database
        /// </summary>
        /// <param name="echo">Information about the echo</param>
        public void getCompany()
        {

            
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
