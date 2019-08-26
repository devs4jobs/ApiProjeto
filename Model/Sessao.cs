using System.Collections.Generic;

namespace Model
{
    public class Sessao : Base
    {
        public List<Pauta> lstPautas { get; set; } = new List<Pauta>();
        public List<Eleitor> lstEleitores { get; set; } = new List<Eleitor>();
        public bool Status { get; set; }
    }
}
