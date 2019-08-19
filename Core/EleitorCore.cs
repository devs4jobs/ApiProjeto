using Model;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Core.util;

namespace Core
{
    public class EleitorCore : AbstractValidator<Eleitor>
    {
        private Eleitor _eleitor;
        public EleitorCore()
        {

        }
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

        public Retorno CadastroEleitor() {

            var results = Validate(_eleitor);

              
            // Se o modelo é inválido retorno false
            if(!results.IsValid)
                return new Retorno { Status = false, Resultado = results.Errors};

            // Caso o modelo seja válido, escreve no arquivo db
            var db = file.ManipulacaoDeArquivos(true,null);

            if (db.sistema == null)
                db.sistema = new Sistema();

            var eleitores = db.sistema.Eleitores;

            
            if (eleitores.Exists(c => c.Documento == _eleitor.Documento))
                return new Retorno() { Status = false, Resultado = null };
            

            db.sistema.Eleitores.Add(_eleitor);
            file.ManipulacaoDeArquivos(false, db.sistema);

          
            return new Retorno() { Status = true, Resultado = _eleitor};
        }


        public Retorno AcharUm(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            if (!db.sistema.Eleitores.Exists(e => e.Id.ToString() == id))
                return new Retorno() { Status = false, Resultado = null };



            var UmEleitor = db.sistema.Eleitores.Find(c => c.Id.ToString() == id);
            return new Retorno() { Status = true, Resultado = UmEleitor };
        }

        public Retorno AcharTodos()
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();


          //  var eleitores = db.sistema.Eleitores;
            return new Retorno() { Status = true, Resultado = db.sistema.Eleitores };
        }


        public Retorno DeletarId(string id)
        {
            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

             var umEleitor = db.sistema.Eleitores.Find(c => c.Id.ToString() == id);

            db.sistema.Eleitores.Remove(umEleitor);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno { Status = true, Resultado = null };

        }


        public Retorno AtualizarUm(string id, Eleitor eleitor )
        {

            var db = file.ManipulacaoDeArquivos(true, null);
            if (db.sistema == null)
                db.sistema = new Sistema();

            var umEleitor = db.sistema.Eleitores.Find(c => c.Id.ToString() == id);
            db.sistema.Eleitores.Remove(umEleitor);

         
            if (eleitor.Nome != null)
                umEleitor.Nome = eleitor.Nome;

            if (eleitor.Id != null)
            umEleitor.Id = eleitor.Id;

            if (eleitor.Documento != null)
            umEleitor.Documento = eleitor.Documento;

            if (eleitor.Sexo != null)
            umEleitor.Sexo = eleitor.Sexo;

            if (eleitor.Idade != 0)
              umEleitor.Idade = eleitor.Idade;


            db.sistema.Eleitores.Add(umEleitor);

            file.ManipulacaoDeArquivos(false, db.sistema);

            return new Retorno { Status = true, Resultado = umEleitor };


        }
    }
}
