use ticketcode;

select * from tcrequestlines line
left join tcrequsets rq on line.iRequestId = rq.Id
where rq.tCreateTime<'2022/1/1' and rq.tExpireTime>=now() and line.bConsume=0;

CREATE TABLE tcrequests_2021 
SELECT * FROM tcrequsets WHERE tCreateTime<'2022/1/1';

CREATE TABLE tcrequestlines_2021
SELECT line.* FROM tcrequestlines line
LEFT JOIN tcrequsets rq ON line.iRequestId = rq.Id
where rq.tCreateTime<'2022/1/1';

CREATE TABLE tcconsume_2021
SELECT c.* FROM tcconsume c
lEFT JOIN tcrequestlines line ON c.iRequestLineId=line.Id
LEFT JOIN tcrequsets rq ON line.iRequestId = rq.Id
where rq.tCreateTime<'2022/1/1';


DELETE c FROM tcconsume c
INNER JOIN tcrequestlines line ON c.iRequestLineId=line.Id
INNER JOIN tcrequsets rq ON line.iRequestId = rq.Id
where rq.tCreateTime<'2022/1/1' AND c.id>0;

select * from tcconsume order by id asc;
 
DELETE line FROM tcrequestlines line
INNER JOIN tcrequsets rq ON line.iRequestId = rq.Id
where rq.tCreateTime<'2022/1/1' AND line.Id>0;

select * from tcrequestlines order by id desc;

DELETE FROM tcrequsets WHERE tCreateTime<'2022/1/1' AND Id>0;