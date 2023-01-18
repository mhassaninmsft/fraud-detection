#!/bin/bash
# az eventhubs eventhub list --namespace-name hassmk1eastusehns -g hass-mk1-eastus-rg

nameSpaceName="hassmk1eastusehns"
rg_name="hass-mk1-eastus-rg"
event_hubs=$( az eventhubs eventhub list --namespace-name ${nameSpaceName} -g ${rg_name} --query "[].{Name:name}" -o tsv)

# echo $event_hubs
for event_hub in $event_hubs
do
    echo "deleting $event_hub ..."
    az eventhubs eventhub delete --name ${event_hub} --namespace-name ${nameSpaceName} -g ${rg_name}
    echo "${event_hub} deleted"
done