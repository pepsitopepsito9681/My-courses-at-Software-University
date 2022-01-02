INSERT INTO 
	[Towns]
	([Name])
	VALUES
	('Sofia'),
	('Plovdiv'),
	('Varna'),
	('Burgas')

INSERT INTO 
	[Addresses]
	([AddressText], [TownId])
	VALUES
	('Prohlada', 1),
	('Hristo Botev', 2)

INSERT INTO 
	[Departments]
	([Name])
	VALUES
	('Engineering'),
	('Marketing'),
	('Sales'),
	('Software Development'),
	('Quality Assurance')

INSERT INTO 
	[Employees]
	([FirstName], [MiddleName], [LastName], [JobTitle], [DepartmentId], [HireDate], [Salary])
	VALUES
	('Ivan', 'Ivanov' ,'Ivanov', '.NET Developer', 4, '01/02/2013', 3500.00),
	('Petar', 'Petrov' ,'Petrov', 'Senior Engineer', 1, '02/03/2004', 4000.00),
	('Maria', 'Petrova' ,'Ivanova', 'Intern', 5, '28/08/2016', 525.25),
	('Georgi', 'Teziev' ,'Ivanov', 'CEO', 3, '09/12/2007', 3000.00),
	('Peter', 'Pan' ,'Pan', 'Intern', 2, '28/08/2016', 599.88)

--DROP TABLE [Employees]

CREATE TABLE [Employees]
					(
						[Id]			INT PRIMARY KEY IDENTITY,
						[FirstName]		NVARCHAR(50)				NOT NULL, 
						[MiddleName]	NVARCHAR(50), 
						[LastName]		NVARCHAR(50)				NOT NULL, 
						[JobTitle]		NVARCHAR(30)				NOT NULL, 
						[DepartmentId] INT FOREIGN KEY REFERENCES [Departments]([Id]) NOT NULL,
						[HireDate] DATE NOT NULL, 
						[Salary] DECIMAL(7, 2) NOT NULL, 
						[AddressId] INT FOREIGN KEY REFERENCES [Addresses]([Id])
						)