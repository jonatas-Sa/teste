using System.ComponentModel.DataAnnotations;

namespace teste.Models
{
    /// <summary>
    /// Representa um tipo de material utilizado na construção
    /// </summary>
    public class Material : EntidadeBase
    {
        [Required(ErrorMessage = "Nome do material é obrigatório")]
        [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty; // Ex: "Cimento", "Tijolo", "Areia"

        [StringLength(500)]
        public string? Descricao { get; set; }

        [StringLength(50)]
        public string? Codigo { get; set; } // Código interno do material

        [Required]
        public UnidadeMedida UnidadeMedida { get; set; }

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(100)]
        public string? Fornecedor { get; set; }

        [Range(0, double.MaxValue)]
        public double? PesoUnitario { get; set; } // Em kg

        [Range(0, double.MaxValue)]
        public double? VolumeUnitario { get; set; } // Em m³

        [StringLength(500)]
        public string? Observacoes { get; set; }

        // Relacionamentos
        public virtual ICollection<EstoqueMaterial> EstoquesMateriais { get; set; } = new List<EstoqueMaterial>();
        public virtual ICollection<ItemSolicitacao> ItensSolicitacao { get; set; } = new List<ItemSolicitacao>();
    }

    /// <summary>
    /// Representa o estoque de um material em um depósito específico
    /// </summary>
    public class EstoqueMaterial : EntidadeBase
    {
        // Relacionamento com Material
        public int MaterialId { get; set; }
        public virtual Material? Material { get; set; }

        // Relacionamento com Depósito
        public int DepositoId { get; set; }
        public virtual Deposito? Deposito { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Quantidade deve ser maior ou igual a zero")]
        public double QuantidadeDisponivel { get; set; }

        [Range(0, double.MaxValue)]
        public double? QuantidadeMinima { get; set; } // Estoque mínimo de segurança

        [Range(0, double.MaxValue)]
        public double? QuantidadeMaxima { get; set; } // Capacidade máxima de armazenamento

        public DateTime? DataUltimaAtualizacao { get; set; }

        [StringLength(200)]
        public string? LocalizacaoNoDeposito { get; set; } // Ex: "Prateleira A3", "Área externa"

        /// <summary>
        /// Verifica se há quantidade suficiente disponível
        /// </summary>
        public bool TemQuantidadeSuficiente(double quantidadeSolicitada)
        {
            return QuantidadeDisponivel >= quantidadeSolicitada;
        }

        /// <summary>
        /// Verifica se o estoque está abaixo do mínimo
        /// </summary>
        public bool EstoqueAbaixoDoMinimo()
        {
            return QuantidadeMinima.HasValue && QuantidadeDisponivel < QuantidadeMinima.Value;
        }
    }
}
