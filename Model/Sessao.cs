using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Sessao : Base
    {
        // instancia de uma votãção 

        public List<Pauta> LstPautas { get; set; } = new List<Pauta>();
        public List<PautaEleitor> LstPautaEleitores { get; set; } = new List<PautaEleitor>();
        public bool Status { get; set; }  

    }
}
