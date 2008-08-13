/****** Object:  Table [dbo].[Roles]    Script Date: 02/15/2007 15:12:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [uniqueidentifier] NULL,
	[Name] [nvarchar](250) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[CodeName] [nvarchar](250) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IsGlobal] [bit] NOT NULL,
 CONSTRAINT [IX_Roles] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[Roles] ([ID],[Name],[CodeName],[IsGlobal])
VALUES	('aaaaaaaa-d9b7-42e1-b6a1-ffffb0e29b01','�����������','Administrator',1)

INSERT INTO [dbo].[Roles] ([ID],[Name],[CodeName],[IsGlobal])
VALUES	('aaaaaaaa-d9b7-42e1-b6a1-ffffb0e29b02','�����','Tutor',1)

INSERT INTO [dbo].[Roles] ([ID],[Name],[CodeName],[IsGlobal])
VALUES	('aaaaaaaa-d9b7-42e1-b6a1-ffffb0e29b03','����� �����������','TutorRegional',0)

INSERT INTO [dbo].[Roles] ([ID],[Name],[CodeName],[IsGlobal])
VALUES	('aaaaaaaa-d9b7-42e1-b6a1-ffffb0e29b04','������-�����','BusinessTutor',1)

INSERT INTO [dbo].[Roles] ([ID],[Name],[CodeName],[IsGlobal])
VALUES	('aaaaaaaa-d9b7-42e1-b6a1-ffffb0e29b05','������-����� �����������','BusinessTutorRegional',0)

INSERT INTO [dbo].[Roles] ([ID],[Name],[CodeName],[IsGlobal])
VALUES	(NULL,'�������','Student',0)
