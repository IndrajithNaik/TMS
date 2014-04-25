USE [SONALIKA_BGk]
GO

/****** Object:  UserDefinedFunction [dbo].[fConvertTens]    Script Date: 04/08/2014 00:52:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fConvertTens]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fConvertTens]
GO

USE [SONALIKA_BGk]
GO

/****** Object:  UserDefinedFunction [dbo].[fConvertTens]    Script Date: 04/08/2014 00:52:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--- function to convert number less than 100 to words
create Function [dbo].[fConvertTens](@decNumber varchar(2)) 
returns varchar(30) 
as 
Begin 
declare 
@strWords varchar(30) 
-- If amt is between 10 and 19
If Left(@decNumber, 1) = 1  
begin 
 Select @strWords = Case @decNumber 
     When '10' then 'Ten' 
     When '11' then 'Eleven' 
     When '12' then 'Twelve' 
     When '13' then 'Thirteen' 
     When '14' then 'Fourteen' 
     When '15' then 'Fifteen' 
     When '16' then 'Sixteen' 
     When '17' then 'Seventeen' 
     When '18' then 'Eighteen' 
     When '19' then 'Nineteen' 
 end 
end 
else  -- if amt is between 20 and 99
begin 
 Select @strWords = Case Left(@decNumber, 1) 
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
 Select @strWords = @strWords + dbo.fConvertDigit(Right(@decNumber, 1)) 
end 
 --Convert ones place digit. 
  
return @strWords 
end 

GO


