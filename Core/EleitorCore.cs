using Model;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Core.util;
using System.Linq;

namespace Core
{
    public class EleitorCore : AbstractValidator<Eleitor>
    {
        private Eleitor _eleitor { get; set; }
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
        }
        public EleitorCore() { }

        public Retorno CadastroEleitor() {

            var results = Validate(_eleitor);

            // Se o modelo é inválido retorno false
            if(!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors};

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true,null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (!db.sistema.Eleitores.Exists(e => e.Documento.Equals(_eleitor.Documento)))
            {
                db.sistema.Eleitores.Add(_eleitor);
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _eleitor };
            }else
                return new Retorno() { Status = false, Resultado = "Já existe um eleitor com esse documento." };

            }

        public Retorno ProcurarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Eleitores.Any(e => e.Id.ToString().Equals(id)))
            {
                return new Retorno() { Status = true, Resultado = db.sistema.Eleitores.SingleOrDefault(e => e.Id.ToString().Equals(id)) };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe um eleitor com esse ID." };

        }

        public Retorno ProcurarTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Eleitores.Any())
            {
                return new Retorno() { Status = true, Resultado = db.sistema.Eleitores };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe nenhum elemento." };

        }

        public Retorno DeletarPorID(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Eleitores.Exists(e => e.Id.ToString().Equals(id)))
            { 
                db.sistema.Eleitores.Remove(db.sistema.Eleitores.Single(e => e.Id.ToString().Equals(id)));
                file.ManipulacaoDeArquivos(false, db.sistema);

                return new Retorno() { Status = true, Resultado = "Eleitor Deletado com sucesso." };
            }
            else
                return new Retorno() { Status = false, Resultado = "Não existe um eleitor com esse ID ,nada foi deletado." };

        }

        public Retorno AtualizarPorID(string id,Eleitor eleitor)
        {
       
            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true, null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            if (db.sistema.Eleitores.Exists(e => e.Id.ToString().Equals(id)))
            {
                db.sistema.Eleitores.Add(TrocarDadosDeEleitores(eleitor, db.sistema.Eleitores.SingleOrDefault(e => e.Id.ToString().Equals(id))));
                file.ManipulacaoDeArquivos(false, db.sistema);
                return new Retorno() { Status = true, Resultado = _eleitor };
            }
            else
                return new Retorno() { Status = false, Resultado = "Já existe um eleitor com esse documento." };

        }

        public Eleitor TrocarDadosDeEleitores(Eleitor FDP1, Eleitor Objeto)
        { 
            if (FDP1.Nome == null)
                FDP1.Nome = Objeto.Nome;
            if (FDP1.Idade == 0)
                FDP1.Idade = Objeto.Idade;
            if (FDP1.Sexo == null)
                FDP1.Sexo = Objeto.Sexo;
            if (FDP1.Documento == null)
                FDP1.Documento = Objeto.Documento;
       
            return FDP1;
        }

    }
    
}

