using teste.Data;
using teste.Models;
using teste.Models.DTOs;
using teste.Utils;
using Microsoft.EntityFrameworkCore;

namespace teste.Services
{
    /// <summary>
    /// Serviço para gerenciamento de solicitações de materiais
    /// Exemplo de implementação da lógica de negócio
    /// </summary>
    public class SolicitacaoService
    {
        private readonly ApplicationDbContext _context;

        public SolicitacaoService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cria uma nova solicitação de materiais
        /// </summary>
        public async Task<Solicitacao> CriarSolicitacaoAsync(CriarSolicitacaoDTO dto)
        {
            // Buscar informações necessárias
            var local = await _context.LocaisSolicitacao
                .Include(l => l.CanteiroObra)
                .FirstOrDefaultAsync(l => l.Id == dto.LocalSolicitacaoId);

            if (local == null)
                throw new Exception("Local de solicitação não encontrado");

            var solicitador = await _context.Solicitadores
                .FirstOrDefaultAsync(s => s.Id == dto.SolicitadorId);

            if (solicitador == null)
                throw new Exception("Solicitador não encontrado");

            // Gerar número de solicitação
            var ultimaSolicitacao = await _context.Solicitacoes
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            var numeroSequencial = (ultimaSolicitacao?.Id ?? 0) + 1;
            var numeroSolicitacao = Solicitacao.GerarNumeroSolicitacao(numeroSequencial);

            // Criar solicitação
            var solicitacao = new Solicitacao
            {
                NumeroSolicitacao = numeroSolicitacao,
                LocalSolicitacaoId = dto.LocalSolicitacaoId,
                SolicitadorId = dto.SolicitadorId,
                CanteiroObraId = local.CanteiroObraId,
                DataNecessidade = dto.DataNecessidade,
                Justificativa = dto.Justificativa,
                Prioridade = dto.Prioridade,
                Status = StatusSolicitacao.Pendente,
                DataCriacao = TimeZoneHelper.GetBrasiliaNow()
            };

            // Adicionar itens
            foreach (var itemDto in dto.Itens)
            {
                var material = await _context.Materiais.FindAsync(itemDto.MaterialId);
                if (material == null)
                    throw new Exception($"Material {itemDto.MaterialId} não encontrado");

                var item = new ItemSolicitacao
                {
                    MaterialId = itemDto.MaterialId,
                    Quantidade = itemDto.Quantidade,
                    Observacoes = itemDto.Observacoes
                };

                solicitacao.Itens.Add(item);
            }

            // Salvar
            _context.Solicitacoes.Add(solicitacao);
            await _context.SaveChangesAsync();

            // Registrar histórico
            await RegistrarHistoricoAsync(solicitacao.Id, StatusSolicitacao.Pendente, 
                solicitador.Id, solicitador.Nome, "Solicitação criada");

            return solicitacao;
        }

        /// <summary>
        /// Busca depósitos que têm todos os materiais disponíveis, ordenados por distância
        /// </summary>
        public async Task<List<DepositoDisponibilidadeDTO>> BuscarDepositosDisponiveisAsync(int solicitacaoId)
        {
            var solicitacao = await _context.Solicitacoes
                .Include(s => s.LocalSolicitacao)
                    .ThenInclude(l => l!.Localizacao)
                .Include(s => s.Itens)
                    .ThenInclude(i => i.Material)
                .FirstOrDefaultAsync(s => s.Id == solicitacaoId);

            if (solicitacao == null)
                throw new Exception("Solicitação não encontrada");

            if (solicitacao.LocalSolicitacao?.Localizacao == null)
                throw new Exception("Local de solicitação ou localização não encontrada");

            var depositos = await _context.Depositos
                .Include(d => d.Localizacao)
                .Include(d => d.EstoqueMateriais)
                    .ThenInclude(e => e.Material)
                .Where(d => d.CanteiroObraId == solicitacao.CanteiroObraId && d.Ativo)
                .ToListAsync();

            var resultado = new List<DepositoDisponibilidadeDTO>();

            foreach (var deposito in depositos)
            {
                if (deposito.Localizacao == null) continue;
                
                var distancia = solicitacao.LocalSolicitacao.Localizacao.CalcularDistancia(deposito.Localizacao);
                var temTodosMateriais = true;
                var materiaisStatus = new List<MaterialDisponibilidadeDTO>();

                foreach (var item in solicitacao.Itens)
                {
                    var estoque = deposito.EstoqueMateriais
                        .FirstOrDefault(e => e.MaterialId == item.MaterialId);

                    var quantidadeDisponivel = estoque?.QuantidadeDisponivel ?? 0;
                    var temSuficiente = estoque?.TemQuantidadeSuficiente(item.Quantidade) ?? false;

                    if (!temSuficiente)
                        temTodosMateriais = false;

                    materiaisStatus.Add(new MaterialDisponibilidadeDTO
                    {
                        MaterialId = item.MaterialId,
                        MaterialNome = item.Material?.Nome ?? "",
                        QuantidadeSolicitada = item.Quantidade,
                        QuantidadeDisponivel = quantidadeDisponivel,
                        TemQuantidadeSuficiente = temSuficiente,
                        UnidadeMedida = item.Material?.UnidadeMedida.ToString() ?? ""
                    });
                }

                resultado.Add(new DepositoDisponibilidadeDTO
                {
                    DepositoId = deposito.Id,
                    DepositoNome = deposito.Nome,
                    Latitude = deposito.Localizacao.Latitude,
                    Longitude = deposito.Localizacao.Longitude,
                    DistanciaDoLocal = distancia,
                    TemTodosMateriais = temTodosMateriais,
                    MateriaisStatus = materiaisStatus
                });
            }

            // Ordenar por: primeiro os que têm todos os materiais, depois por distância
            return resultado
                .OrderByDescending(d => d.TemTodosMateriais)
                .ThenBy(d => d.DistanciaDoLocal)
                .ToList();
        }

        /// <summary>
        /// Busca maquinários disponíveis próximos ao depósito
        /// </summary>
        public async Task<List<MaquinarioDisponivelDTO>> BuscarMaquinariosDisponiveisAsync(
            int canteiroObraId, int depositoId, double pesoNecessario)
        {
            var deposito = await _context.Depositos
                .Include(d => d.Localizacao)
                .FirstOrDefaultAsync(d => d.Id == depositoId);

            if (deposito == null)
                throw new Exception("Depósito não encontrado");

            if (deposito.Localizacao == null)
                throw new Exception("Localização do depósito não encontrada");

            var maquinarios = await _context.Maquinarios
                .Include(m => m.LocalizacaoAtual)
                .Include(m => m.OperadorAtual)
                .Where(m => m.CanteiroObraId == canteiroObraId && 
                           m.Status == StatusMaquinario.Disponivel &&
                           m.CapacidadeCarga >= pesoNecessario &&
                           m.Ativo)
                .ToListAsync();

            var resultado = new List<MaquinarioDisponivelDTO>();

            foreach (var maquinario in maquinarios)
            {
                double? distancia = maquinario.CalcularDistanciaAte(deposito.Localizacao);
                double? tempoEstimado = null;

                // Estimativa simples: 30 km/h velocidade média
                if (distancia.HasValue)
                {
                    tempoEstimado = (distancia.Value / 1000) / 30 * 60; // em minutos
                }

                resultado.Add(new MaquinarioDisponivelDTO
                {
                    Id = maquinario.Id,
                    Nome = maquinario.Nome,
                    Tipo = maquinario.Tipo.ToString(),
                    CapacidadeCarga = maquinario.CapacidadeCarga,
                    DistanciaDoDeposito = distancia,
                    OperadorNome = maquinario.OperadorAtual?.Nome,
                    OperadorTelefone = maquinario.OperadorAtual?.Telefone,
                    TempoEstimadoChegada = tempoEstimado
                });
            }

            return resultado.OrderBy(m => m.DistanciaDoDeposito).ToList();
        }

        /// <summary>
        /// Apontador atribui maquinário e aprova a solicitação
        /// </summary>
        public async Task<bool> AtribuirMaquinarioAsync(AtribuirMaquinarioDTO dto)
        {
            var solicitacao = await _context.Solicitacoes
                .Include(s => s.Itens)
                .FirstOrDefaultAsync(s => s.Id == dto.SolicitacaoId);

            if (solicitacao == null)
                throw new Exception("Solicitação não encontrada");

            var maquinario = await _context.Maquinarios
                .FirstOrDefaultAsync(m => m.Id == dto.MaquinarioId);

            if (maquinario == null || !maquinario.EstaDisponivel())
                throw new Exception("Maquinário não disponível");

            // Verificar se o depósito tem os materiais
            var deposito = await _context.Depositos
                .Include(d => d.EstoqueMateriais)
                .FirstOrDefaultAsync(d => d.Id == dto.DepositoId);

            if (deposito == null)
                throw new Exception("Depósito não encontrado");

            // Verificar capacidade
            var pesoTotal = solicitacao.CalcularPesoTotal();
            if (!maquinario.PodeCarregar(pesoTotal))
                throw new Exception("Maquinário não suporta o peso da carga");

            // Atualizar solicitação
            var statusAnterior = solicitacao.Status;
            solicitacao.ApontadorId = dto.ApontadorId;
            solicitacao.DepositoId = dto.DepositoId;
            solicitacao.MaquinarioId = dto.MaquinarioId;
            solicitacao.Status = StatusSolicitacao.Aprovada;
            solicitacao.DataAprovacao = TimeZoneHelper.GetBrasiliaNow();
            solicitacao.ObservacoesApontador = dto.ObservacoesApontador;
            solicitacao.DataAtualizacao = TimeZoneHelper.GetBrasiliaNow();

            // Atualizar status do maquinário
            maquinario.Status = StatusMaquinario.EmUso;

            await _context.SaveChangesAsync();

            // Registrar histórico
            var apontador = await _context.Apontadores.FindAsync(dto.ApontadorId);
            await RegistrarHistoricoAsync(solicitacao.Id, statusAnterior, 
                dto.ApontadorId, apontador?.Nome ?? "Apontador", 
                $"Maquinário {maquinario.Nome} designado. Depósito: {deposito.Nome}");

            return true;
        }

        /// <summary>
        /// Atualiza o status da solicitação
        /// </summary>
        public async Task<bool> AtualizarStatusAsync(AtualizarStatusSolicitacaoDTO dto)
        {
            var solicitacao = await _context.Solicitacoes
                .Include(s => s.Maquinario)
                .FirstOrDefaultAsync(s => s.Id == dto.SolicitacaoId);

            if (solicitacao == null)
                throw new Exception("Solicitação não encontrada");

            var statusAnterior = solicitacao.Status;
            solicitacao.Status = dto.NovoStatus;
            solicitacao.DataAtualizacao = TimeZoneHelper.GetBrasiliaNow();

            // Se foi entregue, liberar o maquinário
            if (dto.NovoStatus == StatusSolicitacao.Entregue)
            {
                solicitacao.DataConclusao = TimeZoneHelper.GetBrasiliaNow();
                if (solicitacao.Maquinario != null)
                {
                    solicitacao.Maquinario.Status = StatusMaquinario.Disponivel;
                }
            }

            await _context.SaveChangesAsync();

            // Registrar histórico
            var usuario = dto.UsuarioId.HasValue ? 
                await _context.Usuarios.FindAsync(dto.UsuarioId.Value) : null;

            await RegistrarHistoricoAsync(
                solicitacao.Id, 
                statusAnterior, 
                dto.UsuarioId, 
                usuario?.Nome, 
                dto.Observacao,
                dto.Latitude,
                dto.Longitude);

            return true;
        }

        /// <summary>
        /// Registra uma mudança no histórico da solicitação
        /// </summary>
        private async Task RegistrarHistoricoAsync(
            int solicitacaoId, 
            StatusSolicitacao statusAnterior,
            int? usuarioId,
            string? usuarioNome,
            string? observacao,
            double? latitude = null,
            double? longitude = null)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(solicitacaoId);
            if (solicitacao == null) return;

            var historico = new HistoricoSolicitacao
            {
                SolicitacaoId = solicitacaoId,
                StatusAnterior = statusAnterior,
                StatusNovo = solicitacao.Status,
                DataMudanca = TimeZoneHelper.GetBrasiliaNow(),
                UsuarioId = usuarioId,
                UsuarioNome = usuarioNome,
                Observacao = observacao,
                LatitudeNoMomento = latitude,
                LongitudeNoMomento = longitude
            };

            _context.HistoricosSolicitacao.Add(historico);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Busca solicitações pendentes para o apontador
        /// </summary>
        public async Task<List<SolicitacaoDetalhadaDTO>> BuscarSolicitacoesPendentesAsync(int canteiroObraId)
        {
            var solicitacoes = await _context.Solicitacoes
                .Include(s => s.LocalSolicitacao)
                    .ThenInclude(l => l!.Localizacao)
                .Include(s => s.Solicitador)
                .Include(s => s.Itens)
                    .ThenInclude(i => i.Material)
                .Where(s => s.CanteiroObraId == canteiroObraId && 
                           s.Status == StatusSolicitacao.Pendente)
                .OrderBy(s => s.Prioridade)
                .ThenBy(s => s.DataNecessidade)
                .ToListAsync();

            return solicitacoes.Select(s => new SolicitacaoDetalhadaDTO
            {
                Id = s.Id,
                NumeroSolicitacao = s.NumeroSolicitacao,
                Status = s.Status.ToString(),
                DataCriacao = s.DataCriacao,
                DataNecessidade = s.DataNecessidade,
                Prioridade = s.Prioridade,
                LocalIdentificador = s.LocalSolicitacao?.Identificador ?? "",
                LocalTipoConstrucao = s.LocalSolicitacao?.TipoConstrucao,
                LocalLatitude = s.LocalSolicitacao?.Localizacao?.Latitude ?? 0,
                LocalLongitude = s.LocalSolicitacao?.Localizacao?.Longitude ?? 0,
                SolicitadorNome = s.Solicitador?.Nome ?? "",
                SolicitadorTelefone = s.Solicitador?.Telefone,
                PesoTotal = s.CalcularPesoTotal(),
                VolumeTotal = s.CalcularVolumeTotal(),
                Itens = s.Itens.Select(i => new ItemSolicitacaoDetalhadoDTO
                {
                    Id = i.Id,
                    MaterialNome = i.Material?.Nome ?? "",
                    Quantidade = i.Quantidade,
                    UnidadeMedida = i.Material?.UnidadeMedida.ToString() ?? "",
                    Atendido = i.Atendido,
                    QuantidadeEntregue = i.QuantidadeEntregue
                }).ToList()
            }).ToList();
        }
    }
}
