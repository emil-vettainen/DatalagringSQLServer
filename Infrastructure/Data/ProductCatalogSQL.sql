DROP TABLE Prices
DROP TABLE ProductCategories
DROP TABLE Products
DROP TABLE Categories
DROP TABLE Manufactures

CREATE TABLE Manufactures 
(
	Id int identity not null primary key,
	Manufacture nvarchar(100) not null unique
)

CREATE TABLE Categories
(
	Id int identity not null primary key,
	ParentCategoryId int not null references Categories(Id),
	CategoryName nvarchar(50) not null
)

CREATE TABLE Products 
(
	ArticleNumber nvarchar(200) not null primary key,
	ProductTitle nvarchar(100) not null,
	Description nvarchar(max),
	Specification nvarchar(max),
	ManufactureId int not null references Manufactures(Id)
)

CREATE TABLE ProductCategories
(
	ArticleNumber nvarchar(200) not null,
	CategoryId int not null references Categories(Id)
	primary key (ArticleNumber, CategoryId)
)

CREATE TABLE Prices 
(
	ArticleNumber nvarchar(200) not null primary key references Products(ArticleNumber),
	Price money not null,
)