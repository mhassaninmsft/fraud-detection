resource "azurerm_service_plan" "app_service_plan" {
  name                = "app-serviceplan-${var.environment}"
  resource_group_name = azurerm_resource_group.main_resource_group.name
  location            = azurerm_resource_group.main_resource_group.location
  os_type             = "Linux"
  sku_name            = "EP1"
}

resource "azurerm_linux_function_app" "txenhancement_function" {
  name                = "transactionenhancement2-function"
  resource_group_name = azurerm_resource_group.main_resource_group.name
  location            = azurerm_resource_group.main_resource_group.location

  storage_account_name       = azurerm_storage_account.storage_main.name
  storage_account_access_key = azurerm_storage_account.storage_main.primary_access_key
  service_plan_id            = azurerm_service_plan.app_service_plan.id

  site_config {
    # minimum_tls_version                     = "1.2"
    # always_on                               = "true"
    # container_registry_use_managed_identity = true

    application_stack {
      docker {
        registry_url      = var.acr_regitry_url
        image_name        = "transactionenhancement2" # fixed due to VS docker image tag limiations
        image_tag         = var.acr_image_tag
        registry_password = var.acr_password
        registry_username = var.acr_username
      }
      #   use_dotnet_isolated_runtime = true
      #   dotnet_version              = "7.0"

    }
  }

  app_settings = {
    "MyEnv" : "Hello"
    # "ConnectionString" : "dsa"
  }
}
