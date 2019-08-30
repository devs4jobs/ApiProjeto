using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Core
{
    public class PautaCore : AbstractValidator<Pauta>
    {
        private Pauta _pauta { get; set; }

        public PautaCore(Pauta pauta)
        {
            _pauta = pauta;

            RuleFor(e => e.Descricao)
                .Length(5, 35)
                .NotNull()
                .WithMessage("Descrição inválida");
        }

        public PautaCore() { }

        public Retorno CadastroPauta()
        {

            var results = Validate(_pauta);
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Pautas.Exists(x => x.Descricao == _pauta.Descricao))
            {
                return new Retorno() { Status = false, Resultado = "Pauta já cadastrada" };
            }
            db.sistema.Pautas.Add(_pauta);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno() { Status = true, Resultado = _pauta };
        }

        public Retorno ExibirPautaId(string id)
        {

            var arquivo = file.ManipulacaoDeArquivos(true, null);
            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();
            var resultado = arquivo.sistema.Pautas.Where(x => x.Id == new Guid(id));
            return new Retorno() { Status = true, Resultado = resultado };

        }

        public Retorno ExibirPautaDataCadastro(string dataCadastro)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();


            var resultado = arquivo.sistema.Pautas.Where(x => x.DataCadastro.ToString("ddMMyyyy").Equals(dataCadastro));
            return new Retorno() { Status = true, Resultado = resultado };
        }

        public Retorno ExibirTodasPautas(int page, int sizePage)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            Base classeBase = new Base();

            List<Pauta> thirdPage = classeBase.GetPage(arquivo.sistema.Pautas, page, sizePage);
            return new Retorno() { Status = true, Resultado = thirdPage };
        }

        public Retorno DeletarPautaId(string id)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var resultado = arquivo.sistema.Pautas.Remove(arquivo.sistema.Pautas.Find(s => s.Id == new Guid(id)));

            file.ManipulacaoDeArquivos(false, arquivo.sistema);

            return new Retorno() { Status = true, Resultado = "Pauta Deletada!" };
        }

        public Retorno AtualizarPautaId(Pauta nova, string id)
        {
            var arquivo = file.ManipulacaoDeArquivos(true, null);

            if (arquivo.sistema == null)
                arquivo.sistema = new Sistema();

            var velha = arquivo.sistema.Pautas.Find(s => s.Id == new Guid(id));
            var troca = TrocaPauta(nova, velha);

            arquivo.sistema.Pautas.Add(troca);
            arquivo.sistema.Pautas.Remove(velha);

            file.ManipulacaoDeArquivos(false, arquivo.sistema);

            return new Retorno() { Status = true, Resultado = $"{troca}\n\nPauta Atualizada!" };
        }

        public Pauta TrocaPauta(Pauta nova, Pauta velha)
        {
            if (velha.Descricao == null) nova.Descricao = velha.Descricao;
            velha.DataCadastro = nova.DataCadastro;
            velha.Encerrada = nova.Encerrada;
            nova.Id = velha.Id;
            return nova;
        }
    }
}
