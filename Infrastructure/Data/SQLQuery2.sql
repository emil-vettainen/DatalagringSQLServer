CREATE TABLE ProductPriceEntity 
(
	Id int NOT NULL IDENTITY PRIMARY KEY,
	Price money NOT NULL
)


CREATE TABLE ManufactureEntity 
(
	Id int NOT NULL IDENTITY PRIMARY KEY,
	ManufactureName nvarchar(50) NOT NULL UNIQUE
)


CREATE TABLE CategoryEntity 
(
	Id int NOT NULL IDENTITY PRIMARY KEY,
	CategoryName nvarchar(50) NOT NULL UNIQUE
)


CREATE TABLE ProductEntity 
(
	ArticleNumber nvarchar(200) NOT NULL PRIMARY KEY,
	Created Datetime2 NOT NULL,
	Modified Datetime2 NOT NULL,

	ProductPriceId int NOT NULL,
	FOREIGN KEY (ProductPriceId) REFERENCES ProductPriceEntity(Id),

	ManufactureId int NOT NULL,
	FOREIGN KEY (ManufactureId) REFERENCES ManufactureEntity(Id),

	CategoryId int NOT NULL,
	FOREIGN KEY(CategoryId) REFERENCES CategoryEntity(Id)
)


CREATE TABLE ProductInfoEntity 
(
	ArticleNumber nvarchar(200) NOT NULL PRIMARY KEY,
	FOREIGN KEY (ArticleNumber) REFERENCES ProductEntity(ArticleNumber),
	ProductTitle nvarchar(100) NOT NULL,
	Ingress nvarchar(450) NOT NULL,
	Description nvarchar(max) NOT NULL,
	Specification nvarchar(max) NOT NULL,
)