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
	@NoLabor bit =  0
	AS
Create table #Work 
(
[TypeNominal] nvarchar(50),
[EmployeeName] nvarchar(50),
[OperationName] nvarchar(150),
[CardNumber] nvarchar (50),
[OperationLabor] float,
[StartDate] datetime,
[EndDate] datetime
)

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

l.PrefixNumber + '-' +t.TestTypeId+ '-' + CAST(l.Number AS varchar) + ISNULL(CASE WHEN l.SuffixNumber LIKE 'î' THEN NULL ELSE l.SuffixNumber END, '') as [CardNumber],
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
 Inner JOIN [Contract] ct on ct.ContractId = w.ContractId
 inner join Organization org on org.OrganizationId= ct.ClientId 
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

[sp_PulseUserOperations] 'd5769a79-e378-4172-94d2-79e2ad8eb99f',10,2020,1,0

