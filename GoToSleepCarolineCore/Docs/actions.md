# Actions

This file details the different actions that the bot can perform and how to use them.

## Table of contents

| Action name                 | Description                             |
|-----------------------------|-----------------------------------------|
| [ScheduledDM](#scheduleddm) | Sends a DM to a user at a specific time |

## ScheduledDM

Sends a DM to a user at a specific time. The message can be repeated once each day or never be repeated.

### Required Data

The following data is required to be present in the `ActionData` field of the `Actions` table:

| Field name | Type    | Description                       | Additional information |
|------------|---------|-----------------------------------|------------------------|
| target     | integer | Id of the user to send the DM to. | Foreign key to Users   |
| message    | string  | Message to send to the user.      |                        |

