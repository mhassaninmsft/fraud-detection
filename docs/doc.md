# Fraud Detection and Advanced analytics

This paper explains how to use real time streaming analytics in Azure Cosmos Db for Postgresql (Citus database).

## Table of Contents

- [Fraud Detection and Advanced analytics](#fraud-detection-and-advanced-analytics)
  - [Table of Contents](#table-of-contents)
  - [Scenario](#scenario)
    - [Technical Considerations](#technical-considerations)
    - [Why use a Relational database](#why-use-a-relational-database)
      - [Ubiquitousness of SQL](#ubiquitousness-of-sql)
      - [Why NoSQL](#why-nosql)
      - [The Distributed Sql Movment](#the-distributed-sql-movment)
  - [Architecture](#architecture)
  - [Stream Analytics Job](#stream-analytics-job)
  - [Move to Distributed SQL](#move-to-distributed-sql)
    - [Table partitioning](#table-partitioning)
    - [Benefits of Citus Database for SQL](#benefits-of-citus-database-for-sql)

## Scenario

In this example we are assuming that we are PayCo, a fictional credit card issuer that is similar to Visa or MasterCard. PayCo is responsible for issuing Credit cards for users on behalf of different banks and is charged with settling transactions.
In this paper, we will be focusing on near real time fraud detection of credit card transactions. The purpose of this exercise is not to immediately stop a transaction from occuring, but very soon after that to identify is such transaction is valid or not. This is the approach that most cards take for fraud detection. So after the transaction occurs in near real time (10 seconds or so), the user is notified via SMS or email of the transaction and is asked to confirm whether the transaction is legitimate or not.

### Technical Considerations

The desired system should be designed to handle tens of thousands of transactions per second and potentially span multiple regions and countries.

### Why use a Relational database

#### Ubiquitousness of SQL

SQL has been around for a very along time, its ubiquitousness, plethora of tool support, developer familiarity with it makes it a very suitable tool for most applications. The actual strngth of SQL is the relational model and different constraints that can be enforced at the database level. SQL databases can help prevent data drift, missing references, also support for ACID transactions ensure that such constraints are met all the time.

#### Why NoSQL

In the early 2010s, traditional SQL databases failed to meet the demands of the growing internet companies since at that time most Offerings of SQL ran on a single machine for handling writes, replication was only done for backup and serving reads (read replicas). However the system was bottle necked to a single write node. Solutions existed for multi writer nodes such as manually sharding the data across different servers, but such solutions were extremely difficult and required massive engineering feats to get right especially in production and when facing huge workloads. Moreover a typical sharded system would sacrifice distributed transactions. When losing distributed transactions, the system loses one of its main strengths for data consistency at scale. And hence the NoSQL movement emerged which offered easy horizontal scalability with limited support for transactions initially (some flavors Distributed transactions were later supported in MongodDb and other NoSQL providers). Most of the data consistency checks were delegate to the application layer instead of the database. This was a perfectly acceptable solution but at the cost of more application complexity and mental

#### The Distributed Sql Movment

As explained earlier NoSQL databases had to give up many of the data constraints enforecd by SQL and had to relrgate thos cehcks to the application layer
Distributed database SQL include citus databse for SQL, Google Spanner, Cockroach db and Yugabyte. One can think of the distributed SQL movement as the marriage of (traditional SQL databases with their ACID guartntees and Full SQL support) and NoSQL Databases with their horizontal scalability and easy sharding/partitioning

## Architecture

We will start off with a single Postgresql database to store all the transactions, the schema is found [here](../src/database/sql/migrations/1/V1__Init_database.sql)

We are using Debizium Connector to stream the events of the `credit_card_transactions` table into Azure event hubs. We are using Apache Debizium connector for Apache Kafka. Azure event hubs expose a Kafka Compatible endpoint. Debizium can capture all changes to the database It utilizes a PSQL feature called logical replication. The main idea is that PSQL internally uses a Write Ahead Log (WAL) to record every transaction and operation happening in the database both DDL  (such as CREATE TABLE, ALTER Table, ADD Constraint) and DML such as (INSERT INTO, UPDATE). We capture such events from the database and save them to a eventhub.

Ideally we should not have to create another table `enriched_credit_card_transaction` which contains the data in a denormalized fashion and instead we can rely on a SQL view to get the data enriched in that fashion, however the debizium connector unfortuanelty does not operate on views and only operates on tables. Moreover PSQL does not support `live` views where the data is auto populated into the view, and instead a query has to run every time data is updated. Hence we crate the `enriched_credit_card_transaction` table and have a database trigger on new inserts on `credit_card_transaction` to insert into the `enriched_credit_card_transaction` and then use debizium to only stream the changes to `enriched_credit_card_transaction` to an event hub stream.

## Stream Analytics Job

The stream analytics job listens on the stream `enriched_credit_card_transaction` and performs fraud detection. The main idea is that the streaming functiopn looks at a 10 minute sliding window of all transactions partitoned by each credit card. If 2 or more transactions occur in that 10 minute window is in *different zip_codes*. If that is the case, it writes the offending trasnaction to a service bus queue, where each item in the queue triggers an email to interested parties.

## Move to Distributed SQL

So far PayCo has done great with its Single node SQL instance but they want to expand their offering to multiple banks in different locations in the US and Europe. They still want their processing times to fast and always online. For that they use Azure Cosmos DB for PostgreSQL. They add more nodes to their Citus Cluster and they partition their data by running a few SQL commands below

```SQL
SELECT create_reference_table('merchant');
SELECT create_reference_table('pos_machine');
SELECT create_reference_table('bank');

SELECT create_distributed_table('credit_card', 'id');
SELECT create_distributed_table('credit_card_transaction', 'credit_card_id');
```

### Table partitioning

PayCo can be a considered a Multi-Tenant SaaS provider and hence it will benifit immensly from  services many banks We are using the as `bank_name` as a partition key for the datatabse, which means that `citus` will distribute the row of the relationship via the `bank_id` attribute. We use the `create_distributed_table()` and use the issung bank id as the basis for the operation. We also can create supporting tables in the same logical partition as the `bank` table using the `co_locate` comand. As for the bank information table, we can use the `reference_table` command to place certain tables such as the `bank_information` and `pos_machine` at every replica.

Although Citus does not support this yet, a beneifit of sharding the data based on the bank is that it may be possible in the future to specify which physical server each bank information goes to. This will be very beneifical in scenarios where data residency and soverigintiy guarntees such as those imposed by GDPR much easier to manage (Note Cockroach db supports this already)

### Benefits of Citus Database for SQL

1. Simple extension over Postgresql which can be a drop in replacement for most SQL applications.
2. Distributed database can handle more traffic and may forestall the need to use and manage a tradiional NoSql database such as NoSQL Cosmos, mongoDb, or others.
3. In a traditional application
