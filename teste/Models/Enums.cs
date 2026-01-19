namespace teste.Models
{
    /// <summary>
    /// Status da solicitação de material
    /// </summary>
    public enum StatusSolicitacao
    {
        Pendente = 1,           // Aguardando análise do apontador
        EmAnalise = 2,          // Apontador está analisando
        Aprovada = 3,           // Aprovada e maquinário designado
        EmTransporte = 4,       // Maquinário a caminho do depósito
        ColetandoMaterial = 5,  // Coletando material no depósito
        EmEntrega = 6,          // A caminho do local de solicitação
        Entregue = 7,           // Material entregue
        Cancelada = 8,          // Solicitação cancelada
        Rejeitada = 9           // Rejeitada por falta de material ou outro motivo
    }

    /// <summary>
    /// Tipo de usuário no sistema
    /// </summary>
    public enum TipoUsuario
    {
        Apontador = 1,
        Solicitador = 2,
        OperadorMaquinario = 3,
        Administrador = 4
    }

    /// <summary>
    /// Status do maquinário
    /// </summary>
    public enum StatusMaquinario
    {
        Disponivel = 1,
        EmUso = 2,
        EmManutencao = 3,
        Indisponivel = 4
    }

    /// <summary>
    /// Tipo de maquinário
    /// </summary>
    public enum TipoMaquinario
    {
        CarrinhoMao = 1,
        Carrinho = 2,
        MiniTrator = 3,
        Caminhonete = 4,
        Caminhao = 5,
        Empilhadeira = 6,
        Outro = 99
    }

    /// <summary>
    /// Unidade de medida para materiais
    /// </summary>
    public enum UnidadeMedida
    {
        Unidade = 1,
        Metro = 2,
        MetroQuadrado = 3,
        MetroCubico = 4,
        Litro = 5,
        Quilograma = 6,
        Tonelada = 7,
        Saco = 8,
        Caixa = 9,
        Palete = 10
    }
}
