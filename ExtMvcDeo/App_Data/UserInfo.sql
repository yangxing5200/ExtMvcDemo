CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NULL, 
    [Age] INT NULL, 
    [Major] INT NULL, 
    [FullName] NVARCHAR(50) NULL, 
    [Birthday] DATE NULL, 
    [Desc] NVARCHAR(MAX) NULL, 
    [Creator] INT NULL, 
    [CreateDate] DATETIME NULL, 
    [Modify] INT NULL, 
    [ModifyDate] DATETIME NULL
)
