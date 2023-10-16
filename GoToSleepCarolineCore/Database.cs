using GoToSleepCaroline.DataTypes;
using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;

namespace GoToSleepCaroline;

/// <summary>
/// Interfaces with the local database to store and retrieve scheduled actions.
/// </summary>
public class Database
{
    /// <summary>
    /// The connection to the database.
    /// </summary>
    private readonly SqliteConnection _connection;
    
    /// <summary>
    /// Initialises a new instance of the <see cref="Database"/> class.
    /// </summary>
    /// <param name="databasePath">The path to the database.</param>
    /// <exception cref="SqliteException">Thrown when there is errors with the database.</exception>
    public Database(string databasePath)
    {
        // Connect to the database
        _connection = new SqliteConnection($"Data Source={databasePath};");
        _connection.Open();
        
        // Get List of Tables in Database
        SqliteCommand command = new("SELECT name FROM sqlite_master WHERE type='table';", _connection);
        SqliteDataReader reader = command.ExecuteReader();
        
        // Ensure that all tables exist
        if (!reader.HasRows)
        {
            throw new SqliteException("The database is missing tables.", 1);
        }
        
        // Ensure that the tables are named correctly
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
                throw new SqliteException("The database is missing tables.", 1);
            }
        }

        
        if (!tableNames.Contains("Users"))
        {
            // Create the table if it doesn't exist
            command = new SqliteCommand("""
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
            command = new SqliteCommand("""
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
            command = new SqliteCommand("""
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
            command = new SqliteCommand("""
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
            SqliteCommand command = new("SELECT * FROM Users;", _connection);
            SqliteDataReader reader = command.ExecuteReader();
            
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
            SqliteCommand command = new("SELECT * FROM Actions;", _connection);
            SqliteDataReader reader = command.ExecuteReader();
            
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
            SqliteCommand command = new("SELECT * FROM ActionTypes;", _connection);
            SqliteDataReader reader = command.ExecuteReader();
            
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
            SqliteCommand command = new("SELECT * FROM Logs;", _connection);
            SqliteDataReader reader = command.ExecuteReader();
            
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
        SqliteCommand command = new($"SELECT * FROM Users WHERE Id = {userId};", _connection);
        SqliteDataReader reader = command.ExecuteReader();
        
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
    /// <param name="isAdmin">The admin status of the user.</param>
    /// <param name="isBanned">The banned status of the user.</param>
    public void AddUser(ulong userId, string username, string? displayName, bool? isAdmin, bool? isBanned, bool doReturn = true)
    {
        // Create the command 
        SqliteCommand command = new($"INSERT INTO Users (Id, Username, DisplayName, IsAdmin, IsBanned) VALUES ({userId}, '{username}', '{displayName}', '{isAdmin}', '{isBanned}');", _connection);
        command.ExecuteNonQuery();
    }
    

    /// <summary>
    /// Adds an action to the database.
    /// </summary>
    /// <param name="createdBy">The ID of the user who created the action.</param>
    /// <param name="actionType">The ID of the action type.</param>
    /// <param name="actionData">The data of the action.</param>
    /// <param name="actionTime">The time at which the action will be performed.</param>
    /// <param name="actionDate">The date on which the action will be performed.</param>
    /// <param name="repeatAction">The action will be repeated every day at the specified time.</param>
    /// <param name="triggerCount">The number of times the action has been triggered.</param>
    public void AddAction(ulong createdBy, int actionType, JObject actionData, TimeOnly actionTime, DateOnly? actionDate, bool repeatAction, int triggerCount)
    {
        // Create the command
        SqliteCommand command = new($"INSERT INTO Actions (CreatedBy, ActionType, ActionData, ActionTime, ActionDate, RepeatAction, TriggerCount) VALUES ({createdBy}, {actionType}, '{actionData}', '{actionTime}', '{actionDate}', '{repeatAction}', '{triggerCount}');", _connection);
        command.ExecuteNonQuery();
    }
    
    /// <summary>
    /// Adds an action type to the database.
    /// </summary>
    /// <param name="name">The name of the action type.</param>
    /// <param name="description">The description of the action type.</param>
    public void AddActionType(string name, string description)
    {
        // Create the command
        SqliteCommand command = new($"INSERT INTO ActionTypes (Name, Description) VALUES ('{name}', '{description}');", _connection);
        command.ExecuteNonQuery();
    }
    
    /// <summary>
    /// Adds a log to the database.
    /// </summary>
    /// <param name="createdBy">The ID of the user who created the log.</param>
    /// <param name="logLevel">The log level of the log.</param>
    /// <param name="logMessage">The message of the log.</param>
    /// <param name="logData">The data of the log.</param>
    public void AddLog(ulong createdBy, int logLevel, string logMessage, JObject logData)
    {
        // Create the command
        SqliteCommand command = new($"INSERT INTO Logs (CreatedBy, LogLevel, LogMessage, LogData) VALUES ({createdBy}, {logLevel}, '{logMessage}', '{logData}');", _connection);
        command.ExecuteNonQuery();
    }
}