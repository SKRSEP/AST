1.	Для создания БД в MS SQL выполняем следующий Query

CREATE DATABASE Employees;
GO
USE Employees;
GO
CREATE TABLE Employees
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50),
    Age INT,
    Salary DECIMAL(10, 2)
);
GO

2.	Для подключения к базе данных следует заменить 10-ю строку кода (путь к БД)

3.	Кнопка Update обновляет в БД ту строку, которая выделена в dataGridView. 

