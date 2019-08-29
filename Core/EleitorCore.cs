using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;

namespace Core
{
    public class EleitorCore : AbstractValidator<Eleitor>
    {
        private Eleitor _eleitor;
        public Sistema db { get; set; }
        public EleitorCore()
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        }

        public EleitorCore(Eleitor eleitor)
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

            _eleitor = eleitor;

            RuleFor(e => e.Documento)
                .Length(11, 11)
                .NotNull()
                .WithMessage("Cpf inválido");

            RuleFor(e => e.Nome)
                .MinimumLength(3)
                .NotNull()
                .WithMessage("O nome deve ser preenchido e deve ter o mínimo de 3 caracteres");

            RuleFor(a => a.Sexo.ToUpper())
                .NotNull().Must(a => a == "MASCULINO" || a == "FEMININO")
                .WithMessage("Sexo inválida.");
        }
        // Método para cadastro.
        public Retorno CadastroEleitor()
        {
            var results = Validate(_eleitor);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors.Select(c => c.ErrorMessage).ToList() };

            if (db.Eleitores.Exists(c => c.Documento == _eleitor.Documento))
                return new Retorno() { Status = false, Resultado = null };


            db.Eleitores.Add(_eleitor);
            file.ManipulacaoDeArquivos(false, db);

            return new Retorno() { Status = true, Resultado = _eleitor };
        }
      
        // Método para buscar um eleitor
        public Retorno AcharUm(string id)
        {
            if (!db.Eleitores.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Esse eleitor não existe na base de dados" };

            var UmEleitor = db.Eleitores.Find(c => c.Id.ToString() == id);
            return new Retorno() { Status = true, Resultado = UmEleitor };
        }
        //Método para buscar todos os eleitores 
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Eleitores.OrderBy(x => x.Nome) };
   
        // Método deletar por id
        public Retorno DeletarId(string id)
        {
            if (!db.Eleitores.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Esse eleitor não existe na base de dados" };

            db.Eleitores.Remove(db.Eleitores.Find(c => c.Id.ToString() == id));

            file.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = null };
        }

        // Método para efetuar a atualização de um eleitor por id
        public Retorno AtualizarUm(string id, Eleitor eleitor)
        {
            if (!db.Eleitores.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Esse eleitor não existe na base de dados" };

            var umEleitor = db.Eleitores.Find(c => c.Id.ToString() == id);

            if (eleitor.Nome != null)
                umEleitor.Nome = eleitor.Nome;

            if (eleitor.Documento != null)
                umEleitor.Documento = eleitor.Documento;

            if (eleitor.Sexo != null)
                umEleitor.Sexo = eleitor.Sexo;

            if (eleitor.Idade != 0)
                umEleitor.Idade = eleitor.Idade;

            file.ManipulacaoDeArquivos(false, db);
            return new Retorno { Status = true, Resultado = umEleitor };
        }

        // Método para realizar a paginação
        public Retorno PorPaginacao(string ordempor, int numeroPagina, int qtdRegistros)
        {
            // checo se as paginação é valida pelas variaveis e se sim executo o skip take contendo o calculo
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor == null)
                return new Retorno() { Status = true, Resultado = db.Eleitores.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "NOME")
                return new Retorno() { Status = true, Resultado = db.Eleitores.OrderBy(c => c.Nome).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por data. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "DATA")
                return new Retorno() { Status = true, Resultado = db.Eleitores.OrderBy(c => c.DataCadastro).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por idade. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "IDADE")
                return new Retorno() { Status = true, Resultado = db.Eleitores.OrderBy(c => c.Idade).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // se nao der pra fazer a paginação
            return new Retorno() { Status = false, Resultado = "Dados inválidos, nao foi possivel realizar a paginação." };
        }
    }
}
