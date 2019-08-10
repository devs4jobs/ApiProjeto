
using Model;
using System.Collections.Generic;

namespace Core
{
    public class PautaCore
    {

        private Pauta _pauta { get; set; }
        public PautaCore(Pauta pauta)
        {
            _pauta = pauta;
        }
        public PautaCore() { }

        public Pauta Create(string id) => null;

        public Pauta FindBy(string id) => null;

        public Pauta FindAll() => null;

        public Pauta Update(string id) => null;

        public Pauta Delete(string id) => null;
    }
}