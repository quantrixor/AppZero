USE [dbLocal]
GO
/****** Object:  Table [dbo].[Peripherals]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Peripherals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDRack] [int] NOT NULL,
	[Description] [nvarchar](100) NULL,
	[Count] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[IDTypeHall] [int] NOT NULL,
	[IDSubtypeHall] [int] NULL,
 CONSTRAINT [PK_Peripherals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeripheralShelf]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeripheralShelf](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PeripheralID] [int] NOT NULL,
	[ShelfID] [int] NOT NULL,
 CONSTRAINT [PK_PeripheralShelf] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Position]    Script Date: 1/31/2024 10:21:28 PM ******/
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
/****** Object:  Table [dbo].[Rack]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rack](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nchar](10) NOT NULL,
	[CountShelves] [int] NOT NULL,
 CONSTRAINT [PK_Rack] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 1/31/2024 10:21:28 PM ******/
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
/****** Object:  Table [dbo].[Shelves]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shelves](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nchar](10) NOT NULL,
	[IDRack] [int] NOT NULL,
 CONSTRAINT [PK_Shelves] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SignIn]    Script Date: 1/31/2024 10:21:28 PM ******/
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
/****** Object:  Table [dbo].[SpareParts]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpareParts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDRack] [int] NOT NULL,
	[Description] [nvarchar](100) NULL,
	[IDPeripherals] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[IDTypeWarehouse] [int] NOT NULL,
	[IDSubtypeWarehouse] [int] NOT NULL,
 CONSTRAINT [PK_SpareParts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SparePartsShelves]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SparePartsShelves](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDSpareParts] [int] NOT NULL,
	[IDShelf] [int] NOT NULL,
 CONSTRAINT [PK_SparePartsShelves] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubtypeHall]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubtypeHall](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[IDTypeHall] [int] NOT NULL,
 CONSTRAINT [PK_SubtypeHall] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubtypeWarehouseType]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubtypeWarehouseType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[WarehouseTypeId] [int] NULL,
 CONSTRAINT [PK_SubtypeWarehouseType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeHall]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeHall](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Titiel] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TypeHall] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 1/31/2024 10:21:28 PM ******/
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
/****** Object:  Table [dbo].[WarehouseType]    Script Date: 1/31/2024 10:21:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarehouseType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_WarehouseType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Peripherals] ON 

INSERT [dbo].[Peripherals] ([ID], [IDRack], [Description], [Count], [DateAdded], [IDTypeHall], [IDSubtypeHall]) VALUES (4, 2, N'Зал', 1, CAST(N'2024-01-31T22:06:25.983' AS DateTime), 2, 4)
INSERT [dbo].[Peripherals] ([ID], [IDRack], [Description], [Count], [DateAdded], [IDTypeHall], [IDSubtypeHall]) VALUES (5, 1, N'Зал', 2, CAST(N'2024-01-31T22:04:05.603' AS DateTime), 3, 2)
SET IDENTITY_INSERT [dbo].[Peripherals] OFF
GO
SET IDENTITY_INSERT [dbo].[PeripheralShelf] ON 

INSERT [dbo].[PeripheralShelf] ([ID], [PeripheralID], [ShelfID]) VALUES (17, 5, 1)
INSERT [dbo].[PeripheralShelf] ([ID], [PeripheralID], [ShelfID]) VALUES (18, 5, 2)
INSERT [dbo].[PeripheralShelf] ([ID], [PeripheralID], [ShelfID]) VALUES (20, 4, 9)
SET IDENTITY_INSERT [dbo].[PeripheralShelf] OFF
GO
SET IDENTITY_INSERT [dbo].[Position] ON 

INSERT [dbo].[Position] ([ID], [Title]) VALUES (1, N'Инженер кибербезопасности')
INSERT [dbo].[Position] ([ID], [Title]) VALUES (2, N'Руководитель')
INSERT [dbo].[Position] ([ID], [Title]) VALUES (3, N'Сотрудник')
SET IDENTITY_INSERT [dbo].[Position] OFF
GO
SET IDENTITY_INSERT [dbo].[Rack] ON 

INSERT [dbo].[Rack] ([ID], [Number], [CountShelves]) VALUES (1, N'2         ', 1)
INSERT [dbo].[Rack] ([ID], [Number], [CountShelves]) VALUES (2, N'1         ', 0)
INSERT [dbo].[Rack] ([ID], [Number], [CountShelves]) VALUES (3, N'3         ', 0)
INSERT [dbo].[Rack] ([ID], [Number], [CountShelves]) VALUES (4, N'5         ', 0)
SET IDENTITY_INSERT [dbo].[Rack] OFF
GO
INSERT [dbo].[Rule] ([ID], [Title]) VALUES (N'A', N'Admin')
INSERT [dbo].[Rule] ([ID], [Title]) VALUES (N'U', N'User')
GO
SET IDENTITY_INSERT [dbo].[Shelves] ON 

INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (1, N'2.1       ', 1)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (2, N'2.2       ', 1)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (3, N'2.3       ', 1)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (4, N'2.4       ', 1)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (5, N'1.1       ', 2)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (6, N'1.2       ', 2)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (7, N'1.3       ', 2)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (8, N'1.4       ', 2)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (9, N'1.5       ', 2)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (10, N'3.1       ', 3)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (11, N'3.2       ', 3)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (12, N'3.3       ', 3)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (13, N'3.4       ', 3)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (14, N'3.5       ', 3)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (15, N'5.1       ', 4)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (16, N'5.2       ', 4)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (17, N'5.3       ', 4)
INSERT [dbo].[Shelves] ([ID], [Number], [IDRack]) VALUES (18, N'5.4       ', 4)
SET IDENTITY_INSERT [dbo].[Shelves] OFF
GO
SET IDENTITY_INSERT [dbo].[SignIn] ON 

INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (1, N'admin', N'admin', N'A')
INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (2, N'user', N'user', N'U')
INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (3, N'alina_test', N'password', N'U')
INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (1002, N'sasha', N'sasha', N'U')
INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (1003, N'marina', N'marina', N'U')
INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (1005, N'test 2', N'test', N'U')
INSERT [dbo].[SignIn] ([ID], [Username], [Password], [IDRole]) VALUES (1006, N'test', N'testes', N'U')
SET IDENTITY_INSERT [dbo].[SignIn] OFF
GO
SET IDENTITY_INSERT [dbo].[SpareParts] ON 

INSERT [dbo].[SpareParts] ([ID], [IDRack], [Description], [IDPeripherals], [Count], [DateAdded], [IDTypeWarehouse], [IDSubtypeWarehouse]) VALUES (12, 1, N'Склад материлова', 0, 2, CAST(N'2024-01-30T00:00:00.000' AS DateTime), 9, 16)
INSERT [dbo].[SpareParts] ([ID], [IDRack], [Description], [IDPeripherals], [Count], [DateAdded], [IDTypeWarehouse], [IDSubtypeWarehouse]) VALUES (13, 1, N'Деревянная', 0, 1, CAST(N'2024-01-30T00:00:00.000' AS DateTime), 10, 20)
INSERT [dbo].[SpareParts] ([ID], [IDRack], [Description], [IDPeripherals], [Count], [DateAdded], [IDTypeWarehouse], [IDSubtypeWarehouse]) VALUES (14, 3, N'Склад деревянной кровли', 0, 2, CAST(N'2024-01-30T00:00:00.000' AS DateTime), 10, 19)
INSERT [dbo].[SpareParts] ([ID], [IDRack], [Description], [IDPeripherals], [Count], [DateAdded], [IDTypeWarehouse], [IDSubtypeWarehouse]) VALUES (15, 2, N'Склад пластикова кровля', 0, 2, CAST(N'2024-01-30T00:00:00.000' AS DateTime), 9, 17)
SET IDENTITY_INSERT [dbo].[SpareParts] OFF
GO
SET IDENTITY_INSERT [dbo].[SparePartsShelves] ON 

INSERT [dbo].[SparePartsShelves] ([ID], [IDSpareParts], [IDShelf]) VALUES (22, 12, 1)
INSERT [dbo].[SparePartsShelves] ([ID], [IDSpareParts], [IDShelf]) VALUES (23, 12, 2)
INSERT [dbo].[SparePartsShelves] ([ID], [IDSpareParts], [IDShelf]) VALUES (24, 13, 3)
INSERT [dbo].[SparePartsShelves] ([ID], [IDSpareParts], [IDShelf]) VALUES (25, 14, 11)
INSERT [dbo].[SparePartsShelves] ([ID], [IDSpareParts], [IDShelf]) VALUES (26, 14, 12)
INSERT [dbo].[SparePartsShelves] ([ID], [IDSpareParts], [IDShelf]) VALUES (27, 15, 7)
INSERT [dbo].[SparePartsShelves] ([ID], [IDSpareParts], [IDShelf]) VALUES (28, 15, 8)
SET IDENTITY_INSERT [dbo].[SparePartsShelves] OFF
GO
SET IDENTITY_INSERT [dbo].[SubtypeHall] ON 

INSERT [dbo].[SubtypeHall] ([ID], [Title], [IDTypeHall]) VALUES (2, N'Test 2', 3)
INSERT [dbo].[SubtypeHall] ([ID], [Title], [IDTypeHall]) VALUES (4, N'Test', 2)
SET IDENTITY_INSERT [dbo].[SubtypeHall] OFF
GO
SET IDENTITY_INSERT [dbo].[SubtypeWarehouseType] ON 

INSERT [dbo].[SubtypeWarehouseType] ([ID], [Title], [WarehouseTypeId]) VALUES (16, N'Стеклянная кровля', 9)
INSERT [dbo].[SubtypeWarehouseType] ([ID], [Title], [WarehouseTypeId]) VALUES (17, N'Пластиковая кровля', 9)
INSERT [dbo].[SubtypeWarehouseType] ([ID], [Title], [WarehouseTypeId]) VALUES (18, N'Сланцевая кровля', 9)
INSERT [dbo].[SubtypeWarehouseType] ([ID], [Title], [WarehouseTypeId]) VALUES (19, N'Деревянная сосновая кровля', 10)
INSERT [dbo].[SubtypeWarehouseType] ([ID], [Title], [WarehouseTypeId]) VALUES (20, N'Деревянная кедровая кровля', 10)
SET IDENTITY_INSERT [dbo].[SubtypeWarehouseType] OFF
GO
SET IDENTITY_INSERT [dbo].[TypeHall] ON 

INSERT [dbo].[TypeHall] ([ID], [Titiel]) VALUES (2, N'Кровля деревянная')
INSERT [dbo].[TypeHall] ([ID], [Titiel]) VALUES (3, N'Стеклянная кровля')
SET IDENTITY_INSERT [dbo].[TypeHall] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (1, N'Павел', N'Памфилов', N'Александрович', 1, 1)
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (2, N'Александра', N'Кариева', N'Игоревна', 3, 1002)
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (3, N'Михаил', N'Тарасов', N'Алексеевич', 2, 1)
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (4, N'Марина', N'Васильева', N'Сергеевна', 1, 1003)
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (5, N'Test 2', N'Test 2', N'Test', 3, 1005)
INSERT [dbo].[User] ([ID], [FirstName], [LastName], [MiddleName], [IDPosition], [IDSignIn]) VALUES (6, N'Test', N'Test', N'Test', 2, 1006)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[WarehouseType] ON 

INSERT [dbo].[WarehouseType] ([ID], [Title]) VALUES (9, N'Рулонный битумный материал')
INSERT [dbo].[WarehouseType] ([ID], [Title]) VALUES (10, N'Деревянная кровля')
SET IDENTITY_INSERT [dbo].[WarehouseType] OFF
GO
ALTER TABLE [dbo].[Peripherals]  WITH CHECK ADD  CONSTRAINT [FK_Peripherals_Rack] FOREIGN KEY([IDRack])
REFERENCES [dbo].[Rack] ([ID])
GO
ALTER TABLE [dbo].[Peripherals] CHECK CONSTRAINT [FK_Peripherals_Rack]
GO
ALTER TABLE [dbo].[Peripherals]  WITH CHECK ADD  CONSTRAINT [FK_Peripherals_SubtypeHall] FOREIGN KEY([IDSubtypeHall])
REFERENCES [dbo].[SubtypeHall] ([ID])
GO
ALTER TABLE [dbo].[Peripherals] CHECK CONSTRAINT [FK_Peripherals_SubtypeHall]
GO
ALTER TABLE [dbo].[Peripherals]  WITH CHECK ADD  CONSTRAINT [FK_Peripherals_TypeHall] FOREIGN KEY([IDTypeHall])
REFERENCES [dbo].[TypeHall] ([ID])
GO
ALTER TABLE [dbo].[Peripherals] CHECK CONSTRAINT [FK_Peripherals_TypeHall]
GO
ALTER TABLE [dbo].[PeripheralShelf]  WITH CHECK ADD  CONSTRAINT [FK_PeripheralShelf_Peripherals] FOREIGN KEY([PeripheralID])
REFERENCES [dbo].[Peripherals] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PeripheralShelf] CHECK CONSTRAINT [FK_PeripheralShelf_Peripherals]
GO
ALTER TABLE [dbo].[PeripheralShelf]  WITH CHECK ADD  CONSTRAINT [FK_PeripheralShelf_Shelves] FOREIGN KEY([ShelfID])
REFERENCES [dbo].[Shelves] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PeripheralShelf] CHECK CONSTRAINT [FK_PeripheralShelf_Shelves]
GO
ALTER TABLE [dbo].[Shelves]  WITH CHECK ADD  CONSTRAINT [FK_Shelves_Rack] FOREIGN KEY([IDRack])
REFERENCES [dbo].[Rack] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Shelves] CHECK CONSTRAINT [FK_Shelves_Rack]
GO
ALTER TABLE [dbo].[SignIn]  WITH CHECK ADD  CONSTRAINT [FK_SignIn_Rule] FOREIGN KEY([IDRole])
REFERENCES [dbo].[Rule] ([ID])
GO
ALTER TABLE [dbo].[SignIn] CHECK CONSTRAINT [FK_SignIn_Rule]
GO
ALTER TABLE [dbo].[SpareParts]  WITH CHECK ADD  CONSTRAINT [FK_SpareParts_Rack] FOREIGN KEY([IDRack])
REFERENCES [dbo].[Rack] ([ID])
GO
ALTER TABLE [dbo].[SpareParts] CHECK CONSTRAINT [FK_SpareParts_Rack]
GO
ALTER TABLE [dbo].[SpareParts]  WITH CHECK ADD  CONSTRAINT [FK_SpareParts_SubtypeWarehouseType] FOREIGN KEY([IDSubtypeWarehouse])
REFERENCES [dbo].[SubtypeWarehouseType] ([ID])
GO
ALTER TABLE [dbo].[SpareParts] CHECK CONSTRAINT [FK_SpareParts_SubtypeWarehouseType]
GO
ALTER TABLE [dbo].[SpareParts]  WITH CHECK ADD  CONSTRAINT [FK_SpareParts_WarehouseType] FOREIGN KEY([IDTypeWarehouse])
REFERENCES [dbo].[WarehouseType] ([ID])
GO
ALTER TABLE [dbo].[SpareParts] CHECK CONSTRAINT [FK_SpareParts_WarehouseType]
GO
ALTER TABLE [dbo].[SparePartsShelves]  WITH CHECK ADD  CONSTRAINT [FK_SparePartsShelves_Shelves] FOREIGN KEY([IDShelf])
REFERENCES [dbo].[Shelves] ([ID])
GO
ALTER TABLE [dbo].[SparePartsShelves] CHECK CONSTRAINT [FK_SparePartsShelves_Shelves]
GO
ALTER TABLE [dbo].[SparePartsShelves]  WITH CHECK ADD  CONSTRAINT [FK_SparePartsShelves_SpareParts] FOREIGN KEY([IDSpareParts])
REFERENCES [dbo].[SpareParts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SparePartsShelves] CHECK CONSTRAINT [FK_SparePartsShelves_SpareParts]
GO
ALTER TABLE [dbo].[SubtypeHall]  WITH CHECK ADD  CONSTRAINT [FK_SubtypeHall_TypeHall] FOREIGN KEY([IDTypeHall])
REFERENCES [dbo].[TypeHall] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubtypeHall] CHECK CONSTRAINT [FK_SubtypeHall_TypeHall]
GO
ALTER TABLE [dbo].[SubtypeWarehouseType]  WITH CHECK ADD  CONSTRAINT [FK_SubtypeWarehouseType_WarehouseType] FOREIGN KEY([WarehouseTypeId])
REFERENCES [dbo].[WarehouseType] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubtypeWarehouseType] CHECK CONSTRAINT [FK_SubtypeWarehouseType_WarehouseType]
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
