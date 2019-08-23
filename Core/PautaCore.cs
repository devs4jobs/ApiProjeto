using Model;
using FluentValidation;
using Core.util;

namespace Core
{
    public class PautaCore : AbstractValidator<Pauta>
    {
        private Pauta _pauta;
        public PautaCore()
        {

        }
        public PautaCore(Pauta Pauta)
        {
            _pauta = Pauta;

            RuleFor(e => e.Descricao).
                MinimumLength(6)
                .NotNull()
                .WithMessage("Tamanho da descrição invalida");
        }

        // Método para cadastro.
        public Retorno CadastroEleitor()
        {

            var results = Validate(_pauta);


            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            var pautas = db.sistema.Pautas;

            if (pautas.Exists(c => c.Descricao == _pauta.Descricao))
                return new Retorno() { Status = false, Resultado = null };


            db.sistema.Pautas.Add(_pauta);
            file.ManipulacaoDeArquivos(false, db.sistema);


            return new Retorno() { Status = true, Resultado = _pauta };
        }

        // Método para buscar uma pauta
        public Retorno AcharUm(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            if (!db.sistema.Eleitores.Exists(e => e.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = null };



            var UmaPauta = db.sistema.Pautas.Find(c => c.Id.ToString() == id);
            return new Retorno() { Status = true, Resultado = UmaPauta };
        }

        // Método para retornar todas as pautas
        public Retorno AcharTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();


            return new Retorno() { Status = true, Resultado = db.sistema.Pautas };
        }

        // Método para deletar por id
        public Retorno DeletarId(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            var umaPauta = db.sistema.Pautas.Find(c => c.Id.ToString() == id);

            db.sistema.Pautas.Remove(umaPauta);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno { Status = true, Resultado = null };

        }

        // Método para atualizar a pauta por id
        public Retorno AtualizarUm(string id, Pauta pauta)
        {

            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            var umaPauta = db.sistema.Pautas.Find(c => c.Id.ToString() == id);
            db.sistema.Pautas.Remove(umaPauta);


            if (pauta.Descricao != null)
                umaPauta.Descricao = pauta.Descricao;

            if (pauta.Id != null)
                umaPauta.Id = pauta.Id;

            if (pauta.Concluida == true)
                umaPauta.Concluida = pauta.Concluida;


            db.sistema.Pautas.Add(umaPauta);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno { Status = true, Resultado = umaPauta };



        }
    }
}
