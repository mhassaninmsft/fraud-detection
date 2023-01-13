CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE bank(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    name text NOT NULL,
    address text NOT NULL
);

CREATE TABLE IF NOT EXISTS merchant(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    name text NOT NULL
);
-- CREDIT CARD CLIENT
CREATE TABLE IF NOT EXISTS client(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    name text NOT NULL,
    dob DATE NOT NULL,
    ssn integer NOT NULL UNIQUE,
    address text
    -- CONSTRAINTS
);
-- Allows easy access by ssn
CREATE UNIQUE INDEX client_ssn ON client(ssn);
-- Point of Sale Machine
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
    bank_id uuid NOT NULL,
    client_id uuid NOT NULL,
    expiration_month int NOT NULL,
    expiration_year int NOT NULL,
    --CONSTRAINTS
    CONSTRAINT fk_credit_card_bank FOREIGN KEY (bank_id) REFERENCES bank(id),
    CONSTRAINT fk_credit_card_client FOREIGN KEY (client_id) REFERENCES client(id)
);


CREATE TABLE IF NOT EXISTS credit_card_transaction(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    credit_card_id uuid NOT NULL,
    pos_machine_id uuid NOT NULL,
    amount float NOT NULL,
    created_at timestamp NOT NULL DEFAULT now(),
    --CONSTRAINTS
     CONSTRAINT pk_credit_card_transaction PRIMARY KEY (id,credit_card_id),
     CONSTRAINT fk_credit_card_transactions_credit_card FOREIGN KEY (credit_card_id) REFERENCES credit_card (id),
     CONSTRAINT fk_credit_card_transactions_pos_machine FOREIGN KEY (pos_machine_id) REFERENCES pos_machine (id)
);
CREATE INDEX credit_card_transaction_id_index ON credit_card_transaction(id);
CREATE INDEX credit_card_transaction_cc_index ON credit_card_transaction(credit_card_id);

-- Enriched Functions
CREATE TABLE enriched_credit_card_transaction AS
SELECT tx.*, pm.zip_code AS purchase_zip_code, cc.client_id, bank.name AS bank_name, merchant.name AS merchant_name, client.name AS clinet_name
FROM credit_card_transaction tx
INNER JOIN pos_machine pm
ON tx.pos_machine_id = pm.id
INNER JOIN credit_card cc
ON tx.credit_card_id = cc.id
INNER JOIN bank
ON bank.id = cc.bank_id
INNER JOIN merchant
ON merchant.id = pm.merchant_id
INNER JOIN client ON cc.client_id = client.id
;
-- Triggers
CREATE OR REPLACE FUNCTION insert_into_enriched()
RETURNS trigger LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO enriched_credit_card_transaction SELECT tx.*, pm.zip_code AS purchase_zip_code, cc.client_id, bank.name AS bank_name, merchant.name AS merchant_name, client.name AS clinet_name
    FROM credit_card_transaction tx
    INNER JOIN pos_machine pm
    ON tx.pos_machine_id = pm.id
    INNER JOIN credit_card cc
    ON tx.credit_card_id = cc.id
    INNER JOIN bank
    ON bank.id = cc.bank_id
    INNER JOIN merchant
    ON merchant.id = pm.merchant_id
    INNER JOIN client ON cc.client_id = client.id
    WHERE NEW.id = tx.id;
    RETURN NULL;
END $$;

CREATE TRIGGER new_credit_card_transaction
AFTER INSERT
ON credit_card_transaction for each ROW
EXECUTE FUNCTION insert_into_enriched();


-- Functions and Stored Procedures
CREATE OR REPLACE FUNCTION add_two(a integer, b integer) RETURNs integer
    LANGUAGE SQL
    IMMUTABLE
    RETURNS NULL ON NULL INPUT
    RETURN (a + b);

CREATE OR REPLACE FUNCTION concat_mk(a text, b text) RETURNs text
    LANGUAGE SQL
    IMMUTABLE
    RETURNS NULL ON NULL INPUT
    RETURN (a || b);


CREATE OR REPLACE FUNCTION search_by_year(a integer) RETURNs SETOF credit_card
     AS $$
    SELECT * FROM credit_card WHERE expiration_year=a;
$$ LANGUAGE SQL;
