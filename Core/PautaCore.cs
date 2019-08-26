using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;

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

           // Se o modelo é inválido, retorno false
           if (!results.IsValid)
               return new Retorno { Status = false, Resultado = results.Errors };

           // Caso o modelo seja válido, escreve no arquivo db
           var db = file.ManipulacaoDeArquivos(true, null);

           if (db.sistema == null)
               db.sistema = new Sistema();

           if (db.sistema.Pautas.Exists(x => x.Descricao == _pauta.Descricao))
           {
               return new Retorno() { Status = true, Resultado = "Pauta já cadastrada" };
           }

            db.sistema.Pautas.Add(_pauta);

           file.ManipulacaoDeArquivos(false, db.sistema);

           return new Retorno() { Status = true, Resultado = _pauta };
       }

        public Retorno ExibirPautaId(string id)
        {

            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();
            var p = t.sistema.Pautas.Where(x => x.Id == new Guid(id));
            return new Retorno() { Status = true, Resultado = p };

        }

        public Retorno ExibirTodasPautas()
        {
            var y = file.ManipulacaoDeArquivos(true, null);

            if (y.sistema == null)
                y.sistema = new Sistema();

            var q = y.sistema.Pautas;
            return new Retorno() { Status = true, Resultado = q };
        }

        public Retorno DeletarPautaId(string id)
        {
            var t = file.ManipulacaoDeArquivos(true, null);

            if (t.sistema == null)
                t.sistema = new Sistema();

            var p = t.sistema.Pautas.Remove(t.sistema.Pautas.Find(s => s.Id == new Guid(id)));

            file.ManipulacaoDeArquivos(false, t.sistema);

            return new Retorno() { Status = true, Resultado = "Pauta Deletada!" };
        }

        public Retorno AtualizarPautaId(Pauta nova, string id)
        {
            var f = file.ManipulacaoDeArquivos(true, null);

            if (f.sistema == null)
                f.sistema = new Sistema();

            var velha = f.sistema.Pautas.Find(s => s.Id == new Guid(id));
            var troca = TrocaPauta(nova, velha);

            f.sistema.Pautas.Add(troca);
            f.sistema.Pautas.Remove(velha);

            file.ManipulacaoDeArquivos(false, f.sistema);

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
