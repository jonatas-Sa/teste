using System.ComponentModel.DataAnnotations;
using teste.Utils;

namespace teste.Models
{
    /// <summary>
    /// Representa um maquinário/veículo usado para transporte de materiais
    /// </summary>
    public class Maquinario : EntidadeBase
    {
        [Required(ErrorMessage = "Nome/Identificador do maquinário é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty; // Ex: "Caminhonete 01", "Carrinho A"

        [StringLength(300)]
        public string? Descricao { get; set; }

        [Required]
        public TipoMaquinario Tipo { get; set; }

        [Required]
        public StatusMaquinario Status { get; set; } = StatusMaquinario.Disponivel;

        [StringLength(20)]
        public string? Placa { get; set; }

        [StringLength(100)]
        public string? Modelo { get; set; }

        [StringLength(50)]
        public string? Marca { get; set; }

        [Range(0, int.MaxValue)]
        public int? AnoFabricacao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Capacidade de carga deve ser maior que zero")]
        public double CapacidadeCarga { get; set; } // Em kg

        [Range(0, double.MaxValue)]
        public double? CapacidadeVolume { get; set; } // Em m³

        // Localização em tempo real (atualizada via GPS do tablet)
        public Localizacao? LocalizacaoAtual { get; set; }

        public DateTime? DataUltimaAtualizacaoGPS { get; set; }

        [Range(0, double.MaxValue)]
        public double? VelocidadeAtual { get; set; } // Em km/h

        // Informações de manutenção
        public DateTime? DataUltimaManutencao { get; set; }

        public DateTime? DataProximaManutencao { get; set; }

        [Range(0, double.MaxValue)]
        public double? Odometro { get; set; } // Em km

        // Relacionamento com Canteiro de Obras
        public int CanteiroObraId { get; set; }
        public virtual CanteiroObra? CanteiroObra { get; set; }

        // Relacionamento com Operador
        public int? OperadorAtualId { get; set; }
        public virtual OperadorMaquinario? OperadorAtual { get; set; }

        // Relacionamentos
        public virtual ICollection<Solicitacao> SolicitacoesAtribuidas { get; set; } = new List<Solicitacao>();

        /// <summary>
        /// Verifica se o maquinário está disponível para atribuição
        /// </summary>
        public bool EstaDisponivel()
        {
            return Status == StatusMaquinario.Disponivel && Ativo;
        }

        /// <summary>
        /// Verifica se o maquinário pode carregar a carga especificada
        /// </summary>
        public bool PodeCarregar(double pesoCarga)
        {
            return pesoCarga <= CapacidadeCarga;
        }

        /// <summary>
        /// Atualiza a localização do maquinário via GPS
        /// </summary>
        public void AtualizarLocalizacao(double latitude, double longitude)
        {
            if (LocalizacaoAtual == null)
            {
                LocalizacaoAtual = new Localizacao();
            }
            
            LocalizacaoAtual.Latitude = latitude;
            LocalizacaoAtual.Longitude = longitude;
            LocalizacaoAtual.DataAtualizacao = TimeZoneHelper.GetBrasiliaNow();
            DataUltimaAtualizacaoGPS = TimeZoneHelper.GetBrasiliaNow();
        }

        /// <summary>
        /// Calcula a distância até um ponto específico
        /// </summary>
        public double? CalcularDistanciaAte(Localizacao destino)
        {
            if (LocalizacaoAtual == null) return null;
            return LocalizacaoAtual.CalcularDistancia(destino);
        }
    }
}
