USE [3rdEyE]
GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[PK_Vehicle] [uniqueidentifier] NOT NULL,
	[ERP_Id] [bigint] NULL,
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
	[FK_ParkingInOut_Last] [bigint] NULL,
	[FK_LoadingBayUtilization_Last] [bigint] NULL,
	[FK_RequisitionTrip_Last] [bigint] NULL,
 CONSTRAINT [PK_Vehicle_1] PRIMARY KEY CLUSTERED 
(
	[PK_Vehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Vehicle__E8864602BF0E2FFF] UNIQUE NONCLUSTERED 
(
	[RegistrationNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[RowSerial] [bigint] IDENTITY(1,1) NOT NULL,
	[PK_Location] [uniqueidentifier] NOT NULL,
	[DevelopersNote] [varchar](50) NULL,
	[IsDeleted] [bit] NOT NULL,
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
	[SCM_LocationId] [varchar](40) NULL,
 CONSTRAINT [PK_Location_Physical] PRIMARY KEY CLUSTERED 
(
	[PK_Location] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Location__737584F627594474] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocationBuilding]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationBuilding](
	[PK_LocationBuilding] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[PRG_Type] [varchar](10) NOT NULL,
	[FK_Location] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
	[GraceVehicleCount] [int] NULL,
 CONSTRAINT [PK_LocationBuilding] PRIMARY KEY CLUSTERED 
(
	[PK_LocationBuilding] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_LocationBuilding_FK_Location_WITH_Name] UNIQUE NONCLUSTERED 
(
	[FK_Location] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoadingBay]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoadingBay](
	[PK_LoadingBay] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[FK_LocationBuilding] [bigint] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
	[FK_LoadingBayUtilization_Last] [bigint] NULL,
	[CurrentUseType] [varchar](10) NULL,
 CONSTRAINT [PK_LoadingBay] PRIMARY KEY CLUSTERED 
(
	[PK_LoadingBay] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_loadingBay_FK_LocationBuilding_WIITH_Name] UNIQUE NONCLUSTERED 
(
	[FK_LocationBuilding] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Requisition]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requisition](
	[PK_Requisition] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[TrackingID] [varchar](100) NULL,
	[FK_Location_From] [uniqueidentifier] NULL,
	[StartingLocation] [varchar](300) NULL,
	[FK_Location_To] [uniqueidentifier] NULL,
	[FinishingLocation] [varchar](300) NULL,
	[PossibleJourneyStartDateTime] [datetime] NOT NULL,
	[FK_RequisitionVehicleType] [int] NULL,
	[FK_RequisitionBusinessUnit] [bigint] NULL,
	[FK_RequisitionProductType] [bigint] NULL,
	[WantedCount] [float] NULL,
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
	[OrganizationCode] [varchar](10) NULL,
	[OrganizationName] [varchar](100) NULL,
	[FK_LocationDepartment_From] [bigint] NULL,
	[FK_LocationDepartment_To] [bigint] NULL,
	[LCNumber] [varchar](50) NULL,
	[AttachedFile1_Path] [varchar](300) NULL,
	[AttachedFile2_Path] [varchar](300) NULL,
	[AttachedFile3_Path] [varchar](300) NULL,
	[AttachedFile4_Path] [varchar](300) NULL,
	[MailSend] [bit] NULL,
	[MailReceiverGroup] [varchar](100) NULL,
	[MailSentAt] [datetime] NULL,
 CONSTRAINT [PK_InterCompanyRequisitionNew_1] PRIMARY KEY CLUSTERED 
(
	[PK_Requisition] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Depo]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_AppUser_UniqueIDNumber] UNIQUE NONCLUSTERED 
(
	[UniqueIDNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocationDepartment]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationDepartment](
	[PK_LocationDepartment] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Location] [uniqueidentifier] NULL,
	[PRG_Type] [varchar](50) NOT NULL,
	[DepartmentName] [varchar](50) NOT NULL,
	[DepartmentCode] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LocationDepartment] PRIMARY KEY CLUSTERED 
(
	[PK_LocationDepartment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [LocationDepartment_Uniqueness_Location_PRG_Code] UNIQUE NONCLUSTERED 
(
	[FK_Location] ASC,
	[PRG_Type] ASC,
	[DepartmentCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [LocationDepartment_Uniqueness_Location_PRG_Name] UNIQUE NONCLUSTERED 
(
	[FK_Location] ASC,
	[PRG_Type] ASC,
	[DepartmentName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParkingInOut]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParkingInOut](
	[PK_ParkingInOut] [bigint] IDENTITY(1,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[FK_Location] [uniqueidentifier] NOT NULL,
	[InOrOut] [bit] NULL,
	[In_IssueDateTime] [datetime] NOT NULL,
	[In_FK_CreatedByUser] [uniqueidentifier] NOT NULL,
	[BayAssign_IssueDateTime] [datetime] NULL,
	[BayAssign_FK_CreatedByUser] [uniqueidentifier] NULL,
	[FactoryIn_IssueDateTime] [datetime] NULL,
	[FactoryIn_FK_CreatedByUser] [uniqueidentifier] NULL,
	[Out_IssueDateTime] [datetime] NULL,
	[Out_FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_RequisitionTrip] [bigint] NULL,
	[IsBeforeOrAfter] [bit] NULL,
	[FK_PRG_Type] [bigint] NULL,
	[BayAssign_BayName] [varchar](50) NULL,
	[BayAssign_FK_LocationDepartment] [bigint] NULL,
	[In_LoadOrEmpty] [varchar](50) NULL,
	[FactoryOut_IssueDateTime] [datetime] NULL,
	[FactoryOut_FK_CreatedByUser] [uniqueidentifier] NULL,
	[BayAssign_WareHouseName] [varchar](50) NULL,
	[FK_Requisition] [bigint] NULL,
	[BayAssign_FK_LocationBuilding] [bigint] NULL,
	[BayAssign_FK_LoadingBay] [bigint] NULL,
	[FK_LoadingBayUtilization] [bigint] NULL,
 CONSTRAINT [PK_ParkingInOut] PRIMARY KEY CLUSTERED 
(
	[PK_ParkingInOut] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_ParkingInOut_Live]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_ParkingInOut_Live] AS

---------------------------START----------------------------
SELECT 1 as stat,

--ParkingInOut
ParkingInOut.PK_ParkingInOut,
(CASE WHEN ParkingInOut.InOrOut = 1 THEN 'In' 
ELSE 'Out' END) as InOutStatus,
ParkingInOut.In_LoadOrEmpty,

--PRG_Type
(CASE WHEN ParkingInOut.FK_PRG_Type = 1 THEN 'PRAN'  ELSE 
		(CASE WHEN ParkingInOut.FK_PRG_Type = 2 THEN 'RFL'
			ELSE 'Not Found' END) 
		END)
as PRG_Type,

--ParkingLocation
ParkingGateIn.FK_Location as FK_Location,
PrakingLocation.Name as ParkingLocation_Name,

--Gates
ParkingInOut.In_FK_CreatedByUser,
ParkingInOut.Out_FK_CreatedByUser,
ParkingInOut.FactoryIn_FK_CreatedByUser,
ParkingInOut.FactoryOut_FK_CreatedByUser,

ParkingGateIn.FullName as ParkingGateIn, 
ParkingInOut.In_IssueDateTime,
ParkingGateOut.FullName as ParkingGateOut, 
ParkingInOut.Out_IssueDateTime,
FactoryGateIn.FullName as FactoryGateIn, 
ParkingInOut.FactoryIn_IssueDateTime,
FactoryGateOut.FullName as FactoryGateOut, 
ParkingInOut.FactoryOut_IssueDateTime,

--Vehicle
Vehicle.RegistrationNumber,
Vehicle.OWN_MHT_DHT,
Vehicle.FK_Depo,
Vehicle.Internal_VehicleContactNumber +' '+Vehicle.MHT_DHT_DriverContactNumber as ContactNumber,

--Depo
Depo.Name as 'DepoName',

--BayAssign
ParkingInOut.BayAssign_IssueDateTime,
ParkingInOut.BayAssign_BayName,
ParkingInOut.BayAssign_WareHouseName,

--DepartmentInfo
LocationDepartment_BayAssign.DepartmentCode as DepartmentInfo_BayAssign,
LocationDepartment_From.DepartmentCode as DepartmentInfo_From,
LocationDepartment_To.DepartmentCode as DepartmentInfo_To,

--LoadingBay
LocationBuilding.Name as BayAssign_LocationBuilding,
LoadingBay.Name as BayAssign_LoadingBay,

(CASE WHEN LocationDepartment_BayAssign.DepartmentCode is not null THEN LocationDepartment_BayAssign.DepartmentCode else 
	(CASE WHEN ParkingInOut.In_LoadOrEmpty = 'Loaded' and LocationDepartment_To.DepartmentCode is not null then LocationDepartment_To.DepartmentCode else
		(CASE WHEN ParkingInOut.In_LoadOrEmpty = 'Empty' and LocationDepartment_From.DepartmentCode is not null then LocationDepartment_From.DepartmentCode else 'Not Found' 
			END)
		END
	)
END) as DepartmentCode_Final,
(CASE WHEN LocationDepartment_BayAssign.PK_LocationDepartment is not null THEN LocationDepartment_BayAssign.PK_LocationDepartment else 
	(CASE WHEN ParkingInOut.In_LoadOrEmpty = 'Loaded' and LocationDepartment_To.PK_LocationDepartment is not null then LocationDepartment_To.PK_LocationDepartment else
		(CASE WHEN ParkingInOut.In_LoadOrEmpty = 'Empty' and LocationDepartment_From.PK_LocationDepartment is not null then LocationDepartment_From.PK_LocationDepartment else 0
			END)
		END
	)
END) as FK_LocationDepartment_Final,

----PK_RequisitionTrip
--(CASE WHEN RequisitionTrip_Current.PK_RequisitionTrip is not null THEN RequisitionTrip_Current.PK_RequisitionTrip 
--ELSE RequisitionTrip_Finished.PK_RequisitionTrip_Finished END) as PK_RequisitionTrip,

----RequisitionTrip_TrackingID
--(CASE WHEN RequisitionTrip_Current.PK_RequisitionTrip is not null THEN RequisitionTrip_Current.TrackingID 
--ELSE RequisitionTrip_Finished.TrackingID END) as RequisitionTrip_TrackingID,

--PK_Requisition
Requisition.PK_Requisition,

--Requisition_TrackingID
Requisition.TrackingID as RequisitionTrackingID

----Location_From
--(CASE WHEN Requisition.PK_Requisition is not null THEN Location_From.Name
--ELSE Location_Finished_From.Name END) as Location_From,

----Location_To
--(CASE WHEN Requisition.PK_Requisition is not null THEN Location_To.Name
--ELSE Location_Finished_To.Name END) as Location_To,

from ParkingInOut
join AppUser as ParkingGateIn on ParkingInOut.In_FK_CreatedByUser = ParkingGateIn.PK_User
left join AppUser as ParkingGateOut on ParkingInOut.Out_FK_CreatedByUser = ParkingGateOut.PK_User
left join AppUser as FactoryGateIn on ParkingInOut.FactoryIn_FK_CreatedByUser = FactoryGateIn.PK_User
left join AppUser as FactoryGateOut on ParkingInOut.FactoryOut_FK_CreatedByUser = FactoryGateOut.PK_User
join Location as PrakingLocation on ParkingInOut.FK_Location = PrakingLocation.PK_Location
join Vehicle on ParkingInOut.FK_Vehicle = Vehicle.PK_Vehicle
join Depo on Vehicle.FK_Depo = Depo.PK_Depo

--left join RequisitionTrip as RequisitionTrip_Current on ParkingInOut.FK_RequisitionTrip = RequisitionTrip_Current.PK_RequisitionTrip
left join Requisition as Requisition on ParkingInOut.FK_Requisition = Requisition.PK_Requisition
--left join Location as Location_From on Requisition.FK_Location_From = Location_From.PK_Location
--left join Location as Location_To on Requisition.FK_Location_To = Location_To.PK_Location
--left join LocationDepartment as LocationDepartment_From on Requisition.FK_LocationDepartment_From = LocationDepartment_From.PK_LocationDepartment
--left join LocationDepartment as LocationDepartment_To on Requisition.FK_LocationDepartment_To = LocationDepartment_To.PK_LocationDepartment

--left join RequisitionTrip_Finished on ParkingInOut.FK_RequisitionTrip = RequisitionTrip_Finished.PK_RequisitionTrip_Finished
--left join Requisition as Requisition_Finished on RequisitionTrip_Finished.FK_Requisition = Requisition_Finished.PK_Requisition
--left join Location as Location_Finished_From on Requisition_Finished.FK_Location_From = Location_Finished_From.PK_Location
--left join Location as Location_Finished_To on Requisition_Finished.FK_Location_To = Location_Finished_To.PK_Location
--left join LocationDepartment as LocationDepartment_Finished_From on Requisition_Finished.FK_LocationDepartment_From = LocationDepartment_Finished_From.PK_LocationDepartment
--left join LocationDepartment as LocationDepartment_Finished_To on Requisition_Finished.FK_LocationDepartment_To = LocationDepartment_Finished_To.PK_LocationDepartment;


left join LocationDepartment as LocationDepartment_BayAssign on ParkingInOut.BayAssign_FK_LocationDepartment = LocationDepartment_BayAssign.PK_LocationDepartment
left join LocationDepartment as LocationDepartment_From on Requisition.FK_LocationDepartment_From = LocationDepartment_From.PK_LocationDepartment
left join LocationDepartment as LocationDepartment_To on Requisition.FK_LocationDepartment_To = LocationDepartment_To.PK_LocationDepartment
left join LoadingBay on ParkingInOut.BayAssign_FK_LoadingBay = LoadingBay.PK_LoadingBay
left join LocationBuilding on LoadingBay.FK_LocationBuilding = LocationBuilding.PK_LocationBuilding


---------------------------END------------------------------

GO
/****** Object:  Table [dbo].[VehicleInOutManual]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleInOutManual](
	[PK_VehicleInOutManual] [bigint] IDENTITY(1,1) NOT NULL,
	[DevelopersNote] [varchar](300) NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[FK_Location] [uniqueidentifier] NOT NULL,
	[FK_PRG_Type] [bigint] NULL,
	[InOrOut] [bit] NULL,
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
	[SCM_Database_Status] [varchar](10) NULL,
 CONSTRAINT [PK_VehicleInOutManual] PRIMARY KEY CLUSTERED 
(
	[PK_VehicleInOutManual] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_StayReportDaily]    Script Date: 2022-02-24 11:24:49 AM ******/
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
/****** Object:  View [dbo].[vw_ParkingInOut_Detail]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO















CREATE VIEW [dbo].[vw_ParkingInOut_Detail] AS

---------------------------START----------------------------
SELECT 1 as stat,

--ParkingInOut
ParkingInOut.PK_ParkingInOut,
(CASE WHEN ParkingInOut.InOrOut = 1 THEN 'In' 
ELSE 'Out' END) as InOutStatus,
ParkingInOut.In_LoadOrEmpty,

--PRG_Type
(CASE WHEN ParkingInOut.FK_PRG_Type = 1 THEN 'PRAN'  ELSE 
		(CASE WHEN ParkingInOut.FK_PRG_Type = 2 THEN 'RFL'
			ELSE 'Not Found' END) 
		END)
as PRG_Type,

--ParkingLocation
ParkingGateIn.FK_Location as FK_Location,
PrakingLocation.Name as ParkingLocation_Name,

--Gates
ParkingInOut.In_FK_CreatedByUser,
ParkingInOut.Out_FK_CreatedByUser,
ParkingInOut.FactoryIn_FK_CreatedByUser,
ParkingInOut.FactoryOut_FK_CreatedByUser,

ParkingGateIn.FullName as ParkingGateIn, 
ParkingInOut.In_IssueDateTime,
ParkingGateOut.FullName as ParkingGateOut, 
ParkingInOut.Out_IssueDateTime,
FactoryGateIn.FullName as FactoryGateIn, 
ParkingInOut.FactoryIn_IssueDateTime,
FactoryGateOut.FullName as FactoryGateOut, 
ParkingInOut.FactoryOut_IssueDateTime,

--Vehicle
Vehicle.RegistrationNumber,
Vehicle.OWN_MHT_DHT,
Vehicle.FK_Depo,
Vehicle.Internal_VehicleContactNumber +' '+Vehicle.MHT_DHT_DriverContactNumber as ContactNumber,

--Depo
Depo.Name as 'DepoName',

--BayAssign
ParkingInOut.BayAssign_IssueDateTime,
ParkingInOut.BayAssign_BayName,
ParkingInOut.BayAssign_WareHouseName,
ParkingInOut.FK_LoadingBayUtilization,

--DepartmentInfo
LocationDepartment_BayAssign.DepartmentCode as DepartmentInfo_BayAssign,
LocationDepartment_From.DepartmentCode as DepartmentInfo_From,
LocationDepartment_To.DepartmentCode as DepartmentInfo_To,

--LoadingBay
LoadingBay.FK_LocationBuilding as BayAssign_FK_LocationBuilding,
LocationBuilding.Name as BayAssign_LocationBuilding,
ParkingInOut.BayAssign_FK_LoadingBay,
LoadingBay.Name as BayAssign_LoadingBay,

(CASE WHEN LocationDepartment_BayAssign.DepartmentCode is not null THEN LocationDepartment_BayAssign.DepartmentCode else 
	(CASE WHEN ParkingInOut.In_LoadOrEmpty = 'Loaded' and LocationDepartment_To.DepartmentCode is not null then LocationDepartment_To.DepartmentCode else
		(CASE WHEN ParkingInOut.In_LoadOrEmpty = 'Empty' and LocationDepartment_From.DepartmentCode is not null then LocationDepartment_From.DepartmentCode else 'Not Found' 
			END)
		END
	)
END) as DepartmentCode_Final,
(CASE WHEN LocationDepartment_BayAssign.PK_LocationDepartment is not null THEN LocationDepartment_BayAssign.PK_LocationDepartment else 
	(CASE WHEN ParkingInOut.In_LoadOrEmpty = 'Loaded' and LocationDepartment_To.PK_LocationDepartment is not null then LocationDepartment_To.PK_LocationDepartment else
		(CASE WHEN ParkingInOut.In_LoadOrEmpty = 'Empty' and LocationDepartment_From.PK_LocationDepartment is not null then LocationDepartment_From.PK_LocationDepartment else 0
			END)
		END
	)
END) as FK_LocationDepartment_Final,

----PK_RequisitionTrip
--(CASE WHEN RequisitionTrip_Current.PK_RequisitionTrip is not null THEN RequisitionTrip_Current.PK_RequisitionTrip 
--ELSE RequisitionTrip_Finished.PK_RequisitionTrip_Finished END) as PK_RequisitionTrip,

----RequisitionTrip_TrackingID
--(CASE WHEN RequisitionTrip_Current.PK_RequisitionTrip is not null THEN RequisitionTrip_Current.TrackingID 
--ELSE RequisitionTrip_Finished.TrackingID END) as RequisitionTrip_TrackingID,

--PK_Requisition
Requisition.PK_Requisition,

--Requisition_TrackingID
Requisition.TrackingID as RequisitionTrackingID

----Location_From
--(CASE WHEN Requisition.PK_Requisition is not null THEN Location_From.Name
--ELSE Location_Finished_From.Name END) as Location_From,

----Location_To
--(CASE WHEN Requisition.PK_Requisition is not null THEN Location_To.Name
--ELSE Location_Finished_To.Name END) as Location_To,

from ParkingInOut
join AppUser as ParkingGateIn on ParkingInOut.In_FK_CreatedByUser = ParkingGateIn.PK_User
left join AppUser as ParkingGateOut on ParkingInOut.Out_FK_CreatedByUser = ParkingGateOut.PK_User
left join AppUser as FactoryGateIn on ParkingInOut.FactoryIn_FK_CreatedByUser = FactoryGateIn.PK_User
left join AppUser as FactoryGateOut on ParkingInOut.FactoryOut_FK_CreatedByUser = FactoryGateOut.PK_User
join Location as PrakingLocation on ParkingInOut.FK_Location = PrakingLocation.PK_Location
join Vehicle on ParkingInOut.FK_Vehicle = Vehicle.PK_Vehicle
join Depo on Vehicle.FK_Depo = Depo.PK_Depo

--left join RequisitionTrip as RequisitionTrip_Current on ParkingInOut.FK_RequisitionTrip = RequisitionTrip_Current.PK_RequisitionTrip
left join Requisition as Requisition on ParkingInOut.FK_Requisition = Requisition.PK_Requisition
--left join Location as Location_From on Requisition.FK_Location_From = Location_From.PK_Location
--left join Location as Location_To on Requisition.FK_Location_To = Location_To.PK_Location
--left join LocationDepartment as LocationDepartment_From on Requisition.FK_LocationDepartment_From = LocationDepartment_From.PK_LocationDepartment
--left join LocationDepartment as LocationDepartment_To on Requisition.FK_LocationDepartment_To = LocationDepartment_To.PK_LocationDepartment

--left join RequisitionTrip_Finished on ParkingInOut.FK_RequisitionTrip = RequisitionTrip_Finished.PK_RequisitionTrip_Finished
--left join Requisition as Requisition_Finished on RequisitionTrip_Finished.FK_Requisition = Requisition_Finished.PK_Requisition
--left join Location as Location_Finished_From on Requisition_Finished.FK_Location_From = Location_Finished_From.PK_Location
--left join Location as Location_Finished_To on Requisition_Finished.FK_Location_To = Location_Finished_To.PK_Location
--left join LocationDepartment as LocationDepartment_Finished_From on Requisition_Finished.FK_LocationDepartment_From = LocationDepartment_Finished_From.PK_LocationDepartment
--left join LocationDepartment as LocationDepartment_Finished_To on Requisition_Finished.FK_LocationDepartment_To = LocationDepartment_Finished_To.PK_LocationDepartment;


left join LocationDepartment as LocationDepartment_BayAssign on ParkingInOut.BayAssign_FK_LocationDepartment = LocationDepartment_BayAssign.PK_LocationDepartment
left join LocationDepartment as LocationDepartment_From on Requisition.FK_LocationDepartment_From = LocationDepartment_From.PK_LocationDepartment
left join LocationDepartment as LocationDepartment_To on Requisition.FK_LocationDepartment_To = LocationDepartment_To.PK_LocationDepartment
left join LoadingBay on ParkingInOut.BayAssign_FK_LoadingBay = LoadingBay.PK_LoadingBay
left join LocationBuilding on ParkingInOut.BayAssign_FK_LocationBuilding = LocationBuilding.PK_LocationBuilding


---------------------------END------------------------------

GO
/****** Object:  View [dbo].[SillyView]    Script Date: 2022-02-24 11:24:49 AM ******/
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
/****** Object:  Table [dbo].[Accident]    Script Date: 2022-02-24 11:24:49 AM ******/
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
	[OthersCost] [bigint] NULL,
	[FK_DepoFollowUp] [uniqueidentifier] NULL,
	[ActionTakenStaffID] [varchar](100) NULL,
	[ActionTakenStaffName] [varchar](300) NULL,
	[Note] [varchar](max) NULL,
	[Status] [tinyint] NULL,
	[SettlementNote] [varchar](max) NULL,
	[FK_SettledByUser] [uniqueidentifier] NULL,
	[SettledAt] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[DeletedAt] [datetime] NULL,
	[FK_CreatedByUser] [uniqueidentifier] NULL,
	[FK_UpdatedByUser] [uniqueidentifier] NULL,
	[FK_DeletedByUser] [uniqueidentifier] NULL,
	[OthersNote] [varchar](300) NULL,
	[CompensationTaken] [bigint] NULL,
	[CompensationTakenFrom] [varchar](300) NULL,
	[CompensationTakenStaffID] [varchar](100) NULL,
	[CompensationTakenStaffName] [varchar](300) NULL,
	[DeductionTakenFromDriver] [bigint] NULL,
 CONSTRAINT [PK_Accident] PRIMARY KEY CLUSTERED 
(
	[PK_Accident] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccidentDocument]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccidentExpense]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertEmail]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertEmailAttachedDepo]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlertEmailLog]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppErrorLog]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppMenu]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppPermission]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppRole]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppRole_AppMenu]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppRole_AppPermission]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppRole_AppSubMenu]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppSetting]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppSubMenu]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUser_AppMenu]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser_AppMenu](
	[PK_AppUser_AppMenu] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppUser] [uniqueidentifier] NOT NULL,
	[FK_AppMenu] [bigint] NOT NULL,
	[IsAccessible] [bit] NULL,
	[Sequence] [int] NULL,
 CONSTRAINT [PK_AppUser_AppMenu] PRIMARY KEY CLUSTERED 
(
	[PK_AppUser_AppMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUser_AppPermission]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser_AppPermission](
	[PK_AppUser_AppPermission] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppUser] [uniqueidentifier] NOT NULL,
	[FK_AppPermission] [bigint] NOT NULL,
	[IsAccessible] [bit] NULL,
 CONSTRAINT [PK_AppUser_AppPermission] PRIMARY KEY CLUSTERED 
(
	[PK_AppUser_AppPermission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUser_AppSubMenu]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser_AppSubMenu](
	[PK_AppUser_AppSubMenu] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_AppUser] [uniqueidentifier] NOT NULL,
	[FK_AppSubMenu] [bigint] NOT NULL,
	[IsAccessible] [bit] NULL,
 CONSTRAINT [PK_AppUser_AppSubMenu] PRIMARY KEY CLUSTERED 
(
	[PK_AppUser_AppSubMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserAccessibleDepo]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserLoginHistory]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserSurpervisedContructualCompany]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContructualRequisition]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContructualRequisitionCompany]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContructualRequisitionDetail]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContructualRequisitionDetailEntry]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DairyVehicle]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dealer]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepoBorder]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepoGroup]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeviceData]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DHT_RequisitionTrip]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DHT_RequisitionTrip](
	[PK_DHT_RequisitionTrip] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[RegistrationNumber] [varchar](100) NULL,
	[FK_RequisitionTrip] [bigint] NULL,
	[FK_Location_From] [uniqueidentifier] NULL,
	[FK_Location_To] [uniqueidentifier] NULL,
	[FinalWantedAtDateTime] [datetime] NULL,
	[CreatedAt] [date] NULL,
 CONSTRAINT [PK_DHT_RequisitionTrip] PRIMARY KEY CLUSTERED 
(
	[PK_DHT_RequisitionTrip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DisplayMessage]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[District]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Driver]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventDocument]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventType]    Script Date: 2022-02-24 11:24:49 AM ******/
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
	[NextCycleYears] [int] NULL,
 CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED 
(
	[PK_EventType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancingCompany]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[PK_FinancingCompany] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FuelConsumptionDemand]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FuelConsumptionDemand](
	[PK_FuelConsumptionDemand] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[TrackingID] [varchar](100) NULL,
	[PRG_Type] [varchar](10) NULL,
	[FK_FuelPump] [bigint] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NOT NULL,
	[FK_RequisitionTrip] [bigint] NULL,
	[FK_LocationFrom] [uniqueidentifier] NULL,
	[FK_LocationTo] [uniqueidentifier] NULL,
	[RequiredAt] [datetime] NOT NULL,
	[FuelType] [varchar](10) NULL,
	[FuelUnitType] [varchar](10) NULL,
	[Status] [varchar](10) NOT NULL,
	[Vehicle_KPL] [decimal](16, 2) NOT NULL,
	[RequiredQuantity_Auto] [decimal](16, 2) NOT NULL,
	[RequiredQuantity_Manual] [decimal](16, 2) NOT NULL,
	[FuelUnitPrice] [decimal](16, 2) NOT NULL,
	[GivenQuantity] [decimal](16, 2) NULL,
	[GivenQuantityPrice] [decimal](16, 2) NULL,
	[ExcessQuantity] [decimal](16, 2) NULL,
	[ExcessQuantityPrice] [decimal](16, 2) NULL,
	[BusinessUnit_1] [varchar](10) NULL,
	[BusinessUnit_1_CarringPercentage] [decimal](16, 2) NULL,
	[BusinessUnit_1_GivenQuantityPrice] [decimal](16, 2) NULL,
	[BusinessUnit_2] [varchar](10) NULL,
	[BusinessUnit_2_CarringPercentage] [decimal](16, 2) NULL,
	[BusinessUnit_2_GivenQuantityPrice] [decimal](16, 2) NULL,
	[FK_AppUser_Client] [uniqueidentifier] NOT NULL,
	[FK_AppUser_FuelGivenBy] [uniqueidentifier] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[FuelGivenAt] [datetime] NULL,
	[OracleDB_IsPushed] [bit] NULL,
	[OracleDB_PushedAt] [datetime] NULL,
	[OracleDB_IsPulled] [bit] NULL,
	[OracleDB_PulledAt] [datetime] NULL,
	[RequisitionTrip_TrackingID] [varchar](20) NULL,
	[Taker_Staff_ID] [varchar](50) NULL,
	[Taker_Staff_Name] [varchar](300) NULL,
	[FK_AppUser_ExcessMoneyGivenBy] [uniqueidentifier] NULL,
	[ExcessMoneyGivenAt] [datetime] NULL,
	[FK_Requisition] [bigint] NULL,
 CONSTRAINT [PK_FuelConsumptionDemand] PRIMARY KEY CLUSTERED 
(
	[PK_FuelConsumptionDemand] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FuelPump]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FuelPump](
	[PK_FuelPump] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FuelPump] PRIMARY KEY CLUSTERED 
(
	[PK_FuelPump] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GPS_DeviceChangeLog]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GPS_DeviceExisting]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__GPS_Devi__679F14B1562FDC1D] UNIQUE NONCLUSTERED 
(
	[GpsIMEINumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupOfCompany]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Helper]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HiredVehicleDriver]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IndividualRequisition]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IndividualRequisitionBidding]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstantRequisition]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InterCompanyRequisition]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InterCompanyRequisition_ExternalTroller]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InterCompanyRequisition_ExternalVehicle]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InterCompanyRequisition_InternalVehicle]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InterCompanyRequisitionBidding]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InterCompanyRequisitionLocation]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KPLChangeLog]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoadingBayUtilization]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoadingBayUtilization](
	[PK_LoadingBayUtilization] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_LoadingBay] [bigint] NOT NULL,
	[FK_Vehicle] [uniqueidentifier] NULL,
	[UseType] [varchar](10) NOT NULL,
	[FK_ParkingInOut] [bigint] NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NULL,
	[FK_AppUser_StartedBy] [uniqueidentifier] NOT NULL,
	[FK_AppUser_EndedBy] [uniqueidentifier] NULL,
	[StayTimeMinute] [bigint] NULL,
	[OWN_MHT_DHT] [varchar](10) NULL,
 CONSTRAINT [PK_LoadingBayUtilization] PRIMARY KEY CLUSTERED 
(
	[PK_LoadingBayUtilization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocationToLocationMapping]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocationWiseGP]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MapLocation]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MobileMenu]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MobileRole]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MobileRole_MobileMenu]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonthlyBillEntry]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PoliceCase]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PoliceCase_PoliceCaseLaw]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PoliceCaseDocument]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PoliceCaseLaw]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PranOrganization]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PranOrganization](
	[OrganizationCode] [varchar](10) NULL,
	[OrganizationName] [varchar](100) NULL,
	[LocationName] [varchar](100) NULL,
	[FK_Location] [uniqueidentifier] NULL,
	[PK_PranOrganization] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [pk_table_1] PRIMARY KEY CLUSTERED 
(
	[PK_PranOrganization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_PranOrganization_FK_Location_WITH_OrganizationCode] UNIQUE NONCLUSTERED 
(
	[FK_Location] ASC,
	[OrganizationCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_PranOrganization_FK_Location_WITH_OrganizationName] UNIQUE NONCLUSTERED 
(
	[FK_Location] ASC,
	[OrganizationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PRG_Type]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReadyReport]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReceivingRequest]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReceivingRequest](
	[PK_ReceivingRequest] [bigint] IDENTITY(1,1) NOT NULL,
	[TrackingID] [varchar](50) NULL,
	[FK_AppUser_Client] [uniqueidentifier] NOT NULL,
	[FK_LocationDepartment] [bigint] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[AssumedGateReceivingDateTime] [datetime] NOT NULL,
	[CarrerType] [varchar](50) NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[Note_Creation] [varchar](100) NULL,
	[FK_AppUser_ReceivingGate] [uniqueidentifier] NULL,
	[GateReceivingDateTime] [datetime] NULL,
	[FK_AppUser_FinalReceiver] [uniqueidentifier] NULL,
	[FinalReceivingDateTime] [datetime] NULL,
	[Item1_Name] [varchar](100) NULL,
	[Item1_Note_Creation] [varchar](100) NULL,
	[Item1_GateReceivingDoccumentNumber] [varchar](100) NULL,
	[Item1_Note_FinalReceival] [varchar](100) NULL,
	[Item1_FinalReceival_StaffId] [varchar](100) NULL,
	[Item1_FinalReceival_StaffName] [varchar](100) NULL,
	[Item1_Status] [varchar](50) NULL,
	[Item2_Name] [varchar](100) NULL,
	[Item2_Note_Creation] [varchar](100) NULL,
	[Item2_GateReceivingDoccumentNumber] [varchar](100) NULL,
	[Item2_Note_FinalReceival] [varchar](100) NULL,
	[Item2_FinalReceival_StaffId] [varchar](100) NULL,
	[Item2_FinalReceival_StaffName] [varchar](100) NULL,
	[Item2_Status] [varchar](50) NULL,
	[Item3_Name] [varchar](100) NULL,
	[Item3_Note_Creation] [varchar](100) NULL,
	[Item3_GateReceivingDoccumentNumber] [varchar](100) NULL,
	[Item3_Note_FinalReceival] [varchar](100) NULL,
	[Item3_FinalReceival_StaffId] [varchar](100) NULL,
	[Item3_FinalReceival_StaffName] [varchar](100) NULL,
	[Item3_Status] [varchar](50) NULL,
	[Item4_Name] [varchar](100) NULL,
	[Item4_Note_Creation] [varchar](100) NULL,
	[Item4_GateReceivingDoccumentNumber] [varchar](100) NULL,
	[Item4_Note_FinalReceival] [varchar](100) NULL,
	[Item4_FinalReceival_StaffId] [varchar](100) NULL,
	[Item4_FinalReceival_StaffName] [varchar](100) NULL,
	[Item4_Status] [varchar](50) NULL,
	[Item5_Name] [varchar](100) NULL,
	[Item5_Note_Creation] [varchar](100) NULL,
	[Item5_GateReceivingDoccumentNumber] [varchar](100) NULL,
	[Item5_Note_FinalReceival] [varchar](100) NULL,
	[Item5_FinalReceival_StaffId] [varchar](100) NULL,
	[Item5_FinalReceival_StaffName] [varchar](100) NULL,
	[Item5_Status] [varchar](50) NULL,
 CONSTRAINT [PK_ReceivingRequest] PRIMARY KEY CLUSTERED 
(
	[PK_ReceivingRequest] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_ConsolidatedRport]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_TemperatureReport]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_TemperatureReport_Helper]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleConsolidatedReport]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleHaltReadyReport_Helper]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleHaltReadyReport_Helper_Helper]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleHaltReport]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleHaltReport_Helper]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleHistory]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleHistoryDetail]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleInOutHistoryDetail]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleOutOverStay]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleRunAndHaltReport]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleRunAndHaltReport](
	[PK] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [varchar](50) NULL,
	[FK_Vehicle] [varchar](50) NULL,
	[_rowType] [varchar](50) NULL,
	[PK_RowData_Start] [varchar](50) NULL,
	[PK_RowData_End] [varchar](50) NULL,
	[VehicleRegistrationNumber] [varchar](50) NULL,
	[StartTime] [varchar](50) NULL,
	[EndTime] [varchar](50) NULL,
	[Latitude] [varchar](50) NULL,
	[Longitude] [varchar](50) NULL,
	[LapTime] [int] NULL,
	[EngineStatus] [varchar](50) NULL,
	[Speed] [varchar](50) NULL,
	[_DateString] [varchar](50) NULL,
	[_NumberOfRun] [varchar](50) NULL,
 CONSTRAINT [PK_Report_VehicleRunReport] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report_VehicleRunAndHaltReport_Helper]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report_VehicleRunAndHaltReport_Helper](
	[PK_RowData] [bigint] IDENTITY(1,1) NOT NULL,
	[USER_KEY] [varchar](50) NOT NULL,
	[FK_Vehicle] [varchar](50) NOT NULL,
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
 CONSTRAINT [PK_Report_VehicleRunReport_Helper] PRIMARY KEY CLUSTERED 
(
	[PK_RowData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitionAgentNotification]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitionAgentProposedDepo]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitionBusinessUnit]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitionProductType]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitionTrip]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionTrip](
	[PK_RequisitionTrip] [bigint] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[TrackingID] [varchar](20) NULL,
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
	[OracleDB_GPNumberUpdatedAt] [datetime] NULL,
	[FK_ParkingInOut_Before] [bigint] NULL,
	[FK_ParkingInOut_After] [bigint] NULL,
	[ManualParkingEntryTime] [datetime] NULL,
	[AssigningNote] [varchar](300) NULL,
 CONSTRAINT [PK_RequisitionTrip] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionTrip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitionTrip_Finished]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitionTrip_Finished](
	[PK_RequisitionTrip_Finished] [bigint] NOT NULL,
	[IsDeleted] [bit] NULL,
	[TrackingID] [varchar](20) NULL,
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
	[OracleDB_GPNumberUpdatedAt] [datetime] NULL,
	[FK_ParkingInOut_Before] [bigint] NULL,
	[FK_ParkingInOut_After] [bigint] NULL,
	[ManualParkiingEntryTime] [datetime] NULL,
	[AssigningNote] [varchar](300) NULL,
 CONSTRAINT [PK_RequisitionTrip_Finished] PRIMARY KEY CLUSTERED 
(
	[PK_RequisitionTrip_Finished] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitionVehicleType]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFID_AutoDistSuggession]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFID_AutoLetterSuggession]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFID_Entry]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RFID_EntryLog]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RouteChart]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceCall]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempMail]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemporaryVehicle]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Test]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Test](
	[va1] [bigint] NULL,
	[val2] [varchar](50) NULL,
	[Internal_KPL_Loaded_8_To_12_Tons_out] [float] NULL,
	[Internal_KPL_Loaded_12_To_25_Tons_out] [float] NULL,
	[val3] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestRfidData]    Script Date: 2022-02-24 11:24:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestRfidData](
	[PK_TestRfidData] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NULL,
	[RegistrationNumber] [varchar](100) NULL,
	[InOrOut] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[DevelopersNote] [varchar](300) NULL,
 CONSTRAINT [PK_TestRfidData] PRIMARY KEY CLUSTERED 
(
	[PK_TestRfidData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransportCompany]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripExpense]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Upazila]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDesignation]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleBrand]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleInOutManualReason]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleInOutManualTypesOfProduct]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleModel]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleSharing]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleSharingAgentMapping]    Script Date: 2022-02-24 11:24:49 AM ******/
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
/****** Object:  Table [dbo].[VehicleSharingBidding]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleSharingDemand]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleSharingExternalTrip]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleSharingInternalTrip]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleSharingInternalTripAdjustment]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleTracking]    Script Date: 2022-02-24 11:24:49 AM ******/
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
	[EngineStatus_ChangedAt] [datetime] NULL,
	[Location_ChangedAt] [datetime] NULL,
 CONSTRAINT [PK_VehicleTracking] PRIMARY KEY CLUSTERED 
(
	[PK_Vehicle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleTrackingInformation]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleTrip]    Script Date: 2022-02-24 11:24:49 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
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
ALTER TABLE [dbo].[AppUser_AppMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppMenu_AppMenu] FOREIGN KEY([FK_AppMenu])
REFERENCES [dbo].[AppMenu] ([PK_AppMenu])
GO
ALTER TABLE [dbo].[AppUser_AppMenu] CHECK CONSTRAINT [FK_AppUser_AppMenu_AppMenu]
GO
ALTER TABLE [dbo].[AppUser_AppMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppMenu_AppUser] FOREIGN KEY([FK_AppUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUser_AppMenu] CHECK CONSTRAINT [FK_AppUser_AppMenu_AppUser]
GO
ALTER TABLE [dbo].[AppUser_AppPermission]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppPermission_AppPermission] FOREIGN KEY([FK_AppPermission])
REFERENCES [dbo].[AppPermission] ([PK_AppPermission])
GO
ALTER TABLE [dbo].[AppUser_AppPermission] CHECK CONSTRAINT [FK_AppUser_AppPermission_AppPermission]
GO
ALTER TABLE [dbo].[AppUser_AppPermission]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppPermission_AppUser] FOREIGN KEY([FK_AppUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUser_AppPermission] CHECK CONSTRAINT [FK_AppUser_AppPermission_AppUser]
GO
ALTER TABLE [dbo].[AppUser_AppSubMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppSubMenu_AppSubMenu] FOREIGN KEY([FK_AppSubMenu])
REFERENCES [dbo].[AppSubMenu] ([PK_AppSubMenu])
GO
ALTER TABLE [dbo].[AppUser_AppSubMenu] CHECK CONSTRAINT [FK_AppUser_AppSubMenu_AppSubMenu]
GO
ALTER TABLE [dbo].[AppUser_AppSubMenu]  WITH CHECK ADD  CONSTRAINT [FK_AppUser_AppSubMenu_AppUser] FOREIGN KEY([FK_AppUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[AppUser_AppSubMenu] CHECK CONSTRAINT [FK_AppUser_AppSubMenu_AppUser]
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
ALTER TABLE [dbo].[FuelConsumptionDemand]  WITH CHECK ADD  CONSTRAINT [FK_FuelConsumptionDemand_FuelPump] FOREIGN KEY([FK_FuelPump])
REFERENCES [dbo].[FuelPump] ([PK_FuelPump])
GO
ALTER TABLE [dbo].[FuelConsumptionDemand] CHECK CONSTRAINT [FK_FuelConsumptionDemand_FuelPump]
GO
ALTER TABLE [dbo].[FuelConsumptionDemand]  WITH CHECK ADD  CONSTRAINT [FK_FuelConsumptionDemand_Location] FOREIGN KEY([FK_LocationFrom])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[FuelConsumptionDemand] CHECK CONSTRAINT [FK_FuelConsumptionDemand_Location]
GO
ALTER TABLE [dbo].[FuelConsumptionDemand]  WITH CHECK ADD  CONSTRAINT [FK_FuelConsumptionDemand_Location1] FOREIGN KEY([FK_LocationTo])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[FuelConsumptionDemand] CHECK CONSTRAINT [FK_FuelConsumptionDemand_Location1]
GO
ALTER TABLE [dbo].[FuelConsumptionDemand]  WITH CHECK ADD  CONSTRAINT [FK_FuelConsumptionDemand_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[FuelConsumptionDemand] CHECK CONSTRAINT [FK_FuelConsumptionDemand_Vehicle]
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
ALTER TABLE [dbo].[LoadingBay]  WITH CHECK ADD  CONSTRAINT [FK_LoadingBay_LoadingBayUtilization] FOREIGN KEY([FK_LoadingBayUtilization_Last])
REFERENCES [dbo].[LoadingBayUtilization] ([PK_LoadingBayUtilization])
GO
ALTER TABLE [dbo].[LoadingBay] CHECK CONSTRAINT [FK_LoadingBay_LoadingBayUtilization]
GO
ALTER TABLE [dbo].[LoadingBay]  WITH CHECK ADD  CONSTRAINT [FK_LoadingBay_LocationBuilding] FOREIGN KEY([FK_LocationBuilding])
REFERENCES [dbo].[LocationBuilding] ([PK_LocationBuilding])
GO
ALTER TABLE [dbo].[LoadingBay] CHECK CONSTRAINT [FK_LoadingBay_LocationBuilding]
GO
ALTER TABLE [dbo].[LoadingBayUtilization]  WITH CHECK ADD  CONSTRAINT [FK_LoadingBayUtilization_AppUser] FOREIGN KEY([FK_AppUser_StartedBy])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[LoadingBayUtilization] CHECK CONSTRAINT [FK_LoadingBayUtilization_AppUser]
GO
ALTER TABLE [dbo].[LoadingBayUtilization]  WITH CHECK ADD  CONSTRAINT [FK_LoadingBayUtilization_LoadingBay] FOREIGN KEY([FK_LoadingBay])
REFERENCES [dbo].[LoadingBay] ([PK_LoadingBay])
GO
ALTER TABLE [dbo].[LoadingBayUtilization] CHECK CONSTRAINT [FK_LoadingBayUtilization_LoadingBay]
GO
ALTER TABLE [dbo].[LoadingBayUtilization]  WITH CHECK ADD  CONSTRAINT [FK_LoadingBayUtilization_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[LoadingBayUtilization] CHECK CONSTRAINT [FK_LoadingBayUtilization_Vehicle]
GO
ALTER TABLE [dbo].[LocationBuilding]  WITH CHECK ADD  CONSTRAINT [FK_LocationBuilding_Location] FOREIGN KEY([FK_Location])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[LocationBuilding] CHECK CONSTRAINT [FK_LocationBuilding_Location]
GO
ALTER TABLE [dbo].[LocationDepartment]  WITH CHECK ADD  CONSTRAINT [FK_LocationDepartment_Location] FOREIGN KEY([FK_Location])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[LocationDepartment] CHECK CONSTRAINT [FK_LocationDepartment_Location]
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
ALTER TABLE [dbo].[ParkingInOut]  WITH CHECK ADD  CONSTRAINT [FK_ParkingInOut_AppUser] FOREIGN KEY([In_FK_CreatedByUser])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[ParkingInOut] CHECK CONSTRAINT [FK_ParkingInOut_AppUser]
GO
ALTER TABLE [dbo].[ParkingInOut]  WITH CHECK ADD  CONSTRAINT [FK_ParkingInOut_LoadingBay] FOREIGN KEY([BayAssign_FK_LoadingBay])
REFERENCES [dbo].[LoadingBay] ([PK_LoadingBay])
GO
ALTER TABLE [dbo].[ParkingInOut] CHECK CONSTRAINT [FK_ParkingInOut_LoadingBay]
GO
ALTER TABLE [dbo].[ParkingInOut]  WITH CHECK ADD  CONSTRAINT [FK_ParkingInOut_Location] FOREIGN KEY([FK_Location])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[ParkingInOut] CHECK CONSTRAINT [FK_ParkingInOut_Location]
GO
ALTER TABLE [dbo].[ParkingInOut]  WITH CHECK ADD  CONSTRAINT [FK_ParkingInOut_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[ParkingInOut] CHECK CONSTRAINT [FK_ParkingInOut_Vehicle]
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
ALTER TABLE [dbo].[PranOrganization]  WITH CHECK ADD  CONSTRAINT [FK_PranOrganization_Location] FOREIGN KEY([FK_Location])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[PranOrganization] CHECK CONSTRAINT [FK_PranOrganization_Location]
GO
ALTER TABLE [dbo].[ReceivingRequest]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingRequest_Client] FOREIGN KEY([FK_AppUser_Client])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[ReceivingRequest] CHECK CONSTRAINT [FK_ReceivingRequest_Client]
GO
ALTER TABLE [dbo].[ReceivingRequest]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingRequest_FinalReceiver] FOREIGN KEY([FK_AppUser_FinalReceiver])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[ReceivingRequest] CHECK CONSTRAINT [FK_ReceivingRequest_FinalReceiver]
GO
ALTER TABLE [dbo].[ReceivingRequest]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingRequest_LocationDepartment] FOREIGN KEY([FK_LocationDepartment])
REFERENCES [dbo].[LocationDepartment] ([PK_LocationDepartment])
GO
ALTER TABLE [dbo].[ReceivingRequest] CHECK CONSTRAINT [FK_ReceivingRequest_LocationDepartment]
GO
ALTER TABLE [dbo].[ReceivingRequest]  WITH CHECK ADD  CONSTRAINT [FK_ReceivingRequest_ReceivingGate] FOREIGN KEY([FK_AppUser_ReceivingGate])
REFERENCES [dbo].[AppUser] ([PK_User])
GO
ALTER TABLE [dbo].[ReceivingRequest] CHECK CONSTRAINT [FK_ReceivingRequest_ReceivingGate]
GO
ALTER TABLE [dbo].[Report_VehicleOutOverStay]  WITH CHECK ADD  CONSTRAINT [FK_Report_VehicleOutOverStay_Vehicle] FOREIGN KEY([FK_Vehicle])
REFERENCES [dbo].[Vehicle] ([PK_Vehicle])
GO
ALTER TABLE [dbo].[Report_VehicleOutOverStay] CHECK CONSTRAINT [FK_Report_VehicleOutOverStay_Vehicle]
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
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_Requisition_LocationDepartment_From] FOREIGN KEY([FK_LocationDepartment_From])
REFERENCES [dbo].[LocationDepartment] ([PK_LocationDepartment])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_Requisition_LocationDepartment_From]
GO
ALTER TABLE [dbo].[Requisition]  WITH CHECK ADD  CONSTRAINT [FK_Requisition_LocationDepartment_To] FOREIGN KEY([FK_LocationDepartment_To])
REFERENCES [dbo].[LocationDepartment] ([PK_LocationDepartment])
GO
ALTER TABLE [dbo].[Requisition] CHECK CONSTRAINT [FK_Requisition_LocationDepartment_To]
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
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_FinancingCompany] FOREIGN KEY([Internal_FK_FinancingCompany])
REFERENCES [dbo].[FinancingCompany] ([PK_FinancingCompany])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_FinancingCompany]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_LoadingBayUtilization] FOREIGN KEY([FK_LoadingBayUtilization_Last])
REFERENCES [dbo].[LoadingBayUtilization] ([PK_LoadingBayUtilization])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_LoadingBayUtilization]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Location] FOREIGN KEY([FK_LocationInOut])
REFERENCES [dbo].[Location] ([PK_Location])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_Location]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_ParkingInOut] FOREIGN KEY([FK_ParkingInOut_Last])
REFERENCES [dbo].[ParkingInOut] ([PK_ParkingInOut])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_ParkingInOut]
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
