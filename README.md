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
```

## Resources

1. https://materialize.com/docs/integrations/cdc-postgres/#:~:text=Change%20Data%20Capture%20(CDC)%20allows,on%20top%20of%20CDC%20data.
2. https://news.ycombinator.com/item?id=15718226
3. https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-kafka-connect-debezium
4. https://www.youtube.com/watch?v=IuBOco7CQLg
5. https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/october/data-points-hybrid-database-migrations-with-ef-core-and-flyway