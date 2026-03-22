CREATE TABLE Products (
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    Name     NVARCHAR(200)  NOT NULL,
    Price    DECIMAL(18,2)  NOT NULL,
    Category NVARCHAR(100)  NOT NULL,
    InStock  BIT            NOT NULL DEFAULT 1
);