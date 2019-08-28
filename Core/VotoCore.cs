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
            //validar se ssesao est aberta (bool), aplicar na logica antes de adicionar o voto
            try
            {
                var results = Validate(_voto);
                if (!results.IsValid)
                    return new Retorno { Status = false, Resultado = results.Errors };

                var db = file.ManipulacaoDeArquivos(true, null);

                var sessao = db.sistema.todasSessoes.Find(d => d.eleitoresSessao.Exists(x => x.Id == _voto.EleitorId));

                if (db.sistema == null)
                    db.sistema = new Sistema();

                if (sessao.votoSessao.Exists(x => x.PautaId == _voto.PautaId && x.EleitorId == _voto.EleitorId))
                {
                    return new Retorno() { Status = true, Resultado = "Já votado" };
                }

                var pautaSendoVotada = sessao.pautasSessao.Find(u => u.Id == _voto.PautaId);

                if (pautaSendoVotada.Encerrada == true)
                    return new Retorno() { Status = true, Resultado = "Pauta já encerrada" };

                db.sistema.Votos.Add(_voto);
                sessao.votoSessao.Add(_voto);
                var votosDaPauta = sessao.votoSessao.Where(s => s.PautaId == pautaSendoVotada.Id);

                pautaSendoVotada.Encerrada = !(sessao.eleitoresSessao.Count() > votosDaPauta.Count());

                sessao.Encerrada = true;

                foreach (var item in sessao.pautasSessao)
                    if (!item.Encerrada)
                        sessao.Encerrada = false;

                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _voto };
            }
            catch
            {
                return new Retorno() { Status = true, Resultado = "Eleitor não pertence à sessão" };
            }
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
