using Model;
using FluentValidation;
using Core.util;
using System.Linq;
using System;
namespace Core
{
    //essa minha classe EleitorCore eu tenho a regra de negocios da Sessão e ela já Herda a AbstractValidator do FrameWork: FluentValidation.
    public class EleitorCore : AbstractValidator<Eleitor>
    {
        //Aqui eu declaro que minha classe tem um Eleitor como atributo privado e eu consigo pegar e setar valores atráves desse atributo.
        private Eleitor _eleitor { get; set; }

        #region Construtores EleitorCore
        //aqui eu tenho dois construtores um com um Eleitor como assinatura e as validações que um Eleitor precisa ter e outro vazio que eu chamo  
        //só para ultilizar os metodos da minha EleitorCore.
        public EleitorCore( Eleitor eleitor)
        {
            _eleitor = eleitor;

            RuleFor(e => e.Documento)
                .Length(11,11)
                .NotNull()
                .WithMessage("Cpf inválido"); 

            RuleFor(e => e.Nome)
                .MinimumLength(3)
                .NotNull()
                .WithMessage("O nome deve ser preenchido e deve ter o mínimo de 3 caracteres");

            RuleFor(e => e.Sexo.Trim().ToUpper())
                .Must(e => e.Equals("M") || e.Equals("F"))
                .WithMessage("Digite um sexo válido : M para Masculino ou F para Feminino.");

            RuleFor(e => e.Idade)
                .Must(e => e.Equals(e >= 18))
                .WithMessage("O eleitor tenque ser mais de Idade para poder votar.");
        }
        public EleitorCore() { }
        #endregion

        #region Metodos C.R.U.D EleitorCore 
        //Esse metodo CadastroEleitor eu faço toda validação e Registro de Eleitor.
        public Retorno CadastroEleitor() {

            var results = Validate(_eleitor);
            // Se o modelo é inválido retorno false
            if(!results.IsValid) return new Retorno { Status = false, Resultado = results.Errors.Select(m => m.ErrorMessage).ToList() };

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true,null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (!db.sistema.Eleitores.Exists(e => e.Documento.Equals(_eleitor.Documento)))
            {
                db.sistema.Eleitores.Add(_eleitor);
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _eleitor };
            }else
                return new Retorno() { Status = false, Resultado = "Já existe um eleitor com esse documento." };
            }

        //Esse Metodo Procurar por Id  retorna todos os Elementos do Eleitor.
        public Retorno ProcurarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return (db.sistema.Eleitores.Any(e => e.Id.ToString().Equals(id))) ? new Retorno() { Status = true, Resultado = db.sistema.Eleitores.SingleOrDefault(e => e.Id.ToString().Equals(id))} : new Retorno() { Status = false, Resultado = "Não existe um eleitor com esse ID." };
        }


        //esse metodo eu utilizo para retorna somente a guantidade de itens e a pagina que o usuario digita. 
        public Retorno Paginacao(int itens, int numeroPagina)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null) db.sistema = new Sistema();

            if (numeroPagina > 0 && itens > 0)
                return new Retorno { Status = true, Resultado = db.sistema.Eleitores.Skip((numeroPagina - 1) * itens).Take(itens).ToList() };

            return new Retorno { Status = true, Resultado = { "Número da Pagina ou a Quantidade de itens não pode ser igual a 0" } };
        }

        //Esse metodo eu busco os objetos pela data de cadastro inserida pelo usuario.
        public Retorno ProcurarPorData(string datainserida)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null) db.sistema = new Sistema();

            var ValidarData = DateTime.Parse(datainserida).ToString("dd/MM/yyyy");

            if (db.sistema.Eleitores.Exists(s => s.DataCadastro.ToShortDateString().Equals(ValidarData)))
                return new Retorno { Status = true, Resultado = db.sistema.Eleitores.Where(s => s.DataCadastro.ToShortDateString().Equals(ValidarData)).ToList() };

            return new Retorno { Status = false, Resultado = "Não Existe nenhum Eleitor com essa data de cadastro." };

        }

        //Esse metodo eu Procuro todos os Eleitores Cadastrados.
        public Retorno ProcurarTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            return (db.sistema.Eleitores.Any())? new Retorno() { Status = true, Resultado = db.sistema.Eleitores }: new Retorno() { Status = false, Resultado = "Não existe nenhum elemento." };
        }

        //Esse metodo eu uso para Deletar um Eleitor pelo ID 
        public Retorno DeletarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.Eleitores.Exists(e => e.Id.ToString().Equals(id)))
            { 
                db.sistema.Eleitores.Remove(db.sistema.Eleitores.Single(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);

                return new Retorno() { Status = true, Resultado = "Eleitor Deletado com sucesso." };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe um eleitor com esse ID ,nada foi deletado." };
        }

        //Esse Metodo eu Ultilizo para atualizar as informações do Eleitor.
        public Retorno AtualizarPorID(string id,Eleitor eleitor)
        { 
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null) db.sistema = new Sistema();

            if (db.sistema.Eleitores.Exists(e => e.Id.ToString().Equals(id)))
            {
                var elementoAtualizado = TrocarDadosDeEleitores(eleitor, db.sistema.Eleitores.SingleOrDefault(e => e.Id.ToString().Equals(id)));
                db.sistema.Eleitores.Add(elementoAtualizado);
                db.sistema.Eleitores.Remove(db.sistema.Eleitores.FirstOrDefault(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = elementoAtualizado};
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe um eleitor com esse ID, Nenhum eleitor foi atualizado." };

        }

        //esse metodo eu faço a regra de negocio para fazer a troca de atributos e etc ... 
        protected Eleitor TrocarDadosDeEleitores(Eleitor FDP1, Eleitor Objeto)
        { 
            if (FDP1.Nome == null) FDP1.Nome = Objeto.Nome;

            if (FDP1.Idade == 0) FDP1.Idade = Objeto.Idade;

            if (FDP1.Sexo == null) FDP1.Sexo = Objeto.Sexo;

            if (FDP1.Documento == null) FDP1.Documento = Objeto.Documento;

            FDP1.DataCadastro = Objeto.DataCadastro;
            FDP1.Id = Objeto.Id;            
       
            return FDP1;
        }
        #endregion
    }
}

