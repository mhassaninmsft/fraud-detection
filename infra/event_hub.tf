resource "azurerm_eventhub_namespace" "eh_namespace" {
  name                = "${local.prefix}ehns"
  location            = azurerm_resource_group.main_resource_group.location
  resource_group_name = azurerm_resource_group.main_resource_group.name
  sku                 = "Standard"
  capacity            = 1

  tags = {
    environment = var.environment
  }
}

resource "azurerm_eventhub" "eh_topic" {
  name                = "transactions"
  namespace_name      = azurerm_eventhub_namespace.eh_namespace.name
  resource_group_name = azurerm_resource_group.main_resource_group.name
  partition_count     = 2
  message_retention   = 1
}
