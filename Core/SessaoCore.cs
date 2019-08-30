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
            //Busca dos dados
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        }
        public SessaoCore(Sessao Sessao)
        {
            _sessao = Sessao;
            //Busca dos dados
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

            RuleFor(a => a.LstPautas).NotEmpty().WithMessage("Lista de Pautas nao pode ser vazia");
            RuleFor(a => a.Status).NotEqual(true).WithMessage("Não é possivel cadastrar uma sessao ja finalizada.");

            _sessao.LstEleitores.ForEach(c => c.TrocandoDados(db.Eleitores.FirstOrDefault(e => e.Id == c.Id)));
            _sessao.LstPautas.ForEach(c => c.TrocandoDados(db.Pautas.FirstOrDefault(e => e.Id == c.Id)));


        }
        // Método para cadastro.
        public Retorno Cadastro()
        {
            var results = Validate(_sessao);

            // Se o modelo é inválido retorno false
            if (!results.IsValid || !_sessao.ValidaPauta())
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

            return new Retorno() { Status = true, Resultado = db.Sessoes.Find(c => c.Id.ToString() == id) };
        }

        public Retorno PorPaginacao(string ordempor, int numeroPagina, int qtdRegistros)
        {
            // checo se as paginação é valida pelas variaveis e se sim executo o skip take contendo o calculo
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor == null)
                return new Retorno() { Status = true, Resultado = db.Sessoes.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "STATUS")
                return new Retorno() { Status = true, Resultado = db.Sessoes.OrderBy(c => c.Status).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por data. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "DATA")
                return new Retorno() { Status = true, Resultado = db.Sessoes.OrderBy(c => c.DataCadastro).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // se nao der pra fazer a paginação
            return new Retorno() { Status = false, Resultado = "Dados inválidos, nao foi possivel realizar a paginação." };
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
                return new Retorno() { Status = false, Resultado = "Essa sessão nao existe na base de dados" };

            db.Sessoes.Remove(db.Sessoes.Find(c => c.Id.ToString() == id));

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = "Registro removido!" };
        }

        //Método para retornar somente o status de uma unica sessao
        public Retorno RetornaStatus(string id)
        {
            if (!db.Sessoes.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Essa sessão nao existe na base de dados" };

            var umaSessao = db.Sessoes.Find(c => c.Id.ToString() == id);

            if (umaSessao.Status) return new Retorno { Status = true, Resultado = $"O status atual da sessão ID: {umaSessao.Id} está fechada para votações  " };

            return new Retorno { Status = true, Resultado = $"O status atual da sessão ID: {umaSessao.Id} está aberta para votações  " };
        }
    }
}

