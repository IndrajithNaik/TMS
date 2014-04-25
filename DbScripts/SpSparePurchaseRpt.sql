USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpSparePurchaseRpt]    Script Date: 03/20/2014 00:42:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpSparePurchaseRpt]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpSparePurchaseRpt]
GO

USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpSparePurchaseRpt]    Script Date: 03/20/2014 00:42:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[SpSparePurchaseRpt] 
AS
BEGIN
SELECT 
       
       '' AS INVOICE_TOTAL
      ,'' AS INVOICE_NUMBER
      ,'' AS DATE
      

      
 FROM SPARE_PURCHASES_SALES SP
     
END


GO


