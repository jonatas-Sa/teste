namespace teste.Models.DTOs
{
    /// <summary>
    /// DTO principal para exibição do mapa completo
    /// </summary>
    public class MapaCanteiroDTO
    {
        public int CanteiroObraId { get; set; }
        public string CanteiroNome { get; set; } = string.Empty;
        public double CentroLatitude { get; set; }
        public double CentroLongitude { get; set; }
        
        public List<PontoMapaDTO> LocaisSolicitacao { get; set; } = new List<PontoMapaDTO>();
        public List<PontoMapaDTO> Depositos { get; set; } = new List<PontoMapaDTO>();
        public List<MaquinarioMapaDTO> Maquinarios { get; set; } = new List<MaquinarioMapaDTO>();
        public List<SolicitacaoMapaDTO> SolicitacoesAtivas { get; set; } = new List<SolicitacaoMapaDTO>();
    }

    /// <summary>
    /// DTO genérico para pontos fixos no mapa (locais de solicitação e depósitos)
    /// </summary>
    public class PontoMapaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "LocalSolicitacao" ou "Deposito"
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Descricao { get; set; }
        public Dictionary<string, object> DadosAdicionais { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// DTO para solicitação exibida no mapa
    /// </summary>
    public class SolicitacaoMapaDTO
    {
        public int Id { get; set; }
        public string NumeroSolicitacao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Prioridade { get; set; }
        
        // Origem (Local de Solicitação)
        public int LocalSolicitacaoId { get; set; }
        public double OrigemLatitude { get; set; }
        public double OrigemLongitude { get; set; }
        
        // Destino (Depósito)
        public int? DepositoId { get; set; }
        public double? DestinoLatitude { get; set; }
        public double? DestinoLongitude { get; set; }
        
        // Maquinário
        public int? MaquinarioId { get; set; }
        
        public int QuantidadeItens { get; set; }
        public double PesoTotal { get; set; }
    }

    /// <summary>
    /// DTO para análise de depósitos disponíveis para uma solicitação
    /// </summary>
    public class DepositoDisponibilidadeDTO
    {
        public int DepositoId { get; set; }
        public string DepositoNome { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanciaDoLocal { get; set; } // Em metros (do local de solicitação)
        public bool TemTodosMateriais { get; set; }
        public List<MaterialDisponibilidadeDTO> MateriaisStatus { get; set; } = new List<MaterialDisponibilidadeDTO>();
    }

    /// <summary>
    /// DTO para status de disponibilidade de um material
    /// </summary>
    public class MaterialDisponibilidadeDTO
    {
        public int MaterialId { get; set; }
        public string MaterialNome { get; set; } = string.Empty;
        public double QuantidadeSolicitada { get; set; }
        public double QuantidadeDisponivel { get; set; }
        public bool TemQuantidadeSuficiente { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
    }
}
