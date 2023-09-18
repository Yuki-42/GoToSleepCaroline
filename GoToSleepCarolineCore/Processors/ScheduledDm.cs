﻿using Newtonsoft.Json.Linq;

namespace GoToSleepCaroline.Processors;

/// <summary>
/// Provides the functionality to send a DM to a user at a scheduled time.
/// </summary>
public class ScheduledDm
{
    /// <summary>
    /// The ID of the scheduled DM.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The ID of the user who created the scheduled DM.
    /// </summary>
    public long CreatedBy { get; set; }
    
    /// <summary>
    /// The ID of the user who will receive the scheduled DM.
    /// </summary>
    public long Target { get; set; }
    
    /// <summary>
    /// The message to be sent to the user.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// The time at which the DM will be sent.
    /// </summary>
    public TimeOnly ScheduledTime { get; set; }
    
    /// <summary>
    /// The date on which the DM will be sent.
    /// </summary>
    public DateOnly ScheduledDate { get; set; }
    
    /// <summary>
    /// If true, the DM will be sent every day at the specified time.
    /// </summary>
    public bool Repeat { get; set; }


    /// <summary>
    /// Initialises a new instance of the <see cref="ScheduledDm"/> class. This instance will not repeat. 
    /// </summary>
    /// <param name="id">The ID of the scheduled DM.</param>
    /// <param name="createdBy">The ID of the user who created the scheduled DM.</param>
    /// <param name="actionData">The data of the scheduled DM.</param>
    /// <param name="actionTime">The time at which the DM will be sent.</param>
    /// <param name="actionDate">The date on which the DM will be sent.</param>
    public ScheduledDm(int id, long createdBy, string actionData, string actionTime, string actionDate)
    {
        // Set provided values
        Id = id;
        CreatedBy = createdBy;
        Repeat = false;
        
        // Convert the time and date strings to their respective types
        try
        {
            ScheduledTime = TimeOnly.Parse(actionTime);
            ScheduledDate = DateOnly.Parse(actionDate);
        } catch (FormatException)
        {
            throw new FormatException("The provided time or date is not in the correct format.");
        }
        
        // Get all the data from the JSON string
        JObject json = JObject.Parse(actionData);
        Target = (long) (json["target"] ?? throw new MissingFieldException("target"));
        Message = (string) json["message"]! ?? throw new MissingFieldException("message");
    }
    
    /// <summary>
    /// Initialises a new instance of the <see cref="ScheduledDm"/> class. This instance will repeat.
    /// </summary>
    /// <param name="id"> The ID of the scheduled DM.</param>
    /// <param name="createdBy"> The ID of the user who created the scheduled DM.</param>
    /// <param name="actionData"> The data of the scheduled DM.</param>
    /// <param name="actionTime"> The time at which the DM will be sent.</param>
    public ScheduledDm(int id, long createdBy, string actionData, string actionTime)
    {
        // Set provided values
        Id = id;
        CreatedBy = createdBy;
        Repeat = true;
        
        // Convert the time string to its respective type
        try
        {
            ScheduledTime = TimeOnly.Parse(actionTime);
        } catch (FormatException)
        {
            throw new FormatException("The provided time is not in the correct format.");
        }
        
        // Get all the data from the JSON string
        JObject json = JObject.Parse(actionData);
        
        Target = (long) (json["target"] ?? throw new MissingFieldException("target"));
        Message = (string) json["message"]! ?? throw new MissingFieldException("message");
    }
}