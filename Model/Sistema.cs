using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Sistema
    {
 
        public List<Eleitor> Eleitores { get; set; } = new List<Eleitor>();
        public List<Pauta> Pautas { get; set; } = new List<Pauta>();
        public List<PautaEleitor> EleitoresPauta { get; set; } = new List<PautaEleitor>();
    }
}
