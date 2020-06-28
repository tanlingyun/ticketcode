
select * from tcaccounts;



insert into tcaccounts(id,sAppName,sAppId,sAppSecret,tCreateTime,bDisable) values(1,'测试','HXTC202001',replace(uuid(),'-',''),now(),0);

-- 微信/ota订单码
-- 微信/ota游客码

-- 分析订单码
-- 分析游客码

use ticketcode;

select * from tcgroups;

# 分配取票码分组
INSERT INTO tcgroups(Id,iPrefixCode,sName,iLength,iUsedNumber,iIncrNumber,iMinNumber,tCreateTime,iCurrAvaNumber,bDisable,bDelete)
values(2,10,'微信/OTA订单取票码',7,0,10000,2000,now(),0,0,0);

INSERT INTO tcgroups(Id,iPrefixCode,sName,iLength,iUsedNumber,iIncrNumber,iMinNumber,tCreateTime,iCurrAvaNumber,bDisable,bDelete)
values(3,20,'分销订单取票码',7,0,10000,2000,now(),0,0,0);

INSERT INTO tcgroups(Id,iPrefixCode,sName,iLength,iUsedNumber,iIncrNumber,iMinNumber,tCreateTime,iCurrAvaNumber,bDisable,bDelete)
values(4,21,'分销游客取票码',7,0,10000,2000,now(),0,0,0);

# 分配账号
select * from tcaccounts;
select replace(uuid(),"-","");
INSERT INTO tcaccounts(Id,sAppName,sAppId,sAppSecret,tCreateTime,bDisable) values(2,'微信Web',"HXWXWEB",'debed64fb5ff11ea98285404a66171ec',now(),0);
INSERT INTO tcaccounts(Id,sAppName,sAppId,sAppSecret,tCreateTime,bDisable) values(3,'微信OTA接口',"HXWXOTA",'e3008daab5ff11ea98285404a66171ec',now(),0);
INSERT INTO tcaccounts(Id,sAppName,sAppId,sAppSecret,tCreateTime,bDisable) values(4,'分销',"HXAGENT",'e7880816b5ff11ea98285404a66171ec',now(),0);

select * from tcgroupinaccount;

INSERT INTO tcgroupinaccount(id,iGroupId,iAccountId) values(1,2,2);
INSERT INTO tcgroupinaccount(id,iGroupId,iAccountId) values(2,3,2);
INSERT INTO tcgroupinaccount(id,iGroupId,iAccountId) values(3,3,3);
INSERT INTO tcgroupinaccount(id,iGroupId,iAccountId) values(4,3,4);

