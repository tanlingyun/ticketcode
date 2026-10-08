ALTER TABLE `TcRequestLines` ADD `iConsumeType` int NOT NULL DEFAULT 0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20200928015257_AddConsumeType', '3.1.5');

