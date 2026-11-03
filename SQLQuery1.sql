ALTER TABLE [dbo].[Order] 
ADD [DeliveryMethod] NVARCHAR(MAX) NULL,
    [PaymentMethod] NVARCHAR(MAX) NULL;
GO

-- Đổi tên trường AddressDelivery thành ShippingAddress
EXEC sp_rename '[dbo].[Order].[AddressDelivery]', 'ShippingAddress', 'COLUMN';
GO

-- 2. Bảng OrderDetail: Bổ sung trường tính toán TotalPrice
ALTER TABLE [dbo].[OrderDetail]
ADD [TotalPrice] AS ([Quantity] * [UnitPrice]) PERSISTED;
GO