# fraud-detection

- [fraud-detection](#fraud-detection)
  - [database](#database)
    - [entity framework](#entity-framework)
  - [Stream Analytics Query](#stream-analytics-query)
  - [Resources](#resources)
  - [Suggestions](#suggestions)
  - [Tools to explore](#tools-to-explore)
  - [Questions](#questions)
  - [issues created](#issues-created)

## database

export PGPASSWORD=magical_password
psql -h localhost -p 5432 -U bankuser -d magicBank

### entity framework

```bash
dotnet dotnet-ef dbcontext scaffold "Host=localhost;Database=magicBank;Username=bankuser;Password=magical_password" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context-dir Models --force

dotnet dotnet ef dbcontext scaffold Name=Database:ConnectionString Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context-dir Models --force
# conert propertie file to .env file
awk -F= '/=/{gsub(/\./, "_", $1); $1="" toupper($1); gsub(/\[/, "{"); gsub(/\]/, "}"); gsub(/\r/, "")} 1' OFS== env.properties

docker run -v $(pwd):/tmp --env-file ./connect2.env -it debezium/connect:latest /bin/bash

curl -X POST -H "Content-Type: application/json" --data @pg-connector-fraud.json http://localhost:8083/connectors

ALTER USER citus WITH REPLICATION; -- must be executed to allow the wal2sender role
```

table.include.list to include which databases in the debizum connector to stream to

## Stream Analytics Query

```SQL
SELECT
    Collect() AS allEvents,CreditCardId, MIN(Latitude) as lat, MIN(Longtitude) AS lng,  MAX(Latitude) as max_lat, MAX(Longtitude) AS max_lng --Longtitude,Latitude,TxTime
INTO
    [output1]
FROM
    [enriched] TIMESTAMP BY TxTime
    GROUP BY CreditCardId, SlidingWindow(second,10)
    HAVING abs(lat-max_lat) > 3 OR abs(lng-max_lng) > 3


```
## Resources

1. https://materialize.com/docs/integrations/cdc-postgres/#:~:text=Change%20Data%20Capture%20(CDC)%20allows,on%20top%20of%20CDC%20data.
2. https://news.ycombinator.com/item?id=15718226
3. https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-kafka-connect-debezium
4. https://www.youtube.com/watch?v=IuBOco7CQLg
5. https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/october/data-points-hybrid-database-migrations-with-ef-core-and-flyway
6. https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-tutorial-visualize-anomalies

## Suggestions

1. Need a way to auto create `capture-description` to backup event hub data into a storage account, right now it is per event hub not Event hub name space
2. Settint `wal_level` in terraform does not auto restart the azure flexible postgres database. It needs to be restarted manually
3. The documentation for the debizium event hub connector is not up to data.

## Tools to explore

1. Synth: https://www.getsynth.com/docs/content/object, debug postgresql uuid

## Questions

1. EF Core (Write/Read Replicas) pattern
2. Exclude tables in EF core not supported??
3. Canonical way to execute Stored Proecures in EF core. Is there a way to auto generate stored proecudes/ user functions into strongly typed C# functions/classes.
4. How to add a partition key that is nested for a stream analytics event hub input. The parititon key needed is `payload.after.credit_card_id` I get the error `The partition key can only contain alphanumeric characters, underscores, spaces and hyphens`
5. Why does assigning IAM roles and identites in terraform require the `Owner` permission of the service principal on the entire subscription. Can it perhaps be localized to a few resources or resource groups.

## issues created

1. https://github.com/MicrosoftDocs/azure-docs/issues/103340
2. https://github.com/dotnet/efcore/issues/6182
3. https://social.msdn.microsoft.com/Forums/azure/en-US/2b568bf4-8c32-40ea-a504-9d5b581c9145/creating-multiple-databases-in-postgresql-hyperscale-cluster-server-group?forum=AzureDatabaseforPostgreSQL
