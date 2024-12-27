Use [Shopping_Cart]

Create Table tblProducts(ID INT Identity(1,1) Primary key, Name varchar(100), Image varchar(100),ActualPrice Decimal(20,2),
DiscountedPrice Decimal(18,2));

Insert into tblProducts(Name, Image, ActualPrice,DiscountedPrice)
Values('Bioderma New & Improved','assets/images/product_01.png',120.00,100.99);
Insert into tblProducts(Name, Image, ActualPrice,DiscountedPrice)
Values('Chanca Piedra New','assets/images/product_02.png',120.10,100.00);
Insert into tblProducts(Name, Image, ActualPrice,DiscountedPrice)
Values('Umcka Cold Care New','assets/images/product_03.png',130.20,119.00);
Insert into tblProducts(Name, Image, ActualPrice,DiscountedPrice)
Values('Cetyl Pure','assets/images/product_04.png',130.30,99.99);
Insert into tblProducts(Name, Image, ActualPrice,DiscountedPrice)
Values('CLA Core','assets/images/product_05.png',140.40,69.99);
Insert into tblProducts(Name, Image, ActualPrice,DiscountedPrice)
Values('Poo Pourri','assets/images/product_06.png',150.00,79.99);

CREATE TABLE tblCart
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

Insert into tblCart(ProductId, Quantity, Price,CreatedAt)
Values(19,2,49.99,GETDATE());
Insert into tblCart(ProductId, Quantity, Price,CreatedAt)
Values(20,3,99.99,GETDATE());

Select * FROM  tblCart;
Select * from tblProducts;