using Model;
using FluentValidation;
using Core.util;
using System.Linq;

namespace Core
{
    public class PautaEleitorCore : AbstractValidator<PautaEleitor>
    {
        private PautaEleitor _pautaEleitor { get; set; }
        public PautaEleitorCore(PautaEleitor pautaeleitor)
        {
            _pautaEleitor = pautaeleitor;

            RuleFor(e => e.EleitorId)
                .NotNull()
                .WithMessage("o Id do Eleitor não pode ser nulo.");

            RuleFor(e => e.PautaId)
                .NotNull()
                .WithMessage("o id da pauta não pode ser nulo.");
        }
        public PautaEleitorCore() { }

        public Retorno CadastrarPautaEleitor()
        {

            var results = Validate(_pautaEleitor);

            // Se o modelo é inválido retorno false
            if (!results.IsValid) return new Retorno { Status = false, Resultado = results.Errors.Select(m => m.ErrorMessage).ToList() };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();
            if (!db.sistema.Pautas.Exists(p => p.Id.Equals(_pautaEleitor.PautaId)) && !db.sistema.Eleitores.Exists(e => e.Id.Equals(_pautaEleitor)))
                return new Retorno() { Status = false, Resultado = "Não exite uma Pauta/Eleitor com esses ID's na Base de Dados." };

            if (!db.sistema.EleitoresPauta.Exists(e => e.PautaId.Equals(_pautaEleitor.PautaId)))
            {
                db.sistema.EleitoresPauta.Add(_pautaEleitor);
                file.ManipulacaoDeArquivos(false, db.sistema);

                return new Retorno() { Status = true, Resultado = _pautaEleitor };
            }
            else
                return new Retorno() { Status = false, Resultado = "Já existe uma pauta eleitor com esse ID." };

        }

        public Retorno ProcurarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return db.sistema.EleitoresPauta.Any(e => e.PautaId.ToString().Equals(id)) ? new Retorno() { Status = true, Resultado = db.sistema.EleitoresPauta.SingleOrDefault(e => e.PautaId.ToString().Equals(id)) } : new Retorno() { Status = false, Resultado = "Não existe uma PautaEleitor com esse ID." };
        }

        public Retorno ProcurarTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return db.sistema.EleitoresPauta.Any() ? new Retorno() { Status = true, Resultado = db.sistema.EleitoresPauta } : new Retorno() { Status = false, Resultado = "Não existe nenhum elemento." };

        }

        public Retorno DeletarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.EleitoresPauta.Exists(e => e.PautaId.ToString().Equals(id)))
            {
                db.sistema.EleitoresPauta.Remove(db.sistema.EleitoresPauta.Single(e => e.PautaId.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);

                return new Retorno() { Status = true, Resultado = "PautaEleitor Deletada com sucesso." };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma PautaEleitor com esse ID ,nada foi deletado." };

        }

        public Retorno AtualizarPorID(string id, PautaEleitor pautaeleitor)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();


            if (db.sistema.EleitoresPauta.Exists(e => e.PautaId.ToString().Equals(id)))
            {
                var elementoAtualizado = TrocaDadosPautas(pautaeleitor, db.sistema.EleitoresPauta.SingleOrDefault(e => e.PautaId.ToString().Equals(id)));
                db.sistema.EleitoresPauta.Add(elementoAtualizado);
                db.sistema.EleitoresPauta.Remove(db.sistema.EleitoresPauta.FirstOrDefault(e => e.PautaId.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = elementoAtualizado };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma PautaEleitor com esse ID, Nenhuma Pauta foi atualizado." };

        }

        protected PautaEleitor TrocaDadosPautas(PautaEleitor inserida, PautaEleitor Existente)
        {

            if (inserida.Voto == null) inserida.Voto = Existente.Voto;

            if (inserida.Votou == false) inserida.Votou = Existente.Votou;

            inserida.PautaId = Existente.PautaId;

            inserida.EleitorId = Existente.EleitorId;

            return inserida;
        }

    }

}

