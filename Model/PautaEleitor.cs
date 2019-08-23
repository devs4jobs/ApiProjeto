using System;

namespace Model
{
    public class PautaEleitor
    {
        // model para amarrar a pauta e o eleitor
        public Guid PautaId { get; set; }
        public Guid EleitorId { get; set; }
        public bool Votou { get; set; }
        public string Voto { get; set; }
    }
}
