using System.ComponentModel.DataAnnotations;

namespace teste.Models
{
    /// <summary>
    /// Representa um depósito de materiais dentro do canteiro de obras
    /// </summary>
    public class Deposito : EntidadeBase
    {
        [Required(ErrorMessage = "Nome do depósito é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty; // Ex: "Depósito Central", "Tenda 1"

        [StringLength(300)]
        public string? Descricao { get; set; }

        // Localização fixa do depósito
        [Required]
        public required Localizacao Localizacao { get; set; }

        [StringLength(100)]
        public string? ResponsavelNome { get; set; }

        [Phone]
        [StringLength(20)]
        public string? ResponsavelTelefone { get; set; }

        [Range(0, double.MaxValue)]
        public double? AreaTotal { get; set; } // Em metros quadrados

        public bool DepositoCoberto { get; set; } = true;

        // Relacionamento com Canteiro de Obras
        public int CanteiroObraId { get; set; }
        public virtual CanteiroObra? CanteiroObra { get; set; }

        // Relacionamentos
        public virtual ICollection<EstoqueMaterial> EstoqueMateriais { get; set; } = new List<EstoqueMaterial>();
    }
}
