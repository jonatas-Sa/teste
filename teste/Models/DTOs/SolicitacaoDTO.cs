namespace teste.Models.DTOs
{
    /// <summary>
    /// DTO para criação de nova solicitação
    /// </summary>
    public class CriarSolicitacaoDTO
    {
        public int LocalSolicitacaoId { get; set; }
        public int SolicitadorId { get; set; }
        public DateTime DataNecessidade { get; set; }
        public string? Justificativa { get; set; }
        public int Prioridade { get; set; } = 3;
        public List<ItemSolicitacaoDTO> Itens { get; set; } = new List<ItemSolicitacaoDTO>();
    }

    /// <summary>
    /// DTO para item de solicitação
    /// </summary>
    public class ItemSolicitacaoDTO
    {
        public int MaterialId { get; set; }
        public double Quantidade { get; set; }
        public string? Observacoes { get; set; }
    }

    /// <summary>
    /// DTO para visualização de solicitação com detalhes
    /// </summary>
    public class SolicitacaoDetalhadaDTO
    {
        public int Id { get; set; }
        public string NumeroSolicitacao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime DataNecessidade { get; set; }
        public int Prioridade { get; set; }
        
        // Local de Solicitação
        public string LocalIdentificador { get; set; } = string.Empty;
        public string? LocalTipoConstrucao { get; set; }
        public double LocalLatitude { get; set; }
        public double LocalLongitude { get; set; }
        
        // Solicitador
        public string SolicitadorNome { get; set; } = string.Empty;
        public string? SolicitadorTelefone { get; set; }
        
        // Apontador
        public string? ApontadorNome { get; set; }
        
        // Depósito
        public string? DepositoNome { get; set; }
        public double? DepositoLatitude { get; set; }
        public double? DepositoLongitude { get; set; }
        public double? DistanciaDepositoLocal { get; set; } // Em metros
        
        // Maquinário
        public string? MaquinarioNome { get; set; }
        public string? MaquinarioTipo { get; set; }
        public double? MaquinarioLatitude { get; set; }
        public double? MaquinarioLongitude { get; set; }
        public double? DistanciaMaquinarioDeposito { get; set; } // Em metros
        
        // Itens
        public List<ItemSolicitacaoDetalhadoDTO> Itens { get; set; } = new List<ItemSolicitacaoDetalhadoDTO>();
        
        // Totais
        public double PesoTotal { get; set; }
        public double VolumeTotal { get; set; }
    }

    /// <summary>
    /// DTO para item de solicitação detalhado
    /// </summary>
    public class ItemSolicitacaoDetalhadoDTO
    {
        public int Id { get; set; }
        public string MaterialNome { get; set; } = string.Empty;
        public double Quantidade { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
        public bool Atendido { get; set; }
        public double? QuantidadeEntregue { get; set; }
    }

    /// <summary>
    /// DTO para atribuição de maquinário pelo apontador
    /// </summary>
    public class AtribuirMaquinarioDTO
    {
        public int SolicitacaoId { get; set; }
        public int ApontadorId { get; set; }
        public int DepositoId { get; set; }
        public int MaquinarioId { get; set; }
        public string? ObservacoesApontador { get; set; }
    }

    /// <summary>
    /// DTO para atualização de status da solicitação
    /// </summary>
    public class AtualizarStatusSolicitacaoDTO
    {
        public int SolicitacaoId { get; set; }
        public StatusSolicitacao NovoStatus { get; set; }
        public int? UsuarioId { get; set; }
        public string? Observacao { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
