resource "azurerm_container_group" "debizium" {
  name                = "debizium-connector-group"
  location            = azurerm_resource_group.main_resource_group.location
  resource_group_name = azurerm_resource_group.main_resource_group.name
  ip_address_type     = "Public"
  dns_name_label      = "debizium${var.environment}"
  os_type             = "Linux"
  exposed_port {
    port     = 8083
    protocol = "TCP"
  }
  exposed_port {
    port     = 5005
    protocol = "TCP"
  }

  container {
    name   = "debizium-container"
    image  = "debezium/connect:latest"
    cpu    = "2.0"
    memory = "4.0"

    ports {
      port     = 8083
      protocol = "TCP"
    }
    ports {
      port     = 5005
      protocol = "TCP"
    }
    volume {
      name       = "javaempty"
      mount_path = "/etc/crypto-policies/back-ends"
      empty_dir  = true
      #   secret = {
      #     "java.config" : base64encode(" ")
      #   }
    }
    environment_variables = {
      "env"             = "test"
      BOOTSTRAP_SERVERS = "hassmk1eastusehns.servicebus.windows.net:9093" # e.g. namespace.servicebus.windows.net:9093
      GROUP_ID          = "connect-cluster-group3"
      # connect internal topic names, auto-created if not exists
      CONFIG_STORAGE_TOPIC = "connect-cluster-configs3"
      OFFSET_STORAGE_TOPIC = "connect-cluster-offsets3"
      STATUS_STORAGE_TOPIC = "connect-cluster-status3"
      # internal topic replication factors - auto 3x replication in Azure Storage
      CONFIG_STORAGE_REPLICATION_FACTOR = "1"
      OFFSET_STORAGE_REPLICATION_FACTOR = "1"
      STATUS_STORAGE_REPLICATION_FACTOR = "1"

      REST_ADVERTISED_HOST_NAME = "connect"
      OFFSET_FLUSH_INTERVAL_MS  = "10000"

      KEY_CONVERTER            = "org.apache.kafka.connect.json.JsonConverter"
      VALUE_CONVERTER          = "org.apache.kafka.connect.json.JsonConverter"
      INTERNAL_KEY_CONVERTER   = "org.apache.kafka.connect.json.JsonConverter"
      INTERNAL_VALUE_CONVERTER = "org.apache.kafka.connect.json.JsonConverter"

      INTERNAL_KEY_CONVERTER_SCHEMAS_ENABLE   = false
      INTERNAL_VALUE_CONVERTER_SCHEMAS_ENABLE = false

      # required EH Kafka security settings
      CONNECT_SECURITY_PROTOCOL = "SASL_SSL"
      CONNECT_SASL_MECHANISM    = "PLAIN"
      CONNECT_SASL_JAAS_CONFIG  = var.eh_connection_string

      CONNECT_PRODUCER_SECURITY_PROTOCOL = "SASL_SSL"
      CONNECT_PRODUCER_SASL_MECHANISM    = "PLAIN"
      CONNECT_PRODUCER_SASL_JAAS_CONFIG  = var.eh_connection_string

      CONNECT_CONSUMER_SECURITY_PROTOCOL = "SASL_SSL"
      CONNECT_CONSUMER_SASL_MECHANISM    = "PLAIN"
      CONNECT_CONSUMER_SASL_JAAS_CONFIG  = var.eh_connection_string
      # PLUGIN_PATH=/kafka/libs # path to the libs directory within the Kafka release

    }
  }

  #   container {
  #     name   = "sidecar"
  #     image  = "mcr.microsoft.com/azuredocs/aci-tutorial-sidecar"
  #     cpu    = "0.5"
  #     memory = "1.5"
  #   }

  tags = {
    environment = var.environment
  }
}
