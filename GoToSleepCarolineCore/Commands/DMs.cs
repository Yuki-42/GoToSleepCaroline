using System.Globalization;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using Newtonsoft.Json.Linq;

namespace GoToSleepCaroline.Commands;

public class DMs : ApplicationCommandsModule
{
    [SlashCommandGroup("dm", "Commands for managing scheduled DMs.")]
    public class DmCommands : ApplicationCommandsModule
    {
        [SlashCommandGroup("create", "Commands for creating scheduled DMs.")]
        public class CreateCommands : ApplicationCommandsModule
        {
            public Database CommandDatabase { private get; set; }

            [SlashCommand("once", "Creates a scheduled DM that will be sent once.")]
            public async Task Once(
                InteractionContext context, 
                // Create the command options
                [Option("user", "The user to send the DM to.")] DiscordUser user,
                [Option("message", "The message to send.")] string message,
                [Option("time", "The time at which the DM will be sent. Formatted as 'h:mm tt'")] string timeS,
                [Option("date", "The date on which the DM will be sent. Formatted as 'dd MM yyyy'")] string dateS
                )
            {
                // Check if the user exists in the database
                if (!CommandDatabase.CheckUserExists(context.User.Id))
                {
                    CommandDatabase.AddUser(context.User.Id, context.User.Username, context.User.GlobalName, null, null);
                }
                
                // Check that the user to send the DM to exists in the database
                if (!CommandDatabase.CheckUserExists(user.Id))
                {
                    CommandDatabase.AddUser(user.Id, user.Username, user.GlobalName, null, null);
                }
                
                // Parse the time and date  
                DateOnly date = DateOnly.ParseExact(dateS,  "dd MM yyyy", CultureInfo.InvariantCulture);
                TimeOnly time = TimeOnly.ParseExact(timeS, "h:mm tt", CultureInfo.InvariantCulture);
                
                // Create the action
                CommandDatabase.AddAction(context.User.Id, 1, JObject.FromObject(new
                {
                    target = user.Id,
                    messageText = message
                }), time, date, false);
                
                // Send the confirmation message
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                {
                    Content = $"Created a scheduled DM to {user.Mention} at {time.ToString()} on {date.ToString()}."
                });
            }
            
            [SlashCommand("repeat", "Creates a scheduled DM that will be sent repeatedly.")]
            public async Task Repeat(
                InteractionContext context, 
                // Create the command options
                [Option("user", "The user to send the DM to.")] DiscordUser user,
                [Option("message", "The message to send. Formatted as 'h:mm tt'")] string message,
                [Option("time", "The time at which the DM will be sent. Formatted as 'dd MM yyyy'")] string time
            )
            {
                // Check if the user exists in the database
                if (!CommandDatabase.CheckUserExists(context.User.Id))
                {
                    CommandDatabase.AddUser(context.User.Id, context.User.Username, context.User.GlobalName, null, null);
                }
                
                // Check that the user to send the DM to exists in the database
                if (!CommandDatabase.CheckUserExists(user.Id))
                {
                    CommandDatabase.AddUser(user.Id, user.Username, user.GlobalName, null, null);
                }
                
                // Parse the time and date  
                TimeOnly timeOnly = TimeOnly.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture);
                
                // Create the action
                CommandDatabase.AddAction(context.User.Id, 2, JObject.FromObject(new
                {
                    target = user.Id,
                    messageText = message
                }), timeOnly, null, true);
                
                // Send the confirmation message
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                {
                    Content = $"Created a scheduled DM to {user.Mention} at {timeOnly.ToString()} every day."
                });
            }
        }
    }
}