# fraud-detection

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

## Resources

1. https://materialize.com/docs/integrations/cdc-postgres/#:~:text=Change%20Data%20Capture%20(CDC)%20allows,on%20top%20of%20CDC%20data.
2. https://news.ycombinator.com/item?id=15718226
3. https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-kafka-connect-debezium
4. https://www.youtube.com/watch?v=IuBOco7CQLg
5. https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/october/data-points-hybrid-database-migrations-with-ef-core-and-flyway

## Suggestions

1. Need a way to auto create `capture-description` to backup event hub data into a storage account, right now it is per event hub not Event hub name space
2. Settint `wal_level` in terraform does not auto restart the azure flexible postgres database. It needs to be restarted manually
3. The documentation for the debizium event hub connector is not up to data.