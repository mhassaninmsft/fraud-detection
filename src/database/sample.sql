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

INSERT INTO credit_card(card_number,issuing_bank,client_name,expiration_month,expiration_year)
VALUES('1234542323231110','Bank of Commerce','Jason Bourne',12,23);

INSERT INTO credit_card(card_number,issuing_bank,client_name,expiration_month,expiration_year)
VALUES('3234542323231112','Bank of America','Jason Bourne',12,25);