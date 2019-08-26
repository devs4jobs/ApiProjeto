using System.Collections.Generic;

namespace Model
{
    public class Sessao : Base
    {
        public bool Aberta;
        public List<Urna> urnasSessao { get; set; } = new List<Urna>();
        public List<Eleitor> eleitoresSessao { get; set; } = new List<Eleitor>();
        public List<Pauta> pautasSessao { get; set; } = new List<Pauta>();
    }
}
