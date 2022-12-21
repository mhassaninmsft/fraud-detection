# fraud-detection

## database

export PGPASSWORD=magical_password
psql -h localhost -p 5432 -U bankuser -d magicBank

### entity framework

dotnet-ef dbcontext scaffold "Host=localhost;Database=magicBank;Username=bankuser;Password=magical_password" Npgsql.EntityFrameworkCore.PostgreSQL --output-dir Models --context-dir Models --force