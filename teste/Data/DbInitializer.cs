using teste.Models;
using teste.Utils;

namespace teste.Data
{
    /// <summary>
    /// Classe para inicialização do banco de dados com dados de exemplo
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Garante que o banco foi criado
            context.Database.EnsureCreated();

            // Verifica se já existem dados
            if (context.CanteirosObra.Any())
            {
                return; // Banco já foi populado
            }

            // Criar Canteiro de Obra
            var canteiro = new CanteiroObra
            {
                Nome = "Residencial Jardim das Flores",
                Descricao = "Condomínio residencial com 50 casas",
                Endereco = "Rua das Palmeiras, 1000",
                Cidade = "São Paulo",
                Estado = "SP",
                CEP = "01234-567",
                LocalizacaoCentral = new Localizacao
                {
                    Latitude = -23.550520,
                    Longitude = -46.633308
                },
                DataInicio = TimeZoneHelper.GetBrasiliaNow().AddMonths(-6),
                DataPrevisaoTermino = TimeZoneHelper.GetBrasiliaNow().AddMonths(18),
                DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                Ativo = true
            };
            context.CanteirosObra.Add(canteiro);
            context.SaveChanges();

            // Criar Locais de Solicitação
            var locais = new[]
            {
                new LocalSolicitacao
                {
                    Identificador = "Casa 1",
                    TipoConstrucao = "Casa Térrea",
                    Descricao = "Casa térrea - 2 quartos",
                    Localizacao = new Localizacao { Latitude = -23.550320, Longitude = -46.633108 },
                    NumeroLote = "001",
                    Quadra = "A",
                    AreaTerreno = 250,
                    AreaConstruida = 80,
                    PercentualConclusao = 45,
                    CanteiroObraId = canteiro.Id,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new LocalSolicitacao
                {
                    Identificador = "Casa 2",
                    TipoConstrucao = "Casa Térrea",
                    Descricao = "Casa térrea - 3 quartos",
                    Localizacao = new Localizacao { Latitude = -23.550420, Longitude = -46.633208 },
                    NumeroLote = "002",
                    Quadra = "A",
                    AreaTerreno = 300,
                    AreaConstruida = 120,
                    PercentualConclusao = 30,
                    CanteiroObraId = canteiro.Id,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new LocalSolicitacao
                {
                    Identificador = "Sobrado 3",
                    TipoConstrucao = "Sobrado",
                    Descricao = "Sobrado - 4 quartos",
                    Localizacao = new Localizacao { Latitude = -23.550620, Longitude = -46.633408 },
                    NumeroLote = "003",
                    Quadra = "B",
                    AreaTerreno = 350,
                    AreaConstruida = 180,
                    PercentualConclusao = 15,
                    CanteiroObraId = canteiro.Id,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                }
            };
            context.LocaisSolicitacao.AddRange(locais);
            context.SaveChanges();

            // Criar Depósitos
            var depositos = new[]
            {
                new Deposito
                {
                    Nome = "Depósito Central",
                    Descricao = "Depósito principal - materiais gerais",
                    Localizacao = new Localizacao { Latitude = -23.550520, Longitude = -46.633508 },
                    ResponsavelNome = "João Silva",
                    ResponsavelTelefone = "(11) 98765-4321",
                    AreaTotal = 500,
                    DepositoCoberto = true,
                    CanteiroObraId = canteiro.Id,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new Deposito
                {
                    Nome = "Tenda 1",
                    Descricao = "Materiais de acabamento",
                    Localizacao = new Localizacao { Latitude = -23.550220, Longitude = -46.633608 },
                    ResponsavelNome = "Maria Santos",
                    ResponsavelTelefone = "(11) 98765-1234",
                    AreaTotal = 200,
                    DepositoCoberto = true,
                    CanteiroObraId = canteiro.Id,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                }
            };
            context.Depositos.AddRange(depositos);
            context.SaveChanges();

            // Criar Materiais
            var materiais = new[]
            {
                new Material
                {
                    Nome = "Cimento CP II 50kg",
                    Descricao = "Cimento Portland",
                    Codigo = "CIM-001",
                    UnidadeMedida = UnidadeMedida.Saco,
                    Marca = "Votorantim",
                    PesoUnitario = 50,
                    VolumeUnitario = 0.04,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new Material
                {
                    Nome = "Areia Média",
                    Descricao = "Areia para construção",
                    Codigo = "ARE-001",
                    UnidadeMedida = UnidadeMedida.MetroCubico,
                    PesoUnitario = 1500,
                    VolumeUnitario = 1,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new Material
                {
                    Nome = "Tijolo Cerâmico 6 furos",
                    Descricao = "Tijolo 9x14x19cm",
                    Codigo = "TIJ-001",
                    UnidadeMedida = UnidadeMedida.Unidade,
                    Marca = "Cerâmica São Paulo",
                    PesoUnitario = 2.5,
                    VolumeUnitario = 0.0024,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new Material
                {
                    Nome = "Brita 1",
                    Descricao = "Brita para concreto",
                    Codigo = "BRI-001",
                    UnidadeMedida = UnidadeMedida.MetroCubico,
                    PesoUnitario = 1650,
                    VolumeUnitario = 1,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new Material
                {
                    Nome = "Argamassa AC II",
                    Descricao = "Argamassa colante",
                    Codigo = "ARG-001",
                    UnidadeMedida = UnidadeMedida.Saco,
                    Marca = "Quartzolit",
                    PesoUnitario = 20,
                    VolumeUnitario = 0.015,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                }
            };
            context.Materiais.AddRange(materiais);
            context.SaveChanges();

            // Criar Estoque de Materiais
            var estoques = new[]
            {
                // Depósito Central
                new EstoqueMaterial
                {
                    MaterialId = materiais[0].Id,
                    DepositoId = depositos[0].Id,
                    QuantidadeDisponivel = 500,
                    QuantidadeMinima = 100,
                    QuantidadeMaxima = 1000,
                    LocalizacaoNoDeposito = "Prateleira A1",
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new EstoqueMaterial
                {
                    MaterialId = materiais[1].Id,
                    DepositoId = depositos[0].Id,
                    QuantidadeDisponivel = 20,
                    QuantidadeMinima = 5,
                    QuantidadeMaxima = 50,
                    LocalizacaoNoDeposito = "Área externa 1",
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new EstoqueMaterial
                {
                    MaterialId = materiais[2].Id,
                    DepositoId = depositos[0].Id,
                    QuantidadeDisponivel = 5000,
                    QuantidadeMinima = 1000,
                    QuantidadeMaxima = 10000,
                    LocalizacaoNoDeposito = "Área externa 2",
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new EstoqueMaterial
                {
                    MaterialId = materiais[3].Id,
                    DepositoId = depositos[0].Id,
                    QuantidadeDisponivel = 15,
                    QuantidadeMinima = 5,
                    QuantidadeMaxima = 30,
                    LocalizacaoNoDeposito = "Área externa 3",
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                // Tenda 1
                new EstoqueMaterial
                {
                    MaterialId = materiais[4].Id,
                    DepositoId = depositos[1].Id,
                    QuantidadeDisponivel = 300,
                    QuantidadeMinima = 50,
                    QuantidadeMaxima = 500,
                    LocalizacaoNoDeposito = "Prateleira B1",
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                }
            };
            context.EstoquesMateriais.AddRange(estoques);
            context.SaveChanges();

            // Criar Usuários
            var apontador = new Apontador
            {
                Nome = "Carlos Apontador",
                Email = "carlos.apontador@email.com",
                Telefone = "(11) 91234-5678",
                Matricula = "APT-001",
                DataAdmissao = TimeZoneHelper.GetBrasiliaNow().AddYears(-2),
                CanteiroObraId = canteiro.Id,
                DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                Ativo = true
            };

            var solicitador1 = new Solicitador
            {
                Nome = "Pedro Construtor",
                Email = "pedro.construtor@email.com",
                Telefone = "(11) 91234-1111",
                Matricula = "SOL-001",
                Cargo = "Mestre de Obras",
                CanteiroObraId = canteiro.Id,
                DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                Ativo = true
            };

            var solicitador2 = new Solicitador
            {
                Nome = "Ana Pedreiro",
                Email = "ana.pedreiro@email.com",
                Telefone = "(11) 91234-2222",
                Matricula = "SOL-002",
                Cargo = "Encarregada",
                CanteiroObraId = canteiro.Id,
                DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                Ativo = true
            };

            var operador1 = new OperadorMaquinario
            {
                Nome = "Roberto Motorista",
                Email = "roberto.motorista@email.com",
                Telefone = "(11) 91234-3333",
                CNH = "12345678901",
                CategoriaCNH = "D",
                ValidadeCNH = TimeZoneHelper.GetBrasiliaNow().AddYears(3),
                CanteiroObraId = canteiro.Id,
                DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                Ativo = true
            };

            var operador2 = new OperadorMaquinario
            {
                Nome = "José Carrinheiro",
                Email = "jose.carrinheiro@email.com",
                Telefone = "(11) 91234-4444",
                CanteiroObraId = canteiro.Id,
                DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                Ativo = true
            };

            context.Usuarios.AddRange(apontador, solicitador1, solicitador2, operador1, operador2);
            context.SaveChanges();

            // Criar Maquinários
            var maquinarios = new[]
            {
                new Maquinario
                {
                    Nome = "Caminhonete 01",
                    Descricao = "Caminhonete para transporte de materiais pesados",
                    Tipo = TipoMaquinario.Caminhonete,
                    Status = StatusMaquinario.Disponivel,
                    Placa = "ABC-1234",
                    Modelo = "Hilux",
                    Marca = "Toyota",
                    AnoFabricacao = 2020,
                    CapacidadeCarga = 1000,
                    CapacidadeVolume = 2.5,
                    LocalizacaoAtual = new Localizacao { Latitude = -23.550420, Longitude = -46.633508 },
                    DataUltimaAtualizacaoGPS = TimeZoneHelper.GetBrasiliaNow(),
                    CanteiroObraId = canteiro.Id,
                    OperadorAtualId = operador1.Id,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new Maquinario
                {
                    Nome = "Carrinho Manual 02",
                    Descricao = "Carrinho de mão reforçado",
                    Tipo = TipoMaquinario.CarrinhoMao,
                    Status = StatusMaquinario.Disponivel,
                    CapacidadeCarga = 150,
                    CapacidadeVolume = 0.5,
                    LocalizacaoAtual = new Localizacao { Latitude = -23.550320, Longitude = -46.633408 },
                    DataUltimaAtualizacaoGPS = TimeZoneHelper.GetBrasiliaNow(),
                    CanteiroObraId = canteiro.Id,
                    OperadorAtualId = operador2.Id,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                },
                new Maquinario
                {
                    Nome = "Mini Trator 03",
                    Descricao = "Mini trator para cargas médias",
                    Tipo = TipoMaquinario.MiniTrator,
                    Status = StatusMaquinario.Disponivel,
                    CapacidadeCarga = 500,
                    CapacidadeVolume = 1.5,
                    LocalizacaoAtual = new Localizacao { Latitude = -23.550620, Longitude = -46.633308 },
                    DataUltimaAtualizacaoGPS = TimeZoneHelper.GetBrasiliaNow(),
                    CanteiroObraId = canteiro.Id,
                    DataCriacao = TimeZoneHelper.GetBrasiliaNow(),
                    Ativo = true
                }
            };
            context.Maquinarios.AddRange(maquinarios);
            context.SaveChanges();

            // Atualizar operadores com seus maquinários
            operador1.MaquinarioAtualId = maquinarios[0].Id;
            operador2.MaquinarioAtualId = maquinarios[1].Id;
            context.SaveChanges();

            Console.WriteLine("Banco de dados inicializado com dados de exemplo!");
        }
    }
}
