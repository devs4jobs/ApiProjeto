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
        public VotoCore() { }

        public Retorno CadastroVoto()
        {
            //cadastrando voto
            try
            {
                var results = Validate(_voto);
                if (!results.IsValid)
                    return new Retorno { Status = false, Resultado = results.Errors };

                var db = file.ManipulacaoDeArquivos(true, null);

                var sessao = db.sistema.todasSessoes.Find(d => d.eleitoresSessao.Exists(x => x.Id == _voto.EleitorId));

                if (sessao.Encerrada == true)
                    return new Retorno { Status = false, Resultado = "Esta sessão já foi encerrada" };
                //vendo se o sistema esta nulo
                if (db.sistema == null)
                    db.sistema = new Sistema();

                if (sessao.votoSessao.Exists(x => x.PautaId == _voto.PautaId && x.EleitorId == _voto.EleitorId))               
                   return new Retorno() { Status = false, Resultado = "Este Eleitor já votou" };
                
                //após validações realizando o voto
                var pautaSendoVotada = sessao.pautasSessao.Find(u => u.Id == _voto.PautaId);

                if (pautaSendoVotada.Encerrada == true)
                    return new Retorno() { Status = false, Resultado = "Pauta já encerrada" };
                //addicionando o voto
                db.sistema.Votos.Add(_voto);
                sessao.votoSessao.Add(_voto);

                var votosDaPauta = sessao.votoSessao.Where(s => s.PautaId == pautaSendoVotada.Id);
                //conferindo se podemos encerrar a pauta que esta sendo votada
                pautaSendoVotada.Encerrada = sessao.eleitoresSessao.Count() == votosDaPauta.Count();

                var pautaVelha = db.sistema.Pautas.Find(w => w.Id == pautaSendoVotada.Id);
                //atualizando na lista de pautas o voto
                db.sistema.Pautas.Remove(pautaVelha);
                db.sistema.Pautas.Add(pautaSendoVotada);
                //percorrendo a lista e vendo se há pautas nao encerradas
                foreach (var item in sessao.pautasSessao)
                    if (item.Encerrada == false)
                        sessao.Encerrada = false;
                    else
                        sessao.Encerrada = true;

                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _voto };
            }
            catch
            {
                return new Retorno() { Status = true, Resultado = "Eleitor ou Pauta não pertencem à sessão" };
            }
        }

        public Retorno ExibirTodosVotos()
        {
            //exibindo votos 
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var votos = arquivo.sistema.Votos;
            return new Retorno() { Status = true, Resultado = votos };
        }

        public Retorno ExibirVotoId(string id)
        {
            //exibindo o voto por Id de pauta
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();
            var votos = arquivo.sistema.Votos.Where(x => x.PautaId == new Guid(id));
            return new Retorno() { Status = true, Resultado = votos };
        }
    }
}
