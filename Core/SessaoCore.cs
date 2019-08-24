using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System.Collections.Generic;

namespace Core
{
    public class SessaoCore : AbstractValidator<Sessao>
    {
        private List<Sessao> _sessao { get; set; } = new List<Sessao>();
        public SessaoCore(List<Sessao> sessao)
        {
            _sessao = sessao;

            RuleFor(e => e.lstPautas)
                .NotEqual(true)
                .WithMessage("Não pode cadastrar uma sessão finalizada!");

            RuleFor(e => e.lstPautas.All(e => e.Id.ToString()))
                .NotNull()
                .WithMessage("Insira pelo menos os ID's das Pautas.");
        }
        public SessaoCore() { }

        public Retorno CadastroSessao() {

            var results = Validate(_sessao);
            // Se o modelo é inválido retorno false
            if(!results.IsValid) return new Retorno { Status = false, Resultado = results.Errors.Select(m => m.ErrorMessage).ToList() };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true,null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (!db.sistema.Sessao.Exists(e => e.Id.Equals(_sessao.Id)))
            {
                db.sistema.Sessao.Add(_sessao);
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _sessao };
            }else
                return new Retorno() { Status = false, Resultado = "Já existe uma seesão com esse ID." };
            }

        public Retorno ProcurarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return (db.sistema.Sessao.Any(e => e.Id.ToString().Equals(id))) ? new Retorno() { Status = true, Resultado = db.sistema.Sessao.SingleOrDefault(e => e.Id.ToString().Equals(id))} : new Retorno() { Status = false, Resultado = "Não existe uma sessão com esse ID." };
        }

        public Retorno ProcurarTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return (db.sistema.Sessao.Any())? new Retorno() { Status = true, Resultado = db.sistema.Sessao } : new Retorno() { Status = false, Resultado = "Não existe nenhum elemento." };
        }

        public Retorno DeletarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.Sessao.Exists(e => e.Id.ToString().Equals(id)))
            { 
                db.sistema.Sessao.Remove(db.sistema.Sessao.Single(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);

                return new Retorno() { Status = true, Resultado = "sessão Deletado com sucesso." };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma sessão com esse ID ,nada foi deletado." };
        }

        public Retorno AtualizarPorID(string id,Sessao eleitor)
        { 
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.Sessao.Exists(e => e.Id.ToString().Equals(id)))
            {
                var elementoAtualizado = TrocarDadosDeSessao(eleitor, db.sistema.Sessao.SingleOrDefault(e => e.Id.ToString().Equals(id)));
                db.sistema.Sessao.Add(elementoAtualizado);
                db.sistema.Sessao.Remove(db.sistema.Sessao.FirstOrDefault(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = elementoAtualizado};
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe um eleitor com esse ID, Nenhum eleitor foi atualizado." };

        }

        protected Sessao TrocarDadosDeSessao(Sessao sessao, Sessao Objeto)
        { 
            if (sessao.lstPautaEleitor.Count >= 0) sessao.lstPautaEleitor = Objeto.lstPautaEleitor;

            if (sessao.lstPautas.Count >= 0) sessao.lstPautas = Objeto.lstPautas;


            sessao.DataCadastro = Objeto.DataCadastro;
            sessao.Id = Objeto.Id;            
       
            return sessao;
        }
    }  
}

