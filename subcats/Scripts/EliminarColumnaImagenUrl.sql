-- Script para eliminar la columna imagen_url de la tabla productos
USE patitofeo;
GO

-- Verificar si la columna imagen_url existe y eliminarla
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[productos]') AND name = 'imagen_url')
BEGIN
    -- Verificar si hay una restricción de valor predeterminado en la columna
    DECLARE @defaultConstraintName NVARCHAR(200);
    
    SELECT @defaultConstraintName = name
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[productos]')
    AND parent_column_id = (
        SELECT column_id
        FROM sys.columns
        WHERE object_id = OBJECT_ID(N'[dbo].[productos]')
        AND name = 'imagen_url'
    );
    
    -- Si hay una restricción de valor predeterminado, eliminarla primero
    IF @defaultConstraintName IS NOT NULL
    BEGIN
        EXECUTE('ALTER TABLE [dbo].[productos] DROP CONSTRAINT ' + @defaultConstraintName);
        PRINT 'Restricción de valor predeterminado eliminada para la columna "imagen_url".';
    END
    
    -- Eliminar la columna imagen_url
    ALTER TABLE [dbo].[productos] DROP COLUMN [imagen_url];
    PRINT 'La columna "imagen_url" ha sido eliminada de la tabla productos.';
END
ELSE
BEGIN
    PRINT 'La columna "imagen_url" no existe en la tabla productos.';
END

PRINT 'Modificación de la tabla completada exitosamente.';
GO 