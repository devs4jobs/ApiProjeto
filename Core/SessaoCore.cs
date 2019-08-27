using Core.util;
using FluentValidation;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Core
{
    public class SessaoCore : AbstractValidator<Sessao>
    {
        private Sessao _sessao { get; set; }
        public SessaoCore(Sessao sessao)
        {
            _sessao = sessao;

            RuleFor(e => e.dataTermino)
                .Length(8, 8)
                .NotNull()
                .WithMessage("Descrição inválida");
        }

        public SessaoCore() { }

        public Retorno CadastroSessao()
        {

            var results = Validate(_sessao);

            // Se o modelo é inválido, retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Pautas.Exists(x => x.Id == _sessao.Id))
            {
                return new Retorno() { Status = true, Resultado = "Pauta já cadastrada" };
            }

            db.sistema.todasSessoes.Add(_sessao);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _sessao };
        }


        public Retorno adicionarPauta(string idSessao, string idPauta)
        {

            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();

            var p = t.sistema.Pautas.Find(x => x.Id == new Guid(idPauta));
            if(t.sistema.todasSessoes.Exists(y=>y.pautasSessao.Exists(pauta=>pauta.Id == p.Id)))
                new Retorno() { Status = false, Resultado = "Pauta ja cadastrada em outra sessao" };
            var s = t.sistema.todasSessoes.Find(x => x.Id == new Guid(idSessao));
            s.pautasSessao.Add(p);
            var d = file.ManipulacaoDeArquivos(false, t.sistema);
            return new Retorno() { Status = true, Resultado = "Pauta adicionada com sucesso!" };

        }

        public Retorno adicionarEleitor(string idSessao, string idEleitor)
        {

            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();

            var p = t.sistema.Eleitores.Find(x => x.Id == new Guid(idEleitor));
            if(t.sistema.todasSessoes.Exists(x => x.eleitoresSessao.Exists(eleitor => eleitor.Id == p.Id)))
                new Retorno() { Status = false, Resultado = "Eleitor ja cadastrado em outra sessao" };
            var s = t.sistema.todasSessoes.Find(q => q.Id == new Guid(idSessao));
            s.eleitoresSessao.Add(p);
            var d = file.ManipulacaoDeArquivos(false, t.sistema);
            return new Retorno() { Status = true, Resultado = "Eleitor adicionado com sucesso!" };

        }

        public Retorno ExibirTodasSessoes()
        {
            var y = file.ManipulacaoDeArquivos(true, null);

            if (y.sistema == null)
                y.sistema = new Sistema();

            var q = y.sistema.todasSessoes;
            return new Retorno() { Status = true, Resultado = q };
        }

        public Retorno ExibirSessaoId(string id)
        {
            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();
            var p = t.sistema.todasSessoes.Where(x => x.Id == new Guid(id));
            return new Retorno() { Status = true, Resultado = p };

        }

        public Retorno DeletarSessaoId(string id)
        {
            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();

            var p = t.sistema.todasSessoes.Remove(t.sistema.todasSessoes.Find(s => s.Id == new Guid(id)));

            file.ManipulacaoDeArquivos(false, t.sistema);

            return new Retorno() { Status = true, Resultado = "Sessão Deletada!" };
        }

    }

}