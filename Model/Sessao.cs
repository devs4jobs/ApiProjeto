using System.Collections.Generic;

namespace Model
{
    public class Sessao:Base
    {
        public List<Pauta> Pautas { get; set; }
        public List<Eleitor> Eleitores { get; set; } 
        public bool Status { get; set; }

    }
}
