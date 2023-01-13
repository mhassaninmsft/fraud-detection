CREATE TABLE bank(
    id uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    name text NOT NULL,
    address text NOT NULL
);

-- iirecoverable , can't be undone without data loss
ALTER TABLE credit_card DROP COLUMN issuing_bank;

INSERT INTO bank (id,name,address) VALUES('43cd61df-6c29-4e75-a31f-37fa311bbc5c','Bank of Commerce','15 Hemingway way, Alamadea CA 95782')
ALTER TABLE credit_card ADD COLUMN bank_id uuid NOT NULL DEFAULT '43cd61df-6c29-4e75-a31f-37fa311bbc5c';
ALTER TABLE credit_card ALTER COLUMN bank_id DROP DEFAULT;