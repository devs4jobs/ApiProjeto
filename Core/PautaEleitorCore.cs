
using System;
using System.Collections.Generic;
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


        public PautaEleitor Cadastrar(PautaEleitor pautaEleitor) => null;
        public PautaEleitor ProcurarID(string id) => null;
        public List<PautaEleitor> ProcurarTodos() => null;

        public PautaEleitor Atualizar(string id) => null;

        public void Excluir(string id) { }
    }
}
