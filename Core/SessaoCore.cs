using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;

namespace Core
{
    public class SessaoCore : AbstractValidator<Sessao>
    {
        private Sessao _sessao;
        public Sistema db { get; set; }
        public SessaoCore()
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        }
        public SessaoCore(Sessao umasessao)

        {
            _sessao = umasessao;
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

            RuleFor(a => a.LstPautas).NotEmpty().WithMessage("Lista de Pautas nao pode ser vazia");
            RuleFor(a => a.Status).NotEqual(true).WithMessage("Não é possivel cadastrar uma sessao ja finalizada.");
        }
        // Método para cadastro.
        public Retorno Cadastro()
        {

            var results = Validate(_sessao);

            // Se o modelo é inválido retorno false
            if (!results.IsValid || _sessao.ValidaPauta() || _sessao.ValidaSessao(_sessao.DataFim))
                return new Retorno { Status = false, Resultado = results.Errors.Select(c => c.ErrorMessage).ToList() };


            db.Sessoes.Add(_sessao);
            file.ManipulacaoDeArquivos(false, db);


            return new Retorno() { Status = true, Resultado = _sessao };
        }

        // Método para buscar uma sessao
        public Retorno AcharUm(string id)
        {
            if (!db.Sessoes.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Essa sessão nao existe!" };

            var SessaoUm = db.Sessoes.Find(c => c.Id.ToString() == id);
            return new Retorno() { Status = true, Resultado = SessaoUm };
        }

        public Retorno BuscaPorData(string dataComeço, string dataFim)
        {
            // Tento fazer a conversao e checho se ela nao for feita corretamente, se ambas nao forem corretas retorno FALSE
            if (!DateTime.TryParse(dataComeço, out DateTime primeiraData) && !DateTime.TryParse(dataFim, out DateTime segundaData))
                return new Retorno() { Status = false, Resultado = "Dados Invalidos" };

            // Tento fazer a conversao da segunda data for invalida faço somente a pesquisa da primeira data
            if (!DateTime.TryParse(dataFim, out segundaData))
                return new Retorno { Status = true, Resultado = db.Sessoes.Where(c => c.DataCadastro >= primeiraData).ToList() };

            // Tento fazer a conversao da primeiradata for invalida faço somente a pesquisa da segunda data
            if (!DateTime.TryParse(dataComeço, out primeiraData))
                return new Retorno { Status = true, Resultado = db.Sessoes.Where(c => c.DataCadastro <= segundaData).ToList() };

            // returno a lista completa entre as duas datas informadas.
            return new Retorno { Status = true, Resultado = db.Sessoes.Where(c => c.DataCadastro >= primeiraData && c.DataCadastro <= segundaData).ToList() };

        }


        // Método para retornar todas as sessoes
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Sessoes.OrderBy(c => c.Id) };


        // Método para deletar por id
        public Retorno DeletarId(string id)
        {
            if (!db.Sessoes.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Essa sessão nao existe!" };

            var umaSessao = db.Sessoes.Find(c => c.Id.ToString() == id);

            db.Sessoes.Remove(umaSessao);

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = null };

        }

        // Método para retornar o status das sessoes
        public Retorno RetornaStatus(string id)
        {
            if (!db.Sessoes.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Essa sessão nao existe!" };

            var umaSessao = db.Sessoes.Find(c => c.Id.ToString() == id);

            if (umaSessao.Status) return new Retorno { Status = true, Resultado = $"O status atual da sessão ID: {umaSessao.Id} está fechada para votações  " };

            else
                return new Retorno { Status = true, Resultado = $"O status atual da sessão ID: {umaSessao.Id} está aberta para votações  " };
        }


    }
}

