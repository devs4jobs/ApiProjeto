using System.Collections.Generic;

namespace Model
{
    public class Sessao : Base
    {
        public List<Pauta> lstPautas { get; set; } = new List<Pauta>();
        public List<PautaEleitor> lstPautaEleitor { get; set; } = new List<PautaEleitor>();
        public bool status { get; set; }
    }
}
