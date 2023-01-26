SELECT create_reference_table('merchant');
SELECT create_reference_table('pos_machine');
SELECT create_reference_table('bank');
-- SELECT create_distributed_table('credit_card', 'bank_id', colocate_with => 'bank');
SELECT create_reference_table('client', 'id');
SELECT create_distributed_table('credit_card', 'id');
SELECT create_distributed_table('credit_card_transaction', 'credit_card_id');


SELECT truncate_local_data_after_distributing_table($$citus.bank$$)

SELECT undistribute_table('banks');