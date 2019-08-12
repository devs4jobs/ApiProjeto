using Core.Interface;
using Model;
namespace Core
{
    public class EleitorCore : ICore<Eleitor>
    {
        private Eleitor _eleitor { get; set; }
       
        public EleitorCore( Eleitor eleitor)
        {
            _eleitor = eleitor;
         
        }

        public EleitorCore()  { }

        public Eleitor Cadastrar(Eleitor eleitor) => null;

        public Eleitor Achar(string id ) => null;

        public Eleitor AcharTodos() => null;

        public Eleitor Atualizar(string id) => null;

        public void DeletarUm(string id) { }

       
    }
}
