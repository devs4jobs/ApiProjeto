﻿using Model;
using FluentValidation;
using Core.util;
using System.Linq;

namespace Core
{
    public class PautaCore : AbstractValidator<Pauta>
    {
        private Pauta _pauta;
        public Sistema db { get; set; }
        public PautaCore()
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        }
        public PautaCore(Pauta Pauta)
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

            _pauta = Pauta;

            RuleFor(e => e.Descricao).
                MinimumLength(6)
                .NotNull()
                .WithMessage("Tamanho da descrição invalida");
        }

        // Método para cadastro.
        public Retorno CadastroEleitor()
        {
            var results = Validate(_pauta);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors.Select(c => c.ErrorMessage).ToList() };

            if (db.Pautas.Exists(c => c.Descricao == _pauta.Descricao))
                return new Retorno() { Status = false, Resultado = null };

            db.Pautas.Add(_pauta);
            file.ManipulacaoDeArquivos(false, db);


            return new Retorno() { Status = true, Resultado = _pauta };
        }

        // Método para buscar uma pauta
        public Retorno AcharUm(string id)
        {
            if (!db.Pautas.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Essa pauta não existe na base de dados" };

            return new Retorno() { Status = true, Resultado = db.Pautas.Find(c => c.Id.ToString() == id) };
        }

        // Método para retornar todas as pautas
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Pautas };
     
    
        // Método para deletar por id
        public Retorno DeletarId(string id)
        {
            if (!db.Pautas.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Essa pauta não existe na base de dados" };

            db.Pautas.Remove(db.Pautas.Find(c => c.Id.ToString() == id));

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = null };

        }

        // Método para atualizar a pauta por id
        public Retorno AtualizarUm(string id, Pauta pauta)
        {
            if (!db.Eleitores.Any(p => p.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = "Essa pauta  não existe na base de dados" };

            var umaPauta = db.Pautas.Find(c => c.Id.ToString() == id);
     
            if (pauta.Descricao != null)
                umaPauta.Descricao = pauta.Descricao;

            if (pauta.Concluida == true)
                umaPauta.Concluida = pauta.Concluida;


            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = umaPauta };
        }

        //Método para relizar a busca paginada.
        public Retorno PorPaginacao(string ordempor ,int numeroPagina, int qtdRegistros)
        {
            // checo se as paginação é valida pelas variaveis e se sim executo o skip take contendo o calculo
            if (numeroPagina > 0 && qtdRegistros >0 && ordempor == null)
                return new Retorno() { Status = true, Resultado = db.Pautas.Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por nome. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "NOME")
                return new Retorno() { Status = true, Resultado = db.Pautas.OrderBy(c => c.Descricao).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };
           
            // faço a verificação e depois ordeno por data. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "DATA")
                return new Retorno() { Status = true, Resultado = db.Pautas.OrderBy(c => c.DataCadastro).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // faço a verificação e depois ordeno por status. 
            if (numeroPagina > 0 && qtdRegistros > 0 && ordempor.ToUpper().Trim() == "STATUS")
                return new Retorno() { Status = true, Resultado = db.Pautas.OrderBy(c => c.Concluida).Skip((numeroPagina - 1) * qtdRegistros).Take(qtdRegistros).ToList() };

            // se nao der pra fazer a paginação
            return new Retorno() { Status = false, Resultado = "Dados inválidos, nao foi possivel realizar a paginação." };

        }
    }
}
