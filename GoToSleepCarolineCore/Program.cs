using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Timers;
using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.CommandsNext;
using DisCatSharp.Enums;
using GoToSleepCaroline.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace GoToSleepCaroline
{
    internal class Program
    {
        /// <summary>
        /// Stores all timed actions in their relevant threads.
        /// </summary>
        public List<Thread> Actions = new List<Thread>();
        
        
        private DiscordClient Client { get; set; }
        
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
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile($"BotData/config.json");

            // Assign the config
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            
            // Initialise the bot
            DiscordConfiguration configuration = new DiscordConfiguration()
            {
                Token = configurationRoot["Token"] ?? throw new ArgumentNullException("Token", "The bot token is null."),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.DirectMessages
            };
            
            // Create the database connection 
            Database database = new Database(configurationRoot["DatabasePath"] ?? throw new ArgumentNullException("DatabasePath", "The database path is null."));
            
            // Create the client
            DiscordClient client = new DiscordClient(configuration);
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton(database);
            
            ApplicationCommandsExtension commands = client.UseApplicationCommands(new ApplicationCommandsConfiguration
                {
                    ServiceProvider = services.BuildServiceProvider()
                });
            
            commands.RegisterGlobalCommands<DMs>();
            
            // Connect the client
            await client.ConnectAsync();
            
            // Prevent the application from closing
            await Task.Delay(-1);
        }
    }
}