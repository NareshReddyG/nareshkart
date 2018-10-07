CREATE TABLE [dbo].[ShippingDetails] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [UserId]          NVARCHAR (MAX) NULL,
    [FirstName]       NVARCHAR (MAX) NOT NULL,
    [LastName]        NVARCHAR (MAX) NULL,
    [Email]           NVARCHAR (50)  NULL,
    [PhoneNumber]     VARCHAR (20)   NULL,
    [ShippingAddress] VARCHAR (MAX)  NULL,
    [Country]         INT            NULL,
    [Street]          VARCHAR (MAX)  NULL,
    [LandMark]        VARCHAR (MAX)  NULL,
    [State]           VARCHAR (MAX)  NULL,
    [Zipcode]         VARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

