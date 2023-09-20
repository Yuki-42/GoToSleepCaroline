using GoToSleepCaroline.DataTypes;
using Newtonsoft.Json.Linq;

namespace GoToSleepCaroline;

using System.Data.SQLite;
/// <summary>
/// Interfaces with the local database to store and retrieve scheduled actions.
/// </summary>
public class Database
{
    /// <summary>
    /// The connection to the database.
    /// </summary>
    private readonly SQLiteConnection _connection;
    
    /// <summary>
    /// Initialises a new instance of the <see cref="Database"/> class.
    /// </summary>
    /// <param name="databasePath">The path to the database.</param>
    /// <exception cref="SQLiteException">Thrown when there is errors with the database.</exception>
    public Database(string databasePath)
    {
        // Connect to the database
        _connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
        _connection.Open();
        
        // Get List of Tables in Database
        SQLiteCommand command = new("SELECT name FROM sqlite_master WHERE type='table';", _connection);
        SQLiteDataReader reader = command.ExecuteReader();
        
        // Ensure that all tables exist
        if (!reader.HasRows)
        {
            throw new SQLiteException("The database is missing tables.");
        }
        
        // Ensure that the tables are named correctly
        // TODO: Make it ask for user input, and if it gets a yes, create the tables automatically 
        List<string> tableNames = new();
        while (reader.Read())
        {
            tableNames.Add(reader.GetString(0));
        }
        
        // Check if the tables exist
        if (!tableNames.Contains("Users") && !tableNames.Contains("Actions") && !tableNames.Contains("ActionTypes") && !tableNames.Contains("Logs"))
        {
            // Ask the user if they want to create the tables
            Console.WriteLine("The database is missing tables. Would you like to create them? (y/n)");
            string input = Console.ReadLine() ?? throw new InvalidOperationException("The user input is null.");
            
            // Check if the user wants to create the tables
            if (!input.Equals("y"))
            {
                throw new SQLiteException("The database is missing tables.");
            }
        }

        
        if (!tableNames.Contains("Users"))
        {
            // Create the table if it doesn't exist
            command = new SQLiteCommand("""
                                        CREATE TABLE Users (
                                            Id SERIAL PRIMARY KEY,
                                            Username string NOT NULL,
                                            DisplayName string,
                                            IsAdmin BOOLEAN DEFAULT FALSE NOT NULL,
                                            IsBanned BOOLEAN DEFAULT FALSE NOT NULL,
                                            AddedOn DATE DEFAULT CURRENT_DATE NOT NULL
                                        );
                                        """);
            command.ExecuteNonQuery();
        }

        if (!tableNames.Contains("Actions"))
        {
            command = new SQLiteCommand("""
                                        CREATE TABLE Actions (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            CreatedBy INTEGER NOT NULL,
                                            ActionType INTEGER NOT NULL,
                                            ActionData string DEFAULT '{}' NOT NULL,
                                            ActionTime TIME NOT NULL,
                                            ActionDate DATE,
                                            RepeatAction BOOLEAN DEFAULT FALSE NOT NULL,
                                            TriggerCount INTEGER DEFAULT 0 NOT NULL,
                                            CreatedOn DATE DEFAULT CURRENT_DATE NOT NULL,
                                            FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
                                            FOREIGN KEY (ActionType) REFERENCES ActionTypes(Id)
                                        );
                                        """);
            command.ExecuteNonQuery();
        }

        if (!tableNames.Contains("ActionTypes"))
        {
            command = new SQLiteCommand("""
                                        CREATE TABLE ActionTypes (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Name string NOT NULL,
                                            Description string NOT NULL,
                                            CreatedOn DATE DEFAULT CURRENT_DATE NOT NULL
                                        );
                                        """);
            command.ExecuteNonQuery();
        }
        
        if (!tableNames.Contains("Logs"))
        {
            command = new SQLiteCommand("""
                                        CREATE TABLE Logs (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            CreatedBy INTEGER NOT NULL,
                                            LogLevel INTEGER NOT NULL,
                                            LogMessage string NOT NULL,
                                            LogData string DEFAULT '{}' NOT NULL,
                                            CreatedOn DATE DEFAULT CURRENT_DATE NOT NULL,
                                            FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
                                        );
                                        """);
            command.ExecuteNonQuery();
        }
    }

    /*
=========================================================================================================================================================
        * Properties
=========================================================================================================================================================
     */
    
    /// <summary>
    /// All users in the database.
    /// </summary>
    public List<DatabaseUser> Users
    {
        get
        {
            SQLiteCommand command = new("SELECT * FROM Users;", _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            List<DatabaseUser> users = new();
            
            while (reader.Read())
            {
                users.Add(new DatabaseUser(
                    (ulong)reader.GetInt64(0), 
                    reader.GetString(1), 
                    reader.GetString(2), 
                    reader.GetBoolean(3), 
                    reader.GetBoolean(4), 
                    DateOnly.Parse(reader.GetString(5)))
                );
            }
            
            return users;
        }
    }
    
    /// <summary>
    /// All actions in the database.
    /// </summary>
    public List<DatabaseAction> Actions
    {
        get
        {
            // Create the command
            SQLiteCommand command = new("SELECT * FROM Actions;", _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            List<DatabaseAction> actions = new();
            
            // Read the data
            while (reader.Read())
            {
                // Add the data to the list
                actions.Add(new DatabaseAction(
                    reader.GetInt32(0), 
                    (ulong)reader.GetInt64(1), 
                    reader.GetInt32(2), 
                    JObject.Parse(reader.GetString(3)),
                    TimeOnly.Parse(reader.GetString(4)), 
                    // Attempt to parse the date, if the reader returns Null then set the value to null
                    reader.IsDBNull(5) ? null : DateOnly.Parse(reader.GetString(5)), 
                    reader.GetBoolean(6), 
                    reader.GetInt32(7), 
                    DateOnly.Parse(reader.GetString(8)))
                );
            }
            
            return actions;
        }
    }
    
    /// <summary>
    /// All action types in the database.
    /// </summary>
    public List<DatabaseActionType> ActionTypes
    {
        get
        {
            SQLiteCommand command = new("SELECT * FROM ActionTypes;", _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            List<DatabaseActionType> actionTypes = new();
            
            while (reader.Read())
            {
                actionTypes.Add(new DatabaseActionType(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), DateOnly.Parse(reader.GetString(3))));
            }

            return actionTypes;
        }
    }
    
    /// <summary>
    /// All logs in the database.
    /// </summary>
    public List<DatabaseLog> Logs
    {
        get
        {
            SQLiteCommand command = new("SELECT * FROM Logs;", _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            List<DatabaseLog> logs = new();
            
            while (reader.Read())
            {
                logs.Add(new DatabaseLog(
                    reader.GetInt32(0), 
                    (ulong)reader.GetInt64(1), 
                    reader.GetInt32(2), 
                    reader.GetString(3), 
                    JObject.Parse(reader.GetString(4)), 
                    reader.GetDateTime(5))
                );
            }
            
            return logs;
        }
    }


    
    
    /*
=========================================================================================================================================================
     * Check methods
=========================================================================================================================================================
     */
    
    public bool CheckUserExists(ulong userId)
    {
        SQLiteCommand command = new SQLiteCommand($"SELECT * FROM Users WHERE Id = {userId};", _connection);
        SQLiteDataReader reader = command.ExecuteReader();
        
        return reader.HasRows;
    }
    
    /*
=========================================================================================================================================================
        * Add methods
=========================================================================================================================================================
        */

    /// <summary>
    /// Adds a user to the database.
    /// </summary>
    /// <param name="userId">The ID of the user to add.</param>
    /// <param name="username">The username of the user to add.</param>
    /// <param name="displayName">The display name of the user to add.</param>
    public void AddUser(ulong userId, string username, string displayName)
    {
        // Create the command 
        SQLiteCommand command = new($"INSERT INTO Users (Id, Username, DisplayName) VALUES ({userId}, '{username}', '{displayName}');", _connection);
        command.ExecuteNonQuery();
    }
}