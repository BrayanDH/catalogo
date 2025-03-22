using Microsoft.Data.SqlClient;
using subcats.customClass;
using subcats.dto;

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
                
                // Verificar si la tabla existe
                string checkTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'productos')
                BEGIN
                    -- Crear la tabla productos
                    CREATE TABLE productos (
                        id_producto INT IDENTITY(1,1) PRIMARY KEY,
                        nombre VARCHAR(255) NOT NULL,
                        descripcion TEXT NULL,
                        precio DECIMAL(10,2) NOT NULL,
                        costo DECIMAL(10,2) NOT NULL,
                        impuesto DECIMAL(5,2) NOT NULL,
                        descuento DECIMAL(5,2) NULL,
                        stock INT NOT NULL DEFAULT 0,
                        fecha_creacion DATETIME DEFAULT GETDATE(),
                        fecha_actualizacion DATETIME DEFAULT GETDATE()
                    );
                END
                ELSE
                BEGIN
                    -- Verificar si la columna stock existe, si no, agregarla
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('productos') AND name = 'stock')
                    BEGIN
                        ALTER TABLE productos ADD stock INT NOT NULL DEFAULT 0;
                    END
                END";
                
                using (SqlCommand cmd = new SqlCommand(checkTableQuery, cnx.connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the error, but don't throw it to avoid breaking the application
                Console.WriteLine("Error al verificar/crear tabla productos: " + ex.Message);
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
                string query = @"SELECT id_producto, nombre, descripcion, precio, costo, impuesto, descuento, stock, fecha_creacion, fecha_actualizacion 
                               FROM productos WHERE id_producto = @productoId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@productoId", int.Parse(productoId));
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            producto.Id_producto = reader.GetInt32(reader.GetOrdinal("id_producto"));
                            producto.Nombre = reader.GetString(reader.GetOrdinal("nombre"));
                            producto.Descripcion = !reader.IsDBNull(reader.GetOrdinal("descripcion")) ? reader.GetString(reader.GetOrdinal("descripcion")) : null;
                            producto.Precio = reader.GetDecimal(reader.GetOrdinal("precio"));
                            producto.Costo = reader.GetDecimal(reader.GetOrdinal("costo"));
                            producto.Impuesto = reader.GetDecimal(reader.GetOrdinal("impuesto"));
                            producto.Descuento = !reader.IsDBNull(reader.GetOrdinal("descuento")) ? reader.GetDecimal(reader.GetOrdinal("descuento")) : null;
                            producto.Stock = reader.GetInt32(reader.GetOrdinal("stock"));
                            producto.Fecha_creacion = !reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? reader.GetDateTime(reader.GetOrdinal("fecha_creacion")) : null;
                            producto.Fecha_actualizacion = !reader.IsDBNull(reader.GetOrdinal("fecha_actualizacion")) ? reader.GetDateTime(reader.GetOrdinal("fecha_actualizacion")) : null;
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
            return producto;
        }

        public List<Producto> GetAllProductos()
        {
            List<Producto> productos = new List<Producto>();
            try
            {
                cnx.connection.Open();
                string query = @"SELECT id_producto, nombre, descripcion, precio, costo, impuesto, descuento, stock, fecha_creacion, fecha_actualizacion 
                               FROM productos";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto producto = new Producto();
                            producto.Id_producto = reader.GetInt32(reader.GetOrdinal("id_producto"));
                            producto.Nombre = reader.GetString(reader.GetOrdinal("nombre"));
                            producto.Descripcion = !reader.IsDBNull(reader.GetOrdinal("descripcion")) ? reader.GetString(reader.GetOrdinal("descripcion")) : null;
                            producto.Precio = reader.GetDecimal(reader.GetOrdinal("precio"));
                            producto.Costo = reader.GetDecimal(reader.GetOrdinal("costo"));
                            producto.Impuesto = reader.GetDecimal(reader.GetOrdinal("impuesto"));
                            producto.Descuento = !reader.IsDBNull(reader.GetOrdinal("descuento")) ? reader.GetDecimal(reader.GetOrdinal("descuento")) : null;
                            producto.Stock = reader.GetInt32(reader.GetOrdinal("stock"));
                            producto.Fecha_creacion = !reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? reader.GetDateTime(reader.GetOrdinal("fecha_creacion")) : null;
                            producto.Fecha_actualizacion = !reader.IsDBNull(reader.GetOrdinal("fecha_actualizacion")) ? reader.GetDateTime(reader.GetOrdinal("fecha_actualizacion")) : null;
                            productos.Add(producto);
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

        public void ActualizarProducto(Producto producto)
        {
            try
            {
                cnx.connection.Open();
                string query = @"UPDATE productos 
                               SET nombre = @nombre, 
                                   descripcion = @descripcion, 
                                   precio = @precio, 
                                   costo = @costo, 
                                   impuesto = @impuesto, 
                                   descuento = @descuento,
                                   stock = @stock, 
                                   fecha_actualizacion = GETDATE() 
                               WHERE id_producto = @productoId";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@productoId", producto.Id_producto);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@costo", producto.Costo);
                    cmd.Parameters.AddWithValue("@impuesto", producto.Impuesto);
                    cmd.Parameters.AddWithValue("@descuento", producto.Descuento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);
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

        public void InsertarProducto(Producto producto)
        {
            try
            {
                cnx.connection.Open();
                string query = @"INSERT INTO productos (nombre, descripcion, precio, costo, impuesto, descuento, stock) 
                               VALUES (@nombre, @descripcion, @precio, @costo, @impuesto, @descuento, @stock)";
                using (SqlCommand cmd = new SqlCommand(query, cnx.connection))
                {
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@costo", producto.Costo);
                    cmd.Parameters.AddWithValue("@impuesto", producto.Impuesto);
                    cmd.Parameters.AddWithValue("@descuento", producto.Descuento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);
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
    }
}
