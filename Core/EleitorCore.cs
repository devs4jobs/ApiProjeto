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
        private Sistema db { get; set; }
        public EleitorCore(Eleitor eleitor)
        {
            _eleitor = eleitor;

            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null)
                db = new Sistema();

            RuleFor(e => e.Documento)
                .Length(11, 11)
                .NotNull()
                .WithMessage("Cpf inválido");

            RuleFor(e => e.Nome)
                .MinimumLength(3)
                .NotNull()
                .WithMessage("O nome deve ser preenchido e deve ter o mínimo de 3 caracteres");
        }

        // Construtor a necessidade do objeto eleitor
        public EleitorCore()
        {
            // Popula o model "base" em memória
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            // Cria instância do objeto sistema para futura população caso a leitura do arquvio tenha retorno nulo
            if (db == null)
                db = new Sistema();
        }

        public Retorno CadastroEleitor()
        {
            var results = Validate(_eleitor);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            // Caso o modelo seja válido, escreve no arquivo db
            if (db.Eleitores.Where(e => e.Documento == _eleitor.Documento).Any())
                return new Retorno { Status = false, Resultado = "Eleitor já existente" };

            db.Eleitores.Add(_eleitor);
            file.ManipulacaoDeArquivos(false, db);

            return new Retorno() { Status = true, Resultado = _eleitor };
        }
        // Busca eleitor por Id (Query)
        public Retorno BuscaEleitorPorId(string id)
        {
            try
            {
                var eleitor = db.Eleitores.Where(e => e.Id == new Guid(id));
                if (eleitor.Any())
                    return new Retorno() { Status = true, Resultado = eleitor };

                return new Retorno { Status = false, Resultado = "Eleitor inválido" };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = "Eleitor inválido" };
            }

        }

        // Consulta toda a base de eleitores
        public Retorno BuscaEleitores()
        {
            try
            {
                if (db.Eleitores.Any())
                    return new Retorno() { Status = true, Resultado = db.Eleitores };

                return new Retorno { Status = false, Resultado = "Nenhum eleitor cadastrado" };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = "Não foi possiível buscar os eleitores" };
            }

        }

        public Retorno AtualizaEleitor(string id)
        {
            try
            {

                var eleitor = db.Eleitores.FirstOrDefault(e => e.Id == new Guid(id));

                if (eleitor == null)
                    return new Retorno() { Status = false, Resultado = "Eleitor não encontrado" };

                if (_eleitor.Nome != null) eleitor.Nome = _eleitor.Nome;
                if (_eleitor.Idade != 0) eleitor.Idade = _eleitor.Idade;
                if (_eleitor.Documento != null) eleitor.Documento = _eleitor.Documento;
                if (_eleitor.Sexo != null) eleitor.Sexo = _eleitor.Sexo;

                file.ManipulacaoDeArquivos(false, db);

                return new Retorno { Status = true, Resultado = eleitor };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = "Eleitor inválido" };
            }

        }

        public Retorno DeletaEleitor(string id)
        {
            try
            {

                var eleitor = db.Eleitores.FirstOrDefault(e => e.Id == new Guid(id));

                if (eleitor==null)
                    return new Retorno() { Status = false, Resultado = "Eleitor não encontrado" };

                db.Eleitores.Remove(eleitor);
                file.ManipulacaoDeArquivos(false, db);

                return new Retorno { Status = true, Resultado = "Eleitor deletado" };
            }
            catch (Exception)
            {
                return new Retorno { Status = false, Resultado = "Eleitor inválido" };
            }

        }

    }
}
