ALTER TABLE `TcRequestLines` ADD `iPrefixCode` int NOT NULL DEFAULT 0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220617074421_AddPrefixCode', '3.1.5');

