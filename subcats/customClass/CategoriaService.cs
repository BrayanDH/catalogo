using Microsoft.Data.SqlClient;
using subcats.dto;
using System;
using System.Collections.Generic;

namespace subcats.customClass
{
    public class CategoriaService
    {
        private Conection _conn;

        public CategoriaService()
        {
            _conn = new Conection();
        }

        public List<Categoria> ObtenerTodasCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            try
            {
                _conn.connection.Open();
                string query = "SELECT Id, Nombre, Descripcion, Fecha_creacion, FechaActualizacion FROM Categorias ORDER BY Nombre";
                _conn.sqlCommand = new SqlCommand(query, _conn.connection);

                _conn.sqlDataReader = _conn.sqlCommand.ExecuteReader();

                while (_conn.sqlDataReader.Read())
                {
                    Categoria categoria = new Categoria
                    {
                        Id = Convert.ToInt32(_conn.sqlDataReader["Id"]),
                        Nombre = _conn.sqlDataReader["Nombre"].ToString(),
                        Descripcion = _conn.sqlDataReader["Descripcion"] != DBNull.Value 
                            ? _conn.sqlDataReader["Descripcion"].ToString() 
                            : null,
                        FechaCreacion = _conn.sqlDataReader["Fecha_creacion"] != DBNull.Value 
                            ? Convert.ToDateTime(_conn.sqlDataReader["Fecha_creacion"]) 
                            : null,
                        FechaActualizacion = _conn.sqlDataReader["FechaActualizacion"] != DBNull.Value 
                            ? Convert.ToDateTime(_conn.sqlDataReader["FechaActualizacion"]) 
                            : null
                    };
                    categorias.Add(categoria);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categorías: {ex.Message}");
            }
            finally
            {
                _conn.sqlDataReader?.Close();
                _conn.connection.Close();
            }
            return categorias;
        }

        public Categoria ObtenerCategoria(int id)
        {
            Categoria categoria = null;
            try
            {
                _conn.connection.Open();
                string query = "SELECT Id, Nombre, Descripcion, Fecha_creacion, FechaActualizacion FROM Categorias WHERE Id = @Id";
                _conn.sqlCommand = new SqlCommand(query, _conn.connection);
                _conn.sqlCommand.Parameters.AddWithValue("@Id", id);

                _conn.sqlDataReader = _conn.sqlCommand.ExecuteReader();

                if (_conn.sqlDataReader.Read())
                {
                    categoria = new Categoria
                    {
                        Id = Convert.ToInt32(_conn.sqlDataReader["Id"]),
                        Nombre = _conn.sqlDataReader["Nombre"].ToString(),
                        Descripcion = _conn.sqlDataReader["Descripcion"] != DBNull.Value 
                            ? _conn.sqlDataReader["Descripcion"].ToString() 
                            : null,
                        FechaCreacion = _conn.sqlDataReader["Fecha_creacion"] != DBNull.Value 
                            ? Convert.ToDateTime(_conn.sqlDataReader["Fecha_creacion"]) 
                            : null,
                        FechaActualizacion = _conn.sqlDataReader["FechaActualizacion"] != DBNull.Value 
                            ? Convert.ToDateTime(_conn.sqlDataReader["FechaActualizacion"]) 
                            : null
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categoría: {ex.Message}");
            }
            finally
            {
                _conn.sqlDataReader?.Close();
                _conn.connection.Close();
            }
            return categoria;
        }

        public bool CrearCategoria(Categoria categoria)
        {
            bool resultado = false;
            try
            {
                _conn.connection.Open();
                string query = @"INSERT INTO Categorias (Nombre, Descripcion, Fecha_creacion, FechaActualizacion) 
                                VALUES (@Nombre, @Descripcion, GETDATE(), GETDATE());
                                SELECT SCOPE_IDENTITY();";
                
                _conn.sqlCommand = new SqlCommand(query, _conn.connection);
                _conn.sqlCommand.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                _conn.sqlCommand.Parameters.AddWithValue("@Descripcion", (object)categoria.Descripcion ?? DBNull.Value);

                // Obtener el ID generado
                var id = Convert.ToInt32(_conn.sqlCommand.ExecuteScalar());
                categoria.Id = id;
                resultado = id > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear categoría: {ex.Message}");
            }
            finally
            {
                _conn.connection.Close();
            }
            return resultado;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            bool resultado = false;
            try
            {
                _conn.connection.Open();
                string query = @"UPDATE Categorias 
                                SET Nombre = @Nombre, 
                                    Descripcion = @Descripcion, 
                                    FechaActualizacion = GETDATE() 
                                WHERE Id = @Id";
                
                _conn.sqlCommand = new SqlCommand(query, _conn.connection);
                _conn.sqlCommand.Parameters.AddWithValue("@Id", categoria.Id);
                _conn.sqlCommand.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                _conn.sqlCommand.Parameters.AddWithValue("@Descripcion", (object)categoria.Descripcion ?? DBNull.Value);

                int filasAfectadas = _conn.sqlCommand.ExecuteNonQuery();
                resultado = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar categoría: {ex.Message}");
            }
            finally
            {
                _conn.connection.Close();
            }
            return resultado;
        }

        public bool EliminarCategoria(int id)
        {
            bool resultado = false;
            try
            {
                _conn.connection.Open();
                // Realizar un borrado físico en lugar de lógico
                string query = "DELETE FROM Categorias WHERE Id = @Id";
                _conn.sqlCommand = new SqlCommand(query, _conn.connection);
                _conn.sqlCommand.Parameters.AddWithValue("@Id", id);

                int filasAfectadas = _conn.sqlCommand.ExecuteNonQuery();
                resultado = filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar categoría: {ex.Message}");
            }
            finally
            {
                _conn.connection.Close();
            }
            return resultado;
        }
    }
} 