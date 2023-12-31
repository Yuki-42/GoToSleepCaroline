﻿using DisCatSharp;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoToSleepCaroline;

[EventHandler]
public class Events
{
    /// <summary>
    /// The MessageCreated event handler.
    /// </summary>
    /// <param name="client">The client object.</param>
    /// <param name="eventArgs">The event arguments.</param>
    [Event(DiscordEvent.MessageCreated)]
    public async Task MessageCreated(DiscordClient client, MessageCreateEventArgs eventArgs)
    {
        // Check that the message is not from a bot
        if (eventArgs.Author.IsBot)
        {
            return;
        }
        
        Database commandDatabase = eventArgs.ServiceProvider.GetRequiredService<Database>();

        // Check if the user exists in the database
        if (!commandDatabase.CheckUserExists(eventArgs.Author.Id))
        {
            commandDatabase.AddUser(eventArgs.Author.Id, eventArgs.Author.Username, eventArgs.Author.GlobalName, null, null);
        }
    }
    
    [Event(DiscordEvent.Ready)]
    public async Task Ready(DiscordClient client, ReadyEventArgs eventArgs)
    {
        // Get service provider objects
        Utils utils = eventArgs.ServiceProvider.GetRequiredService<Utils>();
        IConfigurationRoot configurationRoot = eventArgs.ServiceProvider.GetRequiredService<IConfigurationRoot>();
        
        
        
        Console.WriteLine("ready");
        
        // Get the status vairables
        string statusType = configurationRoot["statusType"] ?? throw new ArgumentException("Missing config field 'statusType'");
        string statusText = configurationRoot["statusText"] ?? throw new ArgumentException("Missing config field 'statusText'");
        
        // Set the activity
        DiscordActivity activity = utils.ConvertActivity(statusType, statusText);
        
        await client.UpdateStatusAsync(activity, UserStatus.Online);
        
        // Create a new thread 
    }
}