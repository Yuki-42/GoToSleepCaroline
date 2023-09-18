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
        SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table';", _connection);
        SQLiteDataReader reader = command.ExecuteReader();
        
        // Ensure that all tables exist
        if (!reader.HasRows)
        {
            throw new SQLiteException("The database is missing tables.");
        }
        
        // Ensure that the tables are named correctly
        List<string> tableNames = new();
        while (reader.Read())
        {
            tableNames.Add(reader.GetString(0));
        }

        if (!tableNames.Contains("Users"))
        {
            throw new SQLiteException("The database is missing the Users table.");
        }

        if (!tableNames.Contains("Actions"))
        {
            throw new SQLiteException("The database is missing the Actions table.");
        }

        if (!tableNames.Contains("ActionTypes"))
        {
            throw new SQLiteException("The database is missing the ActionTypes table.");
        }
        
        if (!tableNames.Contains("Logs"))
        {
            throw new SQLiteException("The database is missing the Logs table.");
        }
    }

    /// <summary>
    /// All action types in the database.
    /// </summary>
    public List<Tuple<int, string, string, DateOnly>> ActionTypes
    {
        get
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM ActionTypes;", _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            List<Tuple<int, string, string, DateOnly>> actionTypes = new();
            
            while (reader.Read())
            {
                actionTypes.Add(new Tuple<int, string, string, DateOnly>(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), DateOnly.Parse(reader.GetString(3))));
            }

            return actionTypes;
        }
    }

    public List<Tuple<ulong, string, string, bool, bool, DateOnly>> Users
    {
        get
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Users;", _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            
            List<Tuple<ulong, string, string, bool, bool, DateOnly>> users = new();
            
            while (reader.Read())
            {
                users.Add(new Tuple<ulong, string, string, bool, bool, DateOnly>((ulong)reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetBoolean(3), reader.GetBoolean(4), DateOnly.Parse(reader.GetString(5))));
            }
            
            return users;
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
        Console.WriteLine("adding user");
        // Create the command 
        SQLiteCommand command = new SQLiteCommand($"INSERT INTO Users (Id, Username, DisplayName) VALUES ({userId}, '{username}', '{displayName}');", _connection);
        command.ExecuteNonQuery();
    }
}