
-- INSERT INTO merchant(zip_code) VALUES('01519');
-- INSERT INTO pos_machine(zip_code) VALUES('01519');

-- CREATE TABLE IF NOT EXISTS credit_card
-- (
--     id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
--     card_number VARCHAR(16) NOT NULL,
--     issuing_bank TEXT NOT NULL,
--     client_name TEXT NOT NULL,
--     expiration_month int NOT NULL,
--     expiration_year int NOT NULL
-- );
DO $$
DECLARE bank_id           bank.id%TYPE;
DECLARE client_id         client.id%TYPE;
DECLARE merchant_id       merchant.id%TYPE;
DECLARE credit_card1_id   credit_card.id%TYPE;
DECLARE credit_card2_id   credit_card.id%TYPE;
DECLARE pos_machine_id    pos_machine.id%TYPE;
-- DECLARE v_studentid bigint;
BEGIN
INSERT INTO bank(name,address) VALUES('Bank of Commerce','15 Evergreen terrace, WA 01519') RETURNING id INTO bank_id;
INSERT INTO client(name,dob,ssn) VALUES('Jason Bourne','07-01-1998','555245555') RETURNING id INTO client_id;
  INSERt INTO merchant(name)
VALUES('Brewed Awakenings Coffee') RETURNING id INTO merchant_id;
INSERT INTO pos_machine(zip_code,merchant_id)
VALUES('01519',merchant_id) RETURNING id INTO pos_machine_id;
--CREATE CREDIT CARDS
INSERT INTO credit_card(card_number,bank_id,client_id,expiration_month,expiration_year)
VALUES('1234542323231110',bank_id,client_id,12,23) RETURNING id INTO credit_card1_id;
INSERT INTO credit_card(card_number,bank_id,client_id,expiration_month,expiration_year)
VALUES('3234542323231112',bank_id,client_id,12,25)  RETURNING id INTO credit_card2_id;
--CREDIT CARD TRansactions
INSERT INTO credit_card_transaction(credit_card_id,pos_machine_id,amount)
VALUES(credit_card1_id,pos_machine_id,35);
END $$;

-- DECLARE merchant_id merchant.id%TYPE;

-- INSERt INTO merchant(name)
-- VALUES('Brewed Awakenings Coffee') RETURNING id INTO merchant_id;

-- INSERT INTO credit_card(card_number,issuing_bank,client_name,expiration_month,expiration_year)
-- VALUES('1234542323231110','Bank of Commerce','Jason Bourne',12,23);

-- INSERT INTO credit_card(card_number,issuing_bank,client_name,expiration_month,expiration_year)
-- VALUES('3234542323231112','Bank of America','Jason Bourne',12,25);