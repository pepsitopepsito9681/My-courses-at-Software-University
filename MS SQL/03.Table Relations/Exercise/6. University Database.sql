--CREATE DATABASE [UniversityDatabase]

--USE [UniversityDatabase]

CREATE TABLE [Majors]
					(
					[MajorID] INT PRIMARY KEY IDENTITY,
					[Name] NVARCHAR(50) NOT NULL
					)

CREATE TABLE [Students]
					(
					[StudentID] INT PRIMARY KEY IDENTITY,
					[StudentNumber] VARCHAR(10) NOT NULL,
					[StudentName] NVARCHAR(50) NOT NULL,
					[MajorID] INT FOREIGN KEY REFERENCES Majors([MajorID]) NOT NULL
					)
CREATE TABLE [Payments]
					(
						[PaymentID] INT PRIMARY KEY IDENTITY,
						[PaymentDate] DATE NOT NULL,
						[PaymentAmount] DECIMAL(15, 2) NOT NULL,
						[StudentID] INT NOT NULL FOREIGN KEY REFERENCES [Students]([StudentID])
					)

CREATE TABLE [Subjects]
					(
						[SubjectID] INT PRIMARY KEY IDENTITY,
						[SubjectName] NVARCHAR(80) NOT NULL
					)

CREATE TABLE [Agenda]
					(
						[StudentID] INT NOT NULL FOREIGN KEY REFERENCES [Students]([StudentID]),
						[SubjectID] INT NOT NULL FOREIGN KEY REFERENCES [Subjects]([SubjectID])
						PRIMARY KEY([StudentID], [SubjectID])
					)