select count(*) from ticketcode.tcrequestlines where bConsume=1 and tExpireTime<'2022/1/1';

delete  from ticketcode.tcrequestlines where bConsume=1 and tExpireTime<'2022/1/1' and Id>0;