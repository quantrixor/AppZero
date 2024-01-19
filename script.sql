--create database [dbLocal]

USE [dbLocal]
GO
/****** Object:  Table [dbo].[Peripherals]    Script Date: 2/28/2023 4:46:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Peripherals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RackNumber] [nchar](10) NOT NULL,
	[ShelfNumber] [nchar](10) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[Count] [int] NOT NULL,
	[DateAdded] [date] NOT NULL,
 CONSTRAINT [PK_Peripherals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Position]    Script Date: 2/28/2023 4:46:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 2/28/2023 4:46:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rule](
	[ID] [char](1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SignIn]    Script Date: 2/28/2023 4:46:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SignIn](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[IDRole] [char](1) NOT NULL,
 CONSTRAINT [PK_SignIn] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpareParts]    Script Date: 2/28/2023 4:46:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpareParts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RackNumber] [nchar](10) NOT NULL,
	[ShelfNumber] [nchar](10) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[IDTypeObject] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[DateAdded] [date] NOT NULL,
 CONSTRAINT [PK_SpareParts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeObject]    Script Date: 2/28/2023 4:46:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeObject](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TypeObject] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2/28/2023 4:46:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[IDPosition] [int] NOT NULL,
	[IDSignIn] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Peripherals] ON 

INSERT [dbo].[Peripherals] ([ID], [RackNumber], [ShelfNumber], [Description], [Count], [DateAdded]) VALUES (1, N'1110      ', N'113       ', N'Описание 1', 20, CAST(N'2023-02-28' AS Date))
INSERT [dbo].[Peripherals] ([ID], [RackNumber], [ShelfNumber], [Description], [Count], [DateAdded]) VALUES (2, N'2220      ', N'115       ', N'Описание 2', 10, CAST(N'2023-02-26' AS Date))
INSERT [dbo].[Peripherals] ([ID], [RackNumber], [ShelfNumber], [Description], [Count], [DateAdded]) VALUES (3, N'1992      ', N'110       ', N'Описание 3', 5, CAST(N'2023-02-20' AS Date))
SET IDENTITY_INSERT [dbo].[Peripherals] OFF
GO
SET IDENTITY_INSERT [dbo].[Position] ON 

INSERT [dbo].[Position] ([ID], [Title]) VALUES (1, N'Должность А')
INSERT [dbo].[Position] ([ID], [Title]) VALUES (2, N'Должность Б')
SET IDENTITY_INSERT [dbo].[Position] OFF
GO
INSERT [dbo].[Rule] ([ID], [Title]) VALUES (N'A', N'Admin')
INSERT [dbo].[Rule] ([ID], [Title]) VALUES (N'U', N'User')
GO
SET IDENTITY_INSERT [dbo].[SignIn] ON 

INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (1, N'a', N'a', N'A')
INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (2, N'u', N'u', N'U')
INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (4, N'test', N'test', N'U')
SET IDENTITY_INSERT [dbo].[SignIn] OFF
GO
SET IDENTITY_INSERT [dbo].[SpareParts] ON 

INSERT [dbo].[SpareParts] ([ID], [RackNumber], [ShelfNumber], [Description], [IDTypeObject], [Count], [DateAdded]) VALUES (1, N'9981      ', N'201       ', N'Описание 1', 1, 10, CAST(N'2023-02-20' AS Date))
INSERT [dbo].[SpareParts] ([ID], [RackNumber], [ShelfNumber], [Description], [IDTypeObject], [Count], [DateAdded]) VALUES (2, N'9882      ', N'200       ', N'Описание 2', 2, 20, CAST(N'2023-02-22' AS Date))
INSERT [dbo].[SpareParts] ([ID], [RackNumber], [ShelfNumber], [Description], [IDTypeObject], [Count], [DateAdded]) VALUES (3, N'9883      ', N'203       ', N'Описание 3', 3, 2, CAST(N'2023-02-19' AS Date))
SET IDENTITY_INSERT [dbo].[SpareParts] OFF
GO
SET IDENTITY_INSERT [dbo].[TypeObject] ON 

INSERT [dbo].[TypeObject] ([ID], [Title]) VALUES (1, N'Тип 1')
INSERT [dbo].[TypeObject] ([ID], [Title]) VALUES (2, N'Тип 2')
INSERT [dbo].[TypeObject] ([ID], [Title]) VALUES (3, N'Тип 3')
SET IDENTITY_INSERT [dbo].[TypeObject] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (1, N'A', N'A', N'A', 1, 1)
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (2, N'U', N'U', N'U', 2, 2)
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (3, N'Test', N'Test', N'Test', 1, 4)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[SignIn]  WITH CHECK ADD  CONSTRAINT [FK_SignIn_Rule] FOREIGN KEY([IDRole])
REFERENCES [dbo].[Rule] ([ID])
GO
ALTER TABLE [dbo].[SignIn] CHECK CONSTRAINT [FK_SignIn_Rule]
GO
ALTER TABLE [dbo].[SpareParts]  WITH CHECK ADD  CONSTRAINT [FK_SpareParts_TypeObject] FOREIGN KEY([IDTypeObject])
REFERENCES [dbo].[TypeObject] ([ID])
GO
ALTER TABLE [dbo].[SpareParts] CHECK CONSTRAINT [FK_SpareParts_TypeObject]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Position] FOREIGN KEY([IDPosition])
REFERENCES [dbo].[Position] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Position]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_SignIn] FOREIGN KEY([IDSignIn])
REFERENCES [dbo].[SignIn] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_SignIn]
GO
