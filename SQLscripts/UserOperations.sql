USE [Asulive]
GO
DROP PROCEDURE [dbo].[sp_PulseUserOperations]
/****** Object:  StoredProcedure [dbo].[sp_SetWareCustNpp]    Script Date: 26.10.2020 13:29:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_PulseUserOperations]
	@userID uniqueidentifier,
	@Month int,
	@Year int, 
	@OnHand bit = 0,
	@NoLabor bit =  0,
	@Query bit = 0
	AS
Create table #Work 
(
[TypeNominal] nvarchar(150),
[EmployeeName] nvarchar(150),
[OperationName] nvarchar(150),
[CardNumber] nvarchar (50),
[OperationLabor] float,
[StartDate] datetime,
[EndDate] datetime
)

IF @Query  =0
--операции на руках и выполненные
BEGIN
Insert INTO #Work ([TypeNominal],[EmployeeName],[OperationName],[CardNumber],[OperationLabor],[StartDate],[EndDate])

select
w.TypeNominal as [TypeNominal],
ui.LastName +' '+ ui.FN as [EmployeeName],
bo.Name as [OperationName],

l.PrefixNumber + '-' +t.TestTypeId+ '-' + CAST(l.Number AS varchar) + ISNULL(CASE WHEN l.SuffixNumber LIKE 'о' THEN NULL ELSE l.SuffixNumber END, '') as [CardNumber],
(labor.banchLabor* 1.1466  + labor.ItemLabor* L.QTY* 1.1466)/60 as OperationLabor,
casT (ro.StartTime  as DATE) as [StartDate],
CAST (ro.EndTime as DATE) as [EndDate]

from RouteOperation as ro
 INNER JOIN UserInfo as ui on ro.UserID=ui.UserId
 INNER JOIN Operation as o on ro.OperationId=o.OperationId
 INNER JOIN Test as t on t.TestId=o.TestId
 INNER JOIN BaseOperation as bo on bo.BaseOperationId=o.baseOperationId
 INNER JOIN Lot as l on ro.LotId=l.LotId
 INNER JOIN Wares as w on l.WareId=w.WareId
 inner join ClassType cl on cl.ClassId = w.ClassId
 left OUTER  join Estimator_TestChainItem  eet on eet.AsuClassID = cl.ClassId and eet.[AsuBaseOperationID] =  bo.BaseOperationId
 left outer join (select eta.TestChainItemID  , sum (eta.BatchLabor) as banchLabor,sum(eta.ItemLabor) as ItemLabor 
from Estimator.dbo.TestAction eta 
group by  eta.TestChainItemID ) labor on 
labor.TestChainItemID = eet.[TestChainItemID] 

 where 

   (ro.EndTime IS NULL or @OnHand= 0)
   AND (ro.StartTime IS NOT NULL or @OnHand= 0)
   and (ItemLabor is null or @NoLabor =  0)
   and (month(ro.EndTime)=@Month or @OnHand= 1 )
   and (year(ro.EndTime)=@year or @OnHand= 1 )
   and ui.UserId= @userID

insert INTO #Work ([TypeNominal],[EmployeeName],[OperationName],[CardNumber],[OperationLabor],[StartDate],[EndDate])
    select 

w.TypeNominal as [Тип ЭРИ],
ui.LastName +' '+ ui.FN as [Сотрудник],
'Учет и регистрация ЭКБ' as [Операция],
l.PrefixNumber +  '-' + CAST(l.Number AS varchar) + ISNULL(CASE WHEN l.SuffixNumber LIKE 'о' THEN NULL ELSE l.SuffixNumber END, '') as [№ МК],
 (labor.banchLabor* 1.1466  + labor.ItemLabor* S.QTY* 1.1466)/60 as OperationLabor,
Cast(s.RegistrationDate as date),
CAST (s.DeliveryDate as DATE)
from Lot as l  
	 inner join Store_WaresLot as sw on l.LotId=sw.LotId
	 inner join Store as s on s.StoreWareId=sw.StoreWareId
	 inner join UserInfo as ui on s.StoreUserId=ui.UserId
	 INNER JOIN Wares as w on l.WareId=w.WareId
 Inner JOIN [Contract] ct on ct.ContractId = w.ContractId
 inner join Organization org on org.OrganizationId= ct.ClientId 
 inner join ClassType cl on cl.ClassId = w.ClassId
 left outer join (select eta.TestChainItemID  , sum (eta.BatchLabor) as banchLabor,sum(eta.ItemLabor) as ItemLabor 
from Estimator.dbo.TestAction eta 
group by  eta.TestChainItemID ) labor on 
labor.TestChainItemID = 12

where l.LotTypeId='ОС'
AND s.StoreUserId = @userID
and (ItemLabor is null or @NoLabor =  0)
AND (s.DeliveryDate IS Not null and @OnHand = 0)
and (MONTH(s.DeliveryDate)=@Month or @OnHand=1) 
and (YEAR (s.DeliveryDate)=@Year or @OnHand = 1 )
 
insert INTO #Work ([TypeNominal],[EmployeeName],[OperationName],[CardNumber],[OperationLabor],[StartDate],[EndDate])
 select 
w.TypeNominal as [Тип ЭРИ],
ui.LastName +' '+ ui.FN as [Сотрудник],
'Оформление документации на отгрузку' as [Операция],

l.PrefixNumber +  '-' + CAST(l.Number AS varchar) + ISNULL(CASE WHEN l.SuffixNumber LIKE 'о' THEN NULL ELSE l.SuffixNumber END, '') as [№ МК],
(labor.banchLabor * 1.1466  + labor.ItemLabor* l.QTY* 1.1466)/60 as OperationLabor,
o.CreationDate,
o.CreationDate
from Outletter as o
     inner join Lot_Outletter as lo on o.OutletterId=lo.OutletterId
	 Inner Join Lot as L on l.LotId =  lo.LotId
     inner join UserInfo as ui on lo.UserId=ui.UserId
	 INNER JOIN Wares as w on L.WareId=w.WareId
 Inner JOIN [Contract] ct on ct.ContractId = o.ContractId
 inner join Organization org on org.OrganizationId= ct.ClientId 
 inner join ClassType cl on cl.ClassId = w.ClassId
 left outer join (select eta.TestChainItemID  , sum (eta.BatchLabor) as banchLabor,sum(eta.ItemLabor) as ItemLabor 
from Estimator.dbo.TestAction eta 
group by  eta.TestChainItemID ) labor on 
labor.TestChainItemID = 2510

where o.Sent=1 
and lo.UserId = @userID

and (ItemLabor is null or @NoLabor =  0)
AND (o.CreationDate  IS Not null and @OnHand = 0)
and (MONTH(o.CreationDate)=@Month or @OnHand=1) 
and (YEAR (o.CreationDate)=@Year or @OnHand = 1 )


insert INTO #Work ([TypeNominal],[EmployeeName],[OperationName],[CardNumber],[OperationLabor],[StartDate],[EndDate])
SELECT 
w.TypeNominal as [Тип ЭРИ],
ui.LastName +' '+ ui.FN as [Сотрудник],
'Подготовка протоколов и заключений по результатам испытаний' as [Операция],

l.PrefixNumber +  '-' + CAST(l.Number AS varchar) + ISNULL(CASE WHEN l.SuffixNumber LIKE 'о' THEN NULL ELSE l.SuffixNumber END, '') as [№ МК],
 (labor.banchLabor* 1.1466  + labor.ItemLabor* l.QTY* 1.1466)/60 as OperationLabor,
CAST (cp.Response as DATE) as [Дата],
CAST (cp.Request as DATE) as [Дата]

FROM TemplateDocument td
JOIN CoordinatePerson cp ON td.TemplateDocumentId = cp.TemplateDocumentId
JOIN Template t ON td.TemplateId = t.TemplateId
JOIN DocumentType dt ON t.DocumentTypeId = dt.DocumentTypeId
JOIN CoordinatePersonRoles cpr ON cp.CoordinatePersonRolesId = cpr.CoordinatePersonRolesId
JOIN UserInfo ui on ui.UserId = cp.UserId
 Inner Join Lot as L on l.LotId =  td.LotId
 
	 INNER JOIN Wares as w on L.WareId=w.WareId
 Inner JOIN [Contract] ct on ct.ContractId = w.ContractId
 inner join Organization org on org.OrganizationId= ct.ClientId 
 inner join ClassType cl on cl.ClassId = w.ClassId
 left outer join (select eta.TestChainItemID  , sum (eta.BatchLabor) as banchLabor,sum(eta.ItemLabor) as ItemLabor 
from Estimator.dbo.TestAction eta 
group by  eta.TestChainItemID ) labor on 
labor.TestChainItemID = 44


WHERE cpr.Name = 'ГОП'
and Cp.UserId = @userid
AND cp.Response IS NOT NULL AND cp.Request IS NOT NULL

and (ItemLabor is null or @NoLabor =  0)
AND (cp.Response   IS Not null and @OnHand = 0)
and (MONTH(cp.Response)=@Month or @OnHand=1) 
and (YEAR (cp.Response)=@Year or @OnHand = 1 )

and YEAR(cp.Request) = @Year and MONTH(cp.Request)  =@Month

END
 ELSE
 --Непринятые операции (в очереди)
 BEGIN

 Insert INTO #Work 
(
[TypeNominal],
[EmployeeName],
[OperationName] ,
[CardNumber] ,
[OperationLabor],
[StartDate] ,
[EndDate] 
)

select
w.TypeNominal as [TypeNominal],
ui.LastName +' '+ ui.FN as [EmployeeName],
bo.Name as [OperationName],

l.PrefixNumber + '-' +t.TestTypeId+ '-' + CAST(l.Number AS varchar) + ISNULL(CASE WHEN l.SuffixNumber LIKE 'о' THEN NULL ELSE l.SuffixNumber END, '') as [CardNumber],
(labor.banchLabor* 1.1466  + labor.ItemLabor* L.QTY* 1.1466)/60 as OperationLabor,
casT (ro.StartTime  as DATE) as [StartDate],
CAST (ro.EndTime as DATE) as [EndDate]

from RouteOperation as ro
 INNER JOIN UserInfo as ui on ro.UserID=ui.UserId
 INNER JOIN Operation as o on ro.OperationId=o.OperationId
 INNER JOIN Test as t on t.TestId=o.TestId
 INNER JOIN BaseOperation as bo on bo.BaseOperationId=o.baseOperationId
 INNER JOIN Lot as l on ro.LotId=l.LotId
 INNER JOIN Wares as w on l.WareId=w.WareId
 inner join ClassType cl on cl.ClassId = w.ClassId
 left OUTER  join Estimator_TestChainItem  eet on eet.AsuClassID = cl.ClassId and eet.[AsuBaseOperationID] =  bo.BaseOperationId
 left outer join (select eta.TestChainItemID  , sum (eta.BatchLabor) as banchLabor,sum(eta.ItemLabor) as ItemLabor 
from Estimator.dbo.TestAction eta 
group by  eta.TestChainItemID ) labor on 
labor.TestChainItemID = eet.[TestChainItemID] 
 LEFT  JOIN ( select r1.RouteOperationId, r1.LotId,o1.[order]  from 
 RouteOperation r1 
inner  JOIN Operation o1 on o1.OperationId = r1.OperationId 
 where ISNULL(r1.EndTime,0) >0 ) p On p.LotId= ro.LotId 

 where 
isnull(ro.Disabled,0) = 0 
and ro.UserID= @UserID
  and ro.StartTime is NULL
 and ro.EndTime is null
 and   (p.[Order] = o.[order]-1 OR o.[order]=1)
 and ro.RouteOperationId > 388505
 END

 --
 select 
[TypeNominal],
[EmployeeName],
[OperationName] ,
[CardNumber] ,
Isnull([OperationLabor],0) as[OperationLabor],
[StartDate] ,
[EndDate] 
 from #Work
 ORDER BY [EndDate]

   drop table #Work
go 

[sp_PulseUserOperations] '0D44ED13-1D9B-43DD-963C-9D7218886ACA',10,2020,0,0,0

