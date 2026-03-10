-- 1. Створення бази даних
CREATE DATABASE AdoNetAdvancedDB;
GO

USE AdoNetAdvancedDB;
GO

-- ==========================================
-- ТАБЛИЦІ ДЛЯ РІВНЯ 1 (SQL Injection та DataReader)
-- ==========================================

-- Таблиця для перевірки авторизації (Завдання 1.1)
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL -- У реальності тут має бути Hash, але для завдання залишаємо так
);

-- Таблиця товарів для DataReader та Міні-проєкту (Завдання 1.2 та 3.1)
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Description NVARCHAR(MAX) NULL, -- Може бути NULL для перевірки IsDBNull()
    Stock INT NOT NULL DEFAULT 0    -- Кількість на складі для транзакцій
);

-- ==========================================
-- ТАБЛИЦІ ДЛЯ РІВНЯ 2 (Транзакції)
-- ==========================================

-- Таблиця банківських рахунків (Завдання 2.1)
CREATE TABLE Accounts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OwnerName NVARCHAR(100) NOT NULL,
    Balance DECIMAL(18, 2) NOT NULL CHECK (Balance >= 0) -- Баланс не може бути від'ємним
);

-- ==========================================
-- ТАБЛИЦІ ДЛЯ РІВНЯ 3 (Міні-проєкт)
-- ==========================================

-- Таблиця замовлень (Завдання 3.1)
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    CustomerName NVARCHAR(100) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Orders_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO

-- ==========================================
-- ЗАПОВНЕННЯ ТЕСТОВИМИ ДАНИМИ
-- ==========================================

-- Додаємо користувачів
INSERT INTO Users (Username, Password)
VALUES
('admin', 'supersecret'),
('student', '123456');

-- Додаємо товари (зверніть увагу на NULL в Description)
INSERT INTO Products (Name, Price, Description, Stock)
VALUES
(N'Ігровий Ноутбук', 45000.00, N'Потужний ноутбук для ігор', 10),
(N'Офісна Мишка', 350.00, NULL, 50),
(N'Механічна Клавіатура', 2500.00, N'Свічі Cherry MX Red', 5),
(N'Монітор 4K', 12000.00, NULL, 0); -- Немає в наявності (Stock = 0)

-- Додаємо банківські рахунки
INSERT INTO Accounts (OwnerName, Balance)
VALUES
(N'Рахунок А (Відправник)', 5000.00),
(N'Рахунок Б (Отримувач)', 1500.00);
GO

-- Перевірка створених даних
SELECT * FROM Users;
SELECT * FROM Products;
SELECT * FROM Accounts;