using Model;

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

        public Pauta Cadastrar(Pauta pauta) => null;
        public Pauta Achar(string id) => null;
        public Pauta AcharTodos() => null;
        public Pauta Atualizar(string id) => null;
        public void DeletarUm(string id) { }


    }
}