using System.ComponentModel.DataAnnotations;

namespace teste.Models
{
    /// <summary>
    /// Classe base para todos os usuários do sistema
    /// </summary>
    public class Usuario : EntidadeBase
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        public TipoUsuario TipoUsuario { get; set; }

        [StringLength(200)]
        public string? Observacoes { get; set; }

        // Relacionamento com Canteiro de Obras
        public int CanteiroObraId { get; set; }
        public virtual CanteiroObra? CanteiroObra { get; set; }
    }

    /// <summary>
    /// Apontador - responsável por analisar solicitações e designar maquinários
    /// </summary>
    public class Apontador : Usuario
    {
        public Apontador()
        {
            TipoUsuario = TipoUsuario.Apontador;
        }

        [StringLength(50)]
        public string? Matricula { get; set; }

        public DateTime? DataAdmissao { get; set; }

        // Relacionamentos
        public virtual ICollection<Solicitacao> SolicitacoesGerenciadas { get; set; } = new List<Solicitacao>();
    }

    /// <summary>
    /// Solicitador - responsável por criar solicitações de materiais
    /// </summary>
    public class Solicitador : Usuario
    {
        public Solicitador()
        {
            TipoUsuario = TipoUsuario.Solicitador;
        }

        [StringLength(50)]
        public string? Matricula { get; set; }

        [StringLength(100)]
        public string? Cargo { get; set; }

        // Relacionamentos
        public virtual ICollection<Solicitacao> SolicitacoesCriadas { get; set; } = new List<Solicitacao>();
    }

    /// <summary>
    /// Operador de maquinário - responsável por operar os veículos
    /// </summary>
    public class OperadorMaquinario : Usuario
    {
        public OperadorMaquinario()
        {
            TipoUsuario = TipoUsuario.OperadorMaquinario;
        }

        [StringLength(50)]
        public string? CNH { get; set; }

        [StringLength(20)]
        public string? CategoriaCNH { get; set; }

        public DateTime? ValidadeCNH { get; set; }

        // Relacionamento com maquinário
        public int? MaquinarioAtualId { get; set; }
        public virtual Maquinario? MaquinarioAtual { get; set; }
    }
}
