# Events

This file details all event handlers used by the bot, their purpose, and what they do. 

## Event Handlers

### DiscordEvent.Ready

This event is fired when the bot is ready to start receiving events. This is the first event that is fired when the bot is started.

#### Functionality

- Sets the bot's status to the status in the config file.
- Gets all members of all servers it is in and adds them to the database if they are not already in the database

### DiscordEvent.MessageCreated

This event is fired when a new message is created in any channel the bot has access to. 

#### Functionality 

- Add the message sender to the database if they are not a bot and are not already in the database
