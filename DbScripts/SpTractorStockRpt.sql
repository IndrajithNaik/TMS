USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpTractorStockRpt]    Script Date: 03/20/2014 00:42:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpTractorStockRpt]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpTractorStockRpt]
GO

USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpTractorStockRpt]    Script Date: 03/20/2014 00:42:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[SpTractorStockRpt] 
AS
BEGIN
SELECT 
      TRACTOR_ENGINE_NO
      ,TRACTOR_CHASSIS_NO
      ,TRACTOR_MODEL_NAME
      ,TRACTOR_SHOWROOMRATE
      
 FROM TRACTOR_PURCHASES TP
     INNER JOIN TRACTOR_MODELS TM ON TM.TRACTOR_MODEL_ID=TP.TRACTOR_MODEL_ID
 WHERE TM.TRACTOR_STATUS=(SELECT MASTER_ID FROM MASTER WHERE MASTER_VALUE='ACTIVE')
END


GO


