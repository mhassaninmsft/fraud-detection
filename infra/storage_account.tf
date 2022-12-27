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

resource "azurerm_storage_share" "container_mount" {
  name                 = "containermount"
  storage_account_name = azurerm_storage_account.storage_main.name
  quota                = 1

  # acl {
  #   id = "MTIzNDU2Nzg5MDEyMzQ1Njc4OTAxMjM0NTY3ODkwMTI"

  #   access_policy {
  #     permissions = "rwdl"
  #     start       = "2019-07-02T09:38:21.0000000Z"
  #     expiry      = "2019-07-02T10:38:21.0000000Z"
  #   }
  # }
}
