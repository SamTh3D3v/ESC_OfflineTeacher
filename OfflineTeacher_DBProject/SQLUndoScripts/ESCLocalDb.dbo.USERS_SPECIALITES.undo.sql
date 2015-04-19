/****
CAUTION
  To prevent any potential data loss issues, you should review
  this script in detail before running it.

This SQL script was generated by the Configure Data Synchronization
dialog.  This script complements the script that can be used to create
the required database objects required for tracking changes.  This
script contains statements to remove such changes.

For more information see: How to: Configure a Database Server for Synchronization  in help.
****/


IF @@TRANCOUNT > 0
set ANSI_NULLS ON 
set QUOTED_IDENTIFIER ON 

GO
BEGIN TRANSACTION;


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USERS_SPECIALITES_Tombstone]') and TYPE = N'U') 
   DROP TABLE [dbo].[USERS_SPECIALITES_Tombstone];


IF @@TRANCOUNT > 0
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USERS_SPECIALITES_DeletionTrigger]') AND type = 'TR') 
   DROP TRIGGER [dbo].[USERS_SPECIALITES_DeletionTrigger] 

GO
COMMIT TRANSACTION;
