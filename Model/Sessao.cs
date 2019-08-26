using System;
using System.Collections.Generic;

namespace Model
{
    public class Sessao:Base
    {
        public List<Pauta> Pautas { get; set; }
        public List<Eleitor> Eleitores { get; set; } 
        public bool Status { get; set; }

        public bool PautaTrue(List<Pauta> pautas)
        {
            foreach(var temp in pautas)
            {
                if (temp.Concluida == false)
                    return false;
            }
            return true;
        }
    }
}
