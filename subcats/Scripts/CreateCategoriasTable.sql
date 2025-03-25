-- Verificar si la tabla Categorias existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categorias]') AND type IN (N'U'))
BEGIN
    -- Crear la tabla si no existe
    CREATE TABLE [dbo].[Categorias] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [FechaCreacion] DATETIME NULL,
        [FechaActualizacion] DATETIME NULL
    );
    
    -- Insertar datos de ejemplo
    INSERT INTO [dbo].[Categorias] ([Nombre], [Descripcion], [FechaCreacion], [FechaActualizacion])
    VALUES 
        (N'Electrónica', N'Productos electrónicos y gadgets', GETDATE(), GETDATE()),
        (N'Ropa', N'Ropa y accesorios', GETDATE(), GETDATE()),
        (N'Hogar', N'Artículos para el hogar', GETDATE(), GETDATE());
    
    PRINT 'Tabla Categorias creada con datos de ejemplo.';
END
ELSE 
BEGIN
    PRINT 'La tabla Categorias ya existe.';
END 