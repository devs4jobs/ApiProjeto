using Model;
using System;
using FluentValidation;
using Core.util;
using System.Linq;

namespace Core
{
    public class EleitorCore : AbstractValidator<Eleitor>
    {
        private Eleitor _eleitor { get; set; }
        private Sistema Db { get; set; }
        public EleitorCore()
        {
            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();
        }
        public EleitorCore( Eleitor eleitor)
        {
            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();
            _eleitor = eleitor;

            RuleFor(e => e.Documento)
                .Length(11,11)
                .NotNull()
                .WithMessage("Cpf inválido");

            RuleFor(e => e.Idade)
                .GreaterThan(12)
                .WithMessage("O eleitor deve ter mais de 12 anos");

            RuleFor(e => e.Sexo)
                .NotNull()
                .WithMessage("O eleitor deve registrar o sexo");

            RuleFor(e => e.Nome)
                .MinimumLength(3)
                .NotNull()
                .WithMessage("O nome deve ser preenchido e deve ter o mínimo de 3 caracteres");
        }

        public Retorno CadastroEleitor() {

            var results = Validate(_eleitor);

            // Se o modelo é inválido retorno false
            if(!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors};

            if(Db.Eleitores.Find(c=>c.Documento==_eleitor.Documento)!=null)
                return new Retorno() { Status = false, Resultado = "Usuario ja cadastrado" };

            Db.Eleitores.Add(_eleitor);
            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = _eleitor};
        }

        public Retorno ID(Guid id)
        {
           var eleitor = Db.Eleitores.SingleOrDefault(x => x.Id == id);

            if (eleitor == null)
                return new Retorno() { Status = false, Resultado = "Eleitor não existe" };

           return new Retorno() { Status = true, Resultado = eleitor };
        }

        public Retorno Lista()=> new Retorno() { Status = true, Resultado = Db.Eleitores};
        
        public Retorno AtualizaEleitor()
        {

            var eleitor=Db.Eleitores.Find(c => c.Id == _eleitor.Id);
            if(eleitor==null)
                return new Retorno { Status = false, Resultado = "Eleitor não existe" };

            if (_eleitor.Sexo != null) eleitor.Sexo = _eleitor.Sexo;
            if (_eleitor.Idade != 0) eleitor.Idade = _eleitor.Idade;
            if (_eleitor.Documento != null) eleitor.Documento = _eleitor.Documento;
            if (_eleitor.Nome != null) eleitor.Nome = _eleitor.Nome;

            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = eleitor };
        }
        public Retorno DeletaEleitor(Guid id)
        {
            _eleitor=Db.Eleitores.Find(c => c.Id == id);

            if (_eleitor == null)
                return new Retorno() { Status = false, Resultado = "Eleitor não existe" };

            Db.Eleitores.Remove(_eleitor);
            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = _eleitor };
        }
    }
}
