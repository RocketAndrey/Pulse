exec sp_executesql N'SELECT DISTINCT [t8].[FirstStartDate], [t8].[SendDate], [t8].[Percent] AS [Percent], [t8].[ExecutorId] AS [UserId], [t8].[StartTime], [t8].[Qty], [t8].[value] AS [OperationState], CONVERT(NVarChar(MAX),[t8].[value2]) AS [PropertyNames], CONVERT(NVarChar(MAX),[t8].[value3]) AS [PropertyValuesString], [t8].[TestTypeId], [t8].[LotId], [t8].[PrefixNumber], [t8].[Number], [t8].[SuffixNumber], [t8].[ProductNumbers], [t8].[Defected], [t8].[WareId], [t8].[TypeNominal], [t8].[Excluded], [t8].[Priority], [t8].[Name], [t8].[Priority2], [t8].[Priority3]
FROM (
    SELECT [t0].[FirstStartDate], [t0].[SendDate], [t0].[Percent], [t0].[ExecutorId], [t0].[StartTime], [t0].[Qty], 
        (CASE 
            WHEN [t0].[Percent] = @p0 THEN CONVERT(NVarChar(30),@p1)
            WHEN ([t0].[StartTime] IS NULL) AND ([t0].[SendDate] IS NULL) THEN CONVERT(NVarChar(30),@p2)
            WHEN [t0].[StartTime] IS NULL THEN CONVERT(NVarChar(30),@p3)
            WHEN [t0].[StartTime] IS NOT NULL THEN CONVERT(NVarChar(30),@p4)
            ELSE @p5
         END) AS [value], [t4].[PropertyNames] AS [value2], [t4].[PropertyValuesString] AS [value3], [t0].[TestTypeId], [t0].[LotId], [t0].[PrefixNumber], [t0].[Number], [t0].[SuffixNumber], [t0].[ProductNumbers], [t1].[Defected], [t2].[WareId], [t2].[TypeNominal], [t2].[Excluded], [t2].[Priority], [t5].[Name], [t6].[Priority] AS [Priority2], [t7].[Priority] AS [Priority3], [t2].[ContractId], [t0].[generated], [t3].[RouteOperationId], [t0].[TestId], [t0].[IsArchive]
    FROM [dbo].[CurrentRO] AS [t0]
    INNER JOIN [dbo].[Defected] AS [t1] ON [t0].[LotId] = [t1].[LotId]
    INNER JOIN [dbo].[Wares] AS [t2] ON [t0].[WareId] = ([t2].[WareId])
    INNER JOIN [dbo].[RouteOperation] AS [t3] ON [t0].[LotId] = [t3].[LotId]
    LEFT OUTER JOIN [dbo].[Users] AS [t4] ON [t0].[ExecutorId] = ([t4].[UserId])
    INNER JOIN [dbo].[ClassType] AS [t5] ON [t5].[ClassId] = [t2].[ClassId]
    INNER JOIN [dbo].[Contract] AS [t6] ON [t6].[ContractId] = [t2].[ContractId]
    INNER JOIN [dbo].[Organization] AS [t7] ON [t7].[OrganizationId] = [t6].[ClientId]
    ) AS [t8]
WHERE ([t8].[ContractId] = @p6) AND (([t8].[generated] = 1) OR ([t8].[RouteOperationId] = ((
    SELECT [t10].[RouteOperationId]
    FROM (
        SELECT TOP (1) [t9].[RouteOperationId]
        FROM [dbo].[UsersHistory] AS [t9]
        WHERE ([t9].[StatusId] = @p7) AND ([t8].[RouteOperationId] = [t9].[RouteOperationId])
        ORDER BY [t9].[TimeStamp] DESC
        ) AS [t10]
    ))) OR ([t8].[TestId] = ((
    SELECT [t12].[TestId]
    FROM (
        SELECT TOP (1) [t11].[TestId]
        FROM [dbo].[TestApprovedHistory] AS [t11]
        WHERE (NOT ([t11].[Approved] = 1)) AND ([t8].[TestId] = ([t11].[TestId]))
        ORDER BY [t11].[TimeStamp] DESC
        ) AS [t12]
    )))) AND (NOT ([t8].[IsArchive] = 1))
	
	
	AND ([t8].[LotId] IN (@p8))',N'@p0 float,@p1 nvarchar(4000),@p2 nvarchar(4000),@p3 nvarchar(4000),@p4 nvarchar(4000),@p5 nvarchar(4000),@p6 bigint,@p7 int,@p8 bigint',@p0=100,@p1=N'Çàâåðøåíî',@p2=N'Íå íà÷àòà',@p3=N'Ïðîâåäåíî',@p4=N'Ïðîâîäèòñÿ',@p5=N'Îøèáêà â îïðåäåëåíèè ñîñòîÿíèÿ',@p6=20556,@p7=6,@p8=259215