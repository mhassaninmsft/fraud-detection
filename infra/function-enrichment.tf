resource "azurerm_service_plan" "app_service_plan" {
  name                = "app-serviceplan-${var.environment}"
  resource_group_name = azurerm_resource_group.main_resource_group.name
  location            = azurerm_resource_group.main_resource_group.location
  os_type             = "Linux"
  sku_name            = "Y1"
}

resource "azurerm_linux_function_app" "txenhancement_function" {
  name                = "${var.acr_image_name}-function"
  resource_group_name = azurerm_resource_group.main_resource_group.name
  location            = azurerm_resource_group.main_resource_group.location

  storage_account_name       = azurerm_storage_account.storage_main.name
  storage_account_access_key = azurerm_storage_account.storage_main.primary_access_key
  service_plan_id            = azurerm_service_plan.app_service_plan.id

  site_config {
    # minimum_tls_version                     = "1.2"
    # always_on                               = "true"
    container_registry_use_managed_identity = true

    application_stack {
      docker {
        registry_url = var.acr_regitry_url
        image_name   = var.acr_image_name
        image_tag    = var.acr_image_tag
      }

    }
  }

  app_settings = {
    "MyEnv" : "Hello"
    "ConnectionString" : "dsa"
  }
}
