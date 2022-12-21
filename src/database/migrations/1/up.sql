CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS pos_machine(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    zip_code VARCHAR(20) NOT NULL
);

CREATE TABLE IF NOT EXISTS credit_card
(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    card_number VARCHAR(16) NOT NULL,
    issuing_bank TEXT NOT NULL,
    client_name TEXT NOT NULL,
    expiration_month int NOT NULL,
    expiration_year int NOT NULL
);

CREATE TABLE IF NOT EXISTS credit_card_transaction(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    credit_card_id uuid NOT NULL,
    --CONSTRAINTS
     CONSTRAINT fk_credit_card_transactions_credit_card FOREIGN KEY (credit_card_id) REFERENCES credit_card (id)
);