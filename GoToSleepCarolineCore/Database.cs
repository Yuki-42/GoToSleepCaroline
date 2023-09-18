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
        List<string> tableNames = new List<string>();
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
}