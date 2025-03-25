-- Script para eliminar la columna Activo de la tabla Categorias
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Categorias]') AND name = 'Activo')
BEGIN
    -- Obtener el nombre de la restricción de valor predeterminado
    DECLARE @ConstraintName nvarchar(200);
    SELECT @ConstraintName = name
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[Categorias]')
    AND parent_column_id = (
        SELECT column_id
        FROM sys.columns
        WHERE object_id = OBJECT_ID(N'[dbo].[Categorias]')
        AND name = 'Activo'
    );

    -- Eliminar la restricción si existe
    IF @ConstraintName IS NOT NULL
    BEGIN
        DECLARE @SQL nvarchar(500);
        SET @SQL = N'ALTER TABLE [dbo].[Categorias] DROP CONSTRAINT ' + @ConstraintName;
        EXEC sp_executesql @SQL;
        PRINT 'Restricción eliminada: ' + @ConstraintName;
    END

    -- Eliminar la columna Activo
    ALTER TABLE [dbo].[Categorias] DROP COLUMN [Activo];
    PRINT 'La columna Activo ha sido eliminada de la tabla Categorias.';
END
ELSE
BEGIN
    PRINT 'La columna Activo no existe en la tabla Categorias.';
END 