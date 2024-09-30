using Microsoft.EntityFrameworkCore;
using ProdutoAPI.Models;

namespace ProdutoAPI.Data
{
    // Classe ProdutoContext que herda de DbContext e configura o contexto do banco de dados para a entidade Produto
    public class ProdutoContext : DbContext
    {
        // Construtor que passa as opções de configuração para a classe base (DbContext)
        public ProdutoContext(DbContextOptions<ProdutoContext> options)
            : base(options)
        {
        }

        // DbSet que representa a coleção de Produtos no banco de dados
        public DbSet<Produto> Produtos { get; set; }

        // Método OnModelCreating pode ser sobrescrito para configurar o mapeamento do modelo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
