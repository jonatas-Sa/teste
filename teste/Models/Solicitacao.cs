using System.ComponentModel.DataAnnotations;
using teste.Utils;

namespace teste.Models
{
    /// <summary>
    /// Representa uma solicitação de materiais
    /// </summary>
    public class Solicitacao : EntidadeBase
    {
        [Required]
        [StringLength(50)]
        public string NumeroSolicitacao { get; set; } = string.Empty; // Ex: "SOL-2026-0001"

        [Required]
        public StatusSolicitacao Status { get; set; } = StatusSolicitacao.Pendente;

        [Required(ErrorMessage = "Data de necessidade é obrigatória")]
        public DateTime DataNecessidade { get; set; }

        [StringLength(500)]
        public string? Justificativa { get; set; }

        [StringLength(500)]
        public string? ObservacoesApontador { get; set; }

        [StringLength(500)]
        public string? MotivoRejeicao { get; set; }

        public DateTime? DataAprovacao { get; set; }

        public DateTime? DataConclusao { get; set; }

        // Prioridade (1 = urgente, 5 = baixa prioridade)
        [Range(1, 5)]
        public int Prioridade { get; set; } = 3;

        // Relacionamento com Local de Solicitação
        public int LocalSolicitacaoId { get; set; }
        public virtual LocalSolicitacao? LocalSolicitacao { get; set; }

        // Relacionamento com Solicitador
        public int SolicitadorId { get; set; }
        public virtual Solicitador? Solicitador { get; set; }

        // Relacionamento com Apontador
        public int? ApontadorId { get; set; }
        public virtual Apontador? Apontador { get; set; }

        // Relacionamento com Depósito escolhido
        public int? DepositoId { get; set; }
        public virtual Deposito? Deposito { get; set; }

        // Relacionamento com Maquinário designado
        public int? MaquinarioId { get; set; }
        public virtual Maquinario? Maquinario { get; set; }

        // Relacionamento com Canteiro de Obras
        public int CanteiroObraId { get; set; }
        public virtual CanteiroObra? CanteiroObra { get; set; }

        // Itens da solicitação
        public virtual ICollection<ItemSolicitacao> Itens { get; set; } = new List<ItemSolicitacao>();

        // Histórico de mudanças de status
        public virtual ICollection<HistoricoSolicitacao> Historico { get; set; } = new List<HistoricoSolicitacao>();

        /// <summary>
        /// Calcula o peso total estimado da solicitação
        /// </summary>
        public double CalcularPesoTotal()
        {
            return Itens.Sum(i => (i.Material?.PesoUnitario ?? 0) * i.Quantidade);
        }

        /// <summary>
        /// Calcula o volume total estimado da solicitação
        /// </summary>
        public double CalcularVolumeTotal()
        {
            return Itens.Sum(i => (i.Material?.VolumeUnitario ?? 0) * i.Quantidade);
        }

        /// <summary>
        /// Verifica se todos os itens estão disponíveis em um depósito
        /// </summary>
        public bool VerificarDisponibilidadeNoDeposito(Deposito deposito)
        {
            foreach (var item in Itens)
            {
                var estoque = deposito.EstoqueMateriais
                    .FirstOrDefault(e => e.MaterialId == item.MaterialId);
                
                if (estoque == null || !estoque.TemQuantidadeSuficiente(item.Quantidade))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gera um número único de solicitação
        /// </summary>
        public static string GerarNumeroSolicitacao(int sequencial)
        {
            return $"SOL-{TimeZoneHelper.GetBrasiliaNow().Year}-{sequencial:D4}";
        }
    }

    /// <summary>
    /// Representa um item individual dentro de uma solicitação
    /// </summary>
    public class ItemSolicitacao : EntidadeBase
    {
        // Relacionamento com Solicitação
        public int SolicitacaoId { get; set; }
        public virtual Solicitacao? Solicitacao { get; set; }

        // Relacionamento com Material
        public int MaterialId { get; set; }
        public virtual Material? Material { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
        public double Quantidade { get; set; }

        [StringLength(200)]
        public string? Observacoes { get; set; }

        public bool Atendido { get; set; } = false;

        public double? QuantidadeEntregue { get; set; }

        public DateTime? DataEntrega { get; set; }
    }

    /// <summary>
    /// Registra o histórico de mudanças de status de uma solicitação
    /// </summary>
    public class HistoricoSolicitacao : EntidadeBase
    {
        // Relacionamento com Solicitação
        public int SolicitacaoId { get; set; }
        public virtual Solicitacao? Solicitacao { get; set; }

        [Required]
        public StatusSolicitacao StatusAnterior { get; set; }

        [Required]
        public StatusSolicitacao StatusNovo { get; set; }

        [Required]
        public DateTime DataMudanca { get; set; } = TimeZoneHelper.GetBrasiliaNow();

        public int? UsuarioId { get; set; }
        
        [StringLength(100)]
        public string? UsuarioNome { get; set; }

        [StringLength(500)]
        public string? Observacao { get; set; }

        // Informações de localização quando relevante
        public double? LatitudeNoMomento { get; set; }
        public double? LongitudeNoMomento { get; set; }
    }
}
