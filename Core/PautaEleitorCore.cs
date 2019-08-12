using System;
using System.Collections.Generic;
using System.Text;
using Core.Interface;
using Model;
namespace Core
{
    public class PautaEleitorCore : ICore<PautaEleitor>
    {
        private PautaEleitor pautaEleitor;

        public PautaEleitorCore(PautaEleitor pautaEleitor)
        {
            this.pautaEleitor = pautaEleitor;
        }
        public PautaEleitorCore() { }

        public PautaEleitor Cadastrar(PautaEleitor pauta) => null;
        public PautaEleitor Achar(string id) => null;
        public PautaEleitor AcharTodos() => null;
        public PautaEleitor Atualizar(string id) => null;
        public void DeletarUm(string id) { }
    }
}
