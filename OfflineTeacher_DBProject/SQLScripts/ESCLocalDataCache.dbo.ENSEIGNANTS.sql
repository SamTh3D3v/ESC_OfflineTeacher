/****
This SQL script was generated by the Configure Data Synchronization
dialog box. This script contains statements that create the
change-tracking columns, deleted-items table, and triggers on the
server database. These database objects are required by Synchronization
Services to successfully synchronize data between client and server
databases. For more information, see the
‘How to: Configure a Database Server for Synchronization’ topic in Help.
****/


IF @@TRANCOUNT > 0
set ANSI_NULLS ON 
set QUOTED_IDENTIFIER ON 

GO
BEGIN TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[ENSEIGNANTS] 
ADD [LastEditDate] DateTime NULL CONSTRAINT [DF_ENSEIGNANTS_LastEditDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
ALTER TABLE [dbo].[ENSEIGNANTS] 
ADD [CreationDate] DateTime NULL CONSTRAINT [DF_ENSEIGNANTS_CreationDate] DEFAULT (GETUTCDATE()) WITH VALUES
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ENSEIGNANTS_UpdateTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[ENSEIGNANTS_UpdateTrigger] 

GO
CREATE TRIGGER [dbo].[ENSEIGNANTS_UpdateTrigger] 
    ON [dbo].[ENSEIGNANTS] 
    AFTER UPDATE 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[ENSEIGNANTS] 
    SET [LastEditDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[ID_ENSEIGNANT] = [ENSEIGNANTS].[ID_ENSEIGNANT] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ENSEIGNANTS_InsertTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[ENSEIGNANTS_InsertTrigger] 

GO
CREATE TRIGGER [dbo].[ENSEIGNANTS_InsertTrigger] 
    ON [dbo].[ENSEIGNANTS] 
    AFTER INSERT 
AS 
BEGIN 
    SET NOCOUNT ON 
    UPDATE [dbo].[ENSEIGNANTS] 
    SET [CreationDate] = GETUTCDATE() 
    FROM inserted 
    WHERE inserted.[ID_ENSEIGNANT] = [ENSEIGNANTS].[ID_ENSEIGNANT] 
END;
GO
IF @@ERROR <> 0 
     ROLLBACK TRANSACTION;
COMMIT TRANSACTION;
