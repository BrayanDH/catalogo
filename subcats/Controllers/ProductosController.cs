using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using subcats.customClass;
using subcats.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace subcats.Controllers
{
    public class ProductosController : Controller
    {
        private readonly Dao _db;
        private readonly CategoriaService _categoriaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor que maneja la inyección de dependencias
        public ProductosController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = new Dao();
            _categoriaService = new CategoriaService();
        }

        // GET: Productos
        public IActionResult Index()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var productos = _db.GetAllProductos();
                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener los productos: " + ex.Message;
                return View(new List<Producto>());
            }
        }

        // GET: Productos/Details/5
        public IActionResult Details(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
                }

                // Si el producto tiene categoría, cargar su nombre
                if (producto.CategoriaId.HasValue)
                {
                    var categoria = _categoriaService.ObtenerCategoria(producto.CategoriaId.Value);
                    ViewBag.NombreCategoria = categoria?.Nombre ?? "Categoría no encontrada";
                }

                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Cargar las categorías y proveedores para el select
            CargarCategorias();
            CargarProveedores();

            // Inicializar producto con valores por defecto
            var producto = new Producto
            {
                Precio = 0.01m,
                Impuesto = 0,
                Stock = 0,
                Fecha_creacion = DateTime.Now
            };

            return View(producto);
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                // Excluir Imagen de la validación del modelo
                ModelState.Remove("Imagen");

                // Cargar categorías para el select (en caso de error)
                CargarCategorias();

                // Imprimir todos los valores recibidos para depuración
                Console.WriteLine($"Valores recibidos: Nombre={producto.Nombre}, Precio={producto.Precio}, Impuesto={producto.Impuesto}, Stock={producto.Stock}");
                
                // Inicializar valores por defecto para evitar errores de validación
                if (producto.Precio <= 0)
                {
                    producto.Precio = 0.01m;
                }
                
                if (producto.Impuesto < 0)
                {
                    producto.Impuesto = 0;
                }
                
                if (producto.Stock < 0)
                {
                    producto.Stock = 0;
                }

                // Manejar la carga de la imagen
                if (producto.ImagenFile != null)
                {
                    Console.WriteLine($"Procesando archivo: {producto.ImagenFile.FileName}, tamaño: {producto.ImagenFile.Length} bytes");
                    
                    if (producto.ImagenFile.Length > 10 * 1024 * 1024) // 10MB máximo
                    {
                        ModelState.AddModelError("ImagenFile", "La imagen es demasiado grande. El tamaño máximo es 10MB.");
                        TempData["ErrorMessage"] = "La imagen es demasiado grande. El tamaño máximo es 10MB.";
                        return View(producto);
                    }

                    string extension = Path.GetExtension(producto.ImagenFile.FileName).ToLower();
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif")
                    {
                        ModelState.AddModelError("ImagenFile", "Solo se permiten archivos de imagen con formato JPG, JPEG, PNG o GIF.");
                        TempData["ErrorMessage"] = "Solo se permiten archivos de imagen con formato JPG, JPEG, PNG o GIF.";
                        return View(producto);
                    }

                    // Convertir la imagen a bytes[]
                    try
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await producto.ImagenFile.CopyToAsync(memoryStream);
                            producto.Imagen = memoryStream.ToArray();
                            Console.WriteLine($"Imagen convertida a bytes: {producto.Imagen.Length} bytes");
                            
                            // Verificar que la imagen se convirtió correctamente
                            if (producto.Imagen == null || producto.Imagen.Length == 0)
                            {
                                ModelState.AddModelError("ImagenFile", "No se pudo leer la imagen. Intente con otra imagen.");
                                TempData["ErrorMessage"] = "No se pudo leer la imagen. Intente con otra imagen.";
                                return View(producto);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al convertir la imagen: {ex.Message}");
                        ModelState.AddModelError("ImagenFile", "Error al procesar la imagen: " + ex.Message);
                        TempData["ErrorMessage"] = "Error al procesar la imagen.";
                        return View(producto);
                    }
                }
                else
                {
                    Console.WriteLine("No se ha proporcionado un archivo de imagen");
                    // La imagen es opcional, no mostrar error
                    producto.Imagen = null;
                }

                // Mostrar todos los errores del ModelState para depuración
                ImprimirErroresModelState();

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Modelo inválido. Errores encontrados.");
                    // Agregar mensaje de error general
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor revise los campos marcados.";
                    return View(producto);
                }

                Console.WriteLine("Modelo válido. Intentando guardar el producto...");
                
                try
                {
                    int idProducto = _db.InsertarProducto(producto);
                    if (idProducto > 0)
                    {
                        Console.WriteLine($"Producto creado exitosamente con ID: {idProducto}");
                    TempData["SuccessMessage"] = "Producto creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine("No se pudo crear el producto en la base de datos.");
                        TempData["ErrorMessage"] = "No se pudo crear el producto en la base de datos.";
                        return View(producto);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al insertar el producto en la base de datos: {ex.Message}");
                    TempData["ErrorMessage"] = $"Error al guardar: {ex.Message}";
                return View(producto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                TempData["ErrorMessage"] = $"Error al crear el producto: {ex.Message}";
                return View(producto);
            }
        }

        // GET: Productos/Edit/5
        public IActionResult Edit(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
                }

                // Cargar las categorías y proveedores para el select
                CargarCategorias();
                CargarProveedores();

                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Productos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                // Excluir Imagen de la validación del modelo
                ModelState.Remove("Imagen");

                // Cargar las categorías para el select (en caso de error)
                CargarCategorias();

                if (id != producto.Id_producto)
                {
                    return NotFound();
                }

                // Imprimir todos los valores recibidos para depuración
                Console.WriteLine($"Valores recibidos: ID={producto.Id_producto}, Nombre={producto.Nombre}, Precio={producto.Precio}, Impuesto={producto.Impuesto}, Stock={producto.Stock}");
                
                // Inicializar valores por defecto para evitar errores de validación
                if (producto.Precio <= 0)
                {
                    producto.Precio = 0.01m;
                }
                
                if (producto.Impuesto < 0)
                {
                    producto.Impuesto = 0;
                }
                
                if (producto.Stock < 0)
                {
                    producto.Stock = 0;
                }

                // Recuperamos el producto actual para mantener la imagen si no se carga una nueva
                var productoActual = _db.GetProducto(id.ToString());
                if (productoActual == null || productoActual.Id_producto == 0)
                {
                    TempData["ErrorMessage"] = "No se encontró el producto a editar.";
                    return RedirectToAction(nameof(Index));
                }

                // Manejar la carga de imagen, si hay una nueva
                if (producto.ImagenFile != null)
                {
                    Console.WriteLine($"Editando producto con ID: {id}. Procesando archivo: {producto.ImagenFile.FileName}, tamaño: {producto.ImagenFile.Length} bytes");
                    
                    if (producto.ImagenFile.Length > 10 * 1024 * 1024) // 10MB máximo
                    {
                        ModelState.AddModelError("ImagenFile", "La imagen es demasiado grande. El tamaño máximo es 10MB.");
                        TempData["ErrorMessage"] = "La imagen es demasiado grande. El tamaño máximo es 10MB.";
                        return View(producto);
                    }

                    string extension = Path.GetExtension(producto.ImagenFile.FileName).ToLower();
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif")
                    {
                        ModelState.AddModelError("ImagenFile", "Solo se permiten archivos de imagen con formato JPG, JPEG, PNG o GIF.");
                        TempData["ErrorMessage"] = "Solo se permiten archivos de imagen con formato JPG, JPEG, PNG o GIF.";
                        return View(producto);
                    }

                    // Convertir la imagen a bytes[]
                    try
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await producto.ImagenFile.CopyToAsync(memoryStream);
                            producto.Imagen = memoryStream.ToArray();
                            Console.WriteLine($"Nueva imagen convertida a bytes: {producto.Imagen.Length} bytes");
                            
                            // Verificar que la imagen se convirtió correctamente
                            if (producto.Imagen == null || producto.Imagen.Length == 0)
                            {
                                ModelState.AddModelError("ImagenFile", "No se pudo leer la imagen. Intente con otra imagen.");
                                TempData["ErrorMessage"] = "No se pudo leer la imagen. Intente con otra imagen.";
                                return View(producto);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al convertir la imagen: {ex.Message}");
                        ModelState.AddModelError("ImagenFile", "Error al procesar la imagen: " + ex.Message);
                        TempData["ErrorMessage"] = "Error al procesar la imagen.";
                        return View(producto);
                    }
                }
                else if (productoActual != null && productoActual.Imagen != null)
                {
                    // Si no se seleccionó nueva imagen, mantener la anterior
                    producto.Imagen = productoActual.Imagen;
                    Console.WriteLine($"Manteniendo imagen existente de {producto.Imagen.Length} bytes");
                }

                // Mostrar todos los errores del ModelState para depuración
                ImprimirErroresModelState();

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Modelo inválido para edición. Errores encontrados.");
                    // Agregar mensaje de error general
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor revise los campos marcados.";
                    return View(producto);
                }

                try
                {
                    Console.WriteLine("Modelo válido. Intentando actualizar el producto...");
                    bool actualizado = _db.ActualizarProducto(producto);
                    if (actualizado)
                    {
                        Console.WriteLine($"Producto actualizado correctamente. ID: {producto.Id_producto}");
                        TempData["SuccessMessage"] = "Producto actualizado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine($"No se pudo actualizar el producto en la base de datos. ID: {producto.Id_producto}");
                        TempData["ErrorMessage"] = "No se pudo actualizar el producto. Verifique los datos e intente nuevamente.";
                        return View(producto);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al actualizar el producto en la base de datos: {ex.Message}");
                    TempData["ErrorMessage"] = $"Error al guardar: {ex.Message}";
                return View(producto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al editar el producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                TempData["ErrorMessage"] = $"Error al actualizar el producto: {ex.Message}";
                return View(producto);
            }
        }

        // GET: Productos/Delete/5
        public IActionResult Delete(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto == null || producto.Id_producto == 0)
                {
                    return NotFound();
                }

                // Si el producto tiene categoría, cargar su nombre
                if (producto.CategoriaId.HasValue)
                {
                    var categoria = _categoriaService.ObtenerCategoria(producto.CategoriaId.Value);
                    ViewBag.NombreCategoria = categoria?.Nombre ?? "Categoría no encontrada";
                }

                return View(producto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                _db.EliminarProducto(id.ToString());
                TempData["SuccessMessage"] = "Producto eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Productos/GetImage/5
        public IActionResult GetImage(int id)
        {
            try
            {
                var producto = _db.GetProducto(id.ToString());
                if (producto?.Imagen == null || producto.Imagen.Length == 0)
                {
                    // Devolver una imagen por defecto o un error 404
                    return NotFound();
                }

                // Determinar el tipo de contenido basado en los bytes de la imagen
                string contentType = DeterminarTipoContenido(producto.Imagen);
                
                // Devolver la imagen
                return File(producto.Imagen, contentType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener imagen: {ex.Message}");
                return NotFound();
            }
        }

        // Método auxiliar para determinar el tipo de contenido basado en los bytes
        private string DeterminarTipoContenido(byte[] bytes)
        {
            // Verificar los primeros bytes del archivo para determinar el tipo
            if (bytes.Length >= 2)
            {
                if (bytes[0] == 0xFF && bytes[1] == 0xD8) // JPEG
                    return "image/jpeg";
                if (bytes[0] == 0x89 && bytes[1] == 0x50) // PNG
                    return "image/png";
                if (bytes[0] == 0x47 && bytes[1] == 0x49) // GIF
                    return "image/gif";
                if (bytes[0] == 0x42 && bytes[1] == 0x4D) // BMP
                    return "image/bmp";
            }

            // Por defecto, devolver como imagen JPEG
            return "image/jpeg";
        }

        // Método privado para cargar las categorías en ViewBag
        private void CargarCategorias()
        {
            try
            {
                var categorias = _categoriaService.ObtenerTodasCategorias();
                ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre");
            }
            catch (Exception)
            {
                ViewBag.Categorias = new SelectList(new List<Categoria>(), "Id", "Nombre");
            }
        }

        private void CargarProveedores()
        {
            try
            {
                var proveedores = _db.GetAllProveedores();
                ViewBag.Proveedores = new SelectList(proveedores, "Id_proveedor", "Nombre");
            }
            catch (Exception)
            {
                ViewBag.Proveedores = new SelectList(new List<Proveedor>(), "Id_proveedor", "Nombre");
            }
        }

        // Método para imprimir todos los errores del ModelState
        private void ImprimirErroresModelState()
        {
            Console.WriteLine($"Estado del ModelState: IsValid={ModelState.IsValid}, ErrorCount={ModelState.ErrorCount}");
            
            foreach (var key in ModelState.Keys)
            {
                var modelStateEntry = ModelState[key];
                if (modelStateEntry.Errors.Count > 0)
                {
                    Console.WriteLine($"Errores para el campo '{key}':");
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine($"  - {error.ErrorMessage}");
                        if (error.Exception != null)
                        {
                            Console.WriteLine($"  - Exception: {error.Exception.Message}");
                        }
                    }
                }
            }
        }
    }
}