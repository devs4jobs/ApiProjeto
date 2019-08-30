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
                return new Retorno() { Status = false, Resultado = "Sessão já cadastrada" };
            }
            db.sistema.todasSessoes.Add(_sessao);
            file.ManipulacaoDeArquivos(false, db.sistema);
            return new Retorno() { Status = true, Resultado = _sessao };
        }


        public Retorno adicionarPautaEleitor(AdicionaPtEl addSessao)
        {
            //validar se ssesao est aberta (bool), aplicar na logica antes de adicionar o Eleitor
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var sessao = arquivo.sistema.todasSessoes.Find(q => q.Id == new Guid(addSessao.idSessao));

            if (addSessao.eleitoresId.Count == 0 && addSessao.pautasId.Count == 0)
                new Retorno() { Status = false, Resultado = "Nada está sendo adicionado!" };
            //validando o que esta sendo passado para a sessao em quantidade
            if (addSessao.eleitoresId.Count > 0)          
               foreach (var eleitorId in addSessao.eleitoresId)
                   sessao.eleitoresSessao.Add(eleitorId);
            
            if (addSessao.pautasId.Count > 0)          
              foreach (var pautasId in addSessao.pautasId)
                 sessao.pautasSessao.Add(pautasId);
           
            var salva = file.ManipulacaoDeArquivos(false, arquivo.sistema);
            return new Retorno() { Status = true, Resultado = "Iten(s) adicionados com sucesso!" };

        }

        public Retorno ExibirTodasSessoes(int page, int sizePage)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);
            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            Base classeBase = new Base();

            List<Sessao> thirdPage = classeBase.GetPage(arquivo.sistema.todasSessoes, page, sizePage);
            return new Retorno() { Status = true, Resultado = thirdPage };
        }

        public Retorno ExibirSessaoId(string id)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);
            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();
            var sessaoFiltrada = arquivo.sistema.todasSessoes.Where(x => x.Id == new Guid(id));
            return new Retorno() { Status = true, Resultado = sessaoFiltrada };
        }

        public Retorno ExibirSessaoDataCadastro(string dataCadastro)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var resultado = arquivo.sistema.todasSessoes.Where(x => x.DataCadastro.ToString("ddMMyyyy").Equals(dataCadastro));
            return new Retorno() { Status = true, Resultado = resultado };
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