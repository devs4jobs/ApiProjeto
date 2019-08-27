using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;

namespace Core
{
    public class EleitorCore : AbstractValidator<Eleitor>
    {
        //getters setters privados
        private Eleitor _eleitor { get; set; }
        public EleitorCore(Eleitor eleitor)
        {
            //regras para os atributos de eleitor
            _eleitor = eleitor;

            RuleFor(e => e.Documento)
                .Length(11, 11)
                .NotNull()
                .WithMessage("Cpf inválido");

            RuleFor(e => e.Nome)
                .MinimumLength(3)
                .NotNull()
                .WithMessage("O nome deve ser preenchido e deve ter o mínimo de 3 caracteres");
        }
        //construtor
        public EleitorCore(){ }

        //método de criação de Eleitor 
        public Retorno CadastroEleitor() {

            var results = Validate(_eleitor);

            // Se o modelo é inválido, retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();
            
            if (db.sistema.Eleitores.Exists(x => x.Documento == _eleitor.Documento)) {

                return new Retorno() { Status = true, Resultado = "CPF já cadastrado" };
            }
            db.sistema.Eleitores.Add(_eleitor);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _eleitor };
        }

        public Retorno ExibirEleitorId(string id)
        {

            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();

            var p = t.sistema.Eleitores.Where(x => x.Id == new Guid(id));
            return new Retorno() { Status = true, Resultado = p };

        }

        public Retorno ExibirTodos()
        {
            var y = file.ManipulacaoDeArquivos(true, null);

            if (y.sistema == null)
                y.sistema = new Sistema();

            var q = y.sistema.Eleitores;
            return new Retorno() { Status = true, Resultado = q };
        }

        public Retorno DeletarEleitorId(string id)
        {
            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();

            var p = t.sistema.Eleitores.Remove(t.sistema.Eleitores.Find(s=> s.Id == new Guid(id)));

            file.ManipulacaoDeArquivos(false, t.sistema);

            return new Retorno() { Status = true, Resultado = null };
        }

        public Retorno AtualizarId(Eleitor novo, string id)
        {
            var f = file.ManipulacaoDeArquivos(true, null);

            if(f.sistema == null)
                f.sistema = new Sistema();

            var velho = f.sistema.Eleitores.Find(s => s.Id == new Guid(id));
            var troca = TrocaDados(novo, velho);

            f.sistema.Eleitores.Add(troca);
            f.sistema.Eleitores.Remove(velho);
            
            file.ManipulacaoDeArquivos(false, f.sistema);

            return new Retorno() { Status = true, Resultado = troca};
        }

        public Eleitor TrocaDados(Eleitor novo, Eleitor velho)
        {
            if (velho.Nome == null) novo.Nome = velho.Nome;
            if (velho.Documento == null) novo.Documento = velho.Documento;
            if (velho.Sexo == null) novo.Sexo = velho.Sexo; 
            if (velho.Idade == 0) novo.Idade = velho.Idade;
            velho.DataCadastro = novo.DataCadastro;
            novo.Id = velho.Id;
            return novo;
        }

        public Retorno buscarVotoEleitor(string idEleitor)
        {
            var y = file.ManipulacaoDeArquivos(true, null);

            if (y.sistema == null)
                y.sistema = new Sistema();

            var q = y.sistema.Eleitores.Find(x=>x.Id== new Guid(idEleitor));
            var sessao = y.sistema.todasSessoes.Find(d=>d.eleitoresSessao.Exists(x=>x.Id==q.Id));
            var votos = sessao.urnasSessao.Where(d => d.EleitorId==q.Id).ToList();
            var listPauta = sessao.pautasSessao.Where(p=>votos.Exists(v=>v.PautaId==p.Id));

            return new Retorno() { Status = true, Resultado = q };
        }
    }
}
