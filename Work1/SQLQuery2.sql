-- 1. Створення бази даних
CREATE DATABASE AcademyDB;
GO

USE AcademyDB;
GO

-- 2. Таблиця для завдань (проєкт Todo List)
CREATE TABLE Tasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    IsCompleted BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 3. Таблиця для товарів (завдання на DataReader та параметри)
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    IsAvailable BIT DEFAULT 1,
    Description NVARCHAR(MAX) NULL -- Стовпець, що може бути NULL для перевірки IsDBNull()
);

-- 4. Таблиця для користувачів (завдання на Auth та SQL Injection)
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Login NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL, -- Паролі ніколи не зберігаємо у відкритому вигляді!
    Role NVARCHAR(20) DEFAULT 'User'
);

-- 5. Заповнення тестовими даними
INSERT INTO Products (Name, Price, IsAvailable, Description)
VALUES
(N'Ноутбук', 25000.00, 1, N'Потужний ігровий ноутбук'),
(N'Мишка', 450.50, 1, NULL),
(N'Клавіатура', 1200.00, 0, N'Механічна');

INSERT INTO Users (Login, PasswordHash, Role)
VALUES
('admin', 'AQAAAAIAAYagAAAAE...', 'Admin'),
('student', 'AQAAAAIAAYagAAAAE...', 'User');
GO