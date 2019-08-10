
using Model;

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

        public Eleitor Create(string id) => null;


        public Eleitor FindBy(string id) => null;

        public Eleitor FindAll() => null;

        public Eleitor Update(string id) => null;

        public Eleitor Delete(string id) => null;
    }
}
