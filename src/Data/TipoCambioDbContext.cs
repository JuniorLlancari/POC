using Microsoft.EntityFrameworkCore;

namespace TipoCambio.Data;

public class TipoCambioDbContext : DbContext
{
    public TipoCambioDbContext(DbContextOptions<TipoCambioDbContext> options) : base(options) { }
    public DbSet<Models.TipoCambio> TiposCambio { get; set; }
}

