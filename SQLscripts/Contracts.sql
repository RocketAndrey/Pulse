SELECT DISTINCT 
                         c.ContractId, c.Number, c.CreationDate, c.Code, c.Crypt, c.CloseDate, c.Priority, c.ApprovedPI, c.PZ, d.Name AS ContractState, c.ValidityPeriod, c.SignatureDate, p.Number AS Parent, 
                         cl.ShortName AS Client, ex.ShortName AS Executor, sp.ShortName AS Supplier,
						 --���������� �� ������� Outletter
						 ISNULL(shc.ShipCount, 0) AS ShipCount,
						 --�� ������, �� ����������� ��� ��� �� ����������
						 ISNULL(stc.StoreCount, 0) AS StoreCount, 
						 c.IsDeleted, 
						 --���� � ���������
                         w.ContractWaresQty AS ContractCount, 
						 --������� � �������
						 w.WareCount, 
						 --���-�� ���������� ���� � ��������
						 w.WareTestCount, 
						 --����������� �������
						 t.TestCount, c.Solution, c.TZ, c.ListReady, c.EndDate
FROM         dbo.Contract AS c LEFT OUTER JOIN
                         dbo.Contract AS p ON c.ParentId = p.ContractId INNER JOIN
                         dbo.DocumentState AS d ON c.ContractStateId = d.DocumentStateId INNER JOIN
                         dbo.Organization AS cl ON c.ClientId = cl.OrganizationId INNER JOIN
                         dbo.Organization AS ex ON c.ExecutorId = ex.OrganizationId INNER JOIN
                         dbo.Organization AS sp ON c.SupplierId = sp.OrganizationId INNER JOIN
                         dbo.ShippedQty AS shc ON c.ContractId = shc.ContractId LEFT OUTER JOIN
                         dbo.StoreQty AS stc ON c.ContractId = stc.ContractId LEFT OUTER JOIN
                         dbo.vw_WareAverage AS w ON c.ContractId = w.ContractId LEFT OUTER JOIN
                         dbo.vw_TestCount AS t ON c.ContractId = t.ContractId

 where c.ContractId > 600 and c.ContractId= 20556
