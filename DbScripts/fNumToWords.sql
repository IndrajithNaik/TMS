USE [SONALIKA_BGk]
GO

/****** Object:  UserDefinedFunction [dbo].[fNumToWords]    Script Date: 04/08/2014 00:52:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fNumToWords]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fNumToWords]
GO

USE [SONALIKA_BGk]
GO

/****** Object:  UserDefinedFunction [dbo].[fNumToWords]    Script Date: 04/08/2014 00:52:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

----Function numbers to word
Create function [dbo].[fNumToWords] (@decNumber decimal(12, 2)) 
Returns varchar(300) As 
Begin 
	Declare 
		@strnum varchar(100), 
		@strCents varchar(100), 
		@strWords varchar(300), 
		@intIndex integer 
 
		Select @strnum = Cast(@decNumber as varchar(100)) 
		Select @intIndex = CharIndex('.', @strnum) 
		select @strCents = '' 
 
		If(@decNumber>999999999.99) 
		BEGIN  
			RETURN '' 
		END 
 
		If @intIndex > 0  
		Begin 
			Select @strCents = dbo.fConvertTens(Right(@strnum, Len(@strnum) - @intIndex)) 
			Select @strnum = SubString(@strnum, 1, Len(@strnum) - 3) 
			If Len(@strCents) > 0 
				Select @strCents = @strCents + ' paise' 
		End 
 
		Declare @trail_zeros  varchar(3) 
		Declare @strthousands varchar(3) 
		Declare @strMillions varchar(3) 
		declare @dummy varchar(100)
 
		Set @trail_zeros = '000' 
		set @dummy=''
		
		If len(@strnum) <= 3 
		Begin 
			select @strWords = dbo.fConvertHundreds(left(@trail_zeros,3-len(right(@strnum,3)))+ right(@strnum,3)) 
		End  
		If len(@strnum) >= 4 and len(@strnum) <=5 
		Begin 
			select @strthousands = left(@trail_zeros,3 - len(left(right(@strnum,5),len(@strnum)-3))) + left(right(@strnum,5),len(@strnum)-3) 
			select @strWords = dbo.fConvertHundreds(@strthousands) + ' Thousand ' + dbo.fConvertHundreds(left(@trail_zeros,3-len(right(@strnum,3)))+ right(@strnum,3)) 
		End 
		
		If len(@strnum) >= 6 and len(@strnum) <=9 
		Begin 
				select @strMillions = left(@trail_zeros,3-len(left(@strnum,len(@strnum)-5))) + left(@strnum,len(@strnum)-5) 
				select @strthousands = left(right(@strnum,5),2) 
				select @dummy =dbo.fConvertHundreds(@strthousands)
				select @strWords = dbo.fConvertHundreds(@strMillions) + ' Lakhs ' + case @dummy when '' then '' else @dummy + ' Thousand ' end+ dbo.fConvertHundreds(left(@trail_zeros,3-len(right(@strnum,3)))+ right(@strnum,3)) 
		End 
 
		If @strCents <> '' 
			select @strWords = @strWords + ' and ' + @strCents + ' Only' 
		Else 
			select @strWords = @strWords + ' Only' 
 
 return  @strWords 
 
End 
GO


