using System.Data;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.Enums;
using GoToSleepCaroline.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GoToSleepCaroline;

internal class Program
{
    /// <summary>
    /// Stores all timed actions in their relevant threads.
    /// </summary>
    public List<Thread> Actions = new List<Thread>();
        
        
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args"> Command line arguments.</param> 
    public static void Main(string[] args)
    {
        // Execute the async method
        MainAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// The main async entry point for the application.
    /// </summary>
    private static async Task MainAsync()
    {
        // Initialise the config file
        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.AddJsonFile($"BotData/config.json");

        // Assign the config
        IConfigurationRoot configurationRoot = configurationBuilder.Build();
            
        // Create the database connection 
        Database database = new("BotData/database.db");
        
        // Create the Utils instance
        Utils utils = new();
            
        // Set up dependency injection
        ServiceCollection serviceCollection = new();
        serviceCollection.AddSingleton(database);
        serviceCollection.AddSingleton(configurationRoot);
        serviceCollection.AddSingleton(utils);
            
        // Build the service provider
        ServiceProvider services = serviceCollection.BuildServiceProvider();
        
        // Initialise the bot
        DiscordConfiguration configuration = new()
        {
            Token = configurationRoot["token"] ?? throw new DataException("Token is null."),
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.DirectMessages,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Information,
            ServiceProvider = services
        };
            
        // Create the client
        DiscordClient client = new(configuration);
        client.RegisterEventHandlers(Assembly.GetExecutingAssembly());
            
        // Register the commands
        ApplicationCommandsExtension commands = client.UseApplicationCommands(new ApplicationCommandsConfiguration
        {
            ServiceProvider = services
        });
        commands.RegisterGlobalCommands<DMs>();
            
        // Connect the client
        await client.ConnectAsync();
            
        // Prevent the application from closing
        await Task.Delay(-1);
    }
}