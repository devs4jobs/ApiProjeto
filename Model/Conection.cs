using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class Conectionn : DbContext
    {
        public Conectionn(DbContextOptions<Conectionn> options)
          : base(options)
        {
        }

        public DbSet<Pauta> Pautas { get; set; }
        public DbSet<Eleitor> Eleitores { get; set; }

    }
}
