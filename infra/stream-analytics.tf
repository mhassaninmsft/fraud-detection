resource "azurerm_stream_analytics_job" "fraud_detection_stream" {
  name                                     = "fraud-detectin-stream"
  resource_group_name                      = azurerm_resource_group.main_resource_group.name
  location                                 = azurerm_resource_group.main_resource_group.location
  compatibility_level                      = "1.2"
  data_locale                              = "en-US"
  events_late_arrival_max_delay_in_seconds = 60
  events_out_of_order_max_delay_in_seconds = 50
  events_out_of_order_policy               = "Adjust"
  output_error_policy                      = "Drop"
  streaming_units                          = var.streaming_units

  tags = {
    environment = var.environment
  }

  transformation_query = <<QUERY
   WITH
EnchancedData AS
(
  SELECT outev.payload.after.purchase_zip_code  AS zip_code ,
  outev.payload.after.credit_card_id
  FROM fullevents outev
)
SELECT ed1.credit_card_id , COUNT(*), Collect(ed1.zip_code), COUNT( DISTINCT ed1.zip_code) AS zip_codes
INTO queue1
FROM EnchancedData ed1
GROUP BY ed1.credit_card_id, SlidingWindow(Duration(minute, 1))
HAVING zip_codes>1;
QUERY
}

## Input
resource "azurerm_eventhub_consumer_group" "frauddb_consumer" {
  name                = "frauddb_tx_consumer"
  namespace_name      = azurerm_eventhub_namespace.eh_namespace.name
  eventhub_name       = azurerm_eventhub.eh_topic.name
  resource_group_name = azurerm_resource_group.main_resource_group.name
}

resource "azurerm_stream_analytics_stream_input_eventhub_v2" "example" {
  name                         = "fullevents"
  stream_analytics_job_id      = azurerm_stream_analytics_job.fraud_detection_stream.id #data.azurerm_stream_analytics_job.example.id
  eventhub_consumer_group_name = azurerm_eventhub_consumer_group.frauddb_consumer.name
  eventhub_name                = azurerm_eventhub.eh_topic.name
  servicebus_namespace         = azurerm_eventhub_namespace.eh_namespace.name
  shared_access_policy_key     = azurerm_eventhub_namespace.eh_namespace.default_primary_key
  shared_access_policy_name    = "RootManageSharedAccessKey"

  serialization {
    type     = "Json"
    encoding = "UTF8"
  }
}

## Output
resource "azurerm_stream_analytics_output_servicebus_queue" "output1" {
  name                      = "queue1"
  stream_analytics_job_name = azurerm_stream_analytics_job.fraud_detection_stream.name
  resource_group_name       = azurerm_resource_group.main_resource_group.name
  queue_name                = azurerm_servicebus_queue.suspicious_tx.name
  servicebus_namespace      = azurerm_servicebus_namespace.fraud_tx_ns.name
  shared_access_policy_key  = azurerm_servicebus_namespace.fraud_tx_ns.default_primary_key
  shared_access_policy_name = "RootManageSharedAccessKey"

  serialization {
    type     = "Json"
    encoding = "UTF8"
    format   = "Array"
  }
}
