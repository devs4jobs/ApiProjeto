using Model;
using System.Collections.Generic;

namespace Core
{
    public class EleitorCore
    {
        private Eleitor _eleitor { get; set; }
        
        public EleitorCore(Eleitor eleitor)
        {
            _eleitor = eleitor;
        }
        public EleitorCore() { }

        public Eleitor Cadastrar(Eleitor eleitor) => null;

        public Eleitor ProcurarID(string id) => null;

        public List<Eleitor> ProcurarTodos() => null;

        public Eleitor Atualizar(string id) => null;

        public void Excluir(string id) { }
    }
}
