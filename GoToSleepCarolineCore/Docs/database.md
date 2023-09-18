# Database 

This file contains the database structure and the queries used to create the database.

## Structure

The database is composed of 4 tables:

- **Users**: contains the users' information
- **Actions**: contains the actions to be performed by the bot
- **ActionTypes**: contains the types of actions that can be performed by the bot
- **Logs**: contains the logs of the bot

## Users

The users table contains the following fields:

| Field       | Type    | Description                                         | Additional information | Nullable |
|-------------|---------|-----------------------------------------------------|------------------------|----------|
| Id          | serial  | Unique identifier of the user. Provided by discord. | Primary key            | No       |
| Username    | string  | Username of the user. Provided by discord.          |                        | No       |
| DisplayName | string  | Display name of the user. Provided by discord.      |                        | Yes      |
| IsAdmin     | boolean | Whether the user is a bot super-admin or not.       | Default false          | No       |
| IsBanned    | boolean | Whether the user is banned or not.                  | Default false          | No       |
| AddedOn     | date    | Date when the user was added to the database.       | Default right now      | No       |

## Actions

The actions table contains the following fields:

| Field        | Type    | Description                                    | Additional information               | Nullable |
|--------------|---------|------------------------------------------------|--------------------------------------|----------|
| Id           | integer | Unique identifier of the action.               | Primary key, autoincrement           | No       |
| CreatedBy    | serial  | User who created the action.                   | Foreign key to the Users table       | No       |
| ActionType   | integer | Type of the action.                            | Foreign key to the ActionTypes table | No       |
| ActionData   | string  | JSON formatted data of the action.             | Default `{}`                         | No       |
| ActionTime   | time    | Time when the action should be performed.      |                                      | No       |
| ActionDate   | date    | Date when the action should be performed.      |                                      | Yes      |
| RepeatAction | boolean | Whether the action should be repeated or not.  | Default False                        | No       |
| TriggerCount | integer | Number of times the action has been triggered. | Default 0                            | No       |
| CreatedOn    | date    | Date when the action was created.              | Default right now                    | No       |

## ActionTypes

The ActionTypes table contains the following fields:

| Field       | Type    | Description                                        | Additional information     | Nullable |
|-------------|---------|----------------------------------------------------|----------------------------|----------|
| Id          | integer | Unique identifier of the action type.              | Primary key, autoincrement | No       |
| Name        | string  | Name of the action type.                           |                            | No       |
| Description | string  | Description of the action type.                    |                            | No       |
| CreatedOn   | date    | Date when the action type was created.             | Default right now          | No       |

## Logs

The Logs table contains the following fields:

| Field      | Type    | Description                     | Additional information         | Nullable |
|------------|---------|---------------------------------|--------------------------------|----------|
| Id         | integer | Unique identifier of the log.   | Primary key, autoincrement     | No       |
| CreatedBy  | serial  | User who created the log.       | Foreign key to the Users table | No       |
| LogLevel   | integer | Level of the log.               |                                | No       |
| LogMessage | string  | Message of the log.             |                                | No       |
| LogData    | string  | JSON formatted data of the log. | Default `{}`                   | No       |
| CreatedOn  | date    | Date when the log was created.  | Default right now              | No       |

# Create database

```sqlite
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Username string NOT NULL,
    DisplayName string,
    IsAdmin BOOLEAN DEFAULT FALSE NOT NULL,
    IsBanned BOOLEAN DEFAULT FALSE NOT NULL,
    AddedOn DATE DEFAULT CURRENT_DATE NOT NULL
);

CREATE TABLE Actions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CreatedBy INTEGER NOT NULL,
    ActionType INTEGER NOT NULL,
    ActionData string DEFAULT '{}' NOT NULL,
    ActionTime TIME NOT NULL,
    ActionDate DATE,
    RepeatAction BOOLEAN DEFAULT FALSE NOT NULL,
    TriggerCount INTEGER DEFAULT 0 NOT NULL,
    CreatedOn DATE DEFAULT CURRENT_DATE NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    FOREIGN KEY (ActionType) REFERENCES ActionTypes(Id)
);

CREATE TABLE ActionTypes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name string NOT NULL,
    Description string NOT NULL,
    CreatedOn DATE DEFAULT CURRENT_DATE NOT NULL
);

CREATE TABLE Logs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CreatedBy INTEGER NOT NULL,
    LogLevel INTEGER NOT NULL,
    LogMessage string NOT NULL,
    LogData string DEFAULT '{}' NOT NULL,
    CreatedOn DATE DEFAULT CURRENT_DATE NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);
```   