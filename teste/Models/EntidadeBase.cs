using System.ComponentModel.DataAnnotations;
using teste.Utils;

namespace teste.Models
{
    /// <summary>
    /// Classe base para todas as entidades
    /// </summary>
    public abstract class EntidadeBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; } = TimeZoneHelper.GetBrasiliaNow();

        public DateTime? DataAtualizacao { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
