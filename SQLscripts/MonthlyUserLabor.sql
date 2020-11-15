USE [Asulive]
GO
DROP PROCEDURE [dbo].[sp_PulseUsersMonthWork]
/****** Object:  StoredProcedure [dbo].[sp_SetWareCustNpp]    Script Date: 26.10.2020 13:29:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_PulseUsersMonthWork]
	@Month INT= null, @Year int= null
AS


select @Month = isnull(@Month,MONTH(GETDATE()))
select @Year = isnull(@Year,MONTH(GETDATE()))



Create table #Work
(
userID uniqueidentifier,
UserName nvarchar (50),
OnHands int,
CurrentMonthLabor float,
CurrentMonthCount int,
TodayLabor float,
TodayCount Int,
MonthNolaborCount int,
TodayNoLaborCount int

)

--���� � ����������� ���

INSERT INTO #Work (userID,userName,OnHands,CurrentMonthLabor,CurrentMonthCount   )
 
 select 
 ui.UserId, 
ui.LastName +' '+ ui.FN ,
null, Sum((labor.banchLabor* 1.1466  + labor.ItemLabor* S.QTY* 1.1466)/60) as SummaryLabor,
count(l.LotId )

from Lot as l  
	 inner join Store_WaresLot as sw on l.LotId=sw.LotId
	 inner join Store as s on s.StoreWareId=sw.StoreWareId
	 inner join UserInfo as ui on s.StoreUserId=ui.UserId
	 INNER JOIN Wares as w on l.WareId=w.WareId

	inner join ClassType cl on cl.ClassId = w.ClassId
	left outer join (select eta.TestChainItemID  , sum (eta.BatchLabor) as banchLabor,sum(eta.ItemLabor) as ItemLabor 
from Estimator.dbo.TestAction eta 
group by  eta.TestChainItemID ) labor on 
labor.TestChainItemID = 12

where l.LotTypeId='��'
and MONTH(s.DeliveryDate)=@Month and YEAR (s.DeliveryDate)=@Year

GROUP BY  ui.UserId, 
ui.LastName, ui.FN 
--��� ��������
INSERT INTO #Work (userID,userName,OnHands,CurrentMonthLabor,CurrentMonthCount,MonthNolaborCount,TodayCount,TodayLabor,TodayNoLaborCount )
 
select
 ui.UserId, 
ui.LastName +' '+ ui.FN,
null ,
SUM((labor.banchLabor* 1.1466  + labor.ItemLabor* l.QTY* 1.1466)/60) ,
Count(ro.RouteOperationId ),
SUM((Case when isnull(labor.ItemLabor,0)= 0 THEN 1 ELSE 0 END)),
SUM(CASE WHEN datediff([dd], ro.EndTime, getdate()) = 0 THEN 1 ELSE 0 END) ,
SUM(CASE WHEN datediff([dd], ro.EndTime, getdate()) = 0 THEN (labor.banchLabor* 1.1466  + labor.ItemLabor* l.QTY* 1.1466)/60 ELSE 0 END),
SUM((Case when isnull(labor.ItemLabor,0)= 0 and datediff([dd], ro.EndTime, getdate()) = 0 THEN 1 ELSE 0 END))

from RouteOperation as ro
 INNER JOIN UserInfo as ui on ro.UserID=ui.UserId
 INNER JOIN Operation as o on ro.OperationId=o.OperationId
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
 isnull(ro.Disabled,0) = 0 
   and MONTH (ro.EndTime) =@Month
   and  YEAR (ro.EndTime) = @year
  and isnull (ro.EndTime,0)> 0
  GROUP BY  ui.UserId, ui.LastName, ui.FN 

  --��������� ������� �������� �� ����� 
Update #Work set OnHands = (select top 1 sum(CASE WHEN r1.EndTime IS NULL AND r1.StartTime IS NOT NULL THEN 1 ELSE 0 END)
from RouteOperation r1 where r1.UserID= #Work.UserID Group by r1.UserId) 

--�� ��� ������� �� ������ � �������� �� ����� ���� 
INSERT INTO #Work (userID,userName,OnHands,CurrentMonthLabor,CurrentMonthCount,MonthNolaborCount  )
select r1.UserId,
ui.LastName +' '+ ui.FN,
sum(CASE WHEN r1.EndTime IS NULL AND r1.StartTime IS NOT NULL THEN 1 ELSE 0 END),
null,null,null

from RouteOperation r1
INNER JOIN UserInfo as ui on r1.UserID=ui.UserId
where r1.UserID not in (select UserId from #Work)
--and ui.InTablo= 1
Group by r1.UserId, ui.LastName ,ui.FN
HAVING sum(CASE WHEN r1.EndTime IS NULL AND r1.StartTime IS NOT NULL THEN 1 ELSE 0 END)> 0 
/*

SELECT TOP (100) PERCENT UserId, Name, OnHands, Today, ThisYear
FROM   (SELECT TOP (100) PERCENT dbo.RouteOperation.UserId, dbo.UserInfo.LastName + ' ' + dbo.UserInfo.FN AS Name, 
SUM(CASE WHEN dbo.RouteOperation.EndTime IS NULL AND dbo.RouteOperation.StartTime IS NOT NULL THEN 1 ELSE 0 END) AS OnHands, 
SUM(CASE WHEN datediff([dd], dbo.RouteOperation.EndTime, getdate()) = 0 THEN 1 ELSE 0 END) AS Today, 
SUM(CASE WHEN datediff([yyyy], dbo.RouteOperation.EndTime, getdate()) 
                  = 0 THEN 1 ELSE 0 END) AS ThisYear
         FROM   dbo.UserInfo INNER JOIN
                  dbo.RouteOperation ON dbo.UserInfo.UserId = dbo.RouteOperation.UserId
         WHERE  (dbo.UserInfo.InTablo = 'true') AND (dbo.RouteOperation.Disabled IS NULL OR
                  dbo.RouteOperation.Disabled = 0)
         GROUP BY dbo.UserInfo.LastName + ' ' + dbo.UserInfo.FN, dbo.RouteOperation.UserId) AS t
WHERE (OnHands > 0) OR
         (Today > 0) OR
         (ThisYear > 0)
*/
update #work set CurrentMonthCount = 0 where CurrentMonthCount is null
update #work set CurrentMonthLabor = 0 where CurrentMonthLabor is null


select userid As EmployeeID ,UserName as EmployeeName,isnull(sum(OnHands),0) as OnHands, sum(CurrentMonthLabor) as CurrentMonthLabor,sum( CurrentMonthCount) as    CurrentMonthCount,
isnull(sum(MonthNolaborCount),0) as MonthNolaborCount,
isnull(SUM(TodayLabor),0) as TodayLabor,
isnull(SUM(TodayCount),0)   as TodayCount,
isnull(SUM(TodayNoLaborCount),0) as TodayNoLaborCount
from #Work
group by userid,UserName

drop table #work 

GO


exec [sp_PulseUsersMonthWork] 10,2020

update RouteOperation set EndTime = StartTime where 
EndTime IS NULL AND StartTime IS NOT NULL
and UserId in ('5E62FCAC-1B3D-402B-ACBE-54F2208300AC',
'A2088485-2CAF-4B3B-BFAC-473B128884FF',
'415C24F5-8531-412E-96A7-0BCA23A2581C',
'2C1643B3-CFA5-487C-A26E-035EADC91346',
'7FE7459F-8BB1-4B2A-B788-9FA7CA5160D0',
'C1EC8376-CAC4-402F-8F26-9DC25E0667D4',
'1C2F2CDA-9AB7-43C4-AB8B-091327767262',
'1E98C5AF-632F-4BEA-99BD-B2B89DF2DDAC',
'8D19BD4C-DA4F-46AF-8133-3A66281B6F9E',
'18F87B22-A61C-4922-9CB8-455D8A5E5C3A',
'29AD29A9-47DC-4859-A55E-EA8969FF0391')