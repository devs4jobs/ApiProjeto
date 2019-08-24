using Model;
using FluentValidation;
using Core.util;
using System.Linq;

namespace Core
{
    public class PautaEleitorCore : AbstractValidator<PautaEleitor>
    {
        private PautaEleitor _pautaeleitor;
        public Sistema db { get; set; }
        public PautaEleitorCore()
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        }
        public PautaEleitorCore(PautaEleitor pautaeleitor)

        {
            _pautaeleitor = pautaeleitor;
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

            RuleFor(e => e.EleitorId).NotEmpty().WithMessage("O eleitor Id nao pode ser vazio");
            RuleFor(e => e.PautaId).NotEmpty().WithMessage("a pauta id nao pode ser vazia");
        }

        // Método para cadastro.
        public Retorno Cadastro()
        {

            var results = Validate(_pautaeleitor);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors };
            
            
            db.EleitoresPauta.Add(_pautaeleitor);
            file.ManipulacaoDeArquivos(false, db);


            return new Retorno() { Status = true, Resultado = _pautaeleitor };
        }

        // Método para buscar uma pauta
        public Retorno AcharUm(string id)
        {
            if (!db.EleitoresPauta.Any(p => p.PautaId.ToString() == id)) 
                return new Retorno() { Status = false, Resultado = null };

            var UmaPautaEleitor = db.EleitoresPauta.Find(c => c.PautaId.ToString() == id);
            return new Retorno() { Status = true, Resultado = UmaPautaEleitor };
        }

        // Método para retornar todas as pautas
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.EleitoresPauta.OrderBy(c => c.PautaId) };
        

        // Método para deletar por id
        public Retorno DeletarId(string id)
        {
           
           var umaPautaEleitor = db.EleitoresPauta.Find(c => c.PautaId.ToString() == id);

            db.EleitoresPauta.Remove(umaPautaEleitor);

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = null };

        }

        // Método para atualizar a pauta por id
        public Retorno AtualizarUm(string id, PautaEleitor pautaeleitor)
        {
            var umaPautaEleitor = db.EleitoresPauta.Find(c => c.PautaId.ToString() == id);
        
            if (pautaeleitor.Voto != null)
                umaPautaEleitor.Voto = pautaeleitor.Voto;

            if (pautaeleitor.Votou != false)
                umaPautaEleitor.Votou = pautaeleitor.Votou;

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = umaPautaEleitor };
        }
    }
}
