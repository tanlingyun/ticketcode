
use ticketcode;

ALTER TABLE `TcRequestLines` ADD `tExpireTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000';

update ticketcode.tcrequestlines l
set tExpireTime=(select tExpireTime from ticketcode.tcrequsets r where r.id=l.iRequestId)
where l.id<4505918;

-- 删除索引
DROP INDEX `IX_TcRequestLines_bConsume` ON `TcRequestLines`;

CREATE INDEX `IX_TcRequestLines_bConsume_tExpireTime` ON `TcRequestLines` (`bConsume`, `tExpireTime`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220510073733_Update20220510', '3.1.5');

select * from tcrequestlines order by id desc;
