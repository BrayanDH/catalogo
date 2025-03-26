﻿using Microsoft.Data.SqlClient;
using subcats.customClass;
using subcats.dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace subcats.customClass
{
    public class Dao
    {
        private Conection cnx;
        public Dao()
        {
            cnx = new Conection();
            VerificarTablaProductos();
        }

        private void VerificarTablaProductos()
        {
            try
            {
                cnx.connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'productos')
                BEGIN
                    -- Crear la tabla productos
                    CREATE TABLE productos (
                        id_producto INT IDENTITY(1,1) PRIMARY KEY,
                        nombre NVARCHAR(100) NOT NULL,
                        descripcion NVARCHAR(500) NULL,
                        precio DECIMAL(10,2) NOT NULL,
                        impuesto DECIMAL(10,2) NOT NULL,
                        descuento DECIMAL(10,2) NULL,
                        stock INT NOT NULL DEFAULT 0,
                        fecha_creacion DATETIME DEFAULT GETDATE(),
                        fecha_actualizacion DATETIME DEFAULT GETDATE(),
                        CategoriaId INT NULL,
                        Imagen VARBINARY(MAX) NULL,
                        ImagenBinaria VARBINARY(MAX) NULL
                    );
                END
                
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('productos') AND name = 'stock')
                BEGIN
                    ALTER TABLE productos ADD stock INT NOT NULL DEFAULT 0;
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'proveedores')
                BEGIN
                    CREATE TABLE proveedores (
                        id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
                        nombre NVARCHAR(100) NOT NULL,
                        telefono NVARCHAR(20) NOT NULL,
                        email NVARCHAR(100) NOT NULL,
                        direccion NVARCHAR(200) NOT NULL,
                        fecha_creacion DATETIME DEFAULT GETDATE(),
                        fecha_actualizacion DATETIME DEFAULT GETDATE()
                    );
                END
                ");

                SqlCommand command = new SqlCommand(sb.ToString(), cnx.connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar/crear tablas: " + ex.Message);
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        public PanSubCategoria PanSubCategoria(string subCatId)
        {
            PanSubCategoria cat = new PanSubCategoria();
            try
            {
                cnx.connection.Open();
                string query = @"select ID, categoriaId, Description, UserId, Estado, Position from PanSubCategoria where ID = " + subCatId;
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cat.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                            cat.CategoriaId = reader.GetInt32(reader.GetOrdinal("categoriaId")); // Cambiado a 'CategoriaId'
                            cat.Description = reader.GetString(reader.GetOrdinal("Description"));
                            cat.UserId = reader.GetInt32(reader.GetOrdinal("UserId")); // Cambiado a 'UserId'
                            cat.Estado = reader.GetInt32(reader.GetOrdinal("Estado"));
                            cat.Position = reader.GetInt32(reader.GetOrdinal("Position"));
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
            return cat;
        }


        public List<PanSubCategoria> PanSubCategoria2(string CatId)
        {
            List<PanSubCategoria> cats = new List<PanSubCategoria>();
            try
            {
                cnx.connection.Open();
                string query = @"select ID, categoriaId, Description, UserId, Estado, Position from PanSubCategoria where categoriaId = " + CatId;
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PanSubCategoria cat = new PanSubCategoria();
                            cat.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                            cat.CategoriaId = reader.GetInt32(reader.GetOrdinal("categoriaId")); // Cambiado a 'CategoriaId'
                            cat.Description = reader.GetString(reader.GetOrdinal("Description"));
                            cat.UserId = reader.GetInt32(reader.GetOrdinal("UserId")); // Cambiado a 'UserId'
                            cat.Estado = reader.GetInt32(reader.GetOrdinal("Estado"));
                            cat.Position = reader.GetInt32(reader.GetOrdinal("Position"));
                            cats.Add(cat);
                        }
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
            return cats;
        }


        public List<PanSubCategoria> GetAllPanSubCategorias()
        {
            List<PanSubCategoria> cats = new List<PanSubCategoria>();
            try
            {
                cnx.connection.Open();
                string query = @"select ID, categoriaId, Description, UserId, Estado, Position from PanSubCategoria";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PanSubCategoria cat = new PanSubCategoria();
                            cat.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                            cat.CategoriaId = reader.GetInt32(reader.GetOrdinal("categoriaId"));
                            cat.Description = reader.GetString(reader.GetOrdinal("Description"));
                            cat.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                            cat.Estado = reader.GetInt32(reader.GetOrdinal("Estado"));
                            cat.Position = reader.GetInt32(reader.GetOrdinal("Position"));
                            cats.Add(cat);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
            return cats;
        }

        public void BorrarPanSubCategoria(string subCatId)
        {
            try
            {
                cnx.connection.Open();
                string query = @"update PanSubCategoria set Estado = 0 where ID = " + subCatId;
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
        }


        public void ActualizarPanSubCategoria(PanSubCategoria subCategoria)
        {
            try
            {
                cnx.connection.Open();
                string query = @"update PanSubCategoria set categoriaId = @categoriaId, Description = @description, UserId = @userId, Estado = @estado, Position = @position where ID = @id";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@id", subCategoria.ID);
                    cmd.Parameters.AddWithValue("@categoriaId", subCategoria.CategoriaId);
                    cmd.Parameters.AddWithValue("@description", subCategoria.Description);
                    cmd.Parameters.AddWithValue("@userId", subCategoria.UserId);
                    cmd.Parameters.AddWithValue("@estado", subCategoria.Estado);
                    cmd.Parameters.AddWithValue("@position", subCategoria.Position);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
        }


        public void InsertarPanSubCategoria(PanSubCategoria subCategoria)
        {
            try
            {
                cnx.connection.Open();
                string query = @"insert into PanSubCategoria (categoriaId, Description, UserId, Estado, Position) values (@categoriaId, @description, @userId, @estado, @position)";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@categoriaId", subCategoria.CategoriaId);
                    cmd.Parameters.AddWithValue("@description", subCategoria.Description);
                    cmd.Parameters.AddWithValue("@userId", subCategoria.UserId);
                    cmd.Parameters.AddWithValue("@estado", subCategoria.Estado);
                    cmd.Parameters.AddWithValue("@position", subCategoria.Position);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        // Product methods
        public Producto GetProducto(string productoId)
        {
            Producto producto = new Producto();
            try
            {
                cnx.connection.Open();
                string query = @"SELECT id_producto, nombre, descripcion, precio, impuesto, descuento, stock, 
                              fecha_creacion, fecha_actualizacion, CategoriaId, ProveedorId, ImagenBinaria as Imagen
                         FROM productos WHERE id_producto = @productoId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@productoId", int.Parse(productoId));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        producto.Id_producto = reader.GetInt32(reader.GetOrdinal("id_producto"));
                        producto.Nombre = reader.GetString(reader.GetOrdinal("nombre"));
                        producto.Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"));
                        producto.Precio = reader.GetDecimal(reader.GetOrdinal("precio"));
                        producto.Impuesto = reader.GetDecimal(reader.GetOrdinal("impuesto"));
                        producto.Descuento = reader.IsDBNull(reader.GetOrdinal("descuento")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("descuento"));
                        producto.Stock = reader.GetInt32(reader.GetOrdinal("stock"));
                        producto.Fecha_creacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_creacion"));
                        producto.Fecha_actualizacion = reader.IsDBNull(reader.GetOrdinal("fecha_actualizacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_actualizacion"));
                        producto.CategoriaId = reader.IsDBNull(reader.GetOrdinal("CategoriaId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("CategoriaId"));
                        producto.ProveedorId = reader.IsDBNull(reader.GetOrdinal("ProveedorId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("ProveedorId"));
                        
                        // Recuperar datos de imagen si existen
                        if (!reader.IsDBNull(reader.GetOrdinal("Imagen")))
                        {
                            // Obtener el tamaño del campo binario
                            long byteLength = reader.GetBytes(reader.GetOrdinal("Imagen"), 0, null, 0, 0);
                            producto.Imagen = new byte[byteLength];
                            reader.GetBytes(reader.GetOrdinal("Imagen"), 0, producto.Imagen, 0, (int)byteLength);
                            Console.WriteLine($"Imagen recuperada para producto: {producto.Id_producto}, tamaño: {byteLength} bytes");
                        }
                        else
                        {
                            Console.WriteLine($"El producto {producto.Id_producto} no tiene imagen");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener producto: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            finally
            {
                cnx.connection.Close();
            }
            return producto;
        }

        public List<Producto> GetAllProductos()
        {
            List<Producto> productos = new List<Producto>();
            try
            {
                cnx.connection.Open();
                string query = @"SELECT id_producto, nombre, descripcion, precio, impuesto, descuento, stock, 
                              fecha_creacion, fecha_actualizacion, CategoriaId, ProveedorId, ImagenBinaria as Imagen
                         FROM productos";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Producto producto = new Producto();
                        producto.Id_producto = reader.GetInt32(reader.GetOrdinal("id_producto"));
                        producto.Nombre = reader.GetString(reader.GetOrdinal("nombre"));
                        producto.Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"));
                        producto.Precio = reader.GetDecimal(reader.GetOrdinal("precio"));
                        producto.Impuesto = reader.GetDecimal(reader.GetOrdinal("impuesto"));
                        producto.Descuento = reader.IsDBNull(reader.GetOrdinal("descuento")) ? null : (decimal?)reader.GetDecimal(reader.GetOrdinal("descuento"));
                        producto.Stock = reader.GetInt32(reader.GetOrdinal("stock"));
                        producto.Fecha_creacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_creacion"));
                        producto.Fecha_actualizacion = reader.IsDBNull(reader.GetOrdinal("fecha_actualizacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_actualizacion"));
                        producto.CategoriaId = reader.IsDBNull(reader.GetOrdinal("CategoriaId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("CategoriaId"));
                        producto.ProveedorId = reader.IsDBNull(reader.GetOrdinal("ProveedorId")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("ProveedorId"));
                        
                        // Recuperar datos de imagen si existen
                        if (!reader.IsDBNull(reader.GetOrdinal("Imagen")))
                        {
                            // Obtener el tamaño del campo binario
                            long byteLength = reader.GetBytes(reader.GetOrdinal("Imagen"), 0, null, 0, 0);
                            producto.Imagen = new byte[byteLength];
                            reader.GetBytes(reader.GetOrdinal("Imagen"), 0, producto.Imagen, 0, (int)byteLength);
                        }

                        productos.Add(producto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener productos: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            finally
            {
                cnx.connection.Close();
            }
            return productos;
        }

        public void EliminarProducto(string productoId)
        {
            try
            {
                cnx.connection.Open();
                string query = @"DELETE FROM productos WHERE id_producto = @productoId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@productoId", int.Parse(productoId));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        public bool ActualizarProducto(Producto producto)
        {
            try
            {
                Console.WriteLine($"Actualizando producto con ID: {producto.Id_producto}");
                if (producto != null && producto.Imagen != null && producto.Imagen.Length > 0)
                {
                    Console.WriteLine($"El producto tiene una imagen de {producto.Imagen.Length} bytes");
                }
                else
                {
                    Console.WriteLine("El producto no tiene imagen");
                }

                cnx.connection.Open();
                string query = @"UPDATE productos 
                               SET nombre = @nombre, 
                                   descripcion = @descripcion, 
                                   precio = @precio, 
                                   impuesto = @impuesto, 
                                   descuento = @descuento,
                                   stock = @stock,
                                   fecha_actualizacion = GETDATE(),
                                   CategoriaId = @categoriaId,
                                   ProveedorId = @proveedorId,
                                   ImagenBinaria = @imagen
                               WHERE id_producto = @productoId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@productoId", producto.Id_producto);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@impuesto", producto.Impuesto);
                    cmd.Parameters.AddWithValue("@descuento", producto.Descuento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@categoriaId", producto.CategoriaId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@proveedorId", producto.ProveedorId ?? (object)DBNull.Value);
                    
                    // Manejar imagen como parámetro binario
                    if (producto.Imagen != null && producto.Imagen.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@imagen", producto.Imagen);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imagen", DBNull.Value);
                    }
                    
                    Console.WriteLine("Ejecutando consulta de actualización SQL...");
                    // Ejecutar la consulta y obtener el número de filas afectadas
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    Console.WriteLine($"Filas afectadas: {filasAfectadas}");
                    
                    // Retornar true si al menos una fila fue actualizada
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        public int InsertarProducto(Producto producto)
        {
            try
            {
                Console.WriteLine($"Insertando producto: {producto.Nombre}");
                if (producto != null && producto.Imagen != null && producto.Imagen.Length > 0)
                {
                    Console.WriteLine($"El producto tiene una imagen de {producto.Imagen.Length} bytes");
                }
                else
                {
                    Console.WriteLine("El producto no tiene imagen");
                }

                cnx.connection.Open();
                string query = @"INSERT INTO productos (nombre, descripcion, precio, impuesto, descuento, stock, 
                              fecha_creacion, fecha_actualizacion, CategoriaId, ImagenBinaria, ProveedorId)
                              VALUES (@nombre, @descripcion, @precio, @impuesto, @descuento, @stock,
                              GETDATE(), GETDATE(), @categoriaId, @imagen, @proveedorId);
                              SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@impuesto", producto.Impuesto);
                    cmd.Parameters.AddWithValue("@descuento", producto.Descuento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@categoriaId", producto.CategoriaId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@proveedorId", producto.ProveedorId ?? (object)DBNull.Value);
                    
                    // Manejar imagen como parámetro binario
                    if (producto.Imagen != null && producto.Imagen.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@imagen", producto.Imagen);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imagen", DBNull.Value);
                    }

                    Console.WriteLine("Ejecutando consulta SQL...");
                    // Ejecutar la consulta y obtener el ID generado
                    object result = cmd.ExecuteScalar();
                    int id = 0;
                    
                    if (result != null && result != DBNull.Value)
                    {
                        id = Convert.ToInt32(result);
                        Console.WriteLine($"Producto insertado con ID: {id}");
                    }
                    else
                    {
                        Console.WriteLine("No se pudo obtener el ID del producto insertado");
                    }
                    
                    return id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        public List<Proveedor> GetAllProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            try
            {
                cnx.connection.Open();
                string query = @"SELECT id_proveedor, nombre, telefono, email, direccion, 
                              fecha_creacion, fecha_actualizacion FROM proveedores";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Proveedor proveedor = new Proveedor();
                        proveedor.Id_proveedor = reader.GetInt32(reader.GetOrdinal("id_proveedor"));
                        proveedor.Nombre = reader.GetString(reader.GetOrdinal("nombre"));
                        proveedor.Telefono = reader.GetString(reader.GetOrdinal("telefono"));
                        proveedor.Email = reader.GetString(reader.GetOrdinal("email"));
                        proveedor.Direccion = reader.GetString(reader.GetOrdinal("direccion"));
                        proveedor.Fecha_creacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_creacion"));
                        proveedor.Fecha_actualizacion = reader.IsDBNull(reader.GetOrdinal("fecha_actualizacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_actualizacion"));
                        proveedores.Add(proveedor);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener proveedores: " + ex.Message);
            }
            finally
            {
                cnx.connection.Close();
            }
            return proveedores;
        }

        public Proveedor GetProveedor(string proveedorId)
        {
            Proveedor proveedor = new Proveedor();
            try
            {
                cnx.connection.Open();
                string query = @"SELECT id_proveedor, nombre, telefono, email, direccion, 
                              fecha_creacion, fecha_actualizacion 
                              FROM proveedores WHERE id_proveedor = @proveedorId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@proveedorId", int.Parse(proveedorId));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        proveedor.Id_proveedor = reader.GetInt32(reader.GetOrdinal("id_proveedor"));
                        proveedor.Nombre = reader.GetString(reader.GetOrdinal("nombre"));
                        proveedor.Telefono = reader.GetString(reader.GetOrdinal("telefono"));
                        proveedor.Email = reader.GetString(reader.GetOrdinal("email"));
                        proveedor.Direccion = reader.GetString(reader.GetOrdinal("direccion"));
                        proveedor.Fecha_creacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_creacion"));
                        proveedor.Fecha_actualizacion = reader.IsDBNull(reader.GetOrdinal("fecha_actualizacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_actualizacion"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener proveedor: " + ex.Message);
            }
            finally
            {
                cnx.connection.Close();
            }
            return proveedor;
        }

        public int InsertarProveedor(Proveedor proveedor)
        {
            try
            {
                cnx.connection.Open();
                string query = @"INSERT INTO proveedores (nombre, telefono, email, direccion)
                                VALUES (@nombre, @telefono, @email, @direccion);
                                SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("@email", proveedor.Email);
                    cmd.Parameters.AddWithValue("@direccion", proveedor.Direccion);

                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar proveedor: " + ex.Message);
                return 0;
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        public bool ActualizarProveedor(Proveedor proveedor)
        {
            try
            {
                cnx.connection.Open();
                string query = @"UPDATE proveedores 
                               SET nombre = @nombre,
                                   telefono = @telefono,
                                   email = @email,
                                   direccion = @direccion,
                                   fecha_actualizacion = GETDATE()
                               WHERE id_proveedor = @id_proveedor";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@id_proveedor", proveedor.Id_proveedor);
                    cmd.Parameters.AddWithValue("@nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@telefono", proveedor.Telefono);
                    cmd.Parameters.AddWithValue("@email", proveedor.Email);
                    cmd.Parameters.AddWithValue("@direccion", proveedor.Direccion);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar proveedor: " + ex.Message);
                return false;
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        public void EliminarProveedor(string proveedorId)
        {
            try
            {
                cnx.connection.Open();
                string query = @"DELETE FROM proveedores WHERE id_proveedor = @proveedorId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@proveedorId", int.Parse(proveedorId));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar proveedor: " + ex.Message);
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        // Métodos para Empleados
        public List<Empleado> GetAllEmpleados()
        {
            List<Empleado> empleados = new List<Empleado>();
            try
            {
                cnx.connection.Open();
                string query = @"SELECT Id_empleado, Nombre, Apellido, Email, Telefono, Direccion, 
                              Fecha_Nacimiento, Fecha_Ingreso, Salario, Estado, 
                              Fecha_Creacion, Fecha_Actualizacion 
                              FROM empleados 
                              ORDER BY Nombre, Apellido";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Empleado empleado = new Empleado();
                        empleado.Id_empleado = reader.GetInt32(reader.GetOrdinal("Id_empleado"));
                        empleado.Nombre = reader.GetString(reader.GetOrdinal("Nombre"));
                        empleado.Apellido = reader.GetString(reader.GetOrdinal("Apellido"));
                        empleado.Email = reader.GetString(reader.GetOrdinal("Email"));
                        empleado.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        empleado.Direccion = reader.GetString(reader.GetOrdinal("Direccion"));
                        empleado.Fecha_Nacimiento = reader.IsDBNull(reader.GetOrdinal("Fecha_Nacimiento")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("Fecha_Nacimiento"));
                        empleado.Fecha_Ingreso = reader.GetDateTime(reader.GetOrdinal("Fecha_Ingreso"));
                        empleado.Salario = reader.GetDecimal(reader.GetOrdinal("Salario"));
                        empleado.Estado = reader.GetBoolean(reader.GetOrdinal("Estado"));
                        empleado.Fecha_Creacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Creacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("Fecha_Creacion"));
                        empleado.Fecha_Actualizacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Actualizacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("Fecha_Actualizacion"));
                        empleados.Add(empleado);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener empleados: " + ex.Message);
            }
            finally
            {
                cnx.connection.Close();
            }
            return empleados;
        }

        public Empleado GetEmpleado(string empleadoId)
        {
            Empleado empleado = new Empleado();
            try
            {
                cnx.connection.Open();
                string query = @"SELECT Id_empleado, Nombre, Apellido, Email, Telefono, Direccion, 
                              Fecha_Nacimiento, Fecha_Ingreso, Salario, Estado, 
                              Fecha_Creacion, Fecha_Actualizacion 
                              FROM empleados 
                              WHERE Id_empleado = @empleadoId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@empleadoId", int.Parse(empleadoId));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        empleado.Id_empleado = reader.GetInt32(reader.GetOrdinal("Id_empleado"));
                        empleado.Nombre = reader.GetString(reader.GetOrdinal("Nombre"));
                        empleado.Apellido = reader.GetString(reader.GetOrdinal("Apellido"));
                        empleado.Email = reader.GetString(reader.GetOrdinal("Email"));
                        empleado.Telefono = reader.GetString(reader.GetOrdinal("Telefono"));
                        empleado.Direccion = reader.GetString(reader.GetOrdinal("Direccion"));
                        empleado.Fecha_Nacimiento = reader.IsDBNull(reader.GetOrdinal("Fecha_Nacimiento")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("Fecha_Nacimiento"));
                        empleado.Fecha_Ingreso = reader.GetDateTime(reader.GetOrdinal("Fecha_Ingreso"));
                        empleado.Salario = reader.GetDecimal(reader.GetOrdinal("Salario"));
                        empleado.Estado = reader.GetBoolean(reader.GetOrdinal("Estado"));
                        empleado.Fecha_Creacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Creacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("Fecha_Creacion"));
                        empleado.Fecha_Actualizacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Actualizacion")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("Fecha_Actualizacion"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener empleado: " + ex.Message);
            }
            finally
            {
                cnx.connection.Close();
            }
            return empleado;
        }

        public int InsertarEmpleado(Empleado empleado)
        {
            try
            {
                cnx.connection.Open();
                string query = @"INSERT INTO empleados (Nombre, Apellido, Email, Telefono, Direccion, 
                              Fecha_Nacimiento, Fecha_Ingreso, Salario, Estado)
                              VALUES (@nombre, @apellido, @email, @telefono, @direccion, 
                              @fechaNacimiento, @fechaIngreso, @salario, @estado);
                              SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@email", empleado.Email);
                    cmd.Parameters.AddWithValue("@telefono", empleado.Telefono);
                    cmd.Parameters.AddWithValue("@direccion", empleado.Direccion);
                    cmd.Parameters.AddWithValue("@fechaNacimiento", empleado.Fecha_Nacimiento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaIngreso", empleado.Fecha_Ingreso);
                    cmd.Parameters.AddWithValue("@salario", empleado.Salario);
                    cmd.Parameters.AddWithValue("@estado", empleado.Estado);

                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar empleado: " + ex.Message);
                return 0;
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        public bool ActualizarEmpleado(Empleado empleado)
        {
            try
            {
                cnx.connection.Open();
                string query = @"UPDATE empleados 
                               SET Nombre = @nombre,
                                   Apellido = @apellido,
                                   Email = @email,
                                   Telefono = @telefono,
                                   Direccion = @direccion,
                                   Fecha_Nacimiento = @fechaNacimiento,
                                   Fecha_Ingreso = @fechaIngreso,
                                   Salario = @salario,
                                   Estado = @estado,
                                   Fecha_Actualizacion = GETDATE()
                               WHERE Id_empleado = @id_empleado";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@id_empleado", empleado.Id_empleado);
                    cmd.Parameters.AddWithValue("@nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@email", empleado.Email);
                    cmd.Parameters.AddWithValue("@telefono", empleado.Telefono);
                    cmd.Parameters.AddWithValue("@direccion", empleado.Direccion);
                    cmd.Parameters.AddWithValue("@fechaNacimiento", empleado.Fecha_Nacimiento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaIngreso", empleado.Fecha_Ingreso);
                    cmd.Parameters.AddWithValue("@salario", empleado.Salario);
                    cmd.Parameters.AddWithValue("@estado", empleado.Estado);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar empleado: " + ex.Message);
                return false;
            }
            finally
            {
                cnx.connection.Close();
            }
        }

        public void EliminarEmpleado(string empleadoId)
        {
            try
            {
                cnx.connection.Open();
                string query = @"DELETE FROM empleados WHERE Id_empleado = @empleadoId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@empleadoId", int.Parse(empleadoId));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar empleado: " + ex.Message);
            }
            finally
            {
                cnx.connection.Close();
            }
        }
    }
}
