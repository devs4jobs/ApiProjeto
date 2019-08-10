using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class PautaCore
    {
        private Pauta _pauta { get; set; }
        public PautaCore(Pauta Pauta)
        {
            _pauta = Pauta;
        }
        public PautaCore() { }

        public Pauta Achar(string id)
        {
            return null;
        }

        public Pauta AcharTodos()
        {
            return null;
        }
        public void DeletarUm(string id)
        {

        }

        public void Atualizar(string id)
        {

        }
    }
}