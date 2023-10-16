using DisCatSharp.Entities;
using Microsoft.Extensions.Configuration;

namespace GoToSleepCaroline;

public class Utils
{
    /// <summary>
    /// Converts a passed activity type and text into a DiscordActivity object.
    /// </summary>
    /// <param name="type">The type of the activity.</param>
    /// <param name="text">The text to accompany the activity.</param>
    /// <returns>The converted activity.</returns>
    public DiscordActivity ConvertActivity(string type, string text)
    {
        // Match the type to the correct activity type
        ActivityType activityType;
        
        switch (type.ToUpper())
        {
            case "PLAYING":
                activityType = ActivityType.Playing;
                break;
            case "STREAMING":
                activityType = ActivityType.Streaming;
                break;
            case "LISTENING":
                activityType = ActivityType.ListeningTo;
                break;
            case "WATCHING":
                activityType = ActivityType.Watching;
                break;
            case "COMPETING":
                activityType = ActivityType.Competing;
                break;
            default:
                throw new ArgumentException("Invalid activity type.");
        }
        
        // Return the activity
        return new DiscordActivity(text, activityType);
    }
}