using Microsoft.AspNetCore.Mvc;
using teste.Models.DTOs;
using teste.Services;

namespace teste.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de maquinários
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MaquinarioController : ControllerBase
    {
        private readonly MaquinarioService _maquinarioService;
        private readonly ILogger<MaquinarioController> _logger;

        public MaquinarioController(
            MaquinarioService maquinarioService,
            ILogger<MaquinarioController> logger)
        {
            _maquinarioService = maquinarioService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint chamado pelo tablet para atualizar a localização GPS do maquinário
        /// Este endpoint é chamado periodicamente (ex: a cada 10-30 segundos)
        /// </summary>
        /// <param name="dto">Dados de localização do GPS do tablet</param>
        /// <returns>Confirmação da atualização</returns>
        [HttpPost("atualizar-localizacao")]
        public async Task<IActionResult> AtualizarLocalizacao([FromBody] AtualizarLocalizacaoMaquinarioDTO dto)
        {
            try
            {
                _logger.LogInformation(
                    "Atualizando localização do maquinário {MaquinarioId}: Lat={Latitude}, Long={Longitude}",
                    dto.MaquinarioId, dto.Latitude, dto.Longitude);

                var sucesso = await _maquinarioService.AtualizarLocalizacaoAsync(dto);

                if (sucesso)
                {
                    return Ok(new
                    {
                        Sucesso = true,
                        Mensagem = "Localização atualizada com sucesso",
                        DataAtualizacao = dto.Timestamp
                    });
                }

                return BadRequest(new { Sucesso = false, Mensagem = "Erro ao atualizar localização" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar localização do maquinário {MaquinarioId}", dto.MaquinarioId);
                return StatusCode(500, new
                {
                    Sucesso = false,
                    Mensagem = "Erro interno ao processar localização",
                    Erro = ex.Message
                });
            }
        }

        /// <summary>
        /// Busca todos os maquinários do canteiro com suas localizações atuais
        /// Usado pelo mapa para exibir os maquinários em tempo real
        /// </summary>
        [HttpGet("mapa/{canteiroObraId}")]
        public async Task<IActionResult> BuscarMaquinariosParaMapa(int canteiroObraId)
        {
            try
            {
                var maquinarios = await _maquinarioService.BuscarMaquinariosParaMapaAsync(canteiroObraId);
                return Ok(maquinarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar maquinários para o mapa");
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        /// <summary>
        /// Busca um maquinário específico com detalhes completos
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var maquinario = await _maquinarioService.BuscarPorIdAsync(id);
                
                if (maquinario == null)
                    return NotFound(new { Mensagem = "Maquinário não encontrado" });

                return Ok(maquinario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar maquinário {Id}", id);
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        /// <summary>
        /// Atribui um operador a um maquinário
        /// </summary>
        [HttpPost("{maquinarioId}/atribuir-operador/{operadorId}")]
        public async Task<IActionResult> AtribuirOperador(int maquinarioId, int operadorId)
        {
            try
            {
                var sucesso = await _maquinarioService.AtribuirOperadorAsync(maquinarioId, operadorId);
                
                if (sucesso)
                    return Ok(new { Mensagem = "Operador atribuído com sucesso" });

                return BadRequest(new { Mensagem = "Erro ao atribuir operador" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atribuir operador");
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        /// <summary>
        /// Altera o status de um maquinário
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> AlterarStatus(int id, [FromBody] AlterarStatusMaquinarioDTO dto)
        {
            try
            {
                var sucesso = await _maquinarioService.AlterarStatusAsync(id, dto.NovoStatus);
                
                if (sucesso)
                    return Ok(new { Mensagem = "Status alterado com sucesso" });

                return BadRequest(new { Mensagem = "Erro ao alterar status" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status do maquinário {Id}", id);
                return StatusCode(500, new { Erro = ex.Message });
            }
        }

        /// <summary>
        /// Busca maquinários que precisam de manutenção
        /// </summary>
        [HttpGet("manutencao/{canteiroObraId}")]
        public async Task<IActionResult> BuscarMaquinariosParaManutencao(int canteiroObraId)
        {
            try
            {
                var maquinarios = await _maquinarioService.BuscarMaquinariosParaManutencaoAsync(canteiroObraId);
                return Ok(maquinarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar maquinários para manutenção");
                return StatusCode(500, new { Erro = ex.Message });
            }
        }
    }

    /// <summary>
    /// DTO para alterar status do maquinário
    /// </summary>
    public class AlterarStatusMaquinarioDTO
    {
        public Models.StatusMaquinario NovoStatus { get; set; }
    }
}
