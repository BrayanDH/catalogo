-- Crear la tabla Usuarios si no existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Usuarios] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Username] NVARCHAR(50) NOT NULL,
        [Password] NVARCHAR(50) NOT NULL,
        [Role] NVARCHAR(20) NOT NULL
    );
    
    PRINT 'Tabla Usuarios creada.';
END
ELSE
BEGIN
    PRINT 'La tabla Usuarios ya existe.';
END

-- Verificar si existe el usuario admin
IF NOT EXISTS (SELECT * FROM [dbo].[Usuarios] WHERE [Username] = 'admin')
BEGIN
    -- Insertar un usuario administrador por defecto
    INSERT INTO [dbo].[Usuarios] ([Username], [Password], [Role])
    VALUES ('admin', 'admin123', 'Admin');
    
    PRINT 'Usuario administrador creado.';
END
ELSE
BEGIN
    -- Actualizar el rol del usuario admin para asegurarse de que sea Admin
    UPDATE [dbo].[Usuarios]
    SET [Role] = 'Admin'
    WHERE [Username] = 'admin';
    
    PRINT 'Usuario administrador actualizado.';
END 