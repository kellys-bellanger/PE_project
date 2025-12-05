using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UAMarketSimple;

namespace UAMarketSimple.Pages
{
    public class IndexModel : PageModel
    {
        public List<Producto> MisProductos { get; set; } = new List<Producto>();
        
        [BindProperty(SupportsGet = true)]
        public string Busqueda { get; set; } = "";
        
        [BindProperty]
        public string ProductoId { get; set; } = "";

        public void OnGet()
        {
            // Cargar datos de prueba
            Datos.CargarDatosDePrueba();
            
            // Lógica de búsqueda
            if (string.IsNullOrEmpty(Busqueda))
            {
                MisProductos = Datos.ListaProductos;
            }
            else
            {
                MisProductos = new List<Producto>();
                foreach (var p in Datos.ListaProductos)
                {
                    if (p.Nombre.ToLower().Contains(Busqueda.ToLower()))
                    {
                        MisProductos.Add(p);
                    }
                }
            }
        }

       public IActionResult OnPostToggleFavoritoAjax([FromForm] string productoId)
{
    if (Datos.UsuarioLogueado == null)
    {
        return new JsonResult(new { error = "No autenticado" });
    }
    
    if (!string.IsNullOrEmpty(productoId))
    {
        Datos.ToggleFavorito(productoId);
    }
    
    var resultado = new
    {
        esFavorito = Datos.EsFavorito(productoId),
        totalFavoritos = Datos.ContarFavoritosProducto(productoId),
        totalUsuario = Datos.ObtenerFavoritos().Count
    };
    
    return new JsonResult(resultado);
}
    }
}   