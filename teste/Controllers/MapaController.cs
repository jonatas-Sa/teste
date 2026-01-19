using Microsoft.AspNetCore.Mvc;
using teste.Models.DTOs;
using teste.Services;

namespace teste.Controllers
{
    /// <summary>
    /// Controller para dados do mapa
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MapaController : ControllerBase
    {
        private readonly MapaService _mapaService;
        private readonly ILogger<MapaController> _logger;

        public MapaController(
            MapaService mapaService,
            ILogger<MapaController> logger)
        {
            _mapaService = mapaService;
            _logger = logger;
        }

        /// <summary>
        /// Busca todos os dados necessários para renderizar o mapa completo
        /// </summary>
        [HttpGet("completo/{canteiroObraId}")]
        public async Task<IActionResult> BuscarMapaCompleto(int canteiroObraId)
        {
            try
            {
                var mapa = await _mapaService.BuscarMapaCompletoAsync(canteiroObraId);
                return Ok(mapa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar mapa completo");
                return StatusCode(500, new { Erro = ex.Message });
            }
        }
    }
}
