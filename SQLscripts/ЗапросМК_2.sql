select w.WareID,TypeNominal,w.QTY,cl.Name as ClassName,w.ContractId,ct.Number As CounractNumber,
(Select SUM (QTY) from store st  where st.WareID= w.WareID ) As OnStoreQTY,
SUM(CASE WHEN R.EndTime is NULL then 0 else 1 end) as EndCount,
count(r.routeoperationID) as [RouteOperationCount]
from Wares w
INNER JOIN Contract ct on ct.ContractId = w.ContractId 
INNER JOIN  ClassType cl on cl.ClassId = w.ClassId
LEFT JOIN lot l on l.WareId = w.WareId 
left join RouteOperation r on r.LotId = l.LotId
where w.ContractId = 20556 
and isnull(r.RouteOperationId,0)>0 
and l.LotTypeId = CHAR(204) + CHAR(202)
and isnull(r.[Disabled],0) <> 1 
group by  w.WareID,TypeNominal,w.QTY,cl.[Name],w.ContractId,
l.lottypeID,ct.Number
