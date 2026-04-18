--GET REPETEAD
select "Name", Count("Name")
from exchanges
group by "Name";

--UPDATE
update exchanges 
set "Name" = 'contadoconliqui'
where "Name" = 'Contado con liquidación';

--GET NAME AND UPDATE TIME
select "Name", "UpdateTime", Count("UpdateTime")
from exchanges
group by "Name", "UpdateTime"
order by count desc;

--GET BY CONDITION
select "ID"  from exchanges 
where "UpdateTime" = '2026-04-14 21:59:00.000 +0100' and "Name" = 'blmayoristaue'
limit 100;

--DELETE
DELETE  from exchanges
where "ID" in (
'1482d575-365d-41c6-882c-c480e7c92dcc',
'cf7e8ff8-f88f-44c8-8260-c5eca0c9c057',
'3f0965ce-1805-4d57-a0b6-ff11130f468e',
'0275b0b1-c7f6-4d76-b7e3-80d53c423941',
'5e8b3d1b-7ca2-4662-99ca-7b84ff827fa5',
'64e5c180-2330-4292-b804-43b23f6756cb',
'd2607cc3-96f4-468f-85ba-9d979dad57da'
);
