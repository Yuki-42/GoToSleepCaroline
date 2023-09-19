namespace GoToSleepCaroline.DataTypes;

/// <summary>
/// Represents a row in the Users table.
/// </summary>
public class DatabaseUser
{
    /// <summary>
    /// The ID of the user.
    /// </summary>
    public ulong Id { get; set; }
    
    /// <summary>
    /// The username of the user.
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// The display name of the user.
    /// </summary>
    public string? DisplayName { get; set; }
    
    /// <summary>
    /// The admin status of the user.
    /// </summary>
    public bool IsAdmin { get; set; }
    
    /// <summary>
    /// The banned status of the user.
    /// </summary>
    public bool IsBanned { get; set; }
    
    /// <summary>
    /// The date on which the user was added to the database.
    /// </summary>
    public DateOnly AddedOn { get; set; }
    
    /// <summary>
    /// Creates a new instance of the <see cref="DatabaseUser"/> class.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="username">The username of the user.</param>
    /// <param name="displayName">The display name of the user.</param>
    /// <param name="isAdmin">The admin status of the user.</param>
    /// <param name="isBanned">The banned status of the user.</param>
    /// <param name="addedOn">The date on which the user was added to the database.</param>
    
    public DatabaseUser(ulong id, string username, string displayName, bool isAdmin, bool isBanned, DateOnly addedOn)
    {
        // Set provided values
        Id = id;
        Username = username;
        DisplayName = displayName;
        IsAdmin = isAdmin;
        IsBanned = isBanned;
        AddedOn = addedOn;
    }
    
    /// <summary>
    /// Creates a new instance of the <see cref="DatabaseUser"/> class without a display name.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="username">The username of the user.</param>
    /// <param name="isAdmin">The admin status of the user.</param>
    /// <param name="isBanned">The banned status of the user.</param>
    /// <param name="addedOn">The date on which the user was added to the database.</param>
    public DatabaseUser(ulong id, string username, bool isAdmin, bool isBanned, DateOnly addedOn)
    {
        // Set provided values
        Id = id;
        Username = username;
        IsAdmin = isAdmin;
        IsBanned = isBanned;
        AddedOn = addedOn;
    }
}