using Model;
using FluentValidation;
using Core.util;
using System.Linq;

namespace Core
{
    public class PautaCore : AbstractValidator<Pauta>
    {
        private Pauta _pauta{ get; set; }
        public PautaCore( Pauta pauta)
        {
            _pauta = pauta;

            RuleFor(e => e.Descricao)
                .NotNull()
                .WithMessage("Descrição/Causa não pode ser nula.");

            RuleFor(e => e.Concluida)
                .NotEqual(true)
                .WithMessage("A pauta não pode ter sido concluida.");
        }
        public PautaCore() { }

        public Retorno CadastrarPauta() {

            var results = Validate(_pauta);

            // Se o modelo é inválido retorno false
            if(!results.IsValid) return new Retorno { Status = false, Resultado = results.Errors.Select(m => m.ErrorMessage).ToList() };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true,null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (!db.sistema.Pautas.Exists(e => e.Descricao.Equals(_pauta.Descricao)))
            {
                db.sistema.Pautas.Add(_pauta);
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _pauta };
            }else
                return new Retorno() { Status = false, Resultado = "Já existe um eleitor com esse documento." };

            }

        public Retorno ProcurarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Pautas.Any(e => e.Id.ToString().Equals(id)))
            {
                return new Retorno() { Status = true, Resultado = db.sistema.Pautas.SingleOrDefault(e => e.Id.ToString().Equals(id)) };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma Pauta com esse ID." };

        }

        public Retorno ProcurarTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return db.sistema.Pautas.Any() ? new Retorno() { Status = true, Resultado = db.sistema.Pautas } : new Retorno() { Status = false, Resultado = "Não existe nenhum elemento." };

        }

        public Retorno DeletarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Pautas.Exists(e => e.Id.ToString().Equals(id)))
            { 
                db.sistema.Pautas.Remove(db.sistema.Pautas.Single(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);

                return new Retorno() { Status = true, Resultado = "Pauta Deletada com sucesso." };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma pauta com esse ID ,nada foi deletado." };

        }

        public Retorno AtualizarPorID(string id, Pauta pauta)
        { 
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Eleitores.Exists(e => e.Id.ToString().Equals(id)))
            {
                var elementoAtualizado = TrocaDadosPautas(pauta, db.sistema.Pautas.SingleOrDefault(e => e.Id.ToString().Equals(id)));
                db.sistema.Pautas.Add(elementoAtualizado);
                db.sistema.Pautas.Remove(db.sistema.Pautas.FirstOrDefault(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = elementoAtualizado};
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma Pauta com esse ID, Nenhum eleitor foi atualizado." };

        }

        protected Pauta TrocaDadosPautas(Pauta inserida, Pauta Existente)
        { 
            if (inserida.Concluida == false) inserida.Concluida = Existente.Concluida;

            if (inserida.Descricao == null) inserida.Descricao = Existente.Descricao;

            inserida.DataCadastro = Existente.DataCadastro;
            inserida.Id = Existente.Id;            
       
            return inserida;
        }

    }
    
}

