using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;

namespace Core
{

    public class VotoCore : AbstractValidator<Voto>
    {
       
        //getters setters privados
        private Voto _voto { get; set; }
        public VotoCore(Voto voto)
        {
            _voto = voto;
            RuleFor(e => e.PautaId)
                .NotNull()
                .WithMessage("Pauta Id inválido");

            RuleFor(e => e.EleitorId)
                .NotNull()
                .WithMessage("Eleitor Id inválido");
        }
        //construtor vazio
        public VotoCore(){}

        public Retorno CadastroVoto()
        {
            Sessao sessao = new Sessao();

            var results = Validate(_voto);
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Votos.Exists(x => x.PautaId == _voto.PautaId && x.EleitorId == _voto.EleitorId))
            {
                return new Retorno() { Status = true, Resultado = "já cadastrado" };
            }

            var pautaSendoVotada = sessao.pautasSessao.Find(u => u.Id == _voto.PautaId);

            if (pautaSendoVotada.Encerrada == true)
                return new Retorno() { Status = true, Resultado = "Pauta já encerrada" };

            db.sistema.Votos.Add(_voto);

            if (pautaSendoVotada.Encerrada == true)
                return new Retorno() { Status = true, Resultado = "Pauta Encerrou agora" };

            if (!sessao.pautasSessao.Exists(e => e.Encerrada == true))
                sessao.Aberta = false;

            file.ManipulacaoDeArquivos(false, db.sistema);
            return new Retorno() { Status = true, Resultado = _voto };
        }
       
        public Retorno ExibirTodosVotos()
        {
            var y = file.ManipulacaoDeArquivos(true, null);

            if (y.sistema == null)
                y.sistema = new Sistema();

            var q = y.sistema.Votos;
            return new Retorno() { Status = true, Resultado = q };
        }

        public Retorno ExibirVotoId(string id)
        {
            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();
            var p = t.sistema.Votos.Where(x => x.PautaId == new Guid(id));
            return new Retorno() { Status = true, Resultado = p };
        }
    }
}
//validacoes: se a pauta ja nao esta encerrada
//e se pertence aquela sessao
//verificar se nessa sessao q tem essa pauta contem algum eleitor que ainda nao votou
//caso nao tenha eleitores que ainda nao votaram encerrar pauta atraves de um true no booleano 
//perguntar se agora, após o voto feito existe algum eleitor que nao votou?
//se nao tiver encerrar pauta