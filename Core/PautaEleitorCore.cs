
using System;
using Model;

namespace Core
{
    public class PautaEleitorCore
    { 
        private PautaEleitor _pautaEleitor;

        public PautaEleitorCore(PautaEleitor pautaEleitor)
        {
            _pautaEleitor = pautaEleitor;
        }

        public PautaEleitorCore() { }


        public PautaEleitor Create(string id) => null;
        public PautaEleitor FindBy(string id) => null;
        public PautaEleitor FindAll() => null;

        public PautaEleitor Update(string id) => null;

        public PautaEleitor Delete(string id) => null;
    }
}
