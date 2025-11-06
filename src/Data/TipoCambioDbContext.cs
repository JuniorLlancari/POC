using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipoCambio.Data
{
    public class TipoCambioDbContext : DbContext
    {
        public TipoCambioDbContext(DbContextOptions<TipoCambioDbContext> options) : base(options) { }
        public DbSet<Models.TipoCambio> TiposCambio { get; set; }
    }
}
