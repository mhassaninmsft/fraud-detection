resource "azurerm_postgresql_flexible_server" "fraud_server" {
  name                   = "frauddetection${var.environment}"
  resource_group_name    = azurerm_resource_group.main_resource_group.name
  location               = azurerm_resource_group.main_resource_group.location
  version                = "14"
  administrator_login    = var.pg_username
  administrator_password = var.pg_password
  storage_mb             = 32768
  sku_name               = "B_Standard_B1ms"
  zone                   = "2"
}

resource "azurerm_postgresql_flexible_server_database" "fraud_database" {
  name      = "frauddb"
  server_id = azurerm_postgresql_flexible_server.fraud_server.id
  collation = "en_US.utf8"
  charset   = "utf8"
}
resource "azurerm_postgresql_flexible_server_configuration" "logical_wal" {
  name      = "wal_level"
  server_id = azurerm_postgresql_flexible_server.fraud_server.id
  value     = "LOGICAL"
}
