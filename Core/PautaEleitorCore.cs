using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class PautaEleitorCore
    {
        private PautaEleitor _pautaEleitor { get; set; }
        public PautaEleitorCore(PautaEleitor pautaEleitor) 
        {
            _pautaEleitor = pautaEleitor;
        }
    }
}
