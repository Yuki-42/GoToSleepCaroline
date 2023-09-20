using DisCatSharp;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;
using Microsoft.Extensions.DependencyInjection;

namespace GoToSleepCaroline;

[EventHandler]
public class Events
{
    public Database CommandDatabase { private get; set; }
    [Event]
    public async Task MessageCreated(DiscordClient client, MessageCreateEventArgs eventArgs)
    {
        // Check that the message is not from a bot
        if (eventArgs.Author.IsBot)
        {
            return;
        }
        
        CommandDatabase = eventArgs.ServiceProvider.GetRequiredService<Database>();

        // Check if the user exists in the database
        if (!CommandDatabase.CheckUserExists(eventArgs.Author.Id))
        {
            CommandDatabase.AddUser(eventArgs.Author.Id, eventArgs.Author.Username, eventArgs.Author.GlobalName, null, null);
        }
    }
    
    [Event]
    public async Task Ready(DiscordClient client, ReadyEventArgs eventArgs)
    {
        // Set the activity
        await client.UpdateStatusAsync(new DiscordActivity("you sleep.", ActivityType.Watching));
    }
}