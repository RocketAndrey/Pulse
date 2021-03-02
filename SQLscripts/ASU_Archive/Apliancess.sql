BEGIN TRANSACTION
/*
DELETE FROM [Appliance_Task_archive] where  ApplianceId<841
delete from [dbo].[ApplianceCharacteristics_arhive] where  ApplianceId<841
delete from [dbo].[ApplianceCompabilities_archive] where  ApplianceId<841
delete from [dbo].[ApplianceComponent_archive] where  ApplianceId<841
delete from [dbo].[ApplianceEquipment_archive] where ApplianceId<841
delete from Appliances_archive where ApplianceId<841
delete from [dbo].[ApplianceStages_archive] where ApplianceId<841
*/
INSERT INTO [dbo].[Appliance_Task_archive]
           ([ApplianceId]
           ,[TaskId])
 Select [ApplianceId]
           ,[TaskId] from Appliance_Task where ApplianceId<841

INSERT INTO [dbo].[ApplianceCharacteristics_arhive]
           ([ApplianceId]
           ,[CharacteristicId]
           ,[Value])

SELECT [ApplianceId]
      ,[CharacteristicId]
      ,[Value]
  FROM [dbo].[ApplianceCharacteristics] where ApplianceId < 841


INSERT INTO [dbo].[ApplianceCompabilities_archive]
           ([ApplianceId]
           ,[CompatibleId])
SELECT [ApplianceId]
      ,[CompatibleId]
  FROM [dbo].[ApplianceCompabilities] where ApplianceId < 841



INSERT INTO [dbo].[ApplianceComponent_archive]
           ([ApplianceComponentId]
           ,[ApplianceId]
           ,[ApplianceName]
           ,[Designation]
           ,[Class1]
           ,[Class2]
           ,[Typenominal]
           ,[Quantity]
           ,[Comments])
SELECT [ApplianceComponentId]
      ,[ApplianceId]
      ,[ApplianceName]
      ,[Designation]
      ,[Class1]
      ,[Class2]
      ,[Typenominal]
      ,[Quantity]
      ,[Comments]
  FROM [dbo].[ApplianceComponent] where ApplianceId < 841


INSERT INTO [dbo].[ApplianceEquipment_archive]
           ([ApplianceId]
           ,[EquipmentId]
           ,[AdapterEquipmentId]
		    ,[ApplianceEquipmentId])
SELECT [ApplianceId]
      ,[EquipmentId]
      ,[AdapterEquipmentId]
      ,[ApplianceEquipmentId]
  FROM [dbo].[ApplianceEquipment] where ApplianceId < 841



INSERT INTO [dbo].[Appliances_archive]
           ([ApplianceId]
           ,[Description]
           ,[TypeId]
           ,[BoxTypeId]
           ,[StateId]
           ,[WareId]
           ,[TaskTypeId]
           ,[UserId]
           ,[LocationId]
           ,[LastCheckDate]
           ,[NextCheckDate]
           ,[CreationDate]
           ,[InventoryNumber]
           ,[Qty]
           ,[PortsQty]
           ,[LastUserId]
           ,[NumberTO]
           ,[EquipmentXTypeId]
           ,[BoxType]
           ,[Project]
           ,[KUName]
           ,[Equipment])

SELECT [ApplianceId]
      ,[Description]
      ,[TypeId]
      ,[BoxTypeId]
      ,[StateId]
      ,[WareId]
      ,[TaskTypeId]
      ,[UserId]
      ,[LocationId]
      ,[LastCheckDate]
      ,[NextCheckDate]
      ,[CreationDate]
      ,[InventoryNumber]
      ,[Qty]
      ,[PortsQty]
      ,[LastUserId]
      ,[NumberTO]
      ,[EquipmentXTypeId]
      ,[BoxType]
      ,[Project]
      ,[KUName]
      ,[Equipment]
  FROM [dbo].[Appliances] where ApplianceId < 841

INSERT INTO [dbo].[ApplianceStages_archive]
           ([ApplianceId]
           ,[StateId]
           ,[PlanDate]
           ,[RealDate])

SELECT [ApplianceId]
      ,[StateId]
      ,[PlanDate]
      ,[RealDate]
  FROM [dbo].[ApplianceStages] where  ApplianceId<841





DELETE FROM [Appliance_Task] where  ApplianceId<841
delete from [dbo].[ApplianceCharacteristics] where  ApplianceId<841
delete from [dbo].[ApplianceCompabilities] where  ApplianceId<841
delete from [dbo].[ApplianceComponent] where  ApplianceId<841
delete from [dbo].[ApplianceEquipment] where ApplianceId<841
delete from ApplianceStages where  ApplianceId<841
delete from Appliances where ApplianceId<841

commit

select * from Appliances
