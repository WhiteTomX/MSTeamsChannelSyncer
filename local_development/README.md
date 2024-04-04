# Local Development

This folders contains the terraform files to create a service principle to use while developing locally instead of the managed Identity.

### App settings

Add the required app settings to `src\local.settings.json`

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

### Authenticate to Azure

Due to a [missing feature](https://github.com/Azure/azure-cli/issues/22775) in the azure cli, we can't use a simple login to use it in the background.

#### Create Service Principle

We need a service principle with appropriate application permissions. Use provided terraform files in this folder.

#### Use service principle

To use the Service principle, it should be possible to use the service principle to login to az cli. But this lead to `Caller does not have the required permissions for accessing this API. AllowedPermissions:'ChannelMember.Read.All,ChannelMember.ReadWrite.All'`. It worked to set the environment variables:

```
AZURE_CLIENT_ID
AZURE_TENANT_ID 
AZURE_CLIENT_SECRET 
```

### Execute function

To execute a non HTTP triggered function you can post `http://localhost:7044/admin/functions/Management`

```json
{
    "input": null
}
```
