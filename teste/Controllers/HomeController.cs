using Microsoft.AspNetCore.Mvc;

namespace teste.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OperadorMaquinario()
        {
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "operador-maquinario.html"),
                "text/html");
        }

        public IActionResult MapaApontador()
        {
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mapa-apontador.html"),
                "text/html");
        }
    }
}
