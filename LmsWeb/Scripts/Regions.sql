/****** Object:  Table [dbo].[Regions]    Script Date: 02/15/2007 15:52:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Regions](
	[ID] [uniqueidentifier] NULL,
	[Name] [nvarchar](250) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[CodesCommaSeparated] [nvarchar](250) COLLATE Cyrillic_General_CI_AS NULL,
 CONSTRAINT [IX_Regions] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b01','������� ��','kv,kcf')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b02','������� ��','od')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b03','�������� ��','cr')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b04','³������� ��','vn')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b05','��������� ��','lt')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b06','��������������� ��','dp')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b07','�������� ��','dn')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b08','����������� ��','zt')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b09','������������ ��','uz')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b10','��������� ��','zp')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b11','�����-���������� ��','if')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b12','ʳ������������ ��','kr')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b13','��������� ��','lg')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b14','�������� ��','lv')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b15','����������� ��','mk')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b16','���������� ��','pl')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b17','г�������� ��','rv')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b18','������� ��','sm')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b19','����������� ��','tr')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b20','��������� ��','kh')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b21','���������� ��','ks')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b22','����������� ��','km')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b23','��������� ��','ck')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b24','���������� ��','cn')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b25','���������� ��','cv')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES('bbbbbbbb-d9b7-42e1-b6a1-ffffb0e29b26','�������������� ��','sb')
INSERT INTO [dbo].[Regions]([ID],[Name],[CodesCommaSeparated])
VALUES(NULL,'�� �����',NULL)