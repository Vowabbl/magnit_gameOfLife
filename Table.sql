CREATE TABLE Test2.dbo.GOLDatas (
  ID int IDENTITY,
  State nvarchar(max) NULL,
  StateDate datetime NOT NULL DEFAULT (getdate()),
  CONSTRAINT [PK_dbo.GOLDatas] PRIMARY KEY CLUSTERED (ID)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO