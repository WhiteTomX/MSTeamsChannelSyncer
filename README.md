# MSTeamsChannelSyncer

Synchronize members of private Microsoft teams channels

## Roadmap

- [ ] Full synchronizations on timer
- [ ] Delta synchronization on timer
- [ ] Delta synchronization on changes
- [ ] Two way sync
- [ ] Also sync the Roles of users

## Required Configuration

The AppSettings must contain the following settings:

- `SourceTeamId`
- `SourceChannelId`
- `TargetTeamId`
- `TargetChannelId`

To obtain these information, copy the link to each channel:

`https://teams.microsoft.com/l/channel/<ChannelId>/Target?groupId=<TeamId>&tenantId=<ignore the tenant id>&ngc=true`

## Architecture

As membership changes may not happen often the solution uses serverless Azure functions. To avoid polling the API, subscriptions are used. Although full synchronization would hurt that much, the solution uses DeltaQueries to only poll changes instead of comparing all members.

```mermaid
sequenceDiagram
    participant timer as Timer
    participant graph1 as MS Graph Channel Source
    participant graph2 as MS Graph Channel Target
    participant subEndpoint as Change Endpoint
    participant storage as storage
    loop every 2 days
        activate timer
        timer->>graph1: initial Delta query ConversationMembers
        graph1->>timer: Return members & deltaLink
        timer->>storage: save deltaLink
        timer->>graph2: get members
        graph2->>timer: return members
        loop every member in channel 1
            opt is not in channel 2
                timer->>graph2: Add member
            end
        end
        loop every member in channel 2
            opt is not in channel 1
                timer->>graph2: Remove member
            end
        end
        timer->>graph1: get subscription
        alt subscription exists
            graph1->>timer: subscription object
            timer->>graph1: update subscription
        else
            graph1->>timer: 404
            timer->>graph1: Create subscription
        end
        deactivate timer
    end
    graph1->>+subEndpoint: subscription notification
    subEndpoint->>storage: Get delta link
    storage->>subEndpoint: Return delta link
    subEndpoint->>graph1: Delta Query
    graph1->>subEndpoint: Delta response
    loop every change
        subEndpoint->>graph2: add/remove member
    end
    deactivate subEndpoint
```

## Local Development

1. Add the required app settings to `src\local.settings.json`

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "SourceTeamId": "1a2132134-d3434-2134-21323-wre2342354",
    "SourceChannelId": "19%3876jhhgAjhhgugu%40thread.tacv2",
    "TargetTeamId": "1a2132134-d3434-2134-21323-wre2342354",
    "TargetChannelId": "19%Akhih987asai9uhiu%40thread.tacv2"
  }
}
```

2. Authenticate to Azure
