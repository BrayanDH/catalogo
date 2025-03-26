USE patitofeo;
GO

-- Paso 1: Crear la tabla empleados
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[empleados]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[empleados](
        [Id_empleado] [int] IDENTITY(1,1) NOT NULL,
        [Nombre] [nvarchar](100) NOT NULL,
        [Apellido] [nvarchar](100) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [Telefono] [nvarchar](20) NOT NULL,
        [Direccion] [nvarchar](200) NOT NULL,
        [Fecha_Nacimiento] [date] NULL,
        [Fecha_Ingreso] [date] NOT NULL,
        [Salario] [decimal](10,2) NOT NULL,
        [Estado] [bit] NOT NULL DEFAULT(1),
        [Fecha_Creacion] [datetime] NOT NULL DEFAULT(GETDATE()),
        [Fecha_Actualizacion] [datetime] NOT NULL DEFAULT(GETDATE()),
        CONSTRAINT [PK_empleados] PRIMARY KEY CLUSTERED ([Id_empleado] ASC)
    );
    PRINT 'Tabla empleados creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla empleados ya existe.';
END
GO

-- Crear el trigger si no existe
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Empleados_UpdateFechaActualizacion')
BEGIN
    CREATE TRIGGER [dbo].[TR_Empleados_UpdateFechaActualizacion]
    ON [dbo].[empleados]
    AFTER UPDATE
    AS
    BEGIN
        UPDATE [dbo].[empleados]
        SET [Fecha_Actualizacion] = GETDATE()
        FROM [dbo].[empleados] e
        INNER JOIN inserted i ON e.Id_empleado = i.Id_empleado;
    END;
END
GO 