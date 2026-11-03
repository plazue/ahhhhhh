
-- Bảng User: lưu thông tin người dùng / tài khoản
CREATE TABLE [dbo].[User] (
    [Username]     NVARCHAR (255) NOT NULL,
    [Password]     NCHAR (50)     NOT NULL,
    [UserRole]     NCHAR (1)      NOT NULL, -- A: admin, C: customer
    [FullName]     NVARCHAR (100) NULL,
    [Email]        NVARCHAR (100) NULL,
    [Image]        NVARCHAR (255) NULL,
    [IsActive]     BIT            NULL,
    [Role]         INT            NULL,
    PRIMARY KEY CLUSTERED ([Username] ASC)
);

-- Bảng Customer: lưu thông tin khách hàng
CREATE TABLE [dbo].[Customer] (
    [CustomerID]       INT            IDENTITY (1, 1) NOT NULL,
    [CustomerName]     NVARCHAR (MAX) NOT NULL,
    [CustomerPhone]    NVARCHAR (15)  NOT NULL,
    [CustomerEmail]    NVARCHAR (MAX) NULL,
    [CustomerAddress]  NVARCHAR (MAX) NULL,
    [Username]         NVARCHAR (255) NOT NULL,
    [CustomerImage]    NVARCHAR (MAX) NULL,
    [IsActive]         BIT            NULL,
    [Role]             NVARCHAR (1)   NULL,
    [Password]         NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([CustomerID] ASC),
    CONSTRAINT [FK_User_Customer] FOREIGN KEY ([Username]) REFERENCES [dbo].[User] ([Username])
);

-- Bảng Category: lưu thông tin danh mục sản phẩm
CREATE TABLE [dbo].[Category] (
    [CategoryID]       INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName]     NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);

-- Bảng Product: lưu thông tin sản phẩm
CREATE TABLE [dbo].[Product] (
    [ProductID]          INT            IDENTITY (1, 1) NOT NULL,
    [CategoryID]         INT            NOT NULL,
    [ProductName]        NVARCHAR (MAX) NOT NULL,
    [ProductDecription]  NVARCHAR (MAX) NOT NULL,
    [ProductPrice]       DECIMAL (18, 2) NOT NULL,
    [ProductImage]       NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC),
    CONSTRAINT [FK_Pro_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([CategoryID])
);

-- Bảng Order: lưu thông tin đơn hàng của khách hàng
CREATE TABLE [dbo].[Order] (
    [OrderID]          INT            IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT            NOT NULL,
    [OrderDate]        DATE           NOT NULL,
    [TotalAmount]      DECIMAL (18, 2) NOT NULL,
    [PaymentStatus]    NVARCHAR (MAX) NULL,
    [AddressDelivery]  NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderID] ASC),
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID])
);

-- Bảng OrderDetail: lưu thông tin chi tiết các sản phẩm có trong đơn hàng
CREATE TABLE [dbo].[OrderDetail] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [ProductID]  INT             NOT NULL,
    [OrderID]    INT             NOT NULL,
    [Quantity]   INT             NOT NULL,
    [UnitPrice]  DECIMAL (18, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID]),
    FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Order] ([OrderID])
);

ALTER AUTHORIZATION ON DATABASE::MyStore TO sa;