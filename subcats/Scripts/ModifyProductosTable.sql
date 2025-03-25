-- Script para modificar la tabla productos
-- 1. Eliminar la columna de costos
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[productos]') AND name = 'costo')
BEGIN
    -- Obtener el nombre de la restricción de valor predeterminado (si existe)
    DECLARE @ConstraintName nvarchar(200);
    SELECT @ConstraintName = name
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[productos]')
    AND parent_column_id = (
        SELECT column_id
        FROM sys.columns
        WHERE object_id = OBJECT_ID(N'[dbo].[productos]')
        AND name = 'costo'
    );

    -- Eliminar la restricción si existe
    IF @ConstraintName IS NOT NULL
    BEGIN
        DECLARE @SQL nvarchar(500);
        SET @SQL = N'ALTER TABLE [dbo].[productos] DROP CONSTRAINT ' + @ConstraintName;
        EXEC sp_executesql @SQL;
        PRINT 'Restricción eliminada: ' + @ConstraintName;
    END

    -- Eliminar la columna costo
    ALTER TABLE [dbo].[productos] DROP COLUMN [costo];
    PRINT 'La columna "costo" ha sido eliminada de la tabla productos.';
END
ELSE
BEGIN
    PRINT 'La columna "costo" no existe en la tabla productos.';
END

-- 2. Agregar columna para imágenes
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[productos]') AND name = 'imagen_url')
BEGIN
    -- Agregar columna imagen_url
    ALTER TABLE [dbo].[productos] ADD [imagen_url] NVARCHAR(500) NULL;
    PRINT 'La columna "imagen_url" ha sido agregada a la tabla productos.';
END
ELSE
BEGIN
    PRINT 'La columna "imagen_url" ya existe en la tabla productos.';
END 