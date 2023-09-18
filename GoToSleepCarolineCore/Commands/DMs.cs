using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Common.Utilities;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using Microsoft.VisualBasic;

namespace GoToSleepCaroline.Commands;

public class DMs : ApplicationCommandsModule
{
    public Database CommandDatabase { private get; set; }
    
    [SlashCommandGroup("dm", "Commands for managing scheduled DMs.")]
    public class DmCommands : ApplicationCommandsModule
    {
        [SlashCommandGroup("create", "Commands for creating scheduled DMs.")]
        public class CreateCommands : ApplicationCommandsModule
        {
            [SlashCommand("once", "Creates a scheduled DM that will be sent once.")]
            public async Task Once(
                InteractionContext context, 
                // Create the command options
                [Option("user", "The user to send the DM to.")] DiscordUser user,
                [Option("message", "The message to send.")] string message,
                [Option("time", "The time at which the DM will be sent.")] string time,
                [Option("date", "The date on which the DM will be sent.")] string date
                )
            {
            }
        }

    }
}