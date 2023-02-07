# Fraud Detection and Advanced analytics

## Table of Contents

- [Fraud Detection and Advanced analytics](#fraud-detection-and-advanced-analytics)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
    - [Traditional SQL](#traditional-sql)
    - [The NoSQL Movement](#the-nosql-movement)
    - [Distributed SQL](#distributed-sql)
  - [Scenario](#scenario)
  - [Architecture](#architecture)
  - [Stream Analytics Job](#stream-analytics-job)
  - [Moving to use to Distributed SQL](#moving-to-use-to-distributed-sql)
    - [Benefits of Citus Database for SQL](#benefits-of-citus-database-for-sql)
  - [Limitations of the Azure Postresql offering](#limitations-of-the-azure-postresql-offering)
  - [Next steps](#next-steps)
  - [Summary](#summary)

## Introduction

In this post we explore one of the newest offerings into the Azure database ecosystem, namely Azure Cosmos Db for Postgresql. I know that sounds a little surprising because up until that offering, Cosmos DB has been traditionally a NoSQL database, but with that offering Cosmos now offers a truly relational SQL database. So what is new in that offering and why is it different than Microsoft other Postgresql database (Azure Flexible Server). Th3e difference is that Azure Cosmos db for Postgresql offers a Distributed SQL paradigm on top of traditional PostgreSQL. so what is Distributed SQL and why is it a big deal.

### Traditional SQL

Traditional Single node SQL has been around for a very along time, its ubiquitousness, plethora of tool support, developer familiarity with it makes it a very suitable tool for most applications. The actual strength of SQL is the relational model and different constraints that can be enforced at the database level. SQL databases can help prevent data drift, missing references, also support for ACID transactions ensure that such constraints are met all the time.

### The NoSQL Movement

In the early 2010s, traditional SQL databases failed to meet the demands of the growing internet companies since at that time most Offerings of SQL ran on a single machine for handling writes, replication was only done for backup and serving reads (read replicas). However the system was bottle necked to a single write node. Solutions existed for multi writer nodes such as manually sharding the data across different servers, but such solutions were extremely difficult and required massive engineering feats to get right especially in production and when facing huge workloads. Moreover a typical sharded system would sacrifice distributed transactions. When losing distributed transactions, the system loses one of its main strengths for data consistency at scale. And hence the NoSQL movement emerged which offered easy horizontal scalability with limited support for transactions initially (some flavors Distributed transactions were later supported in MongodDb and other NoSQL providers). Most of the data consistency checks were delegate to the application layer instead of the database. This was a perfectly acceptable solution but at the cost of more application complexity and mental manual effort to keep the application state consistent.

### Distributed SQL

As discussed earlier NoSQL databases had to give up many of the data constraints enforced by SQL and had to relegate those checks to the application layer. Distributed database SQL include Azure Cosmos db for Postgresql (formerly citus database for SQL), Google Spanner, Cockroach db and Yugabyte. One can think of the distributed SQL movement as the marriage of (traditional SQL databases with their ACID guartntees and Full SQL support) and NoSQL Databases with their horizontal scalability and easy sharding/partitioning. It

This post explains how to use real time streaming analytics in Azure Cosmos Db for Postgresql (Citus database).

## Scenario

In this example we are assuming that we are PayCo, a fictional credit card issuer that is similar to Visa or MasterCard. PayCo is responsible for issuing Credit cards for users on behalf of different banks and is charged with settling transactions.
In this paper, we will be focusing on near real time fraud detection of credit card transactions. The purpose of this exercise is not to immediately stop a transaction from occuring, but very soon after that to identify is such transaction is valid or not. So after the transaction occurs in near real time, the user is notified via SMS or email of the transaction and is asked to confirm whether the transaction is legitimate or not.

## Architecture

We will start off with a single Postgresql database to store all the transactions, the schema is found [here](../src/database/sql/migrations/1/V1__Init_database.sql)

We are using Debizium Connector to stream the events of the `credit_card_transactions` table into Azure event hubs. We are using Apache Debizium connector for Apache Kafka. Azure event hubs expose a Kafka Compatible endpoint. Debizium can capture all changes to the database It utilizes a PSQL feature called logical replication. The main idea is that PSQL internally uses a Write Ahead Log (WAL) to record every transaction and operation happening in the database both DDL  (such as CREATE TABLE, ALTER Table, ADD Constraint) and DML such as (INSERT INTO, UPDATE). We capture such events from the database and save them to a eventhub.

Ideally we should not have to create another table `enriched_credit_card_transaction` which contains the data in a denormalized fashion and instead we can rely on a SQL view to get the data enriched in that fashion, however the debizium connector unfortuanelty does not operate on views and only operates on tables. Moreover PSQL does not support `live` views where the data is auto populated into the view, and instead a query has to run every time data is updated. Hence we create the `enriched_credit_card_transaction` table and have a database trigger on new inserts on `credit_card_transaction` to insert into the `enriched_credit_card_transaction` and then use debizium to only stream the changes to `enriched_credit_card_transaction` to an event hub stream.
The architecure diagram is shown in the Figure below

![Architecture Diagram](../images/Arch%20Diagram%20Fraud.png?raw=true "Architecture Diagram")

## Stream Analytics Job

The stream analytics job listens on the stream `enriched_credit_card_transaction` and performs fraud detection. The main idea is that the streaming function looks at a 10 minute sliding window of all transactions partitioned by each credit card. If 2 or more transactions occur in that 10 minute window is in *different zip_codes*. If that is the case, it writes the offending transaction to a service bus queue, where each item in the queue triggers an email to interested parties. The code snippet below performs the actual anomaly detection. It reads all transaction events partitioned by each credit_card and performs a sliding window of 10 minutes, during that window all the transactions are grouped together and the zip_codes of the transactions are recorded. If there are more than one distinct zip_code, this signals that a fraudulent event has occurred and an event fires into the next stage

```SQL
 WITH
EnchancedData AS
(
  SELECT fullevents.payload.after.purchase_zip_code  AS zip_code ,
  fullevents.payload.after.credit_card_id
  FROM fullevents fullevents
)
SELECT EnchancedData.credit_card_id , COUNT(*), Collect(EnchancedData.zip_code), COUNT( DISTINCT EnchancedData.zip_code) AS zip_codes
INTO queue1
FROM EnchancedData EnchancedData
GROUP BY EnchancedData.credit_card_id, SlidingWindow(Duration(minute, 10))
HAVING zip_codes>1;
```

## Moving to use to Distributed SQL

So far PayCo has done great with its Single node SQL instance but they want to expand their offering to multiple banks in different locations in the US and Europe. They still want their processing times to fast and always online. For that they use Azure Cosmos DB for PostgreSQL. They add more nodes to their Citus Cluster and they partition their data by running a few SQL commands below

```SQL
SELECT create_reference_table('merchant');
SELECT create_reference_table('pos_machine');

SELECT create_distributed_table('bank','id');
SELECT create_distributed_table('credit_card', 'id');
SELECT create_distributed_table('credit_card_transaction', 'credit_card_id');
```

https://github.com/mhassaninmsft/fraud-detection/blob/final-readme/src/database/sql/migrations/1/V1__Init_database.sql

`reference tables` are tables that are replicated to all worker nodes in the cluster. They are typically small in size and are used often in Joins. `distributed tables` on the other hand tend to be fairly large and benefit from sharding. In our use case, credit cards and the credit card transactions are sharded by credit_card_id, which means all credit_cards and all transactions belonging to that card will belong to the same physical Postgresql server.

Although Cosmos does not support this yet, a benefit of sharding the data based on the bank is that it may be possible in the future to specify which physical server each bank information goes to. This will be very beneficial in scenarios where data residency and sovereignty guarantees such as those imposed by GDPR much easier to manage (Note Cockroach db supports this already).

Once the workload scales to multiple banks and many credit card transactions, it possible to add new worker nodes to distribute the data to. You can add node easily in the Azure Portal or via the CLI. Once you add the nodes, the data does not rebalance auyrom,atically, and you need to manually auto rebalance the data either in the Azure portal or via commands executed at the master node `SELECT rebalance_table_shards();`.

### Benefits of Citus Database for SQL

1. Simple extension over Postgresql which can be a drop in replacement for most SQL applications.
2. Distributed database can handle more traffic and may forestall the need to use and manage a tradiional NoSql database such as NoSQL Cosmos, mongoDb, or others.
3. In a traditional application

## Limitations of the Azure Postresql offering

1. The current offering in Azure has a limitation that the created database user does not have admin privileges. Admin privileges are required for Streaming Data to Event hub (It requires the postgres user to have `REPLICATION` rights). As a workaround. I am running a Citus cluster in Azure Kubernetes Server and not using the managed Cosmos db offering.
2. Currently you need to start a debizium connector per worker node. I am currently doing that via a sidecar container in each worker pod. The sidecar on startup starts a debizium connector and sends the worker logs to Eventhub.

## Next steps

1. I am working on a Postgresql extension to automate creating the debizium connectors when new worker nodes join the cluster instead of manually starting the connector for each worker node

## Summary

There are many innovations happening in the database world. Distributed SQL is a key feature and one that will be the standard in the next decade or so. It will be a lot easier to move data between different storage technologies via out of the box streaming connections and even Zero ETL approaches, where the platform can automatically stream the data between different database technologies. There will be even less need to move data between different database systems  as we have demonstrated here with the Cosmos db Postgresql engine where real time analytics can be performed on the cluster with distributed data. Other innovatins in the data space include separating the storage and compute layer of the databases, decomposing the compute layer into smaller subqueries and dispatching those to the storage layer and the ability to scale the query layer and storage layer separaety( Google ).

The code used in this repo can be found [here](https://github.com/mhassaninmsft/fraud-detection)
