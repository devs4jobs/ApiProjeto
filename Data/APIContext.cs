using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore;

namespace Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options)
            : base(options)
        { }


        public DbSet<Eleitor> Eleitors {get;set;}
        public DbSet<Pauta> Pautas {get;set;}
        public DbSet<PautaEleitor> PautaEleitors {get;set;}

    }
}
