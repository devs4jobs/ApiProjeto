using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db
{
    public class EleicaoContext:DbContext
    {
        // Classe para a Conexao com o banco de dados, entitiy.
        public DbSet<Eleitor> Eleitores { get; set; }
        public DbSet<Pauta> Pautas { get; set; }

        //Construtor
        public EleicaoContext(DbContextOptions<EleicaoContext> options):base (options)  { }
    }
}
