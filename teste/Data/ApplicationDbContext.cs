using Microsoft.EntityFrameworkCore;
using teste.Models;
using teste.Utils;

namespace teste.Data
{
    /// <summary>
    /// Contexto do banco de dados para o sistema de gestão de canteiro de obras
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            ConvertDateTimeToUtc();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDateTimeToUtc();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ConvertDateTimeToUtc()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added ||
                           e.State == Microsoft.EntityFrameworkCore.EntityState.Modified);

            foreach (var entry in entries)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.ClrType == typeof(DateTime))
                    {
                        if (property.CurrentValue != null)
                        {
                            var dateTime = (DateTime)property.CurrentValue;
                            // Converte para UTC considerando que está no fuso de Brasília
                            property.CurrentValue = TimeZoneHelper.ConvertToUtc(dateTime);
                        }
                    }
                    else if (property.Metadata.ClrType == typeof(DateTime?))
                    {
                        var dateTimeValue = property.CurrentValue as DateTime?;
                        if (dateTimeValue.HasValue)
                        {
                            // Converte para UTC considerando que está no fuso de Brasília
                            property.CurrentValue = TimeZoneHelper.ConvertToUtc(dateTimeValue.Value);
                        }
                    }
                }
            }
        }

        // DbSets principais
        public DbSet<CanteiroObra> CanteirosObra { get; set; }
        public DbSet<LocalSolicitacao> LocaisSolicitacao { get; set; }
        public DbSet<Deposito> Depositos { get; set; }
        public DbSet<Material> Materiais { get; set; }
        public DbSet<EstoqueMaterial> EstoquesMateriais { get; set; }
        public DbSet<Maquinario> Maquinarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Apontador> Apontadores { get; set; }
        public DbSet<Solicitador> Solicitadores { get; set; }
        public DbSet<OperadorMaquinario> OperadoresMaquinario { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }
        public DbSet<ItemSolicitacao> ItensSolicitacao { get; set; }
        public DbSet<HistoricoSolicitacao> HistoricosSolicitacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar todas as propriedades DateTime para usar UTC no PostgreSQL
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp with time zone");
                    }
                }
            }

            // Configuração de LocalSolicitacao
            modelBuilder.Entity<LocalSolicitacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.OwnsOne(e => e.Localizacao, loc =>
                {
                    loc.Property(l => l.Latitude).IsRequired();
                    loc.Property(l => l.Longitude).IsRequired();
                });
                entity.HasIndex(e => e.Identificador);
            });

            // Configuração de Depósito
            modelBuilder.Entity<Deposito>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.OwnsOne(e => e.Localizacao, loc =>
                {
                    loc.Property(l => l.Latitude).IsRequired();
                    loc.Property(l => l.Longitude).IsRequired();
                });
            });

            // Configuração de Canteiro de Obra
            modelBuilder.Entity<CanteiroObra>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.OwnsOne(e => e.LocalizacaoCentral, loc =>
                {
                    loc.Property(l => l.Latitude).IsRequired();
                    loc.Property(l => l.Longitude).IsRequired();
                });
            });

            // Configuração de Maquinário
            modelBuilder.Entity<Maquinario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.OwnsOne(e => e.LocalizacaoAtual, loc =>
                {
                    loc.Property(l => l.Latitude).IsRequired();
                    loc.Property(l => l.Longitude).IsRequired();
                });
                entity.HasIndex(e => e.Nome);
            });

            // Configuração de EstoqueMaterial
            modelBuilder.Entity<EstoqueMaterial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.DepositoId, e.MaterialId }).IsUnique();
                
                entity.HasOne(e => e.Material)
                    .WithMany(m => m.EstoquesMateriais)
                    .HasForeignKey(e => e.MaterialId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Deposito)
                    .WithMany(d => d.EstoqueMateriais)
                    .HasForeignKey(e => e.DepositoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração de Solicitação
            modelBuilder.Entity<Solicitacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NumeroSolicitacao).IsUnique();
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.DataCriacao);

                entity.HasOne(e => e.LocalSolicitacao)
                    .WithMany(l => l.Solicitacoes)
                    .HasForeignKey(e => e.LocalSolicitacaoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Solicitador)
                    .WithMany(s => s.SolicitacoesCriadas)
                    .HasForeignKey(e => e.SolicitadorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Apontador)
                    .WithMany(a => a.SolicitacoesGerenciadas)
                    .HasForeignKey(e => e.ApontadorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Deposito)
                    .WithMany()
                    .HasForeignKey(e => e.DepositoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Maquinario)
                    .WithMany(m => m.SolicitacoesAtribuidas)
                    .HasForeignKey(e => e.MaquinarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração de ItemSolicitacao
            modelBuilder.Entity<ItemSolicitacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Solicitacao)
                    .WithMany(s => s.Itens)
                    .HasForeignKey(e => e.SolicitacaoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Material)
                    .WithMany(m => m.ItensSolicitacao)
                    .HasForeignKey(e => e.MaterialId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração de HistoricoSolicitacao
            modelBuilder.Entity<HistoricoSolicitacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.DataMudanca);

                entity.HasOne(e => e.Solicitacao)
                    .WithMany(s => s.Historico)
                    .HasForeignKey(e => e.SolicitacaoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração de Usuário (herança TPH - Table Per Hierarchy)
            modelBuilder.Entity<Usuario>()
                .HasDiscriminator<TipoUsuario>("TipoUsuario")
                .HasValue<Apontador>(TipoUsuario.Apontador)
                .HasValue<Solicitador>(TipoUsuario.Solicitador)
                .HasValue<OperadorMaquinario>(TipoUsuario.OperadorMaquinario)
                .HasValue<Usuario>(TipoUsuario.Administrador);

            // Configuração de OperadorMaquinario
            modelBuilder.Entity<OperadorMaquinario>()
                .HasOne(e => e.MaquinarioAtual)
                .WithOne(m => m.OperadorAtual)
                .HasForeignKey<OperadorMaquinario>(e => e.MaquinarioAtualId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configuração de Material
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Codigo).IsUnique();
                entity.HasIndex(e => e.Nome);
            });
        }
    }
}
