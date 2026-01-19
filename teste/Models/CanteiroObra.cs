using System.ComponentModel.DataAnnotations;

namespace teste.Models
{
    /// <summary>
    /// Representa um canteiro de obras
    /// </summary>
    public class CanteiroObra : EntidadeBase
    {
        [Required(ErrorMessage = "Nome do canteiro é obrigatório")]
        [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório")]
        [StringLength(300)]
        public string Endereco { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Cidade { get; set; }

        [StringLength(2)]
        public string? Estado { get; set; }

        [StringLength(10)]
        public string? CEP { get; set; }

        // Localização central do canteiro
        public required Localizacao LocalizacaoCentral { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime? DataPrevisaoTermino { get; set; }

        // Relacionamentos
        public virtual ICollection<LocalSolicitacao> LocaisSolicitacao { get; set; } = new List<LocalSolicitacao>();
        public virtual ICollection<Deposito> Depositos { get; set; } = new List<Deposito>();
        public virtual ICollection<Maquinario> Maquinarios { get; set; } = new List<Maquinario>();
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();
    }
}
