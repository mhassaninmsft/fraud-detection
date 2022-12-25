CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS merchant(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    name text NOT NULL
);
CREATE TABLE IF NOT EXISTS pos_machine(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    zip_code VARCHAR(20) NOT NULL,
    merchant_id uuid NOT NULL,
    -- CONSTRAINTS
    CONSTRAINT fk_pos_machine_merchant FOREIGN KEY (merchant_id) REFERENCES merchant(id)

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
    pos_machine_id uuid NOT NULL,
    amount float NOT NULL,
    --CONSTRAINTS
     CONSTRAINT fk_credit_card_transactions_credit_card FOREIGN KEY (credit_card_id) REFERENCES credit_card (id),
     CONSTRAINT fk_credit_card_transactions_pos_machine FOREIGN KEY (pos_machine_id) REFERENCES pos_machine (id)
);

-- Functions and Stored Procedures
CREATE OR REPLACE FUNCTION add_two(a integer, b integer) RETURNs integer
    LANGUAGE SQL
    IMMUTABLE
    RETURNS NULL ON NULL INPUT
    RETURN (a + b);


