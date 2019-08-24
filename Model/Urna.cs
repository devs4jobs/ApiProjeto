using System;
using System.Collections.Generic;

namespace Model
{
    public class Urna : Base
    {
        public Guid EleitorId { get; set; } = Guid.NewGuid();
        public bool Votada { get; set; }
        public bool VotoAFavor { get; set; }
        public List<Eleitor> eleitoresUrna { get; set; } = new List<Eleitor>();
        public List<Pauta> pautasUrna { get; set; } = new List<Pauta>();
    }
}
