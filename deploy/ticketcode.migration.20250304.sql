SELECT count(*) FROM ticketcode.tcrequests_2021 order by id desc;
-- 2022_23
SELECT * FROM ticketcode.tcrequsets order by id desc;

CREATE INDEX `IX_TcRequsets_tCreateTime`
ON `tcrequsets`(`tCreateTime`);

create table tcrequests_2022_23
select * from ticketcode.tcrequsets where tCreateTime<'2024/1/1';

CREATE TABLE tcrequestlines_2022_23
SELECT line.* FROM tcrequestlines line
LEFT JOIN tcrequsets rq ON line.iRequestId = rq.Id
where rq.tCreateTime<'2024/1/1';

CREATE TABLE tcconsume_2022_23
SELECT c.* FROM tcconsume c
lEFT JOIN tcrequestlines line ON c.iRequestLineId=line.Id
LEFT JOIN tcrequsets rq ON line.iRequestId = rq.Id
where rq.tCreateTime<'2024/1/1';



DELETE c FROM tcconsume c
INNER JOIN tcrequestlines line ON c.iRequestLineId=line.Id
INNER JOIN tcrequsets rq ON line.iRequestId = rq.Id
where rq.tCreateTime<'2024/1/1' AND c.id>0;

select * from tcconsume order by id asc;
 
DELETE line FROM tcrequestlines line
INNER JOIN tcrequsets rq ON line.iRequestId = rq.Id
where rq.tCreateTime<'2024/1/1' AND line.Id>0;

select * from tcrequestlines order by id desc;

DELETE FROM tcrequsets WHERE tCreateTime<'2024/1/1' AND Id>0;
