-- CREATE OR REPLACE VIEW enriched_credit_card_transaction AS
CREATE TABLE my_temp_1 AS
SELECT tx.*, pm.zip_code AS purchase_zip_code, cc.client_name, bank.name AS bank_name, merchant.name AS merchant_name
FROM credit_card_transaction tx
INNER JOIN pos_machine pm
ON tx.pos_machine_id = pm.id
INNER JOIN credit_card cc
ON tx.credit_card_id = cc.id
INNER JOIN bank
ON bank.id = cc.bank_id
INNER JOIN merchant
ON merchant.id = pm.merchant_id;

CREATE VIEW my_view_1 AS
SELECT tx.*, pm.zip_code AS purchase_zip_code, cc.client_name, bank.name AS bank_name, merchant.name AS merchant_name
FROM credit_card_transaction tx
INNER JOIN pos_machine pm
ON tx.pos_machine_id = pm.id
INNER JOIN credit_card cc
ON tx.credit_card_id = cc.id
INNER JOIN bank
ON bank.id = cc.bank_id
INNER JOIN merchant
ON merchant.id = pm.merchant_id;