-- Functions and Stored Precures
DROP FUNCTION IF EXISTS search_by_year;
DROP FUNCTION IF EXISTS concat_mki0;
DROP FUNCTION IF EXISTS add_two;

--Triggers and Stored Proecures
DROP TRIGGER IF EXISTS new_credit_card_transaction;
DROP FUNCTION IF EXISTS insert_into_enriched;
-- Relations
DROP TABLE IF EXISTS enriched_credit_card_transaction;
DROP TABLE IF EXISTS credit_card_transaction;
DROP TABLE IF EXISTS credit_card;
DROP TABLE IF EXISTS pos_machine;
DROP TABLE IF EXISTS client;
DROP TABLE IF EXISTS merchant;
DROP TABLE IF EXISTS bank;
