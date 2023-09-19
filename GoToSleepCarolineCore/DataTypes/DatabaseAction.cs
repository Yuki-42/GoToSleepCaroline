using Newtonsoft.Json.Linq;

namespace GoToSleepCaroline.DataTypes;

/// <summary>
/// Represents a row in the Actions table.
/// </summary>
public class DatabaseAction
{
    /// <summary>
    /// The ID of the action.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The ID of the user who created the action.
    /// </summary>
    public ulong CreatedBy { get; set; }
    
    /// <summary>
    /// The ID of the action type.
    /// </summary>
    public int ActionType { get; set; }
    
    /// <summary>
    /// The data of the action.
    /// </summary>
    public JObject ActionData { get; set; }
    
    /// <summary>
    /// The time at which the action will be performed.
    /// </summary>
    public TimeOnly ActionTime { get; set; }
    
    /// <summary>
    /// The date on which the action will be performed (if applicable).
    /// </summary>
    public DateOnly? ActionDate { get; set; }
    
    /// <summary>
    /// The action will be repeated every day at the specified time.
    /// </summary>
    public bool RepeatAction { get; set; }
    
    /// <summary>
    /// The number of times the action has been triggered.
    /// </summary>
    public int TriggerCount { get; set; }
    
    /// <summary>
    /// The date on which the action was created.
    /// </summary>
    public DateOnly CreatedOn { get; set; }
    
    /// <summary>
    /// Creates a new instance of the <see cref="DatabaseAction"/> class. 
    /// </summary>
    /// <param name="id">The ID of the action.</param>
    /// <param name="createdBy">The ID of the user who created the action.</param>
    /// <param name="actionType">The ID of the action type.</param>
    /// <param name="actionData">The data of the action.</param>
    /// <param name="actionTime">The time at which the action will be performed.</param>
    /// <param name="actionDate">The date on which the action will be performed.</param>
    /// <param name="repeatAction">The action will be repeated every day at the specified time.</param>
    /// <param name="triggerCount">The number of times the action has been triggered.</param>
    /// <param name="createdOn">The date on which the action was created.</param>
    
    public DatabaseAction(int id, ulong createdBy, int actionType, JObject actionData, TimeOnly actionTime, DateOnly? actionDate, bool repeatAction, int triggerCount, DateOnly createdOn)
    {
        // Set provided values
        Id = id;
        CreatedBy = createdBy;
        ActionType = actionType;
        ActionData = actionData;
        ActionTime = actionTime;
        ActionDate = actionDate;
        RepeatAction = repeatAction;
        TriggerCount = triggerCount;
        CreatedOn = createdOn;
    }
}