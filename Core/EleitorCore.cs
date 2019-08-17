using FluentValidation;
using Model;
namespace Core
{
    public class EleitorCore : AbstractValidator<Eleitor>
    {
        private Eleitor _eleitor { get; set; }
       
        public EleitorCore( Eleitor eleitor)
        {
            _eleitor = eleitor;

            RuleFor(e => e.Documento).NotNull().WithMessage("O Campo documento não pode ser nulo!");

        }

        public dynamic Validacao()
        {
            var results = Validate(_eleitor);

            if (!results.IsValid)
                return results.Errors;

            return null;
        }


        public EleitorCore()  { }

        public Eleitor Cadastrar(Eleitor eleitor) =>eleitor;

        public Eleitor Achar(string id ) => null;

        public Eleitor AcharTodos() => null;

        public Eleitor Atualizar(string id) => null;

        public void DeletarUm(string id) { }

       
    }
}
