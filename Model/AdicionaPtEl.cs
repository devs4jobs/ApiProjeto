using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class AdicionaPtEl
    {
        public string idSessao { get; set; }
        public List<Eleitor> eleitoresId { get; set; }
        public List<Pauta> pautasId { get; set; }
    }
}
