--Creating database only if database is not created yet
IF DB_ID('Zadatak_45') IS NULL
CREATE DATABASE Zadatak_45
GO
USE Zadatak_45;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblStorage')
drop table tblStorage;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblProducts')
drop table tblProducts;

create table tblProducts (
ProductID int identity (1,1) primary key,
ProdName nvarchar (50),
ProdCode nvarchar(50),
Amount int,
Price int,
Stored Bit 
)

create table tblStorage(
StorageID int identity (1,1) primary key,
ProductID int,
Price int
)

Alter Table tblStorage
Add foreign key (ProductID) references tblProducts(ProductID);


