-- Script para crear la tabla de empleados
USE patitofeo;
GO

-- Crear la tabla empleados si no existe
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'empleados')
BEGIN
    CREATE TABLE [dbo].[empleados] (
        [Id_empleado] INT IDENTITY(1,1) PRIMARY KEY,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Apellido] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [Telefono] NVARCHAR(20) NOT NULL,
        [Direccion] NVARCHAR(200) NOT NULL,
        [Fecha_Nacimiento] DATE NULL,
        [Fecha_Ingreso] DATE NOT NULL,
        [Salario] DECIMAL(10,2) NOT NULL,
        [Estado] BIT NOT NULL DEFAULT 1,
        [Fecha_Creacion] DATETIME DEFAULT GETDATE(),
        [Fecha_Actualizacion] DATETIME DEFAULT GETDATE()
    );
    
    PRINT 'Tabla empleados creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla empleados ya existe.';
END
GO 