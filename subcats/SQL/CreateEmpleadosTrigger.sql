-- Paso 2: Crear el trigger
USE patitofeo;

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Empleados_UpdateFechaActualizacion')
BEGIN
    EXEC('CREATE TRIGGER [dbo].[TR_Empleados_UpdateFechaActualizacion]
    ON [dbo].[empleados]
    AFTER UPDATE
    AS
    BEGIN
        UPDATE [dbo].[empleados]
        SET [Fecha_Actualizacion] = GETDATE()
        FROM [dbo].[empleados] e
        INNER JOIN inserted i ON e.Id_empleado = i.Id_empleado;
    END');
    PRINT 'Trigger creado exitosamente.';
END
ELSE
BEGIN
    PRINT 'El trigger ya existe.';
END 