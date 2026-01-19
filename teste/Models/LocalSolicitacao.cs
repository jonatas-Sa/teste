using System.ComponentModel.DataAnnotations;

namespace teste.Models
{
    /// <summary>
    /// Representa um local de construção dentro do canteiro de obras (ponto de solicitação de materiais)
    /// Pode ser uma casa, prédio, galpão ou qualquer outra construção
    /// </summary>
    public class LocalSolicitacao : EntidadeBase
    {
        [Required(ErrorMessage = "Identificador do local é obrigatório")]
        [StringLength(50, ErrorMessage = "Identificador deve ter no máximo 50 caracteres")]
        public string Identificador { get; set; } = string.Empty; // Ex: "Casa 1", "Prédio A", "Galpão 3", "Lote 15"

        [StringLength(200)]
        public string? Descricao { get; set; }

        [StringLength(100)]
        public string? TipoConstrucao { get; set; } // Ex: "Casa", "Prédio", "Galpão", "Sobrado", "Comercial"

        // Localização fixa do local
        [Required]
        public required Localizacao Localizacao { get; set; }

        [StringLength(50)]
        public string? NumeroLote { get; set; }

        [StringLength(50)]
        public string? Quadra { get; set; }

        [Range(0, double.MaxValue)]
        public double? AreaTerreno { get; set; }

        [Range(0, double.MaxValue)]
        public double? AreaConstruida { get; set; }

        [Range(0, 100)]
        public int? PercentualConclusao { get; set; }

        // Relacionamento com Canteiro de Obras
        public int CanteiroObraId { get; set; }
        public virtual CanteiroObra? CanteiroObra { get; set; }

        // Relacionamentos
        public virtual ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();
    }
}
