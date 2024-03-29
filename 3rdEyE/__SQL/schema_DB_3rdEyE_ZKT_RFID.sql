USE [master]
GO
/****** Object:  Database [DB_3rdEyE_ZKT_RFID]    Script Date: 2023-11-05 9:48:44 AM ******/
CREATE DATABASE [DB_3rdEyE_ZKT_RFID]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB_3rdEyE_ZKT_RFID', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.RIPSMS06\MSSQL\DATA\DB_3rdEyE_ZKT_RFID.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DB_3rdEyE_ZKT_RFID_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.RIPSMS06\MSSQL\DATA\DB_3rdEyE_ZKT_RFID_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_3rdEyE_ZKT_RFID].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET RECOVERY FULL 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET  MULTI_USER 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DB_3rdEyE_ZKT_RFID', N'ON'
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET QUERY_STORE = OFF
GO
USE [DB_3rdEyE_ZKT_RFID]
GO
/****** Object:  Table [dbo].[acc_antiback]    Script Date: 2023-11-05 9:48:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_antiback](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [smallint] NOT NULL,
	[device_id] [int] NULL,
	[one_mode] [bit] NOT NULL,
	[two_mode] [bit] NOT NULL,
	[three_mode] [bit] NOT NULL,
	[four_mode] [bit] NOT NULL,
	[five_mode] [bit] NOT NULL,
	[six_mode] [bit] NOT NULL,
	[seven_mode] [bit] NOT NULL,
	[eight_mode] [bit] NOT NULL,
	[nine_mode] [bit] NOT NULL,
	[AntibackType] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_auxiliary]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_auxiliary](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[device_id] [int] NULL,
	[aux_no] [int] NULL,
	[printer_number] [nvarchar](50) NULL,
	[aux_name] [nvarchar](50) NULL,
	[aux_state] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_door]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_door](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[device_id] [int] NULL,
	[door_no] [smallint] NULL,
	[door_name] [nvarchar](30) NULL,
	[lock_delay] [int] NULL,
	[back_lock] [bit] NOT NULL,
	[sensor_delay] [int] NULL,
	[opendoor_type] [int] NULL,
	[inout_state] [int] NULL,
	[lock_active_id] [int] NULL,
	[long_open_id] [int] NULL,
	[wiegand_fmt_id] [int] NULL,
	[card_intervaltime] [int] NULL,
	[reader_type] [int] NULL,
	[is_att] [bit] NOT NULL,
	[force_pwd] [nvarchar](100) NULL,
	[supper_pwd] [nvarchar](100) NULL,
	[door_sensor_status] [int] NULL,
	[map_id] [int] NULL,
	[duration_apb] [int] NULL,
	[reader_io_state] [int] NULL,
	[open_door_delay] [int] NULL,
	[door_normal_open] [bit] NULL,
	[enable_normal_open] [bit] NULL,
	[disenable_normal_open] [bit] NULL,
	[wiegandInType] [smallint] NULL,
	[wiegandOutType] [smallint] NULL,
	[wiegand_fmt_out_id] [smallint] NULL,
	[SRBOn] [smallint] NULL,
	[ManualCtlMode] [int] NULL,
	[ErrTimes] [int] NULL,
	[SensorAlarmTime] [int] NULL,
	[InTimeAPB] [int] NULL,
	[DoorInTimeAPB] [int] NULL,
	[DoorMultiCardInterTime] [int] NULL,
	[DoorFirstCardOpenDoor] [int] NULL,
	[DoorMultiCardOpenDoor] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_firstopen]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_firstopen](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[door_id] [int] NULL,
	[timeseg_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_firstopen_emp]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_firstopen_emp](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[accfirstopen_id] [int] NULL,
	[employee_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[accfirstopen_id] ASC,
	[employee_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_holidays]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_holidays](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[holiday_name] [nvarchar](30) NULL,
	[holiday_type] [int] NULL,
	[start_date] [datetime] NOT NULL,
	[end_date] [datetime] NOT NULL,
	[loop_by_year] [int] NULL,
	[memo] [nvarchar](70) NULL,
	[HolidayNo] [int] NULL,
	[HolidayTZ] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[holiday_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_interlock]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_interlock](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [int] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[device_id] [int] NULL,
	[one_mode] [bit] NOT NULL,
	[two_mode] [bit] NOT NULL,
	[three_mode] [bit] NOT NULL,
	[four_mode] [bit] NOT NULL,
	[InterlockType] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[device_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_levelset]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_levelset](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](30) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](30) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](30) NULL,
	[delete_time] [datetime] NULL,
	[status] [smallint] NOT NULL,
	[level_name] [nvarchar](30) NULL,
	[level_timeseg_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[level_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_levelset_door_group]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_levelset_door_group](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[acclevelset_id] [int] NOT NULL,
	[accdoor_id] [int] NOT NULL,
	[accdoor_no_exp] [int] NULL,
	[accdoor_device_id] [int] NULL,
	[level_timeseg_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[acclevelset_id] ASC,
	[accdoor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_levelset_emp]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_levelset_emp](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[acclevelset_id] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[acclevelset_id] ASC,
	[employee_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_linkageio]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_linkageio](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [nvarchar](50) NULL,
	[status] [int] NULL,
	[linkage_name] [nvarchar](50) NULL,
	[device_id] [int] NULL,
	[trigger_opt] [int] NULL,
	[in_address_hide] [int] NULL,
	[in_address] [int] NULL,
	[out_type_hide] [int] NULL,
	[out_address_hide] [int] NULL,
	[out_address] [int] NULL,
	[action_type] [int] NULL,
	[delay_time] [int] NULL,
	[video_linkageio_id] [int] NULL,
	[lchannel_num] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[linkage_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_map]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_map](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[map_name] [nvarchar](30) NULL,
	[map_path] [nvarchar](max) NULL,
	[area_id] [int] NULL,
	[width] [int] NULL,
	[height] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[map_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_mapdoorpos]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_mapdoorpos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[map_door_id] [int] NULL,
	[map_id] [int] NULL,
	[width] [int] NULL,
	[left] [int] NULL,
	[top] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_monitor_log]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_monitor_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[log_tag] [int] NULL,
	[time] [datetime] NULL,
	[pin] [nvarchar](50) NULL,
	[card_no] [nvarchar](50) NULL,
	[device_id] [int] NULL,
	[device_sn] [nvarchar](50) NULL,
	[device_name] [nvarchar](50) NULL,
	[verified] [int] NULL,
	[state] [int] NULL,
	[event_type] [int] NULL,
	[description] [nvarchar](200) NULL,
	[event_point_type] [int] NULL,
	[event_point_id] [int] NULL,
	[event_point_name] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [time_pin] UNIQUE NONCLUSTERED 
(
	[time] ASC,
	[pin] ASC,
	[card_no] ASC,
	[device_id] ASC,
	[verified] ASC,
	[state] ASC,
	[event_type] ASC,
	[description] ASC,
	[event_point_type] ASC,
	[event_point_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_morecardempgroup]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_morecardempgroup](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[group_name] [nvarchar](50) NULL,
	[memo] [nvarchar](50) NULL,
	[StdGroupNo] [int] NULL,
	[StdGroupTz] [int] NULL,
	[StdGroupVT] [int] NULL,
	[StdValidOnHoliday] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[group_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_morecardgroup]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_morecardgroup](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[comb_id] [int] NULL,
	[group_id] [int] NULL,
	[opener_number] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_morecardset]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_morecardset](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[door_id] [int] NULL,
	[comb_name] [nvarchar](50) NULL,
	[CombNo] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[comb_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_reader]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_reader](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[door_id] [int] NULL,
	[reader_no] [int] NULL,
	[reader_name] [nvarchar](50) NULL,
	[reader_state] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_timeseg]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_timeseg](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[timeseg_name] [nvarchar](50) NULL,
	[memo] [nvarchar](70) NULL,
	[sunday_start1] [datetime] NOT NULL,
	[sunday_end1] [datetime] NULL,
	[sunday_start2] [datetime] NULL,
	[sunday_end2] [datetime] NULL,
	[sunday_start3] [datetime] NULL,
	[sunday_end3] [datetime] NULL,
	[monday_start1] [datetime] NULL,
	[monday_end1] [datetime] NULL,
	[monday_start2] [datetime] NULL,
	[monday_end2] [datetime] NULL,
	[monday_start3] [datetime] NULL,
	[monday_end3] [datetime] NULL,
	[tuesday_start1] [datetime] NULL,
	[tuesday_end1] [datetime] NULL,
	[tuesday_start2] [datetime] NULL,
	[tuesday_end2] [datetime] NULL,
	[tuesday_start3] [datetime] NULL,
	[tuesday_end3] [datetime] NULL,
	[wednesday_start1] [datetime] NULL,
	[wednesday_end1] [datetime] NULL,
	[wednesday_start2] [datetime] NULL,
	[wednesday_end2] [datetime] NULL,
	[wednesday_start3] [datetime] NULL,
	[wednesday_end3] [datetime] NULL,
	[thursday_start1] [datetime] NULL,
	[thursday_end1] [datetime] NULL,
	[thursday_start2] [datetime] NULL,
	[thursday_end2] [datetime] NULL,
	[thursday_start3] [datetime] NULL,
	[thursday_end3] [datetime] NULL,
	[friday_start1] [datetime] NULL,
	[friday_end1] [datetime] NULL,
	[friday_start2] [datetime] NULL,
	[friday_end2] [datetime] NULL,
	[friday_start3] [datetime] NULL,
	[friday_end3] [datetime] NULL,
	[saturday_start1] [datetime] NULL,
	[saturday_end1] [datetime] NULL,
	[saturday_start2] [datetime] NULL,
	[saturday_end2] [datetime] NULL,
	[saturday_start3] [datetime] NULL,
	[saturday_end3] [datetime] NULL,
	[holidaytype1_start1] [datetime] NULL,
	[holidaytype1_end1] [datetime] NULL,
	[holidaytype1_start2] [datetime] NULL,
	[holidaytype1_end2] [datetime] NULL,
	[holidaytype1_start3] [datetime] NULL,
	[holidaytype1_end3] [datetime] NULL,
	[holidaytype2_start1] [datetime] NULL,
	[holidaytype2_end1] [datetime] NULL,
	[holidaytype2_start2] [datetime] NULL,
	[holidaytype2_end2] [datetime] NULL,
	[holidaytype2_start3] [datetime] NULL,
	[holidaytype2_end3] [datetime] NULL,
	[holidaytype3_start1] [datetime] NULL,
	[holidaytype3_end1] [datetime] NULL,
	[holidaytype3_start2] [datetime] NULL,
	[holidaytype3_end2] [datetime] NULL,
	[holidaytype3_start3] [datetime] NULL,
	[holidaytype3_end3] [datetime] NULL,
	[TimeZone1Id] [int] NULL,
	[TimeZone2Id] [int] NULL,
	[TimeZone3Id] [int] NULL,
	[TimeZoneHolidayId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[timeseg_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[acc_wiegandfmt]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[acc_wiegandfmt](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NULL,
	[wiegand_name] [nvarchar](30) NOT NULL,
	[wiegand_count] [int] NULL,
	[odd_start] [int] NULL,
	[odd_count] [int] NULL,
	[even_start] [int] NULL,
	[even_count] [int] NULL,
	[cid_start] [int] NULL,
	[cid_count] [int] NULL,
	[comp_start] [int] NULL,
	[comp_count] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[action_log]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[action_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[action_time] [datetime] NULL,
	[user_id] [int] NULL,
	[content_type_id] [int] NULL,
	[object_id] [int] NULL,
	[object_repr] [nvarchar](50) NULL,
	[action_flag] [int] NULL,
	[change_message] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[areaadmin]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[areaadmin](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[area_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttParam]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttParam](
	[PARANAME] [nvarchar](20) NOT NULL,
	[PARATYPE] [nvarchar](2) NULL,
	[PARAVALUE] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PARANAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[auth_group]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[auth_group](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[Permission] [nvarchar](max) NULL,
	[Remark] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[auth_user]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[auth_user](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NULL,
	[password] [nvarchar](50) NULL,
	[Status] [int] NULL,
	[last_login] [datetime] NULL,
	[RoleID] [int] NULL,
	[Remark] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BioTemplate]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BioTemplate](
	[UserID] [int] NOT NULL,
	[BadgeNumber] [nvarchar](24) NOT NULL,
	[CreateOperator] [nvarchar](30) NULL,
	[CreateTime] [datetime] NULL,
	[ValidFlag] [nvarchar](1) NOT NULL,
	[IsDuress] [nvarchar](1) NOT NULL,
	[BioType] [int] NOT NULL,
	[Version] [nvarchar](20) NOT NULL,
	[DataFormat] [int] NULL,
	[TemplateNO] [int] NOT NULL,
	[TemplateNOIndex] [int] NOT NULL,
	[nOldType] [int] NULL,
	[TemplateData] [nvarchar](3000) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_BioTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_BioTemplate] UNIQUE NONCLUSTERED 
(
	[BadgeNumber] ASC,
	[TemplateNOIndex] ASC,
	[TemplateNO] ASC,
	[BioType] ASC,
	[Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CHECKINOUT]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CHECKINOUT](
	[USERID] [int] NOT NULL,
	[CHECKTIME] [datetime] NOT NULL,
	[CHECKTYPE] [nvarchar](1) NULL,
	[VERIFYCODE] [int] NULL,
	[SENSORID] [nvarchar](5) NULL,
	[LOGID] [int] IDENTITY(1,1) NOT NULL,
	[MachineId] [int] NULL,
	[UserExtFmt] [int] NULL,
	[WorkCode] [int] NULL,
	[Memoinfo] [nvarchar](20) NULL,
	[sn] [nvarchar](20) NULL,
 CONSTRAINT [USERCHECKTIME1] PRIMARY KEY CLUSTERED 
(
	[USERID] ASC,
	[CHECKTIME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Custom_acc_monitor_log]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Custom_acc_monitor_log](
	[id] [int] NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[log_tag] [int] NULL,
	[time] [datetime] NULL,
	[pin] [nvarchar](50) NULL,
	[card_no] [nvarchar](50) NULL,
	[device_id] [int] NULL,
	[device_sn] [nvarchar](50) NULL,
	[device_name] [nvarchar](50) NULL,
	[verified] [int] NULL,
	[state] [int] NULL,
	[event_type] [int] NULL,
	[description] [nvarchar](200) NULL,
	[event_point_type] [int] NULL,
	[event_point_id] [int] NULL,
	[event_point_name] [nvarchar](200) NULL,
 CONSTRAINT [PK_Custom_acc_monitor_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomReport]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomReport](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ReportName] [nvarchar](50) NULL,
	[Memo] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DEPARTMENTS]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DEPARTMENTS](
	[DEPTID] [int] IDENTITY(1,1) NOT NULL,
	[DEPTNAME] [nvarchar](30) NULL,
	[SUPDEPTID] [int] NOT NULL,
	[InheritParentSch] [smallint] NULL,
	[InheritDeptSch] [smallint] NULL,
	[InheritDeptSchClass] [smallint] NULL,
	[AutoSchPlan] [smallint] NULL,
	[InLate] [smallint] NULL,
	[OutEarly] [smallint] NULL,
	[InheritDeptRule] [smallint] NULL,
	[MinAutoSchInterval] [int] NULL,
	[RegisterOT] [smallint] NULL,
	[DefaultSchId] [int] NOT NULL,
	[ATT] [smallint] NULL,
	[Holiday] [smallint] NULL,
	[OverTime] [smallint] NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NULL,
	[code] [nvarchar](50) NULL,
	[type] [nvarchar](50) NULL,
	[invalidate] [datetime] NULL,
 CONSTRAINT [DEPTID] PRIMARY KEY CLUSTERED 
(
	[DEPTID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[deptadmin]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[deptadmin](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[dept_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[devcmds]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[devcmds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NOT NULL,
	[SN_id] [int] NULL,
	[CmdOperate_id] [int] NULL,
	[CmdContent] [text] NOT NULL,
	[CmdCommitTime] [datetime] NOT NULL,
	[CmdTransTime] [datetime] NULL,
	[CmdOverTime] [datetime] NULL,
	[CmdReturn] [int] NULL,
	[CmdReturnContent] [nvarchar](50) NULL,
	[CmdImmediately] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FaceTemp]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FaceTemp](
	[TEMPLATEID] [int] IDENTITY(1,1) NOT NULL,
	[USERNO] [nvarchar](24) NULL,
	[SIZE] [int] NULL,
	[pin] [int] NULL,
	[FACEID] [int] NULL,
	[VALID] [int] NULL,
	[RESERVE] [int] NULL,
	[ACTIVETIME] [int] NULL,
	[VFCOUNT] [int] NULL,
	[TEMPLATE] [image] NULL,
	[FaceType] [tinyint] NULL,
	[StateMigrationFlag] [nvarchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[TEMPLATEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FaceTempEx]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FaceTempEx](
	[TEMPLATEID] [int] IDENTITY(1,1) NOT NULL,
	[USERNO] [nvarchar](24) NULL,
	[SIZE] [int] NULL,
	[pin] [int] NULL,
	[FACEID] [int] NULL,
	[VALID] [int] NULL,
	[RESERVE] [int] NULL,
	[ACTIVETIME] [int] NULL,
	[VFCOUNT] [int] NULL,
	[TEMPLATE] [image] NULL,
	[FaceType] [tinyint] NULL,
	[StateMigrationFlag] [nvarchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FingerVein]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FingerVein](
	[FVID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[FingerID] [int] NULL,
	[Template] [image] NULL,
	[Size] [int] NULL,
	[DuressFlag] [int] NULL,
	[UserCode] [nvarchar](50) NULL,
	[Fv_ID_Index] [int] NULL,
	[StateMigrationFlag] [nvarchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[FVID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FingerVeinEx]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FingerVeinEx](
	[FVID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[FingerID] [int] NULL,
	[Template] [image] NULL,
	[Size] [int] NULL,
	[DuressFlag] [int] NULL,
	[UserCode] [nvarchar](50) NULL,
	[Fv_ID_Index] [int] NULL,
	[StateMigrationFlag] [nvarchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Machines]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Machines](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MachineAlias] [nvarchar](50) NULL,
	[ConnectType] [int] NULL,
	[IP] [nvarchar](20) NULL,
	[SerialPort] [int] NULL,
	[Port] [int] NULL,
	[Baudrate] [int] NULL,
	[MachineNumber] [int] NOT NULL,
	[IsHost] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[CommPassword] [nvarchar](12) NULL,
	[UILanguage] [smallint] NULL,
	[DateFormat] [smallint] NULL,
	[InOutRecordWarn] [smallint] NULL,
	[Idle] [smallint] NULL,
	[Voice] [smallint] NULL,
	[FirmwareVersion] [nvarchar](50) NULL,
	[ProductType] [nvarchar](20) NULL,
	[LockControl] [smallint] NULL,
	[Purpose] [smallint] NULL,
	[ProduceKind] [int] NULL,
	[sn] [nvarchar](20) NULL,
	[PhotoStamp] [nvarchar](20) NULL,
	[IsIfChangeConfigServer2] [int] NULL,
	[pushver] [int] NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NULL,
	[device_type] [int] NULL,
	[last_activity] [datetime] NULL,
	[trans_times] [nvarchar](50) NULL,
	[TransInterval] [int] NULL,
	[log_stamp] [nvarchar](50) NULL,
	[oplog_stamp] [image] NULL,
	[photo_stamp] [image] NULL,
	[UpdateDB] [nvarchar](50) NULL,
	[device_name] [nvarchar](50) NULL,
	[transaction_count] [int] NULL,
	[main_time] [nvarchar](50) NULL,
	[max_user_count] [int] NULL,
	[max_finger_count] [int] NULL,
	[max_attlog_count] [int] NULL,
	[alg_ver] [nvarchar](50) NULL,
	[flash_size] [nvarchar](50) NULL,
	[free_flash_size] [nvarchar](50) NULL,
	[language] [nvarchar](50) NULL,
	[lng_encode] [nvarchar](50) NULL,
	[volume] [nvarchar](50) NULL,
	[is_tft] [nvarchar](50) NULL,
	[platform] [nvarchar](50) NULL,
	[brightness] [nvarchar](50) NULL,
	[oem_vendor] [nvarchar](50) NULL,
	[city] [nvarchar](50) NULL,
	[AccFun] [int] NULL,
	[TZAdj] [int] NULL,
	[comm_type] [int] NULL,
	[agent_ipaddress] [nvarchar](50) NULL,
	[subnet_mask] [nvarchar](50) NULL,
	[gateway] [nvarchar](50) NULL,
	[area_id] [int] NULL,
	[acpanel_type] [int] NULL,
	[sync_time] [bit] NOT NULL,
	[four_to_two] [bit] NOT NULL,
	[video_login] [nvarchar](50) NULL,
	[fp_mthreshold] [int] NULL,
	[Fpversion] [int] NULL,
	[max_comm_size] [int] NULL,
	[max_comm_count] [int] NULL,
	[realtime] [bit] NOT NULL,
	[delay] [int] NULL,
	[encrypt] [bit] NOT NULL,
	[dstime_id] [int] NULL,
	[door_count] [int] NULL,
	[reader_count] [int] NULL,
	[aux_in_count] [int] NULL,
	[aux_out_count] [int] NULL,
	[IsOnlyRFMachine] [int] NULL,
	[alias] [nvarchar](50) NULL,
	[ipaddress] [nvarchar](50) NULL,
	[com_port] [smallint] NULL,
	[com_address] [smallint] NULL,
	[SimpleEventType] [int] NULL,
	[FvFunOn] [int] NULL,
	[fvcount] [smallint] NULL,
	[deviceOption] [image] NULL,
	[DevSDKType] [int] NULL,
	[UTableDesc] [image] NULL,
	[IsTFTMachine] [bit] NULL,
	[PinWidth] [int] NULL,
	[UserExtFmt] [int] NULL,
	[FP1_NThreshold] [int] NULL,
	[FP1_1Threshold] [int] NULL,
	[Face1_NThreshold] [int] NULL,
	[Face1_1Threshold] [int] NULL,
	[Only1_1Mode] [int] NULL,
	[OnlyCheckCard] [int] NULL,
	[MifireMustRegistered] [int] NULL,
	[RFCardOn] [int] NULL,
	[Mifire] [int] NULL,
	[MifireId] [int] NULL,
	[NetOn] [int] NULL,
	[RS232On] [int] NULL,
	[RS485On] [int] NULL,
	[FreeType] [int] NULL,
	[FreeTime] [int] NULL,
	[NoDisplayFun] [int] NULL,
	[VoiceTipsOn] [int] NULL,
	[TOMenu] [int] NULL,
	[StdVolume] [int] NULL,
	[VRYVH] [int] NULL,
	[KeyPadBeep] [int] NULL,
	[BatchUpdate] [int] NULL,
	[CardFun] [int] NULL,
	[FaceFunOn] [int] NULL,
	[FaceCount] [int] NULL,
	[TimeAPBFunOn] [int] NULL,
	[FingerFunOn] [nvarchar](5) NULL,
	[CompatOldFirmware] [nvarchar](5) NULL,
	[ParamValues] [image] NULL,
	[WirelessSSID] [nvarchar](50) NULL,
	[WirelessKey] [nvarchar](50) NULL,
	[WirelessAddr] [nvarchar](50) NULL,
	[WirelessMask] [nvarchar](50) NULL,
	[WirelessGateWay] [nvarchar](50) NULL,
	[IsWireless] [bit] NULL,
	[managercount] [int] NULL,
	[usercount] [int] NULL,
	[fingercount] [int] NULL,
	[SecretCount] [int] NULL,
	[ACFun] [int] NULL,
	[BiometricType] [nvarchar](100) NULL,
	[BiometricVersion] [nvarchar](100) NULL,
	[BiometricMaxCount] [nvarchar](100) NULL,
	[BiometricUsedCount] [nvarchar](100) NULL,
	[WIFI] [int] NULL,
	[WIFIOn] [int] NULL,
	[WIFIDHCP] [int] NULL,
	[IsExtend] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[personnel_area]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[personnel_area](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [smallint] NOT NULL,
	[areaid] [nvarchar](50) NULL,
	[areaname] [nvarchar](50) NULL,
	[parent_id] [int] NULL,
	[remark] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[personnel_issuecard]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[personnel_issuecard](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NULL,
	[UserID_id] [int] NULL,
	[cardno] [nvarchar](50) NULL,
	[effectivenessdate] [datetime] NULL,
	[isvalid] [bit] NOT NULL,
	[cardpwd] [nvarchar](50) NULL,
	[failuredate] [datetime] NULL,
	[cardstatus] [nvarchar](50) NULL,
	[issuedate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReaderProperty]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReaderProperty](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[devid] [int] NULL,
	[doorid] [int] NULL,
	[type] [int] NULL,
	[inout] [int] NULL,
	[address] [int] NULL,
	[ipaddress] [nvarchar](100) NULL,
	[port] [int] NULL,
	[mac] [nvarchar](100) NULL,
	[disable] [int] NULL,
	[verifytype] [int] NULL,
	[multicast] [nvarchar](100) NULL,
	[offlinerefuse] [int] NULL,
	[create_operator] [nvarchar](100) NULL,
 CONSTRAINT [PK_ReaderProperty] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportField]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportField](
	[CRId] [int] NOT NULL,
	[TableName] [nvarchar](50) NOT NULL,
	[FieldName] [nvarchar](50) NOT NULL,
	[ShowIndex] [int] NOT NULL,
 CONSTRAINT [PK_ReportField] PRIMARY KEY CLUSTERED 
(
	[CRId] ASC,
	[TableName] ASC,
	[FieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STD_WiegandFmt]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STD_WiegandFmt](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[DoorId] [int] NULL,
	[OutWiegandFmt] [nvarchar](100) NULL,
	[OutFailureId] [int] NULL,
	[OutAreaCode] [int] NULL,
	[OutPulseWidth] [int] NULL,
	[OutPulseInterval] [int] NULL,
	[OutContent] [int] NULL,
	[InWiegandFmt] [nvarchar](100) NULL,
	[InBitCount] [int] NULL,
	[InPulseWidth] [int] NULL,
	[InPulseInterval] [int] NULL,
	[InContent] [int] NULL,
 CONSTRAINT [PK_STD_WiegandFmt] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEMPLATE]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEMPLATE](
	[TEMPLATEID] [int] IDENTITY(1,1) NOT NULL,
	[USERID] [int] NOT NULL,
	[FINGERID] [int] NOT NULL,
	[TEMPLATE] [image] NULL,
	[TEMPLATE2] [image] NULL,
	[BITMAPPICTURE] [image] NULL,
	[BITMAPPICTURE2] [image] NULL,
	[BITMAPPICTURE3] [image] NULL,
	[BITMAPPICTURE4] [image] NULL,
	[USETYPE] [smallint] NULL,
	[EMACHINENUM] [nvarchar](3) NULL,
	[TEMPLATE1] [image] NULL,
	[Flag] [smallint] NULL,
	[DivisionFP] [smallint] NULL,
	[TEMPLATE4] [image] NULL,
	[TEMPLATE3] [image] NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NULL,
	[Valid] [int] NULL,
	[Fpversion] [nvarchar](50) NULL,
	[bio_type] [int] NULL,
	[SN] [int] NULL,
	[UTime] [datetime] NULL,
	[StateMigrationFlag] [nvarchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[TEMPLATEID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEMPLATEEx]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEMPLATEEx](
	[TEMPLATEID] [int] IDENTITY(1,1) NOT NULL,
	[USERID] [int] NOT NULL,
	[FINGERID] [int] NOT NULL,
	[TEMPLATE] [image] NULL,
	[TEMPLATE2] [image] NULL,
	[BITMAPPICTURE] [image] NULL,
	[BITMAPPICTURE2] [image] NULL,
	[BITMAPPICTURE3] [image] NULL,
	[BITMAPPICTURE4] [image] NULL,
	[USETYPE] [smallint] NULL,
	[EMACHINENUM] [nvarchar](3) NULL,
	[TEMPLATE1] [image] NULL,
	[Flag] [smallint] NULL,
	[DivisionFP] [smallint] NULL,
	[TEMPLATE4] [image] NULL,
	[TEMPLATE3] [image] NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NULL,
	[Valid] [int] NULL,
	[Fpversion] [nvarchar](50) NULL,
	[bio_type] [int] NULL,
	[SN] [int] NULL,
	[UTime] [datetime] NULL,
	[StateMigrationFlag] [nvarchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USERINFO]    Script Date: 2023-11-05 9:48:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERINFO](
	[USERID] [int] IDENTITY(1,1) NOT NULL,
	[Badgenumber] [nvarchar](50) NULL,
	[SSN] [nvarchar](20) NULL,
	[Name] [nvarchar](50) NULL,
	[Gender] [nvarchar](8) NULL,
	[TITLE] [nvarchar](20) NULL,
	[PAGER] [nvarchar](20) NULL,
	[BIRTHDAY] [datetime] NULL,
	[HIREDDAY] [datetime] NULL,
	[street] [nvarchar](80) NULL,
	[CITY] [nvarchar](50) NULL,
	[STATE] [nvarchar](50) NULL,
	[ZIP] [nvarchar](50) NULL,
	[OPHONE] [nvarchar](20) NULL,
	[FPHONE] [nvarchar](20) NULL,
	[VERIFICATIONMETHOD] [smallint] NULL,
	[DEFAULTDEPTID] [int] NULL,
	[SECURITYFLAGS] [smallint] NULL,
	[ATT] [smallint] NOT NULL,
	[INLATE] [smallint] NOT NULL,
	[OUTEARLY] [smallint] NOT NULL,
	[OVERTIME] [smallint] NOT NULL,
	[SEP] [smallint] NOT NULL,
	[HOLIDAY] [smallint] NOT NULL,
	[MINZU] [nvarchar](8) NULL,
	[PASSWORD] [nvarchar](20) NULL,
	[LUNCHDURATION] [smallint] NULL,
	[PHOTO] [image] NULL,
	[mverifypass] [nvarchar](10) NULL,
	[Notes] [image] NULL,
	[privilege] [int] NULL,
	[InheritDeptSch] [smallint] NULL,
	[InheritDeptSchClass] [smallint] NULL,
	[AutoSchPlan] [smallint] NULL,
	[MinAutoSchInterval] [int] NULL,
	[RegisterOT] [smallint] NULL,
	[InheritDeptRule] [smallint] NULL,
	[EMPRIVILEGE] [smallint] NULL,
	[CardNo] [nvarchar](20) NULL,
	[change_operator] [nvarchar](50) NULL,
	[change_time] [datetime] NULL,
	[create_operator] [nvarchar](50) NULL,
	[create_time] [datetime] NULL,
	[delete_operator] [nvarchar](50) NULL,
	[delete_time] [datetime] NULL,
	[status] [int] NULL,
	[lastname] [nvarchar](50) NULL,
	[AccGroup] [int] NULL,
	[TimeZones] [nvarchar](50) NULL,
	[identitycard] [nvarchar](50) NULL,
	[UTime] [datetime] NULL,
	[Education] [nvarchar](50) NULL,
	[OffDuty] [int] NULL,
	[DelTag] [int] NULL,
	[morecard_group_id] [int] NULL,
	[set_valid_time] [bit] NULL,
	[acc_startdate] [datetime] NULL,
	[acc_enddate] [datetime] NULL,
	[birthplace] [nvarchar](50) NULL,
	[Political] [nvarchar](50) NULL,
	[contry] [nvarchar](50) NULL,
	[hiretype] [int] NULL,
	[email] [nvarchar](50) NULL,
	[firedate] [datetime] NULL,
	[isatt] [bit] NULL,
	[homeaddress] [nvarchar](50) NULL,
	[emptype] [int] NULL,
	[bankcode1] [nvarchar](50) NULL,
	[bankcode2] [nvarchar](50) NULL,
	[isblacklist] [int] NULL,
	[Iuser1] [int] NULL,
	[Iuser2] [int] NULL,
	[Iuser3] [int] NULL,
	[Iuser4] [int] NULL,
	[Iuser5] [int] NULL,
	[Cuser1] [nvarchar](50) NULL,
	[Cuser2] [nvarchar](50) NULL,
	[Cuser3] [nvarchar](50) NULL,
	[Cuser4] [nvarchar](50) NULL,
	[Cuser5] [nvarchar](50) NULL,
	[Duser1] [datetime] NULL,
	[Duser2] [datetime] NULL,
	[Duser3] [datetime] NULL,
	[Duser4] [datetime] NULL,
	[Duser5] [datetime] NULL,
	[carNo] [nvarchar](50) NULL,
	[carType] [nvarchar](50) NULL,
	[carBrand] [nvarchar](50) NULL,
	[carColor] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[USERID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_one_mode]  DEFAULT ('False') FOR [one_mode]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_two_mode]  DEFAULT ('False') FOR [two_mode]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_three_mode]  DEFAULT ('False') FOR [three_mode]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_four_mode]  DEFAULT ('False') FOR [four_mode]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_five_mode]  DEFAULT ('False') FOR [five_mode]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_six_mode]  DEFAULT ('False') FOR [six_mode]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_seven_mode]  DEFAULT ('False') FOR [seven_mode]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_eight_mode]  DEFAULT ('False') FOR [eight_mode]
GO
ALTER TABLE [dbo].[acc_antiback] ADD  CONSTRAINT [default_value_acc_antiback_nine_mode]  DEFAULT ('False') FOR [nine_mode]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_door_name]  DEFAULT ('') FOR [door_name]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_lock_delay]  DEFAULT ('5') FOR [lock_delay]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_back_lock]  DEFAULT ('True') FOR [back_lock]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_sensor_delay]  DEFAULT ('15') FOR [sensor_delay]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_opendoor_type]  DEFAULT ('6') FOR [opendoor_type]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_inout_state]  DEFAULT ('0') FOR [inout_state]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_lock_active_id]  DEFAULT ('1') FOR [lock_active_id]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_wiegand_fmt_id]  DEFAULT ('1') FOR [wiegand_fmt_id]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_card_intervaltime]  DEFAULT ('2') FOR [card_intervaltime]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_reader_type]  DEFAULT ('0') FOR [reader_type]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_is_att]  DEFAULT ('False') FOR [is_att]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_force_pwd]  DEFAULT ('') FOR [force_pwd]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_supper_pwd]  DEFAULT ('') FOR [supper_pwd]
GO
ALTER TABLE [dbo].[acc_door] ADD  CONSTRAINT [default_value_acc_door_door_sensor_status]  DEFAULT ('0') FOR [door_sensor_status]
GO
ALTER TABLE [dbo].[acc_door] ADD  DEFAULT ('0') FOR [reader_io_state]
GO
ALTER TABLE [dbo].[acc_door] ADD  DEFAULT ((15)) FOR [open_door_delay]
GO
ALTER TABLE [dbo].[acc_door] ADD  DEFAULT ((1)) FOR [wiegandInType]
GO
ALTER TABLE [dbo].[acc_door] ADD  DEFAULT ((1)) FOR [wiegandOutType]
GO
ALTER TABLE [dbo].[acc_door] ADD  DEFAULT ((0)) FOR [wiegand_fmt_out_id]
GO
ALTER TABLE [dbo].[acc_door] ADD  DEFAULT ((0)) FOR [SRBOn]
GO
ALTER TABLE [dbo].[acc_firstopen] ADD  CONSTRAINT [default_value_acc_firstopen_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_firstopen] ADD  CONSTRAINT [default_value_acc_firstopen_door_id]  DEFAULT ('1') FOR [door_id]
GO
ALTER TABLE [dbo].[acc_holidays] ADD  CONSTRAINT [default_value_acc_holidays_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_holidays] ADD  CONSTRAINT [default_value_acc_holidays_holiday_name]  DEFAULT ('') FOR [holiday_name]
GO
ALTER TABLE [dbo].[acc_holidays] ADD  CONSTRAINT [default_value_acc_holidays_holiday_type]  DEFAULT ('1') FOR [holiday_type]
GO
ALTER TABLE [dbo].[acc_holidays] ADD  CONSTRAINT [default_value_acc_holidays_loop_by_year]  DEFAULT ('2') FOR [loop_by_year]
GO
ALTER TABLE [dbo].[acc_holidays] ADD  CONSTRAINT [default_value_acc_holidays_memo]  DEFAULT ('') FOR [memo]
GO
ALTER TABLE [dbo].[acc_interlock] ADD  CONSTRAINT [default_value_acc_interlock_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_interlock] ADD  CONSTRAINT [default_value_acc_interlock_one_mode]  DEFAULT ('False') FOR [one_mode]
GO
ALTER TABLE [dbo].[acc_interlock] ADD  CONSTRAINT [default_value_acc_interlock_two_mode]  DEFAULT ('False') FOR [two_mode]
GO
ALTER TABLE [dbo].[acc_interlock] ADD  CONSTRAINT [default_value_acc_interlock_three_mode]  DEFAULT ('False') FOR [three_mode]
GO
ALTER TABLE [dbo].[acc_interlock] ADD  CONSTRAINT [default_value_acc_interlock_four_mode]  DEFAULT ('False') FOR [four_mode]
GO
ALTER TABLE [dbo].[acc_levelset] ADD  CONSTRAINT [default_value_acc_levelset_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_levelset] ADD  CONSTRAINT [default_value_acc_levelset_level_name]  DEFAULT ('') FOR [level_name]
GO
ALTER TABLE [dbo].[acc_levelset_door_group] ADD  DEFAULT ((0)) FOR [accdoor_no_exp]
GO
ALTER TABLE [dbo].[acc_levelset_door_group] ADD  DEFAULT ((0)) FOR [accdoor_device_id]
GO
ALTER TABLE [dbo].[acc_levelset_door_group] ADD  DEFAULT ((0)) FOR [level_timeseg_id]
GO
ALTER TABLE [dbo].[acc_linkageio] ADD  CONSTRAINT [default_value_acc_linkageio_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_linkageio] ADD  CONSTRAINT [default_value_acc_linkageio_trigger_opt]  DEFAULT ('0') FOR [trigger_opt]
GO
ALTER TABLE [dbo].[acc_linkageio] ADD  CONSTRAINT [default_value_acc_linkageio_in_address]  DEFAULT ('0') FOR [in_address]
GO
ALTER TABLE [dbo].[acc_linkageio] ADD  CONSTRAINT [default_value_acc_linkageio_action_type]  DEFAULT ('0') FOR [delay_time]
GO
ALTER TABLE [dbo].[acc_linkageio] ADD  CONSTRAINT [default_value_acc_linkageio_delay_time]  DEFAULT ('20') FOR [video_linkageio_id]
GO
ALTER TABLE [dbo].[acc_map] ADD  CONSTRAINT [default_value_acc_map_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_map] ADD  CONSTRAINT [default_value_acc_map_map_name]  DEFAULT ('') FOR [map_name]
GO
ALTER TABLE [dbo].[acc_map] ADD  CONSTRAINT [default_value_acc_map_width]  DEFAULT ('0') FOR [width]
GO
ALTER TABLE [dbo].[acc_map] ADD  CONSTRAINT [default_value_acc_map_height]  DEFAULT ('0') FOR [height]
GO
ALTER TABLE [dbo].[acc_mapdoorpos] ADD  CONSTRAINT [default_value_acc_mapdoorpos_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_mapdoorpos] ADD  CONSTRAINT [default_value_acc_mapdoorpos_width]  DEFAULT ('40') FOR [width]
GO
ALTER TABLE [dbo].[acc_monitor_log] ADD  CONSTRAINT [default_value_acc_monitor_log_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_monitor_log] ADD  CONSTRAINT [default_value_acc_monitor_log_verified]  DEFAULT ('200') FOR [verified]
GO
ALTER TABLE [dbo].[acc_monitor_log] ADD  DEFAULT ('-1') FOR [event_point_type]
GO
ALTER TABLE [dbo].[acc_monitor_log] ADD  DEFAULT ('-1') FOR [event_point_id]
GO
ALTER TABLE [dbo].[acc_morecardempgroup] ADD  CONSTRAINT [default_value_acc_morecardempgroup_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_morecardgroup] ADD  CONSTRAINT [default_value_acc_morecardgroup_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_morecardset] ADD  CONSTRAINT [default_value_acc_morecardset_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  CONSTRAINT [default_value_acc_timeseg_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  CONSTRAINT [default_value_acc_timeseg_memo]  DEFAULT ('') FOR [memo]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  CONSTRAINT [default_value_acc_timeseg_sunday_start1]  DEFAULT ('00:00') FOR [sunday_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [sunday_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [sunday_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [sunday_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [sunday_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [sunday_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [monday_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [monday_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [monday_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [monday_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [monday_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [monday_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [tuesday_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [tuesday_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [tuesday_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [tuesday_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [tuesday_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [tuesday_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [wednesday_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [wednesday_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [wednesday_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [wednesday_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [wednesday_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [wednesday_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [thursday_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [thursday_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [thursday_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [thursday_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [thursday_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [thursday_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [friday_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [friday_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [friday_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [friday_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [friday_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [friday_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [saturday_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [saturday_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [saturday_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [saturday_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [saturday_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [saturday_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype1_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype1_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype1_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype1_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype1_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype1_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype2_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype2_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype2_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype2_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype2_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype2_end3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype3_start1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype3_end1]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype3_start2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype3_end2]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype3_start3]
GO
ALTER TABLE [dbo].[acc_timeseg] ADD  DEFAULT ('00:00') FOR [holidaytype3_end3]
GO
ALTER TABLE [dbo].[acc_wiegandfmt] ADD  CONSTRAINT [default_value_acc_wiegandfmt_wiegand_name]  DEFAULT ('') FOR [wiegand_name]
GO
ALTER TABLE [dbo].[CHECKINOUT] ADD  DEFAULT (getdate()) FOR [CHECKTIME]
GO
ALTER TABLE [dbo].[CHECKINOUT] ADD  DEFAULT ('I') FOR [CHECKTYPE]
GO
ALTER TABLE [dbo].[CHECKINOUT] ADD  DEFAULT ((0)) FOR [VERIFYCODE]
GO
ALTER TABLE [dbo].[Custom_acc_monitor_log] ADD  CONSTRAINT [DF_Custom_acc_monitor_log_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[Custom_acc_monitor_log] ADD  CONSTRAINT [DF_Custom_acc_monitor_log_verified]  DEFAULT ('200') FOR [verified]
GO
ALTER TABLE [dbo].[Custom_acc_monitor_log] ADD  CONSTRAINT [DF_Custom_acc_monitor_log_event_point_type]  DEFAULT ('-1') FOR [event_point_type]
GO
ALTER TABLE [dbo].[Custom_acc_monitor_log] ADD  CONSTRAINT [DF_Custom_acc_monitor_log_event_point_id]  DEFAULT ('-1') FOR [event_point_id]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [SUPDEPTID]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [InheritParentSch]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [InheritDeptSch]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [InheritDeptSchClass]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [AutoSchPlan]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [InLate]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [OutEarly]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [InheritDeptRule]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((24)) FOR [MinAutoSchInterval]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [RegisterOT]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [DefaultSchId]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [ATT]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [Holiday]
GO
ALTER TABLE [dbo].[DEPARTMENTS] ADD  DEFAULT ((1)) FOR [OverTime]
GO
ALTER TABLE [dbo].[devcmds] ADD  CONSTRAINT [default_value_devcmds_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[devcmds] ADD  CONSTRAINT [default_value_devcmds_CmdCommitTime]  DEFAULT ('2011-07-15 16:06:23.608000') FOR [CmdCommitTime]
GO
ALTER TABLE [dbo].[devcmds] ADD  CONSTRAINT [default_value_devcmds_CmdImmediately]  DEFAULT ('False') FOR [CmdImmediately]
GO
ALTER TABLE [dbo].[FaceTemp] ADD  DEFAULT ((0)) FOR [SIZE]
GO
ALTER TABLE [dbo].[FaceTemp] ADD  DEFAULT ((0)) FOR [pin]
GO
ALTER TABLE [dbo].[FaceTemp] ADD  DEFAULT ((0)) FOR [FACEID]
GO
ALTER TABLE [dbo].[FaceTemp] ADD  DEFAULT ((0)) FOR [VALID]
GO
ALTER TABLE [dbo].[FaceTemp] ADD  DEFAULT ((0)) FOR [RESERVE]
GO
ALTER TABLE [dbo].[FaceTemp] ADD  DEFAULT ((0)) FOR [ACTIVETIME]
GO
ALTER TABLE [dbo].[FaceTemp] ADD  DEFAULT ((0)) FOR [VFCOUNT]
GO
ALTER TABLE [dbo].[FingerVein] ADD  DEFAULT ((0)) FOR [UserID]
GO
ALTER TABLE [dbo].[FingerVein] ADD  DEFAULT ((0)) FOR [Size]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((1)) FOR [SerialPort]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('4370') FOR [Port]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((1)) FOR [MachineNumber]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('True') FOR [Enabled]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [UILanguage]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [DateFormat]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [InOutRecordWarn]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [Idle]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [Voice]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [LockControl]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('1111101000') FOR [UpdateDB]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('utf8') FOR [lng_encode]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('0') FOR [AccFun]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('8') FOR [TZAdj]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('2') FOR [acpanel_type]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('True') FOR [sync_time]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('False') FOR [four_to_two]
GO
ALTER TABLE [dbo].[Machines] ADD  CONSTRAINT [default_value_iclock_max_comm_size]  DEFAULT ('40') FOR [max_comm_size]
GO
ALTER TABLE [dbo].[Machines] ADD  CONSTRAINT [default_value_iclock_max_comm_count]  DEFAULT ('20') FOR [max_comm_count]
GO
ALTER TABLE [dbo].[Machines] ADD  CONSTRAINT [default_value_iclock_realtime]  DEFAULT ('True') FOR [realtime]
GO
ALTER TABLE [dbo].[Machines] ADD  CONSTRAINT [default_value_iclock_delay]  DEFAULT ('10') FOR [delay]
GO
ALTER TABLE [dbo].[Machines] ADD  CONSTRAINT [default_value_iclock_encrypt]  DEFAULT ('False') FOR [encrypt]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('1') FOR [com_address]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((0)) FOR [SimpleEventType]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((0)) FOR [FvFunOn]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((0)) FOR [fvcount]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ('False') FOR [IsWireless]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [managercount]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [usercount]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [fingercount]
GO
ALTER TABLE [dbo].[Machines] ADD  DEFAULT ((-1)) FOR [SecretCount]
GO
ALTER TABLE [dbo].[personnel_area] ADD  CONSTRAINT [default_value_personnel_area_status]  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[personnel_area] ADD  DEFAULT ((0)) FOR [parent_id]
GO
ALTER TABLE [dbo].[personnel_issuecard] ADD  DEFAULT ('1') FOR [isvalid]
GO
ALTER TABLE [dbo].[personnel_issuecard] ADD  DEFAULT ('1') FOR [cardstatus]
GO
ALTER TABLE [dbo].[personnel_issuecard] ADD  DEFAULT ('2011-07-15') FOR [issuedate]
GO
ALTER TABLE [dbo].[TEMPLATE] ADD  DEFAULT ('0') FOR [FINGERID]
GO
ALTER TABLE [dbo].[TEMPLATE] ADD  DEFAULT ('1') FOR [Valid]
GO
ALTER TABLE [dbo].[TEMPLATE] ADD  DEFAULT ('0') FOR [bio_type]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [DEFAULTDEPTID]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [ATT]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [INLATE]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [OUTEARLY]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [OVERTIME]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [SEP]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [HOLIDAY]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((0)) FOR [privilege]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [AutoSchPlan]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((24)) FOR [MinAutoSchInterval]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ((1)) FOR [RegisterOT]
GO
ALTER TABLE [dbo].[USERINFO] ADD  DEFAULT ('0') FOR [status]
GO
ALTER TABLE [dbo].[acc_door]  WITH CHECK ADD  CONSTRAINT [CK__acc_door__card_intervaltime] CHECK  (([card_intervaltime]>=(0)))
GO
ALTER TABLE [dbo].[acc_door] CHECK CONSTRAINT [CK__acc_door__card_intervaltime]
GO
ALTER TABLE [dbo].[acc_door]  WITH CHECK ADD  CONSTRAINT [CK__acc_door__door_no] CHECK  (([door_no]>=(0)))
GO
ALTER TABLE [dbo].[acc_door] CHECK CONSTRAINT [CK__acc_door__door_no]
GO
ALTER TABLE [dbo].[acc_door]  WITH CHECK ADD  CONSTRAINT [CK__acc_door__lock_delay] CHECK  (([lock_delay]>=(0)))
GO
ALTER TABLE [dbo].[acc_door] CHECK CONSTRAINT [CK__acc_door__lock_delay]
GO
ALTER TABLE [dbo].[acc_door]  WITH CHECK ADD  CONSTRAINT [CK__acc_door__sensor_delay] CHECK  (([sensor_delay]>=(0)))
GO
ALTER TABLE [dbo].[acc_door] CHECK CONSTRAINT [CK__acc_door__sensor_delay]
GO
USE [master]
GO
ALTER DATABASE [DB_3rdEyE_ZKT_RFID] SET  READ_WRITE 
GO
