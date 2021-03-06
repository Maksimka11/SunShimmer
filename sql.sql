USE [master]
GO
/****** Object:  Database [SunShimmer]    Script Date: 10.06.2021 11:36:24 ******/
CREATE DATABASE [SunShimmer]
GO
ALTER DATABASE [SunShimmer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SunShimmer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SunShimmer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SunShimmer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SunShimmer] SET ARITHABORT OFF 
GO
ALTER DATABASE [SunShimmer] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SunShimmer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SunShimmer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SunShimmer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SunShimmer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SunShimmer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SunShimmer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SunShimmer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SunShimmer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SunShimmer] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SunShimmer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SunShimmer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SunShimmer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SunShimmer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SunShimmer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SunShimmer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SunShimmer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SunShimmer] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SunShimmer] SET  MULTI_USER 
GO
ALTER DATABASE [SunShimmer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SunShimmer] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SunShimmer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SunShimmer] SET TARGET_RECOVERY_TIME = 0 SECONDS 

GO
USE [SunShimmer]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[SecondName] [nvarchar](50) NOT NULL,
	[Patronymic] [nvarchar](50) NOT NULL,
	[DateOfBirthday] [date] NOT NULL,
	[PhoneNumber] [nvarchar](11) NOT NULL,
	[Photo] [nvarchar](100) NULL,
	[UserId] [int] NOT NULL,
	[PrivilegeId] [int] NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufacturer]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufacturer](
	[ManufacturerId] [int] IDENTITY(1,1) NOT NULL,
	[ManufacturerName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Manufacturer] PRIMARY KEY CLUSTERED 
(
	[ManufacturerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Master]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Master](
	[MasterId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[SecondName] [nvarchar](50) NOT NULL,
	[Patronymic] [nvarchar](50) NOT NULL,
	[Photo] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](23) NOT NULL,
	[Qualification] [nvarchar](500) NOT NULL,
	[WorkSchedule] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_Master] PRIMARY KEY CLUSTERED 
(
	[MasterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Privilege]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Privilege](
	[PrivilegeId] [int] IDENTITY(1,1) NOT NULL,
	[PrivilegeName] [nvarchar](50) NOT NULL,
	[Sale] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_Privilege] PRIMARY KEY CLUSTERED 
(
	[PrivilegeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[Price] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[Volume] [int] NOT NULL,
	[Photo] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[ProductType] [nvarchar](30) NOT NULL,
	[ManufacturerId] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProvisionService]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProvisionService](
	[ProvisionServiceId] [int] IDENTITY(1,1) NOT NULL,
	[TimeOfProvision] [datetime] NOT NULL,
	[Price] [int] NOT NULL,
	[SertificateId] [int] NULL,
	[ServiceId] [int] NOT NULL,
	[RecordId] [int] NOT NULL,
	[ProductId] [int] NULL,
 CONSTRAINT [PK_ProvisionService] PRIMARY KEY CLUSTERED 
(
	[ProvisionServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseProduct]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseProduct](
	[PurchaseProductId] [int] IDENTITY(1,1) NOT NULL,
	[Count] [int] NOT NULL,
	[TotalPrice] [int] NOT NULL,
	[TimeOfPurchase] [datetime] NOT NULL,
	[SertificateId] [int] NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[PurchaseProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseSertificate]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseSertificate](
	[SertificateId] [int] IDENTITY(1,1) NOT NULL,
	[TimeOfActivation] [datetime] NOT NULL,
	[SertificateStatus] [bit] NOT NULL,
	[SertificateTypeId] [int] NOT NULL,
	[RestSum] [int] not null
 CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED 
(
	[SertificateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Record]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Record](
	[RecordId] [int] IDENTITY(1,1) NOT NULL,
	[DatefApplication] [datetime] NULL,
	[ApplicationView] [bit] NOT NULL,
	[PhoneNumber] [nvarchar](11) NOT NULL,
	[Comment] [nvarchar](100) NULL,
	[TimeOfRecord] [datetime] NOT NULL,
	[RecordStatus] [nvarchar](15) NOT NULL,
	[MasterId] [int] NOT NULL,
	[ClientId] [int] NOT NULL,
 CONSTRAINT [PK_Record] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SertificateType]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SertificateType](
	[SertificateTypeId] [int] IDENTITY(1,1) NOT NULL,
	[SertificateTypeName] [nchar](10) NOT NULL,
	[Price] [int] NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_SertificateType] PRIMARY KEY CLUSTERED 
(
	[SertificateTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](100) NOT NULL,
	[Price] [int] NOT NULL,
	[Contraindications] [nvarchar](500) NULL,
	[Description] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 10.06.2021 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](30) NOT NULL,
	[Role] [nvarchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Client] ON 

INSERT [dbo].[Client] ([ClientId], [FirstName], [SecondName], [Patronymic], [DateOfBirthday], [PhoneNumber], [Photo], [UserId], [PrivilegeId]) VALUES (1, N'uigyu', N'blujn', N'gyhniku', CAST(N'2000-06-11' AS Date), N'789857564', N'(2)(1)tg.png', 1, 1)
SET IDENTITY_INSERT [dbo].[Client] OFF
SET IDENTITY_INSERT [dbo].[Manufacturer] ON 

INSERT [dbo].[Manufacturer] ([ManufacturerId], [ManufacturerName], [Description]) VALUES (1, N'Australian Gold', N'Лосьоны Australian Gold для загара в солярии созданы из самых новых и избранных ингредиентов, которые улучшают состояние кожи и защищают ее от высыхания и старения. Такие новейшие ингредиенты как бронзаторы Tan Activating и впервые в 2007 году внедренные бронзаторы DermaDark продолжают стимулировать образование более темного загара Вашей кожи.')
INSERT [dbo].[Manufacturer] ([ManufacturerId], [ManufacturerName], [Description]) VALUES (2, N'Body Butter Karat', N'Body Butter Karat - бренд американской, специализирующийся на профессиональных средствах для загара.')
INSERT [dbo].[Manufacturer] ([ManufacturerId], [ManufacturerName], [Description]) VALUES (3, N'California Tan', N'California Tan - ведущий производитель кремов для загара в солярии во всем мире. Крема для загара в солярии от California Tan объединяют в себе продвинутые технологии для достижения максимально роскошного загара и уход за кожей премиум класса.')
INSERT [dbo].[Manufacturer] ([ManufacturerId], [ManufacturerName], [Description]) VALUES (4, N'Hawaiina', N'Hawaiiana - это профессиональная косметика для загара из Германии, которая подарит вам превосходный гавайский загар. Линейка продуктов гипоаллергенна, не вызывает раздражений и шелушений на коже, обеспечивает насыщенный тёмный загар и бережно заботится о вашей коже.')
INSERT [dbo].[Manufacturer] ([ManufacturerId], [ManufacturerName], [Description]) VALUES (5, N'Kardashian glow', N'Kardashian glow - это эксклюзивная линейка профессиональных средств для загара, созданная Ким Кардашьян совместно с известнейшим производителем кремов для солярия Аustralian Gold. Кремы обеспечивают премиум уход за кожей и самый красивый насыщенный загар.')
INSERT [dbo].[Manufacturer] ([ManufacturerId], [ManufacturerName], [Description]) VALUES (6, N'Designer Skin', N'Designer Skin – это линия косметики премиум класса, разработанная компанией, специализирующейся на профессиональных средствах для загара и ставшей «родителем» таких известных брендов как AustralianGold, SwedishBeauty, CalliforniaTan и EmeraldBay. Каждый лосьон Designer Skin – это не просто средство для загара, это волшебный эликсир, который позволит вам обзавестись идеальной кожей.')
INSERT [dbo].[Manufacturer] ([ManufacturerId], [ManufacturerName], [Description]) VALUES (7, N'Callus Peel', N'Callus Peel - бренд корейской косметики, специализирующийся на средствах для педикюра. Уход за стопами от Callus Peel - это инновационные средства, которые используются более чем в 20 странах мира в профессиональной индустрии. Средства Callus Peel работают уже с первого применения. Являются абсолютно безопасными и простыми в применении.')
SET IDENTITY_INSERT [dbo].[Manufacturer] OFF
SET IDENTITY_INSERT [dbo].[Master] ON 

INSERT [dbo].[Master] ([MasterId], [FirstName], [SecondName], [Patronymic], [Photo], [PhoneNumber], [Qualification], [WorkSchedule]) VALUES (1, N'Анастасия', N'Добромирова', N'Рудольфовна', N'1.jpg', N'89023672247', N'Высшая школа косметологии и загара', N'Свободное')
SET IDENTITY_INSERT [dbo].[Master] OFF
SET IDENTITY_INSERT [dbo].[Privilege] ON 

INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Sale]) VALUES (1, N'Бронзовый уровень', N'5')
INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Sale]) VALUES (2, N'Серебрянный уровень', N'10')
INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Sale]) VALUES (3, N'Золотой уровень', N'15')
INSERT [dbo].[Privilege] ([PrivilegeId], [PrivilegeName], [Sale]) VALUES (4, N'Платиновый уровень', N'20')
SET IDENTITY_INSERT [dbo].[Privilege] OFF
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (2, N'Mineral Haze', 11800, 44, 300, N'Infamous.png', N'Стойкий, проявляющийся загар. Тонизирующий крем для загара с технологией Body Blush , эффектом мерцания и комплексного бронзирования.', N'Бронзаторы', 1)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (3, N'HOT! Black', 9600, 16, 250, N'HOT! Black.jpg', N'Витаминный коктейль для загара с исключительной технологией активации меланина и комплексным бронзированием.', N'Усилители загара', 1)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (4, N'JWOWW Shore Nights', 9500, 8, 400, N'JWOWW Shore Nights.jpg', N'Интенсивно восстанавливающий лосьон-активатор с комплексным бронзированием пролонгированного действия.', N'Бронзаторы', 1)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (5, N'JWOWW Stunning', 5000, 12, 300, N'JWOWW Stunning.png', N'Гибридный усилитель на минеральной основе с антивозрастным комплексом.', N'Защита от солнца', 1)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (6, N'Hemp Nation CARAMEL BRULEE LATTE', 2600, 23, 550, N'Hemp Nation CARAMEL BRULEE LATTE.png', N'Профессиональный увлажняющий лосьон для ежедневного применения на основе масла семян конопли с тонизирующим комплексом. Содержит Карамель, освежает оттенок загара.', N'Прочее', 1)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (7, N'Tan - Tekton Face', 2400, 17, 300, N'Tan - Tekton Face.jpg', N'Лосьон для загара в солярии для лица.', N'Усилители загара', 3)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (8, N'Coast Optimizer Step 2', 3000, 8, 300, N'Coast Optimizer Step 2.jpg', N'Лосьон-усилитель загара в солярии с натуральными бронзаторами для мужчин. Линия Coast ™ содержит в своем составе природные ингредиенты по уходу за кожей, которые способствуют приобретению интенсивного и темного загара.', N'Усилители загара', 3)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (9, N'Glow Daze', 3300, 15, 30, N'Glow Daze.png', N'Сенсационный иллюминайзер для лица и тела со светорассеивающей технологией.', N'Прочее', 6)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (10, N'Star Crossed', 1800, 70, 15, N'Star Crossed.png', N'Смягчающая эмульсия-активатор с комплексом, подчеркивающим естественное сияние кожи.', N'Прочее', 6)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (11, N'Axis Optimizer Step 2', 8400, 7, 200, N'Axis Optimizer Step 2.jpg', N'Лосьон для загара в солярии для всех типов кожи. В основе всей коллекции Axis - революционная формула глубокого увлажнения кожи. Ведь ключ к великолепной кожи - это регулярный и качественный уход. А главная задача любого уходового продукта - это глубокое увлажнение.', N'Усилители загара', 3)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Price], [Count], [Volume], [Photo], [Description], [ProductType], [ManufacturerId]) VALUES (12, N'Axis Tan Extender Step 3', 2600, 4, 4790, N'Axis Tan Extender Step 3.jpg', N'Коллекция революционных продуктов Axis - это великолепный уход за кожей, а во главе этого ухода - глубокое увлажнение.', N'Прочее', 3)
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[PurchaseProduct] ON 

INSERT [dbo].[PurchaseProduct] ([PurchaseProductId], [Count], [TotalPrice], [TimeOfPurchase], [SertificateId], [ProductId]) VALUES (1, 1, 9500, CAST(N'2021-06-11T11:21:19.000' AS DateTime), NULL, 4)
SET IDENTITY_INSERT [dbo].[PurchaseProduct] OFF
SET IDENTITY_INSERT [dbo].[PurchaseSertificate] ON 

INSERT [dbo].[PurchaseSertificate] ([SertificateId], [TimeOfActivation], [SertificateStatus], [SertificateTypeId],[RestSum]) VALUES (1, CAST(N'2021-06-11T11:22:53.000' AS DateTime), 0, 2,3000)
SET IDENTITY_INSERT [dbo].[PurchaseSertificate] OFF
SET IDENTITY_INSERT [dbo].[Record] ON 

INSERT [dbo].[Record] ([RecordId], [DatefApplication], [ApplicationView], [PhoneNumber], [Comment], [TimeOfRecord], [RecordStatus], [MasterId], [ClientId]) VALUES (1, CAST(N'2021-06-10T00:00:00.000' AS DateTime), 1, N'89706876987', N'', CAST(N'2021-06-11T11:23:49.000' AS DateTime), N'Активен', 1, 1)
SET IDENTITY_INSERT [dbo].[Record] OFF
SET IDENTITY_INSERT [dbo].[SertificateType] ON 

INSERT [dbo].[SertificateType] ([SertificateTypeId], [SertificateTypeName], [Price], [Description]) VALUES (1, N'Базовый   ', 1000, N'1 месяц с момента активации')
INSERT [dbo].[SertificateType] ([SertificateTypeId], [SertificateTypeName], [Price], [Description]) VALUES (2, N'Подарочный', 3000, N'1 месяц с момента активации')
INSERT [dbo].[SertificateType] ([SertificateTypeId], [SertificateTypeName], [Price], [Description]) VALUES (3, N'Люксовый  ', 5000, N'1 месяц с момента активации')
SET IDENTITY_INSERT [dbo].[SertificateType] OFF
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([ServiceId], [ServiceName], [Price], [Contraindications], [Description]) VALUES (1, N'Моментальный загар', 3000, N'Болезни кожи, заболевания щитовидной железы, беременность', N'Сохранение результата на 5-7 дней')
INSERT [dbo].[Service] ([ServiceId], [ServiceName], [Price], [Contraindications], [Description]) VALUES (2, N'Солярий', 1500, N'Аллергия на ультрафиолетовые лучи, склонность к пигментации кожи', N'Солярий хорош не только тем, что придает нашей коже приятный загорелый оттенок: между природным загаром и загаром в солярии различий нет, в том и другом случае происходит естественный процесс загара кожи.')
SET IDENTITY_INSERT [dbo].[Service] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [Email], [Password], [Role]) VALUES (1, N'admin', N'0000', N'Админ')
INSERT [dbo].[User] ([UserId], [Email], [Password], [Role]) VALUES (2, N'malia@mail.ru', N'1111', N'Пользователь')
INSERT [dbo].[User] ([UserId], [Email], [Password], [Role]) VALUES (3, N'1@1.ru', N'1', N'Пользователь')
INSERT [dbo].[User] ([UserId], [Email], [Password], [Role]) VALUES (4, N'0@0.ru', N'0', N'Пользователь')
INSERT [dbo].[User] ([UserId], [Email], [Password], [Role]) VALUES (5, N'adm@mail.ru', N'090203Ad', N'Пользователь')
SET IDENTITY_INSERT [dbo].[User] OFF
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_Privilege] FOREIGN KEY([PrivilegeId])
REFERENCES [dbo].[Privilege] ([PrivilegeId])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_Privilege]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_User]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Manufacturer] FOREIGN KEY([ManufacturerId])
REFERENCES [dbo].[Manufacturer] ([ManufacturerId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Manufacturer]
GO
ALTER TABLE [dbo].[ProvisionService]  WITH CHECK ADD  CONSTRAINT [FK_ProvisionService_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[ProvisionService] CHECK CONSTRAINT [FK_ProvisionService_Product]
GO
ALTER TABLE [dbo].[ProvisionService]  WITH CHECK ADD  CONSTRAINT [FK_ProvisionService_Record] FOREIGN KEY([RecordId])
REFERENCES [dbo].[Record] ([RecordId])
GO
ALTER TABLE [dbo].[ProvisionService] CHECK CONSTRAINT [FK_ProvisionService_Record]
GO
ALTER TABLE [dbo].[ProvisionService]  WITH CHECK ADD  CONSTRAINT [FK_ProvisionService_Sertificate] FOREIGN KEY([SertificateId])
REFERENCES [dbo].[PurchaseSertificate] ([SertificateId])
GO
ALTER TABLE [dbo].[ProvisionService] CHECK CONSTRAINT [FK_ProvisionService_Sertificate]
GO
ALTER TABLE [dbo].[ProvisionService]  WITH CHECK ADD  CONSTRAINT [FK_ProvisionService_Service] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([ServiceId])
GO
ALTER TABLE [dbo].[ProvisionService] CHECK CONSTRAINT [FK_ProvisionService_Service]
GO
ALTER TABLE [dbo].[PurchaseProduct]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_Sertificate] FOREIGN KEY([SertificateId])
REFERENCES [dbo].[PurchaseSertificate] ([SertificateId])
GO
ALTER TABLE [dbo].[PurchaseProduct] CHECK CONSTRAINT [FK_Purchase_Sertificate]
GO
ALTER TABLE [dbo].[PurchaseProduct]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseProduct_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[PurchaseProduct] CHECK CONSTRAINT [FK_PurchaseProduct_Product]
GO
ALTER TABLE [dbo].[PurchaseSertificate]  WITH CHECK ADD  CONSTRAINT [FK_Sertificate_SertificateType] FOREIGN KEY([SertificateTypeId])
REFERENCES [dbo].[SertificateType] ([SertificateTypeId])
GO
ALTER TABLE [dbo].[PurchaseSertificate] CHECK CONSTRAINT [FK_Sertificate_SertificateType]
GO
ALTER TABLE [dbo].[Record]  WITH CHECK ADD  CONSTRAINT [FK_Record_Client] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([ClientId])
GO
ALTER TABLE [dbo].[Record] CHECK CONSTRAINT [FK_Record_Client]
GO
ALTER TABLE [dbo].[Record]  WITH CHECK ADD  CONSTRAINT [FK_Record_Master] FOREIGN KEY([MasterId])
REFERENCES [dbo].[Master] ([MasterId])
GO
ALTER TABLE [dbo].[Record] CHECK CONSTRAINT [FK_Record_Master]
GO
USE [master]
GO
ALTER DATABASE [SunShimmer] SET  READ_WRITE 
GO
