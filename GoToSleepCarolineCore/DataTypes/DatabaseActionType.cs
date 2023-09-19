namespace GoToSleepCaroline.DataTypes;

/// <summary>
/// Represents a row in the ActionTypes table.
/// </summary>
public class DatabaseActionType
{
    /// <summary>
    /// The ID of the action type.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The name of the action type.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The description of the action type.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// The date on which the action type was created.
    /// </summary>
    public DateOnly CreatedOn { get; set; }
    
    /// <summary>
    /// Creates a new instance of the <see cref="DatabaseActionType"/> class.
    /// </summary>
    /// <param name="id">The ID of the action type.</param>
    /// <param name="name">The name of the action type.</param>
    /// <param name="description">The description of the action type.</param>
    /// <param name="createdOn">The date on which the action type was created.</param>
    public DatabaseActionType(int id, string name, string description, DateOnly createdOn)
    {
        // Set provided values
        Id = id;
        Name = name;
        Description = description;
        CreatedOn = createdOn;
    }
}