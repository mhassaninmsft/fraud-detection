resource "azurerm_storage_account" "storage_main" {
  name                     = "${var.resourceNamePrefix}${var.environment}${var.location}str"
  resource_group_name      = azurerm_resource_group.main_resource_group.name
  location                 = azurerm_resource_group.main_resource_group.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = {
    environment = var.environment
  }
}
