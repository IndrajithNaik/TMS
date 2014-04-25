USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpTractorSalesInvoice]    Script Date: 03/31/2014 23:21:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpGetCustomerAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpGetCustomerAddress]
GO

USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpGetCustomerAddress]    Script Date: 03/31/2014 23:21:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create PROC [dbo].[SpGetCustomerAddress] 
(
@CUSTOMER_ID INT
)
AS
BEGIN
SELECT 
       CUSTOMER_NAME
       ,CUSTOMER_ADDRESS
       ,CONTACT_NO_1 AS CONTACT_NO
       ,VILLAGE_NAME
      
       
 FROM  CUSTOMER C 
     LEFT OUTER JOIN SALES_INVOICES SI ON C.CUSTOMER_ID =SI.CUSTOMER_ID
     LEFT OUTER JOIN  VILLAGES V ON V.VILLAGE_ID=C.VILLAGE_ID
     
     
 WHERE C.CUSTOMER_ID=@CUSTOMER_ID
END


GO


