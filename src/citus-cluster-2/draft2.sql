CREATE TABLE IF NOT EXISTS person(
    id integer PRIMARY KEY,
    name text,
    age integer
);

WITH ids AS( SELECT generate_series(1,1000000) AS id)
INSERT INTO person (
    SELECT ids.id, 'PERSON' || ids.id, ids.id+20
    FROM ids ids
);

-- https://docs.citusdata.com/en/v11.2/get_started/concepts.html#distributed-data
 SELECT * FROM pg_dist_shard;

 SELECT
    shardid,
    node.nodename,
    node.nodeport
FROM pg_dist_placement placement
JOIN pg_dist_node node
  ON placement.groupid = node.groupid
 AND node.noderole = 'primary'::noderole
WHERE shardid = 102027;