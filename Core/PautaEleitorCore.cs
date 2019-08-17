using Model;
namespace Core
{
    public class PautaEleitorCore 
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
