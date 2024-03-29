USE [master]
GO
/****** Object:  Database [3rdEyE]    Script Date: 2021-05-25 10:58:10 AM ******/
CREATE DATABASE [3rdEyE]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'3rdEyE', FILENAME = N'D:\Installed Software''s Folder\SQL Server 2016\MSSQL13.SQLEXPRESS\MSSQL\DATA\3rdEyE.mdf' , SIZE = 8265728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'3rdEyE_log', FILENAME = N'D:\Installed Software''s Folder\SQL Server 2016\MSSQL13.SQLEXPRESS\MSSQL\DATA\3rdEyE_log.ldf' , SIZE = 32055296KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [3rdEyE] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [3rdEyE].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [3rdEyE] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [3rdEyE] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [3rdEyE] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [3rdEyE] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [3rdEyE] SET ARITHABORT OFF 
GO
ALTER DATABASE [3rdEyE] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [3rdEyE] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [3rdEyE] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [3rdEyE] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [3rdEyE] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [3rdEyE] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [3rdEyE] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [3rdEyE] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [3rdEyE] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [3rdEyE] SET  DISABLE_BROKER 
GO
ALTER DATABASE [3rdEyE] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [3rdEyE] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [3rdEyE] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [3rdEyE] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [3rdEyE] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [3rdEyE] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [3rdEyE] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [3rdEyE] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [3rdEyE] SET  MULTI_USER 
GO
ALTER DATABASE [3rdEyE] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [3rdEyE] SET DB_CHAINING OFF 
GO
ALTER DATABASE [3rdEyE] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [3rdEyE] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [3rdEyE] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'3rdEyE', N'ON'
GO
ALTER DATABASE [3rdEyE] SET QUERY_STORE = OFF
GO
USE [3rdEyE]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [3rdEyE]
GO
/****** Object:  User [report_user]    Script Date: 2021-05-25 10:58:10 AM ******/
CREATE USER [report_user] FOR LOGIN [report_user] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [report_user]
GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[PK_Vehicle] [uniqueidentifier] NOT NULL,
	[RegistrationNumber] [varchar](100) NOT NULL,
	[FID] [varchar](300) NULL,
	[PhysicalDocumnetLocation] [varchar](300) NULL,
	[OWN_MHT_DHT] [varchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Company] [uniqueidentifier] NULL,
	[FK_Depo] [uniqueidentifier] NOT NULL,
	[FK_DepoGroup] [uniqueidentifier] NULL,
	[FK_VehicleModel] [uniqueidentifier] NULL,
	[RegisrationDate] [date] NULL,
	[ChassisNumber] [varchar](100) NULL,
	[EngineNumber] [varchar](100) NULL,
	[EngineCC] [bigint] NULL,
	[VehicleType] [varchar](100) NULL,
	[FuelType] [varchar](100) NULL,
	[GpsDeviceModel] [varchar](300) NULL,
	[GpsIMEINumber] [varchar](50) NULL,
	[GpsMobileNumber] [varchar](100) NULL,
	[CapacityTon] [float] NULL,
	[CapacitySeat] [int] NULL,
	[NumberPlate_IsDigital] [bit] NULL,
	[changeImage] [bit] NULL,
	[ImageLocation] [varchar](300) NULL,
	[Internal_VehiclePrice] [bigint] NULL,
	[Internal_PurchaseDate] [date] NULL,
	[Internal_FK_FinancingCompany] [uniqueidentifier] NULL,
	[Internal_FK_PurchasingCompany] [uniqueidentifier] NULL,
	[Internal_ShowTemperature] [bit] NULL,
	[Internal_VehicleContactNumber] [varchar](100) NULL,
	[Internal_FinancingAgrementNumber] [varchar](100) NULL,
	[Internal_FinancingAgrementNumberOfVehicle] [int] NULL,
	[Internal_FinancingAgrementMaturityDate] [datetime] NULL,
	[Internal_FinancingAgrementGRN_MRR] [varchar](100) NULL,
	[Internal_FinancingAgrementGRN_MRR_Date] [datetime] NULL,
	[Internal_FinancingAgrementIsMatured] [bit] NULL,
	[Internal_FinancingAgrementUpdatedBy] [uniqueidentifier] NULL,
	[Internal_FinancingAgrementMaturityUpdatedBy] [uniqueidentifier] NULL,
	[Internal_KPL_Empty] [float] NULL,
	[Internal_KPL_Loaded] [float] NULL,
	[Internal_KPL_Loaded_Plastic] [float] NULL,
	[Internal_KPL_InterCity] [float] NULL,
	[Internal_KPL_InterCHT] [float] NULL,
	[Internal_KPL_Hill] [float] NULL,
	[Internal_KPL_OnlyMover] [float] NULL,
	[Internal_KPL_Loaded_8_To_12_Tons] [float] NULL,
	[Internal_KPL_Loaded_12_To_25_Tons] [float] NULL,
	[Internal_KPL_Loaded_8_To_12_Tons_Out] [float] NULL,
	[Internal_KPL_Loaded_12_To_25_Tons_Out] [float] NULL,
	[MHT_DHT_VehicleSize] [float] NULL,
	[MHT_DHT_IsNewDriver] [bit] NULL,
	[MHT_DHT_DriverName] [varchar](300) NULL,
	[MHT_DHT_DriverLiceneseNumber] [varchar](300) NULL,
	[MHT_DHT_DriverLicenseType] [varchar](300) NULL,
	[MHT_DHT_DriverContactNumber] [varchar](300) NULL,
	[MHT_DHT_DriverFatherName] [varchar](300) NULL,
	[MHT_DHT_DriverAddressVillage] [varchar](300) NULL,
	[MHT_DHT_DriverAddressPostOfiice] [varchar](300) NULL,
	[MHT_DHT_DriverAddressThana] [varchar](300) NULL,
	[MHT_DHT_DriverAddressDistrict] [varchar](300) NULL,
	[MHT_DHT_DriverNID] [varchar](300) NULL,
	[MHT_DHT_DriverImageLocation] [varchar](300) NULL,
	[MHT_DHT_DriverImage_changeImage] [bit] NULL,
	[MHT_DHT_DriverLicenseImageLocation] [varchar](300) NULL,
	[MHT_DHT_DriverLicenseImage_changeImage] [bit] NULL,
	[MHT_DHT_DriverSalary] [bigint] NULL,
	[MHT_DHT_OwnerName] [varchar](300) NULL,
	[MHT_DHT_OwnerFatherName] [varchar](300) NULL,
	[MHT_DHT_OwnerAddressVillage] [varchar](300) NULL,
	[MHT_DHT_OwnerAddressPostOffice] [varchar](300) NULL,
	[MHT_DHT_OwnerAddressThana] [varchar](300) NULL,
	[MHT_DHT_OwnerAddressDistrict] [varchar](300) NULL,
	[MHT_DHT_OwnerContactNumber] [varchar](300) NULL,
	[MHT_DHT_OwnerNID] [varchar](300) NULL,
	[MHT_AgreementFrom] [date] NULL,
	[MHT_AgreementTo] [date] NULL,
	[MHT_MonthlyRentRate] [bigint] NULL,
	[MHT_changeDeedImage] [bit] NULL,
	[MHT_DeedImageLocation] [varchar](300) NULL,
	[DepoInOrOut] [bit] NULL,
	[FK_DepoInOut] [uniqueidentifier] NULL,
	[DepoInOutTime] [datetime] NULL,
	[DepoInOut_Load_Unload_Workshop] [varchar](50) NULL,
	[LocationInOrOut] [bit] NULL,
	[FK_LocationInOut] [uniqueidentifier] NULL,
	[LocationInOutTime] [datetime] NULL,
	[LocationInOut_Load_Unload_Workshop] [varchar](50) NULL,
	[FK_VehicleInOutManual_Last] [bigint] NULL,
	[FK_VehicleInOutManualWorkshop] [bigint] NULL,
	[DepoStayMaximumTimeMinute] [bigint] NULL,
	[FK_VehicleSharingInternalTrip_Pending] [bigint] NULL,
	[FK_VehicleSharingInternalTrip_Current] [bigint] NULL,
	[FK_VehicleSharingExternalTrip_Current] [bigint] NULL,
	[FK_RequisitionTrip_Pending] [bigint] NULL,
	[FK_RequisitionTrip_Current] [bigint] NULL,
	[FK_RequisitionTrip_CurrentAssigner] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
	[IsAddedToSaleSoft] [bit] NULL,
	[SaleSoftRequestTime] [varchar](50) NULL,
 CONSTRAINT [PK_Vehicle_1] PRIMARY KEY CLUSTERED 
(
	[PK_Vehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RegistrationNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Depo]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Depo](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_Depo] [uniqueidentifier] NOT NULL,
	[DevelopersNote] [varchar](50) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Category] [varchar](100) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Code] [varchar](100) NULL,
	[Latitude] [varchar](100) NULL,
	[Longitude] [varchar](100) NULL,
	[CreatedAt] [datetime] NULL,
	[PRG_Type] [varchar](100) NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
	[LocationType] [varchar](100) NULL,
	[LocationZone] [varchar](100) NULL,
	[ShortCode] [varchar](50) NULL,
	[DIC_PRAN_Name] [varchar](50) NULL,
	[DIC_PRAN_Contact] [varchar](50) NULL,
	[DIC_RFL_Name] [varchar](50) NULL,
	[DIC_RFL_Contact] [varchar](50) NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[PK_Depo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleInOutManual]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleInOutManual](
	[PK_VehicleInOutManual] [bigint] IDENTITY(1,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[FK_Depo] [uniqueidentifier] NOT NULL,
	[FK_Location] [uniqueidentifier] NOT NULL,
	[FK_PRG_Type] [bigint] NULL,
	[InOrOut] [bit] NULL,
	[FK_Depo_FromTo] [uniqueidentifier] NULL,
	[In_FK_CreatedByUser] [uniqueidentifier] NOT NULL,
	[In_LoadOrEmpty] [varchar](50) NULL,
	[In_FK_VehicleInOutManualTypesOfProduct] [bigint] NULL,
	[In_FK_VehicleInOutManualReason] [bigint] NULL,
	[In_IssueDateTime] [datetime] NOT NULL,
	[In_IssuedDateTimeAutoOrManual] [varchar](50) NULL,
	[In_IsScannedEntry] [bit] NULL,
	[Note] [varchar](300) NULL,
	[Out_FK_CreatedByUser] [uniqueidentifier] NULL,
	[Out_LoadOrEmpty] [varchar](50) NULL,
	[Out_FK_VehicleInOutManualTypesOfProduct] [bigint] NULL,
	[Out_FK_VehicleInOutManualReason] [bigint] NULL,
	[Out_IssueDateTime] [datetime] NULL,
	[Out_IssuedDateTimeAutoOrManual] [varchar](50) NULL,
	[Out_IsScannedEntry] [bit] NULL,
	[GPNumber] [varchar](50) NULL,
	[StayTimeMinute] [bigint] NULL,
	[OverStayTimeMinute] [bigint] NULL,
 CONSTRAINT [PK_VehicleInOutManual] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleInOutManual] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_User] [uniqueidentifier] NOT NULL,
	[DevelopersNote] [varchar](200) NULL,
	[FID] [varchar](max) NULL,
	[IMEI] [varchar](100) NULL,
	[IsNotifiable] [bit] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsBanned] [bit] NULL,
	[IsActive] [bit] NULL,
	[AppVersionCode] [varchar](50) NULL,
	[PRG_Type] [varchar](50) NOT NULL,
	[AppUserType] [varchar](50) NOT NULL,
	[AppUserSubType] [varchar](50) NULL,
	[FK_AppRole] [bigint] NULL,
	[FK_MobileRole] [bigint] NULL,
	[UniqueIDNumber] [varchar](100) NULL,
	[Password_Encrypted] [varchar](999) NULL,
	[Password] [varchar](100) NULL,
	[FK_TransportCompany] [uniqueidentifier] NULL,
	[FK_ContructualRequisitionCompany] [uniqueidentifier] NULL,
	[FullName] [varchar](300) NULL,
	[HGroupName] [varchar](300) NULL,
	[HCompany] [varchar](300) NULL,
	[HDepartment] [varchar](300) NULL,
	[HLocationName] [varchar](300) NULL,
	[HDesignation] [varchar](300) NULL,
	[HSatus] [varchar](300) NULL,
	[HGender] [varchar](300) NULL,
	[ContactNumber] [varchar](50) NULL,
	[Email] [varchar](300) NULL,
	[ContactAddress] [varchar](max) NULL,
	[FK_Depo] [uniqueidentifier] NOT NULL,
	[FK_Location] [uniqueidentifier] NULL,
	[LoginTryDateTime] [datetime] NULL,
	[LoginTryStatus] [bit] NULL,
	[LoginHRIS_Update] [datetime] NULL,
	[PermissionAdd] [bit] NULL,
	[PermissionEdit] [bit] NULL,
	[PermissionView] [bit] NULL,
	[PermissionDelete] [bit] NULL,
	[PermissionAdmin] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
	[InActiveSessionCheck] [bit] NULL,
	[IsRegistrationCompleted] [bit] NULL,
	[FK_VehicleSharingInternalTrip_Pending] [bigint] NULL,
	[FK_VehicleSharingInternalTrip_Current] [bigint] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[PK_User] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [dbo].[view_StayReportDaily]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[view_StayReportDaily]
AS

--DECLARE @minDate datetime=DATEADD(DAY, -1, CONVERT(date, getdate())) ;
--DECLARE @maxDate datetime= CONVERT(date, getdate());

select --top 10 
Vehicle.RegistrationNumber as 'Vehicle#',
Vehicle.OWN_MHT_DHT as 'V.Type',
(CASE WHEN vehicle_depo.PRG_Type = 'CS' THEN 'CS' 
	ELSE (CASE WHEN vehicle_depo.PRG_Type = 'PRAN' THEN 'PRAN' 
		ELSE (CASE WHEN (vehicle_depo.PRG_Type = 'RFL DIST-1' OR vehicle_depo.PRG_Type = 'RFL DIST-2' OR vehicle_depo.PRG_Type = 'GPT') THEN 'RFL' ELSE 
			(CASE WHEN vehicle_depo.PK_Depo = '00000000-0000-0000-0000-000000000000' THEN in_gate.PRG_Type 
				ELSE ''END)
		END)
	END)
END) as 'Group',
physical_location.Name  as 'Location',
vehicle_depo.Name as 'User/Depot',
in_gate.FullName as 'In Gate',
VehicleInOutManual.In_IssueDateTime as 'In Time',
VehicleInOutManual.In_LoadOrEmpty as 'In Status',
out_gate.FullName as 'Out Gate',
VehicleInOutManual.Out_IssueDateTime as 'Out Time',
VehicleInOutManual.Out_LoadOrEmpty as 'Out Status',
(CASE WHEN (VehicleInOutManual.In_IssueDateTime > DATEADD(DAY, -1, CONVERT(date, getdate())) AND VehicleInOutManual.Out_IssueDateTime < CONVERT(date, getdate())) 
		THEN VehicleInOutManual.StayTimeMinute 
		ELSE (CASE WHEN (VehicleInOutManual.In_IssueDateTime > DATEADD(DAY, -1, CONVERT(date, getdate())) AND (VehicleInOutManual.Out_IssueDateTime > CONVERT(date, getdate()) OR VehicleInOutManual.Out_IssueDateTime is null)) 
				THEN DATEDIFF(MINUTE,VehicleInOutManual.In_IssueDateTime,CONVERT(date, getdate()))
					ELSE (CASE WHEN (VehicleInOutManual.In_IssueDateTime < DATEADD(DAY, -1, CONVERT(date, getdate())) AND (VehicleInOutManual.Out_IssueDateTime > DATEADD(DAY, -1, CONVERT(date, getdate())) AND VehicleInOutManual.Out_IssueDateTime < CONVERT(date, getdate()))) 
							THEN DATEDIFF(MINUTE,DATEADD(DAY, -1, CONVERT(date, getdate())),VehicleInOutManual.Out_IssueDateTime)
								ELSE (CASE WHEN (VehicleInOutManual.In_IssueDateTime < DATEADD(DAY, -1, CONVERT(date, getdate())) AND (VehicleInOutManual.Out_IssueDateTime > CONVERT(date, getdate()) OR VehicleInOutManual.Out_IssueDateTime is null)) 
										THEN 1440
											ELSE -1
										END)
							END)
				END)
END) as 'Stay Time (mm)',
cast(
(CASE WHEN (VehicleInOutManual.In_IssueDateTime > DATEADD(DAY, -1, CONVERT(date, getdate())) AND VehicleInOutManual.Out_IssueDateTime < CONVERT(date, getdate())) 
		THEN VehicleInOutManual.StayTimeMinute 
		ELSE (CASE WHEN (VehicleInOutManual.In_IssueDateTime > DATEADD(DAY, -1, CONVERT(date, getdate())) AND (VehicleInOutManual.Out_IssueDateTime > CONVERT(date, getdate()) OR VehicleInOutManual.Out_IssueDateTime is null)) 
				THEN DATEDIFF(MINUTE,VehicleInOutManual.In_IssueDateTime,CONVERT(date, getdate()))
					ELSE (CASE WHEN (VehicleInOutManual.In_IssueDateTime < DATEADD(DAY, -1, CONVERT(date, getdate())) AND (VehicleInOutManual.Out_IssueDateTime > DATEADD(DAY, -1, CONVERT(date, getdate())) AND VehicleInOutManual.Out_IssueDateTime < CONVERT(date, getdate()))) 
							THEN DATEDIFF(MINUTE,DATEADD(DAY, -1, CONVERT(date, getdate())),VehicleInOutManual.Out_IssueDateTime)
								ELSE (CASE WHEN (VehicleInOutManual.In_IssueDateTime < DATEADD(DAY, -1, CONVERT(date, getdate())) AND (VehicleInOutManual.Out_IssueDateTime > CONVERT(date, getdate()) OR VehicleInOutManual.Out_IssueDateTime is null)) 
										THEN 1440
											ELSE -1
										END)
							END)
				END)
END)/60
as decimal(10,2)) as 'Stay Time (hh:mm)',
physical_location.LocationType as 'L.Type'
from VehicleInOutManual
join Vehicle on Vehicle.PK_Vehicle = VehicleInOutManual.FK_Vehicle
join Depo as vehicle_depo on vehicle_depo.PK_Depo = Vehicle.FK_Depo
join Depo as physical_location on physical_location.PK_Depo = VehicleInOutManual.FK_Depo
join AppUser as in_gate on in_gate.PK_User = VehicleInOutManual.In_FK_CreatedByUser
left join AppUser as out_gate on out_gate.PK_User = VehicleInOutManual.Out_FK_CreatedByUser


where 1=1
and (
(VehicleInOutManual.In_IssueDateTime >= DATEADD(DAY, -1, CONVERT(date, getdate())))
OR (VehicleInOutManual.Out_IssueDateTime >= DATEADD(DAY, -1, CONVERT(date, getdate())))
OR (VehicleInOutManual.In_IssueDateTime < DATEADD(DAY, -1, CONVERT(date, getdate())) AND VehicleInOutManual.Out_IssueDateTime is null)
)
and VehicleInOutManual.In_IssueDateTime <= CONVERT(date, getdate())


--and VehicleInOutManual.InOrOut = 1
--order by VehicleInOutManual.In_IssueDateTime 


 

--OLD	NEW
--Vehicle	Vehicle#
--O/M/D	V.Type
--PRG Type	Group 
--Phy Location	Location
--User/Depot	User/Depot
--In Gate	In Gate
--In Time	In Time
--In Status	In Status
--Out Gate	Out Gate
--Out Time	Out Time
--Out Status	Out Status
--Stay Time (mm)	Stay Time (mm)
--Stay Time (hh:mm)	Stay Time (hh:mm)
-- 	L.Type

GO
/****** Object:  View [dbo].[SillyView]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SillyView]
AS
  SELECT TOP 9999999999 *
  FROM [3rdEyE_TrackingDataBase_2018_10].dbo.DeviceData
  ORDER BY UpdateTime;
GO
/****** Object:  Table [dbo].[Accident]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accident](
	[PK_Accident] [bigint] IDENTITY(1000,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[TrackingID] [varchar](100) NULL,
	[AccusedDriverStaffID] [varchar](100) NULL,
	[AccusedDriverName] [varchar](300) NULL,
	[OccuranceDate] [datetime] NULL,
	[FK_District] [bigint] NULL,
	[FK_Upazila] [bigint] NULL,
	[AccidentLocationDetail] [varchar](300) NULL,
	[CurrentVehicleLocation] [varchar](300) NULL,
	[DamageType] [varchar](300) NULL,
	[DescriptionDuty] [varchar](max) NULL,
	[DescriptionAccident] [varchar](max) NULL,
	[PrimaryCause] [varchar](300) NULL,
	[IsManLoss] [bit] NULL,
	[ManLossCount] [int] NULL,
	[IsDamageProduct] [bit] NULL,
	[DamageProductCost] [bigint] NULL,
	[IsDamageVehicle] [bit] NULL,
	[DamageVehicleDetail] [varchar](max) NULL,
	[OtherDamage] [varchar](300) NULL,
	[RepairCost] [bigint] NULL,
	[FK_DepoFollowUp] [uniqueidentifier] NULL,
	[ActionTakenStaffID] [varchar](100) NULL,
	[ActionTakenStaffName] [varchar](300) NULL,
	[Note] [varchar](max) NULL,
	[Status] [tinyint] NULL,
	[SettlementNote] [varchar](max) NULL,
	[SettlementAmount] [bigint] NULL,
	[FK_SettledByUser] [uniqueidentifier] NULL,
	[SettledAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Accident] PRIMARY KEY CLUSTERED 
(
	[PK_Accident] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AccidentDocument]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccidentDocument](
	[PK_AccidentDocument] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Accident] [bigint] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[IdentitficaitonKey] [varchar](100) NOT NULL,
	[IdentitficaitonValue] [varchar](100) NOT NULL,
	[ImageLocation] [varchar](300) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AccidentDocument] PRIMARY KEY CLUSTERED 
(
	[PK_AccidentDocument] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AccidentExpense]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccidentExpense](
	[PK_AccidentExpense] [bigint] IDENTITY(1,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Accident] [bigint] NULL,
	[TrackingID] [varchar](100) NULL,
	[OccuranceDate] [datetime] NULL,
	[PaidAmount] [bigint] NULL,
	[Purpose] [varchar](max) NULL,
	[Status] [tinyint] NULL,
	[InternalTakerStaffID] [varchar](100) NULL,
	[InternalTakerStaffName] [varchar](300) NULL,
	[ExternalTakerName] [varchar](300) NULL,
	[ExternalTakerContactNumber] [varchar](300) NULL,
	[ExternalTakerContactAddress] [varchar](max) NULL,
	[FK_PaidByUser] [uniqueidentifier] NULL,
	[PaidAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AccidentExpense] PRIMARY KEY CLUSTERED 
(
	[PK_AccidentExpense] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AlertEmail]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertEmail](
	[PK_AlertEmail] [uniqueidentifier] NOT NULL,
	[MailAddress] [varchar](100) NOT NULL,
	[MailLevel] [int] NULL,
	[IsDeleted] [bit] NULL,
	[EventAlert_1] [bit] NULL,
	[EventAlert_2] [bit] NULL,
	[EventAlert_3] [bit] NULL,
	[PoliceCaseAlert_1] [bit] NULL,
	[PoliceCaseAlert_2] [bit] NULL,
 CONSTRAINT [PK_AlertEmail] PRIMARY KEY CLUSTERED 
(
	[PK_AlertEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AlertEmailAttachedDepo]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertEmailAttachedDepo](
	[PK_AlertEmailAttachedDepo] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AlertEmail] [uniqueidentifier] NOT NULL,
	[FK_Depo] [uniqueidentifier] NOT NULL,
	[IsAttachable] [bit] NULL,
 CONSTRAINT [PK_AlertEmailAttachedDepo] PRIMARY KEY CLUSTERED 
(
	[PK_AlertEmailAttachedDepo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AlertEmailLog]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertEmailLog](
	[PK_AlertEmailLog] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AlertEmail] [uniqueidentifier] NOT NULL,
	[RequestTime] [datetime] NOT NULL,
	[EmailContent] [varchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[EorroMessage] [varchar](max) NULL,
 CONSTRAINT [PK_AlertEmailLog] PRIMARY KEY CLUSTERED 
(
	[PK_AlertEmailLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppAction]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppAction](
	[PK_AppAction] [bigint] IDENTITY(1,1) NOT NULL,
	[ActionName] [varchar](100) NULL,
	[VisibleName] [varchar](100) NOT NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK__Action] PRIMARY KEY CLUSTERED 
(
	[PK_AppAction] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppErrorLog]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppErrorLog](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[ErrorMessage] [varchar](max) NULL,
	[UserDefinedMessage] [varchar](max) NULL,
	[ErrorTime] [datetime] NULL,
 CONSTRAINT [PK_AppErrorLog] PRIMARY KEY CLUSTERED 
(
	[RowSerial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppMenu]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppMenu](
	[PK_AppMenu] [bigint] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](300) NOT NULL,
	[VisibleName] [varchar](300) NOT NULL,
	[ModelName] [varchar](300) NOT NULL,
	[Icon] [varchar](100) NOT NULL,
	[Link] [varchar](300) NULL,
	[Sequence] [int] NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_AppMenu] PRIMARY KEY CLUSTERED 
(
	[PK_AppMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppModule]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppModule](
	[PK_AppModule] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[VisibleName] [varchar](300) NOT NULL,
	[Link] [varchar](300) NULL,
	[Serial] [int] NULL,
	[Icon] [varchar](100) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_AppModule] PRIMARY KEY CLUSTERED 
(
	[PK_AppModule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__AppModul__737584F6DD8A0478] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppModule_AppAction]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppModule_AppAction](
	[PK_AppModule_AppAction] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppModule] [bigint] NOT NULL,
	[FK_AppAction] [bigint] NOT NULL,
	[Sequence] [int] NULL,
	[IsAccessible] [bit] NULL,
 CONSTRAINT [PK_AppModule_AppAction] PRIMARY KEY CLUSTERED 
(
	[PK_AppModule_AppAction] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppPermission]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppPermission](
	[PK_AppPermission] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppMenu] [bigint] NOT NULL,
	[VisibleName] [varchar](300) NOT NULL,
	[FullName] [varchar](300) NOT NULL,
	[Sequence] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_AppPermission] PRIMARY KEY CLUSTERED 
(
	[PK_AppPermission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppRole]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRole](
	[PK_AppRole] [bigint] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](300) NOT NULL,
	[Note] [varchar](300) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AppUserRole] PRIMARY KEY CLUSTERED 
(
	[PK_AppRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppRole_AppMenu]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRole_AppMenu](
	[PK_AppRole_AppMenu] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppRole] [bigint] NOT NULL,
	[FK_AppMenu] [bigint] NOT NULL,
	[IsAccessible] [bit] NULL,
	[Sequence] [int] NULL,
 CONSTRAINT [PK_AppUserRole_AppMenu] PRIMARY KEY CLUSTERED 
(
	[PK_AppRole_AppMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppRole_AppPermission]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRole_AppPermission](
	[PK_AppRole_AppPermission] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppRole] [bigint] NOT NULL,
	[FK_AppPermission] [bigint] NOT NULL,
	[IsAccessible] [bit] NULL,
 CONSTRAINT [PK_AppUserRole_AppPermission] PRIMARY KEY CLUSTERED 
(
	[PK_AppRole_AppPermission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppRole_AppSubMenu]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRole_AppSubMenu](
	[PK_AppRole_AppSubMenu] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppRole] [bigint] NOT NULL,
	[FK_AppSubMenu] [bigint] NOT NULL,
	[IsAccessible] [bit] NULL,
 CONSTRAINT [PK_AppUserRole_AppSubMenu] PRIMARY KEY CLUSTERED 
(
	[PK_AppRole_AppSubMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppSetting]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSetting](
	[PK_AppSetting] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_AppSetting] PRIMARY KEY CLUSTERED 
(
	[PK_AppSetting] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppSubMenu]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSubMenu](
	[PK_AppSubMenu] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppMenu] [bigint] NOT NULL,
	[VisibleName] [varchar](300) NOT NULL,
	[Link] [varchar](300) NOT NULL,
	[Sequence] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_AppSubMenu] PRIMARY KEY CLUSTERED 
(
	[PK_AppSubMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppUser_AppModule]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser_AppModule](
	[PK_AppUser_AppModule] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppUser] [uniqueidentifier] NOT NULL,
	[FK_AppModule] [bigint] NOT NULL,
	[IsAccessible] [bit] NOT NULL,
 CONSTRAINT [PK_AppUser_AppModule] PRIMARY KEY CLUSTERED 
(
	[PK_AppUser_AppModule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppUserAccessibleDepo]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserAccessibleDepo](
	[PK_AppUserAccessibleDepo] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppUser] [uniqueidentifier] NOT NULL,
	[FK_Depo] [uniqueidentifier] NOT NULL,
	[IsAccessible] [bit] NOT NULL,
 CONSTRAINT [PK_AppUserAccessibleDepo] PRIMARY KEY CLUSTERED 
(
	[PK_AppUserAccessibleDepo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppUserLoginHistory]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserLoginHistory](
	[PK_AppUserLoginHistory] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppUser] [uniqueidentifier] NOT NULL,
	[LoginTime] [datetime] NOT NULL,
	[ExpirationTime] [datetime] NOT NULL,
	[Reason] [varchar](100) NULL,
 CONSTRAINT [PK_AppUserLoginHistory] PRIMARY KEY CLUSTERED 
(
	[PK_AppUserLoginHistory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AppUserSurpervisedContructualCompany]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserSurpervisedContructualCompany](
	[PK_AppUserSurpervisedContructualCompany] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppUser] [uniqueidentifier] NOT NULL,
	[FK_ContructualRequisitionCompany] [uniqueidentifier] NOT NULL,
	[WillSupervise] [bit] NOT NULL,
 CONSTRAINT [PK_AppUserSurpervisedContructualCompany] PRIMARY KEY CLUSTERED 
(
	[PK_AppUserSurpervisedContructualCompany] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_Company] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_GroupOfCompany] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IsPruchaseCompany] [bit] NULL,
	[IsUserCompany] [bit] NULL,
	[CompanyRegistrationNumber] [varchar](100) NULL,
	[CompnayAddress] [varchar](max) NULL,
	[ExternalOwnersFullName] [varchar](100) NULL,
	[ExternalOwnersNID] [varchar](100) NULL,
	[ExternalOwnersFathersName] [varchar](100) NULL,
	[ExternalOwnersContactNumber] [varchar](100) NULL,
	[ExternalOwnersAddress] [varchar](max) NULL,
 CONSTRAINT [PK_Company_1] PRIMARY KEY CLUSTERED 
(
	[PK_Company] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContructualRequisition]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContructualRequisition](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_ContructualRequisition] [uniqueidentifier] NOT NULL,
	[FK_RequisitionAgent] [uniqueidentifier] NOT NULL,
	[FK_ContructualRequisitionCompany] [uniqueidentifier] NOT NULL,
	[ContructAcitivatingDate] [date] NOT NULL,
	[ContructDeactivatingDate] [date] NOT NULL,
	[Note] [varchar](max) NULL,
	[REF_FK_Depo] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_ContractualRequisition] PRIMARY KEY CLUSTERED 
(
	[PK_ContructualRequisition] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContructualRequisitionCompany]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContructualRequisitionCompany](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_ContructualRequisitionCompany] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[RegistrationNumber] [varchar](300) NULL,
	[ContactNumber] [varchar](100) NULL,
	[ContactAddress] [varchar](max) NULL,
 CONSTRAINT [PK_ContructualRequisitionCompany] PRIMARY KEY CLUSTERED 
(
	[PK_ContructualRequisitionCompany] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContructualRequisitionDetail]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContructualRequisitionDetail](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_ContructualRequisitionDetail] [uniqueidentifier] NOT NULL,
	[FK_ContructualRequisition] [uniqueidentifier] NOT NULL,
	[VehicleTypeLayer1] [varchar](100) NULL,
	[VehicleTypeLayer1_english] [varchar](100) NULL,
	[VehicleTypeLayer1_bangla] [nvarchar](200) NULL,
	[VehicleTypeLayer2] [varchar](100) NULL,
	[VehicleTypeLayer2_english] [varchar](100) NULL,
	[VehicleTypeLayer2_bangla] [nvarchar](200) NULL,
	[VehicleTypeLayer3] [varchar](100) NULL,
	[VehicleTypeLayer3_english] [varchar](100) NULL,
	[VehicleTypeLayer3_bangla] [nvarchar](200) NULL,
	[StartingLocation] [nvarchar](600) NOT NULL,
	[FinishingLocation] [nvarchar](600) NOT NULL,
	[PricePerVehicle] [bigint] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ContructualRequisitionDetail] PRIMARY KEY CLUSTERED 
(
	[PK_ContructualRequisitionDetail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContructualRequisitionDetailEntry]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContructualRequisitionDetailEntry](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_ContructualRequisitionDetailEntry] [uniqueidentifier] NOT NULL,
	[FK_ContructualRequisitionDetail] [uniqueidentifier] NOT NULL,
	[VehicleCount] [int] NOT NULL,
	[PricePerVehicle] [bigint] NOT NULL,
	[FinalContructedPrice] [bigint] NOT NULL,
	[ExecutionDate] [date] NOT NULL,
	[Status] [int] NULL,
	[FK_AppUser_AppliedBy] [uniqueidentifier] NOT NULL,
	[AppliedAt] [datetime] NULL,
	[FK_AppUser_ApprovedBy] [uniqueidentifier] NULL,
	[ApprovedAt] [datetime] NULL,
 CONSTRAINT [PK_ContructualRequisitionDetailEntry] PRIMARY KEY CLUSTERED 
(
	[PK_ContructualRequisitionDetailEntry] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DairyVehicle]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DairyVehicle](
	[PK_DairyVehicle] [uniqueidentifier] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[Destination] [varchar](300) NULL,
	[FK_AppUser_CreatedBy] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_DairyVehicle] PRIMARY KEY CLUSTERED 
(
	[PK_DairyVehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dealer]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dealer](
	[PK_Dealer] [uniqueidentifier] NOT NULL,
	[DealerCode] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[AssignTime] [datetime] NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Dealer_1] PRIMARY KEY CLUSTERED 
(
	[PK_Dealer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DepoBorder]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepoBorder](
	[PK_DepoBorder] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Depo] [uniqueidentifier] NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
 CONSTRAINT [PK_DepoBorder] PRIMARY KEY CLUSTERED 
(
	[PK_DepoBorder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DepoGroup]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepoGroup](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_DepoGroup] [uniqueidentifier] NOT NULL,
	[FK_Depo] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_DepoGroup] PRIMARY KEY CLUSTERED 
(
	[PK_DepoGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DeviceData]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceData](
	[PK_RowData] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Altitude] [float] NULL,
	[EngineStatus] [varchar](50) NULL,
	[Course] [float] NULL,
	[Temperature] [float] NULL,
	[Fuel] [float] NULL,
	[Speed] [float] NULL,
	[Distance] [decimal](13, 5) NULL,
	[UpdateTime] [datetime] NULL,
	[ServerTime] [datetime] NULL,
 CONSTRAINT [PK_RawData] PRIMARY KEY CLUSTERED 
(
	[PK_RowData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DisplayMessage]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DisplayMessage](
	[PK_DisplayMessage] [varchar](300) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DisplayMessage] PRIMARY KEY CLUSTERED 
(
	[PK_DisplayMessage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[District]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[District](
	[PK_District] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[KeyName] [varchar](100) NULL,
	[division_id] [varchar](300) NULL,
	[bn_name] [nvarchar](50) NULL,
	[lat] [float] NULL,
	[lon] [float] NULL,
	[website] [varchar](100) NULL,
 CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED 
(
	[PK_District] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Driver]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Driver](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_Driver] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Depo] [uniqueidentifier] NULL,
	[Name] [varchar](100) NOT NULL,
	[UniqueIDNumber] [varchar](100) NULL,
	[PhoneNumber] [varchar](100) NULL,
	[NID] [varchar](100) NULL,
	[LisenceNumber] [varchar](100) NULL,
	[LisenceRenewalDate] [date] NULL,
 CONSTRAINT [PK_Driver] PRIMARY KEY CLUSTERED 
(
	[PK_Driver] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Event]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_Event] [uniqueidentifier] NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[FK_EventType] [varchar](100) NOT NULL,
	[OtherEventTypeDetail] [varchar](100) NULL,
	[IssueDate] [date] NULL,
	[DepositDate] [date] NULL,
	[IsAlertable] [bit] NULL,
	[ExpirationDate] [date] NULL,
	[AlertDate] [date] NULL,
	[AlertOn] [bit] NULL,
	[RenewedOn] [date] NULL,
	[PK_RenewedEvent] [uniqueidentifier] NULL,
	[FK_AppUser_RenewedBy] [uniqueidentifier] NULL,
	[Insurance_PolicyNumber] [varchar](100) NULL,
	[PrimaryAmount] [decimal](16, 2) NULL,
	[FitnessPaper_AdvancedIncomeTax] [decimal](16, 2) NULL,
	[Insurance_PremiumAmount] [decimal](16, 2) NULL,
	[Other_Registration_DigitalRegistrationFee] [decimal](16, 2) NULL,
	[Other_Registration_HirePurchase] [decimal](16, 2) NULL,
	[FineAmount] [decimal](16, 2) NULL,
	[AdditionalAmount] [decimal](16, 2) NULL,
	[OtherAmount] [decimal](16, 2) NULL,
	[OtherNote] [varchar](max) NULL,
	[TotalAmount] [decimal](16, 2) NULL,
	[StatusText] [varchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[IssuedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_IssuedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[PK_Event] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EventDocument]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventDocument](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_EventDocument] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Event] [uniqueidentifier] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[IdentitficaitonKey] [varchar](100) NOT NULL,
	[IdentitficaitonValue] [varchar](100) NOT NULL,
	[ImageLocation] [varchar](300) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_EventDocument] PRIMARY KEY CLUSTERED 
(
	[PK_EventDocument] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EventType]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventType](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_EventType] [varchar](100) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[NextCycleDays] [int] NOT NULL,
	[AlertBeforeNextCycleDays] [int] NOT NULL,
 CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED 
(
	[PK_EventType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FinancingCompany]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancingCompany](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_FinancingCompany] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [varchar](300) NOT NULL,
 CONSTRAINT [PK_FinancingCompany] PRIMARY KEY CLUSTERED 
(
	[PK_FinancingCompany] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[PK_FinancingCompany] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GPS_DeviceChangeLog]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GPS_DeviceChangeLog](
	[PK_GPS_DeviceChangeLog] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[GpsDeviceModel] [varchar](300) NULL,
	[GpsIMEINumber] [varchar](50) NULL,
	[GpsMobileNumber] [varchar](100) NULL,
	[FK_AppUser_CreatedBy] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_GPS_DeviceChangeLog] PRIMARY KEY CLUSTERED 
(
	[PK_GPS_DeviceChangeLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GPS_DeviceExisting]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GPS_DeviceExisting](
	[PK_GPS_DeviceExisting] [bigint] IDENTITY(1,1) NOT NULL,
	[GpsDeviceModel] [varchar](300) NULL,
	[GpsIMEINumber] [varchar](300) NULL,
	[GpsMobileNumber] [varchar](100) NULL,
	[PurchaseDate] [datetime] NULL,
	[FK_AppUser_CreatetBy] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NULL,
	[FK_AppUser_UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[StatusText] [varchar](100) NULL,
	[FK_AppUser_StatusUpdatedBy] [uniqueidentifier] NULL,
	[Status_UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_GPS_DeviceExisting] PRIMARY KEY CLUSTERED 
(
	[PK_GPS_DeviceExisting] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__GPS_Devi__679F14B1562FDC1D] UNIQUE NONCLUSTERED 
(
	[GpsIMEINumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GroupOfCompany]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupOfCompany](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_GroupOfCompany] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NULL,
	[IsPranRFLGroup] [bit] NULL,
 CONSTRAINT [PK_GroupOfCompanies] PRIMARY KEY CLUSTERED 
(
	[PK_GroupOfCompany] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Helper]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Helper](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_Helper] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Depo] [uniqueidentifier] NULL,
	[UniqueIDNumber] [varchar](100) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[PhoneNumber] [varchar](100) NULL,
	[NID] [varchar](100) NULL,
 CONSTRAINT [PK_Helper] PRIMARY KEY CLUSTERED 
(
	[PK_Helper] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HiredVehicleDriver]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HiredVehicleDriver](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[PK_HiredVehicleDriver] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[DriverName] [varchar](300) NULL,
	[DriverLiceneseNumber] [varchar](300) NULL,
	[DriverLicenseType] [varchar](300) NULL,
	[DriverContactNumber] [varchar](300) NULL,
	[DriverFatherName] [varchar](300) NULL,
	[DriverAddressVillage] [varchar](300) NULL,
	[DriverAddressPostOfiice] [varchar](300) NULL,
	[DriverAddressThana] [varchar](300) NULL,
	[DriverAddressDistrict] [varchar](300) NULL,
	[DriverNID] [varchar](300) NULL,
	[DriverImageLocation] [varchar](300) NULL,
	[DriverLicenseImageLocation] [varchar](300) NULL,
	[DriverSalary] [bigint] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_VehicleDriver] PRIMARY KEY CLUSTERED 
(
	[PK_HiredVehicleDriver] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IndividualRequisition]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndividualRequisition](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_IndividualRequisition] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NULL,
	[WantedCount] [int] NULL,
	[PossibleJourneyStartDateTime] [datetime] NOT NULL,
	[FK_RequisitionAgent] [uniqueidentifier] NOT NULL,
	[FK_Depo_From] [uniqueidentifier] NULL,
	[StartingLocation] [nvarchar](600) NULL,
	[StartingLatitude] [float] NULL,
	[StartingLongitude] [float] NULL,
	[FK_Depo_To] [uniqueidentifier] NULL,
	[FinishingLocation] [nvarchar](600) NOT NULL,
	[FinishingLatitude] [float] NULL,
	[FinishingLongitude] [float] NULL,
	[VehicleType] [varchar](100) NULL,
	[VehicleTypeLayer1] [varchar](100) NULL,
	[VehicleTypeLayer1_english] [varchar](100) NULL,
	[VehicleTypeLayer1_bangla] [nvarchar](200) NULL,
	[VehicleTypeLayer2] [varchar](100) NULL,
	[VehicleTypeLayer2_english] [varchar](100) NULL,
	[VehicleTypeLayer2_bangla] [nvarchar](200) NULL,
	[VehicleTypeLayer3] [varchar](100) NULL,
	[VehicleTypeLayer3_english] [varchar](100) NULL,
	[VehicleTypeLayer3_bangla] [nvarchar](200) NULL,
	[VehicleCapacity] [varchar](100) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[IsEndLevelRequisition] [bit] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_IndividualRequisition] PRIMARY KEY CLUSTERED 
(
	[PK_IndividualRequisition] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[IndividualRequisitionBidding]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndividualRequisitionBidding](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_IndividualRequisitionBidding] [uniqueidentifier] NOT NULL,
	[FK_IndividualRequisition] [uniqueidentifier] NOT NULL,
	[FK_RequisitionAgent_Bidder] [uniqueidentifier] NOT NULL,
	[ManagableQuantity] [int] NULL,
	[PricePerQuantity] [bigint] NULL,
	[ApprovedQuantity] [int] NULL,
	[Status] [int] NULL,
	[BidderRating] [int] NULL,
	[BidderRatingNote] [varchar](300) NULL,
 CONSTRAINT [PK_IndividualRequisitionBidding] PRIMARY KEY CLUSTERED 
(
	[PK_IndividualRequisitionBidding] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InstantRequisition]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstantRequisition](
	[PK_InstantRequisition] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Status] [int] NOT NULL,
	[FK_RequisitionAgent] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[REF_FK_Depo] [uniqueidentifier] NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[DriverStaffID] [varchar](100) NULL,
	[DriverName] [varchar](300) NULL,
	[HelperInfo] [varchar](300) NULL,
	[FK_Depo_Booking] [uniqueidentifier] NULL,
	[FK_Depo_Destination] [uniqueidentifier] NULL,
	[StartingLocation] [varchar](600) NOT NULL,
	[StartingLatitude] [float] NULL,
	[StartingLongitude] [float] NULL,
	[FinishingLocation] [varchar](600) NOT NULL,
	[FinishingLatitude] [float] NULL,
	[FinishingLongitude] [float] NULL,
	[TripFare1] [bigint] NOT NULL,
	[AdvancedToDriver1] [bigint] NULL,
	[Distance_Empty1] [float] NULL,
	[KPL_Empty1] [float] NULL,
	[Distance_Loaded1] [float] NULL,
	[KPL_Loaded1] [float] NULL,
	[Distance_Loaded_8_To_12_Tons_Out1] [float] NULL,
	[KPL_Loaded_8_To_12_Tons_Out1] [float] NULL,
	[Distance_Loaded_12_To_25_Tons_Out1] [float] NULL,
	[KPL_Loaded_12_To_25_Tons_Out1] [float] NULL,
	[FuelConsumedLitre1] [float] NULL,
	[FuelPricePerLitre1] [float] NULL,
	[FuelExpence1] [decimal](16, 2) NULL,
	[FuelExpenceGivenCashOrOil] [bit] NULL,
	[StayCharge1] [decimal](16, 2) NULL,
	[TripBillDriver1] [decimal](16, 2) NULL,
	[TripBillHelper1] [decimal](16, 2) NULL,
	[EntertainmentACharge1] [decimal](16, 2) NULL,
	[RepairCharge1] [decimal](16, 2) NULL,
	[BridgeTollFerryCharge1] [decimal](16, 2) NULL,
	[OpenBodyCharge1] [decimal](16, 2) NULL,
	[TransportAgencyName] [varchar](300) NULL,
	[TransportAgencyContactNumber] [varchar](300) NULL,
	[ResponsibleParsonName] [varchar](300) NULL,
	[ResponsibleParsonContactNumber] [varchar](300) NULL,
	[Note1] [varchar](max) NULL,
	[TotalParent1] [decimal](16, 2) NULL,
	[PolicyKey] [varchar](100) NULL,
	[PolicyDetail] [varchar](max) NULL,
	[BillPaidAt] [datetime] NULL,
	[FK_AppUser_BillPaidBy] [uniqueidentifier] NULL,
	[DriversMoney2] [decimal](16, 2) NULL,
	[HelpersMoney2] [decimal](16, 2) NULL,
	[TPTsMoney2] [decimal](16, 2) NULL,
	[ScaleCharge2] [decimal](16, 2) NULL,
	[NightStayCharge2] [decimal](16, 2) NULL,
	[StayCharge2] [decimal](16, 2) NULL,
	[Distance_Empty2] [float] NULL,
	[KPL_Empty2] [float] NULL,
	[Distance_Loaded2] [float] NULL,
	[KPL_Loaded2] [float] NULL,
	[Distance_Loaded_8_To_12_Tons_Out2] [float] NULL,
	[KPL_Loaded_8_To_12_Tons_Out2] [float] NULL,
	[Distance_Loaded_12_To_25_Tons_Out2] [float] NULL,
	[KPL_Loaded_12_To_25_Tons_Out2] [float] NULL,
	[FuelConsumedLitre2] [float] NULL,
	[FuelPricePerLitre2] [float] NULL,
	[FuelExpence2] [decimal](16, 2) NULL,
	[LaborCharge2] [decimal](16, 2) NULL,
	[RepairCharge2] [decimal](16, 2) NULL,
	[BridgeTollFerryCharge2] [decimal](16, 2) NULL,
	[EntertainmentCCharge2] [decimal](16, 2) NULL,
	[ParkingCharge2] [decimal](16, 2) NULL,
	[TransportCommissionCharge2] [decimal](16, 2) NULL,
	[EntertainmentACharge2] [decimal](16, 2) NULL,
	[GiveDemerageCharge2] [decimal](16, 2) NULL,
	[TotalGiven2] [decimal](16, 2) NULL,
	[Note2] [varchar](max) NULL,
	[Distance_Empty3] [float] NULL,
	[KPL_Empty3] [float] NULL,
	[Distance_Loaded3] [float] NULL,
	[KPL_Loaded3] [float] NULL,
	[Distance_Loaded_8_To_12_Tons_Out3] [float] NULL,
	[KPL_Loaded_8_To_12_Tons_Out3] [float] NULL,
	[Distance_Loaded_12_To_25_Tons_Out3] [float] NULL,
	[KPL_Loaded_12_To_25_Tons_Out3] [float] NULL,
	[FuelConsumedLitre3] [float] NULL,
	[FuelPricePerLitre3] [float] NULL,
	[FuelExpence3] [decimal](16, 2) NULL,
	[BridgeTollFerryCharge3] [decimal](16, 2) NULL,
	[TakeDemerageCharge3] [decimal](16, 2) NULL,
	[TPTsMoney3] [decimal](16, 2) NULL,
	[OthersCharge3] [decimal](16, 2) NULL,
	[TotalTaken3] [decimal](16, 2) NULL,
	[NetProfit3] [decimal](16, 2) NULL,
	[GrossTaken3] [decimal](16, 2) NULL,
	[DepositToAccountLocation] [varchar](100) NULL,
	[Note3] [varchar](max) NULL,
	[AdjustedAt] [datetime] NULL,
	[FK_AppUser_AdjustedBy] [uniqueidentifier] NULL,
	[NetProfit4] [decimal](16, 2) NULL,
	[GrossProfit4] [decimal](16, 2) NULL,
	[DepositeToAcccountLocation] [varchar](50) NULL,
	[MRR] [varchar](50) NULL,
 CONSTRAINT [PK_InstantRequisition_1] PRIMARY KEY CLUSTERED 
(
	[PK_InstantRequisition] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterCompanyRequisition]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterCompanyRequisition](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_InterCompanyRequisition] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NULL,
	[IsTest] [bit] NULL,
	[WantedCount] [int] NULL,
	[AcceptedCount] [int] NULL,
	[PossibleJourneyStartDateTime] [datetime] NOT NULL,
	[FK_AppUser_Client] [uniqueidentifier] NOT NULL,
	[FK_ReferenceDepo] [uniqueidentifier] NULL,
	[FK_AppUser_Approver] [uniqueidentifier] NULL,
	[FK_InterCompanyRequisitionLocation_From] [uniqueidentifier] NULL,
	[StartingLocation] [nvarchar](600) NULL,
	[StartingLatitude] [float] NULL,
	[StartingLongitude] [float] NULL,
	[FK_InterCompanyRequisitionLocation_To] [uniqueidentifier] NULL,
	[FinishingLocation] [nvarchar](600) NULL,
	[FinishingLatitude] [float] NULL,
	[FinishingLongitude] [float] NULL,
	[FK_RequisitionVehicleType] [int] NULL,
	[VehicleType] [varchar](100) NULL,
	[VehicleTypeLayer1] [varchar](100) NULL,
	[VehicleTypeLayer1_english] [varchar](100) NULL,
	[VehicleTypeLayer1_bangla] [nvarchar](200) NULL,
	[VehicleTypeLayer2] [varchar](100) NULL,
	[VehicleTypeLayer2_english] [varchar](100) NULL,
	[VehicleTypeLayer2_bangla] [nvarchar](200) NULL,
	[VehicleTypeLayer3] [varchar](100) NULL,
	[VehicleTypeLayer3_english] [varchar](100) NULL,
	[VehicleTypeLayer3_bangla] [nvarchar](200) NULL,
	[VehicleCapacity] [varchar](100) NULL,
	[ClientNote] [varchar](max) NULL,
	[ApproverNote] [varchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[IsEndLevelRequisition] [bit] NULL,
	[Status] [int] NULL,
	[VerifiedAt] [datetime] NULL,
	[HiredVehicleCount_Internal] [int] NULL,
	[HiredVehicleCount_External] [int] NULL,
 CONSTRAINT [PK_InterCompanyRequisition] PRIMARY KEY CLUSTERED 
(
	[PK_InterCompanyRequisition] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterCompanyRequisition_ExternalTroller]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterCompanyRequisition_ExternalTroller](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_InterCompanyRequisition_ExternalTroller] [uniqueidentifier] NOT NULL,
	[FK_InterCompanyRequisition] [uniqueidentifier] NOT NULL,
	[TrollerNumber] [nchar](200) NOT NULL,
	[Note] [varchar](100) NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_InterCompanyRequisition_ExternalTroller] PRIMARY KEY CLUSTERED 
(
	[PK_InterCompanyRequisition_ExternalTroller] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterCompanyRequisition_ExternalVehicle]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterCompanyRequisition_ExternalVehicle](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_InterCompanyRequisition_ExternalVehicle] [uniqueidentifier] NOT NULL,
	[FK_InterCompanyRequisition] [uniqueidentifier] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[Note] [varchar](100) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[HiredAmount] [bigint] NULL,
 CONSTRAINT [PK_InterCompanyRequisition_ExternalVehicle] PRIMARY KEY CLUSTERED 
(
	[PK_InterCompanyRequisition_ExternalVehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterCompanyRequisition_InternalVehicle]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterCompanyRequisition_InternalVehicle](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_InterCompanyRequisition_InternalVehicle] [uniqueidentifier] NOT NULL,
	[FK_InterCompanyRequisition] [uniqueidentifier] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[Note] [varchar](100) NULL,
 CONSTRAINT [PK_InterCompanyRequisition_Vehicle] PRIMARY KEY CLUSTERED 
(
	[PK_InterCompanyRequisition_InternalVehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterCompanyRequisitionBidding]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterCompanyRequisitionBidding](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_InterCompanyRequisitionBidding] [uniqueidentifier] NOT NULL,
	[FK_InterCompanyRequisition] [uniqueidentifier] NOT NULL,
	[FK_RequisitionAgent_Bidder] [uniqueidentifier] NOT NULL,
	[ManagableQuantity] [int] NULL,
	[PricePerQuantity] [bigint] NULL,
	[ApprovedQuantity] [int] NULL,
	[Status] [int] NULL,
	[BidderRating] [int] NULL,
	[BidderRatingNote] [varchar](300) NULL,
 CONSTRAINT [PK_InterCompanyRequisitionBidding] PRIMARY KEY CLUSTERED 
(
	[PK_InterCompanyRequisitionBidding] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InterCompanyRequisitionLocation]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterCompanyRequisitionLocation](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_InterCompanyRequisitionLocation] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Cetegory] [varchar](100) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_InterCompanyRequisitionLocation] PRIMARY KEY CLUSTERED 
(
	[PK_InterCompanyRequisitionLocation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KPLChangeLog]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KPLChangeLog](
	[PK_KPLChangeLog] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[KPL_Loaded] [float] NULL,
	[KPL_Loaded_Plastic] [float] NULL,
	[KPL_Empty] [float] NULL,
	[KPL_InterCity] [float] NULL,
	[KPL_InterCHT] [float] NULL,
	[KPL_Hill] [float] NULL,
	[KPL_OnlyMover] [float] NULL,
	[KPL_Loaded_8_To_12_Tons] [float] NULL,
	[KPL_Loaded_12_To_25_Tons] [float] NULL,
	[KPL_Loaded_8_To_12_Tons_Out] [float] NULL,
	[KPL_Loaded_12_To_25_Tons_Out] [float] NULL,
 CONSTRAINT [PK_KPLChangeLog] PRIMARY KEY CLUSTERED 
(
	[PK_KPLChangeLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_Location] [uniqueidentifier] NOT NULL,
	[DevelopersNote] [varchar](50) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Category] [varchar](100) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Code] [varchar](100) NULL,
	[Latitude] [varchar](100) NULL,
	[Longitude] [varchar](100) NULL,
	[CreatedAt] [datetime] NULL,
	[PRG_Type] [varchar](100) NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
	[LocationType] [varchar](100) NULL,
	[LocationZone] [varchar](100) NULL,
	[ShortCode] [varchar](50) NULL,
	[DIC_PRAN_Name] [varchar](50) NULL,
	[DIC_PRAN_Contact] [varchar](50) NULL,
	[DIC_RFL_Name] [varchar](50) NULL,
	[DIC_RFL_Contact] [varchar](50) NULL,
	[SiteCode] [varchar](20) NULL,
 CONSTRAINT [PK_Location_Physical] PRIMARY KEY CLUSTERED 
(
	[PK_Location] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__Location__737584F627594474] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationToLocationMapping]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationToLocationMapping](
	[PK_LocationToLocationMapping] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Location_1] [uniqueidentifier] NOT NULL,
	[FK_Location_2] [uniqueidentifier] NOT NULL,
	[StandardTravelTimeMinute] [int] NULL,
	[StandardDistanceKM] [int] NULL,
 CONSTRAINT [PK_LocationToLocationMapping] PRIMARY KEY CLUSTERED 
(
	[PK_LocationToLocationMapping] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationWiseGP]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationWiseGP](
	[PK_LocationWiseGP] [bigint] IDENTITY(1,1) NOT NULL,
	[PRG_Type] [varchar](100) NOT NULL,
	[IssueDate] [date] NOT NULL,
	[FK_Location] [uniqueidentifier] NOT NULL,
	[GPCount] [bigint] NOT NULL,
	[FK_AppUser_Creator] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_LocationWiseGP] PRIMARY KEY CLUSTERED 
(
	[PK_LocationWiseGP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapLocation]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapLocation](
	[PK_MapLoaction] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_MapLocation] PRIMARY KEY CLUSTERED 
(
	[PK_MapLoaction] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MobileMenu]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MobileMenu](
	[PK_MobileMenu] [bigint] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](300) NOT NULL,
	[VisibleName] [varchar](300) NOT NULL,
	[ModelName] [varchar](300) NOT NULL,
	[Icon] [varchar](100) NOT NULL,
	[Link] [varchar](300) NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_MobileMenu] PRIMARY KEY CLUSTERED 
(
	[PK_MobileMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MobileRole]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MobileRole](
	[PK_MobileRole] [bigint] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](300) NOT NULL,
	[Note] [varchar](300) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_MobileUserRole] PRIMARY KEY CLUSTERED 
(
	[PK_MobileRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MobileRole_MobileMenu]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MobileRole_MobileMenu](
	[PK_MobileRole_MobileMenu] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_MobileRole] [bigint] NOT NULL,
	[FK_MobileMenu] [bigint] NOT NULL,
	[IsAccessible] [bit] NULL,
	[Sequence] [int] NULL,
 CONSTRAINT [PK_MobileUserRole_MobileMenu] PRIMARY KEY CLUSTERED 
(
	[PK_MobileRole_MobileMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MonthlyBillEntry]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonthlyBillEntry](
	[PK_MonthlyBillEntry] [bigint] IDENTITY(1,1) NOT NULL,
	[PRG_Type] [varchar](20) NULL,
	[DB_Month] [varchar](50) NULL,
	[FK_Depo] [uniqueidentifier] NULL,
	[DepoName] [varchar](100) NULL,
	[CompanyName] [varchar](100) NULL,
	[RegistrationNumber] [varchar](30) NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[DateCount] [int] NULL,
 CONSTRAINT [PK_MonthlyBillEntry] PRIMARY KEY CLUSTERED 
(
	[PK_MonthlyBillEntry] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PoliceCase]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PoliceCase](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_PoliceCase] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[AccusedDriverStaffID] [varchar](100) NULL,
	[AccusedDriverName] [varchar](300) NULL,
	[CaseID] [varchar](50) NULL,
	[PoliceContactNumber] [varchar](50) NULL,
	[FK_District] [bigint] NULL,
	[FK_Upazila] [bigint] NULL,
	[TypeOfFault] [varchar](max) NULL,
	[Note] [varchar](max) NULL,
	[IssueDate] [date] NULL,
	[IsAlertable] [bit] NULL,
	[AlertDate] [date] NULL,
	[IsSolved] [bit] NULL,
	[SolvedEntryGivenAt] [datetime] NULL,
	[FK_SolvedEntryGivenUser] [uniqueidentifier] NULL,
	[IsPaid] [bit] NULL,
	[PaidEntryGivenAt] [datetime] NULL,
	[FK_PaidEntryGivenUser] [uniqueidentifier] NULL,
	[PrimaryAmount] [bigint] NULL,
	[OtherAmount] [bigint] NULL,
	[OtherNote] [varchar](max) NULL,
	[TotalAmount] [bigint] NULL,
	[CreatedAt] [datetime] NULL,
	[SolvedOn] [date] NULL,
	[SolvedNote] [varchar](max) NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PK_Case] PRIMARY KEY CLUSTERED 
(
	[PK_PoliceCase] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PoliceCase_PoliceCaseLaw]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PoliceCase_PoliceCaseLaw](
	[PK_PoliceCase_PoliceCaseLaw] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_PoliceCase] [uniqueidentifier] NOT NULL,
	[FK_PoliceCaseLaw] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PoliceCase_PoliceCaseLaw] PRIMARY KEY CLUSTERED 
(
	[PK_PoliceCase_PoliceCaseLaw] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PoliceCaseDocument]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PoliceCaseDocument](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_PoliceCaseDocument] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_PoliceCase] [uniqueidentifier] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[IdentitficaitonKey] [varchar](100) NOT NULL,
	[IdentitficaitonValue] [varchar](100) NOT NULL,
	[ImageLocation] [varchar](300) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PoliceCaseDocument] PRIMARY KEY CLUSTERED 
(
	[PK_PoliceCaseDocument] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PoliceCaseLaw]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PoliceCaseLaw](
	[PK_PoliceCaseLaw] [varchar](50) NOT NULL,
	[LawDetail] [nvarchar](300) NULL,
 CONSTRAINT [PK_PoliceCaseLaw] PRIMARY KEY CLUSTERED 
(
	[PK_PoliceCaseLaw] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PRG_Type]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PRG_Type](
	[PK_PRG_Type] [bigint] NOT NULL,
	[Title] [varchar](300) NULL,
	[Sequence] [int] NULL,
	[IsDeleted] [bit] NULL,
	[Show_VehicleGateInOutManual] [bit] NULL,
 CONSTRAINT [PK_VehicleInOutManual_PRG_Type] PRIMARY KEY CLUSTERED 
(
	[PK_PRG_Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReadyReport]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReadyReport](
	[PK_RadyReport] [bigint] IDENTITY(1,1) NOT NULL,
	[BaseModule] [varchar](100) NOT NULL,
	[ReportName] [varchar](100) NOT NULL,
	[PRG_Type] [varchar](100) NULL,
	[Key] [varchar](100) NULL,
	[Key_nvarchar] [nvarchar](300) NULL,
	[Value1] [decimal](16, 2) NULL,
	[Value2] [decimal](16, 2) NULL,
	[Value_varchar] [varchar](100) NULL,
 CONSTRAINT [PK_ReadyReport] PRIMARY KEY CLUSTERED 
(
	[PK_RadyReport] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_ConsolidatedRport]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_ConsolidatedRport](
	[PK_RowData] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [varchar](100) NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Altitude] [float] NULL,
	[EngineStatus] [varchar](50) NULL,
	[Course] [float] NULL,
	[Temperature] [float] NULL,
	[Fuel] [float] NULL,
	[Speed] [float] NULL,
	[Distance] [decimal](13, 5) NULL,
	[UpdateTime] [datetime] NULL,
	[ServerTime] [datetime] NULL,
 CONSTRAINT [PK_Report_ConsolidatedRport] PRIMARY KEY CLUSTERED 
(
	[PK_RowData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_TemperatureReport]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_TemperatureReport](
	[PK_Report_TemperatureReport] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [uniqueidentifier] NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
	[Temperature] [float] NULL,
 CONSTRAINT [PK_Report_TemperatureReport] PRIMARY KEY CLUSTERED 
(
	[PK_Report_TemperatureReport] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_TemperatureReport_Helper]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_TemperatureReport_Helper](
	[PK_Report_TemperatureReport] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [uniqueidentifier] NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NULL,
	[Temperature] [float] NULL,
 CONSTRAINT [PK_Report_TemperatureReport_Helper] PRIMARY KEY CLUSTERED 
(
	[PK_Report_TemperatureReport] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleConsolidatedReport]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleConsolidatedReport](
	[PK] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [varchar](100) NULL,
	[_rowType] [varchar](100) NULL,
	[ThisDate] [varchar](100) NULL,
	[RegistrationNumber] [varchar](100) NULL,
	[MobileNumber] [varchar](100) NULL,
	[LastUpdate] [varchar](100) NULL,
	[LastLocaitonNearName] [varchar](100) NULL,
	[LastLocaitonNearDistance] [varchar](100) NULL,
	[TotalHaltTime] [varchar](100) NULL,
	[HaltCount] [varchar](100) NULL,
	[AverageHaltTime] [varchar](100) NULL,
	[MaximumHaltTime] [varchar](100) NULL,
	[TotalRunTime] [varchar](100) NULL,
	[TotalDistance] [varchar](100) NULL,
	[MaximumSpeed] [varchar](100) NULL,
 CONSTRAINT [PK_Report_VehicleConsolidatedReport] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleHaltReadyReport_Helper]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleHaltReadyReport_Helper](
	[PK_RowData] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Altitude] [float] NULL,
	[EngineStatus] [varchar](50) NULL,
	[Course] [float] NULL,
	[Temperature] [float] NULL,
	[Fuel] [float] NULL,
	[Speed] [float] NULL,
	[Distance] [decimal](13, 5) NULL,
	[UpdateTime] [datetime] NULL,
	[ServerTime] [datetime] NULL,
 CONSTRAINT [PK_Report_VehicleHaltReport_Helper_2] PRIMARY KEY CLUSTERED 
(
	[PK_RowData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleHaltReadyReport_Helper_Helper]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleHaltReadyReport_Helper_Helper](
	[PK_RowData] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Altitude] [float] NULL,
	[EngineStatus] [varchar](50) NULL,
	[Course] [float] NULL,
	[Temperature] [float] NULL,
	[Fuel] [float] NULL,
	[Speed] [float] NULL,
	[Distance] [decimal](13, 5) NULL,
	[UpdateTime] [datetime] NULL,
	[ServerTime] [datetime] NULL,
 CONSTRAINT [Report_VehicleHaltReport_Helper_Helper_] PRIMARY KEY CLUSTERED 
(
	[PK_RowData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleHaltReport]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleHaltReport](
	[PK] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [varchar](100) NULL,
	[_rowType] [varchar](100) NULL,
	[PK_RowData_Start] [varchar](100) NULL,
	[PK_RowData_End] [varchar](100) NULL,
	[VehicleRegistrationNumber] [varchar](100) NULL,
	[StartTime] [varchar](100) NULL,
	[EndTime] [varchar](100) NULL,
	[Latitude] [varchar](100) NULL,
	[Longitude] [varchar](100) NULL,
	[HaltTime] [varchar](100) NULL,
	[NearestMapLocation] [varchar](100) NULL,
	[NearestMapLocationDistance] [varchar](100) NULL,
	[EngineStatus] [varchar](50) NULL,
	[Speed] [varchar](50) NULL,
	[_DateString] [varchar](100) NULL,
	[_NumberOfHalt] [varchar](100) NULL,
 CONSTRAINT [PK_Report_VehicleHaltReport] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleHaltReport_Helper]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleHaltReport_Helper](
	[PK_RowData] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [varchar](100) NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Altitude] [float] NULL,
	[EngineStatus] [varchar](50) NULL,
	[Course] [float] NULL,
	[Temperature] [float] NULL,
	[Fuel] [float] NULL,
	[Speed] [float] NULL,
	[Distance] [decimal](13, 5) NULL,
	[UpdateTime] [datetime] NULL,
	[ServerTime] [datetime] NULL,
 CONSTRAINT [PK_Report_VehicleHaltReport_Helper] PRIMARY KEY CLUSTERED 
(
	[PK_RowData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleHistory]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleHistory](
	[PK] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [uniqueidentifier] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[EngineStatus] [varchar](50) NOT NULL,
	[Speed] [float] NOT NULL,
	[NearestMapLocation] [varchar](100) NOT NULL,
	[NearestMapLocationDistance] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Report_VehicleHistory] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleHistoryDetail]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleHistoryDetail](
	[PK] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [varchar](100) NULL,
	[FK_Vehicle] [varchar](100) NULL,
	[UpdateTime] [varchar](100) NULL,
	[Latitude] [varchar](100) NULL,
	[Longitude] [varchar](100) NULL,
	[EngineStatus] [varchar](100) NULL,
	[Speed] [varchar](100) NULL,
 CONSTRAINT [PK_Report_VehicleHistoryDetail] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleInOutHistoryDetail]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleInOutHistoryDetail](
	[PK] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [uniqueidentifier] NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[UpdateTime] [datetime] NOT NULL,
	[FK_Depo] [uniqueidentifier] NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_Report_VehicleInOutHistoryDetail] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Report_VehicleOutOverStay]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleOutOverStay](
	[PK_Report_VehicleOutOverStay] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[Start_UpdateTime] [datetime] NOT NULL,
	[Start_Latitude] [float] NOT NULL,
	[Start_Longitude] [float] NOT NULL,
	[Start_EngineStatus] [varchar](50) NOT NULL,
	[Start_Speed] [float] NOT NULL,
	[Start_NearestMapLocation] [varchar](300) NOT NULL,
	[Start_NearestMapLocationDistance] [varchar](50) NOT NULL,
	[Finish_UpdateTime] [datetime] NOT NULL,
	[Finish_Latitude] [float] NOT NULL,
	[Finish_Longitude] [float] NOT NULL,
	[Finish_EngineStatus] [varchar](50) NULL,
	[Finish_Speed] [float] NOT NULL,
	[Finish_NearestMapLocation] [varchar](300) NOT NULL,
	[Finish_NearestMapLocationDistance] [varchar](50) NOT NULL,
	[StayTimeMinute] [int] NOT NULL,
 CONSTRAINT [PK_VehicleOutOverStay] PRIMARY KEY CLUSTERED 
(
	[PK_Report_VehicleOutOverStay] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Requisition]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requisition](
	[PK_Requisition] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[TrackingID] [varchar](100) NULL,
	[FK_Depo_From] [uniqueidentifier] NOT NULL,
	[FK_Location_From] [uniqueidentifier] NULL,
	[StartingLocation] [varchar](300) NULL,
	[FK_Depo_To] [uniqueidentifier] NOT NULL,
	[FK_Location_To] [uniqueidentifier] NULL,
	[FinishingLocation] [varchar](300) NULL,
	[PossibleJourneyStartDateTime] [datetime] NOT NULL,
	[FK_RequisitionVehicleType] [int] NULL,
	[FK_RequisitionBusinessUnit] [bigint] NULL,
	[FK_RequisitionProductType] [bigint] NULL,
	[WantedCount] [float] NULL,
	[LoadedOrEmpty] [bit] NULL,
	[ClientNote] [varchar](max) NULL,
	[StatusText] [varchar](50) NULL,
	[FK_AppUser_Client] [uniqueidentifier] NOT NULL,
	[PRG_Type] [varchar](50) NULL,
	[FK_ReferenceDepo] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[FK_AppUser_Verifier] [uniqueidentifier] NULL,
	[VerifiedAt] [datetime] NULL,
	[AcceptedCount] [float] NULL,
 CONSTRAINT [PK_InterCompanyRequisitionNew_1] PRIMARY KEY CLUSTERED 
(
	[PK_Requisition] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequisitionAgentNotification]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionAgentNotification](
	[PK_RequisitionAgentNotification] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_RequisitionAgent] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[SubTitle] [varchar](300) NOT NULL,
	[ViewLink] [varchar](300) NULL,
	[Status] [int] NOT NULL,
	[Category] [varchar](100) NULL,
 CONSTRAINT [PK_RequisitionAgentNotification_1] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionAgentNotification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequisitionAgentProposedDepo]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionAgentProposedDepo](
	[PK_RequisitionAgentProposedDepo] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_RequisitionAgent] [uniqueidentifier] NOT NULL,
	[FK_Depo] [uniqueidentifier] NOT NULL,
	[WillPropose] [bit] NOT NULL,
 CONSTRAINT [PK_RequisitionAgentProposedDepo] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionAgentProposedDepo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequisitionBusinessUnit]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionBusinessUnit](
	[PK_RequisitionBusinessUnit] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_InterCompanyRequisitionNewBusinessUnit] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionBusinessUnit] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequisitionProductType]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionProductType](
	[PK_RequisitionProductType] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_InterCompanyRequisitionNewProductType] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionProductType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequisitionTrip]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionTrip](
	[PK_RequisitionTrip] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[TrackingID] [varchar](100) NULL,
	[FK_Requisition] [bigint] NOT NULL,
	[WantedCount] [float] NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[OWN_MHT_DHT] [varchar](50) NULL,
	[Driver_Staff_ID] [varchar](50) NULL,
	[Driver_Name] [varchar](300) NULL,
	[Driver_ContactNumber] [varchar](100) NULL,
	[TotalAmount] [bigint] NULL,
	[FK_AppUser_Assigner] [uniqueidentifier] NULL,
	[FinalWantedAtDateTime] [datetime] NULL,
	[AssingedAt] [datetime] NULL,
	[TentativeFinishingDateTime] [datetime] NULL,
	[FK_AppUser_Canceller] [uniqueidentifier] NULL,
	[CancelledAt] [datetime] NULL,
	[StatusText] [varchar](50) NULL,
	[StartedAt] [datetime] NULL,
	[FK_AppUser_Start] [uniqueidentifier] NULL,
	[FK_LocationGate_Start] [uniqueidentifier] NULL,
	[StartAutoOrManaul] [varchar](50) NULL,
	[FinishedAt] [datetime] NULL,
	[FK_AppUser_Finish] [uniqueidentifier] NULL,
	[FK_LocationGate_Finish] [uniqueidentifier] NULL,
	[FinishAutoOrManaul] [varchar](50) NULL,
	[IsParent] [bit] NULL,
	[FK_RequisitionTrip_Parent] [bigint] NULL,
	[IsForwarded] [bit] NULL,
	[FK_Location_ForwardedTo] [uniqueidentifier] NULL,
	[PRG_Type_ForwaredTo] [varchar](50) NULL,
	[ForwardedAt] [datetime] NULL,
	[FK_AppUser_ForwardedBy] [uniqueidentifier] NULL,
	[OracleDB_IsPushed] [bit] NULL,
	[OracleDB_PushedAt] [datetime] NULL,
	[OracleDB_IsPulled] [bit] NULL,
	[OracleDB_PulledAt] [datetime] NULL,
	[OracleDB_GPNumber] [varchar](50) NULL,
 CONSTRAINT [PK_RequisitionTrip] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionTrip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequisitionTrip_Finished]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionTrip_Finished](
	[PK_RequisitionTrip_Finished] [bigint] NOT NULL,
	[IsDeleted] [bit] NULL,
	[TrackingID] [varchar](100) NULL,
	[FK_Requisition] [bigint] NOT NULL,
	[WantedCount] [float] NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[OWN_MHT_DHT] [varchar](50) NULL,
	[Driver_Staff_ID] [varchar](50) NULL,
	[Driver_Name] [varchar](300) NULL,
	[Driver_ContactNumber] [varchar](100) NULL,
	[TotalAmount] [bigint] NULL,
	[FK_AppUser_Assigner] [uniqueidentifier] NULL,
	[FinalWantedAtDateTime] [datetime] NULL,
	[AssingedAt] [datetime] NULL,
	[TentativeFinishingDateTime] [datetime] NULL,
	[FK_AppUser_Canceller] [uniqueidentifier] NULL,
	[CancelledAt] [datetime] NULL,
	[StatusText] [varchar](50) NULL,
	[StartedAt] [datetime] NULL,
	[FK_AppUser_Start] [uniqueidentifier] NULL,
	[FK_LocationGate_Start] [uniqueidentifier] NULL,
	[StartAutoOrManaul] [varchar](50) NULL,
	[FinishedAt] [datetime] NULL,
	[FK_AppUser_Finish] [uniqueidentifier] NULL,
	[FK_LocationGate_Finish] [uniqueidentifier] NULL,
	[FinishAutoOrManaul] [varchar](50) NULL,
	[IsParent] [bit] NULL,
	[FK_RequisitionTrip_Finished_Parent] [bigint] NULL,
	[IsForwarded] [bit] NULL,
	[FK_Location_ForwardedTo] [uniqueidentifier] NULL,
	[PRG_Type_ForwaredTo] [varchar](50) NULL,
	[ForwardedAt] [datetime] NULL,
	[FK_AppUser_ForwardedBy] [uniqueidentifier] NULL,
	[OracleDB_IsPushed] [bit] NULL,
	[OracleDB_PushedAt] [datetime] NULL,
	[OracleDB_IsPulled] [bit] NULL,
	[OracleDB_PulledAt] [datetime] NULL,
	[OracleDB_GPNumber] [varchar](50) NULL,
 CONSTRAINT [PK_RequisitionTrip_Finished] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionTrip_Finished] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequisitionVehicleType]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionVehicleType](
	[PK_RequisitionVehicleType] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[Title_English] [varchar](100) NULL,
	[Title_Bangla] [nvarchar](200) NULL,
	[Layer1] [varchar](100) NULL,
	[Layer1_english] [varchar](100) NULL,
	[Layer1_bangla] [nvarchar](200) NULL,
	[Layer2] [varchar](100) NULL,
	[Layer2_english] [varchar](100) NULL,
	[Layer2_bangla] [nvarchar](200) NULL,
	[Layer3] [varchar](100) NULL,
	[Layer3_english] [varchar](100) NULL,
	[Layer3_bangla] [nvarchar](200) NULL,
 CONSTRAINT [PK_RequisitionVehicleType] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionVehicleType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RFID_AutoDistSuggession]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFID_AutoDistSuggession](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RawText] [varchar](20) NULL,
	[DistText] [varchar](20) NULL,
 CONSTRAINT [PK_RFID_AutoDistSuggession] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RFID_AutoLetterSuggession]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFID_AutoLetterSuggession](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RawText] [varchar](20) NULL,
	[LetterText] [varchar](20) NULL,
 CONSTRAINT [PK_RFID_AutoLetterSuggession] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RFID_Entry]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFID_Entry](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TotalText_Raw] [varchar](50) NULL,
	[DistText_Raw] [varchar](20) NULL,
	[FK_Dist_ID] [int] NULL,
	[DistText_Auto] [varchar](20) NULL,
	[DistText_Manual] [varchar](20) NULL,
	[DistText_API] [varchar](20) NULL,
	[LetterText_Raw] [varchar](20) NULL,
	[FK_Letter_ID] [int] NULL,
	[LetterText_Auto] [varchar](20) NULL,
	[LetterText_Manual] [varchar](20) NULL,
	[LetterText_API] [varchar](20) NULL,
	[LastText_Raw] [varchar](20) NULL,
	[LastText_Manual] [varchar](20) NULL,
	[TotalText_API] [varchar](50) NULL,
	[TotalTextMutiple_API] [varchar](1000) NULL,
	[LastEntryAt] [datetime] NULL,
	[Status] [int] NULL,
	[GateInID] [varchar](20) NULL,
	[GateInDateTime] [datetime] NULL,
	[GateOutID] [varchar](20) NULL,
	[GateOutDateTime] [datetime] NULL,
 CONSTRAINT [PK_RFID_Entry] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RFID_EntryLog]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RFID_EntryLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TotalText_Raw] [varchar](50) NULL,
	[FK_RFID_Entry] [bigint] NULL,
	[GateID] [varchar](20) NULL,
	[EntryLogedAt] [datetime] NULL,
 CONSTRAINT [PK_RFID_EntryLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RouteChart]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RouteChart](
	[PK_RouteChart] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Depo1] [uniqueidentifier] NOT NULL,
	[FK_Depo2] [uniqueidentifier] NOT NULL,
	[Distance] [float] NOT NULL,
	[DriversMoney] [decimal](16, 2) NOT NULL,
	[HelpersMoney] [decimal](16, 2) NOT NULL,
	[ApproxTimeHour] [int] NOT NULL,
 CONSTRAINT [PK_RouteChart] PRIMARY KEY CLUSTERED 
(
	[PK_RouteChart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceCall]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceCall](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[CallingMessage] [varchar](max) NULL,
	[UserDefinedMessage] [varchar](max) NULL,
	[CallingTime] [datetime] NULL,
 CONSTRAINT [PK_ServiceCall] PRIMARY KEY CLUSTERED 
(
	[RowSerial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TempMail]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempMail](
	[MailAddress] [varchar](100) NULL,
	[MailLevel] [int] NULL,
	[PK_Temp] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TempMail_1] PRIMARY KEY CLUSTERED 
(
	[PK_Temp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TemporaryVehicle]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemporaryVehicle](
	[PK_TemporaryVehicle] [bigint] IDENTITY(1,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[RegistrationNumber] [varchar](100) NULL,
	[FK_Locaiton] [uniqueidentifier] NULL,
	[FK_PRG_Type] [bigint] NULL,
	[FK_CreatedByLocationGate] [uniqueidentifier] NULL,
	[GPNumber] [varchar](50) NULL,
	[LoadOrEmpty] [varchar](50) NULL,
	[FK_VehicleInOutManualTypesOfProduct] [bigint] NULL,
	[FK_VehicleInOutManualReason] [bigint] NULL,
	[IssueDateTime] [datetime] NOT NULL,
	[IssuedDateTimeAutoOrManual] [varchar](50) NULL,
	[IsScannedEntry] [bit] NULL,
	[Note] [varchar](300) NULL,
	[IsDeleted] [bit] NULL,
	[StatusText] [varchar](50) NULL,
	[EntryCount] [int] NULL,
 CONSTRAINT [PK_TemporatyVehicle] PRIMARY KEY CLUSTERED 
(
	[PK_TemporaryVehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Test]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Test](
	[va1] [bigint] NULL,
	[val2] [varchar](50) NULL,
	[Internal_KPL_Loaded_8_To_12_Tons_out] [float] NULL,
	[Internal_KPL_Loaded_12_To_25_Tons_out] [float] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransportCompany]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransportCompany](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_TransportCompany] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [varchar](300) NOT NULL,
	[RegistrationNumber] [varchar](300) NULL,
	[ContactNumber] [varchar](100) NULL,
	[ContactAddress] [varchar](max) NULL,
	[OwnerName] [varchar](300) NULL,
 CONSTRAINT [PK_TransportCompany] PRIMARY KEY CLUSTERED 
(
	[PK_TransportCompany] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TripExpense]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TripExpense](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_TripExpense] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[FK_Depo_To] [uniqueidentifier] NULL,
	[OWN_MHT_DHT] [varchar](50) NOT NULL,
	[DHT_PartyPoint] [varchar](300) NULL,
	[FK_Depo_From] [uniqueidentifier] NOT NULL,
	[PartyPoint] [varchar](300) NULL,
	[FreshChallan] [bigint] NULL,
	[Toll] [bigint] NULL,
	[Bridge] [bigint] NULL,
	[Ferry] [bigint] NULL,
	[CityCoorporation_LaborTrustChanda] [bigint] NULL,
	[Eant_A] [bigint] NULL,
	[OpenTruck] [bigint] NULL,
	[Van_Rickshaw_Boat_TrollerRent] [bigint] NULL,
	[AllowanceDriver] [bigint] NULL,
	[AllowanceHelper] [bigint] NULL,
	[MobileBill] [bigint] NULL,
	[LandPort_Export_LCStation_AnotherPartyPoint_Workshop] [bigint] NULL,
	[LoadUnloadPoint] [bigint] NULL,
	[NightBill] [bigint] NULL,
	[Conveyance] [bigint] NULL,
	[CarriageOutword] [bigint] NULL,
	[FactoryDiesel] [bigint] NULL,
	[FactoryGas] [bigint] NULL,
	[FactoryOcten] [bigint] NULL,
	[FactoryMobil] [bigint] NULL,
	[Others] [bigint] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TripExpense] PRIMARY KEY CLUSTERED 
(
	[PK_TripExpense] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Upazila]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Upazila](
	[PK_Upazila] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_District] [bigint] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[bn_name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Upazila] PRIMARY KEY CLUSTERED 
(
	[PK_Upazila] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserDesignation]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDesignation](
	[PK_UserDesignation] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Sequence] [int] NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[PK_UserDesignation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleBrand]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleBrand](
	[PK_VehicleBrand] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_VehicleBrand] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleBrand] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleInOutManualReason]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleInOutManualReason](
	[PK_VehicleInOutManualReason] [bigint] IDENTITY(1,1) NOT NULL,
	[TitleBangla] [nvarchar](300) NULL,
	[TitleEnglish] [varchar](300) NULL,
	[Sequence] [int] NULL,
	[IsDeleted] [bit] NULL,
	[IsInReasson] [bit] NULL,
	[IsOutReasson] [bit] NULL,
 CONSTRAINT [PK_VehicleInOutManualReason] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleInOutManualReason] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleInOutManualTypesOfProduct]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleInOutManualTypesOfProduct](
	[PK_VehicleInOutManualTypesOfProduct] [bigint] IDENTITY(1,1) NOT NULL,
	[TitleBangla] [nvarchar](300) NULL,
	[Sequence] [int] NULL,
 CONSTRAINT [PK_VehicleInOutManual_TypesOfProduct] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleInOutManualTypesOfProduct] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleModel]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleModel](
	[PK_VehicleModel] [uniqueidentifier] NOT NULL,
	[FK_VehicleBrand] [uniqueidentifier] NOT NULL,
	[Title2] [varchar](100) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_VehicleBrandModel] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleModel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleSharing]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleSharing](
	[PK_VehicleSharing] [bigint] IDENTITY(1,1) NOT NULL,
	[LoadedOrEmpty] [bit] NULL,
	[WantedCount] [float] NULL,
	[PossibleJourneyStartDateTime] [datetime] NOT NULL,
	[AcceptedCount] [float] NULL,
	[FK_AppUser_Client] [uniqueidentifier] NOT NULL,
	[FK_ReferenceDepo] [uniqueidentifier] NULL,
	[FK_AppUser_Approver] [uniqueidentifier] NULL,
	[FK_Depo_From] [uniqueidentifier] NULL,
	[StartingLocation] [varchar](600) NULL,
	[FK_Depo_To] [uniqueidentifier] NULL,
	[FinishingLocation] [varchar](600) NULL,
	[DistanceGoogle] [float] NULL,
	[FK_RequisitionVehicleType] [int] NOT NULL,
	[VehicleType] [varchar](100) NULL,
	[VehicleCapacity] [varchar](100) NULL,
	[TrackingID] [varchar](100) NULL,
	[FK_AppUser_Assigner] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NULL,
	[IsTest] [bit] NULL,
	[SharingNote] [varchar](max) NULL,
	[Status] [int] NULL,
	[KeepBidOpenUntil] [datetime] NULL,
	[IsExternalAdvertised] [bit] NULL,
	[ExternalWantedCount] [float] NULL,
	[CreatedAt] [datetime] NULL,
	[LockedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_VehicleSharing_1] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleSharing] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleSharingAgentMapping]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleSharingAgentMapping](
	[PK_VehicleSharingAgentMapping] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppUser_InternalAgent] [uniqueidentifier] NOT NULL,
	[FK_AppUser_ExternalAgent] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleSharingBidding]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleSharingBidding](
	[PK_VehicleSharingBidding] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_VehicleSharing] [bigint] NOT NULL,
	[FK_RequisitionAgent_Bidder] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[ManagableQuantity] [int] NOT NULL,
	[PricePerQuantity] [bigint] NULL,
	[ApprovedQuantity] [int] NULL,
	[Status_] [int] NULL,
	[StatusText] [varchar](50) NULL,
	[BidderNote] [varchar](1000) NULL,
	[BiddedAt] [datetime] NULL,
	[VerifiedAt] [datetime] NULL,
	[BidderRating] [int] NULL,
	[BidderRatingNote] [varchar](300) NULL,
	[FK_VehicleSharingBidding_LessPriced] [bigint] NULL,
	[ApprovalMessage] [varchar](1000) NULL,
	[ApprovalNote] [varchar](1000) NULL,
 CONSTRAINT [PK_VehicleSharingBidding] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleSharingBidding] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleSharingDemand]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleSharingDemand](
	[PK_VehicleSharingDemand] [bigint] IDENTITY(1,1) NOT NULL,
	[TrackingID] [varchar](100) NULL,
	[FK_VehicleSharing] [bigint] NULL,
	[IsDeleted] [bit] NULL,
	[IsTest] [bit] NULL,
	[LoadedOrEmpty] [bit] NULL,
	[WantedCount] [float] NULL,
	[PossibleJourneyStartDateTime] [datetime] NOT NULL,
	[AcceptedCount] [float] NULL,
	[FK_AppUser_Client] [uniqueidentifier] NOT NULL,
	[FK_ReferenceDepo] [uniqueidentifier] NULL,
	[FK_AppUser_Approver] [uniqueidentifier] NULL,
	[FK_Depo_From] [uniqueidentifier] NULL,
	[StartingLocation] [varchar](600) NULL,
	[FK_Depo_To] [uniqueidentifier] NULL,
	[FinishingLocation] [varchar](600) NULL,
	[DistanceGoogle] [float] NULL,
	[DistanceRouteChart] [float] NULL,
	[FK_RequisitionVehicleType] [int] NOT NULL,
	[VehicleType] [varchar](100) NULL,
	[VehicleTypeLayer1] [varchar](100) NULL,
	[VehicleTypeLayer1_english] [varchar](100) NULL,
	[VehicleTypeLayer1_bangla] [nvarchar](200) NULL,
	[VehicleTypeLayer2] [varchar](100) NULL,
	[VehicleTypeLayer2_english] [varchar](100) NULL,
	[VehicleTypeLayer2_bangla] [nvarchar](200) NULL,
	[VehicleTypeLayer3] [varchar](100) NULL,
	[VehicleTypeLayer3_english] [varchar](100) NULL,
	[VehicleTypeLayer3_bangla] [nvarchar](200) NULL,
	[VehicleCapacity] [varchar](100) NULL,
	[ClientNote] [varchar](max) NULL,
	[ApproverNote] [varchar](max) NULL,
	[IsHeadDemand] [bit] NULL,
	[IsTailDemand] [bit] NULL,
	[Status] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[VerifiedAt] [datetime] NULL,
	[SharedAt] [datetime] NULL,
 CONSTRAINT [PK_VehicleSharingDemand] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleSharingDemand] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleSharingExternalTrip]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleSharingExternalTrip](
	[PK_VehicleSharingExternalTrip] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_VehicleSharingBidding] [bigint] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[FK_AppUser_Assigner] [uniqueidentifier] NULL,
	[MHT_DHT_DriverName] [varchar](300) NULL,
	[MHT_DHT_DriverContactNumber] [varchar](300) NULL,
	[MHT_DHT_DriverLiceneseNumber] [varchar](300) NULL,
	[IsDeleted] [bit] NULL,
	[IsTest] [bit] NULL,
	[TripNote] [varchar](max) NULL,
	[AssingedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[PrintCopy] [int] NULL,
	[StatusText] [varchar](50) NULL,
	[PaymentStatusText] [varchar](50) NULL,
	[EnteredStartingLocationDateTime] [datetime] NULL,
	[LeftStartingLoactionDateTime] [datetime] NULL,
	[EnteredFinishingLocationDateTime] [datetime] NULL,
	[LeftFinishingLocationDateTime] [datetime] NULL,
	[BillApprovedAt] [datetime] NULL,
	[FK_AppUser_BillApprover] [uniqueidentifier] NULL,
	[BillPaidAt] [datetime] NULL,
	[FK_AppUser_BillPaidBy] [uniqueidentifier] NULL,
	[BillingNote] [varchar](500) NULL,
 CONSTRAINT [PK_VehicleSharingExternalTrip] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleSharingExternalTrip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleSharingInternalTrip]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleSharingInternalTrip](
	[PK_VehicleSharingInternalTrip] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_VehicleSharing] [bigint] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[FK_AppUser_Assigner] [uniqueidentifier] NULL,
	[FK_AppUser_Driver] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NULL,
	[IsTest] [bit] NULL,
	[PossibleJourneyStartDateTime] [datetime] NULL,
	[AssingedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[StatusText] [varchar](50) NULL,
	[PrintCopy] [int] NULL,
	[AdjustmentStatusText] [varchar](50) NULL,
	[EnteredStartingLocationDateTime] [datetime] NULL,
	[LoadingStartDateTime] [datetime] NULL,
	[LoadingDoneDateTime] [datetime] NULL,
	[PossibleJourneyFinishDateTime] [datetime] NULL,
	[PossibleJourneyTimeHour] [int] NULL,
	[LeftStartingLoactionDateTime] [datetime] NULL,
	[EnteredFinishingLocationDateTime] [datetime] NULL,
	[UnloadingStartDateTime] [datetime] NULL,
	[UnloadingDoneDateTime] [datetime] NULL,
	[BillCreatedAt] [datetime] NULL,
	[FK_AppUser_BillCreator] [uniqueidentifier] NULL,
	[BillApprovedAt] [datetime] NULL,
	[FK_AppUser_BillApprover] [uniqueidentifier] NULL,
	[BillPaidAt] [datetime] NULL,
	[FK_AppUser_BillPayer] [uniqueidentifier] NULL,
	[IsNotifiedToDriver] [bit] NULL,
	[NotifiedToDriverAt] [datetime] NULL,
	[Distance_Empty] [float] NULL,
	[KPL_Empty] [float] NULL,
	[Distance_Loaded] [float] NULL,
	[KPL_Loaded] [float] NULL,
	[Distance_Loaded_Plastic] [float] NULL,
	[KPL_Loaded_Plastic] [float] NULL,
	[Distance_InterCity] [float] NULL,
	[KPL_InterCity] [float] NULL,
	[Distance_InterCHT] [float] NULL,
	[KPL_InterCHT] [float] NULL,
	[Distance_Hill] [float] NULL,
	[KPL_Hill] [float] NULL,
	[Distance_OnlyMover] [float] NULL,
	[KPL_OnlyMover] [float] NULL,
	[Distance_Loaded_8_To_12_Tons] [float] NULL,
	[KPL_Loaded_8_To_12_Tons] [float] NULL,
	[Distance_Loaded_12_To_25_Tons] [float] NULL,
	[KPL_Loaded_12_To_25_Tons] [float] NULL,
	[DistanceTrip] [decimal](16, 2) NULL,
	[DistanceGoogle] [float] NULL,
	[DistanceRouteChart] [float] NULL,
	[DistanceManual] [float] NULL,
	[FuelConsumedLitre] [float] NULL,
	[FuelPricePerLitre] [float] NULL,
	[FuelExpence] [decimal](16, 2) NULL,
	[FuelExpenceGivenCashOrOil] [bit] NULL,
	[DriversMoney] [decimal](16, 2) NULL,
	[HelpersMoney] [decimal](16, 2) NULL,
	[BridgeTollFerryCharge] [decimal](16, 2) NULL,
	[LoadingCost] [decimal](16, 2) NULL,
	[UnloadingCost] [decimal](16, 2) NULL,
	[LaborCharge] [decimal](16, 2) NULL,
	[EntertainmentCCharge] [decimal](16, 2) NULL,
	[ParkingCharge] [decimal](16, 2) NULL,
	[EntertainmentACharge] [decimal](16, 2) NULL,
	[RepairCharge] [decimal](16, 2) NULL,
	[OverStayCharge] [decimal](16, 2) NULL,
	[OpenBodyCharge] [decimal](16, 2) NULL,
	[DemurrageCharge] [decimal](16, 2) NULL,
	[TotalExpense] [decimal](16, 2) NULL,
	[BillingNote] [varchar](500) NULL,
 CONSTRAINT [PK_VehicleSharingTrip] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleSharingInternalTrip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleSharingInternalTripAdjustment]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleSharingInternalTripAdjustment](
	[PK_VehicleSharingInternalTripAdjustment] [bigint] NOT NULL,
	[IsDeleted] [bit] NULL,
	[BillAdjustmentCreatedAt] [datetime] NULL,
	[FK_AppUser_BillAdjustmentCreator] [uniqueidentifier] NULL,
	[BillAdjustmentPaidAt] [datetime] NULL,
	[FK_AppUser_BillAdjustmentPaidBy] [uniqueidentifier] NULL,
	[Distance_Empty2] [float] NULL,
	[Distance_Loaded2] [float] NULL,
	[Distance_Loaded_Plastic2] [float] NULL,
	[Distance_InterCity2] [float] NULL,
	[Distance_InterCHT2] [float] NULL,
	[Distance_Hill2] [float] NULL,
	[Distance_OnlyMover2] [float] NULL,
	[Distance_Loaded_8_To_12_Tons2] [float] NULL,
	[Distance_Loaded_12_To_25_Tons2] [float] NULL,
	[FuelConsumedLitre2] [float] NULL,
	[FuelPricePerLitre2] [float] NULL,
	[FuelExpence2] [decimal](16, 2) NULL,
	[FuelExpenceGivenCashOrOil2] [bit] NULL,
	[BridgeTollFerryCharge2] [decimal](16, 2) NULL,
	[EntertainmentCCharge2] [decimal](16, 2) NULL,
	[ParkingCharge2] [decimal](16, 2) NULL,
	[EntertainmentACharge2] [decimal](16, 2) NULL,
	[RepairCharge2] [decimal](16, 2) NULL,
	[OverStayCharge2] [decimal](16, 2) NULL,
	[OpenBodyCharge2] [decimal](16, 2) NULL,
	[DemurrageCharge2] [decimal](16, 2) NULL,
	[TotalExpense2] [decimal](16, 2) NULL,
	[AdjustmentNote2] [varchar](500) NULL,
	[Distance_Empty3] [float] NULL,
	[Distance_Loaded3] [float] NULL,
	[Distance_Loaded_Plastic3] [float] NULL,
	[Distance_InterCity3] [float] NULL,
	[Distance_InterCHT3] [float] NULL,
	[Distance_Hill3] [float] NULL,
	[Distance_OnlyMover3] [float] NULL,
	[Distance_Loaded_8_To_12_Tons3] [float] NULL,
	[Distance_Loaded_12_To_25_Tons3] [float] NULL,
	[FuelConsumedLitre3] [float] NULL,
	[FuelPricePerLitre3] [float] NULL,
	[FuelExpence3] [decimal](16, 2) NULL,
	[FuelExpenceGivenCashOrOil3] [bit] NULL,
	[BridgeTollFerryCharge3] [decimal](16, 2) NULL,
	[EntertainmentCCharge3] [decimal](16, 2) NULL,
	[ParkingCharge3] [decimal](16, 2) NULL,
	[EntertainmentACharge3] [decimal](16, 2) NULL,
	[RepairCharge3] [decimal](16, 2) NULL,
	[OverStayCharge3] [decimal](16, 2) NULL,
	[OpenBodyCharge3] [decimal](16, 2) NULL,
	[DemurrageCharge3] [decimal](16, 2) NULL,
	[TotalExpense3] [decimal](16, 2) NULL,
	[AdjustmentNote3] [varchar](500) NULL,
 CONSTRAINT [PK_VehicleSharingInternalTripAdjustment] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleSharingInternalTripAdjustment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleTracking]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleTracking](
	[PK_Vehicle] [uniqueidentifier] NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Altitude] [float] NOT NULL,
	[EngineStatus] [varchar](50) NOT NULL,
	[Course] [float] NOT NULL,
	[Temperature] [float] NOT NULL,
	[Fuel] [float] NOT NULL,
	[Speed] [float] NOT NULL,
	[Distance] [decimal](13, 5) NOT NULL,
	[EventCode] [varchar](5) NULL,
	[RemainingCash] [int] NULL,
	[UpdateTime] [datetime] NOT NULL,
	[ServerTime] [datetime] NOT NULL,
	[FK_Depo_In] [uniqueidentifier] NULL,
	[DepoInDateTime] [datetime] NULL,
	[FK_Depo_Out] [uniqueidentifier] NULL,
	[DepoOutDateTime] [datetime] NULL,
	[Status_PostionValidity] [varchar](1) NULL,
	[Status_SateliteCount] [int] NULL,
	[Status_GSMSignalStrength] [int] NULL,
	[Event_BatteryLow] [varchar](5) NULL,
	[Event_BatteryLowAt] [datetime] NULL,
	[Event_GSMSignalLost_Recovered] [varchar](5) NULL,
	[Event_GSMSignalLostRecoveredAt] [datetime] NULL,
 CONSTRAINT [PK_VehicleTracking] PRIMARY KEY CLUSTERED 
(
	[PK_Vehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleTrackingInformation]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleTrackingInformation](
	[PK_Vehicle] [uniqueidentifier] NOT NULL,
	[GpsDeviceModel] [varchar](300) NULL,
	[GpsIMEINumber] [varchar](50) NULL,
	[GpsMobileNumber] [varchar](100) NULL,
	[Internal_ShowTemperature] [bit] NULL,
	[FK_AppUser_UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_VehicleTrackingInformation] PRIMARY KEY CLUSTERED 
(
	[PK_Vehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleTrip]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleTrip](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_VehicleTrip] [uniqueidentifier] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[FK_Depo_To] [uniqueidentifier] NULL,
	[OWN_MHT_DHT] [varchar](50) NOT NULL,
	[DHT_PartyPoint] [varchar](300) NULL,
	[FK_Depo_From] [uniqueidentifier] NOT NULL,
	[PartyPoint] [varchar](300) NULL,
	[FreshChallan] [bigint] NULL,
	[Toll] [bigint] NULL,
	[Bridge] [bigint] NULL,
	[Ferry] [bigint] NULL,
	[CityCoorporation_LaborTrustChanda] [bigint] NULL,
	[Eant_A] [bigint] NULL,
	[OpenTruck] [bigint] NULL,
	[Van_Rickshaw_Boat_TrollerRent] [bigint] NULL,
	[AllowanceDriver] [bigint] NULL,
	[AllowanceHelper] [bigint] NULL,
	[MobileBill] [bigint] NULL,
	[LandPort_Export_LCStation_AnotherPartyPoint_Workshop] [bigint] NULL,
	[LoadUnloadPoint] [bigint] NULL,
	[NightBill] [bigint] NULL,
	[Conveyance] [bigint] NULL,
	[CarriageOutword] [bigint] NULL,
	[FactoryDiesel] [bigint] NULL,
	[FactoryGas] [bigint] NULL,
	[FactoryOcten] [bigint] NULL,
	[FactoryMobil] [bigint] NULL,
	[Others] [bigint] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
 CONSTRAINT [PK_VehicleTripExpense] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleTrip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_GroupOfCompanies]    Script Date: 2021-05-25 10:58:11 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupOfCompanies] ON [dbo].[GroupOfCompany]
(
	[RowSerial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [const_defaultValueCompanyIsDeletedSetFalse]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ContructualRequisitionCompany] ADD  CONSTRAINT [DF_ContructualRequisitionCompany_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[FinancingCompany] ADD  CONSTRAINT [DF_FinancingCompany_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[TransportCompany] ADD  CONSTRAINT [DF_TransportCompany_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Accident]  WITH CHECK ADD  CONSTRAINT [FK_Accident_AppUser] FOREIGN KEY([FK_CreatedByUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[Accident] CHECK CONSTRAINT [FK_Accident_AppUser]
GO
ALTER TABLE [dbo].[Accident]  WITH CHECK ADD  CONSTRAINT [FK_Accident_AppUser1] FOREIGN KEY([FK_SettledByUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[Accident] CHECK CONSTRAINT [FK_Accident_AppUser1]
GO
ALTER TABLE [dbo].[Accident]  WITH CHECK ADD  CONSTRAINT [FK_Accident_Depo] FOREIGN KEY([FK_DepoFollowUp])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[Accident] CHECK CONSTRAINT [FK_Accident_Depo]
GO
ALTER TABLE [dbo].[Accident]  WITH CHECK ADD  CONSTRAINT [FK_Accident_District] FOREIGN KEY([FK_District])
REFERENCES [dbo].[District] ([PK_District])
GO
ALTER TABLE [dbo].[Accident] CHECK CONSTRAINT [FK_Accident_District]
GO
ALTER TABLE [dbo].[Accident]  WITH CHECK ADD  CONSTRAINT [FK_Accident_Upazila] FOREIGN KEY([FK_Upazila])
REFERENCES [dbo].[Upazila] ([PK_Upazila])
GO
ALTER TABLE [dbo].[Accident] CHECK CONSTRAINT [FK_Accident_Upazila]
GO
ALTER TABLE [dbo].[Accident]  WITH CHECK ADD  CONSTRAINT [FK_Accident_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[Accident] CHECK CONSTRAINT [FK_Accident_Vehicle]
GO
ALTER TABLE [dbo].[AccidentDocument]  WITH CHECK ADD  CONSTRAINT [FK_AccidentDocument_Accident] FOREIGN KEY([FK_Accident])
REFERENCES [dbo].[Accident] ([PK_Accident])
GO
ALTER TABLE [dbo].[AccidentDocument] CHECK CONSTRAINT [FK_AccidentDocument_Accident]
GO
ALTER TABLE [dbo].[AccidentExpense]  WITH CHECK ADD  CONSTRAINT [FK_AccidentExpense_Accident] FOREIGN KEY([FK_Accident])
REFERENCES [dbo].[Accident] ([PK_Accident])
GO
ALTER TABLE [dbo].[AccidentExpense] CHECK CONSTRAINT [FK_AccidentExpense_Accident]
GO
ALTER TABLE [dbo].[AccidentExpense]  WITH CHECK ADD  CONSTRAINT [FK_AccidentExpense_AppUser] FOREIGN KEY([FK_PaidByUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AccidentExpense] CHECK CONSTRAINT [FK_AccidentExpense_AppUser]
GO
ALTER TABLE [dbo].[AlertEmailAttachedDepo]  WITH CHECK ADD  CONSTRAINT [FK_AlertEmail_AlertEmailAttachedDepo] FOREIGN KEY([FK_AlertEmail])
REFERENCES [dbo].[AlertEmail] ([PK_AlertEmail])
GO
ALTER TABLE [dbo].[AlertEmailAttachedDepo] CHECK CONSTRAINT [FK_AlertEmail_AlertEmailAttachedDepo]
GO
ALTER TABLE [dbo].[AlertEmailAttachedDepo]  WITH CHECK ADD  CONSTRAINT [FK_Depo_AlertEmailAttachedDepo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[AlertEmailAttachedDepo] CHECK CONSTRAINT [FK_Depo_AlertEmailAttachedDepo]
GO
ALTER TABLE [dbo].[AppModule_AppAction]  WITH CHECK ADD  CONSTRAINT [FK_AppModule_AppAction_AppAction] FOREIGN KEY([FK_AppAction])
REFERENCES [dbo].[AppAction] ([PK_AppAction])
GO
ALTER TABLE [dbo].[AppModule_AppAction] CHECK CONSTRAINT [FK_AppModule_AppAction_AppAction]
GO
ALTER TABLE [dbo].[AppModule_AppAction]  WITH CHECK ADD  CONSTRAINT [FK_AppModule_AppAction_AppModule1] FOREIGN KEY([FK_AppModule])
REFERENCES [dbo].[AppModule] ([PK_AppModule])
GO
ALTER TABLE [dbo].[AppModule_AppAction] CHECK CONSTRAINT [FK_AppModule_AppAction_AppModule1]
GO
ALTER TABLE [dbo].[AppPermission]  WITH CHECK ADD  CONSTRAINT [FK_AppPermission_AppMenu] FOREIGN KEY([FK_AppMenu])
REFERENCES [dbo].[AppMenu] ([PK_AppMenu])
GO
ALTER TABLE [dbo].[AppPermission] CHECK CONSTRAINT [FK_AppPermission_AppMenu]
GO
ALTER TABLE [dbo].[AppRole_AppMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppRole_AppMenu_AppMenu] FOREIGN KEY([FK_AppMenu])
REFERENCES [dbo].[AppMenu] ([PK_AppMenu])
GO
ALTER TABLE [dbo].[AppRole_AppMenu] CHECK CONSTRAINT [FK_AppRole_AppMenu_AppMenu]
GO
ALTER TABLE [dbo].[AppRole_AppMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppRole_AppMenu_AppRole] FOREIGN KEY([FK_AppRole])
REFERENCES [dbo].[AppRole] ([PK_AppRole])
GO
ALTER TABLE [dbo].[AppRole_AppMenu] CHECK CONSTRAINT [FK_AppRole_AppMenu_AppRole]
GO
ALTER TABLE [dbo].[AppRole_AppPermission]  WITH CHECK ADD  CONSTRAINT [FK_AppRole_AppPermission_AppPermission] FOREIGN KEY([FK_AppPermission])
REFERENCES [dbo].[AppPermission] ([PK_AppPermission])
GO
ALTER TABLE [dbo].[AppRole_AppPermission] CHECK CONSTRAINT [FK_AppRole_AppPermission_AppPermission]
GO
ALTER TABLE [dbo].[AppRole_AppPermission]  WITH CHECK ADD  CONSTRAINT [FK_AppRole_AppPermission_AppRole] FOREIGN KEY([FK_AppRole])
REFERENCES [dbo].[AppRole] ([PK_AppRole])
GO
ALTER TABLE [dbo].[AppRole_AppPermission] CHECK CONSTRAINT [FK_AppRole_AppPermission_AppRole]
GO
ALTER TABLE [dbo].[AppRole_AppSubMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppRole_AppSubMenu_AppRole] FOREIGN KEY([FK_AppRole])
REFERENCES [dbo].[AppRole] ([PK_AppRole])
GO
ALTER TABLE [dbo].[AppRole_AppSubMenu] CHECK CONSTRAINT [FK_AppRole_AppSubMenu_AppRole]
GO
ALTER TABLE [dbo].[AppRole_AppSubMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppRole_AppSubMenu_AppSubMenu] FOREIGN KEY([FK_AppSubMenu])
REFERENCES [dbo].[AppSubMenu] ([PK_AppSubMenu])
GO
ALTER TABLE [dbo].[AppRole_AppSubMenu] CHECK CONSTRAINT [FK_AppRole_AppSubMenu_AppSubMenu]
GO
ALTER TABLE [dbo].[AppSubMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppSubMenu_AppMenu] FOREIGN KEY([FK_AppMenu])
REFERENCES [dbo].[AppMenu] ([PK_AppMenu])
GO
ALTER TABLE [dbo].[AppSubMenu] CHECK CONSTRAINT [FK_AppSubMenu_AppMenu]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppRole] FOREIGN KEY([FK_AppRole])
REFERENCES [dbo].[AppRole] ([PK_AppRole])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_AppRole]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppUser] FOREIGN KEY([PK_User])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_AppUser]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppUser1] FOREIGN KEY([PK_User])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_AppUser1]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_ContructualRequisitionCompany] FOREIGN KEY([FK_ContructualRequisitionCompany])
REFERENCES [dbo].[ContructualRequisitionCompany] ([PK_ContructualRequisitionCompany])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_ContructualRequisitionCompany]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_Depo]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_Location] FOREIGN KEY([FK_Location])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_Location]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_MobileRole] FOREIGN KEY([FK_MobileRole])
REFERENCES [dbo].[MobileRole] ([PK_MobileRole])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_MobileRole]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_TransportCompany] FOREIGN KEY([FK_TransportCompany])
REFERENCES [dbo].[TransportCompany] ([PK_TransportCompany])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_TransportCompany]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_VehicleSharingInternalTrip] FOREIGN KEY([FK_VehicleSharingInternalTrip_Pending])
REFERENCES [dbo].[VehicleSharingInternalTrip] ([PK_VehicleSharingInternalTrip])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_VehicleSharingInternalTrip]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_VehicleSharingInternalTrip1] FOREIGN KEY([FK_VehicleSharingInternalTrip_Current])
REFERENCES [dbo].[VehicleSharingInternalTrip] ([PK_VehicleSharingInternalTrip])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [FK_AppUser_VehicleSharingInternalTrip1]
GO
ALTER TABLE [dbo].[AppUser_AppModule]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppModule_AppModule] FOREIGN KEY([FK_AppModule])
REFERENCES [dbo].[AppModule] ([PK_AppModule])
GO
ALTER TABLE [dbo].[AppUser_AppModule] CHECK CONSTRAINT [FK_AppUser_AppModule_AppModule]
GO
ALTER TABLE [dbo].[AppUser_AppModule]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppModule_AppUser] FOREIGN KEY([FK_AppUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUser_AppModule] CHECK CONSTRAINT [FK_AppUser_AppModule_AppUser]
GO
ALTER TABLE [dbo].[AppUserAccessibleDepo]  WITH CHECK ADD  CONSTRAINT [FK_AppUserAccessibleDepo_AppUser1] FOREIGN KEY([FK_AppUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUserAccessibleDepo] CHECK CONSTRAINT [FK_AppUserAccessibleDepo_AppUser1]
GO
ALTER TABLE [dbo].[AppUserAccessibleDepo]  WITH CHECK ADD  CONSTRAINT [FK_AppUserAccessibleDepo_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[AppUserAccessibleDepo] CHECK CONSTRAINT [FK_AppUserAccessibleDepo_Depo]
GO
ALTER TABLE [dbo].[AppUserLoginHistory]  WITH CHECK ADD  CONSTRAINT [FK_AppUserLoginHistory_AppUser] FOREIGN KEY([FK_AppUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUserLoginHistory] CHECK CONSTRAINT [FK_AppUserLoginHistory_AppUser]
GO
ALTER TABLE [dbo].[AppUserSurpervisedContructualCompany]  WITH CHECK ADD  CONSTRAINT [FK_AppUserSurpervisedContructualCompany_AppUser] FOREIGN KEY([FK_AppUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUserSurpervisedContructualCompany] CHECK CONSTRAINT [FK_AppUserSurpervisedContructualCompany_AppUser]
GO
ALTER TABLE [dbo].[AppUserSurpervisedContructualCompany]  WITH CHECK ADD  CONSTRAINT [FK_AppUserSurpervisedContructualCompany_ContructualRequisitionCompany] FOREIGN KEY([FK_ContructualRequisitionCompany])
REFERENCES [dbo].[ContructualRequisitionCompany] ([PK_ContructualRequisitionCompany])
GO
ALTER TABLE [dbo].[AppUserSurpervisedContructualCompany] CHECK CONSTRAINT [FK_AppUserSurpervisedContructualCompany_ContructualRequisitionCompany]
GO
ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_Company_GroupOfCompany] FOREIGN KEY([FK_GroupOfCompany])
REFERENCES [dbo].[GroupOfCompany] ([PK_GroupOfCompany])
GO
ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_Company_GroupOfCompany]
GO
ALTER TABLE [dbo].[ContructualRequisition]  WITH CHECK ADD  CONSTRAINT [FK_ContractualRequisition_RequisitionAgent] FOREIGN KEY([FK_RequisitionAgent])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[ContructualRequisition] CHECK CONSTRAINT [FK_ContractualRequisition_RequisitionAgent]
GO
ALTER TABLE [dbo].[ContructualRequisition]  WITH CHECK ADD  CONSTRAINT [FK_ContructualRequisition_ContructualRequisitionCompany] FOREIGN KEY([FK_ContructualRequisitionCompany])
REFERENCES [dbo].[ContructualRequisitionCompany] ([PK_ContructualRequisitionCompany])
GO
ALTER TABLE [dbo].[ContructualRequisition] CHECK CONSTRAINT [FK_ContructualRequisition_ContructualRequisitionCompany]
GO
ALTER TABLE [dbo].[ContructualRequisitionDetail]  WITH CHECK ADD  CONSTRAINT [FK_ContructualRequisitionDetail_ContructualRequisition] FOREIGN KEY([FK_ContructualRequisition])
REFERENCES [dbo].[ContructualRequisition] ([PK_ContructualRequisition])
GO
ALTER TABLE [dbo].[ContructualRequisitionDetail] CHECK CONSTRAINT [FK_ContructualRequisitionDetail_ContructualRequisition]
GO
ALTER TABLE [dbo].[ContructualRequisitionDetailEntry]  WITH CHECK ADD  CONSTRAINT [FK_ContructualRequisitionDetailEntry_AppUser] FOREIGN KEY([FK_AppUser_AppliedBy])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[ContructualRequisitionDetailEntry] CHECK CONSTRAINT [FK_ContructualRequisitionDetailEntry_AppUser]
GO
ALTER TABLE [dbo].[ContructualRequisitionDetailEntry]  WITH CHECK ADD  CONSTRAINT [FK_ContructualRequisitionDetailEntry_AppUser1] FOREIGN KEY([FK_AppUser_ApprovedBy])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[ContructualRequisitionDetailEntry] CHECK CONSTRAINT [FK_ContructualRequisitionDetailEntry_AppUser1]
GO
ALTER TABLE [dbo].[ContructualRequisitionDetailEntry]  WITH CHECK ADD  CONSTRAINT [FK_ContructualRequisitionDetailEntry_ContructualRequisitionDetail] FOREIGN KEY([FK_ContructualRequisitionDetail])
REFERENCES [dbo].[ContructualRequisitionDetail] ([PK_ContructualRequisitionDetail])
GO
ALTER TABLE [dbo].[ContructualRequisitionDetailEntry] CHECK CONSTRAINT [FK_ContructualRequisitionDetailEntry_ContructualRequisitionDetail]
GO
ALTER TABLE [dbo].[DairyVehicle]  WITH CHECK ADD  CONSTRAINT [FK_DairyVehicle_Vehicle] FOREIGN KEY([PK_DairyVehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[DairyVehicle] CHECK CONSTRAINT [FK_DairyVehicle_Vehicle]
GO
ALTER TABLE [dbo].[DepoBorder]  WITH CHECK ADD  CONSTRAINT [FK_DepoBorder_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[DepoBorder] CHECK CONSTRAINT [FK_DepoBorder_Depo]
GO
ALTER TABLE [dbo].[DepoGroup]  WITH CHECK ADD  CONSTRAINT [FK_DepoGroup_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[DepoGroup] CHECK CONSTRAINT [FK_DepoGroup_Depo]
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [FK_Driver_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [FK_Driver_Depo]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_EventType] FOREIGN KEY([FK_EventType])
REFERENCES [dbo].[EventType] ([PK_EventType])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_EventType]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Event] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Vehicle_Event]
GO
ALTER TABLE [dbo].[EventDocument]  WITH CHECK ADD  CONSTRAINT [FK_EventDocument_Event] FOREIGN KEY([FK_Event])
REFERENCES [dbo].[Event] ([PK_Event])
GO
ALTER TABLE [dbo].[EventDocument] CHECK CONSTRAINT [FK_EventDocument_Event]
GO
ALTER TABLE [dbo].[Helper]  WITH CHECK ADD  CONSTRAINT [FK_Helper_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[Helper] CHECK CONSTRAINT [FK_Helper_Depo]
GO
ALTER TABLE [dbo].[HiredVehicleDriver]  WITH CHECK ADD  CONSTRAINT [FK_HiredVehicleDriver_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[HiredVehicleDriver] CHECK CONSTRAINT [FK_HiredVehicleDriver_Vehicle]
GO
ALTER TABLE [dbo].[IndividualRequisition]  WITH CHECK ADD  CONSTRAINT [FK_IndividualRequisition_AppUser] FOREIGN KEY([FK_RequisitionAgent])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[IndividualRequisition] CHECK CONSTRAINT [FK_IndividualRequisition_AppUser]
GO
ALTER TABLE [dbo].[IndividualRequisition]  WITH CHECK ADD  CONSTRAINT [FK_IndividualRequisition_Depo] FOREIGN KEY([FK_Depo_From])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[IndividualRequisition] CHECK CONSTRAINT [FK_IndividualRequisition_Depo]
GO
ALTER TABLE [dbo].[IndividualRequisition]  WITH CHECK ADD  CONSTRAINT [FK_IndividualRequisition_Depo1] FOREIGN KEY([FK_Depo_To])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[IndividualRequisition] CHECK CONSTRAINT [FK_IndividualRequisition_Depo1]
GO
ALTER TABLE [dbo].[IndividualRequisitionBidding]  WITH CHECK ADD  CONSTRAINT [FK_IndividualRequisitionBidding_AppUser1] FOREIGN KEY([FK_RequisitionAgent_Bidder])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[IndividualRequisitionBidding] CHECK CONSTRAINT [FK_IndividualRequisitionBidding_AppUser1]
GO
ALTER TABLE [dbo].[IndividualRequisitionBidding]  WITH CHECK ADD  CONSTRAINT [FK_IndividualRequisitionBidding_IndividualRequisition] FOREIGN KEY([FK_IndividualRequisition])
REFERENCES [dbo].[IndividualRequisition] ([PK_IndividualRequisition])
GO
ALTER TABLE [dbo].[IndividualRequisitionBidding] CHECK CONSTRAINT [FK_IndividualRequisitionBidding_IndividualRequisition]
GO
ALTER TABLE [dbo].[InstantRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InstantRequisition_Depo] FOREIGN KEY([FK_Depo_Booking])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[InstantRequisition] CHECK CONSTRAINT [FK_InstantRequisition_Depo]
GO
ALTER TABLE [dbo].[InstantRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InstantRequisition_Depo1] FOREIGN KEY([FK_Depo_Destination])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[InstantRequisition] CHECK CONSTRAINT [FK_InstantRequisition_Depo1]
GO
ALTER TABLE [dbo].[InstantRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InstantRequisition_RequisitionAgent] FOREIGN KEY([FK_RequisitionAgent])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[InstantRequisition] CHECK CONSTRAINT [FK_InstantRequisition_RequisitionAgent]
GO
ALTER TABLE [dbo].[InstantRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InstantRequisition_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[InstantRequisition] CHECK CONSTRAINT [FK_InstantRequisition_Vehicle]
GO
ALTER TABLE [dbo].[InterCompanyRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_AppUser] FOREIGN KEY([FK_AppUser_Client])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[InterCompanyRequisition] CHECK CONSTRAINT [FK_InterCompanyRequisition_AppUser]
GO
ALTER TABLE [dbo].[InterCompanyRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_AppUser1] FOREIGN KEY([FK_AppUser_Approver])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[InterCompanyRequisition] CHECK CONSTRAINT [FK_InterCompanyRequisition_AppUser1]
GO
ALTER TABLE [dbo].[InterCompanyRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_Depo] FOREIGN KEY([FK_ReferenceDepo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[InterCompanyRequisition] CHECK CONSTRAINT [FK_InterCompanyRequisition_Depo]
GO
ALTER TABLE [dbo].[InterCompanyRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_InterCompanyRequisitionLocation] FOREIGN KEY([FK_InterCompanyRequisitionLocation_From])
REFERENCES [dbo].[InterCompanyRequisitionLocation] ([PK_InterCompanyRequisitionLocation])
GO
ALTER TABLE [dbo].[InterCompanyRequisition] CHECK CONSTRAINT [FK_InterCompanyRequisition_InterCompanyRequisitionLocation]
GO
ALTER TABLE [dbo].[InterCompanyRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_InterCompanyRequisitionLocation1] FOREIGN KEY([FK_InterCompanyRequisitionLocation_To])
REFERENCES [dbo].[InterCompanyRequisitionLocation] ([PK_InterCompanyRequisitionLocation])
GO
ALTER TABLE [dbo].[InterCompanyRequisition] CHECK CONSTRAINT [FK_InterCompanyRequisition_InterCompanyRequisitionLocation1]
GO
ALTER TABLE [dbo].[InterCompanyRequisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_RequisitionVehicleType] FOREIGN KEY([FK_RequisitionVehicleType])
REFERENCES [dbo].[RequisitionVehicleType] ([PK_RequisitionVehicleType])
GO
ALTER TABLE [dbo].[InterCompanyRequisition] CHECK CONSTRAINT [FK_InterCompanyRequisition_RequisitionVehicleType]
GO
ALTER TABLE [dbo].[InterCompanyRequisition_ExternalTroller]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_ExternalTroller_InterCompanyRequisition] FOREIGN KEY([FK_InterCompanyRequisition])
REFERENCES [dbo].[InterCompanyRequisition] ([PK_InterCompanyRequisition])
GO
ALTER TABLE [dbo].[InterCompanyRequisition_ExternalTroller] CHECK CONSTRAINT [FK_InterCompanyRequisition_ExternalTroller_InterCompanyRequisition]
GO
ALTER TABLE [dbo].[InterCompanyRequisition_ExternalVehicle]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_ExternalVehicle_InterCompanyRequisition] FOREIGN KEY([FK_InterCompanyRequisition])
REFERENCES [dbo].[InterCompanyRequisition] ([PK_InterCompanyRequisition])
GO
ALTER TABLE [dbo].[InterCompanyRequisition_ExternalVehicle] CHECK CONSTRAINT [FK_InterCompanyRequisition_ExternalVehicle_InterCompanyRequisition]
GO
ALTER TABLE [dbo].[InterCompanyRequisition_ExternalVehicle]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_ExternalVehicle_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[InterCompanyRequisition_ExternalVehicle] CHECK CONSTRAINT [FK_InterCompanyRequisition_ExternalVehicle_Vehicle]
GO
ALTER TABLE [dbo].[InterCompanyRequisition_InternalVehicle]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_Vehicle_InterCompanyRequisition] FOREIGN KEY([FK_InterCompanyRequisition])
REFERENCES [dbo].[InterCompanyRequisition] ([PK_InterCompanyRequisition])
GO
ALTER TABLE [dbo].[InterCompanyRequisition_InternalVehicle] CHECK CONSTRAINT [FK_InterCompanyRequisition_Vehicle_InterCompanyRequisition]
GO
ALTER TABLE [dbo].[InterCompanyRequisition_InternalVehicle]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisition_Vehicle_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[InterCompanyRequisition_InternalVehicle] CHECK CONSTRAINT [FK_InterCompanyRequisition_Vehicle_Vehicle]
GO
ALTER TABLE [dbo].[InterCompanyRequisitionBidding]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisitionBidding_AppUser] FOREIGN KEY([FK_RequisitionAgent_Bidder])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[InterCompanyRequisitionBidding] CHECK CONSTRAINT [FK_InterCompanyRequisitionBidding_AppUser]
GO
ALTER TABLE [dbo].[InterCompanyRequisitionBidding]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisitionBidding_InterCompanyRequisition] FOREIGN KEY([FK_InterCompanyRequisition])
REFERENCES [dbo].[InterCompanyRequisition] ([PK_InterCompanyRequisition])
GO
ALTER TABLE [dbo].[InterCompanyRequisitionBidding] CHECK CONSTRAINT [FK_InterCompanyRequisitionBidding_InterCompanyRequisition]
GO
ALTER TABLE [dbo].[MobileRole_MobileMenu]  WITH CHECK ADD  CONSTRAINT [FK_MobileRole_MobileMenu_MobileMenu] FOREIGN KEY([FK_MobileMenu])
REFERENCES [dbo].[MobileMenu] ([PK_MobileMenu])
GO
ALTER TABLE [dbo].[MobileRole_MobileMenu] CHECK CONSTRAINT [FK_MobileRole_MobileMenu_MobileMenu]
GO
ALTER TABLE [dbo].[MobileRole_MobileMenu]  WITH CHECK ADD  CONSTRAINT [FK_MobileRole_MobileMenu_MobileRole] FOREIGN KEY([FK_MobileRole])
REFERENCES [dbo].[MobileRole] ([PK_MobileRole])
GO
ALTER TABLE [dbo].[MobileRole_MobileMenu] CHECK CONSTRAINT [FK_MobileRole_MobileMenu_MobileRole]
GO
ALTER TABLE [dbo].[PoliceCase]  WITH CHECK ADD  CONSTRAINT [FK_Case_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[PoliceCase] CHECK CONSTRAINT [FK_Case_Vehicle]
GO
ALTER TABLE [dbo].[PoliceCase]  WITH CHECK ADD  CONSTRAINT [FK_PoliceCase_District] FOREIGN KEY([FK_District])
REFERENCES [dbo].[District] ([PK_District])
GO
ALTER TABLE [dbo].[PoliceCase] CHECK CONSTRAINT [FK_PoliceCase_District]
GO
ALTER TABLE [dbo].[PoliceCase]  WITH CHECK ADD  CONSTRAINT [FK_PoliceCase_Upazila] FOREIGN KEY([FK_Upazila])
REFERENCES [dbo].[Upazila] ([PK_Upazila])
GO
ALTER TABLE [dbo].[PoliceCase] CHECK CONSTRAINT [FK_PoliceCase_Upazila]
GO
ALTER TABLE [dbo].[PoliceCase_PoliceCaseLaw]  WITH CHECK ADD  CONSTRAINT [FK_PoliceCase_PoliceCaseLaw_PoliceCase1] FOREIGN KEY([FK_PoliceCase])
REFERENCES [dbo].[PoliceCase] ([PK_PoliceCase])
GO
ALTER TABLE [dbo].[PoliceCase_PoliceCaseLaw] CHECK CONSTRAINT [FK_PoliceCase_PoliceCaseLaw_PoliceCase1]
GO
ALTER TABLE [dbo].[PoliceCase_PoliceCaseLaw]  WITH CHECK ADD  CONSTRAINT [FK_PoliceCase_PoliceCaseLaw_PoliceCaseLaw] FOREIGN KEY([FK_PoliceCaseLaw])
REFERENCES [dbo].[PoliceCaseLaw] ([PK_PoliceCaseLaw])
GO
ALTER TABLE [dbo].[PoliceCase_PoliceCaseLaw] CHECK CONSTRAINT [FK_PoliceCase_PoliceCaseLaw_PoliceCaseLaw]
GO
ALTER TABLE [dbo].[PoliceCaseDocument]  WITH CHECK ADD  CONSTRAINT [FK_PoliceCaseDocument_PoliceCase] FOREIGN KEY([FK_PoliceCase])
REFERENCES [dbo].[PoliceCase] ([PK_PoliceCase])
GO
ALTER TABLE [dbo].[PoliceCaseDocument] CHECK CONSTRAINT [FK_PoliceCaseDocument_PoliceCase]
GO
ALTER TABLE [dbo].[Report_VehicleOutOverStay]  WITH CHECK ADD  CONSTRAINT [FK_Report_VehicleOutOverStay_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[Report_VehicleOutOverStay] CHECK CONSTRAINT [FK_Report_VehicleOutOverStay_Vehicle]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisitionNew_Depo] FOREIGN KEY([FK_Depo_From])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_InterCompanyRequisitionNew_Depo]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisitionNew_Depo1] FOREIGN KEY([FK_Depo_To])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_InterCompanyRequisitionNew_Depo1]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisitionNew_InterCompanyRequisitionNewBusinessUnit] FOREIGN KEY([FK_RequisitionBusinessUnit])
REFERENCES [dbo].[RequisitionBusinessUnit] ([PK_RequisitionBusinessUnit])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_InterCompanyRequisitionNew_InterCompanyRequisitionNewBusinessUnit]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisitionNew_InterCompanyRequisitionNewProductType] FOREIGN KEY([FK_RequisitionProductType])
REFERENCES [dbo].[RequisitionProductType] ([PK_RequisitionProductType])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_InterCompanyRequisitionNew_InterCompanyRequisitionNewProductType]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_InterCompanyRequisitionNew_RequisitionVehicleType] FOREIGN KEY([FK_RequisitionVehicleType])
REFERENCES [dbo].[RequisitionVehicleType] ([PK_RequisitionVehicleType])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_InterCompanyRequisitionNew_RequisitionVehicleType]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_Requisition_AppUser] FOREIGN KEY([FK_AppUser_Client])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_Requisition_AppUser]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_Requisition_AppUser1] FOREIGN KEY([FK_AppUser_Verifier])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_Requisition_AppUser1]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_Requisition_Location_From] FOREIGN KEY([FK_Location_From])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_Requisition_Location_From]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_Requisition_Location_To] FOREIGN KEY([FK_Location_To])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_Requisition_Location_To]
GO
ALTER TABLE [dbo].[RequisitionAgentNotification]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionAgentNotification_RequisitionAgent] FOREIGN KEY([FK_RequisitionAgent])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[RequisitionAgentNotification] CHECK CONSTRAINT [FK_RequisitionAgentNotification_RequisitionAgent]
GO
ALTER TABLE [dbo].[RequisitionAgentProposedDepo]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionAgentProposedDepo_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[RequisitionAgentProposedDepo] CHECK CONSTRAINT [FK_RequisitionAgentProposedDepo_Depo]
GO
ALTER TABLE [dbo].[RequisitionAgentProposedDepo]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionAgentProposedDepo_RequisitionAgent] FOREIGN KEY([FK_RequisitionAgent])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[RequisitionAgentProposedDepo] CHECK CONSTRAINT [FK_RequisitionAgentProposedDepo_RequisitionAgent]
GO
ALTER TABLE [dbo].[RequisitionTrip]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionTrip_AppUser_Assigner] FOREIGN KEY([FK_AppUser_Assigner])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[RequisitionTrip] CHECK CONSTRAINT [FK_RequisitionTrip_AppUser_Assigner]
GO
ALTER TABLE [dbo].[RequisitionTrip]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionTrip_Requisition] FOREIGN KEY([FK_Requisition])
REFERENCES [dbo].[Requisition] ([PK_Requisition])
GO
ALTER TABLE [dbo].[RequisitionTrip] CHECK CONSTRAINT [FK_RequisitionTrip_Requisition]
GO
ALTER TABLE [dbo].[RequisitionTrip]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionTrip_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[RequisitionTrip] CHECK CONSTRAINT [FK_RequisitionTrip_Vehicle]
GO
ALTER TABLE [dbo].[RequisitionTrip]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionTripParent_RequisitionTrip] FOREIGN KEY([FK_RequisitionTrip_Parent])
REFERENCES [dbo].[RequisitionTrip] ([PK_RequisitionTrip])
GO
ALTER TABLE [dbo].[RequisitionTrip] CHECK CONSTRAINT [FK_RequisitionTripParent_RequisitionTrip]
GO
ALTER TABLE [dbo].[RequisitionTrip_Finished]  WITH CHECK ADD  CONSTRAINT [FK_Requisition_RequisitionTrip_Finished] FOREIGN KEY([FK_Requisition])
REFERENCES [dbo].[Requisition] ([PK_Requisition])
GO
ALTER TABLE [dbo].[RequisitionTrip_Finished] CHECK CONSTRAINT [FK_Requisition_RequisitionTrip_Finished]
GO
ALTER TABLE [dbo].[RequisitionTrip_Finished]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionTrip_Finished_AppUser_Assigner] FOREIGN KEY([FK_AppUser_Assigner])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[RequisitionTrip_Finished] CHECK CONSTRAINT [FK_RequisitionTrip_Finished_AppUser_Assigner]
GO
ALTER TABLE [dbo].[RequisitionTrip_Finished]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionTrip_Finished_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[RequisitionTrip_Finished] CHECK CONSTRAINT [FK_RequisitionTrip_Finished_Vehicle]
GO
ALTER TABLE [dbo].[RequisitionTrip_Finished]  WITH CHECK ADD  CONSTRAINT [FK_RequisitionTrip_FinishedParent_RequisitionTrip_Finished] FOREIGN KEY([FK_RequisitionTrip_Finished_Parent])
REFERENCES [dbo].[RequisitionTrip_Finished] ([PK_RequisitionTrip_Finished])
GO
ALTER TABLE [dbo].[RequisitionTrip_Finished] CHECK CONSTRAINT [FK_RequisitionTrip_FinishedParent_RequisitionTrip_Finished]
GO
ALTER TABLE [dbo].[TemporaryVehicle]  WITH CHECK ADD  CONSTRAINT [FK_TemporaryVehicle_AppUser] FOREIGN KEY([FK_CreatedByLocationGate])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[TemporaryVehicle] CHECK CONSTRAINT [FK_TemporaryVehicle_AppUser]
GO
ALTER TABLE [dbo].[TemporaryVehicle]  WITH CHECK ADD  CONSTRAINT [FK_TemporaryVehicle_Location] FOREIGN KEY([FK_Locaiton])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[TemporaryVehicle] CHECK CONSTRAINT [FK_TemporaryVehicle_Location]
GO
ALTER TABLE [dbo].[TemporaryVehicle]  WITH CHECK ADD  CONSTRAINT [FK_TemporaryVehicle_PRG_Type] FOREIGN KEY([FK_PRG_Type])
REFERENCES [dbo].[PRG_Type] ([PK_PRG_Type])
GO
ALTER TABLE [dbo].[TemporaryVehicle] CHECK CONSTRAINT [FK_TemporaryVehicle_PRG_Type]
GO
ALTER TABLE [dbo].[Upazila]  WITH CHECK ADD  CONSTRAINT [FK_Upazila_District] FOREIGN KEY([FK_District])
REFERENCES [dbo].[District] ([PK_District])
GO
ALTER TABLE [dbo].[Upazila] CHECK CONSTRAINT [FK_Upazila_District]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Company] FOREIGN KEY([FK_Company])
REFERENCES [dbo].[Company] ([PK_Company])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_Company]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_Depo]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_DepoGroup] FOREIGN KEY([FK_DepoGroup])
REFERENCES [dbo].[DepoGroup] ([PK_DepoGroup])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_DepoGroup]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_DepoInOut] FOREIGN KEY([FK_DepoInOut])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_DepoInOut]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_FinancingCompany] FOREIGN KEY([Internal_FK_FinancingCompany])
REFERENCES [dbo].[FinancingCompany] ([PK_FinancingCompany])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_FinancingCompany]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Location] FOREIGN KEY([FK_LocationInOut])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_Location]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_PurchasingCompany] FOREIGN KEY([Internal_FK_PurchasingCompany])
REFERENCES [dbo].[Company] ([PK_Company])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_PurchasingCompany]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_RequisitionTrip_Current] FOREIGN KEY([FK_RequisitionTrip_Current])
REFERENCES [dbo].[RequisitionTrip] ([PK_RequisitionTrip])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_RequisitionTrip_Current]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_RequisitionTrip_Pending] FOREIGN KEY([FK_RequisitionTrip_Pending])
REFERENCES [dbo].[RequisitionTrip] ([PK_RequisitionTrip])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_RequisitionTrip_Pending]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_VehicleInOutManual] FOREIGN KEY([FK_VehicleInOutManual_Last])
REFERENCES [dbo].[VehicleInOutManual] ([PK_VehicleInOutManual])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_VehicleInOutManual]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_VehicleModel] FOREIGN KEY([FK_VehicleModel])
REFERENCES [dbo].[VehicleModel] ([PK_VehicleModel])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_VehicleModel]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_VehicleSharingInternalTrip] FOREIGN KEY([FK_VehicleSharingInternalTrip_Pending])
REFERENCES [dbo].[VehicleSharingInternalTrip] ([PK_VehicleSharingInternalTrip])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_VehicleSharingInternalTrip]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_VehicleSharingInternalTrip1] FOREIGN KEY([FK_VehicleSharingInternalTrip_Current])
REFERENCES [dbo].[VehicleSharingInternalTrip] ([PK_VehicleSharingInternalTrip])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_VehicleSharingInternalTrip1]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_AppUser] FOREIGN KEY([In_FK_CreatedByUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_AppUser]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_AppUser1] FOREIGN KEY([Out_FK_CreatedByUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_AppUser1]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_Depo] FOREIGN KEY([FK_Depo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_Depo]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_Depo1] FOREIGN KEY([FK_Depo_FromTo])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_Depo1]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_Location] FOREIGN KEY([FK_Location])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_Location]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_PRG_Type] FOREIGN KEY([FK_PRG_Type])
REFERENCES [dbo].[PRG_Type] ([PK_PRG_Type])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_PRG_Type]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_Vehicle]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_VehicleInOutManualReason] FOREIGN KEY([In_FK_VehicleInOutManualReason])
REFERENCES [dbo].[VehicleInOutManualReason] ([PK_VehicleInOutManualReason])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_VehicleInOutManualReason]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_VehicleInOutManualReason1] FOREIGN KEY([Out_FK_VehicleInOutManualReason])
REFERENCES [dbo].[VehicleInOutManualReason] ([PK_VehicleInOutManualReason])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_VehicleInOutManualReason1]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_VehicleInOutManualTypesOfProduct] FOREIGN KEY([In_FK_VehicleInOutManualTypesOfProduct])
REFERENCES [dbo].[VehicleInOutManualTypesOfProduct] ([PK_VehicleInOutManualTypesOfProduct])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_VehicleInOutManualTypesOfProduct]
GO
ALTER TABLE [dbo].[VehicleInOutManual]  WITH CHECK ADD  CONSTRAINT [FK_VehicleInOutManual_VehicleInOutManualTypesOfProduct1] FOREIGN KEY([Out_FK_VehicleInOutManualTypesOfProduct])
REFERENCES [dbo].[VehicleInOutManualTypesOfProduct] ([PK_VehicleInOutManualTypesOfProduct])
GO
ALTER TABLE [dbo].[VehicleInOutManual] CHECK CONSTRAINT [FK_VehicleInOutManual_VehicleInOutManualTypesOfProduct1]
GO
ALTER TABLE [dbo].[VehicleModel]  WITH CHECK ADD  CONSTRAINT [FK_VehicleModel_VehicleBrand] FOREIGN KEY([FK_VehicleBrand])
REFERENCES [dbo].[VehicleBrand] ([PK_VehicleBrand])
GO
ALTER TABLE [dbo].[VehicleModel] CHECK CONSTRAINT [FK_VehicleModel_VehicleBrand]
GO
ALTER TABLE [dbo].[VehicleSharing]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharing_AppUser] FOREIGN KEY([FK_AppUser_Client])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharing] CHECK CONSTRAINT [FK_VehicleSharing_AppUser]
GO
ALTER TABLE [dbo].[VehicleSharing]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharing_AppUser1] FOREIGN KEY([FK_AppUser_Approver])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharing] CHECK CONSTRAINT [FK_VehicleSharing_AppUser1]
GO
ALTER TABLE [dbo].[VehicleSharing]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharing_AppUser2] FOREIGN KEY([FK_AppUser_Assigner])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharing] CHECK CONSTRAINT [FK_VehicleSharing_AppUser2]
GO
ALTER TABLE [dbo].[VehicleSharing]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharing_Depo] FOREIGN KEY([FK_Depo_From])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[VehicleSharing] CHECK CONSTRAINT [FK_VehicleSharing_Depo]
GO
ALTER TABLE [dbo].[VehicleSharing]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharing_Depo1] FOREIGN KEY([FK_Depo_To])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[VehicleSharing] CHECK CONSTRAINT [FK_VehicleSharing_Depo1]
GO
ALTER TABLE [dbo].[VehicleSharing]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharing_RequisitionVehicleType] FOREIGN KEY([FK_RequisitionVehicleType])
REFERENCES [dbo].[RequisitionVehicleType] ([PK_RequisitionVehicleType])
GO
ALTER TABLE [dbo].[VehicleSharing] CHECK CONSTRAINT [FK_VehicleSharing_RequisitionVehicleType]
GO
ALTER TABLE [dbo].[VehicleSharingAgentMapping]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingAgentMapping_ExternalAgent] FOREIGN KEY([FK_AppUser_ExternalAgent])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingAgentMapping] CHECK CONSTRAINT [FK_VehicleSharingAgentMapping_ExternalAgent]
GO
ALTER TABLE [dbo].[VehicleSharingAgentMapping]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingAgentMapping_InternalAgent] FOREIGN KEY([FK_AppUser_InternalAgent])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingAgentMapping] CHECK CONSTRAINT [FK_VehicleSharingAgentMapping_InternalAgent]
GO
ALTER TABLE [dbo].[VehicleSharingBidding]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingBidding_Bidder] FOREIGN KEY([FK_RequisitionAgent_Bidder])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingBidding] CHECK CONSTRAINT [FK_VehicleSharingBidding_Bidder]
GO
ALTER TABLE [dbo].[VehicleSharingBidding]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingBidding_VehicleSharing] FOREIGN KEY([FK_VehicleSharing])
REFERENCES [dbo].[VehicleSharing] ([PK_VehicleSharing])
GO
ALTER TABLE [dbo].[VehicleSharingBidding] CHECK CONSTRAINT [FK_VehicleSharingBidding_VehicleSharing]
GO
ALTER TABLE [dbo].[VehicleSharingBidding]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingBidding_VehicleSharingBidding] FOREIGN KEY([FK_VehicleSharingBidding_LessPriced])
REFERENCES [dbo].[VehicleSharingBidding] ([PK_VehicleSharingBidding])
GO
ALTER TABLE [dbo].[VehicleSharingBidding] CHECK CONSTRAINT [FK_VehicleSharingBidding_VehicleSharingBidding]
GO
ALTER TABLE [dbo].[VehicleSharingDemand]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingDemand_AppUser] FOREIGN KEY([FK_AppUser_Client])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingDemand] CHECK CONSTRAINT [FK_VehicleSharingDemand_AppUser]
GO
ALTER TABLE [dbo].[VehicleSharingDemand]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingDemand_Depo] FOREIGN KEY([FK_Depo_From])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[VehicleSharingDemand] CHECK CONSTRAINT [FK_VehicleSharingDemand_Depo]
GO
ALTER TABLE [dbo].[VehicleSharingDemand]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingDemand_Depo1] FOREIGN KEY([FK_Depo_To])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[VehicleSharingDemand] CHECK CONSTRAINT [FK_VehicleSharingDemand_Depo1]
GO
ALTER TABLE [dbo].[VehicleSharingDemand]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingDemand_RequisitionVehicleType] FOREIGN KEY([FK_RequisitionVehicleType])
REFERENCES [dbo].[RequisitionVehicleType] ([PK_RequisitionVehicleType])
GO
ALTER TABLE [dbo].[VehicleSharingDemand] CHECK CONSTRAINT [FK_VehicleSharingDemand_RequisitionVehicleType]
GO
ALTER TABLE [dbo].[VehicleSharingDemand]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingDemand_VehicleSharing] FOREIGN KEY([FK_VehicleSharing])
REFERENCES [dbo].[VehicleSharing] ([PK_VehicleSharing])
GO
ALTER TABLE [dbo].[VehicleSharingDemand] CHECK CONSTRAINT [FK_VehicleSharingDemand_VehicleSharing]
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingExternalTrip_Assigner] FOREIGN KEY([FK_AppUser_Assigner])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip] CHECK CONSTRAINT [FK_VehicleSharingExternalTrip_Assigner]
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingExternalTrip_BillApprover] FOREIGN KEY([FK_AppUser_BillApprover])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip] CHECK CONSTRAINT [FK_VehicleSharingExternalTrip_BillApprover]
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingExternalTrip_Payer] FOREIGN KEY([FK_AppUser_BillPaidBy])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip] CHECK CONSTRAINT [FK_VehicleSharingExternalTrip_Payer]
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingExternalTrip_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip] CHECK CONSTRAINT [FK_VehicleSharingExternalTrip_Vehicle]
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingExternalTrip_VehicleSharingBidding] FOREIGN KEY([FK_VehicleSharingBidding])
REFERENCES [dbo].[VehicleSharingBidding] ([PK_VehicleSharingBidding])
GO
ALTER TABLE [dbo].[VehicleSharingExternalTrip] CHECK CONSTRAINT [FK_VehicleSharingExternalTrip_VehicleSharingBidding]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTrip_Assigner] FOREIGN KEY([FK_AppUser_Assigner])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip] CHECK CONSTRAINT [FK_VehicleSharingInternalTrip_Assigner]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTrip_BillApprover] FOREIGN KEY([FK_AppUser_BillApprover])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip] CHECK CONSTRAINT [FK_VehicleSharingInternalTrip_BillApprover]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTrip_BillCreator] FOREIGN KEY([FK_AppUser_BillCreator])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip] CHECK CONSTRAINT [FK_VehicleSharingInternalTrip_BillCreator]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTrip_BillPayer] FOREIGN KEY([FK_AppUser_BillPayer])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip] CHECK CONSTRAINT [FK_VehicleSharingInternalTrip_BillPayer]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTrip_Driver] FOREIGN KEY([FK_AppUser_Driver])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip] CHECK CONSTRAINT [FK_VehicleSharingInternalTrip_Driver]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTrip_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip] CHECK CONSTRAINT [FK_VehicleSharingInternalTrip_Vehicle]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingTrip_VehicleSharing] FOREIGN KEY([FK_VehicleSharing])
REFERENCES [dbo].[VehicleSharing] ([PK_VehicleSharing])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTrip] CHECK CONSTRAINT [FK_VehicleSharingTrip_VehicleSharing]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTripAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTripAdjustment_AppUser] FOREIGN KEY([FK_AppUser_BillAdjustmentCreator])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTripAdjustment] CHECK CONSTRAINT [FK_VehicleSharingInternalTripAdjustment_AppUser]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTripAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTripAdjustment_AppUser1] FOREIGN KEY([FK_AppUser_BillAdjustmentPaidBy])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTripAdjustment] CHECK CONSTRAINT [FK_VehicleSharingInternalTripAdjustment_AppUser1]
GO
ALTER TABLE [dbo].[VehicleSharingInternalTripAdjustment]  WITH CHECK ADD  CONSTRAINT [FK_VehicleSharingInternalTripAdjustment_VehicleSharingInternalTrip] FOREIGN KEY([PK_VehicleSharingInternalTripAdjustment])
REFERENCES [dbo].[VehicleSharingInternalTrip] ([PK_VehicleSharingInternalTrip])
GO
ALTER TABLE [dbo].[VehicleSharingInternalTripAdjustment] CHECK CONSTRAINT [FK_VehicleSharingInternalTripAdjustment_VehicleSharingInternalTrip]
GO
ALTER TABLE [dbo].[VehicleTracking]  WITH CHECK ADD  CONSTRAINT [FK_VehicleTracking_VehicleTrackingInformation] FOREIGN KEY([PK_Vehicle])
REFERENCES [dbo].[VehicleTrackingInformation] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[VehicleTracking] CHECK CONSTRAINT [FK_VehicleTracking_VehicleTrackingInformation]
GO
ALTER TABLE [dbo].[VehicleTrackingInformation]  WITH CHECK ADD  CONSTRAINT [FK_VehicleTrackingInformation_Vehicle] FOREIGN KEY([PK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[VehicleTrackingInformation] CHECK CONSTRAINT [FK_VehicleTrackingInformation_Vehicle]
GO
ALTER TABLE [dbo].[VehicleTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleTrip_Depo_From] FOREIGN KEY([FK_Depo_From])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[VehicleTrip] CHECK CONSTRAINT [FK_VehicleTrip_Depo_From]
GO
ALTER TABLE [dbo].[VehicleTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleTrip_Depo_To] FOREIGN KEY([FK_Depo_To])
REFERENCES [dbo].[Depo] ([PK_Depo])
GO
ALTER TABLE [dbo].[VehicleTrip] CHECK CONSTRAINT [FK_VehicleTrip_Depo_To]
GO
ALTER TABLE [dbo].[VehicleTrip]  WITH CHECK ADD  CONSTRAINT [FK_VehicleTrip_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[VehicleTrip] CHECK CONSTRAINT [FK_VehicleTrip_Vehicle]
GO
/****** Object:  StoredProcedure [dbo].[CleanDeviceData]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[DataBaseBackup]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteCoreData]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteDbData]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteDbData]    
AS 
  
--layer 3
delete from EventDocument;
DBCC CHECKIDENT ('[EventDocument]', RESEED, 0);
delete from Event;
DBCC CHECKIDENT ('[Event]', RESEED, 0);
delete from PoliceCaseDocument;
DBCC CHECKIDENT ('[PoliceCaseDocument]', RESEED, 0);
delete from PoliceCase;
DBCC CHECKIDENT ('[PoliceCase]', RESEED, 0);
delete from VehicleTracking;
GO
/****** Object:  StoredProcedure [dbo].[GenerateReport_GetVehicleHaltTime]    Script Date: 2021-05-25 10:58:11 AM ******/
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
			IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01'))
			BEGIN
				INSERT INTO [3rdEyE_TrackingDataBase_2021_05].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																				VALUES( @FK_Vehicle,'data_initial_gap', '', '', @StartingDate, @first_UpdateTime, '', '', '', @HaltTime); 	
			END
			ELSE IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01'))
			BEGIN
				INSERT INTO [3rdEyE_TrackingDataBase_2021_06].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
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
				IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01'))
				BEGIN
					INSERT INTO [3rdEyE_TrackingDataBase_2021_05].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																						VALUES(@FK_Vehicle,'data', convert(varchar(50),@standingState_id), convert(varchar(50),@runningState_id), @standingState_DateTime, @runningState_DateTime, @_Latitude, @_Longitude,@_EngineStatus,  @HaltTime); 
				END
				ELSE IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01'))
				BEGIN
					INSERT INTO [3rdEyE_TrackingDataBase_2021_06].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
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
				IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01'))
				BEGIN
					INSERT INTO [3rdEyE_TrackingDataBase_2021_05].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
																				VALUES(@FK_Vehicle,'data_finishing_gap', convert(varchar(50),@standingState_id), '', @standingState_DateTime, @runningState_DateTime, @_Latitude, @_Longitude,@_EngineStatus, @HaltTime); 
				END
				ELSE IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01'))
				BEGIN
					INSERT INTO [3rdEyE_TrackingDataBase_2021_06].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,PK_RowData_Start,PK_RowData_End,StartTime,EndTime,Latitude,Longitude,EngineStatus,HaltTime) 
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
	
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01'))
	BEGIN
		INSERT INTO [3rdEyE_TrackingDataBase_2021_05].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,StartTime,EndTime,TotalDistance,MaximumSpeed,AverageSpeed,LastUpdate,Latitude,Longitude,HaltTime,HaltCount) 
																	VALUES(@FK_Vehicle,'data_consolidated', @startingDate, @startingDate, @TotalDistance, @MaximumSpeed, @AverageSpeed, @LastUpdate,@LastLatitude,@LastLongitude,@TotalHaltTime,@HaltCount); 
	END
	ELSE IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01'))
	BEGIN
		INSERT INTO [3rdEyE_TrackingDataBase_2021_06].dbo.Report_VehicleHaltReport(FK_Vehicle,_rowType,StartTime,EndTime,TotalDistance,MaximumSpeed,AverageSpeed,LastUpdate,Latitude,Longitude,HaltTime,HaltCount) 
																	VALUES(@FK_Vehicle,'data_consolidated', @startingDate, @startingDate, @TotalDistance, @MaximumSpeed, @AverageSpeed, @LastUpdate,@LastLatitude,@LastLongitude,@TotalHaltTime,@HaltCount); 
	END
END

GO
/****** Object:  StoredProcedure [dbo].[GenerateReport_GetVehicleHaltTime_Helper]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[GenerateReport_GetVehicleHaltTime_Helper_Helper]    Script Date: 2021-05-25 10:58:11 AM ******/
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
	
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01'))
	BEGIN
		Insert into Report_VehicleHaltReadyReport_Helper_Helper(FK_Vehicle ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @FK_Vehicle ,DeviceData.Latitude ,DeviceData.Longitude ,DeviceData.Altitude ,DeviceData.EngineStatus ,DeviceData.Course ,DeviceData.Temperature ,DeviceData.Fuel ,DeviceData.Speed ,DeviceData.Distance ,DeviceData.UpdateTime ,DeviceData.ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
		WHERE DeviceData.FK_Vehicle = @FK_Vehicle
		Order by DeviceData.UpdateTime;
	END
	ElSE IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01'))
	BEGIN
		Insert into Report_VehicleHaltReadyReport_Helper_Helper(FK_Vehicle ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @FK_Vehicle ,DeviceData.Latitude ,DeviceData.Longitude ,DeviceData.Altitude ,DeviceData.EngineStatus ,DeviceData.Course ,DeviceData.Temperature ,DeviceData.Fuel ,DeviceData.Speed ,DeviceData.Distance ,DeviceData.UpdateTime ,DeviceData.ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
		WHERE DeviceData.FK_Vehicle = @FK_Vehicle
		Order by DeviceData.UpdateTime;
	END
	

END

GO
/****** Object:  StoredProcedure [dbo].[MoveDeviceData]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[PushDeviceData__Insert]    Script Date: 2021-05-25 10:58:11 AM ******/
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
IF(@UpdateTime >= '2021-06-01' AND @UpdateTime < '2021-07-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData(FK_Vehicle,GpsIMEINumber,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@GpsIMEINumber,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	
END

--Current
ELSE IF(@UpdateTime >= '2021-05-01' AND @UpdateTime < '2021-06-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	
END

--Previous
ELSE IF(@UpdateTime >= '2021-04-01' AND @UpdateTime < '2021-05-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
END
ELSE
BEGIN
  Select '_Insert-OK-UpdateTime out of range' AS 'RESPONSE'; return;
END

Select '_Insert-OK' AS 'RESPONSE';
END

GO
/****** Object:  StoredProcedure [dbo].[PushDeviceData_Insert_Insert]    Script Date: 2021-05-25 10:58:11 AM ******/
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

INSERT INTO VehicleTracking(PK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,RemainingCash,UpdateTime,ServerTime) 
VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@RemainingCash,@UpdateTime,@CurrentDateTime);

--Next
IF(@UpdateTime >= '2021-06-01' AND @UpdateTime < '2021-07-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData(FK_Vehicle,GpsIMEINumber,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@GpsIMEINumber,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

	INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);
END

--Current
ELSE IF(@UpdateTime >= '2021-05-01' AND @UpdateTime < '2021-06-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

	INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);
END

--Previous
ELSE IF(@UpdateTime >= '2021-04-01' AND @UpdateTime < '2021-05-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
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
/****** Object:  StoredProcedure [dbo].[PushDeviceData_Update_Insert]    Script Date: 2021-05-25 10:58:11 AM ******/
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
@RemainingCash int
)
AS
BEGIN
DECLARE @CurrentDateTime datetime; set @CurrentDateTime = GETDATE();
if(@Status_PostionValidity = 'A' OR @Status_PostionValidity = '1')
BEGIN
	UPDATE VehicleTracking SET Latitude = @Latitude, Longitude = @Longitude, Altitude = @Altitude,
	EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
	Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
	RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime
	WHERE PK_Vehicle = @PK_Vehicle;
END
ELSE
BEGIN
	UPDATE VehicleTracking SET Altitude = @Altitude,
	EngineStatus = @EngineStatus, Course = @Course, Temperature = @Temperature,
	Fuel = @Fuel, Speed = @Speed, Distance = @Distance, Status_PostionValidity = @Status_PostionValidity, Status_SateliteCount = @Status_SateliteCount, Status_GSMSignalStrength = @Status_GSMSignalStrength,
	RemainingCash = @RemainingCash, UpdateTime = @UpdateTime, ServerTime = @CurrentDateTime
	WHERE PK_Vehicle = @PK_Vehicle;
END


--Next
IF(@UpdateTime >= '2021-06-01' AND @UpdateTime < '2021-07-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData(FK_Vehicle,GpsIMEINumber,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@GpsIMEINumber,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	

	INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
END

--Current
ELSE IF(@UpdateTime >= '2021-05-01' AND @UpdateTime < '2021-06-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,EventCode,RemainingCash,Status_PostionValidity,Status_SateliteCount,Status_GSMSignalStrength,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@EventCode,@RemainingCash,@Status_PostionValidity,@Status_SateliteCount,@Status_GSMSignalStrength,@UpdateTime,@CurrentDateTime);	
	
	INSERT INTO [3rdEyE].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
END

--Previous
ELSE IF(@UpdateTime >= '2021-04-01' AND @UpdateTime < '2021-05-01')
BEGIN
	INSERT INTO [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData(FK_Vehicle,Latitude,Longitude,Altitude,EngineStatus,Course,Temperature,Fuel,Speed,Distance,UpdateTime,ServerTime) 
	VALUES (@PK_Vehicle,@Latitude,@Longitude,@Altitude,@EngineStatus,@Course,@Temperature,@Fuel,@Speed,@Distance,@UpdateTime,@CurrentDateTime);	
END
ELSE
BEGIN
  Select 'UpdateAndInsert-OK-UpdateTime of range' AS 'RESPONSE'; return;
END

Select 'UpdateAndInsert-OK' AS 'RESPONSE';
END

-- EXEC dbo._PushDeviceData_Update_Insert '7ef0b0ec-2755-4fce-9344-e2b3dfb9bf60','29-Mar-20 12:53:50 PM','23.78143','90.425295','0','1','0','0','0','0','0','35','1','4','27','124'


GO
/****** Object:  StoredProcedure [dbo].[PushDeviceData_UpdateTime]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[Report_GetAccessibleEventList_InDateRange]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[Report_GetTemperatureHistory]    Script Date: 2021-05-25 10:58:11 AM ******/
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

	--#[3rdEyE_TrackingDataBase_2021_02]
	IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2021_03]
	IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_04]
	IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_05]
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_06]
	IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
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
/****** Object:  StoredProcedure [dbo].[Report_GetTemperatureHistory_RE]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetTemperatureHistory_RE](
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
	
	--#[3rdEyE_TrackingDataBase_2021_02]
	IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2021_03]
	IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_04]
	IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_05]
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_06]
	IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	BEGIN
		insert into Report_TemperatureReport_Helper (USER_KEY, FK_Vehicle, UpdateTime, Temperature)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Temperature from [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
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
		--start
IF(@Temperature < 10)
BEGIN
	set @Temperature = 1;
END
ELSE IF(@Temperature < 13)
BEGIN
	set @Temperature = 2;
END
ELSE IF(@Temperature < 17)
BEGIN
	set @Temperature = 3;
END
ELSE IF(@Temperature < 20)
BEGIN
	set @Temperature = 4;
END
ELSE IF(@Temperature < 23)
BEGIN
	set @Temperature = 1;
END
ELSE IF(@Temperature < 27)
BEGIN
	set @Temperature = 2;
END
ELSE IF(@Temperature < 30)
BEGIN
	set @Temperature = 3;
END
ELSE IF(@Temperature < 33)
BEGIN
	set @Temperature = 4;
END
ELSE IF(@Temperature < 37)
BEGIN
	set @Temperature = 1;
END
ElSE
BEGIN
	set @Temperature = 2;
END
--end
			Insert into Report_TemperatureReport(USER_KEY,FK_Vehicle,UpdateTime,Temperature) values(@USER_KEY,@FK_Vehicle,@UpdateTime,@Temperature);
			set @_PreviousUpdateTime = DATEADD(MINUTE, @IntervalMinute, @UpdateTime);
		END
	
		FETCH NEXT FROM data_cursor INTO @UpdateTime, @Temperature;  
	END     
	CLOSE data_cursor;    
	DEALLOCATE data_cursor;  
	select @RegistrationNumber as 'RegistrationNumber', UpdateTime, Temperature from Report_TemperatureReport where USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle;

END

--  EXEC _Report_GetTemperatureHistory '00000000-0000-0000-0000-000000000000', 'FBE0C6D8-F5A2-430B-A410-2C4C71DEBF91', '2019-01-09 00:00:00', '2019-01-10 00:00:00', 10;


GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleConsolidatedReport]    Script Date: 2021-05-25 10:58:11 AM ******/
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
	
	--#[3rdEyE_TrackingDataBase_2021_02]
	IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_03]
	IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_04]
	IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_05]
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2021_06]
	IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	BEGIN
		Insert into Report_ConsolidatedRport(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData 
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
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleConsolidatedReport_ReadyReport]    Script Date: 2021-05-25 10:58:11 AM ******/
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

	--#[3rdEyE_TrackingDataBase_2021_02]
	IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_02].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END

	--#[3rdEyE_TrackingDataBase_2021_03]
	IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_03].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END
	
	--#[3rdEyE_TrackingDataBase_2021_04]
	IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_04].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END
	
	--#[3rdEyE_TrackingDataBase_2021_05]
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_05].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END
	
	--#[3rdEyE_TrackingDataBase_2021_06]
	IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_06].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate 
		) t
	END
END

-- EXEC Report_GetVehicleConsolidatedReport_ReadyReport  '86AA5713-08A9-4CC3-AFCB-4541065A75B9', '2019-03-02';

GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleHaltTime]    Script Date: 2021-05-25 10:58:11 AM ******/
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

	--#[3rdEyE_TrackingDataBase_2021_02]
	IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_03]
	IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_04]
	IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_05]
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData 
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime Between @StartingDate AND @EndingDate
		Order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2021_06]
	IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	BEGIN
		Insert into Report_VehicleHaltReport_Helper(USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime)
		Select @USER_KEY ,Latitude ,Longitude ,Altitude ,EngineStatus ,Course ,Temperature ,Fuel ,Speed ,Distance ,UpdateTime ,ServerTime
		FROM [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData 
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
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleHaltTime_ReadyReport]    Script Date: 2021-05-25 10:58:11 AM ******/
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

	--#[3rdEyE_TrackingDataBase_2021_02]
	IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_02].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END

	--#[3rdEyE_TrackingDataBase_2021_03]
	IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_03].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END
	
	--#[3rdEyE_TrackingDataBase_2021_04]
	IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_04].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END
	
	--#[3rdEyE_TrackingDataBase_2021_05]
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_05].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END

	--#[3rdEyE_TrackingDataBase_2021_06]
	IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	BEGIN
		SELECT
		t.*, 
		(SELECT TOP 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) as 'NearestMapLocation',
		Round((SELECT TOP 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( t.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( t.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( t.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) as 'NearestMapLocationDistance'
		FROM(
		select Report_VehicleHaltReport.* 
			from [3rdEyE_TrackingDataBase_2021_06].dbo.Report_VehicleHaltReport 
			where FK_Vehicle = @FK_Vehicle and StartTime >= @StartingDate and EndTime < @EndingDate and (_rowType = 'data_initial_gap' OR _rowType = 'data' OR _rowType = 'data_finishing_gap') and HaltTime >= @MininumMinuteDealy
		) t
	END
END

--  EXEC Report_GetVehicleHaltTime_ReadyReport '0bdc0f82-d45b-4d9e-8c05-7032f36e0478', '01-Apr-19 12:00:00 AM', '02-Apr-19 12:00:00 AM', '5';



GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleHistory]    Script Date: 2021-05-25 10:58:11 AM ******/
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

	--#[3rdEyE_TrackingDataBase_2021_02]
	IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
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
		FROM [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END

	--#[3rdEyE_TrackingDataBase_2021_03]
	IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
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
		FROM [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_04]
	IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
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
		FROM [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END

	--#[3rdEyE_TrackingDataBase_2021_05]
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
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
		FROM [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END

	--#[3rdEyE_TrackingDataBase_2021_06]
	IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
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
		FROM [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
		WHERE FK_Vehicle = @FK_Vehicle AND UpdateTime BETWEEN @StartingDate AND @EndingDate AND (Status_PostionValidity = 'A' OR Status_PostionValidity = '1')
		GROUP BY (1440 * DATEPART(DAY, UpdateTime) + 60 * DATEPART(HOUR, UpdateTime) + FLOOR( DATEPART(MINUTE, UpdateTime) / @TimeLapMinute))
		) AS JoinedTable
	INNER JOIN
	[3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
	ON
	[3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData AND
	[3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData.PK_RowData = JoinedTable.PK_RowData
	ORDER BY UpdateTime ASC;
	END


	--# Final Selection
	SELECT * FROM Report_VehicleHistory WHERE USER_KEY = @USER_KEY;
	
END  

-- EXEC Report_GetVehicleHistory '00000000-0000-0000-0000-000000000000', '1a4a363f-591b-473c-9ed6-04bf7a4673ef', '26-Apr-20 1:13:00 PM', '26-Apr-20 2:05:00 PM', 1


GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleHistoryDetail]    Script Date: 2021-05-25 10:58:11 AM ******/
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
	
	--#[3rdEyE_TrackingDataBase_2021_02]
	IF((@StartingDate >= '2021-02-01' AND @StartingDate < '2021-03-01') OR (@EndingDate >= '2021-02-01' AND @EndingDate < '2021-03-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_02].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_03]
	IF((@StartingDate >= '2021-03-01' AND @StartingDate < '2021-04-01') OR (@EndingDate >= '2021-03-01' AND @EndingDate < '2021-04-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_03].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_04]
	IF((@StartingDate >= '2021-04-01' AND @StartingDate < '2021-05-01') OR (@EndingDate >= '2021-04-01' AND @EndingDate < '2021-05-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_04].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_05]
	IF((@StartingDate >= '2021-05-01' AND @StartingDate < '2021-06-01') OR (@EndingDate >= '2021-05-01' AND @EndingDate < '2021-06-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_05].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2021_06]
	IF((@StartingDate >= '2021-06-01' AND @StartingDate < '2021-07-01') OR (@EndingDate >= '2021-06-01' AND @EndingDate < '2021-07-01'))
	BEGIN
		insert into Report_VehicleHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, Latitude, Longitude, EngineStatus, Speed)
		select @USER_KEY, @FK_Vehicle, DeviceData.UpdateTime, DeviceData.Latitude, DeviceData.Longitude, DeviceData.EngineStatus, DeviceData.Speed from [3rdEyE_TrackingDataBase_2021_06].dbo.DeviceData
		where DeviceData.FK_Vehicle = @FK_Vehicle and DeviceData.UpdateTime >= @StartingDate and DeviceData.UpdateTime < @EndingDate
		order by UpdateTime;
	END

	SELECT * FROM Report_VehicleHistoryDetail where @USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle;

END  
-- exec Report_GetVehicleHistoryDetail '3c2540db-2f16-4487-bc64-5489328de706', '22-Oct-18 6:00:00 PM', '22-Oct-18 7:00:00 PM';



GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleInOutHistoryDetail]    Script Date: 2021-05-25 10:58:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Report_GetVehicleInOutHistoryDetail] (
@USER_KEY uniqueidentifier,
@FK_Vehicle uniqueidentifier,
@StartingDate datetime,
@EndingDate datetime
)
AS
BEGIN
	
	delete from Report_VehicleInOutHistoryDetail where @USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle;

	--#[3rdEyE_TrackingDataBase_2019_05]
	IF((@StartingDate >= '2019-05-01' AND @StartingDate < '2019-06-01') OR (@EndingDate >= '2019-05-01' AND @EndingDate < '2019-06-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2019_05].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2019_06]
	IF((@StartingDate >= '2019-06-01' AND @StartingDate < '2019-07-01') OR (@EndingDate >= '2019-06-01' AND @EndingDate < '2019-07-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2019_06].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2019_07]
	IF((@StartingDate >= '2019-07-01' AND @StartingDate < '2019-08-01') OR (@EndingDate >= '2019-07-01' AND @EndingDate < '2019-08-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2019_07].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2019_08]
	IF((@StartingDate >= '2019-08-01' AND @StartingDate < '2019-09-01') OR (@EndingDate >= '2019-08-01' AND @EndingDate < '2019-09-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2019_08].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END

	--#[3rdEyE_TrackingDataBase_2019_09]
	IF((@StartingDate >= '2019-09-01' AND @StartingDate < '2019-10-01') OR (@EndingDate >= '2019-09-01' AND @EndingDate < '2019-10-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2019_09].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2019_10]
	IF((@StartingDate >= '2019-10-01' AND @StartingDate < '2019-11-01') OR (@EndingDate >= '2019-10-01' AND @EndingDate < '2019-11-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2019_10].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2019_11]
	IF((@StartingDate >= '2019-11-01' AND @StartingDate < '2019-12-01') OR (@EndingDate >= '2019-11-01' AND @EndingDate < '2019-12-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2019_11].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2019_12]
	IF((@StartingDate >= '2019-12-01' AND @StartingDate < '2020-01-01') OR (@EndingDate >= '2019-12-01' AND @EndingDate < '2020-01-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2019_12].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2020_01]
	IF((@StartingDate >= '2020-01-01' AND @StartingDate < '2020-02-01') OR (@EndingDate >= '2020-01-01' AND @EndingDate < '2020-02-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2020_01].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END
	
	--#[3rdEyE_TrackingDataBase_2020_02]
	IF((@StartingDate >= '2020-02-01' AND @StartingDate < '2020-03-01') OR (@EndingDate >= '2020-02-01' AND @EndingDate < '2020-03-01'))
	BEGIN
		insert into Report_VehicleInOutHistoryDetail (USER_KEY, FK_Vehicle, UpdateTime, FK_Depo, Latitude, Longitude)
		select @USER_KEY, @FK_Vehicle, VehicleInDepo.UpdateTime, VehicleInDepo.FK_Depo, VehicleInDepo.Latitude, VehicleInDepo.Longitude  from [3rdEyE_TrackingDataBase_2020_02].dbo.VehicleInDepo
		where VehicleInDepo.FK_Vehicle = @FK_Vehicle and VehicleInDepo.UpdateTime >= @StartingDate and VehicleInDepo.UpdateTime < @EndingDate
		order by UpdateTime;
	END

	SELECT * FROM Report_VehicleInOutHistoryDetail where @USER_KEY = @USER_KEY and FK_Vehicle = @FK_Vehicle order by Report_VehicleInOutHistoryDetail.UpdateTime;

END  
-- exec Report_GetVehicleInOutHistoryDetail '3c2540db-2f16-4487-bc64-5489328de706', '22-Oct-18 6:00:00 PM', '22-Oct-18 7:00:00 PM';



GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleLastUpdate_withNearMapLocation]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleTracking]    Script Date: 2021-05-25 10:58:11 AM ******/
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
	--,(CASE WHEN VehicleTracking.FK_Depo_In > VehicleTracking.FK_Depo_Out  THEN ('Entered ' + DepoIn.Name + ' At ' + Format(VehicleTracking.DepoInDateTime, 'dd.MM.yy hh:mm tt') +' (aprx)') ELSE ('Left ' + DepoOut.Name + ' At ' + Format(VehicleTracking.DepoOutDateTime, 'dd.MM.yy hh:mm tt') +' (aprx)') END) as 'DepoInOutData'
	--,'' as 'DepoInOutData'
	,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 15 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 

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
	order by Status desc;
	;
END  


-- EXEC _Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'





GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleTrackingByDepo]    Script Date: 2021-05-25 10:58:11 AM ******/
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
		,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 15 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 


		FROM Vehicle
		JOIN VehicleTracking ON Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
		JOIN VehicleTrackingInformation ON Vehicle.PK_Vehicle = VehicleTrackingInformation.PK_Vehicle
		LEFT JOIN Depo ON Vehicle.FK_Depo = Depo.PK_Depo
		LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
		--LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
		Where Vehicle.FK_Depo = @PK_Depo
		order by Status desc;
		;
	END  
	-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'



GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleTrackingByDepoGroup]    Script Date: 2021-05-25 10:58:11 AM ******/
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
		,(CASE WHEN VehicleTrackingInformation.GpsIMEINumber is null THEN -3 ELSE (CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE(CASE WHEN DATEDIFF(mi, VehicleTracking.UpdateTime, @CurrentDateaTime) > 15 THEN -1 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1 AND VehicleTracking.Speed > 0 THEN 2 ELSE (CASE WHEN VehicleTracking.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) END) AS 'Status' 



		FROM Vehicle
		JOIN VehicleTracking ON Vehicle.PK_Vehicle = VehicleTracking.PK_Vehicle
		JOIN VehicleTrackingInformation ON Vehicle.PK_Vehicle = VehicleTrackingInformation.PK_Vehicle
		LEFT JOIN Depo ON Vehicle.FK_Depo = Depo.PK_Depo
		LEFT JOIN Company AS UserCompany ON Vehicle.FK_Company = UserCompany.PK_Company
		--LEFT JOIN VehicleModel ON Vehicle.FK_VehicleModel = VehicleModel.PK_VehicleModel
		Where Vehicle.FK_DepoGroup = @PK_DepoGroup
		order by Status desc;
		;
	END  
	-- EXEC Report_GetVehicleTracking '00000000-0000-0000-0000-000000000000'



GO
/****** Object:  StoredProcedure [dbo].[Report_GetVehicleTrackingForIndividualRequisition]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ReportMobile_GetVehicleTracking_Far]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ReportMobile_GetVehicleTracking_Near]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[ReportMobile_GetVehicleTracking_Single]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_JOB_RegularJob]    Script Date: 2021-05-25 10:58:11 AM ******/
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
	StayTimeMinute = DATEDIFF(MINUTE, In_IssueDateTime,Out_IssueDateTime)
	where Out_IssueDateTime is not null
	and (DATEDIFF(MINUTE, In_IssueDateTime,Out_IssueDateTime)) != StayTimeMinute;
END




GO
/****** Object:  StoredProcedure [dbo].[Temp_T1_report]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[Temp_T366_report]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[Temp_VT1_report]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[Tracking_GetLastDeviceData]    Script Date: 2021-05-25 10:58:11 AM ******/
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
	 from VehicleTracking where PK_Vehicle = @FK_Vehicle;
END

-- EXEC Tracking_GetNextDeviceData '2B3B36A7-E90B-49A7-AD50-6D74B1CF27C4', '2018-08-07 10:28:44.000' 

GO
/****** Object:  StoredProcedure [dbo].[Tracking_GetNextDeviceData]    Script Date: 2021-05-25 10:58:11 AM ******/
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
	(CASE WHEN DATEDIFF(mi, DeviceData.UpdateTime, @CurrentDateaTime) > 720 THEN -2 ELSE (CASE WHEN DATEDIFF(mi, DeviceData.UpdateTime, @CurrentDateaTime) > 30 THEN -1 ELSE (CASE WHEN DeviceData.EngineStatus = 1 AND DeviceData.Speed > 0 THEN 2 ELSE (CASE WHEN DeviceData.EngineStatus = 1  THEN 1 ELSE 0 END) END) END) END) AS 'Status',
	(SELECT top 1  Name  from MapLocation order by (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) )))) AS 'NearestMapLocation',
	Round((SELECT top 1  (3956 * 2 * ASIN(SQRT( POWER(SIN(( DeviceData.Latitude - MapLocation.Latitude) *  pi()/180 / 2), 2) +COS( DeviceData.Latitude * pi()/180) * COS(MapLocation.Latitude * pi()/180) * POWER(SIN(( DeviceData.Longitude - MapLocation.Longitude) * pi()/180 / 2), 2) ))) as distance from MapLocation order by distance),2) AS 'NearestMapLocationDistance'
	 from [3rdEyE].dbo.DeviceData where FK_Vehicle = @FK_Vehicle and UpdateTime> @PreviousUpdateTime order by UpdateTime;
END


GO
/****** Object:  StoredProcedure [dbo].[VehicleTracking_UpdateToGPSLost]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[VehicleTracking_UpdateToGPSRecover]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[VehicleTracking_UpdateToLowBattery]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[VTS_SIM_VerificationReceive]    Script Date: 2021-05-25 10:58:11 AM ******/
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
/****** Object:  StoredProcedure [dbo].[VTS_SIM_VerificationSend]    Script Date: 2021-05-25 10:58:11 AM ******/
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
USE [master]
GO
ALTER DATABASE [3rdEyE] SET  READ_WRITE 
GO
