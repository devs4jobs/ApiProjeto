using Model;
using FluentValidation;
using Core.util;
using System.Linq;

namespace Core
{
    public class PautaCore : AbstractValidator<Pauta>
    {
        private Pauta _pauta;
        public Sistema db { get; set; }
        public PautaCore()
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        }
        public PautaCore(Pauta Pauta)
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

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
                return new Retorno { Status = false, Resultado = results.Errors.Select(c => c.ErrorMessage).ToList() };


            if (db.Pautas.Exists(c => c.Descricao == _pauta.Descricao))
                return new Retorno() { Status = false, Resultado = null };


            db.Pautas.Add(_pauta);
            file.ManipulacaoDeArquivos(false, db);


            return new Retorno() { Status = true, Resultado = _pauta };
        }

        // Método para buscar uma pauta
        public Retorno AcharUm(string id)
        {
      
            if (!db.Eleitores.Exists(e => e.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = null };



            var UmaPauta = db.Pautas.Find(c => c.Id.ToString() == id);
            return new Retorno() { Status = true, Resultado = UmaPauta };
        }

        // Método para retornar todas as pautas
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Pautas };
     
     

        // Método para deletar por id
        public Retorno DeletarId(string id)
        {
       
            var umaPauta = db.Pautas.Find(c => c.Id.ToString() == id);

            db.Pautas.Remove(umaPauta);

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = null };

        }

        // Método para atualizar a pauta por id
        public Retorno AtualizarUm(string id, Pauta pauta)
        {
            if (!db.Eleitores.Exists(e => e.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = null };

            var umaPauta = db.Pautas.Find(c => c.Id.ToString() == id);
     

            if (pauta.Descricao != null)
                umaPauta.Descricao = pauta.Descricao;

            if (pauta.Id != null)
                umaPauta.Id = pauta.Id;

            if (pauta.Concluida == true)
                umaPauta.Concluida = pauta.Concluida;


            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = umaPauta };
        }
    }
}
