DROP TABLE ProductCategoryEntity
DROP TABLE CategoryEntity
DROP TABLE ProductPriceEntity
DROP TABLE ProductEntity
DROP TABLE ManufactureEntity


CREATE TABLE ManufactureEntity 
(
	Id int NOT NULL IDENTITY PRIMARY KEY,
	Manufacturers nvarchar(50) NOT NULL UNIQUE
)

CREATE TABLE ProductEntity 
(
	ArticleNumber nvarchar(200) NOT NULL PRIMARY KEY,
	ProductTitle nvarchar(100) NOT NULL,
	Ingress nvarchar(450) NOT NULL,
	Description nvarchar(max) NOT NULL,
	Specification nvarchar(max) NOT NULL,
	ManufactureId int NOT NULL,
	FOREIGN KEY (ManufactureId) REFERENCES ManufactureEntity(Id)
)


CREATE TABLE ProductPriceEntity 
(
	ArticleNumber nvarchar(200) NOT NULL PRIMARY KEY,
	Price money NOT NULL,
	FOREIGN KEY (ArticleNumber) REFERENCES ProductEntity(ArticleNumber)
)


CREATE TABLE CategoryEntity 
(
	Id int NOT NULL IDENTITY PRIMARY KEY,
	CategoryName nvarchar(50) NOT NULL,
	ParentCategoryId int,
	FOREIGN KEY (ParentCategoryId) REFERENCES CategoryEntity(Id) 
)

CREATE TABLE ProductCategoryEntity
(
	CategoryId int NOT NULL,
	ArticleNumber nvarchar(200) NOT NULL,
	PRIMARY KEY (CategoryId, ArticleNumber),
	FOREIGN KEY (CategoryId) REFERENCES CategoryEntity(Id),
	FOREIGN KEY (ArticleNumber) REFERENCES Productentity(ArticleNumber)
)