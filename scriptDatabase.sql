-- Crear la base de datos CreditCardAccount
USE [master]
GO
CREATE DATABASE [CreditCardAccount]
GO

-- Crear tabla CreditCardHolders
-- Esta tabla almacena información de los titulares de tarjetas de crédito.
-- Incluye:
-- - CardHolderID: Identificador único del titular.
-- - Name: Nombre del titular.
-- - CardNumber: Número único de la tarjeta.
-- - CreditLimit: Límite de crédito asignado al titular.
-- - CurrentBalance: Saldo actual de la tarjeta.
-- - AvailableBalance: Saldo disponible calculado como (CreditLimit - CurrentBalance).
CREATE TABLE [dbo].[CreditCardHolders](
	[CardHolderID] [int] NOT NULL,
	L,
	[CreditLNULL,
	[CurrentBalance] [decimal](18, 2) NOT NULL,
	[AvailableBalance]  AS ([CreditLimit]-[CurrentBalance]),
 CONSTRAINT [PK__CreditCa__05A1440B2944D44C] PRIMARY KEY CLUSTERED ([CardHolderID] ASC),
 CONSTRAINT [UQ__CreditCa__A4E9FFE9300B7AF3] UNIQUE NONCLUSTERED ([CardNumber] ASC)
) ON [PRIMARY]
GO

-- Crear tabla Transactions
-- Esta tabla almacena todas las transacciones realizadas con las tarjetas.
-- Incluye:
-- - TransactionID: Identificador único de la transacción.
-- - CardHolderID: Identificador del titular relacionado.
-- - TransactionDate: Fecha de la transacción.
-- - Description: Descripción de la transacción.
-- - TransactionType: Tipo de transacción ('Compra' o 'Pago').
-- - Amount: Monto de la transacción.
CREATE TABLE [dbo].[Transactions](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[CardHolderID] [int] NOT NULL,
	[TransactionDate] [date] NOT NULL,
	  NULL,
	[TransactionType] [nvarchar](10) NULL,OT NULL,tionID] ASC)
) ON [PRIMARY]
GO

-- Relación entre CreditCardHolders y Transactions
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK__Transacti__CardH__276EDEB3] FOREIGN KEY([CardHolderID])
REFERENCES [dbo].[CreditCardHolders] ([CardHolderID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK__Transacti__CardH__276EDEB3]
GO

-- Validar que TransactionType solo acepte 'Compra' o 'Pago'
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD CHECK  (([TransactionType]='Pago' OR [TransactionType]='Compra'))
GO

-- Crear tabla Configurations
-- Esta tabla almacena configuraciones generales como tasas de interés y porcentaje mínimo de pago.
-- Incluye:
-- - ConfigID: Identificador único de configuración.
-- - InterestRate: Tasa de interés (en porcentaje).
-- - MinimumPaymentRate: Porcentaje mínimo de pago (en porcentaje).
CREATE TABLE [dbo].[Configurations](
	[ConfigID] [int] IDENTITY(1,1) NOT NULL,
	[InterestRate] [decimal](5, 2) NOT NULL,
	[MinimumPaymentRate] [decimal](5, 2) NOT NULL,
PRIMARY KEY CLUSTERED ([ConfigID] ASC)
) ON [PRIMARY]
GO

-- Procedimiento almacenado AddTransaction
-- Inserta una nueva transacción y actualiza el saldo del titular.
CREATE PROCEDURE [dbo].[AddTransaction]
    @CardHolderID INT,
    @TransactionDate DATE,
    @Description NVARCHAR(255),
    @TransactionType NVARCHAR(10),
    @Amount DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar el tipo de transacción
    IF @TransactionType NOT IN ('Compra', 'Pago')
    BEGIN
        THROW 51000, 'TransactionType debe ser Compra o Pago.', 1;
    END;

    -- Validar saldo antes de realizar un pago
    IF @TransactionType = 'Pago'
    BEGIN
        DECLARE @CurrentBalance DECIMAL(18, 2);
        SELECT @CurrentBalance = CurrentBalance FROM CreditCardHolders WHERE CardHolderID = @CardHolderID;

        IF @CurrentBalance < @Amount
        BEGIN
            THROW 51001, 'El pago excede el saldo actual.', 1;
        END;
    END;

    -- Insertar la transacción
    INSERT INTO Transactions (CardHolderID, TransactionDate, Description, TransactionType, Amount)
    VALUES (@CardHolderID, @TransactionDate, @Description, @TransactionType, @Amount);

    -- Actualizar el saldo del titular
    IF @TransactionType = 'Compra'
    BEGIN
        UPDATE CreditCardHolders
        SET CurrentBalance = CurrentBalance + @Amount
        WHERE CardHolderID = @CardHolderID;
    END
    ELSE IF @TransactionType = 'Pago'
    BEGIN
        UPDATE CreditCardHolders
        SET CurrentBalance = CurrentBalance - @Amount
        WHERE CardHolderID = @CardHolderID;
    END;
END;
GO

-- Procedimiento almacenado GetCreditCardStatement
-- Genera el estado de cuenta detallado del titular.
CREATE PROCEDURE [dbo].[GetCreditCardStatement]
    @CardHolderID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Configuración de tasas
    DECLARE @InterestRate DECIMAL(5,2) = 25.00; -- Porcentaje de interés
    DECLARE @MinimumPaymentRate DECIMAL(5,2) = 5.00; -- Porcentaje de cuota mínima

    -- Datos básicos del titular
    SELECT 
        c.Name AS CardHolderName,
        c.CardNumber,
        c.CreditLimit,
        c.CurrentBalance,
        c.CreditLimit - c.CurrentBalance AS AvailableBalance
    INTO #CardHolderInfo
    FROM CreditCardHolders c
    WHERE c.CardHolderID = @CardHolderID;

    -- Cálculos financieros
    SELECT 
        CurrentBalance AS TotalBalance,
        CASE WHEN CurrentBalance > 0 THEN CurrentBalance * (@InterestRate / 100) ELSE 0 END AS InterestAmount,
        CASE WHEN CurrentBalance > 0 THEN CurrentBalance * (@MinimumPaymentRate / 100) ELSE 0 END AS MinimumPayment,
        CASE WHEN CurrentBalance > 0 THEN CurrentBalance + (CurrentBalance * (@InterestRate / 100)) ELSE 0 END AS TotalWithInterest
    INTO #FinancialCalculations
    FROM #CardHolderInfo;

    -- Totales de compras
    SELECT 
        SUM(CASE 
                WHEN MONTH(TransactionDate) = MONTH(GETDATE()) 
                THEN Amount 
                ELSE 0 
            END) AS TotalCurrentMonth,
        SUM(CASE 
                WHEN MONTH(TransactionDate) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
                THEN Amount 
                ELSE 0 
            END) AS TotalPreviousMonth
    INTO #TransactionTotals
    FROM Transactions
    WHERE CardHolderID = @CardHolderID AND TransactionType = 'Compra';

    -- Combinar resultados
    SELECT 
        c.CardHolderName,
        c.CardNumber,
        c.CreditLimit,
        c.CurrentBalance,
        c.AvailableBalance,
        t.TotalCurrentMonth,
        t.TotalPreviousMonth,
        f.TotalBalance,
        f.InterestAmount,
        f.MinimumPayment,
        f.TotalWithInterest
    FROM 
        #CardHolderInfo c
        CROSS JOIN #FinancialCalculations f
        CROSS JOIN #TransactionTotals t;

    -- Limpiar tablas temporales
    DROP TABLE #CardHolderInfo;
    DROP TABLE #FinancialCalculations;
    DROP TABLE #TransactionTotals;
END;
GO

-- Procedimiento almacenado GetTransactionHistory
-- Obtiene el historial de transacciones del mes actual.
CREATE PROCEDURE [dbo].[GetTransactionHistory]
    @CardHolderID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        TransactionDate,
        Description,
        TransactionType,
        Amount
    FROM 
        Transactions
    WHERE 
        CardHolderID = @CardHolderID 
        AND MONTH(TransactionDate) = MONTH(GETDATE())
        AND YEAR(TransactionDate) = YEAR(GETDATE())
    ORDER BY 
        TransactionDate DESC;
END;
GO
