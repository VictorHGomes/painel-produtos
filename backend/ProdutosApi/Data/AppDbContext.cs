using Microsoft.EntityFrameworkCore;
using ProdutosApi.Models;

namespace ProdutosApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<HistoricoProduto> Historicos => Set<HistoricoProduto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(e =>
        {
            e.Property(p => p.Nome).IsRequired().HasMaxLength(150);
            e.Property(p => p.Preco).HasPrecision(12, 2);
        });

        modelBuilder.Entity<Categoria>(e =>
        {
            e.Property(c => c.Nome).IsRequired().HasMaxLength(80);
        });
    }
}