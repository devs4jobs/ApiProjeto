using System.Collections.Generic;

namespace Model
{
    public class Sessao : Base
    {
        public bool Aberta;
        public List<Urna> Urnas { get; set; } = new List<Urna>();
        public List<Eleitor> eleitoresUrna { get; set; } = new List<Eleitor>();
        public List<Pauta> pautasUrna { get; set; } = new List<Pauta>();
    }
}
