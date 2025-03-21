
CREATE TABLE [dbo].[Users] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Login]        NVARCHAR (50)  NOT NULL,
    [PasswordHash] NVARCHAR (100) NOT NULL,
    [Role]         NVARCHAR (20)  NOT NULL,
    [IsActive]     BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Login] ASC)
);


CREATE TABLE [dbo].[Records] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (200)  NOT NULL,
    [Artist]       NVARCHAR (100)  NOT NULL,
    [Publisher]    NVARCHAR (100)  NOT NULL,
    [TrackCount]   INT             NOT NULL,
    [Genre]        NVARCHAR (50)   NOT NULL,
    [ReleaseYear]  INT             NOT NULL,
    [CostPrice]    DECIMAL (10, 2) NOT NULL,
    [SellingPrice] DECIMAL (10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Records_Price] CHECK ([SellingPrice]>[CostPrice])
);


CREATE TABLE [dbo].[Sales] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [RecordId]     INT             NOT NULL,
    [SaleDate]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [CustomerName] NVARCHAR (100)  NOT NULL,
    [Price]        DECIMAL (10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Sales_Records] FOREIGN KEY ([RecordId]) REFERENCES [dbo].[Records] ([Id])
);



CREATE TABLE [dbo].[StockOperations] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [RecordId]      INT            NOT NULL,
    [OperationDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [OperationType] NVARCHAR (10)  NOT NULL,
    [Quantity]      INT            NOT NULL,
    [Reason]        NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockOperations_Records] FOREIGN KEY ([RecordId]) REFERENCES [dbo].[Records] ([Id]),
    CHECK ([OperationType]='OUT' OR [OperationType]='IN')
);



CREATE TABLE [dbo].[Promotions] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [RecordId]           INT            NOT NULL,
    [StartDate]          DATETIME       NOT NULL,
    [EndDate]            DATETIME       NOT NULL,
    [DiscountPercentage] DECIMAL (5, 2) NOT NULL,
    [Description]        NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Promotions_Records] FOREIGN KEY ([RecordId]) REFERENCES [dbo].[Records] ([Id]),
    CONSTRAINT [CK_Promotions_Date] CHECK ([EndDate]>[StartDate]),
    CHECK ([DiscountPercentage]>=(0) AND [DiscountPercentage]<=(100))
);


CREATE TABLE [dbo].[ReservedRecords] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [RecordId]     INT            NOT NULL,
    [CustomerName] NVARCHAR (100) NOT NULL,
    [ReserveDate]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [ExpireDate]   DATETIME       NOT NULL,
    [IsConfirmed]  BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ReservedRecords_Records] FOREIGN KEY ([RecordId]) REFERENCES [dbo].[Records] ([Id]),
    CONSTRAINT [CK_ReservedRecords_Date] CHECK ([ExpireDate]>[ReserveDate])
);


CREATE TABLE [dbo].[CustomerDiscounts] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [CustomerId]         INT             NOT NULL,
    [TotalSpent]         DECIMAL (15, 2) DEFAULT ((0)) NOT NULL,
    [DiscountPercentage] DECIMAL (5, 2)  DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_DiscountPercentage] CHECK ([DiscountPercentage]>=(0) AND [DiscountPercentage]<=(100))
);

