using Importa_planilha_excel.Models;
using Microsoft.EntityFrameworkCore;

namespace Importa_planilha_excel.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProdutoModel> Produtos { get; set; }   
    }
}
