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

            RuleFor(e => e.Sexo.ToLower())
                .NotNull()
                .Must(e => e == "masculino" || e == "feminino")
                .WithMessage("Sexo Invalido");

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

        public Retorno PorData(string date,string time)
        {
            //Testa se os dados são datas
            if (!DateTime.TryParse(date, out DateTime date1) && !DateTime.TryParse(time, out DateTime time1))
                return new Retorno() { Status = false, Resultado = "Dados Invalidos" };

            //Caso Data final seja nula ou errada
            if (!DateTime.TryParse(time, out time1))
                return new Retorno() { Status = true, Resultado = Db.Eleitores.Where(x => x.DataCadastro >= date1) };

            //Caso Data inicial seja nula ou errada
            if (!DateTime.TryParse(date, out date1))
                return new Retorno() { Status = true, Resultado = Db.Eleitores.Where(x => x.DataCadastro <= time1) };

            return new Retorno() { Status = true, Resultado = Db.Eleitores.Where(x => x.DataCadastro >= date1 && x.DataCadastro <= time1) };
        }

        public Retorno PorPagina(int NPagina, string Direcao, int TPagina)
        {

            if (Direcao.ToLower() == "asc"&& NPagina >= 1 && TPagina >= 1)
                return new Retorno() { Status = true, Resultado = Db.Eleitores.OrderBy(x => x.Nome).Skip((NPagina - 1) * TPagina).Take(TPagina).ToList() };

            if (Direcao.ToLower() == "des"&& NPagina >= 1 && TPagina >= 1)
                return new Retorno() { Status = true, Resultado = Db.Eleitores.OrderByDescending(x => x.Nome).Skip((NPagina - 1) * TPagina).Take(TPagina).ToList() };

            //se paginação é não é possivel
            return new Retorno() { Status = false, Resultado = "Digite as propriedades corretas" };
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
