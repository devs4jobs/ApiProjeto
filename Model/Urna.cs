using System;
using System.Collections.Generic;

namespace Model
{
    public class Urna  
    {
        public Guid PautaId { get; set; } = Guid.NewGuid();
        public Guid EleitorId { get; set; } = Guid.NewGuid();
        public bool Votada { get; set; }
        public bool VotoAFavor { get; set; }   
    }
}
