USE [DataAnalystDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_ProcessData_SelectWhere]    Script Date: 26-03-2016 10:16:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		JayamSoft
-- Create date: 17/Mar/2016
-- Description:	To get list of contacts from import data table
-- =============================================
CREATE PROCEDURE [dbo].[sp_ProcessData_SelectWhere] 
	-- Add the parameters for the stored procedure here
	--@dfltWhere nvarchar(max)
	@pDataYear nvarchar(50),
	@pSupplierId int,
	@pListofColumn nvarchar(Max),
	@pPeriodNo int = NULL,
	@pRptType nvarchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @sql nvarchar(max) = ''

	Declare @vDataMonth table (DataMonth nvarchar(5), MonName nvarchar(5));
	--Declare @Mon nvarchar(3);
	--Declare @iCtr int = 1;
	--While @iCtr <= 12
	--Begin
	--	Set @Mon  = '';
	--	if(len(@iCtr) > 1)
	--		Set @Mon  = @Mon + @iCtr;
	--	Else
	--		Set @Mon  = '0'+ @iCtr;

	--	Insert into @vDataMonth (DataMonth) values (@Mon);	
	--	Set @iCtr = @iCtr + 1;
	--End
	
	Insert into @vDataMonth (DataMonth, MonName) values ('01', 'Jan');	
	Insert into @vDataMonth (DataMonth, MonName) values ('02', 'Feb');	
	Insert into @vDataMonth (DataMonth, MonName) values ('03', 'Mar');	
	Insert into @vDataMonth (DataMonth, MonName) values ('04', 'Apr');	
	Insert into @vDataMonth (DataMonth, MonName) values ('05', 'May');	
	Insert into @vDataMonth (DataMonth, MonName) values ('06', 'Jun');	
	Insert into @vDataMonth (DataMonth, MonName) values ('07', 'Jul');	
	Insert into @vDataMonth (DataMonth, MonName) values ('08', 'Aug');	
	Insert into @vDataMonth (DataMonth, MonName) values ('09', 'Sep');	
	Insert into @vDataMonth (DataMonth, MonName) values ('10', 'Oct');	
	Insert into @vDataMonth (DataMonth, MonName) values ('11', 'Nov');	
	Insert into @vDataMonth (DataMonth, MonName) values ('12', 'Dec');	
	--Select DataMonth From @vDataMonth


	----Master value
	--select DataYear,  refSupplierID, 
	--	(Select SupplierName from SupplierMas Where SupplierId = refSupplierID) as SupplierName,
	--	Sum(TotalRebate) TotalRebate
	--from ProcessData
	--Where DataYear In (2010)
	--and DataMonth In (02,03)
	--and refSupplierID In (3,4)
	--Group By refSupplierID, DataYear

	----Detail value
	--select DataYear, DataMonth, refSupplierID, 
	--		(Select SupplierName from SupplierMas Where SupplierId = refSupplierID) as SupplierName,
	--		Sum(TotalRebate) TotalRebate
	--from ProcessData
	--Where DataYear In (2010)
	--and DataMonth In (02,03)
	--and refSupplierID In (3,4)
	--Group By refSupplierID, DataYear,DataMonth

	
	--exec sp_executesql @sql
	

	----Demo Query
	--	Select * From (select Distinct refPharmacyID, 
	--		(Select PharmacyName From PharmacyMas b Where b.PharmacyID = a.refPharmacyID) PharmacyName
	--from ProcessData a
	--right join PharmacyMas b
	--On a.refPharmacyID = b.PharmacyID
	--and refSupplierID = 4
	--and DataYear = '2010'
	--Order By a.refPharmacyID) X,

	--(select (a.Generic + a.Drinks + a.Electrical + a.Mobility) [Feb-10]
	--from ProcessData a 
	--right join PharmacyMas b
	--On a.refPharmacyID = b.PharmacyID
	--and refSupplierID = 4
	--and DataYear = '2010'
	--and DataMonth = '02'
	--Order By a.refPharmacyID) Z,

	--(select (a.Generic + a.Drinks + a.Electrical + a.Mobility) [Mar-10]
	--from ProcessData a
	--right join PharmacyMas b
	--On a.refPharmacyID = b.PharmacyID 
	--and a.refSupplierID = 4
	--and a.DataYear = '2010'
	--and a.DataMonth = '03'
	--Order By a.refPharmacyID) Y

	Begin Try
			Create Table #ProfitCompare (DataYear nvarchar(5),PharmacyId int,PharmacyName nvarchar(70),Apr numeric(18,2),
				May numeric(18,2),Jun numeric(18,2),Jul numeric(18,2),Aug numeric(18,2),Sep numeric(18,2),
				Oct numeric(18,2),Nov numeric(18,2),Dec numeric(18,2),Jan numeric(18,2),Feb numeric(18,2),
				Mar numeric(18,2),AprRb numeric(18,2),
				MayRb numeric(18,2),JunRb numeric(18,2),JulRb numeric(18,2),AugRb numeric(18,2),SepRb numeric(18,2),
				OctRb numeric(18,2),NovRb numeric(18,2),DecRb numeric(18,2),JanRb numeric(18,2),FebRb numeric(18,2),
				MarRb numeric(18,2));

			Declare @Columns nvarchar(Max) = NULL;

			Declare Cur_ColumnList Cursor For
			Select Item From SplitString (@pListofColumn ,',')

			Declare @Item nvarchar(1000);
			
			
			Open Cur_ColumnList
				Fetch Next From Cur_ColumnList Into @Item 

				While @@FETCH_STATUS = 0 
				Begin
					if(@Columns Is Not NUll)
						Set @Columns = @Columns + ' + ';
					Else
						Set @Columns = '';
					
					Set @Columns = @Columns + 'IsNULL('+@Item +',0)';

					Fetch Next From Cur_ColumnList Into @Item 
				End

			Close Cur_ColumnList
			Deallocate Cur_ColumnList

			--Deallocate Cur_Year;
			Declare Cur_Year Cursor For
			Select Item From SplitString (@pDataYear,',')

			Declare Cur_Month Cursor For
			Select DataMonth, MonName From @vDataMonth

			Declare @vMonth nvarchar(5), @MonName nvarchar(5), @Year nvarchar(5);

			Open Cur_Year
			Fetch Next From Cur_Year Into @Year 
				
				While @@FETCH_STATUS = 0
				Begin
					Insert Into #ProfitCompare (DataYear,PharmacyId, PharmacyName) 
					Select @Year, PharmacyID, PharmacyName From PharmacyMas;

					--Print @Year;

					Open Cur_Month
					Fetch Next From Cur_Month Into @vMonth,@MonName 

						While @@FETCH_STATUS = 0 
						Begin
					
							Set @sql = '
								Merge #ProfitCompare T
								Using (Select b.PharmacyID, ('+@Columns+') Total, TotalRebate
									from ProcessData a 
									right join PharmacyMas b
									On a.refPharmacyID = b.PharmacyID
									and refSupplierID = '+ Cast(@pSupplierId as varchar) +'
									and DataYear = '+@Year+'
									and DataMonth = '+@vMonth+') S
								On T.PharmacyId = S.PharmacyID And T.DataYear = '+@Year+'
								When Matched Then 
										Update Set T.'+@MonName+'  =  S.Total,
													T.'+@MonName+'Rb  =  S.TotalRebate;'
								
								exec sp_executesql @sql;

								--Print @vMonth;

							Fetch Next From Cur_Month Into @vMonth, @MonName ;
						End
					Close Cur_Month;

					Fetch Next From Cur_Year Into @Year;
				End
			Close Cur_Year;
			Deallocate Cur_Month;
			

			--Select * from #ProfitCompare;

			Declare @RptQry nvarchar(Max) = '';
			Declare @QryColumn nvarchar(Max) = '';
			Declare @TotalSpandCol nvarchar(Max) = '';
			Declare @PrevYear nvarchar(5) = '';

			Declare @NoofYears int = 0;
			Select @NoofYears = Count(*) From SplitString (@pDataYear, ',');
			
			If(@NoofYears = 1)
			Begin
				If(Upper(@pRptType) = 'QUATER')
				Begin
					Set @RptQry  = 'a.Apr, a.May,a.Jun, (IsNUll(a.Apr,0) +  IsNUll(a.May ,0) + IsNUll(a.Jun,0) ) Total1Q,
						(IsNUll(a.AprRb,0) +  IsNUll(a.MayRb ,0) + IsNUll(a.JunRb,0) ) Total1Rebate,
						a.Jul, a.Aug,a.Sep, (IsNUll(a.Jul ,0) + IsNUll(a.Aug,0)  +IsNUll(a.Sep,0) ) Total2Q,
						(IsNUll(a.JulRb ,0) + IsNUll(a.AugRb,0)  +IsNUll(a.SepRb,0))  Total2Rebate,
						a.Oct, a.Nov,a.[Dec], (IsNUll(a.Oct,0) + IsNUll(a.Nov,0)  +IsNUll(a.[Dec],0) ) Total3Q,
						(IsNUll(a.OctRb,0) + IsNUll(a.NovRb,0)  +IsNUll(a.DecRb,0) ) Total3Rebate,
						a.Jan, a.Feb,a.Mar, (IsNUll(a.Jan,0)  + IsNUll(a.Feb,0)  +IsNUll(a.Mar,0) ) Total4Q,
						a.Jan, a.FebRb,a.Mar, (IsNUll(a.JanRb,0)  + IsNUll(a.FebRb,0)  +IsNUll(a.MarRb,0) ) Total4Rebate,
						((IsNUll(a.Jul ,0) + IsNUll(a.Aug,0)  +IsNUll(a.Sep,0)) - (IsNUll(a.Apr,0) +  IsNUll(a.May ,0) + IsNUll(a.Jun,0))) TotalSpand1,
						((IsNUll(a.JulRb ,0) + IsNUll(a.AugRb,0)  +IsNUll(a.SepRb,0)) - (IsNUll(a.AprRb,0) +  IsNUll(a.MayRb ,0) + IsNUll(a.JunRb,0))) TotalRebate1,
						((IsNUll(a.Oct,0) + IsNUll(a.Nov,0)  +IsNUll(a.[Dec],0) ) -(IsNUll(a.Jul ,0) + IsNUll(a.Aug,0)  +IsNUll(a.Sep,0))) TotalSpand2,
						((IsNUll(a.OctRb,0) + IsNUll(a.NovRb,0)  +IsNUll(a.[DecRb],0) ) -(IsNUll(a.JulRb ,0) + IsNUll(a.AugRb,0)  +IsNUll(a.SepRb,0))) TotalRebate2,
						((IsNUll(a.Jan,0)  + IsNUll(a.Feb,0)  +IsNUll(a.Mar,0)) - (IsNUll(a.Oct,0) + IsNUll(a.Nov,0)  +IsNUll(a.[Dec],0))) TotalSpand3,
						((IsNUll(a.JanRb,0)  + IsNUll(a.FebRb,0)  +IsNUll(a.MarRb,0)) - (IsNUll(a.OctRb,0) + IsNUll(a.NovRb,0)  +IsNUll(a.[DecRb],0))) TotalRebate3'
				End
				Else If(Upper(@pRptType) = 'HALF YEARLY')
				Begin
					Set @RptQry   = 'a.Apr, a.May,a.Jun, a.Jul, a.Aug,a.Sep, 
									( IsNUll(a.Apr,0) +  IsNUll(a.May ,0) + IsNUll(a.Jun,0) +IsNUll(a.Jul ,0) + IsNUll(a.Aug,0)  +IsNUll(a.Sep,0) ) HalfTotal1,
									( IsNUll(a.AprRb,0) +  IsNUll(a.MayRb ,0) + IsNUll(a.JunRb,0) +IsNUll(a.JulRb ,0) + IsNUll(a.AugRb,0)  +IsNUll(a.SepRb,0) ) HalfTotal1Rebate,
									 a.Oct, a.Nov,a.[Dec], a.Jan, a.Feb,a.Mar, 
									(IsNUll(a.Oct,0) + IsNUll(a.Nov,0)  +IsNUll(a.[Dec],0) + IsNUll(a.Jan,0)  + IsNUll(a.Feb,0)  +IsNUll(a.Mar,0)) HalfTotal2,
									(IsNUll(a.OctRb,0) + IsNUll(a.NovRb,0)  +IsNUll(a.[DecRb],0) + IsNUll(a.JanRb,0)  + IsNUll(a.FebRb,0)  +IsNUll(a.MarRb,0)) HalfTotal2Rebate,
						((IsNUll(a.Oct,0) + IsNUll(a.Nov,0)  +IsNUll(a.[Dec],0) + IsNUll(a.Jan,0)  + IsNUll(a.Feb,0)  +IsNUll(a.Mar,0)) - ( IsNUll(a.Apr,0) +  IsNUll(a.May ,0) + IsNUll(a.Jun,0) +IsNUll(a.Jul ,0) + IsNUll(a.Aug,0)  +IsNUll(a.Sep,0) )) TotalSpand1,
						((IsNUll(a.OctRb,0) + IsNUll(a.NovRb,0)  +IsNUll(a.[DecRb],0) + IsNUll(a.JanRb,0)  + IsNUll(a.FebRb,0)  +IsNUll(a.MarRb,0)) - ( IsNUll(a.AprRb,0) +  IsNUll(a.MayRb ,0) + IsNUll(a.JunRb,0) +IsNUll(a.JulRb ,0) + IsNUll(a.AugRb,0)  +IsNUll(a.SepRb,0) )) TotalRebate1'
							
				End
			End

			If(@NoofYears > 1)
			Begin
				Declare @Mon1 nvarchar(5) ='',@Mon2 nvarchar(5) ='',@Mon3 nvarchar(5) ='',@Mon4 nvarchar(5) ='',@Mon5 nvarchar(5) ='',@Mon6 nvarchar(5) ='';
				Declare @vCount int = 0;
				If(Upper(@pRptType) = 'MULTIYEAR QUATER')
				Begin
					
					if(@pPeriodNo Is Not NULL)
					Begin
						Set @pPeriodNo = @pPeriodNo * 3 +1;
						if(@pPeriodNo > 12)
							Set @pPeriodNo = 1;
						if(Len(@pPeriodNo) = 1)
						Begin
							Select @Mon1 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon2 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon3 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
						End
						Else
						Begin
							Select @Mon1 = MonName From @vDataMonth Where DataMonth = Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon2 = MonName From @vDataMonth Where DataMonth = Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon3 = MonName From @vDataMonth Where DataMonth = Cast(@pPeriodNo as varchar(5))
						End
					End
					
					Print @Mon1 + ' ' +@Mon2 + ' ' + @Mon3;

					Open Cur_Year
					Fetch Next From Cur_Year Into @Year
						
						While @@FETCH_STATUS = 0
						Begin
							Set @vCount = @vCount + 1;
							if(@RptQry != '')
							Begin
								--Set @RptQry = @RptQry + ',';
								Set @QryColumn = @QryColumn + ',';
								Set @TotalSpandCol  = @TotalSpandCol + ',';
							End
							Set @RptQry   =  @RptQry + ' inner join (Select b.PharmacyId ,b.'+@Mon1+' As ' +@Mon1 + @Year+ ' ,b.'+@Mon2+' as ' + @Mon2+@Year+', b.'+@Mon3+' as '+ @Mon3+ @Year+',( IsNUll(b.'+@Mon1+',0) +  IsNUll(b.'+@Mon2+',0) + IsNUll(b.'+@Mon3+',0)) QuaterTotal'+ @Year+' 
												,( IsNUll(b.'+@Mon1+'Rb,0) +  IsNUll(b.'+@Mon2+'Rb,0) + IsNUll(b.'+@Mon3+'Rb,0)) QuaterTotalRb'+ @Year+' 
											  From #ProfitCompare b Where b.DataYear = '+@Year +') Data' + @Year+ ' on a.PharmacyId = Data' + @Year+ '.PharmacyId'
											
							Set @QryColumn = @QryColumn + @Mon1 + @Year+ ',' +@Mon2+@Year+',' + @Mon3 + @Year+', QuaterTotal'+ @Year +' , QuaterTotalRb'+ @Year  
							if(@PrevYear != '')
							Begin
								Set @TotalSpandCol = @TotalSpandCol + '(Cast(QuaterTotal'+ @PrevYear + ' as numeric(18,2)) + 
								Cast(QuaterTotal'+ @Year+' as numeric(18,2))) TotalSpand'+Cast(@vCount as varchar)+',
								(Cast(QuaterTotalRb'+ @PrevYear + ' as numeric(18,2)) + 
								Cast(QuaterTotalRb'+ @Year+' as numeric(18,2))) TotalRebate'+Cast(@vCount as varchar)
							End
							Set @PrevYear = @Year;
							--Print @Year;
							--Print  @RptQry;
							--Print @RptQry;
							Fetch Next From Cur_Year Into @Year
						End
					Close Cur_Year;
					
				End

				Else If(Upper(@pRptType) = 'MULTIYEAR HALFMONTHS')
				Begin
					if(@pPeriodNo Is Not NULL)
					Begin
						Set @pPeriodNo = (@pPeriodNo - 1) * 6 + 1 ;
						if(@pPeriodNo > 12)
							Set @pPeriodNo = 1;
						if(@pPeriodNo = 1)
						Begin
							Select @Mon1 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon2 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon3 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon4 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon5 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon6 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
						End
						Else
						Begin
							Select @Mon1 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon2 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon3 = MonName From @vDataMonth Where DataMonth = '0'+Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon4 = MonName From @vDataMonth Where DataMonth = Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon5 = MonName From @vDataMonth Where DataMonth = Cast(@pPeriodNo as varchar(5))
							Set @pPeriodNo = @pPeriodNo +1;
							Select @Mon6 = MonName From @vDataMonth Where DataMonth = Cast(@pPeriodNo as varchar(5))
						End
					End
					
					--Print @Mon1 + ' ' +@Mon2 + ' ' + @Mon3;

					Open Cur_Year
					Fetch Next From Cur_Year Into @Year
						
						While @@FETCH_STATUS = 0
						Begin
							Set @vCount = @vCount + 1;
							if(@RptQry != '')
							Begin
								--Set @RptQry = @RptQry + ',';
								Set @QryColumn = @QryColumn + ',';
								Set @TotalSpandCol  = @TotalSpandCol + ',';
							End
							Set @RptQry   =  @RptQry + ' inner join (Select b.PharmacyId ,b.'+@Mon1+' As ' +@Mon1 + @Year+ ' ,b.'+@Mon2+' as ' + @Mon2+@Year+', b.'+@Mon3+' as '+ @Mon3+ @Year+'
														,b.'+@Mon4+' As ' +@Mon4 + @Year+ ' ,b.'+@Mon5+' as ' + @Mon5+@Year+', b.'+@Mon6+' as '+ @Mon6+ @Year+'
														,( IsNUll(b.'+@Mon1+',0) +  IsNUll(b.'+@Mon2+',0) + IsNUll(b.'+@Mon3+',0)+ IsNUll(b.'+@Mon4+',0) +  IsNUll(b.'+@Mon5+',0) + IsNUll(b.'+@Mon6+',0)) QuaterTotal'+ @Year+' 
														,( IsNUll(b.'+@Mon1+'Rb,0) +  IsNUll(b.'+@Mon2+'Rb,0) + IsNUll(b.'+@Mon3+'Rb,0)+ IsNUll(b.'+@Mon4+'Rb,0) +  IsNUll(b.'+@Mon5+'Rb,0) + IsNUll(b.'+@Mon6+'Rb,0)) QuaterTotalRb'+ @Year+' 
											  From #ProfitCompare b Where b.DataYear = '+@Year +') Data' + @Year+ ' on a.PharmacyId = Data' + @Year+ '.PharmacyId'
											
							Set @QryColumn = @QryColumn + @Mon1 + @Year+ ',' +@Mon2+@Year+',' + @Mon3 + @Year+','+ @Mon4 + @Year+ ',' +@Mon5+@Year+',' + @Mon6 + @Year+', QuaterTotal'+ @Year +' , QuaterTotalRb'+ @Year  
							if(@PrevYear != '')
							Begin
								Set @TotalSpandCol = @TotalSpandCol + '(Cast(QuaterTotal'+ @PrevYear + ' as numeric(18,2)) + 
								Cast(QuaterTotal'+ @Year+' as numeric(18,2))) TotalSpand'+Cast(@vCount as varchar)+', (Cast(QuaterTotalRb'+ @PrevYear + ' as numeric(18,2)) + 
								Cast(QuaterTotalRb'+ @Year+' as numeric(18,2))) TotalRebate'+Cast(@vCount as varchar)
							End
							Set @PrevYear = @Year;
							--Print @Year;
							--Print  @RptQry;
							--Print @RptQry;
							Fetch Next From Cur_Year Into @Year
						End
					Close Cur_Year;
					
				End
			End

			Deallocate Cur_Year;

			Declare @GetTable nvarchar(max) = '';
			If(Upper(@pRptType) = 'MULTIYEAR QUATER' or Upper(@pRptType) = 'MULTIYEAR HALFMONTHS')
			Begin
				Set @GetTable = 'Select Distinct(a.PharmacyId), a.PharmacyName , ' + @QryColumn +' ' + @TotalSpandCol +'
									From #ProfitCompare a 
												'+ @RptQry  +'
									Order By a.PharmacyName'

									--print @GetTable 
			End
			Else
			Begin
				Set @GetTable = 'Select Distinct(a.PharmacyId), a.PharmacyName ,
												'+ @RptQry  +'
									From #ProfitCompare a
									Order By a.PharmacyName'
									--print @GetTable 
			End
			

			

			exec sp_executesql @GetTable;
			--Select a.*, (IsNUll(Apr,0) + IsNUll(May ,0) +IsNUll(Jun ,0) + IsNUll(Jul ,0) + IsNUll(Aug ,0) + IsNUll(Sep,0) + IsNUll(Oct ,0) + IsNUll(Nov ,0) + IsNUll([Dec] ,0) + IsNUll(Jan ,0) + IsNUll(Feb ,0) + IsNUll(Mar,0) ) TotalValue From #ProfitCompare a

	End Try
	Begin Catch
		DECLARE 
			@ErrorMessage NVARCHAR(4000),
			@ErrorSeverity INT,
			@ErrorState INT;
		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();
		RAISERROR (
	        @ErrorMessage,
		    @ErrorSeverity,
			@ErrorState    
			);
	End Catch
END

GO

