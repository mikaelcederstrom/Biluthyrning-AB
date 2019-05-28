CREATE TABLE [dbo].[Cars]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[CarType] nvarchar(450) not null,
	[Registrationnumber] nvarchar(6) not null unique,
	[Kilometer] int not null,
	AvailableForRent bit not null,
	
)

CREATE TABLE [dbo].[CarCleaning]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	CarId int not null references Cars(Id),
	FlaggedForCleaningDate Date not null,
	CleaningDoneDate Date, 
	CleaningDone bit
)
CREATE TABLE [dbo].[CarService]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	CarId int not null references Cars(Id),
	FlaggedForServiceDate Date not null,
	ServiceDoneDate Date,
	ServiceDone bit
	
)
CREATE TABLE [dbo].[CarRetire]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	CarId int not null references Cars(Id),
	FlaggedForRetiringDate Date not null,
	RetiredDate Date,
	Retired bit
)

Insert into Cars(CarType, Registrationnumber, Kilometer, AvailableForRent)
values ('Small car', 'ABC123', 10, 1),
('Van', 'QWE123', 101, 1),
('Minibus', 'ABC456', 25, 1),
('Small car', 'ABC789', 104, 1),
('Van', 'QWE345', 2, 1),
('Minibus', 'QWE333', 154, 1),
('Small car', 'OKJ234', 88, 1),
('Van', 'DAE234', 78, 1),
('Minibus', 'HYT321', 58, 1)


create TABLE [dbo].[Customers]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[FirstName] nvarchar(max) not null,
	[LastName] nvarchar(max) not null,
	[PersonNumber] nvarchar(11) not null unique,
	[NumberOfOrders] int not null,
	[KilometersDriven] float not null,
	[MembershipLevel]  int      NOT NULL
)
create TABLE [dbo].[Orders]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	CarID int not null references Cars(ID),
	CustomerID int not null references Customers(Id),
	[KilometerBeforeRental] int not null,
	[RentalDate] date not null,
	[KilometerAfterRental] int not null,
	[ReturnDate] date not null,
	CarReturned bit not null,
	[Cost] float not null
)

create table [Events]
(
ID int Primary key identity not null,
EventType varchar(32) CHECK (EventType in ('Bil bokad', 'Bil återlämnad', 'Bil tvättad', 'Service av bil', 'Bil borttagen', 'Bil tillagd','Medlemskap uppgraderad', 'Användare skapad', 'Användare borttagen')) not null,
CarID int references Cars(Id) null ,
CustomerID int references Customers(Id) null,
BookingID int references Orders(Id) null,
[Date] DateTime not null
)
