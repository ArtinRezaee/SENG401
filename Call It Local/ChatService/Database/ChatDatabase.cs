using Messages;
using Messages.Database;
using Messages.DataTypes.Database.Chat;
using Messages.NServiceBus.Commands;
using Messages.ServiceBusRequest;

using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChatService.Database
{
    public partial class ChatDatabase : AbstractDatabase
    {
        private ChatDatabase() { }

        public static ChatDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new ChatDatabase();
            }
            return instance;
        }

        public string sendMessage(ChatMessage request)
        {
            if(openConnection() == true)
            {
                string query = $@"INSERT INTO chat(sender, receiver, content, timestamp) VALUES 
                ('{request.sender}', '{request.receiver}', '{request.messageContents}', '{DateTimeOffset.Now.ToUnixTimeSeconds().ToString()}');";                    

                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                closeConnection();  

                return "Successful";              
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
                return "Unable to connect to database.";
            }
        }

        public GetChatContacts getContacts(GetChatContacts contactsCmd)
        {
            GetChatContacts contacts = new GetChatContacts
            {
                usersname = contactsCmd.usersname,
                contactNames = new List<string>()
            };

            if(openConnection() == true)
            {

                string query = $@"(SELECT receiver FROM chat WHERE sender = '{contacts.usersname}') UNION 
                                (SELECT sender FROM chat WHERE receiver = '{contacts.usersname}');";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    contacts.contactNames.Add(reader.GetString("receiver"));
                }

                closeConnection();

                return contacts;                          
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
                return null;
            }
        }

        public ServiceBusResponse getContacts2(GetChatContacts contacts)
        {
            GetChatContacts resp = new GetChatContacts
            {
                usersname = contacts.usersname,
                contactNames = new List<string>()
            };

            if(openConnection() == true)
            {
                try
                {
                    string query = $@"SELECT DISTINCT sender,receiver FROM chat WHERE 
                                    (sender = '{contacts.usersname}' OR receiver = '{contacts.usersname}') GROUP BY sender,receiver;";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string rec = reader.GetString("receiver");
                        string sen = reader.GetString("sender");

                        if (String.Equals(sen, contacts.usersname))
                            resp.contactNames.Add(rec);
                        else
                            resp.contactNames.Add(sen);
                    }

                    closeConnection();

                    return new ServiceBusResponse(true, JsonConvert.SerializeObject(resp));
                }
                catch(Exception err)
                {
                    return new ServiceBusResponse(false, err.Message);
                }                             
            }
            else
            {
                return new ServiceBusResponse(false, "Unable to connect to database.");
            }
        }

        public GetChatHistory getHistory(GetChatHistory historyCmd)
        {
            GetChatHistory history = new GetChatHistory
            {
                history = new ChatHistory
                {
                    user1 = historyCmd.history.user1,
                    user2 = historyCmd.history.user2,
                    messages = new List<ChatMessage>()
                }
            };


            if(openConnection() == true)
            {
                string query = $@"SELECT * FROM chat WHERE (sender = '{history.history.user1}' AND receiver = '{history.history.user2}')
                    OR (sender = '{history.history.user2}' AND receiver = '{history.history.user1}');";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ChatMessage msg = new ChatMessage
                    {
                        sender = reader.GetString("sender"),
                        receiver = reader.GetString("receiver"),
                        messageContents = reader.GetString("content"),
                        unix_timestamp = reader.GetInt32("timestamp")
                    };
                    history.history.messages.Add(msg);
                }

                closeConnection();
                return history;             
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
                return null;
            }
        }
    }

    /// <summary>
    /// This portion of the class contains the properties and variables 
    /// </summary>
    public partial class ChatDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>
        private const String dbname = "chatservicedb";
        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton isntance of the database
        /// </summary>
        protected static ChatDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
            (
                dbname,
                "chat",
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
                        "sender", "VARCHAR(100)",
                        new string[] 
                        {
                            "NOT NULL"
                        },
                        false
                    ),
                    new Column
                    (
                        "receiver", "VARCHAR(100)",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                    new Column
                    (
                        "content", "VARCHAR(2000)",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                    new Column
                    (
                        "timestamp", "INT(32)",
                        new string[]
                        {
                            "NOT NULL",
                        }, false
                    )
                }
            )
        };
    }
}