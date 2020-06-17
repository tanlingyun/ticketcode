select * from tcaccounts;



insert into tcaccounts(id,sAppName,sAppId,sAppSecret,tCreateTime,bDisable) values(1,'测试','HXTC202001',replace(uuid(),'-',''),now(),0);