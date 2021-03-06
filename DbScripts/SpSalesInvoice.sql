USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpSalesInvoice]    Script Date: 03/31/2014 23:19:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpSalesInvoice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SpSalesInvoice]
GO

USE [TRACTOR_DATABASE]
GO

/****** Object:  StoredProcedure [dbo].[SpSalesInvoice]    Script Date: 03/31/2014 23:19:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[SpSalesInvoice] 
(
@INVOICE_ID INT
)
AS
BEGIN
SELECT SI.SALES_INVOICE_ID AS INVOICE_NO
       ,INVOICE_DATE  
        ,INVOICE_DISCOUNT   
       ,INVOICE_VAT_PERCENT
       ,INVOICE_GRANDTOTAL
       ,DC_NO
       ,DC_DATE
       ,SELL_TYPE
       ,TRACTOR_ENGINE_NO
       ,TRACTOR_CHASSIS_NO
       ,TRACTOR_SPECIFICATION
       ,CUSTOMER_NAME
       ,CUSTOMER_ADDRESS
       ,CONTACT_NO_1 AS CONTACT_NO
       ,TRACTOR_MODEL_NAME
       
 FROM SALES_INVOICES SI
     LEFT OUTER JOIN CUSTOMER C ON C.CUSTOMER_ID =SI.CUSTOMER_ID
     LEFT OUTER JOIN TRACTOR_PURCHASES TP ON TP.TRACTOR_ID=SI.TRACTOR_ID
     LEFT OUTER JOIN TRACTOR_MODELS TM ON TM.TRACTOR_MODEL_ID=TP.TRACTOR_MODEL_ID
     
 WHERE SI.SALES_INVOICE_ID=@INVOICE_ID  AND SELL_TYPE=(SELECT MASTER_ID FROM MASTER WHERE MASTER_VALUE='TRACTOR')
END


GO


