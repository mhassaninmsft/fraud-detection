# Fraud Detection and Advanced analytics

This paper explains how to use real time streaming analytics in Azure Cosmos Db for Postgresql (Citus database).

## Table of Contents

- [Fraud Detection and Advanced analytics](#fraud-detection-and-advanced-analytics)
  - [Table of Contents](#table-of-contents)
  - [Scenario](#scenario)
    - [Why use a Relatinal database](#why-use-a-relatinal-database)
      - [Ubiquteness of SQL](#ubiquteness-of-sql)
      - [Why NoSQL](#why-nosql)
      - [The Distributed Sql Movment](#the-distributed-sql-movment)
      - [Benfitis of Citus Database for SQL](#benfitis-of-citus-database-for-sql)
  - [Architecture](#architecture)
    - [Table partitioning](#table-partitioning)
    - [Streaming vs Batch Processing](#streaming-vs-batch-processing)
      - [System used](#system-used)
  - [Azure Enriching Function](#azure-enriching-function)
    - [Alternatives to the Azure Enriching Function](#alternatives-to-the-azure-enriching-function)
  - [Stream Analytics Job](#stream-analytics-job)

## Scenario

In this example we are assuming that we are PayCo, a fictional credit card issuer that is similiar ro Visa or MasterCard. PayCo is responsible for issuing Credit cards for users on behalf of different banks and is charged with settling trasnactions.
In this paper, we will be focusing on near real time fraud detection of credit card transactions. The purpose of this exercise is not to immediately stop a transaction from occuring, but very soon after that to identify is such transaction is valid or not. This is the approach that most cards take for fraud detection. So after the transaction occurs in near real time (10 seconds or so), the user is notified via SMS or email of the transaction and is asked to confirm whether the trasnaction is legitimate or not.

### Why use a Relatinal database

#### Ubiquteness of SQL

SQL has been around for a very along time, its ubiquotenes, plethora of tool support, developer familiarity with it makes it a very suitable tool for most applications. The actual strngth of SQL is the relational model and different constraints that can be enforced at the database level. SQL databases can help prevent data drift, missing references, and so many problems.

#### Why NoSQL

In the early 2010s, SQL failed to meet the demands of the growing int

#### The Distributed Sql Movment

As explained earlier NoSQL databases had to give up many of the data constraints enforecd by SQL and had to relrgate thos cehcks to the application layer
Distributed database SQL include citus databse for SQL, Google Spanner, Cockroach db and Yugabyte. Each

#### Benfitis of Citus Database for SQL

1. Simple extension over Postgresql which can be a drop in replacement for most SQL applications.
2. Distributed database can handle more traffic and may forestall the need to use and manage a tradiional NoSql database such as NoSQL Cosmos, mongoDb, or others.
3. In a traditional application


## Architecture

We are using a Postgresql database to store all the transactions, the schema is dispalyed below

```sql
CREATE TABLE happy(id uuid, name text);
```

### Table partitioning

PayCo can be a considered a Multi-Tenant SaaS provider and hence it will benifit immensly from  services many banks We are using the as `bank_name` as a partition key for the datatabse, which means that `citus` will distribute the row of the relationship via the `bank_id` attribute. We use the `create_distributed_table()` and use the issung bank id as the basis for the operation. We also can create supporting tables in the same logical partition as the `bank` table using the `co_locate` comand. As for the bank information table, we can use the `reference_table` command to place certain tables such as the `bank_information` and `pos_machine` at every replica.

Although Citys does not support this yet, a beneifit of sharding the data based on the bank is that it may be possible in the future to specify which physical server each bank information goes to. This will be very beneifical in scenarios where data residency and soverigintiy guarntees such as those imposed by GDPR much easier to manage (Note Cockroach db supports this already)

### Streaming vs Batch Processing

Explain benifts of streaming vs batch processing

#### System used

We are using Debizium Connector to stream the events of the `credit_card_transactions` table into Azure event hubs. We are using Apache Debizium connector for Apache Kafka. Azure event hubs expose a Kafka Compatible endpoint. Debizium can capture all changes to the database It utilizes a PSQL feature called logical replication. The main idea is that PSQL internally uses a Write Ahead Log (WAL) to record every transaction and operation happening in the database both DDL  (such as CREATE TABLE, ALTER Table, ADD Constraint) and DML such as (INSERT INTO, UPDATE). We capture such events from the database and save them to a eventhub.

## Azure Enriching Function

After the events reach the eventhubs, An Azure function listens on the event hub and is triggered every time a new `credit_card_transaction` is created. The purpose of the Azure function is to create an enriched event that contains all the details of the `credit_card_trasnsaction` event. The need for such event processing function arises due to the fact that our SQL models are normalized and the `credit_card_transaction` table only contains the ids of the credit_card used as well as the pos_machine. But it does not have any direct information on the location of the sale nor information about the merchant. To get that information we have to run the query

```SQL
SELECT *
FROM credit_card_transaction tx
JOIN pos_machine pm ON tx.pos_machine_id = pm.id
JOIN credit_card cc ON  tx.credit_card_id = cc.id
WHERE tx.id=='the transaction id that occurred'
```

### Alternatives to the Azure Enriching Function

1. Using SQL reference data tables is only supported for Microsft SQL server
2. Having the Azure enriching function, we have control over calls to the underlying database and we can cache responses using the SDK and other libraries easily. For example the reference data contained in the `merchant` and the `credit_card` table can be easily cached client side with a valid TTL. Moreover, using the provided database SDK, we can batch requests to the SQL database, allevaiting the load on the database. These considerations can be tricky to implment using Stream analytics alone.

## Stream Analytics Job

The