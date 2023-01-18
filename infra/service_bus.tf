resource "azurerm_servicebus_namespace" "fraud_tx_ns" {
  name                = "fraudelenttx${var.environment}namespace"
  location            = azurerm_resource_group.main_resource_group.location
  resource_group_name = azurerm_resource_group.main_resource_group.name
  sku                 = "Standard"

  tags = {
    source = "terraform"
  }
}

resource "azurerm_servicebus_queue" "suspicious_tx" {
  name         = "suspicioustx"
  namespace_id = azurerm_servicebus_namespace.fraud_tx_ns.id

  enable_partitioning = false
}
