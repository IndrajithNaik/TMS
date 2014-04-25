USE [SONALIKA_BGk]
GO

/****** Object:  UserDefinedFunction [dbo].[fConvertHundreds]    Script Date: 04/08/2014 00:52:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fConvertHundreds]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fConvertHundreds]
GO

USE [SONALIKA_BGk]
GO

/****** Object:  UserDefinedFunction [dbo].[fConvertHundreds]    Script Date: 04/08/2014 00:52:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--- function to convert numbers less than 1000 to words
Create Function [dbo].[fConvertHundreds] (@decNumber varchar(3)) 
returns varchar(200) 
as  
Begin 
declare @strWords varchar(200) 
 
If  (Len(@decNumber) = 2)
begin
	If (left(@decNumber,2))='00'
		begin
			select @strWords=''
		end
	else
	begin
	 Select @strWords = Case left(@decNumber, 2) 
     When '0' then ''   
     When '2' then 'Twenty ' 
     When '3' then 'Thirty ' 
     When '4' then 'Forty ' 
     When '5' then 'Fifty ' 
     When '6' then 'Sixty ' 
     When '7' then 'Seventy ' 
     When '8' then 'Eighty ' 
     When '9' then 'Ninety '  
    end
   end
 end 
 Else  
Begin 
 Select @strWords = Case left(@decNumber,1) 
     When '1' then 'One' 
     When '2' then 'Two' 
     When '3' then 'Three' 
     When '4' then 'Four' 
     When '5' then 'Five' 
     When '6' then 'Six'  
     When '7' then 'Seven' 
     When '8' then 'Eight' 
     When '9' then 'Nine' 
     Else '' 
 end 
end 
  
 if ltrim(rtrim(@strWords)) <> '' and @strWords is not null 
  select @strWords = @strWords + ' Hundred '+ dbo.fconvertTens(right(@decNumber,2)) 
 else 
  select @strWords = dbo.fconvertTens(right(@decNumber,2)) 
  
return @strWords 
end 
 
GO


