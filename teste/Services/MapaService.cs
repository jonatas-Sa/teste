using teste.Data;
using teste.Models;
using teste.Models.DTOs;
using teste.Utils;
using Microsoft.EntityFrameworkCore;

namespace teste.Services
{
    /// <summary>
    /// Serviço para gerenciamento de visualizações de mapa
    /// </summary>
    public class MapaService
    {
        private readonly ApplicationDbContext _context;

        public MapaService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca todos os dados necessários para renderizar o mapa completo do canteiro
        /// </summary>
        public async Task<MapaCanteiroDTO> BuscarMapaCompletoAsync(int canteiroObraId)
        {
            var canteiro = await _context.CanteirosObra
                .Include(c => c.LocalizacaoCentral)
                .FirstOrDefaultAsync(c => c.Id == canteiroObraId);

            if (canteiro == null)
                throw new Exception("Canteiro de obras não encontrado");

            var locais = await BuscarLocaisSolicitacaoParaMapaAsync(canteiroObraId);
            var depositos = await BuscarDepositosParaMapaAsync(canteiroObraId);
            var maquinarios = await BuscarMaquinariosParaMapaAsync(canteiroObraId);
            var solicitacoes = await BuscarSolicitacoesAtivasParaMapaAsync(canteiroObraId);

            return new MapaCanteiroDTO
            {
                CanteiroObraId = canteiro.Id,
                CanteiroNome = canteiro.Nome,
                CentroLatitude = canteiro.LocalizacaoCentral.Latitude,
                CentroLongitude = canteiro.LocalizacaoCentral.Longitude,
                LocaisSolicitacao = locais,
                Depositos = depositos,
                Maquinarios = maquinarios,
                SolicitacoesAtivas = solicitacoes
            };
        }

        /// <summary>
        /// Busca locais de solicitação para exibir no mapa
        /// </summary>
        private async Task<List<PontoMapaDTO>> BuscarLocaisSolicitacaoParaMapaAsync(int canteiroObraId)
        {
            var locais = await _context.LocaisSolicitacao
                .Include(l => l.Localizacao)
                .Where(l => l.CanteiroObraId == canteiroObraId && l.Ativo)
                .ToListAsync();

            return locais.Select(l => new PontoMapaDTO
            {
                Id = l.Id,
                Nome = l.Identificador,
                Tipo = "LocalSolicitacao",
                Latitude = l.Localizacao.Latitude,
                Longitude = l.Localizacao.Longitude,
                Descricao = l.Descricao,
                DadosAdicionais = new Dictionary<string, object>
                {
                    { "TipoConstrucao", l.TipoConstrucao ?? "" },
                    { "Lote", l.NumeroLote ?? "" },
                    { "Quadra", l.Quadra ?? "" },
                    { "PercentualConclusao", l.PercentualConclusao ?? 0 },
                    { "AreaConstruida", l.AreaConstruida ?? 0 }
                }
            }).ToList();
        }

        /// <summary>
        /// Busca depósitos para exibir no mapa
        /// </summary>
        private async Task<List<PontoMapaDTO>> BuscarDepositosParaMapaAsync(int canteiroObraId)
        {
            var depositos = await _context.Depositos
                .Include(d => d.Localizacao)
                .Include(d => d.EstoqueMateriais)
                .Where(d => d.CanteiroObraId == canteiroObraId && d.Ativo)
                .ToListAsync();

            return depositos.Select(d => new PontoMapaDTO
            {
                Id = d.Id,
                Nome = d.Nome,
                Tipo = "Deposito",
                Latitude = d.Localizacao.Latitude,
                Longitude = d.Localizacao.Longitude,
                Descricao = d.Descricao,
                DadosAdicionais = new Dictionary<string, object>
                {
                    { "Responsavel", d.ResponsavelNome ?? "" },
                    { "Telefone", d.ResponsavelTelefone ?? "" },
                    { "QuantidadeMateriais", d.EstoqueMateriais.Count },
                    { "Coberto", d.DepositoCoberto }
                }
            }).ToList();
        }

        /// <summary>
        /// Busca maquinários para exibir no mapa
        /// </summary>
        private async Task<List<MaquinarioMapaDTO>> BuscarMaquinariosParaMapaAsync(int canteiroObraId)
        {
            var maquinarios = await _context.Maquinarios
                .Include(m => m.LocalizacaoAtual)
                .Include(m => m.OperadorAtual)
                .Where(m => m.CanteiroObraId == canteiroObraId && m.Ativo)
                .ToListAsync();

            return maquinarios
                .Where(m => m.LocalizacaoAtual != null)
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
        /// Busca solicitações ativas para exibir no mapa
        /// </summary>
        private async Task<List<SolicitacaoMapaDTO>> BuscarSolicitacoesAtivasParaMapaAsync(int canteiroObraId)
        {
            var statusAtivos = new[]
            {
                StatusSolicitacao.Pendente,
                StatusSolicitacao.EmAnalise,
                StatusSolicitacao.Aprovada,
                StatusSolicitacao.EmTransporte,
                StatusSolicitacao.ColetandoMaterial,
                StatusSolicitacao.EmEntrega
            };

            var solicitacoes = await _context.Solicitacoes
                .Include(s => s.LocalSolicitacao)
                    .ThenInclude(l => l!.Localizacao)
                .Include(s => s.Deposito)
                    .ThenInclude(d => d!.Localizacao)
                .Include(s => s.Itens)
                .Where(s => s.CanteiroObraId == canteiroObraId && 
                           statusAtivos.Contains(s.Status))
                .ToListAsync();

            return solicitacoes.Select(s => new SolicitacaoMapaDTO
            {
                Id = s.Id,
                NumeroSolicitacao = s.NumeroSolicitacao,
                Status = s.Status.ToString(),
                Prioridade = s.Prioridade,
                LocalSolicitacaoId = s.LocalSolicitacaoId,
                OrigemLatitude = s.LocalSolicitacao?.Localizacao?.Latitude ?? 0,
                OrigemLongitude = s.LocalSolicitacao?.Localizacao?.Longitude ?? 0,
                DepositoId = s.DepositoId,
                DestinoLatitude = s.Deposito?.Localizacao?.Latitude,
                DestinoLongitude = s.Deposito?.Localizacao?.Longitude,
                MaquinarioId = s.MaquinarioId,
                QuantidadeItens = s.Itens.Count,
                PesoTotal = s.CalcularPesoTotal()
            }).ToList();
        }

        /// <summary>
        /// Calcula a melhor rota entre depósito e local de solicitação (simplificado)
        /// </summary>
        public async Task<Dictionary<string, object>> CalcularRotaAsync(
            int depositoId, 
            int localSolicitacaoId)
        {
            var deposito = await _context.Depositos
                .Include(d => d.Localizacao)
                .FirstOrDefaultAsync(d => d.Id == depositoId);

            var local = await _context.LocaisSolicitacao
                .Include(l => l.Localizacao)
                .FirstOrDefaultAsync(l => l.Id == localSolicitacaoId);

            if (deposito == null || local == null)
                throw new Exception("Depósito ou Local de solicitação não encontrados");

            var distancia = deposito.Localizacao.CalcularDistancia(local.Localizacao);
            
            // Estimativa simples de tempo (30 km/h)
            var tempoEstimadoMinutos = (distancia / 1000) / 30 * 60;

            return new Dictionary<string, object>
            {
                { "OrigemLatitude", deposito.Localizacao.Latitude },
                { "OrigemLongitude", deposito.Localizacao.Longitude },
                { "DestinoLatitude", local.Localizacao.Latitude },
                { "DestinoLongitude", local.Localizacao.Longitude },
                { "DistanciaMetros", distancia },
                { "DistanciaKm", Math.Round(distancia / 1000, 2) },
                { "TempoEstimadoMinutos", Math.Round(tempoEstimadoMinutos, 0) }
            };
        }

        /// <summary>
        /// Busca estatísticas do canteiro para dashboard
        /// </summary>
        public async Task<Dictionary<string, object>> BuscarEstatisticasCanteiroAsync(int canteiroObraId)
        {
            var totalLocais = await _context.LocaisSolicitacao
                .CountAsync(l => l.CanteiroObraId == canteiroObraId && l.Ativo);

            var totalDepositos = await _context.Depositos
                .CountAsync(d => d.CanteiroObraId == canteiroObraId && d.Ativo);

            var totalMaquinarios = await _context.Maquinarios
                .CountAsync(m => m.CanteiroObraId == canteiroObraId && m.Ativo);

            var maquinariosDisponiveis = await _context.Maquinarios
                .CountAsync(m => m.CanteiroObraId == canteiroObraId && 
                                m.Status == StatusMaquinario.Disponivel && 
                                m.Ativo);

            var solicitacoesPendentes = await _context.Solicitacoes
                .CountAsync(s => s.CanteiroObraId == canteiroObraId && 
                                s.Status == StatusSolicitacao.Pendente);

            var solicitacoesEmAndamento = await _context.Solicitacoes
                .CountAsync(s => s.CanteiroObraId == canteiroObraId && 
                                (s.Status == StatusSolicitacao.Aprovada ||
                                 s.Status == StatusSolicitacao.EmTransporte ||
                                 s.Status == StatusSolicitacao.ColetandoMaterial ||
                                 s.Status == StatusSolicitacao.EmEntrega));

            var solicitacoesHoje = await _context.Solicitacoes
                .CountAsync(s => s.CanteiroObraId == canteiroObraId && 
                                s.DataCriacao.Date == DateTime.Today);

            return new Dictionary<string, object>
            {
                { "TotalLocaisSolicitacao", totalLocais },
                { "TotalDepositos", totalDepositos },
                { "TotalMaquinarios", totalMaquinarios },
                { "MaquinariosDisponiveis", maquinariosDisponiveis },
                { "SolicitacoesPendentes", solicitacoesPendentes },
                { "SolicitacoesEmAndamento", solicitacoesEmAndamento },
                { "SolicitacoesHoje", solicitacoesHoje },
                { "DataAtualizacao", TimeZoneHelper.GetBrasiliaNow() }
            };
        }
    }
}
