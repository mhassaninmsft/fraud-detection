output "location" {
  value = var.location
}

output "rg_name" {
  value = resource.azurerm_resource_group.main_resource_group.name
}

output "storage_account_name" {
  value = resource.azurerm_storage_account.storage_main.name
}

output "environment" {
  value = var.environment
}

output "eh" {
  value     = resource.azurerm_eventhub_namespace.eh_namespace
  sensitive = true
}

