using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
   public class PautaEleitor : Base
    {
        // Classe para amarrar a pauta e o eleitor
        public Guid PautaId { get; set; }
        public Guid EleitorId { get; set; }
        public bool Votou { get; set; }
        public string Voto { get; set; }
    }
}
