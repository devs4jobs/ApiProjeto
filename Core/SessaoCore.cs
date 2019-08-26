using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;

namespace Core
{
    public class SessaoCore : AbstractValidator<Sessao>
    {
        private Sessao _sessao;
        public Sistema db { get; set; }
        public SessaoCore()
        {
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();
        }
        public SessaoCore(Sessao umasessao)

        {
            _sessao = umasessao;
            db = file.ManipulacaoDeArquivos(true, null).sistema;

            if (db == null) db = new Sistema();

         //   RuleFor(a => a.LstPautaEleitores).Empty().WithMessage("A lista de votos precisa estar vazia");
            RuleFor(a => a.LstPautas).NotEmpty().WithMessage("Lista de Pautas nao pode ser vazia");
            RuleFor(a => a.Status).NotEqual(true).WithMessage("Não é possivel cadastrar uma sessao ja finalizada.");
          
        }
        // Método para cadastro.
        public Retorno Cadastro()
        {

            var results = Validate(_sessao);

            // Se o modelo é inválido retorno false
            if (!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors.Select(c => c.ErrorMessage).ToList() };

            
            
            db.Sessoes.Add(_sessao);
            file.ManipulacaoDeArquivos(false, db);


            return new Retorno() { Status = true, Resultado = _sessao };
        }

        // Método para buscar uma pauta
        public Retorno AcharUm(string id)
        {
            if (!db.Sessoes.Any(p => p.Id.ToString() == id)) 
                return new Retorno() { Status = false, Resultado = null };

            var SessaoUm = db.Sessoes.Find(c => c.Id.ToString() == id);
            return new Retorno() { Status = true, Resultado = SessaoUm };
        }

        // Método para retornar todas as pautas
        public Retorno AcharTodos() => new Retorno() { Status = true, Resultado = db.Sessoes.OrderBy(c => c.Id) };
        

        // Método para deletar por id
        public Retorno DeletarId(string id)
        {
           
           var umaSessao = db.Sessoes.Find(c => c.Id.ToString() == id);

            db.Sessoes.Remove(umaSessao);

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = null };

        }
        // Método para atualizar a pauta por id
        public Retorno AtualizarUm(string id, Sessao sessao)
        {
            var umaSessao = db.Sessoes.Find(c => c.Id.ToString() == id);

            if (sessao.Status != false)
                umaSessao.Status = sessao.Status;

            if (sessao.LstPautas != null)
                umaSessao.LstPautas = sessao.LstPautas;

            file.ManipulacaoDeArquivos(false, db);

            return new Retorno { Status = true, Resultado = umaSessao };
        }


    }
}
