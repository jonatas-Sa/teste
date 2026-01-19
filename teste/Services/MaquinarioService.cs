using teste.Data;
using teste.Models;
using teste.Models.DTOs;
using teste.Utils;
using Microsoft.EntityFrameworkCore;

namespace teste.Services
{
    /// <summary>
    /// Serviço para gerenciamento de maquinários e suas localizações
    /// </summary>
    public class MaquinarioService
    {
        private readonly ApplicationDbContext _context;

        public MaquinarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Atualiza a localização do maquinário via GPS (chamado pelo tablet)
        /// </summary>
        public async Task<bool> AtualizarLocalizacaoAsync(AtualizarLocalizacaoMaquinarioDTO dto)
        {
            var maquinario = await _context.Maquinarios
                .FirstOrDefaultAsync(m => m.Id == dto.MaquinarioId);

            if (maquinario == null)
                throw new Exception("Maquinário não encontrado");

            // Atualizar localização
            maquinario.AtualizarLocalizacao(dto.Latitude, dto.Longitude);
            maquinario.VelocidadeAtual = dto.VelocidadeAtual;
            maquinario.DataAtualizacao = TimeZoneHelper.GetBrasiliaNow();

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Busca todos os maquinários do canteiro para exibir no mapa
        /// </summary>
        public async Task<List<MaquinarioMapaDTO>> BuscarMaquinariosParaMapaAsync(int canteiroObraId)
        {
            var maquinarios = await _context.Maquinarios
                .Include(m => m.LocalizacaoAtual)
                .Include(m => m.OperadorAtual)
                .Where(m => m.CanteiroObraId == canteiroObraId && m.Ativo)
                .ToListAsync();

            return maquinarios
                .Where(m => m.LocalizacaoAtual != null) // Apenas os que têm localização
                .Select(m => new MaquinarioMapaDTO
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    Descricao = m.Descricao ?? "",
                    Tipo = m.Tipo.ToString(),
                    Status = m.Status.ToString(),
                    Latitude = m.LocalizacaoAtual!.Latitude,
                    Longitude = m.LocalizacaoAtual.Longitude,
                    DataUltimaAtualizacao = m.DataUltimaAtualizacaoGPS,
                    CapacidadeCarga = m.CapacidadeCarga,
                    CapacidadeVolume = m.CapacidadeVolume,
                    OperadorNome = m.OperadorAtual?.Nome,
                    EstaDisponivel = m.EstaDisponivel()
                })
                .ToList();
        }

        /// <summary>
        /// Busca informações detalhadas de um maquinário
        /// </summary>
        public async Task<Maquinario?> BuscarPorIdAsync(int id)
        {
            return await _context.Maquinarios
                .Include(m => m.LocalizacaoAtual)
                .Include(m => m.OperadorAtual)
                .Include(m => m.CanteiroObra)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <summary>
        /// Altera o status de um maquinário
        /// </summary>
        public async Task<bool> AlterarStatusAsync(int maquinarioId, StatusMaquinario novoStatus)
        {
            var maquinario = await _context.Maquinarios.FindAsync(maquinarioId);
            
            if (maquinario == null)
                throw new Exception("Maquinário não encontrado");

            maquinario.Status = novoStatus;
            maquinario.DataAtualizacao = TimeZoneHelper.GetBrasiliaNow();

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Atribui um operador a um maquinário
        /// </summary>
        public async Task<bool> AtribuirOperadorAsync(int maquinarioId, int operadorId)
        {
            var maquinario = await _context.Maquinarios.FindAsync(maquinarioId);
            var operador = await _context.OperadoresMaquinario.FindAsync(operadorId);

            if (maquinario == null)
                throw new Exception("Maquinário não encontrado");

            if (operador == null)
                throw new Exception("Operador não encontrado");

            // Remover operador do maquinário anterior, se houver
            if (operador.MaquinarioAtualId.HasValue)
            {
                var maquinarioAnterior = await _context.Maquinarios
                    .FindAsync(operador.MaquinarioAtualId.Value);
                
                if (maquinarioAnterior != null)
                {
                    maquinarioAnterior.OperadorAtualId = null;
                }
            }

            // Atribuir novo operador
            maquinario.OperadorAtualId = operadorId;
            operador.MaquinarioAtualId = maquinarioId;

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Verifica a disponibilidade de maquinários por tipo
        /// </summary>
        public async Task<Dictionary<TipoMaquinario, int>> BuscarDisponibilidadePorTipoAsync(int canteiroObraId)
        {
            var maquinarios = await _context.Maquinarios
                .Where(m => m.CanteiroObraId == canteiroObraId && 
                           m.Status == StatusMaquinario.Disponivel &&
                           m.Ativo)
                .GroupBy(m => m.Tipo)
                .Select(g => new { Tipo = g.Key, Quantidade = g.Count() })
                .ToListAsync();

            return maquinarios.ToDictionary(x => x.Tipo, x => x.Quantidade);
        }

        /// <summary>
        /// Busca maquinários que precisam de manutenção
        /// </summary>
        public async Task<List<Maquinario>> BuscarMaquinariosParaManutencaoAsync(int canteiroObraId)
        {
            var dataLimite = TimeZoneHelper.GetBrasiliaNow().AddDays(7); // Próximos 7 dias

            return await _context.Maquinarios
                .Include(m => m.OperadorAtual)
                .Where(m => m.CanteiroObraId == canteiroObraId && 
                           m.Ativo &&
                           (m.Status == StatusMaquinario.EmManutencao ||
                            (m.DataProximaManutencao.HasValue && 
                             m.DataProximaManutencao.Value <= dataLimite)))
                .OrderBy(m => m.DataProximaManutencao)
                .ToListAsync();
        }
    }
}
