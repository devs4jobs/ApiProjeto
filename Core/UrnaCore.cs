using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;

namespace Core
{
    public class UrnaCore : AbstractValidator<Urna>
    {
        //getters setters privados
        private Urna _urna { get; set; }
        public UrnaCore(Urna urna)
        {
            
            _urna = urna;
            RuleFor(e => e.Id)
                .NotNull()
                .WithMessage("Pauta Id inválido");

            RuleFor(e => e.EleitorId)
                .NotNull()
                .WithMessage("Eleitor Id inválido");
        }
        //construtor vazio
        public UrnaCore() {}

        public Retorno CadastroUrna()
        {

            var results = Validate(_urna);
    
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.PautaEleitores.Exists(x => x.Id == _urna.Id))
            {
                return new Retorno() { Status = true, Resultado = "já cadastrado" };
            }
            db.sistema.PautaEleitores.Add(_urna);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _urna };
        }    

        public Retorno ExibirUrnaId(string id)
        {

            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();

            var p = t.sistema.PautaEleitores.Where(x => x.Id == new Guid(id));
            return new Retorno() { Status = true, Resultado = p };

        }
        public Retorno ExibirTodasUrnas()
        {
            var y = file.ManipulacaoDeArquivos(true, null);

            if (y.sistema == null)
                y.sistema = new Sistema();

            var q = y.sistema.PautaEleitores;
            return new Retorno() { Status = true, Resultado = q };
        }

        public Retorno DeletarUrnaId(string id)
        {
            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();

            var p = t.sistema.PautaEleitores.Remove(t.sistema.PautaEleitores.Find(s => s.Id == new Guid(id)));

            file.ManipulacaoDeArquivos(false, t.sistema);

            return new Retorno() { Status = true, Resultado = null };
        }

        public Retorno AtualizarUrnaId(Urna novo, string id)
        {
            var f = file.ManipulacaoDeArquivos(true, null);

            if (f.sistema == null)
                f.sistema = new Sistema();

            var velho = f.sistema.PautaEleitores.Find(s => s.Id == new Guid(id));
            var troca = TrocaDados(novo, velho);

            f.sistema.PautaEleitores.Add(troca);
            f.sistema.PautaEleitores.Remove(velho);

            file.ManipulacaoDeArquivos(false, f.sistema);

            return new Retorno() { Status = true, Resultado = troca };
        }

        public Urna TrocaDados(Urna novo, Urna velho)
        {
            novo.VotoAFavor = velho.VotoAFavor;
            novo.Id = velho.Id;
            novo.EleitorId = velho.EleitorId;
            velho.Votada = novo.Votada;
            return novo;
        }
    }
}
