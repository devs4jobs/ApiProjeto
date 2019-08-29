using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;
namespace Core
{
    //essa minha classe PautaCore eu tenho a regra de negocios da Pauta e ela já Herda a AbstractValidator do FrameWork: FluentValidation.
    public class PautaCore : AbstractValidator<Pauta>
    {
        //Aqui eu declaro que ela tem uma Pauta como atributo privado e eu consigo pegar e setar valores atráves desse atributo.
        private Pauta _pauta{ get; set; }

        #region Construtores PautaCore
        //aqui eu tenho dois construtores um com uma Pauta como assinatura e as validações que uma Pauta precisa ter e outro vazio que eu chamo  
        //só para ultilizar os metodos da minha PautaCore.
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
        #endregion

        #region Metodos C.R.U.D PautaCore 
        //Esse metodo CadastrarPauta eu faço toda validação e Registro de Pauta.
        public Retorno CadastrarPauta() {

            var results = Validate(_pauta);

            // Se o modelo é inválido retorno false
            if(!results.IsValid) return new Retorno { Status = false, Resultado = results.Errors.Select(m => m.ErrorMessage).ToList() };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true,null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (!db.sistema.Pautas.Exists(e => e.Descricao.Equals(_pauta.Descricao)))
            {
                db.sistema.Pautas.Add(_pauta);
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _pauta };
            }else
                return new Retorno() { Status = false, Resultado = "Já existe um eleitor com esse documento." };

            }


        //esse metodo eu utilizo para retorna somente a guantidade de itens e a pagina que o usuario digita. 
        public Retorno Paginacao(int itens, int numeroPagina)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null) db.sistema = new Sistema();



            if (numeroPagina > 0 && itens > 0)
                return new Retorno { Status = true, Resultado = db.sistema.Pautas.Skip((numeroPagina - 1) * itens).Take(itens).ToList() };
            if (itens > db.sistema.Pautas.Count())
                return new Retorno { Status = false, Resultado = "Não contém essa quantidade de regostros." };

            return new Retorno { Status = false, Resultado = { "Número da Pagina ou a Quantidade de itens não pode ser igual a 0." } };
        }

        //Esse Metodo Procurar por Id retorna a Pauta que contém o ID inserido.
        public Retorno ProcurarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            
            if (db.sistema == null) db.sistema = new Sistema();

            return db.sistema.Pautas.Any(e => e.Id.ToString().Equals(id))? new Retorno() { Status = true, Resultado = db.sistema.Pautas.SingleOrDefault(e => e.Id.ToString().Equals(id))}: new Retorno() { Status = false, Resultado = "Não existe uma Pauta com esse ID." };
        }


        //Esse metodo eu busco os objetos pela data de cadastro inserida pelo usuario.
        public Retorno ProcurarPorData(string datainserida)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null) db.sistema = new Sistema();

            var ValidarData = DateTime.Parse(datainserida).ToString("dd/MM/yyyy");

            if (db.sistema.Pautas.Exists(s => s.DataCadastro.ToShortDateString().Equals(ValidarData)))
                return new Retorno { Status = true, Resultado = db.sistema.Pautas.Where(s => s.DataCadastro.ToShortDateString().Equals(ValidarData)).ToList() };

            return new Retorno { Status = false, Resultado = "Não Existe nenhum Eleitor com essa data de cadastro." };

        }

        //Esse metodo eu Procuro todas as Pautas
        public Retorno ProcurarTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return db.sistema.Pautas.Any() ? new Retorno() { Status = true, Resultado = db.sistema.Pautas } : new Retorno() { Status = false, Resultado = "Não existe nenhum elemento." };

        }

        //Esse metodo eu uso para Deletar uma Pauta pelo ID.
        public Retorno DeletarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.Pautas.Exists(e => e.Id.ToString().Equals(id)))
            { 
                db.sistema.Pautas.Remove(db.sistema.Pautas.Single(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);

                return new Retorno() { Status = true, Resultado = "Pauta Deletada com sucesso." };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma pauta com esse ID ,nada foi deletado." };

        }

        //Esse Metodo eu Ultilizo para atualizar as informações da Pauta.
        public Retorno AtualizarPorID(string id, Pauta pauta)
        { 
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();


            if (db.sistema.Pautas.Exists(e => e.Id.ToString().Equals(id)))
            {
                var elementoAtualizado = TrocaDadosPautas(pauta, db.sistema.Pautas.SingleOrDefault(e => e.Id.ToString().Equals(id)));
                db.sistema.Pautas.Add(elementoAtualizado);
                db.sistema.Pautas.Remove(db.sistema.Pautas.FirstOrDefault(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = elementoAtualizado};
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe uma Pauta com esse ID, Nenhuma Pauta foi atualizado." };

        }

        //Nesse metodo eu faço a regra de negocio para fazer a troca de atributos e etc ...
        protected Pauta TrocaDadosPautas(Pauta inserida, Pauta Existente)
        { 

            if (inserida.Descricao == null) inserida.Descricao = Existente.Descricao;

            inserida.DataCadastro = Existente.DataCadastro;
            inserida.Id = Existente.Id;            
       
            return inserida;
        }
        #endregion
    }

}

