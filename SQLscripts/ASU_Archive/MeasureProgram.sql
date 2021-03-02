USE [asulive]
GO
BEGIN TRANSACTION
--очищаем архив
SET ANSI_NULLS ON
DELETE from [MeasureProgram_RouteOperation_archive] where  MeasureProgramId >342
delete from [dbo].[MeasureProgram_Documents_Archive]where  MeasureProgramId >342
delete from [dbo].[MeasureProgram_Archive]where  MeasureProgramId >342

--заполняем архив из записями старых договоров
INSERT INTO [MeasureProgram_RouteOperation_archive]
([MeasureProgramId],[RouteOperationId])
select [MeasureProgramId],[RouteOperationId]
from MeasureProgram_RouteOperation where MeasureProgramId < 343

INSERT INTO[dbo].[MeasureProgram_Documents_archive] 
([MeasureProgramId], [DocumentId] )
select [MeasureProgramId], [DocumentId] 
from MeasureProgram_Documents where  MeasureProgramId < 343



INSERT INTO [dbo].[MeasureProgram_Archive]
           ([MeasureProgramId]
           ,[Name]
           ,[InventoryId]
           ,[DocumentId]
           ,[UserId]
           ,[CreateDate]
           ,[EditDate]
           ,[IsArchive]
           ,[CheckSum]
           ,[Description]
           ,[LastEditUserId])
SELECT [MeasureProgramId]
      ,[Name]
      ,[InventoryId]
      ,[DocumentId]
      ,[UserId]
      ,[CreateDate]
      ,[EditDate]
      ,[IsArchive]
      ,[CheckSum]
      ,[Description]
      ,[LastEditUserId]
  FROM [dbo].[MeasureProgram]
where MeasureProgramId < 343


--очищаем MeasureProgram_RouteOperation
DELETE FROM MeasureProgram_RouteOperation 
where MeasureProgramId < 343
delete  from MeasureProgram_Documents where  MeasureProgramId < 343
delete from MeasureProgram where MeasureProgramId < 343


COMMIT

