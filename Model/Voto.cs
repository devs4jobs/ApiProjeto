using System;
using System.Collections.Generic;

namespace Model
{
    public class Voto  
    {
        public Guid sessaoId { get; set; } = Guid.NewGuid();
        public Guid PautaId { get; set; } = Guid.NewGuid();
        public Guid EleitorId { get; set; } = Guid.NewGuid();       
        public bool VotoAFavor { get; set; }   
    }
}
