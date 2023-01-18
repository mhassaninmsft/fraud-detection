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

# Add the enriched event output name here, change name from transactins to
# ds1.public.enriched_event_data
resource "azurerm_eventhub" "eh_topic" {
  name                = "ds23.public.enriched_credit_card_transaction"
  namespace_name      = azurerm_eventhub_namespace.eh_namespace.name
  resource_group_name = azurerm_resource_group.main_resource_group.name
  partition_count     = 1
  message_retention   = 7
}
