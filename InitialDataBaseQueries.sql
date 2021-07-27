CREATE DATABASE ShopBridge;
GO

USE [ShopBridge]
GO

CREATE TABLE Products (
    ProductID int IDENTITY(1,1) PRIMARY KEY,
    ProductName varchar(255) NOT NULL UNIQUE,
    ProductDescription  varchar(500),
    Price money,
    Qty int NOT NULL
);
GO

INSERT INTO Products
VALUES ('Pen', 'Scrikss Trio 93 Glossy Black Multi pen Ball Point Pen Ball pen & Pencil With 0.5 mm Lead', 15,4);
INSERT INTO Products
VALUES ('Pencil', 'Bianyo Artist Quality Fine Art Drawing & Sketching Pencils', 5,12);


select * from products