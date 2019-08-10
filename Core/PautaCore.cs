using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class PautaCore
    {
        private Pauta _pauta { get; set; }
        public PautaCore(Pauta pauta)
        {
            _pauta = pauta;
        }
    }
}
