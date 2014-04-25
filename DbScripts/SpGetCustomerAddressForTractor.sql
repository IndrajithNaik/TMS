USE [SONALIKA_BKG]
GO

/****** Object:  StoredProcedure [dbo].[SpGetCustomerAddressForTractor]    Script Date: 04/06/2014 16:23:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpGetCustomerAddressForTractor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpGetCustomerAddressForTractor]
GO

USE [SONALIKA_BKG]
GO

/****** Object:  StoredProcedure [dbo].[SpGetCustomerAddressForTractor]    Script Date: 04/06/2014 16:23:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROC [dbo].[SpGetCustomerAddressForTractor] 
(
@INVOICE_ID INT
)
AS
BEGIN
SELECT 
     DISTINCT
       CUSTOMER_NAME
       ,CUSTOMER_ADDRESS
       ,CONTACT_NO_1 AS CONTACT_NO
       ,VILLAGE_NAME
      
       
 FROM  CUSTOMER C 
     LEFT OUTER JOIN SALES_INVOICES SI ON C.CUSTOMER_ID =SI.CUSTOMER_ID
     LEFT OUTER JOIN  VILLAGES V ON V.VILLAGE_ID=C.VILLAGE_ID
     
     
 WHERE SI.SALES_INVOICE_ID=@INVOICE_ID
END



GO


