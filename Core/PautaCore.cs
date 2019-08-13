
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

        public Pauta Cadastrar(Pauta pauta) => null;

        public Pauta ProcurarID(string id) => null;

        public List<Pauta> ProcurarTodos() => null;

        public Pauta Atualizar(string id) => null;

        public void Excluir(string id) { }
    }
}