using System.Collections.Generic;

namespace Model
{
    public class Sessao : Base
    {
        public bool Aberta;
        public string dataTermino;
        public List<Voto> urnasSessao { get; set; } = new List<Voto>();
        public List<Eleitor> eleitoresSessao { get; set; } = new List<Eleitor>();
        public List<Pauta> pautasSessao { get; set; } = new List<Pauta>();
    }
}
