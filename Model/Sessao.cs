using System.Collections.Generic;

namespace Model
{
    class Sessao : Base
    {
        public bool Concluida;
        public List<Urna> Urnas { get; set; } = new List<Urna>();
    }
}
