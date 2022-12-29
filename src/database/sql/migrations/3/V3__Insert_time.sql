ALTER TABLE credit_card_transaction
 ADD COLUMN created_at timestamp NOT NULL DEFAULT now();