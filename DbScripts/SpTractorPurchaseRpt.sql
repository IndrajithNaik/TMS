USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpTractorPurchaseRpt]    Script Date: 03/20/2014 00:42:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpTractorPurchaseRpt]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpTractorPurchaseRpt]
GO

USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpTractorPurchaseRpt]    Script Date: 03/20/2014 00:42:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[SpTractorPurchaseRpt] 
AS
BEGIN
SELECT 
       
       '' AS INVOICE_TOTAL
      ,'' AS INVOICE_NUMBER
      ,'' AS DATE
      

      
 FROM TRACTOR_PURCHASES TP
     
END


GO


