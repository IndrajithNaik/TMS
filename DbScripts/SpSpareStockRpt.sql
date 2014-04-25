USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpSpareStockRpt]    Script Date: 03/20/2014 00:42:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpSpareStockRpt]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpSpareStockRpt]
GO

USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpSpareStockRpt]    Script Date: 03/20/2014 00:42:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[SpSpareStockRpt] 
AS
BEGIN
SELECT 
       SPARE_PART_CODE
       ,SPARE_PART_DESCRIPTION
      ,QUANTITY
      ,SUB_TOTAL

      
 FROM SPARE_PURCHASES_SALES SPS
     INNER JOIN SPARE_PARTS SP ON SPS.SPARE_PART_ID=SP.SPARE_PART_ID
 WHERE SP.SPARE_STATUS=(SELECT MASTER_ID FROM MASTER WHERE MASTER_VALUE='ACTIVE')
END


GO


