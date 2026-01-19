using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace teste.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CanteirosObra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Endereco = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    CEP = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    LocalizacaoCentral_Latitude = table.Column<double>(type: "double precision", nullable: false),
                    LocalizacaoCentral_Longitude = table.Column<double>(type: "double precision", nullable: false),
                    LocalizacaoCentral_DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPrevisaoTermino = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanteirosObra", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materiais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UnidadeMedida = table.Column<int>(type: "integer", nullable: false),
                    Marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Fornecedor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PesoUnitario = table.Column<double>(type: "double precision", nullable: true),
                    VolumeUnitario = table.Column<double>(type: "double precision", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Depositos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Localizacao_Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Localizacao_Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Localizacao_DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResponsavelNome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ResponsavelTelefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AreaTotal = table.Column<double>(type: "double precision", nullable: true),
                    DepositoCoberto = table.Column<bool>(type: "boolean", nullable: false),
                    CanteiroObraId = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depositos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depositos_CanteirosObra_CanteiroObraId",
                        column: x => x.CanteiroObraId,
                        principalTable: "CanteirosObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocaisSolicitacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identificador = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TipoConstrucao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Localizacao_Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Localizacao_Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Localizacao_DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumeroLote = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Quadra = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AreaTerreno = table.Column<double>(type: "double precision", nullable: true),
                    AreaConstruida = table.Column<double>(type: "double precision", nullable: true),
                    PercentualConclusao = table.Column<int>(type: "integer", nullable: true),
                    CanteiroObraId = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocaisSolicitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocaisSolicitacao_CanteirosObra_CanteiroObraId",
                        column: x => x.CanteiroObraId,
                        principalTable: "CanteirosObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maquinarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Placa = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Marca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AnoFabricacao = table.Column<int>(type: "integer", nullable: true),
                    CapacidadeCarga = table.Column<double>(type: "double precision", nullable: false),
                    CapacidadeVolume = table.Column<double>(type: "double precision", nullable: true),
                    LocalizacaoAtual_Latitude = table.Column<double>(type: "double precision", nullable: true),
                    LocalizacaoAtual_Longitude = table.Column<double>(type: "double precision", nullable: true),
                    LocalizacaoAtual_DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataUltimaAtualizacaoGPS = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VelocidadeAtual = table.Column<double>(type: "double precision", nullable: true),
                    DataUltimaManutencao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataProximaManutencao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Odometro = table.Column<double>(type: "double precision", nullable: true),
                    CanteiroObraId = table.Column<int>(type: "integer", nullable: false),
                    OperadorAtualId = table.Column<int>(type: "integer", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maquinarios_CanteirosObra_CanteiroObraId",
                        column: x => x.CanteiroObraId,
                        principalTable: "CanteirosObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstoquesMateriais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    DepositoId = table.Column<int>(type: "integer", nullable: false),
                    QuantidadeDisponivel = table.Column<double>(type: "double precision", nullable: false),
                    QuantidadeMinima = table.Column<double>(type: "double precision", nullable: true),
                    QuantidadeMaxima = table.Column<double>(type: "double precision", nullable: true),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LocalizacaoNoDeposito = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoquesMateriais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoquesMateriais_Depositos_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstoquesMateriais_Materiais_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipoUsuario = table.Column<int>(type: "integer", nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CanteiroObraId = table.Column<int>(type: "integer", nullable: false),
                    Apontador_Matricula = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DataAdmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CNH = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CategoriaCNH = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ValidadeCNH = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaquinarioAtualId = table.Column<int>(type: "integer", nullable: true),
                    Matricula = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Cargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_CanteirosObra_CanteiroObraId",
                        column: x => x.CanteiroObraId,
                        principalTable: "CanteirosObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Maquinarios_MaquinarioAtualId",
                        column: x => x.MaquinarioAtualId,
                        principalTable: "Maquinarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroSolicitacao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataNecessidade = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Justificativa = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ObservacoesApontador = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MotivoRejeicao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataAprovacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    LocalSolicitacaoId = table.Column<int>(type: "integer", nullable: false),
                    SolicitadorId = table.Column<int>(type: "integer", nullable: false),
                    ApontadorId = table.Column<int>(type: "integer", nullable: true),
                    DepositoId = table.Column<int>(type: "integer", nullable: true),
                    MaquinarioId = table.Column<int>(type: "integer", nullable: true),
                    CanteiroObraId = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_CanteirosObra_CanteiroObraId",
                        column: x => x.CanteiroObraId,
                        principalTable: "CanteirosObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Depositos_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_LocaisSolicitacao_LocalSolicitacaoId",
                        column: x => x.LocalSolicitacaoId,
                        principalTable: "LocaisSolicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Maquinarios_MaquinarioId",
                        column: x => x.MaquinarioId,
                        principalTable: "Maquinarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Usuarios_ApontadorId",
                        column: x => x.ApontadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Usuarios_SolicitadorId",
                        column: x => x.SolicitadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoricosSolicitacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SolicitacaoId = table.Column<int>(type: "integer", nullable: false),
                    StatusAnterior = table.Column<int>(type: "integer", nullable: false),
                    StatusNovo = table.Column<int>(type: "integer", nullable: false),
                    DataMudanca = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioNome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LatitudeNoMomento = table.Column<double>(type: "double precision", nullable: true),
                    LongitudeNoMomento = table.Column<double>(type: "double precision", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosSolicitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricosSolicitacao_Solicitacoes_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensSolicitacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SolicitacaoId = table.Column<int>(type: "integer", nullable: false),
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    Quantidade = table.Column<double>(type: "double precision", nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Atendido = table.Column<bool>(type: "boolean", nullable: false),
                    QuantidadeEntregue = table.Column<double>(type: "double precision", nullable: true),
                    DataEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensSolicitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensSolicitacao_Materiais_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensSolicitacao_Solicitacoes_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_CanteiroObraId",
                table: "Depositos",
                column: "CanteiroObraId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoquesMateriais_DepositoId_MaterialId",
                table: "EstoquesMateriais",
                columns: new[] { "DepositoId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstoquesMateriais_MaterialId",
                table: "EstoquesMateriais",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosSolicitacao_DataMudanca",
                table: "HistoricosSolicitacao",
                column: "DataMudanca");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosSolicitacao_SolicitacaoId",
                table: "HistoricosSolicitacao",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensSolicitacao_MaterialId",
                table: "ItensSolicitacao",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensSolicitacao_SolicitacaoId",
                table: "ItensSolicitacao",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_LocaisSolicitacao_CanteiroObraId",
                table: "LocaisSolicitacao",
                column: "CanteiroObraId");

            migrationBuilder.CreateIndex(
                name: "IX_LocaisSolicitacao_Identificador",
                table: "LocaisSolicitacao",
                column: "Identificador");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinarios_CanteiroObraId",
                table: "Maquinarios",
                column: "CanteiroObraId");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinarios_Nome",
                table: "Maquinarios",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_Codigo",
                table: "Materiais",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_Nome",
                table: "Materiais",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_ApontadorId",
                table: "Solicitacoes",
                column: "ApontadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_CanteiroObraId",
                table: "Solicitacoes",
                column: "CanteiroObraId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_DataCriacao",
                table: "Solicitacoes",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_DepositoId",
                table: "Solicitacoes",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_LocalSolicitacaoId",
                table: "Solicitacoes",
                column: "LocalSolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_MaquinarioId",
                table: "Solicitacoes",
                column: "MaquinarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_NumeroSolicitacao",
                table: "Solicitacoes",
                column: "NumeroSolicitacao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_SolicitadorId",
                table: "Solicitacoes",
                column: "SolicitadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_Status",
                table: "Solicitacoes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CanteiroObraId",
                table: "Usuarios",
                column: "CanteiroObraId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_MaquinarioAtualId",
                table: "Usuarios",
                column: "MaquinarioAtualId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstoquesMateriais");

            migrationBuilder.DropTable(
                name: "HistoricosSolicitacao");

            migrationBuilder.DropTable(
                name: "ItensSolicitacao");

            migrationBuilder.DropTable(
                name: "Materiais");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "Depositos");

            migrationBuilder.DropTable(
                name: "LocaisSolicitacao");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Maquinarios");

            migrationBuilder.DropTable(
                name: "CanteirosObra");
        }
    }
}
