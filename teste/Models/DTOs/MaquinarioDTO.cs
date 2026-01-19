using teste.Utils;

namespace teste.Models.DTOs
{
    /// <summary>
    /// DTO para visualização de maquinário no mapa
    /// </summary>
    public class MaquinarioMapaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? DataUltimaAtualizacao { get; set; }
        public double CapacidadeCarga { get; set; }
        public double? CapacidadeVolume { get; set; }
        public string? OperadorNome { get; set; }
        public bool EstaDisponivel { get; set; }
    }

    /// <summary>
    /// DTO para atualização de localização do maquinário (usado pelo tablet)
    /// </summary>
    public class AtualizarLocalizacaoMaquinarioDTO
    {
        public int MaquinarioId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? VelocidadeAtual { get; set; }
        public DateTime Timestamp { get; set; } = TimeZoneHelper.GetBrasiliaNow();
    }

    /// <summary>
    /// DTO para listar maquinários disponíveis com distâncias
    /// </summary>
    public class MaquinarioDisponivelDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public double CapacidadeCarga { get; set; }
        public double? DistanciaDoDeposito { get; set; } // Em metros
        public string? OperadorNome { get; set; }
        public string? OperadorTelefone { get; set; }
        public double? TempoEstimadoChegada { get; set; } // Em minutos
    }
}
