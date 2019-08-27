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

        public Retorno CadastroUrna()
        {

            var results = Validate(_voto);

            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };


            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Urnas.Exists(x => x.PautaId == _voto.PautaId && x.EleitorId == _voto.EleitorId))
            {
                return new Retorno() { Status = true, Resultado = "já cadastrado" };
            }
            //validacoes , bool encerrada e se pertence a sessao
            db.sistema.Urnas.Add(_voto);
            //verificar, se nessa sessao q tem essa pauta contem algum eleitor que ainda nao votou
            //caso tenha encerrar sessao atraves de um true no bool 
            // e agora tem algum eleitor que nao votou?

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _voto };
        }

        //public Retorno ExibirUrnaId(string id)
        //{

        //    var t = file.ManipulacaoDeArquivos(true, null);

        //    if (t.sistema == null)
        //        t.sistema = new Sistema();

        //    var p = t.sistema.Urnas.Where(x => x.PautaId == new Guid(id));
        //    return new Retorno() { Status = true, Resultado = p };

        //}
        public Retorno ExibirTodasUrnas()
        {
            var y = file.ManipulacaoDeArquivos(true, null);

            if (y.sistema == null)
                y.sistema = new Sistema();

            var q = y.sistema.Urnas;
            return new Retorno() { Status = true, Resultado = q };
        }

        //public Retorno DeletarUrnaId(string id)
        //{
        //    var t = file.ManipulacaoDeArquivos(true, null);

        //    if (t.sistema == null)
        //        t.sistema = new Sistema();

        //    var p = t.sistema.Urnas.Remove(t.sistema.Urnas.Find(s => s.PautaId == new Guid(id)));

        //    file.ManipulacaoDeArquivos(false, t.sistema);

        //    return new Retorno() { Status = true, Resultado = null };
        //}

        //public Retorno AtualizarUrnaId(Voto novo, string id)
        //{
        //    var f = file.ManipulacaoDeArquivos(true, null);

        //    if (f.sistema == null)
        //        f.sistema = new Sistema();

        //    var velho = f.sistema.Urnas.Find(s => s.PautaId == new Guid(id));
        //    var troca = TrocaDados(novo, velho);

        //    f.sistema.Urnas.Add(troca);
        //    f.sistema.Urnas.Remove(velho);

        //    file.ManipulacaoDeArquivos(false, f.sistema);

        //    return new Retorno() { Status = true, Resultado = troca };
        //}

        //public Voto TrocaDados(Voto novo, Voto velho)
        //{
        //    novo.VotoAFavor = velho.VotoAFavor;
        //    novo.PautaId = velho.PautaId;
        //    novo.EleitorId = velho.EleitorId;
        //    velho.Votada = novo.Votada;
        //    return novo;
        //}
    }
}
