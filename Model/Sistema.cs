using System.Collections.Generic;

namespace Model
{
    public class Sistema
    {
        public List<Eleitor> Eleitores { get; set; } = new List<Eleitor>();
        public List<Pauta> Pautas { get; set; } = new List<Pauta>();
        public List<Urna> PautaEleitores { get; set; } = new List<Urna>();
    }
}
