using Core.util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class SessaoCore : AbstractValidator<Sessao>
    {
        private Sessao _Sessao { get; set; }
        private Sistema Db { get; set; }
        public SessaoCore()
        {

            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();
        }
        public SessaoCore(Sessao sessao)
        {
            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();

            _Sessao = sessao;

            RuleFor(e => e.Eleitores)
                .NotNull()
                .WithMessage("Eleitores não pode ser nulo");

            RuleFor(e => e.Pautas)
                .NotNull()
                .WithMessage("Pautas não pode ser nulo");

        }

        public Retorno IniciarSessao()
        {

            var results = Validate(_Sessao);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors.Select(c => c.ErrorMessage) };

            _Sessao.Status = true;
            _Sessao.Eleitores.ForEach(c => c = Db.Eleitores.SingleOrDefault(d => c.Id == d.Id));
            _Sessao.Pautas.ForEach(c => c = Db.Pautas.SingleOrDefault(d => c.Id == d.Id));


            Db.Sessaos.Add(_Sessao);

            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = _Sessao };
        }

        public Retorno ID(Guid id)
        {
            var sessao = Db.Sessaos.SingleOrDefault(e => e.Id == id);

            if (sessao == null)
                return new Retorno() { Status = false, Resultado = "Sessão não existe" };

            return new Retorno() { Status = true, Resultado = sessao };
        }

        public Retorno Lista() => new Retorno() { Status = true, Resultado = Db.EleitoresPauta.OrderBy(e => e.Votou) };

        public Retorno AdicionaPauta(Guid id,Pauta pauta)
        {
            var Sessao = Db.Sessaos.SingleOrDefault(c => c.Id == id);
            if (Sessao == null)
                return new Retorno() { Status = false, Resultado = "Sessão não existe" };

            pauta = Db.Pautas.SingleOrDefault(c => c.Id == pauta.Id);
            if (pauta == null)
                return new Retorno() { Status = false, Resultado = "Pauta não existe" };

            Sessao.Pautas.Add(pauta);

            return new Retorno() { Status = true, Resultado = Sessao };
        }
        public Retorno DeletaVoto(Guid id)
        {
            _Sessao = Db.Sessaos.SingleOrDefault(c => c.Id == id);

            if (_Sessao == null)
                return new Retorno() { Status = false, Resultado = "Sessão não existe" };

            Db.Sessaos.Remove(_Sessao);
            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = _Sessao };
        }
    }
}
