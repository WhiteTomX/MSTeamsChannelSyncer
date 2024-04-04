data "azuread_client_config" "current" {

}
data "azuread_application_published_app_ids" "well_known" {}

data "azuread_service_principal" "msgraph" {
  client_id = data.azuread_application_published_app_ids.well_known.result["MicrosoftGraph"]
}

resource "azuread_application_registration" "this" {
  display_name = "sp-dev-MSTeamsChannelSyncer-LocalDevelopment"
  description  = "Used to access to locally Development"
}

resource "azuread_application_owner" "this" {
  application_id  = azuread_application_registration.this.id
  owner_object_id = data.azuread_client_config.current.object_id
}

resource "azuread_application_api_access" "msgraph" {
  application_id = azuread_application_registration.this.id
  api_client_id  = data.azuread_application_published_app_ids.well_known.result["MicrosoftGraph"]

  role_ids = [
    data.azuread_service_principal.msgraph.app_role_ids["ChannelMember.ReadWrite.All"]
  ]
}

resource "azuread_service_principal" "this" {
  client_id = azuread_application_registration.this.client_id
}


resource "azuread_service_principal" "msgraph" {
  client_id    = data.azuread_application_published_app_ids.well_known.result["MicrosoftGraph"]
  use_existing = true
}

resource "azuread_app_role_assignment" "this" {
  app_role_id         = data.azuread_service_principal.msgraph.app_role_ids["ChannelMember.ReadWrite.All"]
  principal_object_id = azuread_service_principal.this.object_id
  resource_object_id  = azuread_service_principal.msgraph.object_id
}


resource "time_rotating" "this" {
  rotation_hours = 7
}

resource "azuread_application_password" "this" {
  application_id    = azuread_application_registration.this.id
  display_name      = "LocalDevelopment"
  end_date_relative = "8h"
  rotate_when_changed = {
    rotation = time_rotating.this.id
  }
}

output "login" {
  value = "az login --service-principal -u ${azuread_service_principal.this.client_id} -p ${nonsensitive(azuread_application_password.this.value)} --tenant ${data.azuread_client_config.current.tenant_id} --allow-no-subscriptions"
}
