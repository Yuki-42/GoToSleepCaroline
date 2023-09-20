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

| Field       | Type     | Description                                         | Additional information    | Nullable |
|-------------|----------|-----------------------------------------------------|---------------------------|----------|
| Id          | serial   | Unique identifier of the user. Provided by discord. | primary key               | No       |
| Username    | string   | Username of the user. Provided by discord.          |                           | No       |
| DisplayName | string   | Display name of the user. Provided by discord.      |                           | Yes      |
| IsAdmin     | boolean  | Whether the user is a bot super-admin or not.       | default false             | No       |
| IsBanned    | boolean  | Whether the user is banned or not.                  | default false             | No       |
| CreatedOn   | datetime | Date when the user was added to the database.       | default current_timestamp | No       |

## Actions

The actions table contains the following fields:

| Field        | Type     | Description                                    | Additional information               | Nullable |
|--------------|----------|------------------------------------------------|--------------------------------------|----------|
| Id           | integer  | Unique identifier of the action.               | primary key autoincrement            | No       |
| CreatedBy    | serial   | User who created the action.                   | Foreign key to the Users table       | No       |
| ActionType   | integer  | Type of the action.                            | Foreign key to the ActionTypes table | No       |
| ActionData   | string   | JSON formatted data of the action.             | Default `{}`                         | No       |
| ActionTime   | time     | Time when the action should be performed.      |                                      | No       |
| ActionDate   | date     | Date when the action should be performed.      |                                      | Yes      |
| RepeatAction | boolean  | Whether the action should be repeated or not.  | Default False                        | No       |
| TriggerCount | integer  | Number of times the action has been triggered. | Default 0                            | No       |
| CreatedOn    | datetime | Date when the action was created.              | Default right now                    | No       |

## ActionTypes

The ActionTypes table contains the following fields:

| Field       | Type     | Description                            | Additional information     | Nullable |
|-------------|----------|----------------------------------------|----------------------------|----------|
| Id          | integer  | Unique identifier of the action type.  | Primary key, autoincrement | No       |
| Name        | string   | Name of the action type.               |                            | No       |
| Description | string   | Description of the action type.        |                            | No       |
| CreatedOn   | datetime | Date when the action type was created. | Default right now          | No       |

## Logs

The Logs table contains the following fields:

| Field      | Type     | Description                     | Additional information         | Nullable |
|------------|----------|---------------------------------|--------------------------------|----------|
| Id         | integer  | Unique identifier of the log.   | Primary key, autoincrement     | No       |
| CreatedBy  | serial   | User who created the log.       | Foreign key to the Users table | No       |
| LogLevel   | integer  | Level of the log.               |                                | No       |
| LogMessage | string   | Message of the log.             |                                | No       |
| LogData    | string   | JSON formatted data of the log. | Default `{}`                   | No       |
| CreatedOn  | datetime | Date when the log was created.  | Default right now              | No       |

# Create database

```sqlite
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Username string NOT NULL,
    DisplayName string,
    IsAdmin BOOLEAN DEFAULT FALSE NOT NULL,
    IsBanned BOOLEAN DEFAULT FALSE NOT NULL,
    CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL
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
    CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    FOREIGN KEY (ActionType) REFERENCES ActionTypes(Id)
);

CREATE TABLE ActionTypes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name string NOT NULL,
    Description string NOT NULL,
    CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE Logs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CreatedBy INTEGER NOT NULL,
    LogLevel INTEGER NOT NULL,
    LogMessage string NOT NULL,
    LogData string DEFAULT '{}' NOT NULL,
    CreatedOn DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);
```   


# Database Code Representation

This section describes how the database is accessed through the code and how the data is represented.

## Custom Database Types

### DatabaseUser

This type is used to represent a user in the database.

#### Properties

| Name        | Type     | Description                                         |
|-------------|----------|-----------------------------------------------------|
| Id          | ulong    | Unique identifier of the user. Provided by discord. |
| Username    | string   | Username of the user. Provided by discord.          |
| DisplayName | string?  | Display name of the user. Provided by discord.      |
| IsAdmin     | bool     | Whether the user is a bot super-admin or not.       |
| IsBanned    | bool     | Whether the user is banned or not.                  |
| AddedOn     | DateTime | Date when the user was added to the database.       |

### DatabaseAction

This type is used to represent an action in the database.

#### Properties

| Name         | Type      | Description                                    |
|--------------|-----------|------------------------------------------------|
| Id           | int       | Unique identifier of the action.               |
| CreatedBy    | ulong     | User who created the action.                   |
| ActionType   | int       | Type of the action.                            |
| ActionData   | JObject   | JSON formatted data of the action.             |
| ActionTime   | TimeOnly  | Time when the action should be performed.      |
| ActionDate   | DateOnly? | Date when the action should be performed.      |
| Repeat       | bool      | Whether the action should be repeated.         |
| TriggerCount | int       | Number of times the action has been triggered. |
| CreatedOn    | DateTime  | Date when the action was created.              |

### DatabaseActionType

This type is used to represent an action type in the database.

#### Properties

| Name        | Type     | Description                                        |
|-------------|----------|----------------------------------------------------|
| Id          | int      | Unique identifier of the action type.              |
| Name        | string   | Name of the action type.                           |
| Description | string   | Description of the action type.                    |
| CreatedOn   | DateTime | Date when the action type was created.             |

### DatabaseLog

This type is used to represent a log in the database.

| Name       | Type     | Description                     |
|------------|----------|---------------------------------|
| Id         | int      | Unique identifier of the log.   |
| CreatedBy  | ulong    | User who created the log.       |
| LogLevel   | int      | Level of the log.               |
| LogMessage | string   | Message of the log.             |
| LogData    | JObject  | JSON formatted data of the log. |
| CreatedOn  | DateTime | Date when the log was created.  |

## Database Class

This class is used to interact with the database described above.

## Properties

| Name  | Type               | Description                   |
|-------|--------------------|-------------------------------|
| db    | SQLiteConnection   | Database object               |
| Users | List<DatabaseUser> | List of users in the database |

## Methods

This section details the different methods in the Database class, sorted by their effect on the database.

### Add methods 

This section details all the methods used to add data to the database.

#### AddUser

This method adds a user to the database using the provided information.

##### Parameters

| Name        | Type    | Description                                         | Nullable |
|-------------|---------|-----------------------------------------------------|----------|
| userId      | ulong   | Unique identifier of the user. Provided by discord. | No       |
| username    | string  | Username of the user. Provided by discord.          | No       |
| displayName | string? | Display name of the user. Provided by discord.      | Yes      |
| isAdmin     | bool?   | The admin status of the user                        | yes      | 

### Remove methods 

This section details all the methods used to remove data from the database.

### Alter Methods 

This section details all the methods used to alter data in the database.