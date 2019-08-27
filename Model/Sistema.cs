using System.Collections.Generic;

namespace Model
{
    public class Sistema
    {
        public List<Sessao> todasSessoes { get; set; } = new List<Sessao>();
        public List<Eleitor> Eleitores { get; set; } = new List<Eleitor>();
        public List<Pauta> Pautas { get; set; } = new List<Pauta>();
        public List<Voto> Urnas { get; set; } = new List<Voto>();
    }
}
