USE [3rdEyE]
GO
/****** Object:  StoredProcedure [dbo].[CleanDeviceData]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CleanDeviceData]
AS
BEGIN

DECLARE @CleanDealerDataBefore datetime = DATEADD(HOUR, -8, GETDATE());
update Dealer set FK_Vehicle = null where FK_Vehicle is not null AND AssignTime < @CleanDealerDataBefore;

DECLARE @CleanDeviceDataBefore datetime = DATEADD(MINUTE, -60, GETDATE());
DELETE from DeviceData WHERE UpdateTime < @CleanDeviceDataBefore select ( CONVERT(varchar(20), @@ROWCOUNT) + ' data deleted before ' + CONVERT(varchar(30), @CleanDeviceDataBefore) ) as 'RESPONSE';

END




GO
/****** Object:  StoredProcedure [dbo].[DataBaseBackup]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DataBaseBackup]
AS
BEGIN
	SET NOCOUNT ON;
	declare @path nvarchar(500)
	declare @file nvarchar(30)
	--set @file=CONVERT(VARCHAR(20), GETDATE(), 112)

	set @path= 'D:\DB Backups\'+'3rdEyE '+ FORMAT(getdate(),'yyyy-MM-dd HH-mm') +'.bak';
	backup database [3rdEyE] to disk=@path;
	
END

--select FORMAT(getdate(),'yyyy-MM-dd hh-mm tt');
--select FORMAT(getdate(),'yyyy-MM-dd HH-mm');


GO
/****** Object:  StoredProcedure [dbo].[DeleteCoreData]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCoreData]     
AS   
	delete from Company;
DBCC CHECKIDENT ('[Company]', RESEED, 0);

delete from FinancingCompany;
DBCC CHECKIDENT ('[FinancingCompany]', RESEED, 0);

delete from GroupOfCompany;
DBCC CHECKIDENT ('[GroupOfCompany]', RESEED, 0);

delete from AppUser;
DBCC CHECKIDENT ('[AppUser]', RESEED, 0);

delete from Depo;
DBCC CHECKIDENT ('[Depo]', RESEED, 0);



GO
/****** Object:  StoredProcedure [dbo].[GenerateReport_GetVehicleHaltTime]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[GenerateReport_GetVehicleHaltTime](
@FK_Vehicle varchar(50),
@startingDate datetime,
@endingDate datetime
)
AS
BEGIN
    -- Halt Report
	DECLARE @MininumMinuteDealy BIGINT=1;

	DECLARE @HaltTime BIGINT=0;
	DECLARE @TotalHaltTime BIGINT=0;
	DECLARE @HaltCount BIGINT=0;
	DECLARE @first_id BIGINT = 0;
	Declare @first_UpdateTime DATETIME;
	Declare @min_ID BIGINT;
	DECLARE @max_ID BIGINT;
	DECLARE @standingState_id BIGINT=0;
	DECLARE @runningState_id BIGINT=0;
	DECLARE @standingState_DateTime datetime;
	DECLARE @runningState_DateTime datetime;
	DECLARE @_Latitude [varchar](300);
	DECLARE @_Longitude [varchar](300);
	DECLARE @_EngineStatus [varchar](300);
	DECLARE @_Speed [varchar](300);
	
	
	--first_id calculation and insert
	SELECT TOP 1 @first_id = PK_RowData, @first_UpdateTime = UpdateTime
	FROM Report_VehicleHaltReadyReport_Helper;
	IF(@first_id != 0)
	BEGIN
		SET @HaltTime = DATEDIFF(mi, @StartingDate, @first_UpdateTime);
		IF(@HaltTime >= @MininumMinuteDealy)
		BEGIN
			SET @TotalHaltTime = @TotalHaltTime + @HaltTime;
			IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01'))
			BEGIN
				INSERT INTO [3rdEyE_TrackingDataBase_2022_11].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																				VALUES( @FK_Vehicle,'data_initial_gap', '', '', @StartingDate, @first_UpdateTime, '', '', '', @HaltTime); 	
			END
			ELSE IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01'))
			BEGIN
				INSERT INTO [3rdEyE_TrackingDataBase_2022_12].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																				VALUES(@FK_Vehicle, 'data_initial_gap', '', '', @StartingDate, @first_UpdateTime, '', '', '', @HaltTime); 	
			END
			SET @HaltCount = @HaltCount +1;
		END
	END
		
	--min_DateTime calculation
	SET @min_ID = @first_id;
	--SELECT TOP 1 @min_ID = PK_RowData FROM Report_VehicleHaltReadyReport_Helper 
	--WHERE USER_KEY = @USER_KEY
	--AND EngineStatus = '0'
	--AND Speed = 0;
	
	--max_Datetime calculation
	SET @max_ID = 0;
	SELECT TOP 1 @max_ID = PK_RowData FROM Report_VehicleHaltReadyReport_Helper 
	ORDER BY PK_RowData DESC;
		
	--INSERT 
	WHILE(@min_ID != 0 AND @max_ID != 0 AND @min_ID < @max_ID)
	BEGIN

		--STAT 0
		SET @standingState_id = 0;
		SET @standingState_DateTime = '2000-01-01';
		SELECT TOP 1 
		@standingState_id = PK_RowData, 
		@standingState_DateTime = UpdateTime,
		@_Latitude= Latitude,
		@_Longitude= Longitude,
		@_EngineStatus= EngineStatus,
		@_Speed= Speed
		FROM Report_VehicleHaltReadyReport_Helper 
		WHERE PK_RowData >= @min_ID AND EngineStatus = '0' AND Speed = 0;

		IF(@standingState_id != 0)
		BEGIN
			SET @min_ID = @standingState_id + 1;
		END
		ELSE
		BEGIN
			BREAK;
		END

		--STAT 1
		SET @runningState_id = 0;
		SET @runningState_DateTime = '2000-01-01';
		SELECT TOP 1 
		@runningState_id = PK_RowData, 
		@runningState_DateTime = UpdateTime
		FROM Report_VehicleHaltReadyReport_Helper 
		WHERE PK_RoWData >= @min_ID AND PK_RowData <= @max_ID AND (EngineStatus = '1' AND Speed > 0);

		IF(@runningState_id != 0)
		BEGIN
			SET @min_ID = @runningState_id + 1;

			SET @HaltTime = DATEDIFF(mi, @standingState_DateTime, @runningState_DateTime);
			IF(@HaltTime >= @MininumMinuteDealy)
			BEGIN
				SET @TotalHaltTime = @TotalHaltTime + @HaltTime;
				IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01'))
				BEGIN
					INSERT INTO [3rdEyE_TrackingDataBase_2022_11].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																						VALUES(@FK_Vehicle,'data', convert(varchar(50),@standingState_id), convert(varchar(50),@runningState_id), @standingState_DateTime, @runningState_DateTime, @_Latitude, @_Longitude,@_EngineStatus,  @HaltTime); 
				END
				ELSE IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2022-12-01'))
				BEGIN
					INSERT INTO [3rdEyE_TrackingDataBase_2022_12].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																						VALUES(@FK_Vehicle,'data', convert(varchar(50),@standingState_id), convert(varchar(50),@runningState_id), @standingState_DateTime, @runningState_DateTime, @_Latitude, @_Longitude,@_EngineStatus,  @HaltTime); 
				END
				SET @HaltCount = @HaltCount +1;
			END
		END
		ELSE
		BEGIN
			--# SET gap time to last update time/ ending time
			SET @runningState_DateTime = @EndingDate;----Uncomment this to calculate untill last momment

			SET @HaltTime = DATEDIFF(mi, @standingState_DateTime, @runningState_DateTime); 
			
			IF(@HaltTime >= @MininumMinuteDealy)
			BEGIN 
				SET @TotalHaltTime = @TotalHaltTime + @HaltTime;
				IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01'))
				BEGIN
					INSERT INTO [3rdEyE_TrackingDataBase_2022_11].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																				VALUES(@FK_Vehicle,'data_finishing_gap', convert(varchar(50),@standingState_id), '', @standingState_DateTime, @runningState_DateTime, @_Latitude, @_Longitude,@_EngineStatus, @HaltTime); 
				END
				ELSE IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2022-12-01'))
				BEGIN
					INSERT INTO [3rdEyE_TrackingDataBase_2022_12].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																				VALUES(@FK_Vehicle,'data_finishing_gap', convert(varchar(50),@standingState_id), '', @standingState_DateTime, @runningState_DateTime, @_Latitude, @_Longitude,@_EngineStatus, @HaltTime); 
				END
				SET @HaltCount = @HaltCount +1;
			END
			BREAK;
		END
	END
	


	--Consolidated
	Declare @LastUpdate varchar(50);
	Declare @LastLatitude varchar(300);
	Declare @LastLongitude varchar(300);

	Declare @TotalDistance float = 0;
	Declare @MaximumSpeed float = 0;
	Declare @AverageSpeed float = 0;
	Declare @AverageHaltTime float = 0;
	Declare @MaximumHaltTime int = 0;
	SELECT
	@TotalDistance = SUM(Distance),
	@MaximumSpeed = MAX(Speed),
	@AverageSpeed = ROUND(AVG(Report_VehicleHaltReadyReport_Helper.Speed),2)
	FROM Report_VehicleHaltReadyReport_Helper
	WHERE (EngineStatus = '1' AND Speed > 0);
	
	--IF(@TotalDistance is null)
	--BEGIN
	--	SET @TotalDistance = 0;
	--END

	--IF(@MaximumSpeed is null)
	--BEGIN
	--	SET @MaximumSpeed = 0;
	--END

	--IF(@MaximumSpeed is null)
	--BEGIN
	--	SET @MaximumSpeed = 0;
	--END


	--LastUpdate--LastLatitude--LastLongitude
	SELECT TOP 1 
	@LastLatitude = Latitude
	,@LastLongitude = Longitude
	,@LastUpdate = UpdateTime
	FROM Report_VehicleHaltReadyReport_Helper
	WHERE UpdateTime > @StarTingDate
	AND UpdateTime < @EndingDate
	--ORDER BY PK_RowData DESC;
	ORDER BY UpdateTime DESC;

	--IF(@LastLatitude is null)
	--BEGIN
	--	SET @LastLatitude = '';
	--END

	--IF(@LastLongitude is null)
	--BEGIN
	--	SET @LastLongitude = '';
	--END

	--IF(@LastUpdate is null)
	--BEGIN
	--	SET @LastUpdate = '';
	--END
	
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01'))
	BEGIN
		INSERT INTO [3rdEyE_TrackingDataBase_2022_11].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,StartTime,EndTime,TotalDistance,MaximumSpeed,AverageSpeed,LastUpdate,Latitude,Longitude,HaltTime,HaltCount) 
																	VALUES(@FK_Vehicle,'data_consolidated', @startingDate, @startingDate, @TotalDistance, @MaximumSpeed, @AverageSpeed, @LastUpdate,@LastLatitude,@LastLongitude,@TotalHaltTime,@HaltCount); 
	END
	ELSE IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2022-12-01'))
	BEGIN
		INSERT INTO [3rdEyE_TrackingDataBase_2022_12].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,StartTime,EndTime,TotalDistance,MaximumSpeed,AverageSpeed,LastUpdate,Latitude,Longitude,HaltTime,HaltCount) 
																	VALUES(@FK_Vehicle,'data_consolidated', @startingDate, @startingDate, @TotalDistance, @MaximumSpeed, @AverageSpeed, @LastUpdate,@LastLatitude,@LastLongitude,@TotalHaltTime,@HaltCount); 
	END
END
GO
/****** Object:  StoredProcedure [dbo].[GenerateReport_GetVehicleHaltTime_Helper]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GenerateReport_GetVehicleHaltTime_Helper](
@FK_Vehicle varchar(50),
@startingDate datetime,
@endingDate datetime
)
AS
BEGIN
--###################################### H E A D ##########################################
	delete from Report_VehicleHaltReadyReport_Helper;
	
	Insert into Report_VehicleHaltReadyReport_Helper(FK_Vehicle ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	Select @FK_Vehicle ,Report_VehicleHaltReadyReport_Helper_Helper.Latitude ,Report_VehicleHaltReadyReport_Helper_Helper.Longitude ,Report_VehicleHaltReadyReport_Helper_Helper.Altitude ,Report_VehicleHaltReadyReport_Helper_Helper.EngineStatus ,Report_VehicleHaltReadyReport_Helper_Helper.Course ,Report_VehicleHaltReadyReport_Helper_Helper.Temperature ,Report_VehicleHaltReadyReport_Helper_Helper.Fuel ,Report_VehicleHaltReadyReport_Helper_Helper.Speed ,Report_VehicleHaltReadyReport_Helper_Helper.Distance ,Report_VehicleHaltReadyReport_Helper_Helper.UpdateTime ,Report_VehicleHaltReadyReport_Helper_Helper.ServerTime
	FROM Report_VehicleHaltReadyReport_Helper_Helper 
	WHERE Report_VehicleHaltReadyReport_Helper_Helper.FK_Vehicle = @FK_Vehicle AND Report_VehicleHaltReadyReport_Helper_Helper.UpdateTime> @startingDate AND Report_VehicleHaltReadyReport_Helper_Helper.UpdateTime < @endingDate;
END
GO
/****** Object:  StoredProcedure [dbo].[GenerateReport_GetVehicleHaltTime_Helper_Helper]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GenerateReport_GetVehicleHaltTime_Helper_Helper](
@FK_Vehicle varchar(50),
@StartingDate datetime
)
AS
BEGIN
--###################################### H E A D ##########################################
	delete from Report_VehicleHaltReadyReport_Helper_Helper;
	
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01'))
	BEGIN
		Insert into Report_VehicleHaltReadyReport_Helper_Helper(FK_Vehicle ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @FK_Vehicle ,DeviceData.Latitude ,DeviceData.Longitude ,DeviceData.Altitude ,DeviceData.EngineStatus ,DeviceData.Course ,DeviceData.Temperature ,DeviceData.Fuel ,DeviceData.Speed ,DeviceData.Distance ,DeviceData.UpdateTime ,DeviceData.ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData
		WHERE DeviceData.FK_Vehicle = @FK_Vehicle
		Order by DeviceData.UpdateTime;
	END
	ElSE IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01'))
	BEGIN
		Insert into Report_VehicleHaltReadyReport_Helper_Helper(FK_Vehicle ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @FK_Vehicle ,DeviceData.Latitude ,DeviceData.Longitude ,DeviceData.Altitude ,DeviceData.EngineStatus ,DeviceData.Course ,DeviceData.Temperature ,DeviceData.Fuel ,DeviceData.Speed ,DeviceData.Distance ,DeviceData.UpdateTime ,DeviceData.ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData
		WHERE DeviceData.FK_Vehicle = @FK_Vehicle
		Order by DeviceData.UpdateTime;
	END
	

END
GO
/****** Object:  StoredProcedure [dbo].[Move_RequisitionTrip]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Move_RequisitionTrip]    
AS 
BEGIN

DROP TABLE IF EXISTS [dbo].[#TempRequisitionTrip_Mover];
create table #TempRequisitionTrip_Mover
(
    ID bigint
);

Insert Into #TempRequisitionTrip_Mover
SELECT TOP (100) 
	[RequisitionTrip].[PK_RequisitionTrip]
	FROM [RequisitionTrip] 
	where [RequisitionTrip].[AssingedAt] < DATEADD(day, -7, GETDATE())
	AND [RequisitionTrip].IsParent is null 
	AND [RequisitionTrip].FK_RequisitionTrip_Parent is null
	order by PK_RequisitionTrip;

INSERT INTO [dbo].[RequisitionTrip_Finished]
		   ([PK_RequisitionTrip_Finished]
           ,[IsDeleted]
           ,[TrackingID]
           ,[FK_Requisition]
           ,[WantedCount]
           ,[FK_Vehicle]
           ,[OWN_MHT_DHT]
           ,[Driver_Staff_ID]
           ,[Driver_Name]
           ,[Driver_ContactNumber]
           ,[TotalAmount]
           ,[CommissionAmount]
           ,[AdvanceAmount]
           ,[PendingAmount]
           ,[FK_TransportAgency]
           ,[FK_AppUser_Assigner]
           ,[FinalWantedAtDateTime]
           ,[AssingedAt]
           ,[TentativeFinishingDateTime]
           ,[FK_AppUser_Canceller]
           ,[CancelledAt]
           ,[StatusText]
           ,[StartedAt]
           ,[FK_AppUser_Start]
           ,[FK_LocationGate_Start]
           ,[StartAutoOrManaul]
           ,[FinishedAt]
           ,[FK_AppUser_Finish]
           ,[FK_LocationGate_Finish]
           ,[FinishAutoOrManaul]
           ,[IsParent]
           ,[FK_RequisitionTrip_Finished_Parent]
           ,[IsForwarded]
           ,[FK_Location_ForwardedTo]
           ,[PRG_Type_ForwaredTo]
           ,[ForwardedAt]
           ,[FK_AppUser_ForwardedBy]
           ,[OracleDB_IsPushed]
           ,[OracleDB_PushedAt]
           ,[OracleDB_IsPulled]
           ,[OracleDB_PulledAt]
           ,[OracleDB_GPNumber]
           ,[OracleDB_GPNumberUpdatedAt]
           ,[FK_ParkingInOut_Before]
           ,[FK_ParkingInOut_After]
           ,[ManualParkingEntryTime]
           ,[AssigningNote]
           ,[IsGatePassUsed]
           ,[FK_VehicleInOutManual_Before]
           ,[FK_VehicleInOutManual_After])
select
			[PK_RequisitionTrip]
           ,[IsDeleted]
           ,[TrackingID]
           ,[FK_Requisition]
           ,[WantedCount]
           ,[FK_Vehicle]
           ,[OWN_MHT_DHT]
           ,[Driver_Staff_ID]
           ,[Driver_Name]
           ,[Driver_ContactNumber]
           ,[TotalAmount]
           ,[CommissionAmount]
           ,[AdvanceAmount]
           ,[PendingAmount]
           ,[FK_TransportAgency]
           ,[FK_AppUser_Assigner]
           ,[FinalWantedAtDateTime]
           ,[AssingedAt]
           ,[TentativeFinishingDateTime]
           ,[FK_AppUser_Canceller]
           ,[CancelledAt]
           ,[StatusText]
           ,[StartedAt]
           ,[FK_AppUser_Start]
           ,[FK_LocationGate_Start]
           ,[StartAutoOrManaul]
           ,[FinishedAt]
           ,[FK_AppUser_Finish]
           ,[FK_LocationGate_Finish]
           ,[FinishAutoOrManaul]
           ,[IsParent]
           ,[FK_RequisitionTrip_Parent]
           ,[IsForwarded]
           ,[FK_Location_ForwardedTo]
           ,[PRG_Type_ForwaredTo]
           ,[ForwardedAt]
           ,[FK_AppUser_ForwardedBy]
           ,[OracleDB_IsPushed]
           ,[OracleDB_PushedAt]
           ,[OracleDB_IsPulled]
           ,[OracleDB_PulledAt]
           ,[OracleDB_GPNumber]
           ,[OracleDB_GPNumberUpdatedAt]
           ,[FK_ParkingInOut_Before]
           ,[FK_ParkingInOut_After]
           ,[ManualParkingEntryTime]
           ,[AssigningNote]
           ,[IsGatePassUsed]
           ,[FK_VehicleInOutManual_Before]
           ,[FK_VehicleInOutManual_After]
	from [RequisitionTrip] 
	where [RequisitionTrip].[PK_RequisitionTrip] in (select ID from #TempRequisitionTrip_Mover);

delete from [RequisitionTrip] 
where [RequisitionTrip].[PK_RequisitionTrip] in (select ID from #TempRequisitionTrip_Mover);

END

GO
/****** Object:  StoredProcedure [dbo].[MoveDeviceData]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MoveDeviceData]
AS
BEGIN

--#Create New Database
--GenerateReport_GetVehicleHaltTime
--GenerateReport_GetVehicleHaltTime_Helper_Helper
--PushDeviceData__Insert
--PushDeviceData_Insert_Insert
--PushDeviceData_Update_Insert
--Report_GetTemperatureHistory
--Report_GetTemperatureHistory_RE
--Report_GetVehicleConsolidatedReport
--Report_GetVehicleConsolidatedReport_ReadyReport
--Report_GetVehicleHaltTime
--Report_GetVehicleHaltTime_ReadyReport
--Report_GetVehicleHistory
--Report_GetVehicleHistoryDetail
--Report_GetVehicleRunAndHaltReport
--//Report_GetVehicleInOutHistoryDetail
--//VehicleInDepo_Insert

--INSERT INTO [3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
--SELECT old.FK_Vehicle,old.Latitude,old.Longitude,old.Altitude,old.EngineStatus,old.Course,old.Temperature,old.Fuel,old.Speed,old.Distance,old.UpdateTime,old.ServerTime 
--FROM [3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData as old
--where old.UpdateTime >= '2019-04-01' ;

--Delete from [3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData where UpdateTime >= '2019-04-01';

select '';
END
GO
/****** Object:  StoredProcedure [dbo].[MYSP_CreateModel]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[MYSP_CreateModel]
	@objname nvarchar(776) = NULL		-- object name we're after
AS
BEGIN
	set nocount on
	declare	@dbname	sysname ,@no varchar(35), @yes varchar(35), @none varchar(35);
	select @no = 'no', @yes = 'yes', @none = 'none';

	declare @objid int;
	select @objid = object_id from sys.all_objects where object_id = object_id(@objname);

	----Default:Start
	--if exists (select * from sys.all_columns where object_id = @objid)
	--begin
	--	declare @precscaletypes nvarchar(150);
	--	select @precscaletypes = N'tinyint,smallint,decimal,int,bigint,real,money,float,numeric,smallmoney,date,time,datetime2,datetimeoffset,';
	--	select
	--		'Column_name'			= name,
	--		'Type'					= type_name(user_type_id),
	--		'Computed'				= case when ColumnProperty(object_id, name, 'IsComputed') = 0 then @no else @yes end,
	--		'Length'					= convert(int, max_length),
	--		'Prec'					= case when charindex(type_name(system_type_id) + ',', @precscaletypes) > 0
	--									then convert(char(5),ColumnProperty(object_id, name, 'precision'))
	--									else '     ' end,
	--		'Scale'					= case when charindex(type_name(system_type_id) + ',', @precscaletypes) > 0
	--									then convert(char(5),OdbcScale(system_type_id,scale))
	--									else '     ' end,
	--		'Nullable'				= case when is_nullable = 0 then @no else @yes end,
	--		'TrimTrailingBlanks'	= case ColumnProperty(object_id, name, 'UsesAnsiTrim')
	--									when 1 then @no
	--									when 0 then @yes
	--									else '(n/a)' end,
	--		'FixedLenNullInSource'	= case
	--					when type_name(system_type_id) not in ('varbinary','varchar','binary','char')
	--						then '(n/a)'
	--					when is_nullable = 0 then @no else @yes end,
	--		'Collation'		= collation_name
	--	from sys.all_columns where object_id = @objid;
	--end
	----Default:End

	--Custom:Start
	if exists (select * from sys.all_columns where object_id = @objid)
	begin
		declare @precscaletypes nvarchar(150);
		select @precscaletypes = N'tinyint,smallint,decimal,int,bigint,real,money,float,numeric,smallmoney,date,time,datetime2,datetimeoffset,';
		select
			'public'				 = 'public',
			'Type'					= [dbo].[GetDotNetDataType](type_name(user_type_id)),
			'Column_name'			= name,
			
			'Nullable'				= case when is_nullable = 0 then @no else @yes end
			
		from sys.all_columns where object_id = @objid;
	end
	--Custom:End



END
GO
/****** Object:  StoredProcedure [dbo].[MYSP_Help]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[MYSP_Help]
	@objname nvarchar(776) = NULL		-- object name we're after
AS
BEGIN
	set nocount on
	declare	@dbname	sysname ,@no varchar(35), @yes varchar(35), @none varchar(35);
	select @no = 'no', @yes = 'yes', @none = 'none';

	declare @objid int;
	select @objid = object_id from sys.all_objects where object_id = object_id(@objname);

	if exists (select * from sys.all_columns where object_id = @objid)
	begin
		declare @precscaletypes nvarchar(150);
		select @precscaletypes = N'tinyint,smallint,decimal,int,bigint,real,money,float,numeric,smallmoney,date,time,datetime2,datetimeoffset,';
		select
			
			'Column_name'			= name,
			'Type'					= [dbo].[GetDotNetDataType](type_name(user_type_id)),
			'Nullable'				= case when is_nullable = 0 then @no else @yes end
			
		from sys.all_columns where object_id = @objid
		order by 'Column_name';
	end
	--Custom:End



END
GO
/****** Object:  StoredProcedure [dbo].[PushDeviceData__Insert]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PushDeviceData__Insert](
@PK_Vehicle uniqueidentifier,
@GpsIMEINumber varchar(50),
@UpdateTime datetime,
@Latitude varchar(50),
@Longitude varchar(50),
@Altitude varchar(50),
@EngineStatus varchar(50),
@Course varchar(50),
@Temperature varchar(50),
@Fuel varchar(50),
@Speed varchar(50),
@Distance varchar(50),
@EventCode varchar(5),
@Status_PostionValidity varchar(1),
@Status_SateliteCount int,
@Status_GSMSignalStrength int,
@RemainingCash int
)
AS
BEGIN
DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();

--Next
IF(@UpdateTime >= '2022-12-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData(FK_Vehicle,GpsIMEINumber,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@GpsIMEINumber,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	
END

--Current
ELSE IF(@UpdateTime >= '2022-11-01' AND @UpdateTime < '2022-12-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	
END

--Previous
ELSE IF(@UpdateTime >= '2022-10-01' AND @UpdateTime < '2022-11-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
END
ELSE
BEGIN
  Select '_Insert-OK-UpdateTime out of range' AS 'RESPONSE'; return;
END

Select '_Insert-OK' AS 'RESPONSE';
END
GO
/****** Object:  StoredProcedure [dbo].[PushDeviceData_Insert_Insert]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PushDeviceData_Insert_Insert](
@GpsIMEINumber varchar(50),
@UpdateTime datetime,
@Latitude varchar(50),
@Longitude varchar(50),
@Altitude varchar(50),
@EngineStatus varchar(50),
@Course varchar(50),
@Temperature varchar(50),
@Fuel varchar(50),
@Speed varchar(50),
@Distance varchar(50),
@EventCode varchar(5),
@Status_PostionValidity varchar(1),
@Status_SateliteCount int,
@Status_GSMSignalStrength int,
@RemainingCash int
)
AS
BEGIN
DECLARE @PK_Vehicle varchar(50) = ( SELECT TOP 1 PK_Vehicle FROM VehicleTrackingInformation where GpsIMEINumber = @GpsIMEINumber );
IF((SELECT ISNULL ( @PK_Vehicle, '')) = '')
	BEGIN
	SELECT 'InsertAndInsert-No vehicle found for given IMEI' AS 'RESPONSE';
	return;
END

DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();

INSERT INTO VehicleTracking(PK_Vehicle,WillInsert,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,RemainingCash,UpdateTime,ServerTime) 
VALUES (@PK_Vehicle,1,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@RemainingCash,@UpdateTime,@CurrentDateTime);

--Next
IF(@UpdateTime >= '2022-12-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData(FK_Vehicle,GpsIMEINumber,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@GpsIMEINumber,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

	--INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	--VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);
END

--Current
ELSE IF(@UpdateTime >= '2022-11-01' AND @UpdateTime < '2022-12-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

	--INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	--VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);
END

--Previous
ELSE IF(@UpdateTime >= '2022-10-01' AND @UpdateTime < '2022-11-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
END

ELSE
BEGIN
  Select 'InsertAndInsert-OK-UpdateTime out of range' AS 'RESPONSE'; return;
END

Select 'InsertAndInsert-OK' AS 'RESPONSE';
END

-- EXEC VTS_PushDeviceData_Insert_Insert 'ASDF', '23.2525', '90.2536', '12.0235', '0', '0', '0', '0', '0', '0' 
GO
/****** Object:  StoredProcedure [dbo].[PushDeviceData_Insert_Insert_VT1]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PushDeviceData_Insert_Insert_VT1](
@GpsIMEINumber varchar(50),
@UpdateTime datetime,
@Latitude varchar(50),
@Longitude varchar(50),
@Altitude varchar(50),
@EngineStatus varchar(50),
@Course varchar(50),
@Temperature varchar(50),
@Fuel varchar(50),
@Speed varchar(50),
@Distance varchar(50),
@EventCode varchar(5),
@Status_PostionValidity varchar(1),
@Status_SateliteCount int,
@Status_GSMSignalStrength int,
@RemainingCash int
)
AS
BEGIN
DECLARE @PK_Vehicle varchar(50) = ( SELECT TOP 1 PK_Vehicle FROM VehicleTrackingInformation where GpsIMEINumber = @GpsIMEINumber );
IF((SELECT ISNULL ( @PK_Vehicle, '')) = '')
	BEGIN
	SELECT 'InsertAndInsert-No vehicle found for given IMEI' AS 'RESPONSE';
	return;
END

DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();

INSERT INTO VehicleTrackingVT1(PK_Vehicle,WillInsert,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,RemainingCash,UpdateTime,ServerTime) 
VALUES (@PK_Vehicle,1,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@RemainingCash,@UpdateTime,@CurrentDateTime);

--Next
IF(@UpdateTime >= '2022-12-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData(FK_Vehicle,GpsIMEINumber,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@GpsIMEINumber,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

	--INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	--VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);
END

--Current
ELSE IF(@UpdateTime >= '2022-11-01' AND @UpdateTime < '2022-12-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

	--INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	--VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);
END

--Previous
ELSE IF(@UpdateTime >= '2022-10-01' AND @UpdateTime < '2022-11-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
END

ELSE
BEGIN
  Select 'InsertAndInsert-OK-UpdateTime out of range' AS 'RESPONSE'; return;
END

Select 'InsertAndInsert-OK' AS 'RESPONSE';
END

-- EXEC VTS_PushDeviceData_Insert_Insert 'ASDF', '23.2525', '90.2536', '12.0235', '0', '0', '0', '0', '0', '0' 
GO
/****** Object:  StoredProcedure [dbo].[PushDeviceData_Update_Insert]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PushDeviceData_Update_Insert](
@PK_Vehicle uniqueidentifier,
@GpsIMEINumber varchar(50),
@UpdateTime datetime,
@Latitude varchar(50),
@Longitude varchar(50),
@Altitude varchar(50),
@EngineStatus varchar(50),
@Course varchar(50),
@Temperature varchar(50),
@Fuel varchar(50),
@Speed varchar(50),
@Distance varchar(50),
@EventCode varchar(5),
@Status_PostionValidity varchar(1),
@Status_SateliteCount int,
@Status_GSMSignalStrength int,
@RemainingCash int,
@IsLocationChanged bit
)
AS
BEGIN
DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();

IF(@Status_PostionValidity = 'A' OR @Status_PostionValidity = '1')
BEGIN
	IF(@IsLocationChanged = 1)
	BEGIN
		UPDATE VehicleTracking SET WillInsert= 1, Latitude = @Latitude, Longitude = @Longitude, Altitude = @Altitude,
		EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
		Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
		RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime, Location_ChangedAt = @CurrentDateTime
		WHERE PK_Vehicle = @PK_Vehicle;
	END
	ELSE
	BEGIN
		UPDATE VehicleTracking SET WillInsert= 1, Latitude = @Latitude, Longitude = @Longitude, Altitude = @Altitude,
		EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
		Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
		RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime
		WHERE PK_Vehicle = @PK_Vehicle;
	END
END
ELSE
BEGIN
IF(@IsLocationChanged = 1)
	BEGIN
		UPDATE VehicleTracking SET WillInsert= 1, Altitude = @Altitude,
		EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
		Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
		RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime, Location_ChangedAt = @CurrentDateTime
		WHERE PK_Vehicle = @PK_Vehicle;
	END
	ELSE
	BEGIN
		UPDATE VehicleTracking SET WillInsert= 1, Altitude = @Altitude,
		EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
		Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
		RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime
		WHERE PK_Vehicle = @PK_Vehicle;
	END	
END




----Next
--IF(@UpdateTime >= '2022-09-01')
--BEGIN
--	INSERT INTO [3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData(FK_Vehicle,GpsIMEINumber,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@GpsIMEINumber,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

--	INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
--END

----Current
--ELSE IF(@UpdateTime >= '2022-08-01' AND @UpdateTime < '2022-09-01')
--BEGIN
--	INSERT INTO [3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	
	
--	INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
--END

----Previous
--ELSE IF(@UpdateTime >= '2022-07-01' AND @UpdateTime < '2022-08-01')
--BEGIN
--	INSERT INTO [3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
--END
--ELSE
--BEGIN
--  Select 'UpdateAndInsert-OK-UpdateTime of range' AS 'RESPONSE'; return;
--END

Select 'UpdateAndInsert-OK' AS 'RESPONSE';
END

--EXEC dbo.PushDeviceData_Update_Insert 
--'F324DA22-DE90-4B11-A7AE-861DA92D070C',
--'864495031516724',
--'2021-05-27 10:48:14.000',
--'25.91191',
--'88.765033',
--'25.91191',
--'1',
--'1',
--'1',
--'1',
--'1',
--'1',
--'35',
--'1',
--1,
--1,
--1


GO
/****** Object:  StoredProcedure [dbo].[PushDeviceData_Update_Insert_VT1]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PushDeviceData_Update_Insert_VT1](
@PK_Vehicle uniqueidentifier,
@GpsIMEINumber varchar(50),
@UpdateTime datetime,
@Latitude varchar(50),
@Longitude varchar(50),
@Altitude varchar(50),
@EngineStatus varchar(50),
@Course varchar(50),
@Temperature varchar(50),
@Fuel varchar(50),
@Speed varchar(50),
@Distance varchar(50),
@EventCode varchar(5),
@Status_PostionValidity varchar(1),
@Status_SateliteCount int,
@Status_GSMSignalStrength int,
@RemainingCash int,
@IsLocationChanged bit
)
AS
BEGIN
DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();

IF(@Status_PostionValidity = 'A' OR @Status_PostionValidity = '1')
BEGIN
	IF(@IsLocationChanged = 1)
	BEGIN
		UPDATE VehicleTrackingVT1 SET WillInsert= 1, Latitude = @Latitude, Longitude = @Longitude, Altitude = @Altitude,
		EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
		Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
		RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime, Location_ChangedAt = @CurrentDateTime
		WHERE PK_Vehicle = @PK_Vehicle;
	END
	ELSE
	BEGIN
		UPDATE VehicleTrackingVT1 SET WillInsert= 1, Latitude = @Latitude, Longitude = @Longitude, Altitude = @Altitude,
		EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
		Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
		RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime
		WHERE PK_Vehicle = @PK_Vehicle;
	END
END
ELSE
BEGIN
IF(@IsLocationChanged = 1)
	BEGIN
		UPDATE VehicleTrackingVT1 SET WillInsert= 1, Altitude = @Altitude,
		EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
		Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
		RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime, Location_ChangedAt = @CurrentDateTime
		WHERE PK_Vehicle = @PK_Vehicle;
	END
	ELSE
	BEGIN
		UPDATE VehicleTrackingVT1 SET WillInsert= 1, Altitude = @Altitude,
		EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
		Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
		RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime
		WHERE PK_Vehicle = @PK_Vehicle;
	END	
END




----Next
--IF(@UpdateTime >= '2022-09-01')
--BEGIN
--	INSERT INTO [3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData(FK_Vehicle,GpsIMEINumber,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@GpsIMEINumber,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

--	INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
--END

----Current
--ELSE IF(@UpdateTime >= '2022-08-01' AND @UpdateTime < '2022-09-01')
--BEGIN
--	INSERT INTO [3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	
	
--	INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
--END

----Previous
--ELSE IF(@UpdateTime >= '2022-07-01' AND @UpdateTime < '2022-08-01')
--BEGIN
--	INSERT INTO [3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
--	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
--END
--ELSE
--BEGIN
--  Select 'UpdateAndInsert-OK-UpdateTime of range' AS 'RESPONSE'; return;
--END

Select 'UpdateAndInsert-OK' AS 'RESPONSE';
END

--EXEC dbo.PushDeviceData_Update_Insert 
--'F324DA22-DE90-4B11-A7AE-861DA92D070C',
--'864495031516724',
--'2021-05-27 10:48:14.000',
--'25.91191',
--'88.765033',
--'25.91191',
--'1',
--'1',
--'1',
--'1',
--'1',
--'1',
--'35',
--'1',
--1,
--1,
--1


GO
/****** Object:  StoredProcedure [dbo].[PushDeviceData_UpdateTime]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PushDeviceData_UpdateTime](
@PK_Vehicle uniqueidentifier,
@UpdateTime datetime
)
AS
BEGIN
DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();
UPDATE VehicleTracking SET UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime WHERE PK_Vehicle = @PK_Vehicle;
Select 'UpdateTime-OK' AS 'RESPONSE';
END

-- EXEC PushDeviceData_UpdateTime '14A23894-BD84-4D10-8BF1-50702A62F24A'
GO
/****** Object:  StoredProcedure [dbo].[Report_GetAccessibleEventList_InDateRange]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetAccessibleEventList_InDateRange] (
@FK_Vehicle uniqueidentifier
--@StartingDate Date,
--@EndingDate Date
)
AS
BEGIN  
	select Vehicle.RegistrationNumber,
	(select SUM(Event.TotalAmount) from Event where Event.FK_Vehicle = Vehicle.PK_Vehicle AND Event.FK_EventType = 'tax_token' AND Event.IssueDate >= '2018-01-01' AND Event.IssueDate <= '2018-01-01') as tax_token,
	(select SUM(Event.TotalAmount) from Event where Event.FK_Vehicle = Vehicle.PK_Vehicle AND Event.FK_EventType = 'fitness_paper' AND Event.IssueDate >= '2018-01-01' AND Event.IssueDate <= '2018-01-01') as fitness_paper,
	(select SUM(Event.TotalAmount) from Event where Event.FK_Vehicle = Vehicle.PK_Vehicle AND Event.FK_EventType = 'route_permit' AND Event.IssueDate >= '2018-01-01' AND Event.IssueDate <= '2018-01-01') as route_permit

	from Vehicle where (select count(*) from Event where Event.FK_Vehicle = Vehicle.PK_Vehicle AND Event.TotalAmount Is not null)>0
	;
END  
--exec [Report_GetAccessibleEventList_InDateRange] '1A79E829-AC74-4B57-8E46-AAF7E357B10A'

GO
/****** Object:  StoredProcedure [dbo].[Report_GetTemperatureHistory]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetTemperatureHistory](
@USER_KEY varchar(50),
@FK_Vehicle uniqueidentifier,
@StartingDate datetime,
@EndingDate datetime,
@IntervalMinute int
)
AS
BEGIN
	Declare @RegistrationNumber varchar(50);
	SELECT TOP 1  @RegistrationNumber = Vehicle.RegistrationNumber FROM [dbo].[Vehicle] WHERE Vehicle.PK_Vehicle = @FK_Vehicle;	

	delete from Report_TemperatureReport where @USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle;
	delete from Report_TemperatureReport_Helper where @USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle;

	----#[3rdEyE_TrackingDataBase_2018_09]
	--IF((@StartingDate >= '2018-09-01' AND @StartingDate < '2018-10-01') OR (@EndingDate >= '2018-09-01' AND @EndingDate < '2018-10-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2018_09].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_10]
	--IF((@StartingDate >= '2018-10-01' AND @StartingDate < '2018-11-01') OR (@EndingDate >= '2018-10-01' AND @EndingDate < '2018-11-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2018_11]
	--IF((@StartingDate >= '2018-11-01' AND @StartingDate < '2018-12-01') OR (@EndingDate >= '2018-11-01' AND @EndingDate < '2018-12-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_12]
	--IF((@StartingDate >= '2018-12-01' AND @StartingDate < '2019-01-01') OR (@EndingDate >= '2018-12-01' AND @EndingDate < '2019-01-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_01]
	--IF((@StartingDate >= '2019-01-01' AND @StartingDate < '2019-02-01') OR (@EndingDate >= '2019-01-01' AND @EndingDate < '2019-02-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_02]
	--IF((@StartingDate >= '2019-02-01' AND @StartingDate < '2019-03-01') OR (@EndingDate >= '2019-02-01' AND @EndingDate < '2019-03-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_03]
	--IF((@StartingDate >= '2019-03-01' AND @StartingDate < '2019-04-01') OR (@EndingDate >= '2019-03-01' AND @EndingDate < '2019-04-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_04]
	--IF((@StartingDate >= '2019-04-01' AND @StartingDate < '2019-05-01') OR (@EndingDate >= '2019-04-01' AND @EndingDate < '2019-05-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_05]
	--IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_06]
	--IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_07]
	--IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_08]
	--IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_09]
	--IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_10]
	--IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_11]
	--IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_12]
	--IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_01]
	--IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2020_02]
	--IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_03]
	--IF((@StartingDate >= '2020-03-01' AND @StartingDate < '2020-04-01') OR (@EndingDate >= '2020-03-01' AND @EndingDate < '2020-04-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_04]
	--IF((@StartingDate >= '2020-04-01' AND @StartingDate < '2020-05-01') OR (@EndingDate >= '2020-04-01' AND @EndingDate < '2020-05-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_05]
	--IF((@StartingDate >= '2020-05-01' AND @StartingDate < '2020-06-01') OR (@EndingDate >= '2020-05-01' AND @EndingDate < '2020-06-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_06]
	--IF((@StartingDate >= '2020-06-01' AND @StartingDate < '2020-07-01') OR (@EndingDate >= '2020-06-01' AND @EndingDate < '2020-07-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_07]
	--IF((@StartingDate >= '2020-07-01' AND @StartingDate < '2020-08-01') OR (@EndingDate >= '2020-07-01' AND @EndingDate < '2020-08-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_08]
	--IF((@StartingDate >= '2020-08-01' AND @StartingDate < '2020-09-01') OR (@EndingDate >= '2020-08-01' AND @EndingDate < '2020-09-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_09]
	--IF((@StartingDate >= '2020-09-01' AND @StartingDate < '2020-10-01') OR (@EndingDate >= '2020-09-01' AND @EndingDate < '2020-10-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_10]
	--IF((@StartingDate >= '2020-10-01' AND @StartingDate < '2020-11-01') OR (@EndingDate >= '2020-10-01' AND @EndingDate < '2020-11-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_11]
	--IF((@StartingDate >= '2020-11-01' AND @StartingDate < '2020-12-01') OR (@EndingDate >= '2020-11-01' AND @EndingDate < '2020-12-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2020_12]
	--IF((@StartingDate >= '2020-12-01' AND @StartingDate < '2021-01-01') OR (@EndingDate >= '2020-12-01' AND @EndingDate < '2021-01-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_01]
	--IF((@StartingDate >= '2021-01-01' AND @StartingDate < '2021-02-01') OR (@EndingDate >= '2021-01-01' AND @EndingDate < '2021-02-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_02]
	--IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_03]
	--IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_04]
	--IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_05]
	--IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_06]
	--IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	--#[3rdEyE_TrackingDataBase_2021_07]
	--IF((@StartingDate >= '2021-07-01' AND @StartingDate < '2021-08-01') OR (@EndingDate >= '2021-07-01' AND @EndingDate < '2021-08-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_08]
	--IF((@StartingDate >= '2021-08-01' AND @StartingDate < '2021-09-01') OR (@EndingDate >= '2021-08-01' AND @EndingDate < '2021-09-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_09]
	--IF((@StartingDate >= '2021-09-01' AND @StartingDate < '2021-10-01') OR (@EndingDate >= '2021-09-01' AND @EndingDate < '2021-10-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_10]
	--IF((@StartingDate >= '2021-10-01' AND @StartingDate < '2021-11-01') OR (@EndingDate >= '2021-10-01' AND @EndingDate < '2021-11-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_11]
	--IF((@StartingDate >= '2021-11-01' AND @StartingDate < '2021-12-01') OR (@EndingDate >= '2021-11-01' AND @EndingDate < '2021-12-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_12]
	--IF((@StartingDate >= '2021-12-01' AND @StartingDate < '2022-01-01') OR (@EndingDate >= '2021-12-01' AND @EndingDate < '2022-01-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_01]
	--IF((@StartingDate >= '2022-01-01' AND @StartingDate < '2022-02-01') OR (@EndingDate >= '2022-01-01' AND @EndingDate < '2022-02-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_02]
	--IF((@StartingDate >= '2022-02-01' AND @StartingDate < '2022-03-01') OR (@EndingDate >= '2022-02-01' AND @EndingDate < '2022-03-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_03]
	--IF((@StartingDate >= '2022-03-01' AND @StartingDate < '2022-04-01') OR (@EndingDate >= '2022-03-01' AND @EndingDate < '2022-04-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_04]
	--IF((@StartingDate >= '2022-04-01' AND @StartingDate < '2022-05-01') OR (@EndingDate >= '2022-04-01' AND @EndingDate < '2022-05-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_05]
	--IF((@StartingDate >= '2022-05-01' AND @StartingDate < '2022-06-01') OR (@EndingDate >= '2022-05-01' AND @EndingDate < '2022-06-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_06]
	--IF((@StartingDate >= '2022-06-01' AND @StartingDate < '2022-07-01') OR (@EndingDate >= '2022-06-01' AND @EndingDate < '2022-07-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_07]
	--IF((@StartingDate >= '2022-07-01' AND @StartingDate < '2022-08-01') OR (@EndingDate >= '2022-07-01' AND @EndingDate < '2022-08-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_08]
	--IF((@StartingDate >= '2022-08-01' AND @StartingDate < '2022-09-01') OR (@EndingDate >= '2022-08-01' AND @EndingDate < '2022-09-01'))
	--BEGIN
	--	insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	--#[3rdEyE_TrackingDataBase_2022_09]
	IF((@StartingDate >= '2022-09-01' AND @StartingDate < '2022-10-01') OR (@EndingDate >= '2022-09-01' AND @EndingDate < '2022-10-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_10]
	IF((@StartingDate >= '2022-10-01' AND @StartingDate < '2022-11-01') OR (@EndingDate >= '2022-10-01' AND @EndingDate < '2022-11-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2022_11]
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01') OR (@EndingDate >= '2022-11-01' AND @EndingDate < '2022-12-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
		
	--#[3rdEyE_TrackingDataBase_2022_12]
	IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01') OR (@EndingDate >= '2022-12-01' AND @EndingDate < '2023-01-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END

	

	DECLARE @UpdateTime datetime,@Temperature float;    
	DECLARE @_PreviousUpdateTime datetime = @StartingDate;    
	DECLARE data_cursor CURSOR local FOR     
	select UpdateTime, Temperature from Report_TemperatureReport_Helper
	where @USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle; 
  
	OPEN data_cursor;    
	FETCH NEXT FROM data_cursor INTO @UpdateTime, @Temperature;
    
	WHILE @@FETCH_STATUS = 0    
	BEGIN
		IF(@UpdateTime > @_PreviousUpdateTime)	    
		BEGIN
			Insert into Report_TemperatureReport(USER_KEY,FK_Vehicle,UpdateTime,Temperature) values(@USER_KEY,@FK_Vehicle,@UpdateTime,@Temperature);
			set @_PreviousUpdateTime = DATEADD(MINUTE, @IntervalMinute, @UpdateTime);
		END
	
		FETCH NEXT FROM data_cursor INTO @UpdateTime, @Temperature;  
	END     
	CLOSE data_cursor;    
	DEALLOCATE data_cursor;  
	select @RegistrationNumber as 'RegistrationNumber', UpdateTime, Temperature from Report_TemperatureReport where USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle;

END

--  EXEC Report_GetTemperatureHistory '00000000-0000-0000-0000-000000000000', '5765b1a0-1f25-4a04-ad00-46c1d199cb25', '19-Oct-19 6:00:00 AM', '19-Oct-19 8:00:00 AM', '30'

GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleConsolidatedReport]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[Report_GetVehicleConsolidatedReport](
@USER_KEY varchar(50),
@FK_Vehicle Uniqueidentifier,
@StarTingDate datetime
)
AS
BEGIN
	
--###################################### H E A D ##########################################
	Declare @EndingDate datetime = dateadd(dd,1,@StarTingDate);
	Declare @RegistrationNumber varchar(50);
	Declare @MobileNumber varchar(50);
	Declare @LastUpdate varchar(50);
	Declare @NearestMapLocation varchar(300);
	Declare @NearestMapLocationDistance varchar(300);
	
	
	Declare @AverageHaltTime float = 0;
	Declare @MininumMinuteDealy int = 1;
	Declare @MaximumHaltTime int = 0;

	DECLARE @HaltTime BIGINT=0;
	DECLARE @TotalHaltTime BIGINT=0;
	DECLARE @HaltCount BIGINT=0;

	DECLARE @first_id BIGINT = 0;
	Declare @first_UpdateTime DATETIME;

	Declare @min_ID  BIGINT=0;
	Declare @max_ID BIGINT=0;

	DECLARE @standingState_id BIGINT=0;
	DECLARE @runningState_id BIGINT=0;

	DECLARE @standingState_DateTime datetime;
	DECLARE @runningState_DateTime datetime;

	
	--last updated
	SELECT TOP 1 
	@RegistrationNumber = Vehicle.RegistrationNumber
	,@MobileNumber = Vehicle.Internal_VehicleContactNumber
	FROM [dbo].[VehicleTracking]
	JOIN Vehicle ON VehicleTracking.PK_Vehicle = Vehicle.PK_Vehicle
	WHERE VehicleTracking.PK_Vehicle = @FK_Vehicle;	

	DELETE FROM Report_ConsolidatedRport WHERE USER_KEY = @USER_KEY;  

--###################################### B O D Y ##########################################

	
	----#[3rdEyE_TrackingDataBase_2018_09]
	--IF((@StartingDate >= '2018-09-01' AND @StartingDate < '2018-10-01') OR (@EndingDate >= '2018-09-01' AND @EndingDate < '2018-10-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_10]
	--IF((@StartingDate >= '2018-10-01' AND @StartingDate < '2018-11-01') OR (@EndingDate >= '2018-10-01' AND @EndingDate < '2018-11-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2018_11]
	--IF((@StartingDate >= '2018-11-01' AND @StartingDate < '2018-12-01') OR (@EndingDate >= '2018-11-01' AND @EndingDate < '2018-12-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_12]
	--IF((@StartingDate >= '2018-12-01' AND @StartingDate < '2019-01-01') OR (@EndingDate >= '2018-12-01' AND @EndingDate < '2019-01-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_01]
	--IF((@StartingDate >= '2019-01-01' AND @StartingDate < '2019-02-01') OR (@EndingDate >= '2019-01-01' AND @EndingDate < '2019-02-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_02]
	--IF((@StartingDate >= '2019-02-01' AND @StartingDate < '2019-03-01') OR (@EndingDate >= '2019-02-01' AND @EndingDate < '2019-03-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_03]
	--IF((@StartingDate >= '2019-03-01' AND @StartingDate < '2019-04-01') OR (@EndingDate >= '2019-03-01' AND @EndingDate < '2019-04-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_04]
	--IF((@StartingDate >= '2019-04-01' AND @StartingDate < '2019-05-01') OR (@EndingDate >= '2019-04-01' AND @EndingDate < '2019-05-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_05]
	--IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_06]
	--IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_07]
	--IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_08]
	--IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_09]
	--IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_10]
	--IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_11]
	--IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_12]
	--IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_01]
	--IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_02]
	--IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_03]
	--IF((@StartingDate >= '2020-03-01' AND @StartingDate < '2020-04-01') OR (@EndingDate >= '2020-03-01' AND @EndingDate < '2020-04-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2020_04]
	--IF((@StartingDate >= '2020-04-01' AND @StartingDate < '2020-05-01') OR (@EndingDate >= '2020-04-01' AND @EndingDate < '2020-05-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_05]
	--IF((@StartingDate >= '2020-05-01' AND @StartingDate < '2020-06-01') OR (@EndingDate >= '2020-05-01' AND @EndingDate < '2020-06-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_06]
	--IF((@StartingDate >= '2020-06-01' AND @StartingDate < '2020-07-01') OR (@EndingDate >= '2020-06-01' AND @EndingDate < '2020-07-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_07]
	--IF((@StartingDate >= '2020-07-01' AND @StartingDate < '2020-08-01') OR (@EndingDate >= '2020-07-01' AND @EndingDate < '2020-08-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_08]
	--IF((@StartingDate >= '2020-08-01' AND @StartingDate < '2020-09-01') OR (@EndingDate >= '2020-08-01' AND @EndingDate < '2020-09-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_09]
	--IF((@StartingDate >= '2020-09-01' AND @StartingDate < '2020-10-01') OR (@EndingDate >= '2020-09-01' AND @EndingDate < '2020-10-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_10]
	--IF((@StartingDate >= '2020-10-01' AND @StartingDate < '2020-11-01') OR (@EndingDate >= '2020-10-01' AND @EndingDate < '2020-11-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_11]
	--IF((@StartingDate >= '2020-11-01' AND @StartingDate < '2020-12-01') OR (@EndingDate >= '2020-11-01' AND @EndingDate < '2020-12-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_12]
	--IF((@StartingDate >= '2020-12-01' AND @StartingDate < '2021-01-01') OR (@EndingDate >= '2020-12-01' AND @EndingDate < '2021-01-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_01]
	--IF((@StartingDate >= '2021-01-01' AND @StartingDate < '2021-02-01') OR (@EndingDate >= '2021-01-01' AND @EndingDate < '2021-02-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_02]
	--IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_03]
	--IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_04]
	--IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_05]
	--IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_06]
	--IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_07]
	--IF((@StartingDate >= '2021-07-01' AND @StartingDate < '2021-08-01') OR (@EndingDate >= '2021-07-01' AND @EndingDate < '2021-08-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_08]
	--IF((@StartingDate >= '2021-08-01' AND @StartingDate < '2021-09-01') OR (@EndingDate >= '2021-08-01' AND @EndingDate < '2021-09-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_09]
	--IF((@StartingDate >= '2021-09-01' AND @StartingDate < '2021-10-01') OR (@EndingDate >= '2021-09-01' AND @EndingDate < '2021-10-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_10]
	--IF((@StartingDate >= '2021-10-01' AND @StartingDate < '2021-11-01') OR (@EndingDate >= '2021-10-01' AND @EndingDate < '2021-11-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_11]
	--IF((@StartingDate >= '2021-11-01' AND @StartingDate < '2021-12-01') OR (@EndingDate >= '2021-11-01' AND @EndingDate < '2021-12-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_12]
	--IF((@StartingDate >= '2021-12-01' AND @StartingDate < '2022-01-01') OR (@EndingDate >= '2021-12-01' AND @EndingDate < '2022-01-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_01]
	--IF((@StartingDate >= '2022-01-01' AND @StartingDate < '2022-02-01') OR (@EndingDate >= '2022-01-01' AND @EndingDate < '2022-02-01'))	
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_02]
	--IF((@StartingDate >= '2022-02-01' AND @StartingDate < '2022-03-01') OR (@EndingDate >= '2022-02-01' AND @EndingDate < '2022-03-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_03]
	--IF((@StartingDate >= '2022-03-01' AND @StartingDate < '2022-04-01') OR (@EndingDate >= '2022-03-01' AND @EndingDate < '2022-04-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
		
	----#[3rdEyE_TrackingDataBase_2022_04]
	--IF((@StartingDate >= '2022-04-01' AND @StartingDate < '2022-05-01') OR (@EndingDate >= '2022-04-01' AND @EndingDate < '2022-05-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_05]
	--IF((@StartingDate >= '2022-05-01' AND @StartingDate < '2022-06-01') OR (@EndingDate >= '2022-05-01' AND @EndingDate < '2022-06-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_06]
	--IF((@StartingDate >= '2022-06-01' AND @StartingDate < '2022-07-01') OR (@EndingDate >= '2022-06-01' AND @EndingDate < '2022-07-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_07]
	--IF((@StartingDate >= '2022-07-01' AND @StartingDate < '2022-08-01') OR (@EndingDate >= '2022-07-01' AND @EndingDate < '2022-08-01'))
	--BEGIN
	--	Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
		
	--#[3rdEyE_TrackingDataBase_2022_08]
	IF((@StartingDate >= '2022-08-01' AND @StartingDate < '2022-09-01') OR (@EndingDate >= '2022-08-01' AND @EndingDate < '2022-09-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_09]
	IF((@StartingDate >= '2022-09-01' AND @StartingDate < '2022-10-01') OR (@EndingDate >= '2022-09-01' AND @EndingDate < '2022-10-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_10]
	IF((@StartingDate >= '2022-10-01' AND @StartingDate < '2022-11-01') OR (@EndingDate >= '2022-10-01' AND @EndingDate < '2022-11-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_11]
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01') OR (@EndingDate >= '2022-11-01' AND @EndingDate < '2022-12-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2022_12]
	IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01') OR (@EndingDate >= '2022-12-01' AND @EndingDate < '2023-01-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	SELECT TOP 1 
		@NearestMapLocation = (SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( Report_ConsolidatedRport.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( Report_ConsolidatedRport.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( Report_ConsolidatedRport.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))))
		,@NearestMapLocationDistance = convert(varchar(50),Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( Report_ConsolidatedRport.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( Report_ConsolidatedRport.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( Report_ConsolidatedRport.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2))
		,@LastUpdate = convert(varchar(50),Report_ConsolidatedRport.UpdateTime)
		FROM Report_ConsolidatedRport
		WHERE Report_ConsolidatedRport.USER_KEY = @USER_KEY
		AND Report_ConsolidatedRport.UpdateTime > @StarTingDate
		AND Report_ConsolidatedRport.UpdateTime < @EndingDate
		--ORDER BY PK_RowData DESC;
		ORDER BY UpdateTime DESC;

		IF(@NearestMapLocation is null)
		BEGIN
			SET @NearestMapLocation = '';
		END

		IF(@NearestMapLocationDistance is null)
		BEGIN
			SET @NearestMapLocationDistance = '';
		END

		IF(@LastUpdate is null)
		BEGIN
			SET @LastUpdate = '';
		END


		--first_id calculation and insert
		SELECT TOP 1 @first_id = PK_RowData, @first_UpdateTime = UpdateTime FROM Report_ConsolidatedRport
		WHERE USER_KEY = @USER_KEY;
		IF(@first_id != 0)
		BEGIN
			SET @HaltTime = DATEDIFF(mi, @StartingDate, @first_UpdateTime);
			IF(@HaltTime >= @MininumMinuteDealy)
			BEGIN
				set @TotalHaltTime = @TotalHaltTime + @HaltTime;
				set @HaltCount = @HaltCount + 1;
				
				IF(@HaltTime>@MaximumHaltTime)
				BEGIN
					SET @MaximumHaltTime = @HaltTime;
				END
			END
		END
	
		--min_DateTime calculation
		SET @min_ID = @first_id;
		--SELECT TOP 1 @min_ID = PK_RowData FROM Report_ConsolidatedRport 
		--WHERE USER_KEY = @USER_KEY
		--AND EngineStatus = '0'
		--AND Speed = 0;

		--max_Datetime calculation
		SET @max_ID = 0;
		SELECT TOP 1 @max_ID = PK_RowData FROM Report_ConsolidatedRport 
		WHERE USER_KEY = @USER_KEY
		ORDER BY PK_RowData DESC;
	
		--INSERT 
		WHILE(@min_ID != 0 AND @max_ID != 0 AND @min_ID < @max_ID)
		BEGIN

			--STAT 0
			SET @standingState_id = 0;
			SET @standingState_DateTime = '2000-01-01';
			SELECT TOP 1 
			@standingState_id = PK_RowData, 
			@standingState_DateTime = UpdateTime
			FROM Report_ConsolidatedRport 
			WHERE USER_KEY = @USER_KEY AND PK_RowData >= @min_ID AND EngineStatus = '0' AND Speed = 0;

			IF(@standingState_id != 0)
			BEGIN
				SET @min_ID = @standingState_id + 1;
			END
			ELSE
			BEGIN
				BREAK;
			END

			--STAT 1
			SET @runningState_id = 0;
			SET @runningState_DateTime = '2000-01-01';
			SELECT TOP 1 
			@runningState_id = PK_RowData, 
			@runningState_DateTime = UpdateTime
			FROM Report_ConsolidatedRport 
			WHERE USER_KEY = @USER_KEY AND PK_RoWData >= @min_ID AND PK_RowData <= @max_ID AND (EngineStatus = '1' AND Speed > 0);

			IF(@runningState_id != 0)
			BEGIN
				SET @min_ID = @runningState_id + 1;

				SET @HaltTime = DATEDIFF(mi, @standingState_DateTime, @runningState_DateTime);
				IF(@HaltTime >= @MininumMinuteDealy)
				BEGIN
					set @TotalHaltTime = @TotalHaltTime + @HaltTime;
					set @HaltCount = @HaltCount + 1;
				
					IF(@HaltTime>@MaximumHaltTime)
					BEGIN
						SET @MaximumHaltTime = @HaltTime;
					END
				END
			END
			ELSE
			BEGIN
				--# SET gap time to last update time/ ending time
				SET @runningState_DateTime = @EndingDate;----Uncomment this to calculate untill last momment

				SET @HaltTime = DATEDIFF(mi, @standingState_DateTime, @runningState_DateTime); 
			
				IF(@HaltTime >= @MininumMinuteDealy)
				BEGIN
					set @TotalHaltTime = @TotalHaltTime + @HaltTime;
					set @HaltCount = @HaltCount + 1;
				
					IF(@HaltTime>@MaximumHaltTime)
					BEGIN
						SET @MaximumHaltTime = @HaltTime;
					END
				END
				BREAK;
			END
		END
		


	
--###################################### T A I L ##########################################

	Declare @TotalRunTime float = 0; 
	IF(@TotalHaltTime > 0)
	BEGIN
		SET @TotalRunTime = 1440 - @TotalHaltTime; -- 1 day = 1440 minute
	END

	Declare @TotalDistance float = 0;
	Declare @MaximumSpeed float = 0;
	--Declare @AverageSpeed float = 0;
	
	SELECT
	@TotalDistance = SUM(Report_ConsolidatedRport.Distance),
	@MaximumSpeed = MAX(Report_ConsolidatedRport.Speed)
	--@AverageSpeed = ROUND(AVG(Report_ConsolidatedRport.Speed),2)
	FROM Report_ConsolidatedRport
	WHERE USER_KEY = @USER_KEY
	AND (EngineStatus = '1' AND Speed > 0);
	
	IF(@TotalDistance is null)
	BEGIN
		SET @TotalDistance = 0;
	END

	IF(@MaximumSpeed is null)
	BEGIN
		SET @MaximumSpeed = 0;
	END
	

	SELECT
	'data_consolidated' AS '_rowType',
	CONVERT(varchar(11), @StarTingDate, 100) AS 'ThisDate'
	,@RegistrationNumber AS 'RegistrationNumber'
	,@MobileNumber AS 'MobileNumber'
	,@LastUpdate AS 'LastUpdate'
	,@NearestMapLocation AS 'NearestMapLocation'
	,@NearestMapLocationDistance AS 'NearestMapLocationDistance'
	
	,@TotalHaltTime AS 'TotalHaltTime'
	,@HaltCount AS 'HaltCount'
	,@AverageHaltTime AS 'AverageHaltTime'
	,@MaximumHaltTime AS 'MaximumHaltTime'

	,@TotalRunTime AS 'TotalRunTime'
	
	,@TotalDistance AS 'TotalDistance'
	,@MaximumSpeed AS 'MaximumSpeed'
	;
	
END

-- EXEC _Report_GetVehicleConsolidatedReport 'af4acec4-eb1c-4f06-b729-66f6491846c0', '2018-08-10 12:00:00 AM'

GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleConsolidatedReport_ReadyReport]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleConsolidatedReport_ReadyReport](
@FK_Vehicle uniqueidentifier,
@StartingDate datetime
)
AS

BEGIN
	Declare @EndingDate datetime = dateadd(dd,1,@StarTingDate);

	----#[3rdEyE_TrackingDataBase_2019_03]
	--IF((@StartingDate >= '2019-03-01' AND @StartingDate < '2019-04-01') AND (@EndingDate >= '2019-03-01' AND @EndingDate < '2019-04-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_03].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2019_04]
	--IF((@StartingDate >= '2019-04-01' AND @StartingDate < '2019-05-01') AND (@EndingDate >= '2019-04-01' AND @EndingDate < '2019-05-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_04].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2019_05]
	--IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_05].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_06]
	--IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_06].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2019_07]
	--IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_07].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_08]
	--IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_08].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_09]
	--IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_09].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_10]
	--IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_10].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_11]
	--IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_11].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_12]
	--IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_12].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_01]
	--IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_01].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_02]
	--IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_02].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_03]
	--IF((@StartingDate >= '2020-03-01' AND @StartingDate < '2020-04-01') OR (@EndingDate >= '2020-03-01' AND @EndingDate < '2020-04-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_03].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2020_04]
	--IF((@StartingDate >= '2020-04-01' AND @StartingDate < '2020-05-01') OR (@EndingDate >= '2020-04-01' AND @EndingDate < '2020-05-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_04].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_05]
	--IF((@StartingDate >= '2020-05-01' AND @StartingDate < '2020-06-01') OR (@EndingDate >= '2020-05-01' AND @EndingDate < '2020-06-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_05].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_06]
	--IF((@StartingDate >= '2020-06-01' AND @StartingDate < '2020-07-01') OR (@EndingDate >= '2020-06-01' AND @EndingDate < '2020-07-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_06].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_07]
	--IF((@StartingDate >= '2020-07-01' AND @StartingDate < '2020-08-01') OR (@EndingDate >= '2020-07-01' AND @EndingDate < '2020-08-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_07].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_08]
	--IF((@StartingDate >= '2020-08-01' AND @StartingDate < '2020-09-01') OR (@EndingDate >= '2020-08-01' AND @EndingDate < '2020-09-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_08].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_09]
	--IF((@StartingDate >= '2020-09-01' AND @StartingDate < '2020-10-01') OR (@EndingDate >= '2020-09-01' AND @EndingDate < '2020-10-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_09].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_10]
	--IF((@StartingDate >= '2020-10-01' AND @StartingDate < '2020-11-01') OR (@EndingDate >= '2020-10-01' AND @EndingDate < '2020-11-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_10].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2020_11]
	--IF((@StartingDate >= '2020-11-01' AND @StartingDate < '2020-12-01') OR (@EndingDate >= '2020-11-01' AND @EndingDate < '2020-12-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_11].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_12]
	--IF((@StartingDate >= '2020-12-01' AND @StartingDate < '2021-01-01') OR (@EndingDate >= '2020-12-01' AND @EndingDate < '2021-01-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_12].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_01]
	--IF((@StartingDate >= '2021-01-01' AND @StartingDate < '2021-02-01') OR (@EndingDate >= '2021-01-01' AND @EndingDate < '2021-02-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_01].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_02]
	--IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_02].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_03]
	--IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_03].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_04]
	--IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_04].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_05]
	--IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_05].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_06]
	--IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_06].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_07]
	--IF((@StartingDate >= '2021-07-01' AND @StartingDate < '2021-08-01') OR (@EndingDate >= '2021-07-01' AND @EndingDate < '2021-08-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_07].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_08]
	--IF((@StartingDate >= '2021-08-01' AND @StartingDate < '2021-09-01') OR (@EndingDate >= '2021-08-01' AND @EndingDate < '2021-09-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_08].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_09]
	--IF((@StartingDate >= '2021-09-01' AND @StartingDate < '2021-10-01') OR (@EndingDate >= '2021-09-01' AND @EndingDate < '2021-10-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_09].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_10]
	--IF((@StartingDate >= '2021-10-01' AND @StartingDate < '2021-11-01') OR (@EndingDate >= '2021-10-01' AND @EndingDate < '2021-11-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_10].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_11]
	--IF((@StartingDate >= '2021-11-01' AND @StartingDate < '2021-12-01') OR (@EndingDate >= '2021-11-01' AND @EndingDate < '2021-12-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_11].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_12]
	--IF((@StartingDate >= '2021-12-01' AND @StartingDate < '2022-01-01') OR (@EndingDate >= '2021-12-01' AND @EndingDate < '2022-01-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_12].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_01]
	--IF((@StartingDate >= '2022-01-01' AND @StartingDate < '2022-02-01') OR (@EndingDate >= '2022-01-01' AND @EndingDate < '2022-02-01'))	
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2022_01].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2022_02]
	--IF((@StartingDate >= '2022-02-01' AND @StartingDate < '2022-03-01') OR (@EndingDate >= '2022-02-01' AND @EndingDate < '2022-03-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2022_02].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2022_03]
	--IF((@StartingDate >= '2022-03-01' AND @StartingDate < '2022-04-01') OR (@EndingDate >= '2022-03-01' AND @EndingDate < '2022-04-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2022_03].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_04]
	--IF((@StartingDate >= '2022-04-01' AND @StartingDate < '2022-05-01') OR (@EndingDate >= '2022-04-01' AND @EndingDate < '2022-05-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2022_04].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
		
	----#[3rdEyE_TrackingDataBase_2022_05]
	--IF((@StartingDate >= '2022-05-01' AND @StartingDate < '2022-06-01') OR (@EndingDate >= '2022-05-01' AND @EndingDate < '2022-06-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2022_05].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2022_06]
	--IF((@StartingDate >= '2022-06-01' AND @StartingDate < '2022-07-01') OR (@EndingDate >= '2022-06-01' AND @EndingDate < '2022-07-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2022_06].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
	--	) t
	--END
	
	--#[3rdEyE_TrackingDataBase_2022_07]
	IF((@StartingDate >= '2022-07-01' AND @StartingDate < '2022-08-01') OR (@EndingDate >= '2022-07-01' AND @EndingDate < '2022-08-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_07].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END

	
	--#[3rdEyE_TrackingDataBase_2022_08]
	IF((@StartingDate >= '2022-08-01' AND @StartingDate < '2022-09-01') OR (@EndingDate >= '2022-08-01' AND @EndingDate < '2022-09-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_08].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END

		
	--#[3rdEyE_TrackingDataBase_2022_09]
	IF((@StartingDate >= '2022-09-01' AND @StartingDate < '2022-10-01') OR (@EndingDate >= '2022-09-01' AND @EndingDate < '2022-10-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_09].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END

			
	--#[3rdEyE_TrackingDataBase_2022_10]
	IF((@StartingDate >= '2022-10-01' AND @StartingDate < '2022-11-01') OR (@EndingDate >= '2022-10-01' AND @EndingDate < '2022-11-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_10].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END
	
			
	--#[3rdEyE_TrackingDataBase_2022_11]
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01') OR (@EndingDate >= '2022-11-01' AND @EndingDate < '2022-12-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_11].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END
		
			
	--#[3rdEyE_TrackingDataBase_2022_12]
	IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01') OR (@EndingDate >= '2022-12-01' AND @EndingDate < '2023-01-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_11].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END
END

-- EXEC Report_GetVehicleConsolidatedReport_ReadyReport  '86AA5713-08A9-4CC3-AFCB-4541065A75B9', '2019-03-02';
GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleHaltTime]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleHaltTime](
@USER_KEY varchar(50),
@FK_Vehicle uniqueidentifier,
@StartingDate datetime,
@EndingDate datetime,
@MininumMinuteDealy int
)
AS
BEGIN
--###################################### H E A D ##########################################
	--CLEAR DATA TABLE
	DELETE FROM Report_VehicleHaltReport WHERE USER_KEY = @USER_KEY;  
	DELETE FROM Report_VehicleHaltReport_Helper WHERE USER_KEY = @USER_KEY;  
	
	
	
	
	--Vehicle registation number
	DECLARE @_VehicleRegistrationNumber [varchar](50);
	SET @_VehicleRegistrationNumber = (SELECT TOP 1 RegistrationNumber FROM [3rdEyE].dbo.Vehicle
	WHERE PK_Vehicle = @FK_Vehicle);

	DECLARE @HaltTime BIGINT=0;
	DECLARE @TotalHaltTime BIGINT=0;
	DECLARE @HaltCount BIGINT=0;

	-- COMMON VARIABLES
	DECLARE @first_id BIGINT = 0;
	Declare @first_UpdateTime DATETIME;
	Declare @min_ID BIGINT;
	DECLARE @max_ID BIGINT;
	DECLARE @standingState_id BIGINT=0;
	DECLARE @runningState_id BIGINT=0;
	DECLARE @standingState_DateTime datetime;
	DECLARE @runningState_DateTime datetime;
	DECLARE @_Latitude [varchar](300);
	DECLARE @_Longitude [varchar](300);
	DECLARE @_EngineStatus [varchar](300);
	DECLARE @_Speed [varchar](300);
	DECLARE @_NearestMapLocation [varchar](300);
	DECLARE @_NearestMapLocationDistance [varchar](300);

--###################################### B O D Y ##########################################

	----#[3rdEyE_TrackingDataBase_2018_09]
	--IF((@StartingDate >= '2018-09-01' AND @StartingDate < '2018-10-01') OR (@EndingDate >= '2018-09-01' AND @EndingDate < '2018-10-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_10]
	--IF((@StartingDate >= '2018-10-01' AND @StartingDate < '2018-11-01') OR (@EndingDate >= '2018-10-01' AND @EndingDate < '2018-11-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2018_11]
	--IF((@StartingDate >= '2018-11-01' AND @StartingDate < '2018-12-01') OR (@EndingDate >= '2018-11-01' AND @EndingDate < '2018-12-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_12]
	--IF((@StartingDate >= '2018-12-01' AND @StartingDate < '2019-01-01') OR (@EndingDate >= '2018-12-01' AND @EndingDate < '2019-01-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_01]
	--IF((@StartingDate >= '2019-01-01' AND @StartingDate < '2019-02-01') OR (@EndingDate >= '2019-01-01' AND @EndingDate < '2019-02-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_02]
	--IF((@StartingDate >= '2019-02-01' AND @StartingDate < '2019-03-01') OR (@EndingDate >= '2019-02-01' AND @EndingDate < '2019-03-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_03]
	--IF((@StartingDate >= '2019-03-01' AND @StartingDate < '2019-04-01') OR (@EndingDate >= '2019-03-01' AND @EndingDate < '2019-04-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_04]
	--IF((@StartingDate >= '2019-04-01' AND @StartingDate < '2019-05-01') OR (@EndingDate >= '2019-04-01' AND @EndingDate < '2019-05-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_05]
	--IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_06]
	--IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_07]
	--IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_08]
	--IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_09]
	--IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_10]
	--IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_11]
	--IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_12]
	--IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_01]
	--IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2020_02]
	--IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_03]
	--IF((@StartingDate >= '2020-03-01' AND @StartingDate < '2020-04-01') OR (@EndingDate >= '2020-03-01' AND @EndingDate < '2020-04-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_04]
	--IF((@StartingDate >= '2020-04-01' AND @StartingDate < '2020-05-01') OR (@EndingDate >= '2020-04-01' AND @EndingDate < '2020-05-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_05]
	--IF((@StartingDate >= '2020-05-01' AND @StartingDate < '2020-06-01') OR (@EndingDate >= '2020-05-01' AND @EndingDate < '2020-06-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_06]
	--IF((@StartingDate >= '2020-06-01' AND @StartingDate < '2020-07-01') OR (@EndingDate >= '2020-06-01' AND @EndingDate < '2020-07-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_07]
	--IF((@StartingDate >= '2020-07-01' AND @StartingDate < '2020-08-01') OR (@EndingDate >= '2020-07-01' AND @EndingDate < '2020-08-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_08]
	--IF((@StartingDate >= '2020-08-01' AND @StartingDate < '2020-09-01') OR (@EndingDate >= '2020-08-01' AND @EndingDate < '2020-09-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_09]
	--IF((@StartingDate >= '2020-09-01' AND @StartingDate < '2020-10-01') OR (@EndingDate >= '2020-09-01' AND @EndingDate < '2020-10-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_10]
	--IF((@StartingDate >= '2020-10-01' AND @StartingDate < '2020-11-01') OR (@EndingDate >= '2020-10-01' AND @EndingDate < '2020-11-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2020_11]
	--IF((@StartingDate >= '2020-11-01' AND @StartingDate < '2020-12-01') OR (@EndingDate >= '2020-11-01' AND @EndingDate < '2020-12-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_12]
	--IF((@StartingDate >= '2020-12-01' AND @StartingDate < '2021-01-01') OR (@EndingDate >= '2020-12-01' AND @EndingDate < '2021-01-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_01]
	--IF((@StartingDate >= '2021-01-01' AND @StartingDate < '2021-02-01') OR (@EndingDate >= '2021-01-01' AND @EndingDate < '2021-02-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_02]
	--IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_03]
	--IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_04]
	--IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_05]
	--IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_06]
	--IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_07]
	--IF((@StartingDate >= '2021-07-01' AND @StartingDate < '2021-08-01') OR (@EndingDate >= '2021-07-01' AND @EndingDate < '2021-08-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_08]
	--IF((@StartingDate >= '2021-08-01' AND @StartingDate < '2021-09-01') OR (@EndingDate >= '2021-08-01' AND @EndingDate < '2021-09-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_09]
	--IF((@StartingDate >= '2021-09-01' AND @StartingDate < '2021-10-01') OR (@EndingDate >= '2021-09-01' AND @EndingDate < '2021-10-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_10]
	--IF((@StartingDate >= '2021-10-01' AND @StartingDate < '2021-11-01') OR (@EndingDate >= '2021-10-01' AND @EndingDate < '2021-11-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_11]
	--IF((@StartingDate >= '2021-11-01' AND @StartingDate < '2021-12-01') OR (@EndingDate >= '2021-11-01' AND @EndingDate < '2021-12-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_12]
	--IF((@StartingDate >= '2021-12-01' AND @StartingDate < '2022-01-01') OR (@EndingDate >= '2021-12-01' AND @EndingDate < '2022-01-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_01]
	--IF((@StartingDate >= '2022-01-01' AND @StartingDate < '2022-02-01') OR (@EndingDate >= '2022-01-01' AND @EndingDate < '2022-02-01'))	
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_02]
	--IF((@StartingDate >= '2022-02-01' AND @StartingDate < '2022-03-01') OR (@EndingDate >= '2022-02-01' AND @EndingDate < '2022-03-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_03]
	--IF((@StartingDate >= '2022-03-01' AND @StartingDate < '2022-04-01') OR (@EndingDate >= '2022-03-01' AND @EndingDate < '2022-04-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_04]
	--IF((@StartingDate >= '2022-04-01' AND @StartingDate < '2022-05-01') OR (@EndingDate >= '2022-04-01' AND @EndingDate < '2022-05-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_05]
	--IF((@StartingDate >= '2022-05-01' AND @StartingDate < '2022-06-01') OR (@EndingDate >= '2022-05-01' AND @EndingDate < '2022-06-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_06]
	--IF((@StartingDate >= '2022-06-01' AND @StartingDate < '2022-07-01') OR (@EndingDate >= '2022-06-01' AND @EndingDate < '2022-07-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_07]
	--IF((@StartingDate >= '2022-07-01' AND @StartingDate < '2022-08-01') OR (@EndingDate >= '2022-07-01' AND @EndingDate < '2022-08-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_08]
	--IF((@StartingDate >= '2022-08-01' AND @StartingDate < '2022-09-01') OR (@EndingDate >= '2022-08-01' AND @EndingDate < '2022-09-01'))
	--BEGIN
	--	Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	--#[3rdEyE_TrackingDataBase_2022_09]
	IF((@StartingDate >= '2022-09-01' AND @StartingDate < '2022-10-01') OR (@EndingDate >= '2022-09-01' AND @EndingDate < '2022-10-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_10]
	IF((@StartingDate >= '2022-10-01' AND @StartingDate < '2022-11-01') OR (@EndingDate >= '2022-10-01' AND @EndingDate < '2022-11-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2022_11]
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01') OR (@EndingDate >= '2022-11-01' AND @EndingDate < '2022-12-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_12]
	IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01') OR (@EndingDate >= '2022-12-01' AND @EndingDate < '2023-01-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	BEGIN
		-- RESET COMMON VARIABLES
		SET @first_id = 0;
		SET @min_ID = 0;
		SET @max_ID = 0;
		SET @standingState_id = 0;
		SET @runningState_id = 0;
	
		--first_id calculation and insert
		SELECT TOP 1 @first_id = PK_RowData, @first_UpdateTime = UpdateTime,
		@_NearestMapLocation=(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( Report_VehicleHaltReport_Helper.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( Report_VehicleHaltReport_Helper.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( Report_VehicleHaltReport_Helper.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))),
		@_NearestMapLocationDistance=Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( Report_VehicleHaltReport_Helper.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( Report_VehicleHaltReport_Helper.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( Report_VehicleHaltReport_Helper.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2)
		FROM Report_VehicleHaltReport_Helper
		WHERE USER_KEY = @USER_KEY;
		IF(@first_id != 0)
		BEGIN
			SET @HaltTime = DATEDIFF(mi, @StartingDate, @first_UpdateTime);
			IF(@HaltTime >= @MininumMinuteDealy)
			BEGIN
				SET @TotalHaltTime = @TotalHaltTime + @HaltTime;
				
				INSERT INTO Report_VehicleHaltReport(USER_KEY,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,HaltTime,NearestMapLocation,NearestMapLocationDistance) VALUES(@USER_KEY, 'data_initial_gap', '', '', @_VehicleRegistrationNumber, @StartingDate, @first_UpdateTime, '', '', '', '', @HaltTime, @_NearestMapLocation, @_NearestMapLocationDistance); 
				SET @HaltCount = @HaltCount +1;
			END
		END
		
		--min_DateTime calculation
		SET @min_ID = @first_id;
		--SELECT TOP 1 @min_ID = PK_RowData FROM Report_VehicleHaltReport_Helper 
		--WHERE USER_KEY = @USER_KEY
		--AND EngineStatus = '0'
		--AND Speed = 0;
	
		--max_Datetime calculation
		SET @max_ID = 0;
		SELECT TOP 1 @max_ID = PK_RowData FROM Report_VehicleHaltReport_Helper 
		WHERE USER_KEY = @USER_KEY
		ORDER BY PK_RowData DESC;
		
		--INSERT 
		WHILE(@min_ID != 0 AND @max_ID != 0 AND @min_ID < @max_ID)
		BEGIN

			--STAT 0
			SET @standingState_id = 0;
			SET @standingState_DateTime = '2000-01-01';
			SELECT TOP 1 
			@standingState_id = PK_RowData, 
			@standingState_DateTime = UpdateTime,
			@_Latitude= Latitude,
			@_Longitude= Longitude,
			@_EngineStatus= EngineStatus,
			@_Speed= Speed,
			@_NearestMapLocation=(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( Report_VehicleHaltReport_Helper.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( Report_VehicleHaltReport_Helper.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( Report_VehicleHaltReport_Helper.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))),
			@_NearestMapLocationDistance=Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( Report_VehicleHaltReport_Helper.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( Report_VehicleHaltReport_Helper.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( Report_VehicleHaltReport_Helper.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2)
			FROM Report_VehicleHaltReport_Helper 
			WHERE USER_KEY = @USER_KEY AND PK_RowData >= @min_ID AND EngineStatus = '0' AND Speed = 0;

			IF(@standingState_id != 0)
			BEGIN
				SET @min_ID = @standingState_id + 1;
			END
			ELSE
			BEGIN
				BREAK;
			END

			--STAT 1
			SET @runningState_id = 0;
			SET @runningState_DateTime = '2000-01-01';
			SELECT TOP 1 
			@runningState_id = PK_RowData, 
			@runningState_DateTime = UpdateTime
			FROM Report_VehicleHaltReport_Helper 
			WHERE USER_KEY = @USER_KEY AND PK_RoWData >= @min_ID AND PK_RowData <= @max_ID AND (EngineStatus = '1' AND Speed > 0);

			IF(@runningState_id != 0)
			BEGIN
				SET @min_ID = @runningState_id + 1;

				SET @HaltTime = DATEDIFF(mi, @standingState_DateTime, @runningState_DateTime);
				IF(@HaltTime >= @MininumMinuteDealy)
				BEGIN
					SET @TotalHaltTime = @TotalHaltTime + @HaltTime;
					INSERT INTO Report_VehicleHaltReport(USER_KEY,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,HaltTime,NearestMapLocation,NearestMapLocationDistance) VALUES(@USER_KEY, 'data', convert(varchar(50),@standingState_id), convert(varchar(50),@runningState_id), @_VehicleRegistrationNumber, @standingState_DateTime, @runningState_DateTime, @_Latitude, @_Longitude,@_EngineStatus, @_Speed, @HaltTime, @_NearestMapLocation, @_NearestMapLocationDistance); 
					SET @HaltCount = @HaltCount +1;
				END
			END
			ELSE
			BEGIN
				--# SET gap time to last update time/ ending time
				SET @runningState_DateTime = @EndingDate;----Uncomment this to calculate untill last momment

				SET @HaltTime = DATEDIFF(mi, @standingState_DateTime, @runningState_DateTime); 
			
				IF(@HaltTime >= @MininumMinuteDealy)
				BEGIN 
					SET @TotalHaltTime = @TotalHaltTime + @HaltTime;
					INSERT INTO Report_VehicleHaltReport(USER_KEY,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,HaltTime,NearestMapLocation,NearestMapLocationDistance) VALUES(@USER_KEY, 'data_finishing_gap', convert(varchar(50),@standingState_id), '', @_VehicleRegistrationNumber, @standingState_DateTime, @runningState_DateTime, @_Latitude, @_Longitude,@_EngineStatus, @_Speed, @HaltTime, @_NearestMapLocation, @_NearestMapLocationDistance); 
					SET @HaltCount = @HaltCount +1;
				END
				BREAK;
			END
		END
	END



--###################################### T A I L ##########################################
	
	--SELECT
	SELECT * FROM Report_VehicleHaltReport WHERE USER_KEY = @USER_KEY; 
	
END





--  EXEC _Report_GetVehicleHaltTime '00000000-0000-0000-0000-000000000000', '087505d5-6d8c-4411-a80b-5fa105e13579', '2018-10-08 12:00:00 AM', '2018-10-09 12:00:00 AM', '5';

GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleHaltTime_ReadyReport]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleHaltTime_ReadyReport](
@FK_Vehicle uniqueidentifier,
@StartingDate datetime,
@EndingDate datetime,
@MininumMinuteDealy int
)
AS
BEGIN
	----#[3rdEyE_TrackingDataBase_2019_03]
	--IF((@StartingDate >= '2019-03-01' AND @StartingDate < '2019-04-01') AND (@EndingDate >= '2019-03-01' AND @EndingDate < '2019-04-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_03].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2019_04]
	--IF((@StartingDate >= '2019-04-01' AND @StartingDate < '2019-05-01') AND (@EndingDate >= '2019-04-01' AND @EndingDate < '2019-05-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_04].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2019_05]
	--IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_05].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_06]
	--IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_06].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_07]
	--IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_07].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2019_08]
	--IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_08].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_09]
	--IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_09].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_10]
	--IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_10].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_11]
	--IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_11].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_12]
	--IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2019_12].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_01]
	--IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_01].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2020_02]
	--IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_02].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_03]
	--IF((@StartingDate >= '2020-03-01' AND @StartingDate < '2020-04-01') OR (@EndingDate >= '2020-03-01' AND @EndingDate < '2020-04-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_03].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_04]
	--IF((@StartingDate >= '2020-04-01' AND @StartingDate < '2020-05-01') OR (@EndingDate >= '2020-04-01' AND @EndingDate < '2020-05-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_04].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_05]
	--IF((@StartingDate >= '2020-05-01' AND @StartingDate < '2020-06-01') OR (@EndingDate >= '2020-05-01' AND @EndingDate < '2020-06-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_05].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_06]
	--IF((@StartingDate >= '2020-06-01' AND @StartingDate < '2020-07-01') OR (@EndingDate >= '2020-06-01' AND @EndingDate < '2020-07-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_06].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_07]
	--IF((@StartingDate >= '2020-07-01' AND @StartingDate < '2020-08-01') OR (@EndingDate >= '2020-07-01' AND @EndingDate < '2020-08-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_07].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_08]
	--IF((@StartingDate >= '2020-08-01' AND @StartingDate < '2020-09-01') OR (@EndingDate >= '2020-08-01' AND @EndingDate < '2020-09-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_08].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_09]
	--IF((@StartingDate >= '2020-09-01' AND @StartingDate < '2020-10-01') OR (@EndingDate >= '2020-09-01' AND @EndingDate < '2020-10-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_09].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_10]
	--IF((@StartingDate >= '2020-10-01' AND @StartingDate < '2020-11-01') OR (@EndingDate >= '2020-10-01' AND @EndingDate < '2020-11-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_10].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_11]
	--IF((@StartingDate >= '2020-11-01' AND @StartingDate < '2020-12-01') OR (@EndingDate >= '2020-11-01' AND @EndingDate < '2020-12-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_11].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_12]
	--IF((@StartingDate >= '2020-12-01' AND @StartingDate < '2021-01-01') OR (@EndingDate >= '2020-12-01' AND @EndingDate < '2021-01-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2020_12].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_01]
	--IF((@StartingDate >= '2021-01-01' AND @StartingDate < '2021-02-01') OR (@EndingDate >= '2021-01-01' AND @EndingDate < '2021-02-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_01].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_02]
	--IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_02].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_03]
	--IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_03].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_04]
	--IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_04].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_05]
	--IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_05].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_06]
	--IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_06].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
		
	----#[3rdEyE_TrackingDataBase_2021_07]
	--IF((@StartingDate >= '2021-07-01' AND @StartingDate < '2021-08-01') OR (@EndingDate >= '2021-07-01' AND @EndingDate < '2021-08-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_07].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_08]
	--IF((@StartingDate >= '2021-08-01' AND @StartingDate < '2021-09-01') OR (@EndingDate >= '2021-08-01' AND @EndingDate < '2021-09-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_08].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_09]
	--IF((@StartingDate >= '2021-09-01' AND @StartingDate < '2021-10-01') OR (@EndingDate >= '2021-09-01' AND @EndingDate < '2021-10-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_09].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2021_10]
	--IF((@StartingDate >= '2021-10-01' AND @StartingDate < '2021-11-01') OR (@EndingDate >= '2021-10-01' AND @EndingDate < '2021-11-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_10].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_11]
	--IF((@StartingDate >= '2021-11-01' AND @StartingDate < '2021-12-01') OR (@EndingDate >= '2021-11-01' AND @EndingDate < '2021-12-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_11].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_12]
	--IF((@StartingDate >= '2021-12-01' AND @StartingDate < '2022-01-01') OR (@EndingDate >= '2021-12-01' AND @EndingDate < '2022-01-01'))
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2021_12].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END

	----#[3rdEyE_TrackingDataBase_2022_01]
	--IF((@StartingDate >= '2022-01-01' AND @StartingDate < '2022-02-01') OR (@EndingDate >= '2022-01-01' AND @EndingDate < '2022-02-01'))	
	--BEGIN
	--	SELECT
	--	t.*, 
	--	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	--	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	--	FROM(
	--	select Report_VehicleHaltReport.* 
	--		from [3rdEyE_TrackingDataBase_2022_01].dbo.Report_VehicleHaltReport 
	--		where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
	--	) t
	--END
	
	--#[3rdEyE_TrackingDataBase_2022_02]
	IF((@StartingDate >= '2022-02-01' AND @StartingDate < '2022-03-01') OR (@EndingDate >= '2022-02-01' AND @EndingDate < '2022-03-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_02].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END

	--#[3rdEyE_TrackingDataBase_2022_03]
	IF((@StartingDate >= '2022-03-01' AND @StartingDate < '2022-04-01') OR (@EndingDate >= '2022-03-01' AND @EndingDate < '2022-04-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_03].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END
	
	--#[3rdEyE_TrackingDataBase_2022_04]
	IF((@StartingDate >= '2022-04-01' AND @StartingDate < '2022-05-01') OR (@EndingDate >= '2022-04-01' AND @EndingDate < '2022-05-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_04].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END

	--#[3rdEyE_TrackingDataBase_2022_05]
	IF((@StartingDate >= '2022-05-01' AND @StartingDate < '2022-06-01') OR (@EndingDate >= '2022-05-01' AND @EndingDate < '2022-06-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_05].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END
	
	--#[3rdEyE_TrackingDataBase_2022_06]
	IF((@StartingDate >= '2022-06-01' AND @StartingDate < '2022-07-01') OR (@EndingDate >= '2022-06-01' AND @EndingDate < '2022-07-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_06].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END

	--#[3rdEyE_TrackingDataBase_2022_07]
	IF((@StartingDate >= '2022-07-01' AND @StartingDate < '2022-08-01') OR (@EndingDate >= '2022-07-01' AND @EndingDate < '2022-08-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_07].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END

	--#[3rdEyE_TrackingDataBase_2022_08]
	IF((@StartingDate >= '2022-08-01' AND @StartingDate < '2022-09-01') OR (@EndingDate >= '2022-08-01' AND @EndingDate < '2022-09-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_08].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END

	--#[3rdEyE_TrackingDataBase_2022_09]
	IF((@StartingDate >= '2022-09-01' AND @StartingDate < '2022-10-01') OR (@EndingDate >= '2022-09-01' AND @EndingDate < '2022-10-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_09].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END

	
	--#[3rdEyE_TrackingDataBase_2022_10]
	IF((@StartingDate >= '2022-10-01' AND @StartingDate < '2022-11-01') OR (@EndingDate >= '2022-10-01' AND @EndingDate < '2022-11-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_10].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END
	
	
	--#[3rdEyE_TrackingDataBase_2022_11]
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01') OR (@EndingDate >= '2022-11-01' AND @EndingDate < '2022-12-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_11].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END
		
	
	--#[3rdEyE_TrackingDataBase_2022_12]
	IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01') OR (@EndingDate >= '2022-12-01' AND @EndingDate < '2023-01-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2022_12].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END
END

--  EXEC Report_GetVehicleHaltTime_ReadyReport '0bdc0f82-d45b-4d9e-8c05-7032f36e0478', '01-Apr-19 12:00:00 AM', '02-Apr-19 12:00:00 AM', '5';


GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleHistory]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleHistory] (
@USER_KEY varchar(50),
@FK_Vehicle uniqueidentifier,
@StartingDate datetime,
@EndingDate datetime,
@TimeLapMinute int
)
AS
BEGIN  
	delete from Report_VehicleHistory  where USER_KEY = @USER_KEY;
	
	IF(@EndingDate>GETDATE())
	BEGIN
		SET @EndingDate = GETDATE();
	END

	----[3rdEyE_TrackingDataBase_2018_09]
	--IF((@StartingDate >= '2018-09-01' AND @StartingDate < '2018-10-01') OR (@EndingDate >= '2018-09-01' AND @EndingDate < '2018-10-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----[3rdEyE_TrackingDataBase_2018_10]
	--IF((@StartingDate >= '2018-10-01' AND @StartingDate < '2018-11-01') OR (@EndingDate >= '2018-10-01' AND @EndingDate < '2018-11-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----[3rdEyE_TrackingDataBase_2018_11]
	--IF((@StartingDate >= '2018-11-01' AND @StartingDate < '2018-12-01') OR (@EndingDate >= '2018-11-01' AND @EndingDate < '2018-12-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----[3rdEyE_TrackingDataBase_2018_12]
	--IF((@StartingDate >= '2018-12-01' AND @StartingDate < '2019-01-01') OR (@EndingDate >= '2018-12-01' AND @EndingDate < '2019-01-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	
	----[3rdEyE_TrackingDataBase_2019_01]
	--IF((@StartingDate >= '2019-01-01' AND @StartingDate < '2019-02-01') OR (@EndingDate >= '2019-01-01' AND @EndingDate < '2019-02-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END


	----[3rdEyE_TrackingDataBase_2019_02]
	--IF((@StartingDate >= '2019-02-01' AND @StartingDate < '2019-03-01') OR (@EndingDate >= '2019-02-01' AND @EndingDate < '2019-03-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2019_03]
	--IF((@StartingDate >= '2019-03-01' AND @StartingDate < '2019-04-01') OR (@EndingDate >= '2019-03-01' AND @EndingDate < '2019-04-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END


	----#[3rdEyE_TrackingDataBase_2019_04]
	--IF((@StartingDate >= '2019-04-01' AND @StartingDate < '2019-05-01') OR (@EndingDate >= '2019-04-01' AND @EndingDate < '2019-05-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END


	----#[3rdEyE_TrackingDataBase_2019_05]
	--IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END


	----#[3rdEyE_TrackingDataBase_2019_06]
	--IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2019_07]
	--IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2019_08]
	--IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2019_09]
	--IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2019_10]
	--IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2019_11]
	--IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2019_12]
	--IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2020_01]
	--IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2020_02]
	--IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2020_03]
	--IF((@StartingDate >= '2020-03-01' AND @StartingDate < '2020-04-01') OR (@EndingDate >= '2020-03-01' AND @EndingDate < '2020-04-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END


	----#[3rdEyE_TrackingDataBase_2020_04]
	--IF((@StartingDate >= '2020-04-01' AND @StartingDate < '2020-05-01') OR (@EndingDate >= '2020-04-01' AND @EndingDate < '2020-05-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2020_05]
	--IF((@StartingDate >= '2020-05-01' AND @StartingDate < '2020-06-01') OR (@EndingDate >= '2020-05-01' AND @EndingDate < '2020-06-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	
	----#[3rdEyE_TrackingDataBase_2020_06]
	--IF((@StartingDate >= '2020-06-01' AND @StartingDate < '2020-07-01') OR (@EndingDate >= '2020-06-01' AND @EndingDate < '2020-07-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	
	
	----#[3rdEyE_TrackingDataBase_2020_07]
	--IF((@StartingDate >= '2020-07-01' AND @StartingDate < '2020-08-01') OR (@EndingDate >= '2020-07-01' AND @EndingDate < '2020-08-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	----#[3rdEyE_TrackingDataBase_2020_08]
	--IF((@StartingDate >= '2020-08-01' AND @StartingDate < '2020-09-01') OR (@EndingDate >= '2020-08-01' AND @EndingDate < '2020-09-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_09]
	--IF((@StartingDate >= '2020-09-01' AND @StartingDate < '2020-10-01') OR (@EndingDate >= '2020-09-01' AND @EndingDate < '2020-10-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
		
	----#[3rdEyE_TrackingDataBase_2020_10]
	--IF((@StartingDate >= '2020-10-01' AND @StartingDate < '2020-11-01') OR (@EndingDate >= '2020-10-01' AND @EndingDate < '2020-11-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
			
	----#[3rdEyE_TrackingDataBase_2020_11]
	--IF((@StartingDate >= '2020-11-01' AND @StartingDate < '2020-12-01') OR (@EndingDate >= '2020-11-01' AND @EndingDate < '2020-12-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
				
	----#[3rdEyE_TrackingDataBase_2020_12]
	--IF((@StartingDate >= '2020-12-01' AND @StartingDate < '2021-01-01') OR (@EndingDate >= '2020-12-01' AND @EndingDate < '2021-01-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
				
	----#[3rdEyE_TrackingDataBase_2021_01]
	--IF((@StartingDate >= '2021-01-01' AND @StartingDate < '2021-02-01') OR (@EndingDate >= '2021-01-01' AND @EndingDate < '2021-02-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	----#[3rdEyE_TrackingDataBase_2021_02]
	--IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	----#[3rdEyE_TrackingDataBase_2021_03]
	--IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_04]
	--IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	----#[3rdEyE_TrackingDataBase_2021_05]
	--IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	----#[3rdEyE_TrackingDataBase_2021_06]
	--IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	----#[3rdEyE_TrackingDataBase_2021_07]
	--IF((@StartingDate >= '2021-07-01' AND @StartingDate < '2021-08-01') OR (@EndingDate >= '2021-07-01' AND @EndingDate < '2021-08-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	----#[3rdEyE_TrackingDataBase_2021_08]
	--IF((@StartingDate >= '2021-08-01' AND @StartingDate < '2021-09-01') OR (@EndingDate >= '2021-08-01' AND @EndingDate < '2021-09-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	----#[3rdEyE_TrackingDataBase_2021_09]
	--IF((@StartingDate >= '2021-09-01' AND @StartingDate < '2021-10-01') OR (@EndingDate >= '2021-09-01' AND @EndingDate < '2021-10-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	
	----#[3rdEyE_TrackingDataBase_2021_10]
	--IF((@StartingDate >= '2021-10-01' AND @StartingDate < '2021-11-01') OR (@EndingDate >= '2021-10-01' AND @EndingDate < '2021-11-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

		
	----#[3rdEyE_TrackingDataBase_2021_11]
	--IF((@StartingDate >= '2021-11-01' AND @StartingDate < '2021-12-01') OR (@EndingDate >= '2021-11-01' AND @EndingDate < '2021-12-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	
		
	----#[3rdEyE_TrackingDataBase_2021_12]
	--IF((@StartingDate >= '2021-12-01' AND @StartingDate < '2022-01-01') OR (@EndingDate >= '2021-12-01' AND @EndingDate < '2022-01-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
		
		
	----#[3rdEyE_TrackingDataBase_2022_01]
	--IF((@StartingDate >= '2022-01-01' AND @StartingDate < '2022-02-01') OR (@EndingDate >= '2022-01-01' AND @EndingDate < '2022-02-01'))	
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END


	----#[3rdEyE_TrackingDataBase_2022_02]
	--IF((@StartingDate >= '2022-02-01' AND @StartingDate < '2022-03-01') OR (@EndingDate >= '2022-02-01' AND @EndingDate < '2022-03-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2022_03]
	--IF((@StartingDate >= '2022-03-01' AND @StartingDate < '2022-04-01') OR (@EndingDate >= '2022-03-01' AND @EndingDate < '2022-04-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END


	----#[3rdEyE_TrackingDataBase_2022_04]
	--IF((@StartingDate >= '2022-04-01' AND @StartingDate < '2022-05-01') OR (@EndingDate >= '2022-04-01' AND @EndingDate < '2022-05-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END
	

	----#[3rdEyE_TrackingDataBase_2022_05]
	--IF((@StartingDate >= '2022-05-01' AND @StartingDate < '2022-06-01') OR (@EndingDate >= '2022-05-01' AND @EndingDate < '2022-06-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

	
	----#[3rdEyE_TrackingDataBase_2022_06]
	--IF((@StartingDate >= '2022-06-01' AND @StartingDate < '2022-07-01') OR (@EndingDate >= '2022-06-01' AND @EndingDate < '2022-07-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END

		
	----#[3rdEyE_TrackingDataBase_2022_07]
	--IF((@StartingDate >= '2022-07-01' AND @StartingDate < '2022-08-01') OR (@EndingDate >= '2022-07-01' AND @EndingDate < '2022-08-01'))
	--BEGIN
	
	--INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	--SELECT
	--	@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
	--	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
	--	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	--FROM
	--	(
	--	select
	--	PK_RowData = MIN(PK_RowData)
	--	FROM [3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
	--	GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
	--	) AS JoinedTable
	--INNER JOIN
	--[3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData
	--ON
	--[3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	--[3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	--ORDER BY UpdateTime ASC;
	--END


	--#[3rdEyE_TrackingDataBase_2022_08]
	IF((@StartingDate >= '2022-08-01' AND @StartingDate < '2022-09-01') OR (@EndingDate >= '2022-08-01' AND @EndingDate < '2022-09-01'))
	BEGIN
	
	INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	SELECT
		@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
		(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
		Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	FROM
		(
		select
		PK_RowData = MIN(PK_RowData)
		FROM [3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END


	--#[3rdEyE_TrackingDataBase_2022_09]
	IF((@StartingDate >= '2022-09-01' AND @StartingDate < '2022-10-01') OR (@EndingDate >= '2022-09-01' AND @EndingDate < '2022-10-01'))
	BEGIN
	
	INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	SELECT
		@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
		(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
		Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	FROM
		(
		select
		PK_RowData = MIN(PK_RowData)
		FROM [3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END

	
	--#[3rdEyE_TrackingDataBase_2022_10]
	IF((@StartingDate >= '2022-10-01' AND @StartingDate < '2022-11-01') OR (@EndingDate >= '2022-10-01' AND @EndingDate < '2022-11-01'))
	BEGIN
	
	INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	SELECT
		@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
		(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
		Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	FROM
		(
		select
		PK_RowData = MIN(PK_RowData)
		FROM [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END

	--#[3rdEyE_TrackingDataBase_2022_11]
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01') OR (@EndingDate >= '2022-11-01' AND @EndingDate < '2022-12-01'))
	BEGIN
	
	INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	SELECT
		@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
		(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
		Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	FROM
		(
		select
		PK_RowData = MIN(PK_RowData)
		FROM [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END


	--#[3rdEyE_TrackingDataBase_2022_12]
	IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01') OR (@EndingDate >= '2022-12-01' AND @EndingDate < '2023-01-01'))
	BEGIN
	
	INSERT INTO Report_VehicleHistory (USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,NearestMapLocation,NearestMapLocationDistance)
	SELECT
		@USER_KEY,UpdateTime,Latitude,Longitude,EngineStatus,Speed,
		(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as NearestMapLocation,
		Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as	NearestMapLocationDistance
	FROM
		(
		select
		PK_RowData = MIN(PK_RowData)
		FROM [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END

	--# Final Selection
	SELECT @FK_Vehicle as FK_Vehicle, Report_VehicleHistory.* FROM Report_VehicleHistory WHERE USER_KEY = @USER_KEY;
	
END  

-- EXEC Report_GetVehicleHistory '00000000-0000-0000-0000-000000000000', '1a4a363f-591b-473c-9ed6-04bf7a4673ef', '26-Apr-20 1:13:00 PM', '26-Apr-20 2:05:00 PM', 1

GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleHistoryDetail]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleHistoryDetail] (
@USER_KEY uniqueidentifier,
@FK_Vehicle uniqueidentifier,
@StartingDate datetime,
@EndingDate datetime
)
AS
BEGIN
	
	delete from Report_VehicleHistoryDetail where @USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle;

	----#[3rdEyE_TrackingDataBase_2018_09]
	--IF((@StartingDate >= '2018-09-01' AND @StartingDate < '2018-10-01') OR (@EndingDate >= '2018-09-01' AND @EndingDate < '2018-10-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2018_09].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_10]
	--IF((@StartingDate >= '2018-10-01' AND @StartingDate < '2018-11-01') OR (@EndingDate >= '2018-10-01' AND @EndingDate < '2018-11-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2018_11]
	--IF((@StartingDate >= '2018-11-01' AND @StartingDate < '2018-12-01') OR (@EndingDate >= '2018-11-01' AND @EndingDate < '2018-12-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_12]
	--IF((@StartingDate >= '2018-12-01' AND @StartingDate < '2019-01-01') OR (@EndingDate >= '2018-12-01' AND @EndingDate < '2019-01-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_01]
	--IF((@StartingDate >= '2019-01-01' AND @StartingDate < '2019-02-01') OR (@EndingDate >= '2019-01-01' AND @EndingDate < '2019-02-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_02]
	--IF((@StartingDate >= '2019-02-01' AND @StartingDate < '2019-03-01') OR (@EndingDate >= '2019-02-01' AND @EndingDate < '2019-03-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_03]
	--IF((@StartingDate >= '2019-03-01' AND @StartingDate < '2019-04-01') OR (@EndingDate >= '2019-03-01' AND @EndingDate < '2019-04-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_04]
	--IF((@StartingDate >= '2019-04-01' AND @StartingDate < '2019-05-01') OR (@EndingDate >= '2019-04-01' AND @EndingDate < '2019-05-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_05]
	--IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_06]
	--IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_07]
	--IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_08]
	--IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_09]
	--IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_10]
	--IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_11]
	--IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_12]
	--IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_01]
	--IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_02]
	--IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_03]
	--IF((@StartingDate >= '2020-03-01' AND @StartingDate < '2020-04-01') OR (@EndingDate >= '2020-03-01' AND @EndingDate < '2020-04-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_04]
	--IF((@StartingDate >= '2020-04-01' AND @StartingDate < '2020-05-01') OR (@EndingDate >= '2020-04-01' AND @EndingDate < '2020-05-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_05]
	--IF((@StartingDate >= '2020-05-01' AND @StartingDate < '2020-06-01') OR (@EndingDate >= '2020-05-01' AND @EndingDate < '2020-06-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_06]
	--IF((@StartingDate >= '2020-06-01' AND @StartingDate < '2020-07-01') OR (@EndingDate >= '2020-06-01' AND @EndingDate < '2020-07-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_07]
	--IF((@StartingDate >= '2020-07-01' AND @StartingDate < '2020-08-01') OR (@EndingDate >= '2020-07-01' AND @EndingDate < '2020-08-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_08]
	--IF((@StartingDate >= '2020-08-01' AND @StartingDate < '2020-09-01') OR (@EndingDate >= '2020-08-01' AND @EndingDate < '2020-09-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_09]
	--IF((@StartingDate >= '2020-09-01' AND @StartingDate < '2020-10-01') OR (@EndingDate >= '2020-09-01' AND @EndingDate < '2020-10-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_10]
	--IF((@StartingDate >= '2020-10-01' AND @StartingDate < '2020-11-01') OR (@EndingDate >= '2020-10-01' AND @EndingDate < '2020-11-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_11]
	--IF((@StartingDate >= '2020-11-01' AND @StartingDate < '2020-12-01') OR (@EndingDate >= '2020-11-01' AND @EndingDate < '2020-12-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_12]
	--IF((@StartingDate >= '2020-12-01' AND @StartingDate < '2021-01-01') OR (@EndingDate >= '2020-12-01' AND @EndingDate < '2021-01-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_01]
	--IF((@StartingDate >= '2021-01-01' AND @StartingDate < '2021-02-01') OR (@EndingDate >= '2021-01-01' AND @EndingDate < '2021-02-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_02]
	--IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_03]
	--IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_04]
	--IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_05]
	--IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_06]
	--IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_07]
	--IF((@StartingDate >= '2021-07-01' AND @StartingDate < '2021-08-01') OR (@EndingDate >= '2021-07-01' AND @EndingDate < '2021-08-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_08]
	--IF((@StartingDate >= '2021-08-01' AND @StartingDate < '2021-09-01') OR (@EndingDate >= '2021-08-01' AND @EndingDate < '2021-09-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_09]
	--IF((@StartingDate >= '2021-09-01' AND @StartingDate < '2021-10-01') OR (@EndingDate >= '2021-09-01' AND @EndingDate < '2021-10-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_10]
	--IF((@StartingDate >= '2021-10-01' AND @StartingDate < '2021-11-01') OR (@EndingDate >= '2021-10-01' AND @EndingDate < '2021-11-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_11]
	--IF((@StartingDate >= '2021-11-01' AND @StartingDate < '2021-12-01') OR (@EndingDate >= '2021-11-01' AND @EndingDate < '2021-12-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_12]
	--IF((@StartingDate >= '2021-12-01' AND @StartingDate < '2022-01-01') OR (@EndingDate >= '2021-12-01' AND @EndingDate < '2022-01-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_01]
	--IF((@StartingDate >= '2022-01-01' AND @StartingDate < '2022-02-01') OR (@EndingDate >= '2022-01-01' AND @EndingDate < '2022-02-01'))	
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_02]
	--IF((@StartingDate >= '2022-02-01' AND @StartingDate < '2022-03-01') OR (@EndingDate >= '2022-02-01' AND @EndingDate < '2022-03-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_03]
	--IF((@StartingDate >= '2022-03-01' AND @StartingDate < '2022-04-01') OR (@EndingDate >= '2022-03-01' AND @EndingDate < '2022-04-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_04]
	--IF((@StartingDate >= '2022-04-01' AND @StartingDate < '2022-05-01') OR (@EndingDate >= '2022-04-01' AND @EndingDate < '2022-05-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_05]
	--IF((@StartingDate >= '2022-05-01' AND @StartingDate < '2022-06-01') OR (@EndingDate >= '2022-05-01' AND @EndingDate < '2022-06-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_06]
	--IF((@StartingDate >= '2022-06-01' AND @StartingDate < '2022-07-01') OR (@EndingDate >= '2022-06-01' AND @EndingDate < '2022-07-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_07]
	--IF((@StartingDate >= '2022-07-01' AND @StartingDate < '2022-08-01') OR (@EndingDate >= '2022-07-01' AND @EndingDate < '2022-08-01'))
	--BEGIN
	--	insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
	--	select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData
	--	where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
	--	order by UpdateTime;
	--END	

	--#[3rdEyE_TrackingDataBase_2022_08]
	IF((@StartingDate >= '2022-08-01' AND @StartingDate < '2022-09-01') OR (@EndingDate >= '2022-08-01' AND @EndingDate < '2022-09-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END	

	--#[3rdEyE_TrackingDataBase_2022_09]
	IF((@StartingDate >= '2022-09-01' AND @StartingDate < '2022-10-01') OR (@EndingDate >= '2022-09-01' AND @EndingDate < '2022-10-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END	

	--#[3rdEyE_TrackingDataBase_2022_10]
	IF((@StartingDate >= '2022-10-01' AND @StartingDate < '2022-11-01') OR (@EndingDate >= '2022-10-01' AND @EndingDate < '2022-11-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END	
	
	--#[3rdEyE_TrackingDataBase_2022_11]
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01') OR (@EndingDate >= '2022-11-01' AND @EndingDate < '2022-12-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END	
		
	--#[3rdEyE_TrackingDataBase_2022_12]
	IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01') OR (@EndingDate >= '2022-12-01' AND @EndingDate < '2023-01-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END	

	SELECT @FK_Vehicle as FK_Vehicle, Report_VehicleHistoryDetail.* FROM Report_VehicleHistoryDetail where @USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle;

END  
-- exec Report_GetVehicleHistoryDetail '3c2540db-2f16-4487-bc64-5489328de706', '22-Oct-18 6:00:00 PM', '22-Oct-18 7:00:00 PM';


GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleLastUpdate_withNearMapLocation]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleLastUpdate_withNearMapLocation](
@PK_User uniqueidentifier
)
AS  
BEGIN 
	DECLARE @CurrentDateaTime datetime=getdate(); 
	
	Select
	Vehicle.PK_Vehicle
	,(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation'
	,Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	,VehicleTracking.Latitude
    ,VehicleTracking.Longitude
    ,VehicleTracking.Altitude
    ,VehicleTracking.EngineStatus
    ,VehicleTracking.Course
    ,VehicleTracking.Temperature
    ,VehicleTracking.Fuel
    ,VehicleTracking.Speed
    ,VehicleTracking.Distance
    ,VehicleTracking.UpdateTime
    ,VehicleTracking.ServerTime
	,DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) as 'UpdatedAgo_Minute'
	
	,Vehicle.RegistrationNumber
	,Vehicle.FK_Company
	,Vehicle.FK_Depo
	,Vehicle.MHT_DHT_DriverContactNumber
	,Vehicle.VehicleType
	,Vehicle.GpsMobileNumber
	,Vehicle.GpsIMEINumber
	
	
	,UserCompany.Name AS UserCompany_Name
	,Depo.Name  AS Depo_Name
	,VehicleBrand.Name +'/'+VehicleModel.Title AS 'BrandModel'
	,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 15 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 

	FROM AppUserAccessibleDepo
	JOIN Depo on AppUserAccessibleDepo.FK_Depo = Depo.PK_Depo
	JOIN Vehicle on Depo.PK_Depo = Vehicle.FK_Depo
	JOIN VehicleTracking on Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
	JOIN VehicleTrackingInformation ON Vehicle.PK_Vehicle = VehicleTrackingInformation.PK_Vehicle
	LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
	LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
	LEFT JOIN VehicleBrand ON VehicleModel.FK_VehicleBrand = VehicleBrand.PK_VehicleBrand
	where AppUserAccessibleDepo.FK_AppUser = @PK_User AND AppUserAccessibleDepo.IsAccessible = 1 
	order by Vehicle.RegistrationNumber
	;
END  
-- EXEC Report_GetVehicleLastUpdate_withNearMapLocation '00000000-0000-0000-0000-000000000000'

GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleRunAndHaltReport]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleRunAndHaltReport](
@USER_KEY varchar(50),
@FK_Vehicle uniqueidentifier,
@StartingDate datetime,
@EndingDate datetime
)
AS
BEGIN
	--###################################### H E A D ##########################################
	--CLEAR DATA TABLE
	DELETE FROM Report_VehicleRunAndHaltReport WHERE USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle;  
	DELETE FROM Report_VehicleRunAndHaltReport_Helper WHERE USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle;  
	
	--Vehicle registation number
	DECLARE @_VehicleRegistrationNumber [varchar](50);
	SET @_VehicleRegistrationNumber = (SELECT TOP 1 RegistrationNumber FROM [3rdEyE].dbo.Vehicle
	WHERE PK_Vehicle = @FK_Vehicle);

	DECLARE @HaltTime BIGINT=0;
	DECLARE @TotalHaltTime BIGINT=0;
	DECLARE @HaltCount BIGINT=0;

	-- CONSTANT VARIABLES
	DECLARE @MinimumGapMinute INT = 5;
	-- COMMON VARIABLES
	DECLARE @LapTime BIGINT=0;

	DECLARE @first_id BIGINT;
	Declare @first_UpdateTime DATETIME;
	Declare @first_status [varchar](10);
	DECLARE @last_id BIGINT;
	Declare @last_UpdateTime DATETIME;
	Declare @last_status [varchar](10);

	DECLARE @current_id BIGINT;
	Declare @current_UpdateTime DATETIME;
	Declare @current_status [varchar](10);
	DECLARE @previous_id BIGINT;
	Declare @previous_UpdateTime DATETIME;
	
	DECLARE @_Latitude [varchar](300);
	DECLARE @_Longitude [varchar](300);
	DECLARE @_EngineStatus [varchar](300);
	DECLARE @_Speed [varchar](300);

	--###################################### B O D Y ##########################################


	----#[3rdEyE_TrackingDataBase_2018_09]
	--IF((@StartingDate >= '2018-09-01' AND @StartingDate < '2018-10-01') OR (@EndingDate >= '2018-09-01' AND @EndingDate < '2018-10-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_10]
	--IF((@StartingDate >= '2018-10-01' AND @StartingDate < '2018-11-01') OR (@EndingDate >= '2018-10-01' AND @EndingDate < '2018-11-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2018_11]
	--IF((@StartingDate >= '2018-11-01' AND @StartingDate < '2018-12-01') OR (@EndingDate >= '2018-11-01' AND @EndingDate < '2018-12-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2018_12]
	--IF((@StartingDate >= '2018-12-01' AND @StartingDate < '2019-01-01') OR (@EndingDate >= '2018-12-01' AND @EndingDate < '2019-01-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2018_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_01]
	--IF((@StartingDate >= '2019-01-01' AND @StartingDate < '2019-02-01') OR (@EndingDate >= '2019-01-01' AND @EndingDate < '2019-02-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_02]
	--IF((@StartingDate >= '2019-02-01' AND @StartingDate < '2019-03-01') OR (@EndingDate >= '2019-02-01' AND @EndingDate < '2019-03-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_03]
	--IF((@StartingDate >= '2019-03-01' AND @StartingDate < '2019-04-01') OR (@EndingDate >= '2019-03-01' AND @EndingDate < '2019-04-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_04]
	--IF((@StartingDate >= '2019-04-01' AND @StartingDate < '2019-05-01') OR (@EndingDate >= '2019-04-01' AND @EndingDate < '2019-05-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_05]
	--IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2019_06]
	--IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_07]
	--IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_08]
	--IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_09]
	--IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_10]
	--IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_11]
	--IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2019_12]
	--IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2019_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_01]
	--IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2020_02]
	--IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_03]
	--IF((@StartingDate >= '2020-03-01' AND @StartingDate < '2020-04-01') OR (@EndingDate >= '2020-03-01' AND @EndingDate < '2020-04-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_04]
	--IF((@StartingDate >= '2020-04-01' AND @StartingDate < '2020-05-01') OR (@EndingDate >= '2020-04-01' AND @EndingDate < '2020-05-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_05]
	--IF((@StartingDate >= '2020-05-01' AND @StartingDate < '2020-06-01') OR (@EndingDate >= '2020-05-01' AND @EndingDate < '2020-06-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_06]
	--IF((@StartingDate >= '2020-06-01' AND @StartingDate < '2020-07-01') OR (@EndingDate >= '2020-06-01' AND @EndingDate < '2020-07-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_07]
	--IF((@StartingDate >= '2020-07-01' AND @StartingDate < '2020-08-01') OR (@EndingDate >= '2020-07-01' AND @EndingDate < '2020-08-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_08]
	--IF((@StartingDate >= '2020-08-01' AND @StartingDate < '2020-09-01') OR (@EndingDate >= '2020-08-01' AND @EndingDate < '2020-09-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_09]
	--IF((@StartingDate >= '2020-09-01' AND @StartingDate < '2020-10-01') OR (@EndingDate >= '2020-09-01' AND @EndingDate < '2020-10-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_10]
	--IF((@StartingDate >= '2020-10-01' AND @StartingDate < '2020-11-01') OR (@EndingDate >= '2020-10-01' AND @EndingDate < '2020-11-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2020_11]
	--IF((@StartingDate >= '2020-11-01' AND @StartingDate < '2020-12-01') OR (@EndingDate >= '2020-11-01' AND @EndingDate < '2020-12-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2020_12]
	--IF((@StartingDate >= '2020-12-01' AND @StartingDate < '2021-01-01') OR (@EndingDate >= '2020-12-01' AND @EndingDate < '2021-01-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2020_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_01]
	--IF((@StartingDate >= '2021-01-01' AND @StartingDate < '2021-02-01') OR (@EndingDate >= '2021-01-01' AND @EndingDate < '2021-02-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_02]
	--IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_03]
	--IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_04]
	--IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_05]
	--IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_06]
	--IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_07]
	--IF((@StartingDate >= '2021-07-01' AND @StartingDate < '2021-08-01') OR (@EndingDate >= '2021-07-01' AND @EndingDate < '2021-08-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_07].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_08]
	--IF((@StartingDate >= '2021-08-01' AND @StartingDate < '2021-09-01') OR (@EndingDate >= '2021-08-01' AND @EndingDate < '2021-09-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_08].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_09]
	--IF((@StartingDate >= '2021-09-01' AND @StartingDate < '2021-10-01') OR (@EndingDate >= '2021-09-01' AND @EndingDate < '2021-10-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_09].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2021_10]
	--IF((@StartingDate >= '2021-10-01' AND @StartingDate < '2021-11-01') OR (@EndingDate >= '2021-10-01' AND @EndingDate < '2021-11-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_10].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_11]
	--IF((@StartingDate >= '2021-11-01' AND @StartingDate < '2021-12-01') OR (@EndingDate >= '2021-11-01' AND @EndingDate < '2021-12-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_11].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2021_12]
	--IF((@StartingDate >= '2021-12-01' AND @StartingDate < '2022-01-01') OR (@EndingDate >= '2021-12-01' AND @EndingDate < '2022-01-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2021_12].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_01]
	--IF((@StartingDate >= '2022-01-01' AND @StartingDate < '2022-02-01') OR (@EndingDate >= '2022-01-01' AND @EndingDate < '2022-02-01'))	
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_01].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_02]
	--IF((@StartingDate >= '2022-02-01' AND @StartingDate < '2022-03-01') OR (@EndingDate >= '2022-02-01' AND @EndingDate < '2022-03-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_02].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	----#[3rdEyE_TrackingDataBase_2022_03]
	--IF((@StartingDate >= '2022-03-01' AND @StartingDate < '2022-04-01') OR (@EndingDate >= '2022-03-01' AND @EndingDate < '2022-04-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_03].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_04]
	--IF((@StartingDate >= '2022-04-01' AND @StartingDate < '2022-05-01') OR (@EndingDate >= '2022-04-01' AND @EndingDate < '2022-05-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_04].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END
	
	----#[3rdEyE_TrackingDataBase_2022_05]
	--IF((@StartingDate >= '2022-05-01' AND @StartingDate < '2022-06-01') OR (@EndingDate >= '2022-05-01' AND @EndingDate < '2022-06-01'))
	--BEGIN
	--	Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
	--	Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
	--	FROM [3rdEyE_TrackingDataBase_2022_05].dbo.DeviceData 
	--	WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
	--	Order by UpdateTime;
	--END

	--#[3rdEyE_TrackingDataBase_2022_06]
	IF((@StartingDate >= '2022-06-01' AND @StartingDate < '2022-07-01') OR (@EndingDate >= '2022-06-01' AND @EndingDate < '2022-07-01'))
	BEGIN
		Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_06].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2022_07]
	IF((@StartingDate >= '2022-07-01' AND @StartingDate < '2022-08-01') OR (@EndingDate >= '2022-07-01' AND @EndingDate < '2022-08-01'))
	BEGIN
		Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_07].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_08]
	IF((@StartingDate >= '2022-08-01' AND @StartingDate < '2022-09-01') OR (@EndingDate >= '2022-08-01' AND @EndingDate < '2022-09-01'))
	BEGIN
		Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_08].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_09]
	IF((@StartingDate >= '2022-09-01' AND @StartingDate < '2022-10-01') OR (@EndingDate >= '2022-09-01' AND @EndingDate < '2022-10-01'))
	BEGIN
		Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_09].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2022_10]
	IF((@StartingDate >= '2022-10-01' AND @StartingDate < '2022-11-01') OR (@EndingDate >= '2022-10-01' AND @EndingDate < '2022-11-01'))
	BEGIN
		Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_10].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2022_11]
	IF((@StartingDate >= '2022-11-01' AND @StartingDate < '2022-12-01') OR (@EndingDate >= '2022-11-01' AND @EndingDate < '2022-12-01'))
	BEGIN
		Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_11].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2022_12]
	IF((@StartingDate >= '2022-12-01' AND @StartingDate < '2023-01-01') OR (@EndingDate >= '2022-12-01' AND @EndingDate < '2023-01-01'))
	BEGIN
		Insert into Report_VehicleRunAndHaltReport_Helper(USER_KEY,FK_Vehicle,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY , @FK_Vehicle, Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2022_12].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#Starting part calculation & insert
	SET @first_id = 0;

	SELECT TOP 1 @first_id = PK_RowData, @first_UpdateTime = UpdateTime, @first_status = (CASE WHEN EngineStatus = '0' AND Speed = 0  THEN 'Halt' ELSE 'Run' END)
	FROM Report_VehicleRunAndHaltReport_Helper WHERE USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle;
	
	IF(@first_id != 0)
	BEGIN
		SET @LapTime = DATEDIFF(mi, @StartingDate, @first_UpdateTime);
		INSERT INTO Report_VehicleRunAndHaltReport(USER_KEY,FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,LapTime) VALUES
		(@USER_KEY, @FK_Vehicle, 'Starting ' + @first_status, '-', @first_id, @_VehicleRegistrationNumber, @StartingDate, @first_UpdateTime, '', '', '', '', @LapTime); 
	END

	--#Ending part calculation
	set @last_id = 0;

	SELECT TOP 1 @last_id = PK_RowData, @last_UpdateTime = UpdateTime, @last_status = (CASE WHEN EngineStatus = '0' AND Speed = 0  THEN 'Halt' ELSE 'Run' END)
	FROM Report_VehicleRunAndHaltReport_Helper WHERE USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle ORDER BY PK_RowData DESC;
	
	--#Middle part calculation & insert
	IF(@first_id != 0)
	BEGIN
		SET @previous_id = @first_id;

		SET @current_status = @first_status;
		SET @previous_UpdateTime = @first_UpdateTime;

	END

	IF(@first_id != 0 AND @last_id != 0 AND @first_id != @last_id)
	BEGIN
		--WHILE(@previous_id < @last_id AND (@current_id != 0 OR @current_id != @last_id))
		WHILE(1=1)
		BEGIN
			set @current_id = 0;
			
			IF(@current_status = 'Halt')
			BEGIN
				
				print 'In Halt';
				SELECT TOP 1 @current_id = PK_RowData, @current_UpdateTime = UpdateTime,@_Latitude= Latitude,@_Longitude=Longitude,@_EngineStatus=EngineStatus,@_Speed=Speed
				FROM Report_VehicleRunAndHaltReport_Helper WHERE USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle AND PK_RowData > @previous_id AND (EngineStatus = '1' OR Speed > 0) AND UpdateTime > DATEADD(minute, @MinimumGapMinute, @previous_UpdateTime);
				print 'found current_id-' print @current_id;
				
				IF(@current_id != 0)
				BEGIN
					print 'In Halt 1.1';
					SET @LapTime = DATEDIFF(mi, @previous_UpdateTime, @current_UpdateTime);
					INSERT INTO Report_VehicleRunAndHaltReport(USER_KEY,FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,LapTime) VALUES
					(@USER_KEY, @FK_Vehicle, @current_status, @previous_id, @current_id, @_VehicleRegistrationNumber, @previous_UpdateTime, @current_UpdateTime, @_Latitude, @_Longitude, @_EngineStatus, @_Speed, @LapTime); 
					print 'Inserted halt done.';
					SET @previous_id = @current_id;
					set @previous_UpdateTime = @current_UpdateTime;
					print 'Next id from -'; print @previous_id;
					SET @current_status = 'Run';
				END

				ELSE
				BEGIN
					print 'In Halt 1.2';
					SELECT TOP 1 @current_id = PK_RowData, @current_UpdateTime = UpdateTime,@_Latitude= Latitude,@_Longitude=Longitude,@_EngineStatus=EngineStatus,@_Speed=Speed
					FROM Report_VehicleRunAndHaltReport_Helper WHERE PK_RowData = @last_id; --USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle AND PK_RowData > @previous_id AND (EngineStatus = '1' OR Speed > 0);			
					
					SET @LapTime = DATEDIFF(mi, @previous_UpdateTime, @current_UpdateTime);
					INSERT INTO Report_VehicleRunAndHaltReport(USER_KEY,FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,LapTime) VALUES
					(@USER_KEY, @FK_Vehicle, @current_status, @previous_id, @current_id, @_VehicleRegistrationNumber, @previous_UpdateTime, @current_UpdateTime, @_Latitude, @_Longitude, @_EngineStatus, @_Speed, @LapTime); 
					print 'Inserted halt done.';
					print 'Will Break.';
					BREAK;
				END

			END
			ELSE --IF(@previous_status = 'Run')
			BEGIN

				print 'In Run';
				SELECT TOP 1 @current_id = PK_RowData, @current_UpdateTime = UpdateTime,@_Latitude= Latitude,@_Longitude=Longitude,@_EngineStatus=EngineStatus,@_Speed=Speed
				FROM Report_VehicleRunAndHaltReport_Helper WHERE USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle AND PK_RowData > @previous_id AND (EngineStatus = '0' AND Speed = 0) AND UpdateTime > DATEADD(minute, @MinimumGapMinute, @previous_UpdateTime);
				print @current_id;
				
				IF(@current_id != 0)
				BEGIN
					print 'In Run 1.1';
					SET @LapTime = DATEDIFF(mi, @previous_UpdateTime, @current_UpdateTime);
					INSERT INTO Report_VehicleRunAndHaltReport(USER_KEY,FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,LapTime) VALUES
					(@USER_KEY, @FK_Vehicle, @current_status, @previous_id, @current_id, @_VehicleRegistrationNumber, @previous_UpdateTime, @current_UpdateTime, @_Latitude, @_Longitude, @_EngineStatus, @_Speed, @LapTime); 
					print 'Inserted run done.';
					SET @previous_id = @current_id;
					set @previous_UpdateTime = @current_UpdateTime;
					print 'Next id from -'; print @previous_id;
					SET @current_status = 'Halt';
				END
				
				ELSE
				BEGIN
					print 'In Run 1.2';
					SELECT TOP 1 @current_id = PK_RowData, @current_UpdateTime = UpdateTime,@_Latitude= Latitude,@_Longitude=Longitude,@_EngineStatus=EngineStatus,@_Speed=Speed
					FROM Report_VehicleRunAndHaltReport_Helper WHERE PK_RowData = @last_id; --USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle AND PK_RowData > @previous_id AND (EngineStatus = '1' OR Speed > 0);			
					
					SET @LapTime = DATEDIFF(mi, @previous_UpdateTime, @current_UpdateTime);
					INSERT INTO Report_VehicleRunAndHaltReport(USER_KEY,FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,LapTime) VALUES
					(@USER_KEY, @FK_Vehicle, @current_status, @previous_id, @current_id, @_VehicleRegistrationNumber, @previous_UpdateTime, @current_UpdateTime, @_Latitude, @_Longitude, @_EngineStatus, @_Speed, @LapTime); 
					print 'Inserted run done.';
					print 'Will Break.';
					BREAK;
				END

			END

			print 'while-end';
		END
	END
	
	--#Ending part insert
	IF(@last_id != 0)
	BEGIN
		SET @LapTime = DATEDIFF(mi, @last_UpdateTime, @EndingDate);
		INSERT INTO Report_VehicleRunAndHaltReport(USER_KEY,FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,VehicleRegistrationNumber,StartTime,EndTime,Latitude,Longitude,EngineStatus,Speed,LapTime) VALUES
		(@USER_KEY, @FK_Vehicle, 'Ending ' + @last_status, @last_id, '-', @_VehicleRegistrationNumber, @last_UpdateTime, @EndingDate, '', '', '', '', @LapTime); 
	END

	--###################################### T A I L ##########################################
	
	--#Result selection
	SELECT *,
	(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( Report_VehicleRunAndHaltReport.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( Report_VehicleRunAndHaltReport.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( Report_VehicleRunAndHaltReport.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
	Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( Report_VehicleRunAndHaltReport.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( Report_VehicleRunAndHaltReport.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( Report_VehicleRunAndHaltReport.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
	FROM Report_VehicleRunAndHaltReport WHERE USER_KEY = @USER_KEY AND FK_Vehicle = @FK_Vehicle; 

END





--  EXEC Report_GetVehicleRunAndHaltReport '00000000-0000-0000-0000-000000000000', '7C724A6F-A7E8-4CF2-93D3-AB0DC077F36B', '2021-10-01 12:00:00 AM', '2021-10-02 12:00:00 AM';

GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleTracking]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleTracking](
@PK_User uniqueidentifier
)
AS  
BEGIN 
	DECLARE @CurrentDateaTime datetime=getdate(); 
	
	SELECT  
	Vehicle.PK_Vehicle
	,VehicleTrackingInformation.GpsIMEINumber
	,(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation'
	,Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	,VehicleTracking.Latitude
    ,VehicleTracking.Longitude
    ,VehicleTracking.Altitude
    ,VehicleTracking.EngineStatus
    ,VehicleTracking.Course
    ,VehicleTracking.Temperature
    ,VehicleTracking.Fuel
    ,VehicleTracking.Speed
    ,VehicleTracking.Distance
    ,VehicleTracking.RemainingCash
    ,VehicleTracking.UpdateTime as 'UpdateTime_'
    ,Format(VehicleTracking.UpdateTime, 'dd.MM.yy hh:mm tt') as 'UpdateTime'
	,Format(VehicleTracking.ServerTime, 'dd.MM.yy hh:mm tt') as 'ServerTime'
	--,DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) as 'UpdatedAgo_Minute'
	
	,Vehicle.RegistrationNumber + ' ['+ISNULL(Vehicle.GpsDeviceModel, 'N/A') + ']'+ (CASE WHEN VehicleTracking.Status_PostionValidity = 'V' THEN ' FL' ELSE '' END)  as RegistrationNumber
	,Vehicle.FK_Company
	,Vehicle.FK_Depo
	,Vehicle.Internal_VehicleContactNumber
	,Vehicle.VehicleType
	,Vehicle.GpsMobileNumber
	,Vehicle.GpsIMEINumber
	,Vehicle.Internal_ShowTemperature

	
	,UserCompany.Name AS UserCompany_Name
	,Depo.Name AS DepoName
	--,(CASE WHEN VehicleTracking.FK_Depo_In > VehicleTracking.FK_Depo_Out  THEN ('Entered ' + DepoIn.Name + ' At ' + Format(VehicleTracking.DepoInDateTime, 'dd.MM.yy hh:mm tt') +' (aprx)') ELSE ('Left ' + DepoOut.Name + ' At ' + Format(VehicleTracking.DepoOutDateTime, 'dd.MM.yy hh:mm tt') +' (aprx)') END) as 'DepoInOutData'
	--,'' as 'DepoInOutData'
	,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 5 THEN -1 ELSE (CASE WHEN VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 

	FROM AppUserAccessibleDepo
	JOIN Depo ON AppUserAccessibleDepo.FK_Depo = Depo.PK_Depo 
	JOIN Vehicle ON Depo.PK_Depo = Vehicle.FK_Depo
	JOIN vw_VehicleTracking as VehicleTracking ON Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
	JOIN VehicleTrackingInformation ON Vehicle.PK_Vehicle = VehicleTrackingInformation.PK_Vehicle
	LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
	LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
	LEFT JOIN Depo as DepoIn ON VehicleTracking.FK_Depo_In = DepoIn.PK_Depo 
	LEFT JOIN Depo as DepoOut ON VehicleTracking.FK_Depo_Out = DepoOut.PK_Depo 
	Where AppUserAccessibleDepo.FK_AppUser = @PK_User AND AppUserAccessibleDepo.IsAccessible = 1 
	--AND  DATEDIFF(day, VehicleTracking.ServerTime, @CurrentDateaTime) <= 7
	order by Status desc;
	;
END  


-- EXEC _Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'




GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleTrackingByDepo]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE PROCEDURE [dbo].[Report_GetVehicleTrackingByDepo](
	@PK_Depo uniqueidentifier
	)
	AS  
	BEGIN 
		DECLARE @CurrentDateaTime datetime=getdate(); 
		SELECT  
		Vehicle.PK_Vehicle
		,VehicleTrackingInformation.GpsIMEINumber
		,(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation'
		,Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
		,VehicleTracking.Latitude
		,VehicleTracking.Longitude
		,VehicleTracking.Altitude
		,VehicleTracking.EngineStatus
		,VehicleTracking.Course
		,VehicleTracking.Temperature
		,VehicleTracking.Fuel
		,VehicleTracking.Speed
		,VehicleTracking.Distance
		,Format(VehicleTracking.UpdateTime, 'dd.MM.yy hh:mm tt') as 'UpdateTime'
		,Format(VehicleTracking.ServerTime, 'dd.MM.yy hh:mm tt') as 'ServerTime'
		--,DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) as 'UpdatedAgo_Minute'
	
		,Vehicle.RegistrationNumber + ' ['+ISNULL(Vehicle.GpsDeviceModel, 'N/A')+']' + (CASE WHEN VehicleTracking.Status_PostionValidity = 'V' THEN ' FL' ELSE '' END)  as RegistrationNumber
		,Vehicle.FK_Company
		,Vehicle.FK_Depo
		,Vehicle.Internal_VehicleContactNumber
		,Vehicle.VehicleType
		,Vehicle.GpsMobileNumber
		,Vehicle.GpsIMEINumber
		,Vehicle.Internal_ShowTemperature
	
	
		,UserCompany.Name AS UserCompany_Name
		,Depo.Name AS DepoName
		--,VehicleBrand.Name +'/'+VehicleModel.Title AS 'BrandModel'
		,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 15 THEN -1 ELSE (CASE WHEN VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 


		FROM Vehicle
		JOIN vw_VehicleTracking as VehicleTracking ON Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
		JOIN VehicleTrackingInformation ON Vehicle.PK_Vehicle = VehicleTrackingInformation.PK_Vehicle
		LEFT JOIN Depo ON Vehicle.FK_Depo = Depo.PK_Depo
		LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
		--LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
		Where Vehicle.FK_Depo = @PK_Depo
		--AND  DATEDIFF(day, VehicleTracking.ServerTime, @CurrentDateaTime) <= 7
		order by Status desc;
		;
	END  
	-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'


GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleTrackingByDepoGroup]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE PROCEDURE [dbo].[Report_GetVehicleTrackingByDepoGroup](
	@PK_DepoGroup uniqueidentifier
	)
	AS  
	BEGIN 
		DECLARE @CurrentDateaTime datetime=getdate(); 
		SELECT  
		Vehicle.PK_Vehicle
		,VehicleTrackingInformation.GpsIMEINumber
		,(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation'
		,Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
		,VehicleTracking.Latitude
		,VehicleTracking.Longitude
		,VehicleTracking.Altitude
		,VehicleTracking.EngineStatus
		,VehicleTracking.Course
		,VehicleTracking.Temperature
		,VehicleTracking.Fuel
		,VehicleTracking.Speed
		,VehicleTracking.Distance
		,Format(VehicleTracking.UpdateTime, 'dd.MM.yy hh:mm tt') as 'UpdateTime'
		,Format(VehicleTracking.ServerTime, 'dd.MM.yy hh:mm tt') as 'ServerTime'
		--,DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) as 'UpdatedAgo_Minute'
	
		,Vehicle.RegistrationNumber + ' ['+ISNULL(Vehicle.GpsDeviceModel, 'N/A')+']' + (CASE WHEN VehicleTracking.Status_PostionValidity = 'V' THEN ' FL' ELSE '' END)  as RegistrationNumber
		,Vehicle.FK_Company
		,Vehicle.FK_Depo
		,Vehicle.Internal_VehicleContactNumber
		,Vehicle.VehicleType
		,Vehicle.GpsMobileNumber
		,Vehicle.GpsIMEINumber
		,Vehicle.Internal_ShowTemperature
	
	
		,UserCompany.Name AS UserCompany_Name
		,Depo.Name AS DepoName
		--,VehicleBrand.Name +'/'+VehicleModel.Title AS 'BrandModel'
		,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 15 THEN -1 ELSE (CASE WHEN VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 



		FROM Vehicle
		JOIN vw_VehicleTracking as VehicleTracking ON Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
		JOIN VehicleTrackingInformation ON Vehicle.PK_Vehicle = VehicleTrackingInformation.PK_Vehicle
		LEFT JOIN Depo ON Vehicle.FK_Depo = Depo.PK_Depo
		LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
		--LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
		Where Vehicle.FK_DepoGroup = @PK_DepoGroup
		--AND  DATEDIFF(day, VehicleTracking.ServerTime, @CurrentDateaTime) <= 7
		order by Status desc;
		;
	END  
	-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'


GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleTrackingForIndividualRequisition]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleTrackingForIndividualRequisition](
@PK_IndividualRequisition uniqueidentifier
)
AS  
BEGIN 
	DECLARE @CurrentDateaTime datetime=getdate(); 
	
	SELECT  
	Vehicle.PK_Vehicle
	,(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation'
	,Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	,VehicleTracking.Latitude
    ,VehicleTracking.Longitude
    ,VehicleTracking.Altitude
    ,VehicleTracking.EngineStatus
    ,VehicleTracking.Course
    ,VehicleTracking.Temperature
    ,VehicleTracking.Fuel
    ,VehicleTracking.Speed
    ,VehicleTracking.Distance
    ,Format(VehicleTracking.UpdateTime, 'dd.MM.yy hh:mm tt') as 'UpdateTime'
	,Format(VehicleTracking.ServerTime, 'dd.MM.yy hh:mm tt') as 'ServerTime'
	--,DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) as 'UpdatedAgo_Minute'
	
	,Vehicle.RegistrationNumber
	,Vehicle.FK_Company
	,Vehicle.FK_Depo
	,Vehicle.Internal_VehicleContactNumber
	,Vehicle.VehicleType
	,Vehicle.GpsMobileNumber
	,Vehicle.GpsIMEINumber
	,Vehicle.Internal_ShowTemperature

	
	,UserCompany.Name AS UserCompany_Name
	,Depo.Name AS DepoName
	--,VehicleBrand.Name +'/'+VehicleModel.Title AS 'BrandModel'
	,(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 30 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) AS 'Status' 

	FROM IndividualRequisitionAssignedVehicle
	JOIN Vehicle ON IndividualRequisitionAssignedVehicle.FK_Vehicle = Vehicle.PK_Vehicle
	join Depo on Vehicle.FK_Depo  = Depo.PK_Depo
	LEFT JOIN VehicleTracking ON Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
	LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
	LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
	--LEFT JOIN VehicleBrand ON VehicleModel.FK_VehicleBrand = VehicleBrand.PK_VehicleBrand
	--Where IndividualRequisitionAssignedVehicle.FK_IndividualRequisition = @PK_IndividualRequisition
	order by Status desc;
	;
END  


--EXEC Report_GetVehicleTracking '19d8a397-c0a1-4deb-a809-5f33e56f7028'

GO
/****** Object:  StoredProcedure [dbo].[ReportMobile_GetVehicleTracking_Far]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportMobile_GetVehicleTracking_Far](
@PK_User uniqueidentifier,
@Latitude varchar(10),
@Longitude varchar(10)
)
AS  
BEGIN 
	DECLARE @CurrentDateaTime datetime=getdate(); 
	
	SELECT  
	--top 10
	Vehicle.PK_Vehicle
	,VehicleTrackingInformation.GpsIMEINumber
	,(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation'
	,Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	,VehicleTracking.Latitude
    ,VehicleTracking.Longitude
    ,VehicleTracking.Altitude
    ,VehicleTracking.EngineStatus
    ,VehicleTracking.Course
    ,VehicleTracking.Temperature
    ,VehicleTracking.Fuel
    ,VehicleTracking.Speed
    ,VehicleTracking.Distance
    ,VehicleTracking.RemainingCash
    ,VehicleTracking.UpdateTime as 'UpdateTime_'
    ,Format(VehicleTracking.UpdateTime, 'dd.MM.yy hh:mm tt') as 'UpdateTime'
	,Format(VehicleTracking.ServerTime, 'dd.MM.yy hh:mm tt') as 'ServerTime'
	--,DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) as 'UpdatedAgo_Minute'
	
	,Vehicle.RegistrationNumber + ' ['+Vehicle.GpsDeviceModel+']' as RegistrationNumber
	,Vehicle.FK_Company
	,Vehicle.FK_Depo
	,Vehicle.Internal_VehicleContactNumber
	,Vehicle.VehicleType
	,Vehicle.GpsMobileNumber
	,Vehicle.GpsIMEINumber
	,Vehicle.Internal_ShowTemperature

	
	,UserCompany.Name AS UserCompany_Name
	,Depo.Name AS DepoName
	--,(CASE WHEN VehicleTracking.FK_Depo_In > VehicleTracking.FK_Depo_Out  THEN ('Entered ' + DepoIn.Name + ' At ' + Format(VehicleTracking.DepoInDateTime, 'dd.MM.yy hh:mm tt') +' (aprx)') ELSE ('Left ' + DepoOut.Name + ' At ' + Format(VehicleTracking.DepoOutDateTime, 'dd.MM.yy hh:mm tt') +' (aprx)') END) as 'DepoInOutData'
	--,'' as 'DepoInOutData'
	,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 5 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 

	FROM AppUserAccessibleDepo
	JOIN Depo ON AppUserAccessibleDepo.FK_Depo = Depo.PK_Depo 
	JOIN Vehicle ON Depo.PK_Depo = Vehicle.FK_Depo
	JOIN VehicleTracking ON Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
	JOIN VehicleTrackingInformation ON Vehicle.PK_Vehicle = VehicleTrackingInformation.PK_Vehicle
	LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
	LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
	LEFT JOIN Depo as DepoIn ON VehicleTracking.FK_Depo_In = DepoIn.PK_Depo 
	LEFT JOIN Depo as DepoOut ON VehicleTracking.FK_Depo_Out = DepoOut.PK_Depo 
Where AppUserAccessibleDepo.FK_AppUser = @PK_User AND AppUserAccessibleDepo.IsAccessible = 1 
	and VehicleTracking.Latitude Not like @Latitude+'%' and VehicleTracking.Longitude Not like @Longitude+'%' 
	order by VehicleTracking.ServerTime desc;
	;
END  


-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'
-- EXEC Report_GetVehicleTracking_Minified '00000000-0000-0000-0000-000000000000', '23','90'


GO
/****** Object:  StoredProcedure [dbo].[ReportMobile_GetVehicleTracking_Near]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ReportMobile_GetVehicleTracking_Near](
@PK_User uniqueidentifier,
@Latitude varchar(10),
@Longitude varchar(10)
)
AS  
BEGIN 
	DECLARE @CurrentDateaTime datetime=getdate(); 
	
	SELECT  
	--top 10
	Vehicle.PK_Vehicle
	,VehicleTrackingInformation.GpsIMEINumber
	,(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation'
	,Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	,VehicleTracking.Latitude
    ,VehicleTracking.Longitude
    ,VehicleTracking.Altitude
    ,VehicleTracking.EngineStatus
    ,VehicleTracking.Course
    ,VehicleTracking.Temperature
    ,VehicleTracking.Fuel
    ,VehicleTracking.Speed
    ,VehicleTracking.Distance
    ,VehicleTracking.RemainingCash
    ,VehicleTracking.UpdateTime as 'UpdateTime_'
    ,Format(VehicleTracking.UpdateTime, 'dd.MM.yy hh:mm tt') as 'UpdateTime'
	,Format(VehicleTracking.ServerTime, 'dd.MM.yy hh:mm tt') as 'ServerTime'
	--,DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) as 'UpdatedAgo_Minute'
	
	,Vehicle.RegistrationNumber + ' ['+Vehicle.GpsDeviceModel+']' as RegistrationNumber
	,Vehicle.FK_Company
	,Vehicle.FK_Depo
	,Vehicle.Internal_VehicleContactNumber
	,Vehicle.VehicleType
	,Vehicle.GpsMobileNumber
	,Vehicle.GpsIMEINumber
	,Vehicle.Internal_ShowTemperature

	
	,UserCompany.Name AS UserCompany_Name
	,Depo.Name AS DepoName
	--,(CASE WHEN VehicleTracking.FK_Depo_In > VehicleTracking.FK_Depo_Out  THEN ('Entered ' + DepoIn.Name + ' At ' + Format(VehicleTracking.DepoInDateTime, 'dd.MM.yy hh:mm tt') +' (aprx)') ELSE ('Left ' + DepoOut.Name + ' At ' + Format(VehicleTracking.DepoOutDateTime, 'dd.MM.yy hh:mm tt') +' (aprx)') END) as 'DepoInOutData'
	--,'' as 'DepoInOutData'
	,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 5 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 

	FROM AppUserAccessibleDepo
	JOIN Depo ON AppUserAccessibleDepo.FK_Depo = Depo.PK_Depo 
	JOIN Vehicle ON Depo.PK_Depo = Vehicle.FK_Depo
	JOIN VehicleTracking ON Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
	JOIN VehicleTrackingInformation ON Vehicle.PK_Vehicle = VehicleTrackingInformation.PK_Vehicle
	LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
	LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
	LEFT JOIN Depo as DepoIn ON VehicleTracking.FK_Depo_In = DepoIn.PK_Depo 
	LEFT JOIN Depo as DepoOut ON VehicleTracking.FK_Depo_Out = DepoOut.PK_Depo 
Where AppUserAccessibleDepo.FK_AppUser = @PK_User AND AppUserAccessibleDepo.IsAccessible = 1 
	and VehicleTracking.Latitude like @Latitude+'%' and VehicleTracking.Longitude like @Longitude+'%' 
	order by VehicleTracking.ServerTime desc;
	;
END  


-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'
-- EXEC Report_GetVehicleTracking_Minified '00000000-0000-0000-0000-000000000000', '23','90'


GO
/****** Object:  StoredProcedure [dbo].[ReportMobile_GetVehicleTracking_Single]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[ReportMobile_GetVehicleTracking_Single](
@PK_Vehicle uniqueidentifier
)
AS  
BEGIN 
	DECLARE @CurrentDateaTime datetime=getdate(); 
	
	SELECT  
	@PK_Vehicle as 'PK_Vehicle',
	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation'
	,Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	,VehicleTracking.Latitude
    ,VehicleTracking.Longitude
    ,VehicleTracking.Altitude
    ,VehicleTracking.EngineStatus
    ,VehicleTracking.Course
    ,VehicleTracking.Temperature
    ,VehicleTracking.Fuel
    ,VehicleTracking.Speed
    ,VehicleTracking.Distance
    ,Format(VehicleTracking.UpdateTime, 'dd.MM.yy hh:mm tt') as 'UpdateTime'
	,Format(VehicleTracking.ServerTime, 'dd.MM.yy hh:mm tt') as 'ServerTime'
	,(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 30 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) AS 'Status' 

	FROM VehicleTracking
	Where PK_Vehicle = @PK_Vehicle;
END  


--EXEC Report_GetVehicleTracking_Single '064d49a4-e736-460f-83cb-b02c0c0c78ba'


GO
/****** Object:  StoredProcedure [dbo].[SP_JOB_InsertDeviceData]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_JOB_InsertDeviceData]
AS
BEGIN

INSERT INTO [3rdEyE_TrackingDataBase_2022_11].[dbo].[DeviceData]
        ([FK_Vehicle]
        ,[GpsIMEINumber]
        ,[Latitude]
        ,[Longitude]
        ,[Altitude]
        ,[EngineStatus]
        ,[Course]
        ,[Temperature]
        ,[Fuel]
        ,[UpdateTime]
        ,[ServerTime]
        ,[EventCode]
        ,[RemainingCash]
        ,[Status_PostionValidity]
        ,[Status_SateliteCount]
        ,[Status_GSMSignalStrength]
        ,[Speed]
        ,[Distance])
SELECT [PK_Vehicle]
	,[GpsIMEINumber]
        ,[Latitude]
        ,[Longitude]
        ,[Altitude]
        ,[EngineStatus]
        ,[Course]
        ,[Temperature]
        ,[Fuel]
        ,[UpdateTime]
        ,[ServerTime]
        ,[EventCode]
        ,[RemainingCash]
        ,[Status_PostionValidity]
        ,[Status_SateliteCount]
        ,[Status_GSMSignalStrength]
        ,[Speed]
        ,[Distance]
FROM [dbo].[VehicleTracking]
where [dbo].[VehicleTracking].WillInsert = 1 and [dbo].[VehicleTracking].UpdateTime > '2022-11-01';

update [dbo].[VehicleTracking] set [dbo].[VehicleTracking].WillInsert = 0;



INSERT INTO [3rdEyE_TrackingDataBase_2022_11].[dbo].[DeviceData]
        ([FK_Vehicle]
        ,[GpsIMEINumber]
        ,[Latitude]
        ,[Longitude]
        ,[Altitude]
        ,[EngineStatus]
        ,[Course]
        ,[Temperature]
        ,[Fuel]
        ,[UpdateTime]
        ,[ServerTime]
        ,[EventCode]
        ,[RemainingCash]
        ,[Status_PostionValidity]
        ,[Status_SateliteCount]
        ,[Status_GSMSignalStrength]
        ,[Speed]
        ,[Distance])
SELECT [PK_Vehicle]
	,[GpsIMEINumber]
        ,[Latitude]
        ,[Longitude]
        ,[Altitude]
        ,[EngineStatus]
        ,[Course]
        ,[Temperature]
        ,[Fuel]
        ,[UpdateTime]
        ,[ServerTime]
        ,[EventCode]
        ,[RemainingCash]
        ,[Status_PostionValidity]
        ,[Status_SateliteCount]
        ,[Status_GSMSignalStrength]
        ,[Speed]
        ,[Distance]
FROM [dbo].[VehicleTrackingVT1]
where [dbo].[VehicleTrackingVT1].WillInsert = 1 and [dbo].[VehicleTrackingVT1].UpdateTime > '2022-11-01';

update [dbo].[VehicleTrackingVT1] set [dbo].[VehicleTrackingVT1].WillInsert = 0;


END
GO
/****** Object:  StoredProcedure [dbo].[SP_JOB_RegularJob]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_JOB_RegularJob]
AS
BEGIN
	
	--# Task: Remove Double Entry of Vehicle In from VehicleInOutManual
	delete from VehicleInOutManual
	where VehicleInOutManual.Out_IssueDateTime is null
	and VehicleInOutManual.PK_VehicleInOutManual not in (
	select FK_VehicleInOutManual_Last from Vehicle where FK_VehicleInOutManual_Last is not null
	);

	--# Task: Recalculate StayTimeMinute in VehicleInOutManual
	update [3rdEyE].[dbo].[VehicleInOutManual] set 
	InStayTimeMinute = DATEDIFF(MINUTE, In_IssueDateTime,Out_IssueDateTime)
	where Out_IssueDateTime is not null
	and (DATEDIFF(MINUTE, In_IssueDateTime,Out_IssueDateTime)) != InStayTimeMinute;

	--# Task: Remove Double Entry of Vehicle In from ParkingInOut
	delete from ParkingInOut where Out_IssueDateTime is null
	and ParkingInOut.PK_ParkingInOut not in (
	select FK_ParkingInOut_Last from Vehicle where FK_ParkingInOut_Last is not null
	);

	--# Task: Remove older data from DHT_RequisitionTrip
	delete from DHT_RequisitionTrip where DHT_RequisitionTrip.FinalWantedAtDateTime < DATEADD(hour, -24,GETDATE());

	--# Task: Move RequisitionTrip to RequisitionTrip_Finished
	exec Move_RequisitionTrip;
END



GO
/****** Object:  StoredProcedure [dbo].[Temp_T1_report]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Temp_T1_report]
AS  
BEGIN 
--select UpdateTime, DATEDIFF(hour, VehicleTracking.UpdateTime, Getdate()) as 'Updated Before',VehicleTracking.RemainingCash, VehicleTracking.ServerTime from VehicleTracking
select Vehicle.RegistrationNumber,GpsMobileNumber,VehicleTracking.RemainingCash, VehicleTracking.ServerTime from VehicleTracking
join Vehicle on VehicleTracking.PK_Vehicle = Vehicle.PK_Vehicle
where Vehicle.GpsDeviceModel like '% T1'
and VehicleTracking.ServerTime > DATEADD(HOUR,-48,Getdate())
and VehicleTracking.RemainingCash <= 300
order by Vehicle.RegistrationNumber desc
END  


-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'


GO
/****** Object:  StoredProcedure [dbo].[Temp_T366_report]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Temp_T366_report]
AS  
BEGIN 
--select UpdateTime, DATEDIFF(hour, VehicleTracking.UpdateTime, Getdate()) as 'Updated Before',VehicleTracking.RemainingCash, VehicleTracking.ServerTime from VehicleTracking
select Vehicle.RegistrationNumber,GpsMobileNumber,VehicleTracking.RemainingCash, VehicleTracking.ServerTime, VehicleTracking.UpdateTime from VehicleTracking
join Vehicle on VehicleTracking.PK_Vehicle = Vehicle.PK_Vehicle
where Vehicle.GpsDeviceModel like '%T366'
and VehicleTracking.ServerTime < DATEADD(MINUTE,-15,Getdate())
and VehicleTracking.RemainingCash <= 300
order by VehicleTracking.ServerTime desc
END  


-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'


GO
/****** Object:  StoredProcedure [dbo].[Temp_VT1_report]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Temp_VT1_report]
AS  
BEGIN 
--select UpdateTime, DATEDIFF(hour, VehicleTracking.UpdateTime, Getdate()) as 'Updated Before',VehicleTracking.RemainingCash, VehicleTracking.ServerTime from VehicleTracking
select Vehicle.RegistrationNumber,GpsMobileNumber,VehicleTracking.RemainingCash, VehicleTracking.ServerTime from VehicleTracking
join Vehicle on VehicleTracking.PK_Vehicle = Vehicle.PK_Vehicle
where Vehicle.GpsDeviceModel like '%VT1'
and VehicleTracking.ServerTime > DATEADD(HOUR,-120,Getdate())
--and VehicleTracking.RemainingCash <= 300
order by Vehicle.RegistrationNumber desc
END  


-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'


GO
/****** Object:  StoredProcedure [dbo].[Tracking_GetLastDeviceData]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Tracking_GetLastDeviceData](
@FK_Vehicle uniqueidentifier
)
AS
BEGIN
DECLARE @CurrentDateaTime datetime=getdate(); 
select top 1 
	*,
	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation',
	(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 30 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) AS 'Status',
	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	from vw_VehicleTracking as VehicleTracking where PK_Vehicle = @FK_Vehicle;
END

-- EXEC Tracking_GetNextDeviceData '2B3B36A7-E90B-49A7-AD50-6D74B1CF27C4', '2018-08-07 10:28:44.000' 
GO
/****** Object:  StoredProcedure [dbo].[Tracking_GetNextDeviceData]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Tracking_GetNextDeviceData](
@FK_Vehicle uniqueidentifier,
@PreviousUpdateTime datetime
)
AS
BEGIN
DECLARE @CurrentDateaTime datetime=getdate(); 
select top 1 
	*,
	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation',
	(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 30 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) AS 'Status',
	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( VehicleTracking.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( VehicleTracking.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( VehicleTracking.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	from vw_VehicleTracking as VehicleTracking where PK_Vehicle = @FK_Vehicle and UpdateTime> @PreviousUpdateTime;
END

-- EXEC Tracking_GetNextDeviceData '2B3B36A7-E90B-49A7-AD50-6D74B1CF27C4', '2018-08-07 10:28:44.000' 

GO
/****** Object:  StoredProcedure [dbo].[VehicleTracking_UpdateToGPSLost]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VehicleTracking_UpdateToGPSLost](
@PK_Vehicle uniqueidentifier,
@UpdateTime datetime
)
AS
BEGIN
DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();
UPDATE VehicleTracking SET Event_GSMSignalLost_Recovered = 'Lost', Event_GSMSignalLostRecoveredAt = @UpdateTime WHERE PK_Vehicle = @PK_Vehicle;
Select 'VehicleTracking_UpdateToGPSLost-OK' AS 'RESPONSE';
END

-- EXEC PushDeviceData_UpdateTime '14A23894-BD84-4D10-8BF1-50702A62F24A'
GO
/****** Object:  StoredProcedure [dbo].[VehicleTracking_UpdateToGPSRecover]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VehicleTracking_UpdateToGPSRecover](
@PK_Vehicle uniqueidentifier,
@UpdateTime datetime
)
AS
BEGIN
DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();
UPDATE VehicleTracking SET Event_GSMSignalLost_Recovered = 'Recovered', Event_GSMSignalLostRecoveredAt = @UpdateTime WHERE PK_Vehicle = @PK_Vehicle;
Select 'VehicleTracking_UpdateToGPSRecover-OK' AS 'RESPONSE';
END

-- EXEC PushDeviceData_UpdateTime '14A23894-BD84-4D10-8BF1-50702A62F24A'
GO
/****** Object:  StoredProcedure [dbo].[VehicleTracking_UpdateToLowBattery]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VehicleTracking_UpdateToLowBattery](
@PK_Vehicle uniqueidentifier,
@UpdateTime datetime
)
AS
BEGIN
DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();
UPDATE VehicleTracking SET Event_BatteryLow = 'Low', Event_BatteryLowAt = @UpdateTime WHERE PK_Vehicle = @PK_Vehicle;
Select 'VehicleTracking_UpdateToLowBattery-OK' AS 'RESPONSE';
END

-- EXEC PushDeviceData_UpdateTime '14A23894-BD84-4D10-8BF1-50702A62F24A'
GO
/****** Object:  StoredProcedure [dbo].[VTS_SIM_VerificationReceive]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VTS_SIM_VerificationReceive](
@SimNumber varchar(50),
@ReceivedAt datetime,
@ReceivedMessage varchar(1000),
@ResIMEI varchar(50)
)
AS
BEGIN

IF(@ReceivedAt >= '2020-02-01' AND @ReceivedAt < '2020-03-01')
BEGIN
	UPDATE [3rdEyE_TrackingDataBase_2020_02].dbo.VTS_SIM_Verification
	SET ReceivedAt = @ReceivedAt, ReceivedMessage= @ReceivedMessage, ResIMEI = @ResIMEI
	WHERE SimNumber = @SimNumber;
END
ELSE
BEGIN
  Select 'VTS_SIM_VerificationReceive-OK-UpdateTime out of range' AS 'RESPONSE'; return;
END

Select 'VTS_SIM_VerificationReceive-OK' AS 'RESPONSE';
END

--EXEC VTS_SIM_VerificationSend '01937722003', 'appVehicleRegistration', 'imei', '2020-02-01', 'text message' 
GO
/****** Object:  StoredProcedure [dbo].[VTS_SIM_VerificationSend]    Script Date: 11/28/2022 8:50:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VTS_SIM_VerificationSend](
@SimNumber varchar(50),
@AppVehicleRegistration varchar(50),
@AppVehicleIMEI varchar(50),
@SentAt datetime,
@SentMessage varchar(50)
)
AS
BEGIN

IF(@SentAt >= '2020-02-01' AND @SentAt < '2020-03-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2020_02].dbo.VTS_SIM_Verification(SimNumber,AppVehicleRegistration,AppVehicleIMEI,SentAt,SentMessage) 
	VALUES (@SimNumber,@AppVehicleRegistration,@AppVehicleIMEI,@SentAt,@SentMessage);	
END
ELSE
BEGIN
  Select 'VTS_SIM_VerificationSend-OK-UpdateTime out of range' AS 'RESPONSE'; return;
END

Select 'VTS_SIM_VerificationSend-OK' AS 'RESPONSE';
END

--EXEC VTS_SIM_VerificationSend '01937722003', 'appVehicleRegistration', 'imei', '2020-02-01', 'text message' 
GO
