namespace UAMarketSimple
{
    // 1. ACTUALIZA LA CLASE PRODUCTO
    public class Producto
    {
        // Generamos un ID único automático cada vez que se crea un producto
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        
        public string Nombre { get; set; } = "";
        public int Precio { get; set; }
        public string Descripcion { get; set; } = "";
        public string FotoUrl { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string CifVendedor { get; set; } = "";
        public int Likes { get; set; } = 0;
    }

    public class Usuario
    {
        public string CIF { get; set; } = "";
        public string PIN { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Telefono { get; set; } = "";
    }

    // --- 2. BASE DE DATOS Y LÓGICA (Todo en uno) ---
    public static class Datos
    {
        // ESTAS SON LAS LÍNEAS QUE FALTABAN:
        public static List<Producto> ListaProductos = new List<Producto>();
        public static List<Usuario> ListaUsuarios = new List<Usuario>();
        
        // Variable para la sesión
        public static Usuario? UsuarioLogueado = null;
        
        // Función para llenar datos de prueba
        public static void CargarDatosDePrueba()
        {
            // Solo cargamos si está vacío
            if (ListaProductos.Count == 0)
            {
                ListaProductos.Add(new Producto { 
                    Nombre="Calculadora", 
                    Precio=200, 
                    Descripcion="Casio FX", 
                    Telefono="86319685", 
                    FotoUrl="/imagenes/calculadora.jpeg" 
                });

                ListaProductos.Add(new Producto { 
                    Nombre="Bata laboratorio", 
                    Precio=500, 
                    Descripcion="Marca Dior -- Color blanco", 
                    Telefono="87168339", 
                    FotoUrl="/imagenes/bata.jpeg" 
                });
            }

            // Usuario Admin por defecto
            if (ListaUsuarios.Count == 0)
            {
                ListaUsuarios.Add(new Usuario { Nombre="Admin", CIF="123456", PIN="1234" });
            }
        }

        // Función Login
        public static bool IniciarSesion(string cif, string pin)
        {
            foreach (var u in ListaUsuarios)
            {
                if (u.CIF == cif && u.PIN == pin)
                {
                    UsuarioLogueado = u;
                    return true;
                }
            }
            return false;
        }

        // Función Salir
        public static void CerrarSesion()
        {
            UsuarioLogueado = null;
        }

        // Función para eliminar un producto por ID
        public static void EliminarProducto(string idParaBorrar)
        {
            // Algoritmo Estructurado: Recorrer y eliminar
            for (int i = 0; i < ListaProductos.Count; i++)
            {
                if (ListaProductos[i].Id == idParaBorrar)
                {
                    ListaProductos.RemoveAt(i);
                    break;
                }
            }
        }

        // ========== SISTEMA DE FAVORITOS ==========

        // Lista de IDs de productos favoritos (por usuario CIF)
        private static Dictionary<string, List<string>> _favoritosPorUsuario = new();

        // Agregar/quitar producto de favoritos
        public static void ToggleFavorito(string productoId)
        {
            if (UsuarioLogueado == null) return;
            
            string cifUsuario = UsuarioLogueado.CIF;
            
            if (!_favoritosPorUsuario.ContainsKey(cifUsuario))
            {
                _favoritosPorUsuario[cifUsuario] = new List<string>();
            }
            
            var listaFavoritos = _favoritosPorUsuario[cifUsuario];
            
            if (listaFavoritos.Contains(productoId))
            {
                listaFavoritos.Remove(productoId);
            }
            else
            {
                listaFavoritos.Add(productoId);
            }
        }

        // Verificar si un producto es favorito del usuario actual
        public static bool EsFavorito(string productoId)
        {
            if (UsuarioLogueado == null) return false;
            
            string cifUsuario = UsuarioLogueado.CIF;
            
            if (!_favoritosPorUsuario.ContainsKey(cifUsuario)) return false;
            
            return _favoritosPorUsuario[cifUsuario].Contains(productoId);
        }

        // Obtener lista de productos favoritos del usuario actual
        public static List<Producto> ObtenerFavoritos()
        {
            var productosFavoritos = new List<Producto>();
            
            if (UsuarioLogueado == null) return productosFavoritos;
            
            string cifUsuario = UsuarioLogueado.CIF;
            
            if (!_favoritosPorUsuario.ContainsKey(cifUsuario)) return productosFavoritos;
            
            foreach (var productoId in _favoritosPorUsuario[cifUsuario])
            {
                var producto = ListaProductos.FirstOrDefault(p => p.Id == productoId);
                if (producto != null)
                {
                    productosFavoritos.Add(producto);
                }
            }
            
            return productosFavoritos;
        }

        // Contar cuántos favoritos tiene un producto (todos los usuarios)
        public static int ContarFavoritosProducto(string productoId)
        {
            int total = 0;
            
            foreach (var listaUsuario in _favoritosPorUsuario.Values)
            {
                if (listaUsuario.Contains(productoId))
                {
                    total++;
                }
            }
            
            return total;
        }
    }  // ← CIERRA LA CLASE Datos
}  // ← CIERRA EL NAMESPACE