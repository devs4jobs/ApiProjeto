using Core.util;
using FluentValidation;
using Model;
using System;
using System.Linq;

namespace Core
{
    public class PautaEleitorCore: AbstractValidator<PautaEleitor>
    {
        private PautaEleitor _PautaEleitor { get; set; }
        private Sistema Db { get; set; }
        public PautaEleitorCore()
        {

            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();
        }
        public PautaEleitorCore(PautaEleitor pautaEleitor)
        {
            Db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (Db == null)
                Db = new Sistema();

            _PautaEleitor = pautaEleitor;

            _PautaEleitor.Votou = true;

            RuleFor(e => e.EleitorId)
                .NotNull()
                .WithMessage("EleitorId não pode ser nulo");

            RuleFor(e => e.PautaId)
                .NotNull()
                .WithMessage("PautaId não pode ser nulo");

            RuleFor(e => e.Voto.ToLower())
                .NotNull()
                .Must(e => e == "a favor" || e == "contra")
                .WithMessage("Voto Invalido");

        }

        public Retorno Votacao()
        {

            var results = Validate(_PautaEleitor);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors.Select(c=>c.ErrorMessage) };

            //Procura se a sessão ja foi iniciada
            var sessao = Db.Sessaos.SingleOrDefault(c => c.Pautas.SingleOrDefault(d => d.Id == _PautaEleitor.PautaId).Id==_PautaEleitor.PautaId);

            //Caso não exista a pauta ou ja tenha finalizada
            if (sessao == null||!sessao.Status)
                return new Retorno() { Status = false, Resultado = "Sessão não iniciada ou Sessão ja finalizada" };

            //Caso não exista o eleitor
            if (sessao.Eleitores.SingleOrDefault(c => c.Id == _PautaEleitor.EleitorId) == null)
                return new Retorno() { Status = false, Resultado = "Eleitor não cadastrada para essa sessão" };

            //Caso ja tenha votado
            if (Db.EleitoresPauta.SingleOrDefault(c => c.PautaId == _PautaEleitor.PautaId&&c.EleitorId==_PautaEleitor.EleitorId) != null)
                return new Retorno() { Status = false, Resultado = "Voto ja registrado" };

            Db.EleitoresPauta.Add(_PautaEleitor);
            //Verifica se a Pauta deve ter votação encerrada
            if (sessao.Eleitores.Count == Db.EleitoresPauta.Where(c => c.PautaId == _PautaEleitor.PautaId).ToList().Count)
            {
                //Alterando a pauta na arquivo na parte pautas
                var Pauta = Db.Pautas.SingleOrDefault(c => c.Id == _PautaEleitor.PautaId);
                Pauta.Concluida = true;
                //Alterando a pauta no arquivo na parte da sessão
                Pauta=sessao.Pautas.SingleOrDefault(c => c.Id == _PautaEleitor.PautaId);
                Pauta.Concluida = true;
                //Verifica se a Sessão ja deve ser fechada
                if (sessao.PautaTrue(sessao.Pautas))
                    sessao.Status = false;
            }
            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = _PautaEleitor };
        }
        public Retorno ID(Guid id)
        {
            var Votos = Db.EleitoresPauta.Where(e => e.PautaId == id).ToList();
            if (Votos == null)
                return new Retorno() { Status = false, Resultado = "Pauta não existe" };
            return new Retorno() { Status = true, Resultado = Votos };
        }
        public Retorno Lista() => new Retorno() { Status = true, Resultado = Db.EleitoresPauta.OrderBy(e=>e.Votou) };

        public Retorno AtualizaVoto()
        {
            var Voto = Db.EleitoresPauta.SingleOrDefault(c => c.EleitorId == _PautaEleitor.EleitorId && c.PautaId==_PautaEleitor.PautaId);
            if (Voto == null)
                return new Retorno { Status = false, Resultado = "Voto não existe" };

            if (_PautaEleitor.Voto != null) Voto.Voto = _PautaEleitor.Voto;

            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = Voto };
        }
        public Retorno DeletaVoto(Guid id)
        {
            _PautaEleitor = Db.EleitoresPauta.SingleOrDefault(c => c.PautaId == id);

            if (_PautaEleitor == null)
                return new Retorno() { Status = false, Resultado = "Voto não existe" };

            Db.EleitoresPauta.Remove(_PautaEleitor);
            file.ManipulacaoDeArquivos(false, Db);

            return new Retorno() { Status = true, Resultado = _PautaEleitor };
        }
    }
}

