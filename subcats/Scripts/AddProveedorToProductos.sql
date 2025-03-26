-- Script para agregar la columna de proveedor a la tabla productos
USE patitofeo;
GO

-- Agregar la columna ProveedorId si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[productos]') AND name = 'ProveedorId')
BEGIN
    ALTER TABLE [dbo].[productos] ADD [ProveedorId] INT NULL;
    PRINT 'La columna "ProveedorId" ha sido agregada a la tabla productos.';
END
ELSE
BEGIN
    PRINT 'La columna "ProveedorId" ya existe en la tabla productos.';
END

-- Agregar la clave foránea si no existe
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'[dbo].[productos]') AND referenced_object_id = OBJECT_ID(N'[dbo].[proveedores]'))
BEGIN
    ALTER TABLE [dbo].[productos]
    ADD CONSTRAINT [FK_Productos_Proveedores] 
    FOREIGN KEY ([ProveedorId]) 
    REFERENCES [dbo].[proveedores] ([Id_proveedor]);
    PRINT 'La clave foránea FK_Productos_Proveedores ha sido agregada.';
END
ELSE
BEGIN
    PRINT 'La clave foránea FK_Productos_Proveedores ya existe.';
END

PRINT 'Modificación de la tabla completada exitosamente.';
GO 