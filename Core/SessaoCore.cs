using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;
namespace Core
{
    //essa minha classe sessão core eu tenho a regra de negocios da Sessão e ela já Herda a AbstractValidator do FrameWork: FluentValidation.
    public class SessaoCore : AbstractValidator<Sessao>
    {
      
        //Aqui eu declaro que ela tem uma sessão como atributo privado e eu consigo pegar e setar valores atráves desse atributo.
        private Sessao _sessao { get; set; }

        #region Construtores SessaoCore
        //aqui eu tenho dois construtores um com uma sessão como assinatura e as validações que uma sessão precisa ter e outro vazio que eu chamo  
        //só para ultilizar os metodos da minha sessão core.
        public SessaoCore(Sessao sessao)
        {
            _sessao = sessao;

            RuleFor(e => e.lstPautas)
                .NotEmpty()
                .WithMessage("Não pode cadastrar uma sessão sem uma lista de ID de Pautas.");
                

            RuleFor(s => s.lstEleitores)
                .NotEmpty()
                .WithMessage("A lista de Eleitores não pode ser nula.");

            RuleFor(s => s.Status)
                .NotEqual(true)
                .WithMessage("Não pode Cadastrar uma Sessão já finalizada.");
        }
        public SessaoCore() { }
        #endregion

        #region Metodos C.R.U.D SessãoCore 
        //Esse metodo CadastroSessão eu faço toda validação e Registro de Sessão.
        public Retorno CadastroSessao()
        {

            var results = Validate(_sessao);
            // Se o modelo é inválido retorno false
            if (!results.IsValid) return new Retorno { Status = false, Resultado = results.Errors.Select(m => m.ErrorMessage).ToList() };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true, null);


            if (db.sistema == null) db.sistema = new Sistema();

            if (!db.sistema.Sessao.Exists(e => e.Id.Equals(_sessao.Id)))
            {
                db.sistema.Sessao.Add(_sessao);
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _sessao };
            }
            else
                return new Retorno() { Status = false, Resultado = "Já existe uma sessão com esse ID." };
        }

        //Esse metodo StatusSessão eu Verifico que o ID que a pessoa Mandou pra mim é valido e se a sessão já foi concluida ou não.
       public Retorno StatusSessao(Guid SessaoID)
       {
           var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.Sessao.Exists(s => s.Id.Equals(SessaoID)))
            {
                var sessaoSelecionada = db.sistema.Sessao.Single(s => s.Id.Equals(SessaoID));

                return new Retorno() { Status = true, Resultado = $"A Sessão está finalizada : {sessaoSelecionada.Status} Sua identificação é {sessaoSelecionada.Id}" };
            }
            return new Retorno() { Status = false, Resultado = $"Não existe uma Sessão com o ID inserido." };

        }

        //esse metodo eu utilizo para retorna somente a guantidade de itens e a pagina que o usuario digita. 
        public Retorno Paginacao(int itens, int numeroPagina)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null) db.sistema = new Sistema();

            if (numeroPagina > 0 && itens > 0 )
                return new Retorno { Status = true, Resultado = { {db.sistema.Sessao.Skip((numeroPagina - 1) * itens).Take(itens).ToList() } } };

            return new Retorno { Status = true, Resultado = { "Número da Pagina ou a Quantidade de itens não pode ser igual a 0" } };
        }

        //Esse metodo eu busco os objetos pela data de cadastro inserida pelo usuario.
        public Retorno ProcurarPorData(string datainserida)
        {
            var db = file.ManipulacaoDeArquivos(true,null);
            if (db.sistema == null) db.sistema = new Sistema();

            var ValidarData = DateTime.Parse(datainserida).ToString("dd/MM/yyyy");
                        
            if(db.sistema.Sessao.Exists(s => s.DataCadastro.ToShortDateString().Equals(ValidarData)))
                return new Retorno { Status = true, Resultado = db.sistema.Sessao.Where(s => s.DataCadastro.ToShortDateString().Equals(ValidarData)).ToList() };

                 return new Retorno { Status = false, Resultado = "Não Existe nenhuma sessão com essa data." };
               
        }

        //Esse Metodo Procurar por Id é Similar ao StatusSessão porém ele retorna todos os Elementos da sessão e não somente o Status.
        public Retorno ProcurarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return (db.sistema.Sessao.Any(e => e.Id.ToString().Equals(id))) ? new Retorno() { Status = true, Resultado = db.sistema.Sessao.SingleOrDefault(e => e.Id.ToString().Equals(id)) } : new Retorno() { Status = false, Resultado = "Não existe uma sessão com esse ID." };
        }

        //Esse metodo eu Procuro todas as Sessões estejam elas abertas ou fechadas.
        public Retorno ProcurarTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return (db.sistema.Sessao.Any()) ? new Retorno() { Status = true, Resultado = db.sistema.Sessao } : new Retorno() { Status = false, Resultado = "Não existe nenhum elemento." };
        }

        //Esse metodo eu uso para Deletar uma Sessão pelo ID .
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

        //Esse Metodo eu Ultilizo para atualizar as informações da sessão.
        public Retorno AtualizarPorID(string id, Sessao eleitor)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.Sessao.Exists(e => e.Id.ToString().Equals(id)))
            {
                var elementoAtualizado = TrocarDadosDeSessao(eleitor, db.sistema.Sessao.SingleOrDefault(e => e.Id.ToString().Equals(id)));
                db.sistema.Sessao.Add(elementoAtualizado);
                db.sistema.Sessao.Remove(db.sistema.Sessao.FirstOrDefault(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = elementoAtualizado };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe um eleitor com esse ID, Nenhum eleitor foi atualizado." };

        }

        //esse metodo eu faço a regra de negocio para fazer a troca de atributos e etc ... 
        protected Sessao TrocarDadosDeSessao(Sessao sessao, Sessao Objeto)
        {
            if (sessao.lstEleitores.Count >= 0) sessao.lstEleitores = Objeto.lstEleitores;

            if (sessao.lstPautas.Count >= 0) sessao.lstPautas = Objeto.lstPautas;


            sessao.DataCadastro = Objeto.DataCadastro;
            sessao.Id = Objeto.Id;

            return sessao;
        }
        #endregion
    }
}

