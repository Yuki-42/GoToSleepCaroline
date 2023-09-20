using Newtonsoft.Json.Linq;

namespace GoToSleepCaroline.DataTypes;

/// <summary>
/// Represents a log entry in the database.
/// </summary>
public class DatabaseLog
{
    /// <summary>
    /// The ID of the log entry.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The ID of the user who created the log entry.
    /// </summary>
    public ulong CreatedBy { get; set; }
    
    /// <summary>
    /// The log level of the log entry.
    /// </summary>
    public int LogLevel { get; set; }
    
    /// <summary>
    /// The message of the log entry.
    /// </summary>
    public string LogMessage { get; set; }
    
    /// <summary>
    /// The data of the log entry.
    /// </summary>
    public JObject LogData { get; set; }
    
    /// <summary>
    /// The date on which the log entry was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }
    
    /// <summary>
    /// Creates a new instance of the <see cref="DatabaseLog"/> class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="logLevel"></param>
    /// <param name="logMessage"></param>
    /// <param name="logData"></param>
    /// <param name="createdOn"></param>
    public DatabaseLog(int id, ulong createdBy, int logLevel, string logMessage, JObject logData, DateTime createdOn)
    {
        // Set the properties
        Id = id;
        CreatedBy = createdBy;
        LogLevel = logLevel;
        LogMessage = logMessage;
        LogData = logData;
        CreatedOn = createdOn;
    }
}