
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `TcAccounts` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `sAppName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `sAppId` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `sAppSecret` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `tCreateTime` datetime(6) NOT NULL,
    `bDisable` tinyint(1) NOT NULL,
    CONSTRAINT `PK_TcAccounts` PRIMARY KEY (`id`)
);

CREATE TABLE `TcGroups` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `iPrefixCode` int NOT NULL,
    `sName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `iLength` int NOT NULL,
    `iUsedNumber` bigint NOT NULL,
    `iIncrNumber` bigint NOT NULL,
    `iMinNumber` bigint NOT NULL,
    `tCreateTime` datetime(6) NOT NULL,
    `iCurrAvaNumber` bigint NOT NULL,
    `tUpdateTime` datetime(6) NULL,
    `bDisable` tinyint(1) NOT NULL,
    `bDelete` tinyint(1) NOT NULL,
    CONSTRAINT `PK_TcGroups` PRIMARY KEY (`id`)
);

CREATE TABLE `TcGroupInAccount` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `iGroupId` bigint NOT NULL,
    `iAccountId` bigint NOT NULL,
    CONSTRAINT `PK_TcGroupInAccount` PRIMARY KEY (`id`),
    CONSTRAINT `FK_TcGroupInAccount_TcAccounts_iAccountId` FOREIGN KEY (`iAccountId`) REFERENCES `TcAccounts` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TcGroupInAccount_TcGroups_iGroupId` FOREIGN KEY (`iGroupId`) REFERENCES `TcGroups` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `TcRequsets` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `sRequestNo` longtext CHARACTER SET utf8mb4 NOT NULL,
    `sOuterNo` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `iNumber` int NOT NULL,
    `tCreateTime` datetime(6) NOT NULL,
    `tExpireTime` datetime(6) NOT NULL,
    `iGroupId` bigint NOT NULL,
    `iAccountId` bigint NOT NULL,
    `sMemo` varchar(250) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_TcRequsets` PRIMARY KEY (`id`),
    CONSTRAINT `FK_TcRequsets_TcAccounts_iAccountId` FOREIGN KEY (`iAccountId`) REFERENCES `TcAccounts` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TcRequsets_TcGroups_iGroupId` FOREIGN KEY (`iGroupId`) REFERENCES `TcGroups` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `TcRequestLines` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `iRequestId` bigint NOT NULL,
    `iCode` bigint NOT NULL,
    `iFullCode` bigint NOT NULL,
    `bConsume` tinyint(1) NOT NULL,
    `tConsumeTime` datetime(6) NULL,
    CONSTRAINT `PK_TcRequestLines` PRIMARY KEY (`id`),
    CONSTRAINT `FK_TcRequestLines_TcRequsets_iRequestId` FOREIGN KEY (`iRequestId`) REFERENCES `TcRequsets` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `TcConsume` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `iRequestLineId` bigint NOT NULL,
    `iFullCode` bigint NOT NULL,
    `tConsumeTime` datetime(6) NOT NULL,
    `iGroupId` bigint NOT NULL,
    `iAccountId` bigint NOT NULL,
    CONSTRAINT `PK_TcConsume` PRIMARY KEY (`id`),
    CONSTRAINT `FK_TcConsume_TcAccounts_iAccountId` FOREIGN KEY (`iAccountId`) REFERENCES `TcAccounts` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TcConsume_TcGroups_iGroupId` FOREIGN KEY (`iGroupId`) REFERENCES `TcGroups` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_TcConsume_TcRequestLines_iRequestLineId` FOREIGN KEY (`iRequestLineId`) REFERENCES `TcRequestLines` (`id`) ON DELETE RESTRICT
);

CREATE UNIQUE INDEX `IX_TcAccounts_sAppId` ON `TcAccounts` (`sAppId`);

CREATE INDEX `IX_TcConsume_iAccountId` ON `TcConsume` (`iAccountId`);

CREATE INDEX `IX_TcConsume_iFullCode` ON `TcConsume` (`iFullCode`);

CREATE INDEX `IX_TcConsume_iGroupId` ON `TcConsume` (`iGroupId`);

CREATE UNIQUE INDEX `IX_TcConsume_iRequestLineId` ON `TcConsume` (`iRequestLineId`);

CREATE INDEX `IX_TcGroupInAccount_iAccountId` ON `TcGroupInAccount` (`iAccountId`);

CREATE INDEX `IX_TcGroupInAccount_iGroupId` ON `TcGroupInAccount` (`iGroupId`);

CREATE INDEX `IX_TcRequestLines_iFullCode` ON `TcRequestLines` (`iFullCode`);

CREATE INDEX `IX_TcRequestLines_iRequestId` ON `TcRequestLines` (`iRequestId`);

CREATE INDEX `IX_TcRequsets_iGroupId` ON `TcRequsets` (`iGroupId`);

CREATE UNIQUE INDEX `IX_TcRequsets_iAccountId_iGroupId_sOuterNo` ON `TcRequsets` (`iAccountId`, `iGroupId`, `sOuterNo`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20200630024048_InitDatabase', '3.1.5');

CREATE INDEX `IX_TcRequestLines_bConsume` ON `TcRequestLines` (`bConsume`);

