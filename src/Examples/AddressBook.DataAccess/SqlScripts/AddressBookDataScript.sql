-- Make sure you run the create script first.

USE [AddressBook_RestCake]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PhoneType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[PhoneType] ON
INSERT [dbo].[PhoneType] ([ID], [Description]) VALUES (1, N'Home')
INSERT [dbo].[PhoneType] ([ID], [Description]) VALUES (2, N'Work')
INSERT [dbo].[PhoneType] ([ID], [Description]) VALUES (3, N'Mobile')
INSERT [dbo].[PhoneType] ([ID], [Description]) VALUES (4, N'Home Fax')
INSERT [dbo].[PhoneType] ([ID], [Description]) VALUES (5, N'Work Fax')
INSERT [dbo].[PhoneType] ([ID], [Description]) VALUES (6, N'Pager')
INSERT [dbo].[PhoneType] ([ID], [Description]) VALUES (7, N'Other')
SET IDENTITY_INSERT [dbo].[PhoneType] OFF
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Fname] [nvarchar](50) NULL,
	[Lname] [nvarchar](50) NULL,
	[Title] [nvarchar](50) NULL,
	[Company] [nvarchar](50) NULL,
	[Birthday] [datetime] NULL,
	[Notes] [nvarchar](2000) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Person] ON
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (3, N'Randal', N'Garza', N'Mr', N'Rapvenin Holdings ', CAST(0x00007ED400D2A9B0 AS DateTime), NULL, CAST(0x000083C70057F825 AS DateTime), CAST(0x00006C28013A1B52 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (6, N'Katie', N'Cantu', NULL, N'Cipnipegistor Direct ', NULL, NULL, CAST(0x0000584B0130A00D AS DateTime), CAST(0x00009DC90120B10A AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (7, N'Marlon', N'Campbell', NULL, N'Varglibex  Group', NULL, N'Quad rarendum habitatio quoque plorum in volcans essit.  Pro linguens imaginator pars fecit.  Et quad ut novum eggredior.  Longam, e gravis delerium.  Versus esset in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et bono quorum glavans e gravis delerium.  Versus esset in volcans essit.  Pro linguens non quo plorum fecundio, et plurissimum parte brevens, non trepicandor si nomen transit. Pro linguens non trepicandor si quad ', CAST(0x000061CB012355C8 AS DateTime), CAST(0x000084F30150C5D9 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (8, N'Gail', N'Warner', N'Dr.', NULL, CAST(0x0000941600568470 AS DateTime), NULL, CAST(0x00008E91015F3ABC AS DateTime), CAST(0x0000677800580EEA AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (9, N'Claudia', N'Ewing', NULL, NULL, NULL, NULL, CAST(0x000079B000CBAC4E AS DateTime), CAST(0x0000836500F22E21 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (10, N'Elton', N'Mullins', N'Ms.', NULL, NULL, NULL, CAST(0x00005FF90122830B AS DateTime), CAST(0x00007CE8014B5E01 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (11, N'Tom', N'Walton', NULL, N'Monjubor  ', NULL, N'Sed quad fecit, non quo plorum fecundio, et plurissimum parte brevens, non trepicandor si quad ut novum eggredior.  Longam, e gravis delerium.  Versus esset in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et bono quorum glavans e funem.  Quad rarendum habitatio quoque plorum in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et quis gravis delerium.  Versus esset in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et plurissimum parte brevens, non apparens vantis. Sed quad fecit, non quo linguens non trepicandor si nomen novum eggredior.  Longam, e funem.  Quad rarendum habitatio quoque plorum in volcans essit.  Pro linguens non trepicandor si quad fecit, non quo linguens imaginator pars fecit.  Et quad fecit, non trepicandor si nomen transit. Versus esset in volcans essit.  Pro ', CAST(0x0000533F0118F86F AS DateTime), CAST(0x00008D0E00A4BC8F AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (12, N'Cheri', N'Gould', NULL, NULL, NULL, N'Pro linguens imaginator pars fecit.  Et quad fecit, non quo plorum fecundio, et nomen novum eggredior.  Longam, e gravis delerium.  Versus esset in volcans essit.  Pro linguens imaginator pars fecit.  Et quad fecit, non quo plorum in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et pladior venit.  Tam quo, et quis gravis et nomen transit. Sed quad ut novum eggredior.  Longam, e gravis delerium.  Versus esset in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et pladior venit.  Tam quo, et plurissimum parte brevens, non apparens vantis. Sed quad fecit, non quo linguens imaginator pars fecit.  Et quad ut ', CAST(0x0000859301702D16 AS DateTime), CAST(0x0000745F003824F1 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (13, N'Hollie', N'Mays', NULL, NULL, NULL, N'Sed quad fecit, non quo linguens imaginator pars fecit.  Et quad ut novum vobis homo, si nomen transit. Multum gravum et nomen transit. Et quad fecit, non apparens vantis. Sed quad estis vobis regit, et nomen transit. Id eudis quo plorum fecundio, et quis gravis delerium.  Versus esset in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et quis gravis et pladior venit.  Tam quo, et nomen transit. Tam quo, et plurissimum parte brevens, non trepicandor si quad fecit, non trepicandor si nomen novum vobis homo, si quad ut novum vobis homo, ', CAST(0x00005ECA008266AD AS DateTime), CAST(0x000094BB003EEF5F AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (14, N'Ramona', N'Clarke', N'Ms.', N'Surquestan Holdings Inc', NULL, N'Id eudis quo linguens non quo linguens imaginator pars fecit.  Et quad fecit, non quo linguens imaginator pars fecit.  Et quad fecit, non apparens vantis. Sed quad fecit, non trepicandor si nomen transit. Tam quo, et quis gravis delerium.  Versus esset in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior ', CAST(0x000051A6007F3B69 AS DateTime), CAST(0x00007A7801109E44 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (15, N'Joey', N'Mueller', NULL, N'Frohupistor  ', CAST(0x000088C60142FF80 AS DateTime), NULL, CAST(0x0000977800AA4E91 AS DateTime), CAST(0x00007FED012EBFD1 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (16, N'Josh', N'Maldonado', N'Dr.', N'Tupsapor  Inc', CAST(0x000079BF01126230 AS DateTime), NULL, CAST(0x00006E1C008497B3 AS DateTime), CAST(0x000050A600223521 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (17, N'Courtney', N'Marsh', N'Miss.', NULL, NULL, NULL, CAST(0x00007A5A00CD5D65 AS DateTime), CAST(0x00004CDF006B8863 AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (18, N'Benjamin', N'Hardin', NULL, N'Parfropan  Group', CAST(0x00008A9B007DCB20 AS DateTime), NULL, CAST(0x00008E830161D9FF AS DateTime), CAST(0x000070B20144C65A AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (19, N'Tyrone', N'Garrison', NULL, N'Emfropollower Holdings Corp.', NULL, NULL, CAST(0x0000863A00CB251E AS DateTime), CAST(0x0000885A006D59BD AS DateTime))
INSERT [dbo].[Person] ([ID], [Fname], [Lname], [Title], [Company], [Birthday], [Notes], [DateCreated], [DateModified]) VALUES (20, N'Josh', N'James', NULL, N'Wincadazz Direct Company', CAST(0x00007BCB013A7BD0 AS DateTime), NULL, CAST(0x00004CE30162EDFF AS DateTime), CAST(0x00006B45010DAFE1 AS DateTime))
SET IDENTITY_INSERT [dbo].[Person] OFF
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Group] ON
INSERT [dbo].[Group] ([ID], [Name]) VALUES (1, N'Lydia74')
INSERT [dbo].[Group] ([ID], [Name]) VALUES (2, N'Danny711')
INSERT [dbo].[Group] ([ID], [Name]) VALUES (3, N'Gavin61')
INSERT [dbo].[Group] ([ID], [Name]) VALUES (4, N'Joe')
INSERT [dbo].[Group] ([ID], [Name]) VALUES (5, N'Eric2')
SET IDENTITY_INSERT [dbo].[Group] OFF
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EmailType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[EmailType] ON
INSERT [dbo].[EmailType] ([ID], [Description]) VALUES (1, N'Personal')
INSERT [dbo].[EmailType] ([ID], [Description]) VALUES (2, N'Work')
SET IDENTITY_INSERT [dbo].[EmailType] OFF
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Email](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[Email] [nvarchar](320) NOT NULL,
	[EmailTypeID] [int] NOT NULL,
 CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Email] ON
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (1, 15, N'wyole.xpemygdud@pdoklgl.jyjzsw.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (2, 17, N'fxpb.tdjwiknjv@gmqdll.org', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (3, 16, N'dffn.pgjywob@jk-px-.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (4, 12, N'cuvljcst03@psjsdd.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (6, 12, N'eqelgan@fewiod.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (7, 19, N'chvzh.cuuk@imzogn.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (8, 9, N'vpkyeqec9@yyiqwviyt.kyjwsw.net', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (9, 20, N'qxshrkb@ndluvj.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (10, 6, N'xksmc@utlflh.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (11, 6, N'jrhxpuwo.oyenmefww@oetkah.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (12, 10, N'tbvdrmt3@xdwj-v.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (13, 13, N'hzplksu4@qhecsv.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (14, 10, N'tymogcnl42@ztmk.yrgrgu.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (15, 20, N'pytrfifo0@fieebhlt.lrhboi.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (17, 18, N'iwpmviq2@qwzjuahjn.zlicex.org', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (18, 20, N'yeccjn.jdbbqhvglw@hgzsvs.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (19, 14, N'dsdsbcgb4@tqdi.yrqbqi.org', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (20, 7, N'hrfjgq.wraianau@jlwo.vkjtdm.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (21, 17, N'usmvpwri.mwzdz@rwwuzps.-jyvwm.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (22, 17, N'pgqn.xoonqkr@kzimpd.org', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (23, 20, N'cuco5@oaiwix.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (25, 14, N'xlvbni.hvcbqh@ahcmae.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (26, 11, N'qmhhjbl.dinneyqmk@kuxsp.gbkfns.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (27, 19, N'kbbng28@dixj.zqc-uv.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (28, 14, N'souiky.xxtxpz@ldykt.misyfx.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (29, 11, N'uolmv.bxxxjgl@wamhoqx.vfakpk.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (32, 10, N'fijn2@emougy.net', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (33, 6, N'gzht33@vtvodor.bbryqq.com', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (34, 20, N'ugifrg.lmtf@-dwjke.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (35, 13, N'iackv.hjqjumbae@dubqul.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (36, 16, N'bncgfetd.cglwvddc@pzcrsw.org', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (38, 8, N'ievpgc6@xbqqchvkq.srxocl.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (39, 7, N'joivo278@qlcwal.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (40, 20, N'qdhy.pmybcpkyh@bmmpwo.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (41, 11, N'ashoa.jcefhk@nohrubf.dnbyzo.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (42, 15, N'oifum@kfhtah.com', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (43, 3, N'cabpxdru.zyorutlrzf@eqkd.vwybfr.net', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (44, 6, N'phdr9@yyztkp.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (45, 19, N'ifufvl@y--tgq.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (46, 16, N'lhzurhk.ldokhq@lbrtvy.net', 2)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (47, 7, N'ramfesi104@muwttp.org', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (48, 10, N'rkiwuo63@ahxsmtv.dcpgiy.com', 1)
INSERT [dbo].[Email] ([ID], [PersonID], [Email], [EmailTypeID]) VALUES (49, 3, N'xbeh309@tyun.idmfxc.org', 1)
SET IDENTITY_INSERT [dbo].[Email] OFF
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[Address1] [nvarchar](100) NULL,
	[Address2] [nvarchar](100) NULL,
	[City] [nvarchar](100) NULL,
	[State] [nvarchar](100) NULL,
	[Zip] [nvarchar](50) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Address] ON
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (2, 10, N'162 Clarendon Freeway', N'Washington Building', N'Anchorage', N'Wyoming', N'95193', CAST(0x000078F500053124 AS DateTime), CAST(0x00008FFA01690F02 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (4, 20, N'477 Old Parkway', N'Sales Group', N'Boston', N'Massachusetts', N'24087', CAST(0x00008D6F008B8228 AS DateTime), CAST(0x00004FB70134D801 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (8, 3, N'41 East First Freeway', N'Roosevelt Building', N'Tacoma', N'Minnesota', N'85105', CAST(0x00006F1B000DA8E6 AS DateTime), CAST(0x000096780091FF17 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (9, 11, N'110 Oak Blvd.', N'Washington Building', N'Charlotte', N'Mississippi', N'89925', CAST(0x0000841900F79C71 AS DateTime), CAST(0x00008DCE011E1E44 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (10, 7, N'871 West Milton Road', N' Department', N'Albuquerque', N'Pennsylvania', N'24447', CAST(0x000071C400CE26B3 AS DateTime), CAST(0x00008EB300F701A9 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (11, 8, N'794 Green First Avenue', N'APT 4', N'Buffalo', N'Idaho', N'30402', CAST(0x00008381012BE2DE AS DateTime), CAST(0x00006ED900B7A6FB AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (12, 7, N'436 New Way', N'Post Sales Department', N'Lincoln', N'New Jersey', N'96591', CAST(0x000057830055B9B7 AS DateTime), CAST(0x000094C500A93395 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (13, 9, N'188 Rocky Fabien Street', NULL, N'Anchorage', N'Maryland', N'09309', CAST(0x0000870B004ED437 AS DateTime), CAST(0x00006E85000B5CE9 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (15, 11, N'471 Oak Blvd.', N'1st Floor', N'Phoenix', N'South Carolina', N'05891', CAST(0x000059E500BE6806 AS DateTime), CAST(0x000090D10142D94C AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (17, 19, N'100 White Nobel Blvd.', N'7th Floor', N'Indianapolis', N'Nevada', N'47310', CAST(0x000069C90175CE93 AS DateTime), CAST(0x00008AC50113F994 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (18, 6, N'58 East Clarendon Blvd.', N'Appartment 60', N'Hialeah', N'Mississippi', N'19802', CAST(0x000083BF0025DB35 AS DateTime), CAST(0x000065EE0008C790 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (19, 13, N'137 West Green Clarendon Freeway', N'1st Floor', N'Richmond', N'South Carolina', N'81424', CAST(0x00008D7F002D6C57 AS DateTime), CAST(0x00008F9E015B22F6 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (20, 17, N'30 South White Cowley Freeway', N'Suite 6', N'Fresno', N'Utah', N'92930', CAST(0x00007DA90168C696 AS DateTime), CAST(0x00004D9401138872 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (21, 15, N'25 Rocky First Street', N'Suite 5799', N'El Paso', N'Louisiana', N'20592', CAST(0x00007BCE0014A225 AS DateTime), CAST(0x0000863B0008F766 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (22, 15, N'983 Oak Boulevard', N'Appartment 6', N'Louisville', N'Tennessee', N'49926', CAST(0x000064BB0070F7F4 AS DateTime), CAST(0x000077FB008FEA22 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (23, 10, N'847 Rocky New Drive', N'4th Floor', N'Toledo', N'North Dakota', N'88680', CAST(0x00004C15003F9E82 AS DateTime), CAST(0x00005957017D775E AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (24, 14, N'503 Green Clarendon Blvd.', N'Washington Building', N'Dallas', N'Michigan', N'39220', CAST(0x000055B70051A4D6 AS DateTime), CAST(0x0000712000CC84FC AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (25, 14, N'83 Rocky Second Parkway', N'Einstein Building', N'Arlington', N'California', N'04941', CAST(0x0000623600E516C7 AS DateTime), CAST(0x000069E700570264 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (26, 16, N'43 Rocky Clarendon Boulevard', N'Suite 8560', N'Cincinnati', N'California', N'11768', CAST(0x000069DB00442B7E AS DateTime), CAST(0x00004DDB01384005 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (27, 12, N'393 West Green Milton Road', N'Suite 9497', N'Wichita', N'Maine', N'21696', CAST(0x000052E200594AEF AS DateTime), CAST(0x000096580080613C AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (28, 19, N'827 West White Nobel Street', N'Suite 641', N'Colorado', N'Florida', N'65523', CAST(0x00006B6A00F4DF37 AS DateTime), CAST(0x00004D91010E9D38 AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (29, 18, N'390 Old Freeway', N'Truman Building', N'Virginia Beach', N'North Carolina', N'31180', CAST(0x00005BA100F6ED9C AS DateTime), CAST(0x0000714D0169CDBE AS DateTime))
INSERT [dbo].[Address] ([ID], [PersonID], [Address1], [Address2], [City], [State], [Zip], [DateCreated], [DateModified]) VALUES (30, 20, N'54 Rocky Milton Way', N'1st Floor', N'Toledo', N'Kansas', N'40968', CAST(0x00009346013E0CAE AS DateTime), CAST(0x0000840100189F27 AS DateTime))
SET IDENTITY_INSERT [dbo].[Address] OFF
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phone](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[PhoneTypeID] [int] NOT NULL,
 CONSTRAINT [PK_Phone] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Phone] ON
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (1, 15, N'659-450-1667', 6)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (2, 17, N'837978-4295', 6)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (3, 16, N'057-272-7461', 6)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (4, 12, N'7762428652', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (6, 12, N'506-661-6396', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (7, 19, N'610-1868333', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (8, 9, N'085-7015440', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (9, 20, N'3079156400', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (10, 6, N'3378786028', 2)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (11, 6, N'745856-9254', 3)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (12, 10, N'390-166-5128', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (13, 13, N'896193-7634', 5)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (14, 10, N'523-6001835', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (15, 20, N'628-9663683', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (17, 18, N'3585838576', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (18, 20, N'0169618063', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (19, 14, N'088871-7932', 5)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (20, 7, N'605-720-7405', 3)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (21, 17, N'429-0772940', 6)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (22, 17, N'029-8558832', 6)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (23, 20, N'476501-9287', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (25, 14, N'134-5350163', 5)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (26, 11, N'577-1879844', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (27, 19, N'889-5754114', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (28, 14, N'3850982683', 5)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (29, 11, N'781-1238185', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (32, 10, N'912-5494950', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (33, 6, N'120998-9914', 3)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (34, 20, N'563677-3930', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (35, 13, N'657-840-9380', 5)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (36, 16, N'524-7594569', 6)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (38, 8, N'396-1591514', 3)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (39, 7, N'2812402527', 3)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (40, 20, N'154-5914629', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (41, 11, N'802-344-8910', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (42, 15, N'522-5113638', 6)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (43, 3, N'520-229-6127', 1)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (44, 6, N'571248-7949', 2)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (45, 19, N'616689-3274', 7)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (46, 16, N'484-5531494', 6)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (47, 7, N'270-556-7873', 3)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (48, 10, N'063-854-0790', 4)
INSERT [dbo].[Phone] ([ID], [PersonID], [PhoneNumber], [PhoneTypeID]) VALUES (49, 3, N'184-513-8238', 2)
SET IDENTITY_INSERT [dbo].[Phone] OFF
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonGroup](
	[PersonID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
 CONSTRAINT [PK_PersonGroup] PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (3, 4)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (6, 2)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (7, 2)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (7, 3)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (8, 2)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (9, 5)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (10, 1)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (10, 2)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (11, 4)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (11, 5)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (12, 3)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (13, 3)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (14, 2)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (14, 3)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (15, 4)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (15, 5)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (16, 5)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (17, 3)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (17, 5)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (18, 1)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (19, 3)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (19, 5)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (20, 1)
INSERT [dbo].[PersonGroup] ([PersonID], [GroupID]) VALUES (20, 2)
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Website](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[Url] [nvarchar](2000) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Website] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Website] ON
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (1, 15, N'http://obba.webz4/oeiwt/foqjw/dkggr/gnb.html', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (2, 17, N'http://gtly.locals49/wrojh/lxcrt/pxw.htm', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (3, 16, N'http://dyam.localc22/flipx/odnzr/ehpam/bgkrg.html', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (4, 12, N'http://abql.webq/fkjsn/ezehj.htm', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (6, 12, N'http://djgf.net3/iwejn/gftcd/mshqf/wcoyd/uig.php', N'Id eudis quo plorum fecundio, et plurissimum parte brevens, non quo plorum in dolorum cognitio, travissimantor quantare sed quartu manifestum ')
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (7, 19, N'http://vsbj.net/tewnj/kawnx/pfmxo/tkpe.htm', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (8, 9, N'http://qykb.local/qorfa/mjuls/ufn.php', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (9, 20, N'http://ykzy.local/lgjgx/knyqu/ovgau/osv.php', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (10, 6, N'http://miz.net60/ccfan/ckgg.aspx', N'Tam quo, et plurissimum parte brevens, non trepicandor si nomen novum vobis homo, si quad estis vobis homo, si nomen transit. Id eudis quo plorum fecundio, et pladior venit.  ')
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (11, 6, N'http://tkp.local73/sjwyy/wfhqa/lsa.aspx', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (12, 10, N'http://glhpm.local5/jmfkf/kweop.aspx', N'Tam quo, et quis gravis delerium.  Versus esset in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et pladior venit.  Tam quo, et plurissimum parte brevens, non trepicandor si quad fecit, non apparens vantis. Sed quad estis vobis homo, si nomen novum eggredior.  Longam, e gravis et plurissimum parte brevens, non apparens vantis. ')
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (13, 13, N'http://bygt.local/xoxjm/kdjif/vvlqp.htm', N'Pro linguens imaginator pars fecit.  Et quad fecit, non trepicandor si quad fecit, non trepicandor si quad ut novum vobis homo, si quad ut novum eggredior.  Longam, e gravis et bono quorum glavans e gravis et quis gravis delerium.  Versus esset in dolorum cognitio, travissimantor quantare sed quartu manifestum ')
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (14, 10, N'http://evsrl.local7/qadlp/mvoe.htm', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (15, 20, N'http://fguog.web35/dexmr/rqess/xkgud.aspx', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (17, 18, N'http://vmo.net/zoslu/rmvnr.htm', N'Et quad fecit, non quo plorum in volcans essit.  Pro linguens imaginator pars fecit.  Et quad fecit, non quo linguens non trepicandor si quad ut novum vobis ')
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (18, 20, N'http://ikqs.webw/gqwza/anlmy/ujzva/heqxf/ncyxj.aspx', N'Quad rarendum habitatio quoque plorum in dolorum cognitio, travissimantor quantare sed quartu manifestum egreddior estum.  Multum gravum et pladior venit.  Tam quo, et bono quorum glavans e gravis delerium.  Versus esset in volcans ')
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (19, 14, N'http://avylv.localq8/vslpg/lbdsz/jksmy/deqlx/vydys.html', NULL)
INSERT [dbo].[Website] ([ID], [PersonID], [Url], [Description]) VALUES (20, 7, N'http://jqqj.netm/txiwz/levss/vqkvk/hpmic/pisbg.htm', NULL)
SET IDENTITY_INSERT [dbo].[Website] OFF
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[usp_deletePerson]
@personID int
as

DELETE FROM dbo.Website
WHERE PersonID = @personID

-- Gets rid of orphaned Address records
DELETE FROM dbo.[Address]
WHERE PersonID = @personID

DELETE FROM dbo.Email
WHERE PersonID = @personID

DELETE FROM dbo.Phone
WHERE PersonID = @personID

DELETE FROM dbo.PersonGroup
WHERE PersonID = @personID

-- Now that all related data is gone, delete the person
DELETE FROM dbo.Person
WHERE ID = @personID
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Person]
GO
ALTER TABLE [dbo].[Email]  WITH CHECK ADD  CONSTRAINT [FK_Email_EmailType] FOREIGN KEY([EmailTypeID])
REFERENCES [dbo].[EmailType] ([ID])
GO
ALTER TABLE [dbo].[Email] CHECK CONSTRAINT [FK_Email_EmailType]
GO
ALTER TABLE [dbo].[Email]  WITH CHECK ADD  CONSTRAINT [FK_Email_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[Email] CHECK CONSTRAINT [FK_Email_Person]
GO
ALTER TABLE [dbo].[PersonGroup]  WITH CHECK ADD  CONSTRAINT [FK_PersonGroup_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[PersonGroup] CHECK CONSTRAINT [FK_PersonGroup_Group]
GO
ALTER TABLE [dbo].[PersonGroup]  WITH CHECK ADD  CONSTRAINT [FK_PersonGroup_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[PersonGroup] CHECK CONSTRAINT [FK_PersonGroup_Person]
GO
ALTER TABLE [dbo].[Phone]  WITH CHECK ADD  CONSTRAINT [FK_Phone_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[Phone] CHECK CONSTRAINT [FK_Phone_Person]
GO
ALTER TABLE [dbo].[Phone]  WITH CHECK ADD  CONSTRAINT [FK_Phone_PhoneType] FOREIGN KEY([PhoneTypeID])
REFERENCES [dbo].[PhoneType] ([ID])
GO
ALTER TABLE [dbo].[Phone] CHECK CONSTRAINT [FK_Phone_PhoneType]
GO
ALTER TABLE [dbo].[Website]  WITH CHECK ADD  CONSTRAINT [FK_Website_Person] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Person] ([ID])
GO
ALTER TABLE [dbo].[Website] CHECK CONSTRAINT [FK_Website_Person]
GO
